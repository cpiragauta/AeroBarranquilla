using System;
using System.Windows.Controls;
using Core.WPF;
using WpfFront.Model;
using System.Windows;
using WpfFront.Common;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using WpfFront.Modelo;


namespace WpfFront.Vista
{
    public partial class DatosCarnetsView : UserControlBase, IDatosCarnetsView
    {
        //  private readonly WMSServiceClient service = new WMSServiceClient();
        wmsEntities db = new wmsEntities();
        public DatosCarnetsView()
        {
            InitializeComponent();
        }

        public DatosCarnetsModel Model
        {
            get
            { return this.DataContext as DatosCarnetsModel; }
            set
            { this.DataContext = value; }

        }
        public Object PresenterParent { get; set; }
        #region Eventos

        private void DatosCarnets_Loaded(object sender, RoutedEventArgs e)
        {
            //Validacion para que a tesoreria solo se le muestre lo que le corresponde
            //Le asigno permisos al Boton de Anular Facturas
            Rol rol = App.curUser.Rol;
            
                if (rol.Nombre == "Administrador" || rol.Nombre == "Tesoreria")
                {
                    BotonesPago.Visibility = Visibility.Visible;
                }
                else
                {
                    BotonesPago.Visibility = Visibility.Collapsed;
                }
            

            if (Model.RecordEncabezado.RowID != 0)//Si el rowid es diferente de 0 es porque esta cargando un registro de la lista de solicitudes
            {
                TerceroSolicita.Terceros.RowID = Model.RecordEncabezado.TerceroSolicitaID.Value;
                #region Solicitud
                CargarListasSolicitudes();
                #endregion

                #region Pago

               // Model.Pagos = service.SelectPago(new Pago { Encabezado = Model.RecordEncabezado });
                Model.Pagos = db.Pago.Where(f => f.Encabezado == Model.RecordEncabezado).ToList();

                //Si tiene registrado un pago general
                if (Model.Pagos.Where(f => f.Tipo.Codigo == "GENERAL").Count() > 0)
                  
                {
                    panelBotonesSolicitud.IsEnabled = false;
                    panelBotonesPago.IsEnabled = false;
                }

                //  Model.SolicitudesParaPago = service.GetSolicitud(new Solicitud { Encabezado = Model.RecordEncabezado, Estado = new Status { Name = "Confirmada", StatusType = new StatusType { Name = "SolicitudesCarnets" } } });
                Model.SolicitudesParaPago = db.Solicitud.Where(f=>f.Encabezado == Model.RecordEncabezado && f.Estado.Nombre == "Confirmada" &&  f.Estado.Tipo.Codigo== "SolicitudesCarnets").ToList();

                if (Model.SolicitudesParaPago.Count > 0 || Model.Pagos.Count > 0)
                {
                    TabPagos.IsEnabled = true;
                }
                else
                {
                    TabPagos.IsEnabled = false;
                }


                #endregion

                #region Entrega

               // Model.Entregas = service.GetEntrega(new Entrega { Encabezado = Model.RecordEncabezado }).OrderBy(f => f.RowID).ToList();
                Model.Entregas = db.Entrega.Where(f=>f.Encabezado ==  Model.RecordEncabezado).OrderBy(f=>f.RowID).ToList();

                //Si tiene registrado un pago general
                if (Model.Entregas.Where(f => f.Tipo.Codigo == "GENERAL").Count() > 0)
                {
                    panelBotonesSolicitud.IsEnabled = false;
                    panelBotonesPago.IsEnabled = false;
                    panelBotonesEntrega.IsEnabled = false;
                }

                //int CantPagosParaEntrega = service.SelectPago(new Pago { Encabezado = Model.RecordEncabezado, Estado = new Status { Name = "Confirmada", StatusType = new StatusType { Name = "PagosCarnets" } } }).Count();
                int CantPagosParaEntrega = db.Pago.Select(f=>f.Encabezado == Model.RecordEncabezado && f.Estado.Nombre == "Confirmada"&& f.Estado.Tipo.Nombre== "PagosCarnets").Count();

                if (CantPagosParaEntrega > 0 || Model.Entregas.Count > 0)
                {
                    TabEntregas.IsEnabled = true;
                }
                else
                {
                    TabEntregas.IsEnabled = false;
                }


                #endregion
            }
            else//Si el rowid es 0 es porque es un encabezado nuevo
            {
                //Si selecciono un tercero no recargo la pestaña
                if (Model.RecordEncabezado.TerceroSolicitaID != null)
                {
                    return;
                }
              //  Model.RecordEncabezado.Estado = service.GetStatus(new Status { Name = "ParaSolicitud" }).First();
                Model.RecordEncabezado.Estado = db.Estado.FirstOrDefault(f=>f.Nombre== "ParaSolicitud");
            }

            Model.RecordSolicitud = new Solicitud();
            Model.RecordPago = new Pago();
            Model.RecordEntrega = new Entrega();

            #region Solicitud
          //  Model.ListTipoSolicitud = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOSOLICITUD" } }).OrderBy(O => O.NumOrder).ToList();
            Model.ListTipoSolicitud = db.Tipo.Where(f=>f.Agrupacion.Codigo == "TIPOSOLICITUD").OrderBy(f=>f.Orden).ToList();
            if (Model.ListTipoSolicitud.Count > 0)
            {
                //Selecciona el tipo de solicitud NORMAL
                TipoSolicitud.SelectedIndex = 0;
            }
            // Model.ListTipoCarnet = service.GetMMaster(new  { MetaType = new MType { Code = "TIPOCARNET" } });
            Model.ListTipoCarnet = db.Tipo.Where(f=>f.Agrupacion.Codigo== "TIPOCARNET").ToList();
            //Model.ListTipoEmpleado = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOPERSONA" } });
            Model.ListTipoEmpleado = db.Tipo.Where(f=>f.Agrupacion.Codigo== "TIPOPERSONA").ToList();
            // Model.ListAreasPersonal = service.GetMMaster(new MMaster { MetaType = new MType { Code = "AREASPERSONA" } });
            Model.ListTipoEmpleado = db.Tipo.Where(f => f.Agrupacion.Codigo == "AREASPERSONA").ToList();
            //Model.ListAreasVehiculos = service.GetMMaster(new MMaster { MetaType = new MType { Code = "AREASVEHICULO" } });
            Model.ListTipoEmpleado = db.Tipo.Where(f => f.Agrupacion.Codigo == "AREASVEHICULO").ToList();
            //Model.ListAdicional = service.GetMMaster(new MMaster { MetaType = new MType { Code = "ADICIONAL" } });
            Model.ListTipoEmpleado = db.Tipo.Where(f => f.Agrupacion.Codigo == "ADICIONAL").ToList();
            #endregion
            #region Pago

           // Model.ListTipoPago = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOPAGO" } });
            Model.ListTipoEmpleado = db.Tipo.Where(f => f.Agrupacion.Codigo == "TIPOPAGO").ToList();
            // Model.ListFormaPago = service.GetMMaster(new MMaster { MetaType = new MType { Code = "FORMAPAGO" } });
            Model.ListTipoEmpleado = db.Tipo.Where(f => f.Agrupacion.Codigo == "FORMAPAGO").ToList();

            #endregion
            #region Entrega
            // Model.ListTipoEntrega = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOPAGO" } });
            Model.ListTipoEntrega = db.Tipo.Where(f => f.Agrupacion.Codigo == "TIPOPAGO").ToList();

            #endregion
        }



