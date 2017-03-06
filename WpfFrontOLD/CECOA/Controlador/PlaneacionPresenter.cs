using System;
using WpfFront.Vista;
using WpfFront.Modelo;
using WpfFront.Model;
using Assergs.Windows;
using WMComposite.Events;
using Microsoft.Practices.Unity;
using WpfFront.Common;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Globalization;
using Microsoft.Office.Core;
using System.IO;
using System.Reflection;


namespace WpfFront.Controlador
{

    public interface IPlaneacionPresenter
    {
        IPlaneacionView View { get; set; }
        ToolWindow Window { get; set; }
    }


    public class PlaneacionPresenter : IPlaneacionPresenter
    {
        public IPlaneacionView View { get; set; }
        private readonly IUnityContainer container;
        private wmsEntities db;
        public ToolWindow Window { get; set; }
        public Object PresenterParent { get; set; }

        public PlaneacionPresenter(IUnityContainer container, IPlaneacionView view)
        {
            View = view;
            this.container = container;
            db = new wmsEntities();
            View.Model = this.container.Resolve<PlaneacionModel>();

            View.Model.RecordPlaneacion = new Planeacion();
            View.Model.RecordBusquedaPlaneacion = new Planeacion();

            #region Definicion Metodos
            view.BuscarPlaneacion += this.OnBuscarPlaneacion;
            View.CargarPlaneacion += this.OnCargarPlaneacion;
            view.SavePlaneacion += this.OnSavePlaneacion;
            view.DeletePlaneacion += this.OnDeletePlaneacion;
            view.NewPlaneacion += this.OnNewPlaneacion;
            view.NuevoRegistro += this.onNuevoRegistro;
            View.ActualizarListaPlaneacion += this.onActualizarListaPlaneacion;
            view.ValidarRangoHora += this.OnValidarRangoHora;
            View.ExportarPlaneacionExcel += this.OnExportarPLaneacionExcel;
            view.CerrarTab += this.onCerrarTab;


            #endregion

            #region Datos

            //Obtengo la conexion

            this.ActualizarListaPlaneacion();
            view.Model.ListaBanda = db.Tipo.Where(f => f.Agrupacion.Codigo == "BANDA").ToList();

            //Inicio las variables
            CleanToCreate();


            #endregion
        }

        #region Declaracion Metodos

        public void OnBuscarPlaneacion(object sender, EventArgs e)
        {

            View.Model.ListadoPlaneacion = db.Planeacion.ToList();

            View.Model.RecordBusquedaPlaneacion.Tercero = View.SrchFiltroCompania.Terceros;

            if (!String.IsNullOrEmpty(View.Model.RecordBusquedaPlaneacion.NVueloEntrada))
            {
                View.Model.ListadoPlaneacion = View.Model.ListadoPlaneacion.Where(f => f.NVueloEntrada.StartsWith(View.Model.RecordBusquedaPlaneacion.NVueloEntrada)).ToList();
            }


            if (!String.IsNullOrEmpty(View.Model.RecordBusquedaPlaneacion.NVueloSalida))
            {
                View.Model.ListadoPlaneacion = View.Model.ListadoPlaneacion.Where(f => f.NVueloSalida.StartsWith(View.Model.RecordBusquedaPlaneacion.NVueloSalida)).ToList();
            }


            if (View.Model.RecordBusquedaPlaneacion.Fecha != null)
            {
                View.Model.ListadoPlaneacion = View.Model.ListadoPlaneacion.Where(f => f.Fecha.Value == View.Model.RecordBusquedaPlaneacion.Fecha.Value).ToList();
            }

            if (View.Model.RecordBusquedaPlaneacion.Tercero != null)
            {
                if (View.Model.RecordBusquedaPlaneacion.Tercero.RowID != 0)
                {
                    View.Model.ListadoPlaneacion = View.Model.ListadoPlaneacion.Where(f => f.CompaniaID == View.Model.RecordBusquedaPlaneacion.Tercero.RowID).ToList();
                }
            }

            View.Model.ListadoPlaneacion = View.Model.ListadoPlaneacion.OrderByDescending(f => f.RowID).ToList();

        }

        private void OnCargarPlaneacion(object sender, DataEventArgs<Planeacion> e)
        {
            //Limpio los campos
            this.CleanToCreate();
            //Cargo la pantalla con el documento creado
            CargarPlaneacion(e.Value);
        }

