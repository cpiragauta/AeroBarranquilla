﻿using System;
using WpfFront.Model;
using WpfFront.Modelo;
using WpfFront.Vista;
using Assergs.Windows;
using WMComposite.Events;
using Microsoft.Practices.Unity;
using WpfFront.Common;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace WpfFront.Controlador
{

    public interface IFacturasTasasPresenter
    {
        IFacturasTasasView View { get; set; }
        ToolWindow Window { get; set; }
    }


    public class FacturasTasasPresenter : IFacturasTasasPresenter
    {
        public IFacturasTasasView View { get; set; }
        private readonly IUnityContainer container;
        private readonly wmsEntities _db;
        public ToolWindow Window { get; set; }
        int diasVen = 0;
        int diasEntrega = 0;


        public FacturasTasasPresenter(IUnityContainer container, IFacturasTasasView view)
        {

            View = view;
            this.container = container;
            this._db = new wmsEntities();
            View.Model = this.container.Resolve<FacturasTasasModel>();

            view.CargarDatosFactura += this.OnCargarDatosFactura;
            view.CargarDatosServicios += this.OnCargarDatosServicios;

            View.SearchRecords += new EventHandler<EventArgs>(this.OnSearchRecords);
            View.ProcessRecords += new EventHandler<EventArgs>(this.OnProcessRecords);
            View.getListaSinFiltros += new EventHandler<EventArgs>(this.OnGetListaSinFiltros);
            View.EnviarERP += new EventHandler<EventArgs>(this.OnEnviarERP);
            View.ConfirmacionEnvioERP += new EventHandler<EventArgs>(this.OnConfirmacionEnvioERP);
            //view.Model.ListaTipoOperacion = _db.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOVUELO" }, Active = true }).OrderBy(f => f.NumOrder).ToList();
            view.Model.ListaTipoOperacion = _db.Tipo.Where(f=> f.Agrupacion.Codigo == "TIPOVUELO" && f.Activo == true).OrderBy(f => f.Orden).ToList();
            //view.Model.ListaTipoTasa = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOTARIFA" }, Active = true }).OrderBy(f => f.NumOrder).Where(f => f.Codigo.Contains("TASAS")).ToList();
            this.getListaSinFiltros();
            getListaFacturasAgrupadas();
            View.Model.Factura = new Facturas();
        }

        private void OnSearchRecords(object sender, EventArgs e)
        {
            try
            {
                //Controlo que seleccione las dos fechas 
                if (String.IsNullOrEmpty(View.Model.Factura.FechaInicio.ToString()) || String.IsNullOrEmpty(View.Model.Factura.FechaFinal.ToString()))
                {
                    Util.ShowError("Seleccione Fecha Inicial y Final");
                    return;
                }
                else if (View.TipoOperacionList.SelectedItem == null)
                {
                    Util.ShowError("Seleccione un Tipo de Operacion");
                    return;
                }
                else if (View.CompaniaFactura.Terceros == null)
                {
                    Util.ShowError("Seleccione una Aerolinea");
                    return;
                }
                else
                {
                    string tipoOperacion = ((Tipo)View.TipoOperacionList.SelectedItem).Codigo;
                    int idCompania = View.CompaniaFactura.Terceros.RowID;
                    //Si NO estan seleccionadas las fechas
                    //if (String.IsNullOrEmpty(View.Model.Factura.FechaInicio.ToString()) && String.IsNullOrEmpty(View.Model.Factura.FechaFinal.ToString()))
                    //{
                    //    //Cargo la lista Agrupada
                    //    View.Model.RecordListAerodromoAgrupada = service.GetServicios(new Servicios
                    //    {                            
                    //        Operacion = new Operacion
                    //        {
                    //            Llegada = new Llegada { TipoVuelo = new MMaster { Code = tipoOperacion } },
                    //            Aeronave = new Aeronaves { CompañiaFactura = new Terceros { RowID = idCompania } }
                    //        },
                    //        Status = new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "Servicios" } }
                    //    }).Where(f => f.Tipo.Codigo == tipoTasa && f.Operacion.Tipo.Codigo != "CONTADO").ToList();
                    //}
                    //Si estan seleccionadas las fechas
                    //else
                    //{
                    //Cargo la lista Agrupada
                    //View.Model.RecordListAerodromoAgrupada = _db.GetServicios(new Servicios
                    //{
                    //    Operacion = new Operacion
                    //    {
                    //        Llegada = new Llegada { TipoVuelo = new MMaster { Code = tipoOperacion } },
                    //        Aeronave = new Aeronaves { CompañiaFactura = new Terceros { RowID = idCompania } }
                    //    },
                    //    Status = new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "Servicios" } }
                    //}).Where(f => (f.Operacion.Salida.FechaSalida >= View.Model.Factura.FechaInicio && f.Operacion.Salida.FechaSalida <= View.Model.Factura.FechaFinal) && f.Operacion.Tipo.Codigo != "CONTADO" && (f.Tipo.Codigo == "TASAS" || f.Tipo.Codigo == "TASASDEBITO" || f.Tipo.Codigo == "TASASCREDITO")
                    //).ToList();

                    View.Model.RecordListAerodromoAgrupada = _db.Servicios.Where(f => f.Operacion.Aeronave.CompañiaFacturaID == idCompania && f.Operacion.Llegada.Tipo5.Codigo == tipoOperacion && f.Estado.Nombre == "ParaFacturar" && f.Estado.Tipo.Nombre == "Servicios")
                        .Where(f => (f.Operacion.Salida.FechaSalida >= View.Model.Factura.FechaInicio && f.Operacion.Salida.FechaSalida <= View.Model.Factura.FechaFinal) && f.Operacion.Tipo.Codigo != "CONTADO" && (f.Tipo.Codigo == "TASAS" || f.Tipo.Codigo == "TASASDEBITO" || f.Tipo.Codigo == "TASASCREDITO")
               ).ToList();


                    //}
                    //Cargo la lista detallada
                    View.Model.RecordListAerodromo = View.Model.RecordListAerodromoAgrupada.OrderBy(f => f.Operacion.Aeronave.Tercero.RowID).ToList();
                    //Agrupo por Compañia
                    View.Model.RecordListAerodromoAgrupada = View.Model.RecordListAerodromoAgrupada
                                    .GroupBy(l => l.Operacion.Aeronave.CompañiaFacturaID)
                                    .SelectMany(cl => cl.Select(
                                        csLine => new Servicios
                                        {
                                            Operacion = cl.First().Operacion,
                                            Cantidad = cl.Count(),
                                            Valor = cl.Sum(c => c.Valor),
                                            TipoServicioID = cl.First().Tipo.RowID,
                                            Estado = cl.First().Estado,
                                            UsuarioCreacion = cl.First().Operacion.UsuarioCreacion,
                                        })).Distinct().ToList();


                    //Elimino los registros repetidos, agrupando por operacion
                    View.Model.RecordListAerodromoAgrupada = View.Model.RecordListAerodromoAgrupada.GroupBy(a => a.Operacion.RowID).Select(grp => grp.First()).ToList();
                    //Si encontro registros
                    if (View.Model.RecordListAerodromoAgrupada.Count != 0)
                    {
                        View.FechaEmision.SelectedDate = DateTime.Now;
                        View.FechaInicial.SelectedDate = View.Model.Factura.FechaInicio;
                        View.FechaFinal.SelectedDate = View.Model.Factura.FechaFinal;
                        View.TotalAgrupado.Text = View.Model.RecordListAerodromoAgrupada.Sum(s => s.Valor).Value.ToString("N0");
                        View.TotalDetallado.Text = View.TotalAgrupado.Text;
                        //Habilito el boton procesar
                        View.ButtonProcesar.IsEnabled = true;
                        View.Model.RecordFacturas = new Facturas();
                    }
                    else
                    {
                        View.TotalAgrupado.Text = "0";
                        View.TotalDetallado.Text = "0";
                        Util.ShowError("No se encontraron servicios a facturar.");
                    }
                }
            }

            catch (Exception ex)
            {
                Util.ShowError("Error:" + ex.Message);
            }

        }

        public void OnGetListaSinFiltros(object sender, EventArgs e)
        {
            getListaSinFiltros();
        }


        public void getListaSinFiltros()
        {
            //Traigo todos los servicios de la Operacion agrupados por factura
            View.Model.RecordListAerodromoAgrupada = _db.GetServicios(new Servicios
            {
                Status = new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "Servicios" } }
            }).Where(f => (f.Tipo.Codigo == "TASAS" || f.Tipo.Codigo == "TASASDEBITO" || f.Tipo.Codigo == "TASASCREDITO") && (f.Operacion.Tipo.Codigo != "CONTADO")).ToList();

            //Cargo la lista detallada
            View.Model.RecordListAerodromo = View.Model.RecordListAerodromoAgrupada.OrderBy(f => f.Operacion.Aeronave.CompañiaFacturaID).ToList();

            //Agrupo por Compañia
            View.Model.RecordListAerodromoAgrupada = View.Model.RecordListAerodromoAgrupada
                            .GroupBy(l => l.Operacion.Aeronave.CompañiaFacturaID)
                            .SelectMany(cl => cl.Select(
                                csLine => new Servicios
                                {
                                    Operacion = cl.First().Operacion,
                                    Cantidad = cl.Count(),
                                    Valor = cl.Sum(c => c.Valor),
                                    TipoServicioID = cl.First().Tipo.RowID,
                                    Estado = cl.First().Estado,
                                    UsuarioCreacion = cl.First().Operacion.UsuarioCreacion,
                                })).Distinct().ToList();


            //Elimino los registros repetidos, agrupando por operacion
            View.Model.RecordListAerodromoAgrupada = View.Model.RecordListAerodromoAgrupada.GroupBy(a => a.Operacion.RowID).Select(grp => grp.First()).ToList();
            View.TotalAgrupado.Text = View.Model.RecordListAerodromoAgrupada.Sum(s => s.Valor).Value.ToString("N0");
            View.TotalDetallado.Text = View.TotalAgrupado.Text;
        }

        private void OnSearchInvoiceList(object sender, EventArgs e)
        {
            //Evaluo si la fecha inicio fue seleccionada
            if (String.IsNullOrEmpty(View.Model.Factura.FechaInicio.ToString()))
            {
                Util.ShowError("Por favor seleccionar una fecha de inicio para la busqueda de facturas");
                return;
            }

            //Evaluo si la fecha final fue seleccionada
            if (String.IsNullOrEmpty(View.Model.Factura.FechaFinal.ToString()))
            {
                Util.ShowError("Por favor seleccionar una fecha final para la busqueda de facturas");
                return;
            }
        }

        private void OnProcessRecords(object sender, EventArgs e)
        {
            //Evaluo si existen registros para procesar
            if (View.Model.RecordListAerodromo == null || View.Model.RecordListAerodromo.Count == 0)
            {
                Util.ShowError("No hay Registros a procesar");
                return;
            }

            if (View.FechaEmision.SelectedDate == null)
            {
                Util.ShowError("Seleccione Fecha de emision");
                return;
            }
            else if (View.FechaInicial.SelectedDate == null)
            {
                Util.ShowError("Seleccione Fecha inicial");
                return;
            }
            else if (View.FechaFinal.SelectedDate == null)
            {
                Util.ShowError("Seleccione Fecha final");
                return;
            }
            else if (View.FechaInicial.SelectedDate > View.FechaFinal.SelectedDate)
            {
                Util.ShowError("La fecha incial debe ser menor a la Fecha Final");
                return;
            }

            //Muestro la ventana de confirmacion para procesar los registros
            if (!UtilWindow.ConfirmOK("Esta seguro que desea facturar estos registros?") == true)
                return;

            try
            {
                //Agrupo por tipo de servicios
                Facturas facturaServicios;

                //Creo una factura para asignarle a los servicios
                if (View.Model.RecordListAerodromo.Where(f => f.Tipo.Codigo == "TASAS").ToList().Count > 0)
                {
                    facturaServicios = CrearFactura();
                    GenerarFacturacion(View.Model.RecordListAerodromo.ToList(), "TASAS", facturaServicios);
                }

                //Creo una factura para asignarle a los servicios
                if (View.Model.RecordListAerodromo.Where(f => f.Tipo.Codigo == "TASASDEBITO").ToList().Count > 0)
                {
                    facturaServicios = CrearFactura();
                    GenerarFacturacion(View.Model.RecordListAerodromo.ToList(), "TASASDEBITO", facturaServicios);
                }

                //Creo una factura para asignarle a los servicios
                if (View.Model.RecordListAerodromo.Where(f => f.Tipo.Codigo == "TASASCREDITO").ToList().Count > 0)
                {
                    facturaServicios = CrearFactura();
                    GenerarFacturacion(View.Model.RecordListAerodromo.ToList(), "TASASCREDITO", facturaServicios);
                }


                //Muestro la ventana de confirmacion
                Util.ShowMessage("Proceso de facturación realizado exitosamente.");
                getListaFacturasAgrupadas();
                limpiarCampos();

            }
            catch (Exception ex)
            {
                Util.ShowError("Error al procesar los registros, por favor vuelva a intentar.\n" + ex.Message);
            }
        }


        public void GenerarFacturacion(List<Servicios> listaServicios, String codigo, Facturas facturaServicios)
        {
            List<Servicios> ListaServiciosFiltrada = View.Model.RecordListAerodromo.Where(f => f.Tipo.Codigo == codigo).ToList();
            foreach (Servicios servicio in ListaServiciosFiltrada)
            {
                servicio.Facturas = facturaServicios;
                servicio.FechaModificacion = DateTime.Now;
                servicio.UsuarioModificacion = App.curUser.NombreUsuario;
                //Cambio status A ParaFacturar
                //servicio.Estado = _db.GetStatus(new Status { Name = "ParaEnviarERP", StatusType = new StatusType { Name = "Servicios" } }).First();
                servicio.Estado = _db.Estado.FirstOrDefault(f => f.Nombre == "ParaEnviarERP" && f.Tipo.Nombre == "Servicios");
                _db.SaveChanges();
                //_db.UpdateServicios(servicio);
            }
        }

        public Facturas CrearFactura()
        {
            Facturas facturaServicios = new Facturas();
            facturaServicios.FechaEmision = View.FechaEmision.SelectedDate;
            facturaServicios.FechaInicio = View.FechaInicial.SelectedDate.Value;
            facturaServicios.FechaFinal = View.FechaFinal.SelectedDate.Value;
            //facturaServicios.Estado = _db.GetStatus(new Status { Name = "ParaEnviarERP", StatusType = new StatusType { Name = "FacturaServicios" } }).First();
            facturaServicios.Estado = _db.Estado.FirstOrDefault(f => f.Nombre == "ParaEnviarERP" && f.Tipo.Nombre == "FacturaServicios");
            facturaServicios.UsuarioCreacion = App.curUser.NombreUsuario;
            facturaServicios.FechaCreacion = DateTime.Now;
            facturaServicios = _db.SaveFacturas(facturaServicios);
            return facturaServicios;
        }

        public void limpiarCampos()
        {
            View.FechaEmision.Text = null;
            View.FechaInicial.Text = null;
            View.FechaFinal.Text = null;
            //Inhabilito el boton de procesar
            View.ButtonProcesar.IsEnabled = false;
            this.getListaSinFiltros();
        }

        public void getListaFacturasAgrupadas()
        {
            //Traigo todos los servicios de la Operacion agrupados por factura
            //Traigo todos los servicios de la Operacion agrupados por factura
            View.Model.RecordServiciosAgrupadosList = _db.GetServicios(new Servicios
            {
            }).Where(f => (f.Factura != null) && (f.Estado.Name == "ParaEnviarERP" || f.Estado.Name == "EnviadaERP") && (f.Operacion.Tipo.Codigo != "CONTADO") && (f.Tipo.Codigo == "TASAS" || f.Tipo.Codigo == "TASASDEBITO" || f.Tipo.Codigo == "TASASCREDITO") && f.Estado.EstadoType.Name == "Servicios").ToList();

            if (View.Model.RecordServiciosAgrupadosList.Count >= 1)
            {
                View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList
                                .GroupBy(l => l.FacturaID)
                                .SelectMany(cl => cl.Select(
                                    csLine => new Servicios
                                    {
                                        RowID = cl.First().FacturaID,
                                        Cantidad = cl.Count(),
                                        Facturas = cl.First().Facturas,
                                        Operacion = cl.First().Operacion,
                                        TipoServicio = cl.First().Tipo,
                                        Valor = cl.Sum(c => c.Valor),
                                        UsuarioCreacion = cl.First().Facturas.UsuarioCreacion,
                                        Estado = cl.First().Factura.Estado,
                                    })).Distinct().ToList();
                //Elimino los repetidos
                View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList.GroupBy(a => a.RowID).Select(grp => grp.First()).OrderBy(f => f.RowID).ToList();
            }
        }






        public static void ReturnDataTable(string Query, string sWhere, string tableName, SqlConnection connection)
        {
            try
            {

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                DataTable ds = new DataTable(tableName);

                sWhere = string.IsNullOrEmpty(sWhere) ? sWhere : " AND " + sWhere;

                SqlDataAdapter objAdapter = new SqlDataAdapter(Query + sWhere, connection);

                //Console.WriteLine(Query + sWhere);

                objAdapter.Fill(ds);
            }
            catch (Exception ex)
            {
                // ExceptionMngr.WriteEvent("ReturnDataTable: " + Query + sWhere, ListValues.EventType.Error, ex, null, ListValues.ErrorCategory.ErpConnection);
            }
            finally { connection.Close(); }
        }


        void OnEnviarERP(object sender, EventArgs e)
        {
            if (View.listaFacturasSeleccionadas.Items.Count == 0)
            {
                Util.ShowError("No hay Facturas a Procesar");
                return;
            }
            if (View.listaFacturasSeleccionadas.SelectedItems.Count == 0)
            {
                Util.ShowError("Seleccione las facturas a Procesar");
                return;
            }

            IList<Servicios> ListaFacturas = new List<Servicios>();

            foreach (Servicios serv in View.listaFacturasSeleccionadas.SelectedItems)
            {
                if (serv.Estado.Nombre == "ParaEnviarERP")
                {
                    ListaFacturas.Add(serv);
                }
            }
            if (ListaFacturas.Count == 0)
            {
                Util.ShowError("No hay Facturas a Procesar");
                return;
            }


            //Le Cambio el status a las facturas que selecciono a EnviadaERP
            //Status auxStatusFacturas = _db.GetStatus(new Status { Name = "EnviadaERP", StatusType = new StatusType { Name = "FacturaServicios" } }).First();
            Estado auxStatusFacturas = _db.Estado.FirstOrDefault(f => f.Nombre == "EnviadaERP" && f.Tipo.Nombre == "FacturaServicios");
            //Status auxStatusServicios = _db.GetStatus(new Status { Name = "EnviadaERP", StatusType = new StatusType { Name = "Servicios" } }).First();
            Estado auxStatusServicios = _db.Estado.FirstOrDefault(f => f.Nombre == "EnviadaERP" && f.Tipo.Nombre == "Servicios");
            foreach (Servicios servicio in ListaFacturas)
            {
                //Actualizo el status de la factura
                //Facturas auxFactura = _db.GetFacturas(new Facturas { RowID = servicio.FacturaID }).First();
                Facturas auxFactura = _db.Facturas.FirstOrDefault(f => f.RowID == servicio.FacturaID);
                auxFactura.Estado = auxStatusFacturas;
                auxFactura.FechaModificacion = DateTime.Now;
                auxFactura.UsuarioModificacion = App.curUser.NombreUsuario;
                //_db.UpdateFacturas(auxFactura);
                _db.SaveChanges();
                //Actualizo el status de los servicios
                //IList<Servicios> listaServicios = _db.GetServicios(new Servicios { Factura = servicio.Factura }).ToList();
                IList<Servicios> listaServicios = _db.Servicios.Where(f=> f.FacturaID == servicio.FacturaID).ToList();
                foreach (Servicios aux in listaServicios)
                {
                    aux.Estado = auxStatusServicios;
                    aux.FechaModificacion = DateTime.Now;
                    aux.UsuarioModificacion = App.curUser.NombreUsuario;
                    _db.SaveChanges();
                    //_db.UpdaKteServicios(aux);
                }
            }
            //Ejecuto el sp que procesa las facturas con estado EnviadaERP
            try
            {
                ReturnDataTable("exec [ConectorXpress].[dbo].[Ejecutar_Schdule_Facturas_Creditos]", "", "PRESUPUESTO", new SqlConnection(_db.Database.Connection.ConnectionString));
            }
            catch (Exception exp)
            {
                Util.ShowError("Error " + exp.ToString());
                return;
            }

            //Refresco Lista
            getListaFacturasAgrupadas();
            Util.ShowMessage("Facturas enviadas a ERP correctamente");
        }

        void OnConfirmacionEnvioERP(object sender, EventArgs e)
        {

            IList<Servicios> ListaFacturas = new List<Servicios>();
            foreach (Servicios serv in View.listaFacturasSeleccionadas.Items)
            {
                if (serv.Estado.Nombre == "EnviadaERP")
                {
                    ListaFacturas.Add(serv);
                }
            }
            if (ListaFacturas.Count == 0)
            {
                return;
            }



            //Status auxStatusFacturas = _db.GetStatus(new Status { Name = "Facturada", StatusType = new StatusType { Name = "FacturaServicios" } }).First();
            Estado auxStatusFacturas = _db.Estado.FirstOrDefault(f => f.Nombre == "Facturada" && f.Tipo.Nombre == "FacturaServicios");
            //Status auxStatusServicios = _db.GetStatus(new Status { Name = "Facturada", StatusType = new StatusType { Name = "Servicios" } }).First();
            Estado auxStatusServicios = _db.Estado.FirstOrDefault(f => f.Nombre == "Facturada" && f.Tipo.Nombre == "Servicios");
            foreach (Servicios servicio in ListaFacturas)
            {
                //Actualizo el status de la factura
                //Facturas auxFactura = _db.GetFacturas(new Facturas { RowID = servicio.Factura.RowID }).First();
                Facturas auxFactura = _db.Facturas.FirstOrDefault(f => f.RowID == servicio.FacturaID);
                auxFactura.Estado = auxStatusFacturas;
                auxFactura.FechaModificacion = DateTime.Now;
                auxFactura.UsuarioModificacion = App.curUser.UsuarioModificacion;
                //_db.UpdateFacturas(auxFactura);
                _db.SaveChanges();
                //Actualizo el status de los servicios
                //IList<Servicios> listaServicios = _db.GetServicios(new Servicios { Factura = servicio.Factura }).ToList();
                IList<Servicios> listaServicios = _db.Servicios.Where(f=> f.FacturaID == servicio.FacturaID).ToList();
                foreach (Servicios aux in listaServicios)
                {
                    aux.Estado = auxStatusServicios;
                    aux.FechaModificacion = DateTime.Now;
                    aux.UsuarioModificacion = App.curUser.NombreUsuario;
                    //_db.UpdateServicios(aux);
                    _db.SaveChanges();
                }
            }
            //Refresco Lista
            getListaFacturasAgrupadas();
            Util.ShowMessage("OK");
        }


        private void OnCargarDatosFactura(object sender, DataEventArgs<Int32> FacturaID)
        {
            CargarDatosFactura(FacturaID.Value);
        }

        private void OnCargarDatosServicios(object sender, DataEventArgs<String> Parametros)
        {
            if (!String.IsNullOrEmpty(Parametros.Value))
            {
                //Variables Auxiliares
                TabItem NewTabItemFactura;
                IListaServiciosPorAerolineaPresenter ServicioPresenter;
                try
                {
                    String[] Array = Parametros.Value.Split('^');
                    String nombreAerolinea = Array[4].Length >= 21 ? Array[4].Substring(0, 20) : Array[4];
                    // Creo los datos para el nuevo Tab
                    NewTabItemFactura = new TabItem
                    {
                        Header = nombreAerolinea,//+ FacturaID,
                        Name = "Tab",//+ FacturaID,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };

                    //Creo los datos para el UserControl que me controla los TakeOff
                    ServicioPresenter = this.container.Resolve<ListaServiciosPorAerolineaPresenter>();

                    //Inicializo los datos del documento a cargar
                    ServicioPresenter.CargarDocumentoTasas(Parametros.Value, this);

                    //Adiciono al Tab el StackPanel del TakeOff
                    NewTabItemFactura.Content = ServicioPresenter.View;

                    //Adiciono el nuevo Tab a la vista
                    View.TabPadre.Items.Add(NewTabItemFactura);

                    //Selecciono por defecto el nuevo Tab
                    NewTabItemFactura.Focus();
                }
                catch (Exception Ex)
                {
                    Util.ShowError("Error cargando el documento. Error: " + Ex);
                }
            }


        }

        public void CargarDatosFactura(Int32 FacturaID)
        {
            if (FacturaID != null)
            {
                //Variables Auxiliares
                TabItem NewTabItemFactura;
                IListaServiciosPorFacturaPresenter ServicioPresenter;
                try
                {
                    // Creo los datos para el nuevo Tab
                    NewTabItemFactura = new TabItem
                    {
                        Header = "Registro de Factura # " + FacturaID,
                        Name = "Tab_" + FacturaID,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };

                    //Creo los datos para el UserControl que me controla los TakeOff
                    ServicioPresenter = container.Resolve<ListaServiciosPorFacturaPresenter>();

                    //Inicializo los datos del documento a cargar
                    ServicioPresenter.CargarDocumento(FacturaID, this);

                    //Adiciono al Tab el StackPanel del TakeOff
                    NewTabItemFactura.Content = ServicioPresenter.View;

                    //Adiciono el nuevo Tab a la vista
                    View.TabPadreListaFacturas.Items.Add(NewTabItemFactura);

                    //Selecciono por defecto el nuevo Tab
                    NewTabItemFactura.Focus();
                }
                catch (Exception Ex)
                {
                    Util.ShowError("Error cargando el documento. Error: " + Ex);
                }
            }
        }
    }
}