        #region Encabezado

        private void TipoSolicitud_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //Evaluo que haya sido seleccionado un registro
            if (TipoSolicitud.SelectedItem != null)
            {
                if (((Tipo)TipoSolicitud.SelectedItem).Codigo == "NORMAL")
                {
                    PanelDatosCarnet.IsEnabled = true;
                    PanelDatosPersona.IsEnabled = true;
                    PanelDatosVehiculo.IsEnabled = true;
                }
                else//Si entra la solicitud es una reexpedicion
                {
                    PanelDatosCarnet.IsEnabled = false;
                    PanelDatosPersona.IsEnabled = false;
                    PanelDatosVehiculo.IsEnabled = false;
                }
            }
        }


        private void CheckDeterioro_Unchecked_1(object sender, RoutedEventArgs e)
        {
            PanelDatosCarnet.IsEnabled = true;
            PanelDatosPersona.IsEnabled = false;
            PanelDatosVehiculo.IsEnabled = false;
        }

        private void CheckDeterioro_Checked_1(object sender, RoutedEventArgs e)
        {
            PanelDatosCarnet.IsEnabled = false;
            PanelDatosPersona.IsEnabled = false;
            PanelDatosVehiculo.IsEnabled = false;
        }

        private void SearchTerceroSolicitante_OnSelected_1(object sender, EventArgs e)
        {
            Model.RecordEncabezado.TerceroSolicitaID = (TerceroSolicita.Terceros).RowID;
            TerceroFactura.Terceros = TerceroSolicita.Terceros;
            Model.RecordPago.TerceroFacturaID = (TerceroFactura.Terceros).RowID;
        }

        #endregion

        #region Solicitud

        private void SearchTerceroFactura_OnSelected_1(object sender, EventArgs e)
        {
            Model.RecordPago.TerceroFacturaID = (TerceroFactura.Terceros).RowID;
        }