        public void CargarPlaneacion(Planeacion RecordPlaneacion1)
        {

            if (RecordPlaneacion1 != null)
            {
                this.controlarPanelNuevoRegistro(true); //Mostrar el panel nuevo registro
                View.Model.RecordPlaneacion = RecordPlaneacion1;  // cargar datos de Planeacion
                //Cargo combos y controles

                if (RecordPlaneacion1.Tercero != null)
                {
                    View.SearchCompania.Terceros = RecordPlaneacion1.Tercero;
                    View.SearchCompania.txtDescripcion.Text = RecordPlaneacion1.Tercero.Nombre + " " + RecordPlaneacion1.Tercero.Apellidos;
                    View.SearchCompania.txtData.Text = RecordPlaneacion1.Tercero.Nombre + " " + RecordPlaneacion1.Tercero.Apellidos;
                    View.SearchCompania.Terceros = RecordPlaneacion1.Tercero;
                }

                if (RecordPlaneacion1.OrigenID != 0 && RecordPlaneacion1.OrigenID != null)
                {
                    View.Origen.cargarValorEspecifico(RecordPlaneacion1.Aeropuerto1.SiglaIATA+ "/"+ RecordPlaneacion1.Aeropuerto1.Ciudad);
                    View.Origen.Aeropuertos.RowID = RecordPlaneacion1.OrigenID.Value;
                }
                if (RecordPlaneacion1.DestinoID != 0 && RecordPlaneacion1.DestinoID != null)
                {
                    View.Destino.cargarValorEspecifico(RecordPlaneacion1.Aeropuerto.SiglaIATA + "/" + RecordPlaneacion1.Aeropuerto.Ciudad);
                    View.Destino.Aeropuertos.RowID = RecordPlaneacion1.DestinoID.Value;
                }
                if (RecordPlaneacion1.Tipo != null)
                {
                    View.cbxBanda.SelectedValue = RecordPlaneacion1.Tipo.RowID;
                }

                if (!string.IsNullOrEmpty(RecordPlaneacion1.NVueloEntrada))
                {
                    View.CheckLlegada.IsChecked = true;
                    View.DatosLlegada.IsEnabled = true;
                }
                if (!string.IsNullOrEmpty(RecordPlaneacion1.NVueloSalida))
                {
                    View.CheckSalida.IsChecked = true;
                    View.DatosSalida.IsEnabled = true;
                }


            }
        }

        public void OnNewPlaneacion(Object sender, EventArgs e)
        {
            CleanToCreate();
        }

        public void OnSavePlaneacion(Object sender, EventArgs e)
        {
            //Cuando es un nuevo registro
            if (View.Model.RecordPlaneacion.RowID == 0)
            {
                //Cargo valores de los combos
                //View.Model.RecordPlaneacion.TipoAeronave = View.Model.RecordPlaneacion.TipoAeronave.ToUpper();
                Planeacion nuevo = new Planeacion();
                nuevo = View.Model.RecordPlaneacion;

                if (View.SearchCompania.Terceros!= null)
                {
                    if (View.SearchCompania.Terceros.RowID != 0)
                    {
                        nuevo.CompaniaID = View.SearchCompania.Terceros.RowID;
                    }
                }
                if (View.Origen.Aeropuertos.RowID != 0)
                {
                    int idOrigen = View.Origen.Aeropuertos.RowID;
                    nuevo.OrigenID = idOrigen;
                }
                if (View.Destino.Aeropuertos.RowID != 0)
                {
                    int idDestino = View.Destino.Aeropuertos.RowID;
                    nuevo.DestinoID= idDestino;
                }
                if (View.cbxBanda.SelectedItem != null)
                {
                    nuevo.BandaID = ((Tipo)View.cbxBanda.SelectedItem).RowID;
                }
                //Cargo datos de Creacion
                nuevo.FechaCreacion = DateTime.Now;
                nuevo.UsuarioCreacion = App.curUser.NombreUsuario;
                db.Planeacion.Add(nuevo);
                db.SaveChanges();
                Util.ShowMessage("Se almaceno correctamente");
            }
            //Cuando es para actualiar
            else
            {
                //Cargo valores de los combos
                //View.Model.RecordPlaneacion.Tercero = View.SearchCompania.Terceros;

                if (View.SearchCompania.Terceros != null)
                {
                    if (View.SearchCompania.Terceros.RowID != 0)
                    {
                        int idTercerp = 0;
                        idTercerp = (View.SearchCompania.Terceros as Tercero).RowID;
                        View.Model.RecordPlaneacion.Tercero = db.Tercero.FirstOrDefault( f=> f.RowID == idTercerp);
                    }
                }

                if (View.Origen.Aeropuertos.RowID != 0)
                {
                    int idOrigen = 0;
                     idOrigen = View.Origen.Aeropuertos.RowID;
                     View.Model.RecordPlaneacion.OrigenID = idOrigen;
                }
                if (View.Destino.Aeropuertos.RowID != 0)
                {
                    int idDestino = 0;
                    idDestino = View.Destino.Aeropuertos.RowID;
                    View.Model.RecordPlaneacion.DestinoID = idDestino;
                }
                if (View.cbxBanda.SelectedItem != null)
                {
                    View.Model.RecordPlaneacion.Tipo = (Tipo)View.cbxBanda.SelectedItem;
                }
                //Cargo datos de Modificacion
                View.Model.RecordPlaneacion.FechaModificacion = DateTime.Now;
                View.Model.RecordPlaneacion.UsuarioModificacion = App.curUser.NombreUsuario;

                db.SaveChanges();
                Util.ShowMessage("Se actualizo correctamente");
            }

            this.ActualizarListaPlaneacion();
            this.controlarPanelNuevoRegistro(false);
            CleanToCreate();
        }

        public void OnDeletePlaneacion(Object sender, EventArgs e)
        {

            
            if (View.Model.RecordPlaneacion.RowID == 0)
            {
                return;
            }
            bool? respuesta = UtilWindow.ConfirmOK("Confirma Eliminar este Registro?");
            if (respuesta == true)
            {
                db.Planeacion.Remove(View.Model.RecordPlaneacion);
                db.SaveChanges();
            }
            this.ActualizarListaPlaneacion();
            this.controlarPanelNuevoRegistro(false);
            CleanToCreate();
        }


        private void OnExportarPLaneacionExcel(object sender, EventArgs e)
        {
            string bandera = "1";
            Microsoft.Office.Interop.Excel.Application excel = null;
            bandera = "2";
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            bandera = "3";
            object missing = Type.Missing;
            bandera = "4";
            Microsoft.Office.Interop.Excel.Worksheet ws = null;
            bandera = "5";
            Microsoft.Office.Interop.Excel.Range rng = null;
            bandera = "6";
            Microsoft.Office.Interop.Excel.Range encabezado = null;

            try
            {
                bandera = "7";
                excel = new Microsoft.Office.Interop.Excel.Application();
                bandera = "8";
                wb = excel.Workbooks.Add();
                bandera = "9";
                ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet;

                ///Rango para la imagen
                bandera = "10";
                Microsoft.Office.Interop.Excel.Range rangoImagen;
                bandera = "11";
                rangoImagen = ws.get_Range("A1", "C4");
                //Insertar Imagen
                string Imagen = "";
                try
                {
                    Imagen = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    Imagen = Imagen + @"\LogoPixeladoConLetras.PNG";
                    ws.Shapes.AddPicture(Imagen, MsoTriState.msoTriStateMixed, MsoTriState.msoTriStateMixed, 6f, 3f, 65, 50);
                }
                catch (Exception exp1)
                {
                    Util.ShowMessage("No se encontro imagen en: " + Imagen);
                }
                bandera = "12";
                rangoImagen.Cells.MergeCells = true;
                bandera = "13";
                rangoImagen.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;


                //Rango para el titulo
                ws.get_Range("D1", "J3").Cells.MergeCells = true;
                ws.get_Range("D1", "J3").Font.Size = 12;
                ws.get_Range("D1", "J3").Font.Bold = true;
                ws.get_Range("D1", "J3").VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ws.get_Range("D1", "J3").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ws.get_Range("D1", "J3").Value = "FORMATO DE PROGRAMACION DE VUELOS - CECOA";


                //Rango de la fecha 
                ws.get_Range("D4", "J4").Merge(true);
                ws.get_Range("D4", "J4").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //Si selecciono una fecha en el formulario es la que muestra en el informe, de lo contrario muestro la de hoy
                if (!String.IsNullOrEmpty(View.FechaAExportar.Text))
                {
                    ws.get_Range("D4", "J4").Value = Convert.ToDateTime(View.FechaAExportar.Text).ToString("D", CultureInfo.CreateSpecificCulture("es-ES"));
                }
                else
                {
                    ws.get_Range("D4", "J4").Value = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("es-ES"));
                }
                ws.get_Range("D4", "J4").Font.Bold = true;
                ws.get_Range("D1", "J4").Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;


                //Informacion que nunca Cambia cabecera
                ws.get_Range("K1").Value = "CODIGO";
                ws.get_Range("L1").Value = "FO-145";
                ws.get_Range("L1").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ws.get_Range("K2").Value = "REVISION";
                ws.get_Range("L2").Value = "0";
                ws.get_Range("L2").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ws.get_Range("K3").Value = "FECHA";
                ws.get_Range("L3").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


                //Si selecciono una fecha en el formulario es la que muestra en el informe, de lo contrario muestro la de hoy
                ws.get_Range("L3").Value = "29-09-2015";
                ws.get_Range("L3").NumberFormat = "dd-MM-yyyy";
                ws.get_Range("K4").Value = "PAGINA";
                ws.get_Range("L4").Value = "1 de 1";
                ws.get_Range("L4").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ws.get_Range("K1", "L4").Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

                //Rango para el encabezado
                encabezado = ws.get_Range("A5", "M5");
                encabezado.Characters.Font.Bold = true;
                encabezado.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                encabezado.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

                ws.Range["A5"].Offset[0, 0].Value = "No";
                ws.Range["A5"].Offset[0, 1].Value = "ETA";
                ws.Range["A5"].Offset[0, 2].Value = "CIA";
                ws.Range["A5"].Offset[0, 3].Value = "VUELOS";
                ws.Range["A5"].Offset[0, 4].Value = "ORIGEN";
                ws.Range["A5"].Offset[0, 5].Value = "STAND";
                ws.Range["A5"].Offset[0, 6].Value = "T. AVO";
                ws.Range["A5"].Offset[0, 7].Value = "EDT";
                ws.Range["A5"].Offset[0, 8].Value = "VUELOS";
                ws.Range["A5"].Offset[0, 9].Value = "DESTINO";
                ws.Range["A5"].Offset[0, 10].Value = "SALA";
                ws.Range["A5"].Offset[0, 11].Value = "CINTA";
                ws.Range["A5"].Offset[0, 12].Value = "FECHA";



                int i = 6;
                IList<Planeacion> listaPlaneacionFiltrada = obtenerListaAExportar();
                foreach (Planeacion aux in listaPlaneacionFiltrada)
                {
                    string[] datos = new String[13];
                    datos[0] = aux.RowID.ToString();
                    datos[6] = aux.TipoAeronave;
                    if (aux.Fecha != null)
                    {
                     datos[12] = aux.Fecha.Value.ToString("dd/MM/yyyy");
                    }
                    //Para agregar diminutivo a la compaañia
                    if (aux.Tercero != null)
                    {
                        switch (aux.Tercero.Identificacion)
                        {
                            case 890100577:
                                datos[2] = "AVA";
                                break;
                            case 890704196:
                                datos[2] = "LAN";
                                break;
                            case 444447818:
                                datos[2] = "LAN";
                                break;
                            case 900313349:
                                datos[2] = "VVC";
                                break;
                            case 899999143:
                                datos[2] = "NSE";
                                break;
                            case 900340795:
                                datos[2] = "INC";
                                break;
                            case 900088915:
                                datos[2] = "EFY";
                                break;
                            case 800095254:
                                datos[2] = "AAL";
                                break;
                            case 800019344:
                                datos[2] = "ADA";
                                break;
                            case 800185781:
                                datos[2] = "CMP";
                                break;
                            default:
                                datos[2] = aux.Tercero.Nombre +" " + aux.Tercero.Apellidos;
                                break;

                        }
                    }

                    //Si estan guardados datos de entrada
                    if ((!string.IsNullOrEmpty(aux.NVueloEntrada)))
                    {
                        datos[1] = !String.IsNullOrEmpty(aux.HoraEntrada)?aux.HoraEntrada:"";
                        datos[3] = !String.IsNullOrEmpty(aux.NVueloEntrada)?aux.NVueloEntrada:"";
                        if (aux.Aeropuerto1 != null)
                        {
                            datos[4] = aux.Aeropuerto1.SiglaIATA;
                        }
                        else
                        {
                            datos[4] = "";
                        }
                        datos[5] = aux.Stand;
                        if (aux.Tipo != null)
                        {
                            datos[11] = aux.Tipo.Codigo;
                        }
                        else
                        {
                            datos[11] = "";
                        }
                    }
                    else
                    {
                        datos[1] = "";
                        datos[3] = "";
                        datos[4] = "";
                        datos[5] = "";
                        datos[11] = "";
                    }
                    //Si estan guardados datos de salida
                    if ((!string.IsNullOrEmpty(aux.NVueloSalida)))
                    {
                        datos[7] = aux.HoraSalida;
                        datos[8] = aux.NVueloSalida.ToString();
                        if (aux.Aeropuerto != null)
                        {
                            datos[9] = aux.Aeropuerto.SiglaIATA;
                        }
                        else
                        {
                            datos[9] = "";
                        }
                        datos[10] = aux.Sala.ToString();
                    }
                    else
                    {
                        datos[7] = "";
                        datos[8] = "";
                        datos[9] = "";
                        datos[10] = "";
                    }

                    ws.Range["A" + (i)].Offset[0].Resize[1, 13].Value = datos;
                    ws.Range["A" + (i)].Offset[0].Resize[1, 13].Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    i++;
                }

                ws.Columns.AutoFit();
                excel.Visible = true;
                wb.Activate();
            }
            catch (Exception ex)
            { Util.ShowMessage("Problema al exportar la información a Excel: "+bandera+"   " + ex.ToString()); }


        }