        private void RangoCarnet_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (Model.RecordSolicitud.FechaInicio == FechaInicio.SelectedDate)
            {
                Model.RecordSolicitud.FechaInicio = null;
                Model.RecordSolicitud.FechaFinal = null;
            }
        }

        private void cmb_TipoCarnet_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //Evaluo que haya sido seleccionado un registro
            if (TipoCarnet.SelectedItem != null)
            {
                if (((Tipo)TipoCarnet.SelectedItem).Nombre == "Vehiculo")
                {
                    PanelDatosVehiculo.Visibility = Visibility.Visible;
                    PanelDatosPersona.Visibility = Visibility.Collapsed;
                    //Cargo las opciones de carnets dependiendo del tipo de carnet solicitado
                   // Model.ListRangoCarnet = service.GetMMaster(new MMaster { MetaType = new MType { Code = "RANGOVEHICULOS" } }).OrderBy(f => f.NumOrder).ToList();
                    Model.ListRangoCarnet = db.Tipo.Where(f=>f.Agrupacion.Codigo == "RANGOVEHICULOS").OrderBy(f=>f.Orden).ToList();
                }
                else if (((Tipo)TipoCarnet.SelectedItem).Nombre == "Persona")
                {
                    PanelAreasPersona.Visibility = Visibility.Visible;
                    PanelDatosVehiculo.Visibility = Visibility.Collapsed;
                    PanelDatosPersona.Visibility = Visibility.Visible;
                    //Cargo las opciones de carnets dependiendo del tipo de carnet solicitado
                   // Model.ListRangoCarnet = service.GetMMaster(new MMaster { MetaType = new MType { Code = "RANGOPERSONAL" } }).OrderBy(f => f.NumOrder).ToList();
                    Model.ListRangoCarnet = db.Tipo.Where(f => f.Agrupacion.Codigo == "RANGOPERSONAL").OrderBy(f => f.Orden).ToList();

                }
                else if (((Tipo)TipoCarnet.SelectedItem).Nombre == "Persona(AP)")
                {
                    PanelAreasPersona.Visibility = Visibility.Collapsed;
                    PanelDatosVehiculo.Visibility = Visibility.Collapsed;
                    PanelDatosPersona.Visibility = Visibility.Visible;
                    //Cargo las opciones de carnets dependiendo del tipo de carnet solicitado
                   // Model.ListRangoCarnet = service.GetMMaster(new MMaster { MetaType = new MType { Code = "RANGOPERSONAL" } }).OrderBy(f => f.NumOrder).ToList();
                    Model.ListRangoCarnet = db.Tipo.Where(f => f.Agrupacion.Codigo == "RANGOPERSONAL").OrderBy(f => f.Orden).ToList();

                }
            }
        }

        private void btn_Cargar_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Filter = "Archivos de Imagen|*.jpg";

            if ((bool)open.ShowDialog())
            {
                Stream Archivo = open.OpenFile();
                String NombreArchivo = open.FileName;
                Foto.Text = NombreArchivo;
            }

        }

        private void btn_Capturar_Click_1(object sender, RoutedEventArgs e)
        {
            //Habilito el panel para tomar foto
            if (stk_TomarFoto.Visibility == Visibility.Visible)
            {
                stk_TomarFoto.Visibility = Visibility.Collapsed;
            }
            else
            {
                stk_TomarFoto.Visibility = Visibility.Visible;
                //ControlWebCam = new Webcam();
            }
        }

        private void dp_FechaInicio_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (RangoCarnet.SelectedIndex != -1 && FechaInicio.SelectedDate != null)
            {
                //Con el rango genero la fecha siguiente con el valor en el campo Code2 de la opcion seleccionada
                FechaFinal.SelectedDate = FechaInicio.SelectedDate.Value.AddDays(Double.Parse(((Tipo)RangoCarnet.SelectedItem).Codigo2));
            }
        }

        private void btn_GuardarSolicitud_Click_1(object sender, RoutedEventArgs e)
        {
            if (TipoSolicitud.SelectedItem == null)
            {
                Util.ShowError("Seleccione un Tipo de Solicitud");
                return;
            }
            if (((Tipo)TipoSolicitud.SelectedItem).Codigo2 == "REEXPEDICION")
            {
               // Solicitud solicitudReexpedida = service.GetSolicitud(new Solicitud { NoDocumento_Placa = NoDocumento.Text }).First();
                Solicitud solicitudReexpedida = db.Solicitud.FirstOrDefault(f => f.NoDocumento_Placa == NoDocumento.Text);


                if (solicitudReexpedida.FechaFinal < DateTime.Today)
                {
                    Util.ShowError("La solicitud seleccionada no cuenta con una vigencia disponible");
                    return;
                }
            }

            //Si es igual a 0 es porque es un registro nuevo de encabezado y solicitud
            if (Model.RecordEncabezado.RowID == 0 && Model.RecordSolicitud.RowID == 0)
            {
                //Validar ingreso de datos

                if (TerceroSolicita.Terceros == null)
                {
                    Util.ShowError("Seleccione un tercero solicitante");
                    return;
                }

                //Asigno los datos del encabezado

                Model.RecordEncabezado.UsuarioCreacion = App.curUser.NombreUsuario;
                Model.RecordEncabezado.FechaCreacion = DateTime.Now;
                //Model.RecordEncabezado = service.SaveEncabezado(Model.RecordEncabezado);
                db.Encabezado.Add(Model.RecordEncabezado);
                db.SaveChanges();
                //Guardo el encabezado

                GuardarSolicitud();
                //Guardo la solicitud


            }
            //Si la solicitud es 0 es porque es otro item del documento
            else if (Model.RecordSolicitud.RowID == 0)
            {
                GuardarSolicitud();
            }
            else
            {
                Model.RecordEncabezado.UsuarioModificacion = App.curUser.NombreUsuario;
                Model.RecordEncabezado.FechaModificacion = DateTime.Now;
                // service.UpdateEncabezado(Model.RecordEncabezado);
                db.SaveChanges();
                Model.RecordSolicitud.Area1 = (Tipo)Area1Persona.SelectedItem;
                Model.RecordSolicitud.Area2 = (Tipo)Area2Persona.SelectedItem;
                Model.RecordSolicitud.Area3_Puerta1 = (Tipo)Area3Persona.SelectedItem;
                Model.RecordSolicitud.Area4_Puerta2 = (Tipo)Area4Persona.SelectedItem;
                Model.RecordSolicitud.Adicional = (Tipo)Adicional.SelectedItem;
                Model.RecordSolicitud.Foto = Foto.Text;
                // service.UpdateSolicitud(Model.RecordSolicitud);
                db.SaveChanges();

                CleanToCreateSolicitud();

                Util.ShowMessage("Se actualizo exitosamente la solicitud");
            }
        }

        private void GuardarSolicitud()
        {
            if (TipoCarnet.SelectedIndex == -1)
            {
                Util.ShowError("Seleccione un tipo de carnet");
                return;
            }
            if (RangoCarnet.SelectedIndex == -1)
            {
                Util.ShowError("Seleccione un rango de carnet");
                return;
            }
            if (TipoEmpleado.SelectedIndex == -1)
            {
                Util.ShowError("Seleccione un tipo de empleado");
                return;
            }
            if (FechaInicio.SelectedDate == null)
            {
                Util.ShowError("Seleccione una fecha de inicio");
                return;
            }
            if (FechaInicio.SelectedDate == null)
            {
                Util.ShowError("Seleccione una fecha final");
                return;
            }
            // if (service.GetTarifa(new Tarifa { Rango = new Tipo { MetaMasterID = ((Tipo)RangoCarnet.SelectedItem).MetaMasterID } }).Where(f => f.FechaInicio <= FechaInicio.SelectedDate && f.FechaFinal >= FechaInicio.SelectedDate).Count() == 0)
            if ( db.Tarifa.Where(f=>f.Tipo5.RowID== ((Tipo)RangoCarnet.SelectedItem).RowID && f.FechaInicio <= FechaInicio.SelectedDate && f.FechaFinal >= FechaInicio.SelectedDate).Count() == 0)
            {
                Util.ShowError("No cuenta con una tarifa activa para la fecha solicitada");
                return;

            }
            if (((Tipo)TipoCarnet.SelectedItem).Codigo == "PERSONA")
            {
                if (NoDocumento.Text == "")
                {
                    Util.ShowError("Ingrese el no. de identificacion");
                    return;
                }
                if (String.IsNullOrEmpty(Nombres.Text))
                {
                    Util.ShowError("Ingrese el nombre ");
                    return;
                }
                if (String.IsNullOrEmpty(Apellidos.Text))
                {
                    Util.ShowError("Ingrese el apellido ");
                    return;
                }
                if (String.IsNullOrEmpty(Cargo.Text))
                {
                    Util.ShowError("Ingrese el cargo ");
                    return;
                }
                if (PanelAreasPersona.Visibility == Visibility.Visible)
                {
                    if (Area1Persona.SelectedIndex == -1)
                    {
                        Util.ShowError("Seleccione un area");
                        return;
                    }
                }
            }

            if (((Tipo)TipoCarnet.SelectedItem).Codigo == "VEHICULO")
            {
                if (Placa.Text == "")
                {
                    Util.ShowError("Ingrese la placa");
                    return;
                }
                if (Marca.Text == "")
                {
                    Util.ShowError("Ingrese la marca");
                    return;
                }
                if (Modelo.Text == "")
                {
                    Util.ShowError("Ingrese el modelo");
                    return;
                }
                if (NoMotor.Text == "")
                {
                    Util.ShowError("Ingrese el no. de motor");
                    return;
                }
                if (Area1Vehiculo.SelectedIndex == -1)
                {
                    Util.ShowError("Seleccione un area");
                    return;
                }
                if (Puerta1.SelectedIndex == -1)
                {
                    Util.ShowError("Seleccione una puerta");
                    return;
                }
                if (Soat.SelectedDate == null)
                {
                    Util.ShowError("Seleccione la fecha de vencimiento del soat");
                    return;
                }
            }

            //Asigno la tarifa de la solicitud
            Model.RecordSolicitud.Tarifa = service.GetTarifa(new Tarifa { Rango = new MMaster { MetaMasterID = ((MMaster)RangoCarnet.SelectedItem).MetaMasterID } })
               .Where(f => f.FechaInicio <= FechaInicio.SelectedDate && f.FechaFinal >= FechaInicio.SelectedDate).First();

            //Asigno los datos de la solicitud
            Model.RecordSolicitud.Encabezado = Model.RecordEncabezado;
            Model.RecordSolicitud.TipoSolicitud = TipoSolicitud.SelectedItem != null ? (Tipo)TipoSolicitud.SelectedItem : null;
            Model.RecordSolicitud.TipoCarnet = (Tipo)TipoCarnet.SelectedItem;
            Model.RecordSolicitud.RangoCarnet = (Tipo)RangoCarnet.SelectedItem;
            Model.RecordSolicitud.TipoEmpleado = (Tipo)TipoEmpleado.SelectedItem;
            Model.RecordSolicitud.Adicional = (Tipo)Adicional.SelectedItem;
            Model.RecordSolicitud.FechaInicio = FechaInicio.SelectedDate;
            Model.RecordSolicitud.FechaFinal = FechaFinal.SelectedDate;
            Model.RecordSolicitud.Comentario = ComentarioSolicitud.Text;
            Model.RecordSolicitud.Foto = Foto.Text;
            // Model.RecordSolicitud.Estado = service.GetStatus(new Status { Name = "Nueva", StatusType = new StatusType { Name = "SolicitudesCarnets" } }).First();
            Model.RecordSolicitud.Estado = db.Estado.FirstOrDefault(f=>f.Nombre == "Nueva" && f.);
            Model.RecordSolicitud.UsuarioCreacion = App.curUser.NombreUsuario;
            Model.RecordSolicitud.FechaCreacion = DateTime.Now;

            if (Model.RecordSolicitud.TipoCarnet.Code2 == "PERSONA")
            {
                Model.RecordSolicitud.NoDocumento_Placa = NoDocumento.Text;
                Model.RecordSolicitud.Nombres_Marca = Nombres.Text.ToUpper();
                Model.RecordSolicitud.Apellidos_Modelo = Apellidos.Text.ToUpper();
                Model.RecordSolicitud.Cargo_NoMotor = Cargo.Text.ToUpper();
                Model.RecordSolicitud.Area1 = (Tipo)Area1Persona.SelectedItem;
                Model.RecordSolicitud.Area2 = (Tipo)Area2Persona.SelectedItem;
                Model.RecordSolicitud.Area3_Puerta1 = (Tipo)Area3Persona.SelectedItem;
                Model.RecordSolicitud.Area4_Puerta2 = (Tipo)Area4Persona.SelectedItem;
            }
            if (Model.RecordSolicitud.TipoCarnet.Code == "VEHICULO")
            {
                Model.RecordSolicitud.NoDocumento_Placa = Placa.Text.ToUpper();
                Model.RecordSolicitud.Nombres_Marca = Marca.Text.ToUpper();
                Model.RecordSolicitud.Apellidos_Modelo = Modelo.Text.ToUpper();
                Model.RecordSolicitud.Cargo_NoMotor = NoMotor.Text.ToUpper();
                Model.RecordSolicitud.Area1 = (Tipo)Area1Vehiculo.SelectedItem;
                Model.RecordSolicitud.Area2 = (Tipo)Area2Vehiculo.SelectedItem;
                Model.RecordSolicitud.Area3_Puerta1 = (Tipo)Puerta1.SelectedItem;
                Model.RecordSolicitud.Area4_Puerta2 = (Tipo)Puerta2.SelectedItem;
            }
            //Guardo la solicitud
           // service.SaveSolicitud(Model.RecordSolicitud);
            db.SaveChanges.Add(Model.RecordSolicitud);
            db.SaveChanges();
            Util.ShowMessage("Se registro exitosamente la solicitud");

            CargarListasSolicitudes();

            CleanToCreateSolicitud();
        }

        public void CleanToCreateSolicitud()
        {
            Model.RecordSolicitud = new Solicitud();
            btn_AnularSolicitud.Visibility = Visibility.Collapsed;
        }

        public void CargarListasSolicitudes()
        {
            //Para las consultas se utilizan code2 porque para carnet de persona hay dos tipos: area publica y persona
           // Model.SolicitudesPersona = service.GetSolicitud(new Solicitud { Encabezado = Model.RecordEncabezado }).Where(f => f.TipoCarnet.Code2 != "VEHICULO").ToList();
            Model.SolicitudesPersona = 
            Model.SolicitudesVehiculo = service.GetSolicitud(new Solicitud { Encabezado = Model.RecordEncabezado, TipoCarnet = new MMaster { Code2 = "VEHICULO" } });
        }

        private void lvSolicitudesPersona_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (SolicitudesPersona.SelectedItem != null)
            {
                Model.RecordSolicitud = (Solicitud)SolicitudesPersona.SelectedItem;

                TerceroSolicita.Terceros = Model.RecordSolicitud.Encabezado.TerceroSolicita;

                //Asigno las areas 
                if (Model.RecordSolicitud.Area1 != null)
                {
                    Area1Persona.SelectedValue = Model.RecordSolicitud.Area1.MetaMasterID;
                }
                if (Model.RecordSolicitud.Area2 != null)
                {
                    Area2Persona.SelectedValue = Model.RecordSolicitud.Area2.MetaMasterID;
                }
                if (Model.RecordSolicitud.Area3_Puerta1 != null)
                {
                    Area3Persona.SelectedValue = Model.RecordSolicitud.Area3_Puerta1.MetaMasterID;
                }
                if (Model.RecordSolicitud.Area4_Puerta2 != null)
                {
                    Area4Persona.SelectedValue = Model.RecordSolicitud.Area4_Puerta2.MetaMasterID;
                }

                //Si esta confirmada la solicitud no puede realizar modificaciones
                if (Model.RecordSolicitud.Estado.Nombre == "Confirmada")
                {
                    panelBotonesSolicitud.IsEnabled = false;
                }
                else if (Model.RecordSolicitud.Estado.Nombre == "Anulada")
                {
                    panelBotonesSolicitud.IsEnabled = false;
                }
                else
                {
                    panelBotonesSolicitud.IsEnabled = true;
                }

                //btn_AnularSolicitud.Visibility = Visibility.Visible;
                btn_AnularSolicitud.Visibility = Visibility.Collapsed;
            }
        }

        private void SolicitudesVehiculo_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (SolicitudesVehiculo.SelectedItem != null)
            {
                Model.RecordSolicitud = (Solicitud)SolicitudesVehiculo.SelectedItem;
                TerceroSolicita.Terceros = Model.RecordSolicitud.Encabezado.TerceroSolicita;

                //Asigno las areas 
                if (Model.RecordSolicitud.Area1 != null)
                {
                    Area1Persona.SelectedValue = Model.RecordSolicitud.Area1.MetaMasterID;
                }
                if (Model.RecordSolicitud.Area2 != null)
                {
                    Area2Persona.SelectedValue = Model.RecordSolicitud.Area2.MetaMasterID;
                }
                if (Model.RecordSolicitud.Area3_Puerta1 != null)
                {
                    Area3Persona.SelectedValue = Model.RecordSolicitud.Area3_Puerta1.MetaMasterID;
                }
                if (Model.RecordSolicitud.Area4_Puerta2 != null)
                {
                    Area4Persona.SelectedValue = Model.RecordSolicitud.Area4_Puerta2.MetaMasterID;
                }

                if (Model.RecordSolicitud.Estado.Nombre == "Confirmada")
                {
                    panelBotonesSolicitud.IsEnabled = false;
                }
                else if (Model.RecordSolicitud.Estado.Nombre == "Anulada")
                {
                    panelBotonesSolicitud.IsEnabled = false;
                }
                else
                {
                    panelBotonesSolicitud.IsEnabled = true;
                }

                // btn_AnularSolicitud.Visibility = Visibility.Visible;
                btn_AnularSolicitud.Visibility = Visibility.Collapsed;
            }
        }

        private void BuscarSolicitud_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (BuscarSolicitud.Text == "")
                {
                    Util.ShowError("Ingrese una identidicacion o placa");
                    return;
                }

                if (service.GetSolicitud(new Solicitud { NoDocumento_Placa = BuscarSolicitud.Text }).Count == 0)
                {
                    Util.ShowError("No existe registro");
                    return;
                }

                Tipo tipoSeleccionado = null;//variable aux para que guarde el tipo de solicitud que habia selecccionado.

                //Valida si hay un tipo de solicitud seleccionado
                if (TipoSolicitud.SelectedItem != null)
                {
                    tipoSeleccionado = (Tipo)TipoSolicitud.SelectedItem;
                }

                Model.RecordSolicitud = service.GetSolicitud(new Solicitud { NoDocumento_Placa = BuscarSolicitud.Text }).OrderByDescending(f => f.CreationDate).First();
                Model.RecordSolicitud.RowID = 0;
                Model.RecordSolicitud.TipoSolicitud = tipoSeleccionado;//Asigno el tipo de solicitud ingresada anteriormente.


                TerceroSolicita.Terceros = Model.RecordSolicitud.Encabezado.TerceroSolicita;
                Model.RecordEncabezado.TerceroSolicita = Model.RecordSolicitud.Encabezado.TerceroSolicita;

                //if (Model.RecordSolicitud.Area1 != null)
                //{
                //    Area1Persona.SelectedValue = Model.RecordSolicitud.Area1.MetaMasterID;
                //}
                //if (Model.RecordSolicitud.Area2 != null)
                //{
                //    Area2Persona.SelectedValue = Model.RecordSolicitud.Area2.MetaMasterID;
                //}
                //if (Model.RecordSolicitud.Area3_Puerta1 != null)
                //{
                //    Area3Persona.SelectedValue = Model.RecordSolicitud.Area3_Puerta1.MetaMasterID;
                //}
                //if (Model.RecordSolicitud.Area4_Puerta2 != null)
                //{
                //    Area4Persona.SelectedValue = Model.RecordSolicitud.Area4_Puerta2.MetaMasterID;
                //}
                BuscarSolicitud.Text = "";

            }
        }

        private void btnCerrarTab_Click_1(object sender, RoutedEventArgs e)
        {
            if (ControlWebCam.videoSourcePlayer1.IsRunning)
            {
                ControlWebCam.videoSourcePlayer1.SignalToStop();
                ControlWebCam.videoSourcePlayer1.Stop();
            }
            ((ListaCarnetsPresenter)PresenterParent).View.TabPadre.Items.Remove(((TabItem)((ListaCarnetsPresenter)PresenterParent).View.TabPadre.SelectedItem));

            //Selecciono por defecto el Tab con el listado de Carnet
            ((TabItem)((ListaCarnetsPresenter)PresenterParent).View.TabPadre.Items[0]).Focus();

        }

        private void btn_ConfirmarSolicitud_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Model.RecordSolicitud.Foto))
            {
                Util.ShowError("No tiene ninguna foto asignada.");
                return;
            }

            if (UtilWindow.ConfirmOK("Esta seguro de confirmar la solicitud? ") == true)
            {
                if (Model.RecordSolicitud.RowID != 0)
                {
                    Model.RecordSolicitud.Estado = service.GetStatus(new Status { Name = "Confirmada", StatusType = new StatusType { Name = "SolicitudesCarnets" } }).First();
                    Model.RecordSolicitud.UsuarioModificacion = App.curUser.NombreUsuario;
                    Model.RecordSolicitud.FechaModificacion = DateTime.Now;

                    service.UpdateSolicitud(Model.RecordSolicitud);
                    Util.ShowMessage("Se confirmo exitosamente la solicitud");
                    //Si es exento o reexpedicion atribuible genero el pago automaticamente
                    if (((Tipo)Model.RecordSolicitud.TipoEmpleado).Code2 == "EXENTO" || ((Tipo)TipoSolicitud.SelectedItem).Codigo == "REEXPEDICIONATRIBUIBLE")
                    {
                        Model.RecordPago.Encabezado = Model.RecordEncabezado;
                        Model.RecordPago.Solicitud = Model.RecordSolicitud;
                        Model.RecordPago.TipoPago = service.GetMMaster(new MMaster { Code = "UNITARIO" }).First();
                        Model.RecordPago.FormaPago = service.GetMMaster(new MMaster { Code = "EFECTIVO" }).First();
                        Model.RecordPago.TerceroFactura = Model.RecordEncabezado.TerceroSolicita;
                        Model.RecordPago.Estado = service.GetStatus(new Status { Name = "Confirmada", StatusType = new StatusType { Name = "PagosCarnets" } }).First();
                        Model.RecordPago.Comentario = ((MMaster)Model.RecordSolicitud.TipoEmpleado).Code2 == "EXENTO" ? "Pago automatico generado por Exento" : "Pago automatico generado por Reexpedicion Atribuible";
                        Model.RecordPago.PagoAutomatico = true;
                        Model.RecordPago.UsuarioCreacion = App.curUser.NombreUsuario;
                        Model.RecordPago.FechaCreacion = DateTime.Now;
                        service.SavePago(Model.RecordPago);
                        CleanToCreatePago();
                    }

                    CleanToCreateSolicitud();
                    TabPagos.IsSelected = true;
                    TabPagos.IsEnabled = true;
                    Model.SolicitudesParaPago = service.GetSolicitud(new Solicitud { Encabezado = Model.RecordEncabezado, Estado = new Status { Name = "Confirmada", StatusType = new StatusType { Name = "SolicitudesCarnets" } } });
                    Model.Pagos = service.SelectPago(new Pago { Encabezado = Model.RecordEncabezado });
                }
                else
                {
                    Util.ShowError("Seleccione una solicitud a confirmar");
                    return;
                }
            }
        }

        private void btn_NuevaSolicitud_Click_1(object sender, RoutedEventArgs e)
        {
            CleanToCreateSolicitud();
            if (Model.Pagos != null && Model.Pagos.Where(f => f.TipoPago.Code == "GENERAL").Count() > 0)
            {
                panelBotonesSolicitud.IsEnabled = false;
            }
            else
            {
                panelBotonesSolicitud.IsEnabled = true;

            }
        }

        private void Foto_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Foto.Text))
            {
                if (!File.Exists(Foto.Text))
                {
                    Util.ShowError("No existe la ruta asignada a la foto.");
                    return;
                }
                imagen_Foto.Visibility = Visibility.Visible;
                imagen_Foto.Source = new BitmapImage(new Uri(Foto.Text));
                if (Model.RecordSolicitud.RowID != 0)
                {
                    Model.RecordSolicitud.Foto = Foto.Text;
                    service.UpdateSolicitud(Model.RecordSolicitud);
                }

            }
            else
            {
                imagen_Foto.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Pago

        private void FormaPago_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (FormaPago.SelectedIndex != -1)
            {
                if (((Tipo)FormaPago.SelectedItem).Codigo == "EFECTIVO")
                {
                    panelNoCheque.Visibility = Visibility.Collapsed;
                    panelBanco.Visibility = Visibility.Collapsed;
                }
                else
                {
                    panelNoCheque.Visibility = Visibility.Visible;
                    panelBanco.Visibility = Visibility.Visible;
                }
            }
        }

        private void TipoPago_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (TipoPago.SelectedIndex != -1)
            {
                if (((Tipo)TipoPago.SelectedItem).Codigo == "UNITARIO")
                {
                    panelSeleccionarSolicitud.Visibility = Visibility.Visible;
                }
                else
                {
                    //Busco las solicitudes normales
                    IList<Solicitud> listaSolicitudesNormal = service.GetSolicitud(new Solicitud { TipoSolicitud = new MMaster { Code = "NORMAL" }, Encabezado = Model.RecordEncabezado });
                    //Busco las solicitudes de reexpedicion cobrables
                    IList<Solicitud> listaSolicitudesReexpedicion = service.GetSolicitud(new Solicitud { TipoSolicitud = new MMaster { Code = "REEXPEDICIONNOATRIBUIBLE" }, Encabezado = Model.RecordEncabezado });

                    //Asigno el valor de la sumatoria de todas las solicitudes
                    Model.RecordPago.Valor = listaSolicitudesNormal.Sum(F => F.Tarifa.Valor);
                    Model.RecordPago.Valor += listaSolicitudesReexpedicion.Sum(F => F.Tarifa.ValorReexpedicion);
                    SolicitudAPagar.SelectedIndex = -1;
                    panelSeleccionarSolicitud.Visibility = Visibility.Collapsed;
                }
            }


        }

        private void SolicitudAPagar_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Solicitud solicitud = (Solicitud)SolicitudAPagar.SelectedItem;

            if (solicitud != null)
            {
                if (((Tipo)solicitud.TipoSolicitud).Code == "REEXPEDICIONNOATRIBUIBLE")
                {
                    if (solicitud.RangoCarnet.DefValue == "PERMANENTE")
                    {
                        Model.RecordPago.Valor = solicitud.Tarifa.ValorReexpedicion;
                    }
                    else
                    {
                        Model.RecordPago.Valor = solicitud.Tarifa.Valor;
                    }
                }
                else if (((Tipo)solicitud.TipoSolicitud).Code == "REEXPEDICIONATRIBUIBLE")
                {
                    Model.RecordPago.Valor = 0;
                }
                else//Si entra es porque la solicitud es normal.
                {
                    Model.RecordPago.Valor = solicitud.Tarifa.Valor;
                }

            }
        }

        private void btn_GuardarPago_Click_1(object sender, RoutedEventArgs e)
        {
            if (panelSeleccionarSolicitud.Visibility == Visibility.Visible)
            {
                if (SolicitudAPagar.SelectedItem == null)
                {
                    Util.ShowError("Seleccione una solicitud");
                    return;
                }
            }

            //Si el RowID del Pago es 0 es porque es otro item del documento
            if (Model.RecordPago.RowID == 0)
            {
                IList<Pago> listaPagos = service.SelectPago(new Pago { Solicitud = (Solicitud)SolicitudAPagar.SelectedItem, Encabezado = Model.RecordEncabezado, TipoPago = new MMaster { Code = "UNITARIO" } });
                IList<Pago> listaPagos = db.Solicitud.Where(f=>f.);
                // IList<Pago> listaPagosGeneral = service.SelectPago(new Pago { Encabezado = Model.RecordEncabezado, TipoPago = new MMaster { Code = "GENERAL" } });
                IList<Pago> listaPagosGeneral = db.Pago.Where(f=>f.Encabezado==Model.RecordEncabezado && f.Tipo.Codigo == "GENERAL").ToList();

                if (listaPagos.Count > 0 || listaPagosGeneral.Count > 0)
                {
                    Util.ShowError("La solicitud ya cuenta con un pago");
                    return;
                }

                GuardarPago();
            }
            else
            {

                service.UpdateEncabezado(Model.RecordEncabezado);

                if (SolicitudAPagar.SelectedIndex != -1)
                {
                    Model.RecordPago.Solicitud = (Solicitud)SolicitudAPagar.SelectedItem;
                }
                if (TerceroFactura.Terceros != null)
                {
                    Model.RecordPago.TerceroFactura = TerceroFactura.Terceros;
                }
                service.UpdatePago(Model.RecordPago);
                Util.ShowMessage("Se actualizo exitosamente el Pago");
            }
        }

        private void GuardarPago()
        {
            if (TipoPago.SelectedIndex == -1)
            {
                Util.ShowError("Seleccione un tipo de pago");
                return;
            }
            if (((Tipo)TipoPago.SelectedItem).Codigo == "UNITARIO")
            {
                if (SolicitudAPagar.SelectedIndex == -1)
                {
                    Util.ShowError("Seleccione una solicitud a pagar");
                    return;
                }
            }
            if (FormaPago.SelectedIndex == -1)
            {
                Util.ShowError("Seleccione una forma de pago");
                return;
            }
            if (Model.RecordPago.ValorPagado == 0 || Model.RecordPago.ValorPagado < Model.RecordPago.Valor)
            {
                Util.ShowError("Ingrese el valor recibido");
                return;
            }


            Model.RecordPago.Encabezado = Model.RecordEncabezado;
            if (SolicitudAPagar.SelectedIndex != -1)
            {
                Model.RecordPago.Solicitud = (Solicitud)SolicitudAPagar.SelectedItem;
            }

            Model.RecordPago.TipoPago = (Tipo)TipoPago.SelectedItem;
            Model.RecordPago.FormaPago = (Tipo)FormaPago.SelectedItem;
            Model.RecordPago.PagoAutomatico = false;
            Model.RecordPago.Estado = service.GetStatus(new Status { Name = "Nueva", StatusType = new StatusType { Name = "PagosCarnets" } }).First();
            Model.RecordPago.UsuarioCreacion = App.curUser.NombreUsuario;
            Model.RecordPago.FechaCreacion = DateTime.Now;

            service.SavePago(Model.RecordPago);
            Util.ShowMessage("Se registro exitosamente el pago");

            Model.Pagos = service.SelectPago(new Pago { Encabezado = Model.RecordEncabezado });
            CleanToCreatePago();
        }

        public void CleanToCreatePago()
        {
            Model.RecordPago = new Pago();
            ValorCambio.Text = "";
            TerceroFactura.Terceros = new Terceros();
            TerceroFactura.txtData.Text = "";
        }

        private void Pagos_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (Pagos.SelectedItem == null)
            { return; }

            Model.RecordPago = (Pago)Pagos.SelectedItem;
            if (Model.RecordPago.TerceroFactura != null)
            {
                TerceroFactura.Terceros = Model.RecordPago.TerceroFactura;
            }

            foreach (UserByRol rol in App.curUser.UserRols)
            {
                if (rol.Rol.Name == "OperacionCarnet")
                {
                    BotonesPago.IsEnabled = false;
                    return;
                }
            }

            if (Model.RecordPago.Estado.Nombre == "Confirmada")
            {
                panelBotonesPago.IsEnabled = false;
                panel_BotonesImpresion.Visibility = Visibility.Visible;
            }
            else if (Model.RecordPago.Estado.Nombre == "Anulada")
            {
                panelBotonesPago.IsEnabled = false;
                panel_BotonesImpresion.Visibility = Visibility.Collapsed;
            }
            else
            {
                panelBotonesPago.IsEnabled = true;
                panel_BotonesImpresion.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_ConfirmarPago_Click_1(object sender, RoutedEventArgs e)
        {
            if (Model.RecordPago.RowID != 0)
            {
                if (Model.RecordPago.Estado.Nombre != "Nueva")
                {
                    Util.ShowError("No se puede Confirmar este pago");
                    return;
                }
                if (UtilWindow.ConfirmOK("Esta seguro de confirmar el pago? ") == true)
                {
                    //IList<Status> d = service.GetStatus(new Status { Name = "Confirmada", StatusType = new StatusType { Name = "PagosCarnets" } });
                    Model.RecordPago.Estado = service.GetStatus(new Status { Name = "Confirmada", StatusType = new StatusType { Name = "PagosCarnets" } }).First();
                    Model.RecordPago.UsuarioModificacion = App.curUser.NombreUsuario;
                    Model.RecordPago.FechaModificacion = DateTime.Now;
                    service.UpdatePago(Model.RecordPago);
                    Util.ShowMessage("Se confirmo exitosamente el pago");
                    Model.RecordEntrega = new Entrega();
                    Model.RecordEntrega.Encabezado = Model.RecordEncabezado;
                    Model.RecordEntrega.Pago = Model.RecordPago;
                    Model.RecordEntrega.Estado = service.GetStatus(new Status { Name = "Nueva", StatusType = new StatusType { Name = "EntregasCarnets" } }).First();
                    btn_ImprimirFactura_Click_1(sender, e);
                    TabEntregas.IsEnabled = true;
                    TabEntregas.Focus();
                    //   Model.Pagos = service.SelectPago(new Pago { Encabezado = Model.RecordEncabezado });
                    CleanToCreatePago();
                }


            }
            else
            {
                Util.ShowError("Seleccione un pago a confirmar");
                return;
            }
        }

        private void ValorRecibido_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                if (string.IsNullOrEmpty(ValorRecibido.Text))
                {
                    Util.ShowError("Valor invalido");
                    return;
                }
                if (int.Parse(ValorRecibido.Text) == 0 || int.Parse(ValorRecibido.Text) < Model.RecordPago.Valor)
                {
                    Util.ShowError("Valor invalido");
                    return;
                }

                ValorCambio.Text = (int.Parse(ValorRecibido.Text) - int.Parse(ValorPago.Text)).ToString();
            }
        }

        private void btn_NuevoPago_Click_1(object sender, RoutedEventArgs e)
        {
            CleanToCreatePago();
            if (Model.Pagos.Where(f => f.TipoPago.Code == "GENERAL").Count() > 0)
            {
                panelBotonesPago.IsEnabled = false;
            }
            else
            {
                panelBotonesPago.IsEnabled = true;

            }
        }

        private void btn_ImprimirFactura_Click_1(object sender, RoutedEventArgs e)
        {
            if (Pagos.SelectedIndex == -1)
            {
                Util.ShowError("Seleccione un pago");
            }
            //Para los pagos que se generan automaticos y quedan sin tercero, no se les permite imprimir
            if (((Pago)Pagos.SelectedItem).TerceroFactura == null)
            { return; }
            IList<Solicitud> listaSolicitudes;

            if (((Pago)Pagos.SelectedItem).TipoPago.Code == "GENERAL")
            {
                listaSolicitudes = service.GetSolicitud(new Solicitud { Encabezado = Model.RecordEncabezado, Estado = new Status { Name = "Confirmada" } });
            }
            else//Entra si el pago es unitario, La solicitud ya esta asignada al pago
            {
                listaSolicitudes = new List<Solicitud>();
            }

            //Llamo al reporte enviandole la informacion de las solicitudes y pago.
            PrinterControl.imprimirFacturaCarnet(listaSolicitudes, (Pago)Pagos.SelectedItem);


        }


        #endregion

        #region Entrega

        private void btn_AnularSolicitud_Click_1(object sender, RoutedEventArgs e)
        {
            //Comentario para probar
            //if (UtilWindow.ConfirmOK("Esta seguro de anular la solicitud? ") == true)
            //{
            //    if (Model.RecordSolicitud != null)
            //    {
            //        Model.RecordSolicitud.Estado = service.GetStatus(new Status { Name = "Anulada", StatusType = new StatusType { Name = "SolicitudesCarnets" } }).First();
            //        service.UpdateSolicitud(Model.RecordSolicitud);
            //        Util.ShowMessage("Se anulo exitosamente la solicitud");
            //        CleanToCreateSolicitud();
            //    }
            //    else
            //    {
            //        Util.ShowError("Debe seleccionar una solicitud");
            //    }
            //}
        }

        #endregion

        private void TipoEntrega_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (TipoEntrega.SelectedItem == null)
            { return; }

            if (((Tipo)TipoEntrega.SelectedItem).Codigo == "UNITARIO")
            {
                panelSeleccionarPago.Visibility = Visibility.Visible;
                Model.PagosParaEntrega = service.SelectPago(new Pago { Encabezado = Model.RecordEncabezado, Estado = new Status { Name = "Confirmada", StatusType = new StatusType { Name = "PagosCarnets" } } }).ToList();
            }
            else
            {
                panelSeleccionarPago.Visibility = Visibility.Collapsed;
            }
        }

        private void Entregas_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (Entregas.SelectedItem == null)
            { return; }

            Model.RecordEntrega = (Entrega)Entregas.SelectedItem;

            TipoEntrega.SelectedValue = ((Entrega)Entregas.SelectedItem).TipoEntrega.MetaMasterID;

            PagoAEntregar.SelectedValue = ((Entrega)Entregas.SelectedItem).Pago != null ? ((Entrega)Entregas.SelectedItem).Pago.RowID : 0;

            if (Model.RecordEntrega.Estado.Nombre == "Confirmada")
            {
                panelBotonesEntrega.IsEnabled = false;
                panelImprimirCarnet.Visibility = Visibility.Visible;
            }
            else
            {
                panelBotonesEntrega.IsEnabled = true;
                panelImprimirCarnet.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_NuevoEntrega_Click_1(object sender, RoutedEventArgs e)
        {
            CleanToCreateEntrega();
            if (Model.Entregas.Where(f => f.TipoEntrega.Code == "GENERAL").Count() > 0)
            {
                panelBotonesEntrega.IsEnabled = false;
            }
            else
            {
                panelBotonesEntrega.IsEnabled = true;

            }
        }

        private void btn_GuardarEntrega_Click_1(object sender, RoutedEventArgs e)
        {
            if (panelSeleccionarPago.Visibility == Visibility.Visible)
            {
                if (PagoAEntregar.SelectedItem == null)
                {
                    Util.ShowError("Seleccione un Pago");
                    return;
                }
            }

            //Si el RowID del Pago es 0 es porque es otro item del documento
            if (Model.RecordEntrega.RowID == 0)
            {
                //Si tiene registrada una entrega con el pago seleccionado
                IList<Entrega> listaEntrega = service.GetEntrega(new Entrega { Pago = (Pago)PagoAEntregar.SelectedItem, Encabezado = Model.RecordEncabezado, TipoEntrega = new MMaster { Code = "UNITARIO" } });
                //Si tiene registrada una entrega general
                IList<Entrega> listaEntregaGeneral = service.GetEntrega(new Entrega { Encabezado = Model.RecordEncabezado, TipoEntrega = new Tipo { Codigo = "GENERAL" } });

                if (listaEntrega.Count > 0 || listaEntregaGeneral.Count > 0)
                {
                    Util.ShowError("El pago ya cuenta con una entrega");
                    return;
                }

                GuardarEntrega();
            }
            else
            {
                //Para Actualizar
                service.UpdateEncabezado(Model.RecordEncabezado);

                if (PagoAEntregar.SelectedIndex != -1)
                {
                    Model.RecordEntrega.Pago = (Pago)PagoAEntregar.SelectedItem;
                }
                service.UpdateEntrega(Model.RecordEntrega);
                Util.ShowMessage("Se actualizo exitosamente la Entrega");
            }
        }

        private void GuardarEntrega()
        {
            if (TipoEntrega.SelectedIndex == -1)
            {
                Util.ShowError("Seleccione un tipo de Entrega");
                return;
            }
            if (((Tipo)TipoEntrega.SelectedItem).Codigo == "UNITARIO")
            {
                if (PagoAEntregar.SelectedIndex == -1)
                {
                    Util.ShowError("Seleccione pago a Entregar");
                    return;
                }
            }

            Model.RecordEntrega.Encabezado = Model.RecordEncabezado;
            if (((Tipo)TipoEntrega.SelectedItem).Codigo == "UNITARIO")
            {
                if (PagoAEntregar.SelectedIndex != -1)
                {
                    Model.RecordEntrega.Pago = (Pago)PagoAEntregar.SelectedItem;
                }
            }

            Model.RecordEntrega.TipoEntrega = (Tipo)TipoEntrega.SelectedItem;
            Model.RecordEntrega.Estado = service.GetStatus(new Status { Name = "Nueva", StatusType = new StatusType { Name = "EntregasCarnets" } }).First();
            Model.RecordPago.UsuarioCreacion = App.curUser.NombreUsuario;
            Model.RecordPago.FechaCreacion = DateTime.Now;

            service.SaveEntrega(Model.RecordEntrega);
            Util.ShowMessage("Se registro exitosamente la entrega");

            Model.Entregas = service.GetEntrega(new Entrega { Encabezado = Model.RecordEncabezado }).OrderBy(f => f.RowID).ToList();
            CleanToCreateEntrega();
        }

        public void CleanToCreateEntrega()
        {
            Model.RecordEntrega = new Entrega();
            TipoEntrega.SelectedIndex = -1;
            PagoAEntregar.SelectedIndex = -1;
        }

        private void btn_ConfirmarEntrega_Click_1(object sender, RoutedEventArgs e)
        {
            if (Model.RecordEntrega.RowID != 0)
            {
                if (Model.RecordEntrega.Estado.Nombre != "Nueva")
                {
                    Util.ShowError("No se puede Confirmar esta entrega");
                    return;
                }
                if (UtilWindow.ConfirmOK("Esta seguro de realizar la entrega? ") == true)
                {
                    Model.RecordEntrega.Estado = service.GetStatus(new Status { Name = "Confirmada", StatusType = new StatusType { Name = "EntregasCarnets" } }).First();
                    Model.RecordEntrega.UsuarioModificacion = App.curUser.NombreUsuario;
                    Model.RecordEntrega.FechaModificacion = DateTime.Now;
                    service.UpdateEntrega(Model.RecordEntrega);
                    Util.ShowMessage("Se confirmo exitosamente la Entrega");
                    CleanToCreateEntrega();
                    panelImprimirCarnet.Visibility = Visibility.Visible;
                }
            }
            else
            {
                Util.ShowError("Seleccione una entrega a confirmar");
                return;
            }
        }

        private void btn_ImprimirCarnet_Click_1(object sender, RoutedEventArgs e)
        {
            if (Entregas.SelectedIndex == -1)
            {
                Util.ShowError("Seleccione una Entrega");
                return;
            }
            Entrega auxEntrega = (Entrega)Entregas.SelectedItem;

            if (auxEntrega.Estado.Nombre != "Confirmada")
            {
                Util.ShowError("No puede imprimir este carnet");
                return;
            }
            int cantEntregasGenerales = service.GetEntrega(new Entrega { Encabezado = Model.RecordEncabezado, TipoEntrega = new Tipo { Codigo = "GENERAL" } }).Count();

            //Si tiene general, imprimo todos los pagos que tenga registrados
            if (cantEntregasGenerales > 0)
            {
                IList<Pago> listaPagos = service.SelectPago(new Pago { Encabezado = Model.RecordEncabezado, Estado = new Status { Name = "Confirmada", StatusType = new StatusType { Name = "PagosCarnets" } } });
                auxEntrega.CantidadImpreso = auxEntrega.CantidadImpreso + 1;
                foreach (Pago aux in listaPagos)
                {
                    if (aux.Solicitud.RangoCarnet.DefValue == "VIGENCIA")
                    {
                        PrinterControl.imprimirReporteStiker(aux.Solicitud);
                    }
                    else
                    {
                        PrinterControl.imprimirReporteCarnet(aux.Solicitud);
                    }
                }
                service.UpdateEntrega(auxEntrega);
            }
            //Si no tiene general imprimo solamente el que selecciono
            else
            {
                auxEntrega.CantidadImpreso = auxEntrega.CantidadImpreso + 1;
                if (auxEntrega.Pago.Solicitud.RangoCarnet.DefValue == "VIGENCIA")
                {
                    PrinterControl.imprimirReporteStiker(auxEntrega.Pago.Solicitud);
                    service.UpdateEntrega(auxEntrega);
                }
                else
                {
                    service.UpdateEntrega(auxEntrega);
                    PrinterControl.imprimirReporteCarnet(((Entrega)Entregas.SelectedItem).Pago.Solicitud);
                }
            }
            Model.Entregas = service.GetEntrega(new Entrega { Encabezado = Model.RecordEncabezado }).OrderBy(f => f.RowID).ToList();
        }

        private void TabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion
        private void CapturarImagen_Click(object sender, RoutedEventArgs e)
        {
            if (ControlWebCam.videoSourcePlayer1.IsRunning == true)
            {
                ControlWebCam.CapturarImagen();
            }
            else
            {
                Util.ShowError("Seleccione un dispositivo");
            }

            if (!String.IsNullOrEmpty(ControlWebCam.ruta))
            {
                Foto.Text = ControlWebCam.ruta;
                Model.RecordSolicitud.Foto = ControlWebCam.ruta;
                //ControlWebCam.videoSourcePlayer1.Stop();                
                ControlWebCam.cmbDispositivoCamara.SelectedIndex = -1;
                stk_TomarFoto.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_AnularPagos_Click(object sender, RoutedEventArgs e)
        {
            if (UtilWindow.ConfirmOK("Esta seguro de anular el pago? ") == true)
            {
                if (Model.RecordPago != null)
                {
                    if (Model.RecordPago.Estado.Nombre == "Nueva")
                    {
                        Model.RecordSolicitud.Estado = service.GetStatus(new Status { Name = "Anulada", StatusType = new StatusType { Name = "PagosCarnets" } }).First();
                        service.UpdateSolicitud(Model.RecordSolicitud);
                        Util.ShowMessage("Se anulo exitosamente el pago");
                        CleanToCreatePago();
                    }
                    else { Util.ShowError("No puede anular este pago"); }
                }
                else
                {
                    Util.ShowError("Debe seleccionar un Pago");
                }
            }
        }

    }

    public interface IDatosCarnetsView
    {
        //Clase Modelo
        DatosCarnetsModel Model { get; set; }
        Object PresenterParent { get; set; }
    }

}