        public IList<Planeacion> obtenerListaAExportar()
        {
            //Hago los filtros para que solamente ecxporte en el excel los terceros comerciales
            IList<Planeacion> lista = db.Planeacion.Where(f => f.Tercero.Identificacion == 890100577 ||
                f.Tercero.Identificacion == 890704196 || f.Tercero.Identificacion == 444447818 || f.Tercero.Identificacion == 900313349 ||
                f.Tercero.Identificacion == 899999143 || f.Tercero.Identificacion == 900340795 || f.Tercero.Identificacion == 900088915 ||
                f.Tercero.Identificacion == 800095254 || f.Tercero.Identificacion == 800019344 || f.Tercero.Identificacion == 800185781).ToList();

            if (View.FechaAExportar.SelectedDate != null)
            {
                lista = lista.Where( f=> f.Fecha.Value == View.FechaAExportar.SelectedDate.Value).ToList();
            }
            return lista;
        }

        private void onCerrarTab(object sender, EventArgs e)
        {
            this.controlarPanelNuevoRegistro(false);
            this.CleanToCreate();
        }

        public void onNuevoRegistro(Object sender, EventArgs e)
        {
            controlarPanelNuevoRegistro(true);
            //Limpio los campos 
            this.CleanToCreate();

        }


        public void onActualizarListaPlaneacion(Object sender, EventArgs e)
        {
            this.ActualizarListaPlaneacion();
            this.CleanToRefresh();
        }



        /**
         * 
         * Actualiza la lista de pasajeros 
         * */
        public void ActualizarListaPlaneacion()
        {
            View.Model.ListadoPlaneacion = db.Planeacion.OrderByDescending(f => f.RowID).Take(200).ToList();

        }
        /**
         * Oculta o muestra el panel de nuevo registro
         * true= mostrar
         * false= ocultar
         * */
        public void controlarPanelNuevoRegistro(bool status)
        {
            if (status)
            {
                View.PanelNuevoRegistro.Visibility = Visibility.Visible;
            }
            else
            {
                View.PanelNuevoRegistro.Visibility = Visibility.Collapsed;
            }
        }

        public void CleanToCreate()
        {
            View.Model.RecordPlaneacion = new Planeacion();
            View.SearchCompania.Terceros = new Tercero();
            View.Destino.Aeropuertos = new Aeropuerto();
            View.Origen.Aeropuertos = new Aeropuerto();
            View.CheckLlegada.IsChecked = false;
            View.CheckSalida.IsChecked = false;
            View.DatosLlegada.IsEnabled = false;
            View.DatosSalida.IsEnabled = false;

            View.SearchCompania.txtData.Text = "";
            View.SearchCompania.txtDescripcion.Text = "";
            View.Origen.txtData.Text = "";
            View.Origen.txtDescripcion.Text = "";
            View.Destino.txtData.Text = "";
            View.Destino.txtDescripcion.Text = "";
            View.cbxBanda.SelectedIndex = -1;

        }

        public void CleanToRefresh()
        {
            View.TXT_FiltroNVuelo.Text = 0.ToString();
            View.TXT_FiltroNVueloSalida.Text = 0.ToString();
            View.FiltroFecha_Operacion.Text = "";
            View.SrchFiltroCompania.Terceros = new Tercero();
            View.SrchFiltroCompania.txtData.Text = "";
            View.SrchFiltroCompania.txtDescripcion.Text = "";
            View.Model.RecordBusquedaPlaneacion = new Planeacion();
        }

        /// <summary>
        /// Valido que la hora ingresada este en el rango de 00 a 24 y los minutos de 00 a 60
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnValidarRangoHora(object sender, EventArgs e)
        {
            if (View.horaAValidar != "__:__" && !(View.horaAValidar.Contains("_")))
            {
                string[] arrayHora = View.horaAValidar.Split(':');

                if ((Int32.Parse(arrayHora[0]) < 0 || Int32.Parse(arrayHora[0]) >= 24) || (Int32.Parse(arrayHora[1]) < 0 || Int32.Parse(arrayHora[1]) >= 60))
                {
                    Util.ShowError("Hora no valida");
                }
            }

        }


        #endregion
    }
}