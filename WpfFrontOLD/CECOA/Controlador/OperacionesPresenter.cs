using System;
using WpfFront.Model;
using WpfFront.Modelo;
using WpfFront.Controlador;
using WpfFront.Vista;
using Assergs.Windows;
using WMComposite.Events;
using Microsoft.Practices.Unity;
using WpfFront.Common;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using System.Globalization;


namespace WpfFront.Controlador
{

    public interface IOperacionesPresenter
    {
        IOperacionesView View { get; set; }
        ToolWindow Window { get; set; }

        void CargarDocumento(Operacion Documento, Object Presenter);
    }


    public class OperacionesPresenter : IOperacionesPresenter
    {
        public IOperacionesView View { get; set; }
        private readonly IUnityContainer container;
        private wmsEntities  db;
        //Variables que determinan si se cobra (aplica para calamidad)
        private Boolean cobraAerodromo = true;
        private Boolean cobraParqueo = true;
        private Boolean cobraPuentes = true;
        private Boolean cobraBomberos = true;
        public ToolWindow Window { get; set; }
        //Variable que maneja los datos del Presenter Parent
        public Object PresenterParent { get; set; }
        //Variables Auxiliares 

        public OperacionesPresenter(IUnityContainer container, IOperacionesView view)
        {



            View = view;
            this.container = container;
            this.db = new wmsEntities();
            View.Model = this.container.Resolve<OperacionesModel>();

            #region Metodos

            #region Operacion
            View.ConfirmarRecordLlegada += this.OnConfirmarRecordLlegada;
            View.ConfirmarRecordSalida += this.OnConfirmarRecordSalida;
            View.CerrarTab += this.OnCerrarTab;
            view.CargarDatosDesdeAeronave += new EventHandler<EventArgs>(this.OnCargarDatosDesdeAeronave);
            view.CargarDatosDesdePlaneacionLlegada += new EventHandler<EventArgs>(this.OnCargarDatosDesdePlaneacionLlegada);
            view.CargarDatosDesdePlaneacionSalida += new EventHandler<EventArgs>(this.OnCargarDatosDesdePlaneacionSalida);
            view.CalcularEstado += new EventHandler<EventArgs>(this.OnCalcularEstado);
            view.ValidarRangoHora += new EventHandler<EventArgs>(this.OnValidarRangoHora);
            view.cambiarNoVuelo += new EventHandler<EventArgs>(this.OnCambiarNoVuelo);
            View.ActualizarDatosOperacion += new EventHandler<DataEventArgs<Operacion>>(this.OnActualizarDatosOperacion);



            #endregion

            #region Adicionados
            View.EliminarServicioBomberos += new EventHandler<EventArgs>(this.OnEliminarServicioBomberos);
            View.guardarServicioBomberos += new EventHandler<EventArgs>(this.OnGuardarServicioBomberos);
            #endregion


            #region Liquidacion
            View.GuardarDatosLiquidacion += new EventHandler<EventArgs>(this.onGuardarDatosLiquidacion);
            View.CalcularDatosLiquidacion += new EventHandler<EventArgs>(this.onCalcularDatosLiquidacion);

            #endregion

            #region Facturacion
            View.CalcularFacturacionContado += new EventHandler<EventArgs>(this.OnCalcularFacturacionContado);
            View.CalcularFacturacionContadoConAdicionales += new EventHandler<EventArgs>(this.onCalcularFacturacionContadoConAdicionales);
            View.CalcularTotalFacturacion += new EventHandler<EventArgs>(this.OnCalcularTotalFacturacion);
            view.ObteberListaFacturas += new EventHandler<EventArgs>(this.OnObteberListaFacturas);


            #endregion

            #endregion

            #region Datos

            //Obtengo la conexion
            //Cargo los comboBox
            //view.Model.ListaTipoFacturacion = db.GetTipo(new Tipo { MetaType = new MType { Code = "TIPOFACTURACION" }, Active = true });
            view.Model.ListaTipoFacturacion = db.Tipo.Where(f => f.Agrupacion.Nombre == "TIPOFACTURACION" && f.Activo == true).ToList();
            view.Model.ListaTipoPosicion =db.Tipo.Where(f => f.Agrupacion.Nombre == "TIPOPOSICION"&& f.Activo == true).ToList();
            view.Model.ListaTipoPosicionSalida =db.Tipo.Where(f => f.Agrupacion.Nombre == "TIPOPOSICION"&& f.Activo == true).ToList();
            view.Model.ListaTipoOperacion =db.Tipo.Where(f => f.Agrupacion.Nombre == "TIPOOPERACION" && f.Activo == true).OrderBy(f => f.Orden).ToList();
            view.Model.ListaTipoVuelo =db.Tipo.Where(f => f.Agrupacion.Nombre == "TIPOVUELO" && f.Activo == true).ToList();
            view.Model.ListaTipoDeclaracion =db.Tipo.Where(f => f.Agrupacion.Nombre == "TIPODECLARACION" && f.Activo == true).OrderBy(f => f.Orden).ToList();
            view.Model.ListaBanda =db.Tipo.Where(f => f.Agrupacion.Nombre == "BANDA" && f.Activo == true).ToList();
            view.Model.ListaPosicion =db.Tipo.Where(f => f.Agrupacion.Nombre == "POSICION" && f.Activo == true).OrderBy(f => f.Orden).ToList();
            view.Model.ListaPosicionSalida =db.Tipo.Where(f => f.Agrupacion.Nombre == "POSICION" && f.Activo == true).OrderBy(f => f.Orden).ToList();
            view.Model.ListaTipoServicio =db.Tipo.Where(f => f.Agrupacion.Nombre == "ServicioBomberos" && f.Activo == true).ToList();
            view.Model.ListaTipoTasas =db.Tipo.Where(f => f.Agrupacion.Nombre == "TIPOTASA" && f.Activo == true && f.Codigo == "NORMAL").ToList();
            view.Model.Record = new Operacion();
            //Cargo los datos de la pestaña Liquidacion
            view.Model.RecordTasas = new Tasas();
            view.Model.RecordTasasAdicionales = new Tasas();
            View.Model.RecordBomberos = new Bombero();
            View.Model.RecordBomberosAdicional = new Bombero();
            View.Model.RecordLlegada = new Llegada();
            View.Model.RecordSalida = new Salida();

            #endregion
        }

        public void OnActualizarDatosOperacion(object sender, DataEventArgs<Operacion> Documento2)
        {
            this.CargarDocumento(Documento2.Value, null);
        }

        public void CargarDocumento(Operacion Documento, Object Presenter)
        {
            View.Model.Record = Documento;
            View.Model.RecordSalida = Documento.Salida == null ? new Salida() : Documento.Salida;
            View.Model.RecordLlegada = Documento.Llegada == null ? new Llegada() : Documento.Llegada;

            if (Documento.RowID != 0)
            {
                //Cargo los servicios bomberos que tenga
                try
                {
                    //View.Model.RegistroBomberosList = db.GetBomberos(new Bomberos { Operacion = Documento, Activo = true }).Where(f => f.Status.StatusType.Name == "ServicioBomberos").ToList();
                    View.Model.RegistroBomberosList = db.Bombero.Where( f=> f.OperacionID == Documento.RowID && f.Activo == true && f.Estado.Tipo.Nombre == "ServicioBomberos").ToList();
                }
                catch { }
                //Cargo los servicios bomberos Adicionales que tenga
                try
                { View.Model.RegistroBomberosAdicionalesList = db.Bombero.Where(f => f.OperacionID == Documento.RowID && f.Activo == true && f.Estado.Tipo.Nombre == "ServicioBomberosAdicional").ToList(); }
                catch { }
                //Cargo las Tasas
                try
                { View.Model.RegistroTasasList = db.Tasas.Where(f => f.OperacionID == Documento.RowID).ToList(); }
                catch { }
                //Cargo las Tasas Adicionales
                try
                //{ View.Model.RegistroTasasAdicionalesList = db.GetTasas(new Tasas { Operacion = Documento }).Where(f => f.TipoTasa.Code != "NORMAL").ToList(); }
                { View.Model.RegistroTasasAdicionalesList = db.Tasas.Where(f => f.OperacionID == Documento.RowID && f.Tipo.Codigo != "NORMAL").ToList(); }
                catch { }
                //Cargo los datos de la pestaña liquidacion 
                try
                { View.Model.RecordTasas = db.Tasas.FirstOrDefault(f => f.OperacionID == Documento.RowID); } //Poner estado
                catch { }
                //Cargo la lista de factura Agrupadas
                try
                { getListaFacturasAgrupadas(); }
                catch { }

                //Cargo Adicionales
                try
                { View.Model.RegistroAdicionalesPyPList = db.AdicionalesPyP.Where(f => f.OperacionID == Documento.RowID).ToList(); }
                catch { }
                if (Documento.Llegada != null)
                {
                    //Cargo los comboBox de tipo de posicion y posicion de llegada y salida
                    if (Documento.Llegada.RowID != 0)
                    {
                        //Si tiene llegada habilito el boton de confirmar
                        View.BtnConfirmarLlegada.IsEnabled = true;
                        //Cargo la compania Factura
                        if (Documento.Llegada.Tercero != null)
                        {
                            View.CompañiaFactura.Terceros = Documento.Llegada.Tercero;
                            View.CompañiaFactura.txtDescripcion.Text = Documento.Llegada.Tercero.Nombre + " " + Documento.Llegada.Tercero.Apellidos;
                            View.CompañiaFactura.txtData.Text = Documento.Llegada.Tercero.Nombre + " " + Documento.Llegada.Tercero.Apellidos;
                        }
                        //Cargo los comboBox de tipo vuelo llegada 
                        View.TipoVuelo.SelectedValue = Documento.Llegada.Tipo5 != null ? Documento.Llegada.Tipo5.RowID : -1;
                        View.TipoDeclaracion.SelectedValue = Documento.Llegada.Tipo3 != null ? Documento.Llegada.Tipo3.RowID : -1;
                        View.TipoPosicion.SelectedValue = Documento.Llegada.Tipo4 != null ? Documento.Llegada.Tipo4.RowID : -1;
                        //Cargo NVueloLlegada
                        View.NumVuelo.Text = Documento.Llegada.NVuelo;
                        //Cargo los datos de los controles y comboBox si estan seteados
                        if (Documento.Llegada.OrigenID != 0)
                        {
                            View.Origen.cargarValorEspecifico(Documento.Llegada.Aeropuerto.SiglaIATA+"/"+ Documento.Llegada.Aeropuerto.Ciudad);
                            View.Origen.Aeropuertos = Documento.Llegada.Aeropuerto;
                        }
                        if (Documento.Llegada.Tipo4 != null)
                        {
                            if (Documento.Llegada.Tipo4.Codigo != "HANGAR")
                            {
                                View.Posicion.SelectedValue = Documento.Llegada.Tipo2 != null ? Documento.Llegada.Tipo2.RowID : -1;
                            }
                        }

                    }
                    if (Documento.Llegada.EstadoID != 0)
                    {
                        //Si la Llegada esta confirmada
                        if (Documento.Llegada.Estado.RowID == View.Model.StatusLlegadaSalidaConfirmada.RowID)
                        {
                            View.BtnConfirmarLlegada.IsEnabled = false;
                            View.BtnGuardarLlegada.IsEnabled = false;
                            View.PanelDatosOperacionLlegada.IsEnabled = false;
                            View.PanelDatosCabecera.IsEnabled = false;
                        }
                        //Si esta guardada
                        if (Documento.Llegada.Estado.RowID == View.Model.StatusLlegadaSalidaGuardada.RowID)
                        {
                            View.BtnConfirmarLlegada.IsEnabled = true;
                            View.BtnGuardarLlegada.IsEnabled = true;
                            View.PanelDatosOperacionLlegada.IsEnabled = true;
                            View.PanelDatosCabecera.IsEnabled = true;
                        }
                    }
                }


                if (Documento.Salida != null)
                {
                    if (Documento.Salida.DestinoID != 0)
                    {
                        View.Destino.cargarValorEspecifico(Documento.Salida.Aeropuerto.SiglaIATA+"/"+ Documento.Salida.Aeropuerto.Ciudad);
                        View.Destino.Aeropuertos = Documento.Salida.Aeropuerto;
                    }
                    if (Documento.Salida.RowID != 0)
                    {
                        //Si tiene llegada habilito el boton de confirmar
                        View.BtnConfirmarSalida.IsEnabled = true;
                        if (Documento.Salida.FechaSalida != null && Documento.Salida.Tipo2 != null)
                        {
                            this.OnCalcularValoresLiquidacion();
                        }
                        if (Documento.Salida.TipoPosicionSalidaID != 0)
                        {
                            if (Documento.Salida.Tipo1.Codigo != "HANGAR")
                            {
                                View.PosicionSalida.SelectedValue = Documento.Salida.Tipo != null ? Documento.Salida.Tipo.RowID : -1;
                            }
                        }
                        View.TipoPosicionSalida.SelectedValue = Documento.Salida.Tipo1 != null ? Documento.Salida.Tipo1.RowID : -1;
                        View.TipoVueloSalida.SelectedValue = Documento.Salida.Tipo2 != null ? Documento.Salida.Tipo2.RowID : -1;

                        if (Documento.Salida.EstadoID != 0)
                        {
                            //Si la Salida esta confirmada
                            if (Documento.Salida.Estado.RowID == View.Model.StatusLlegadaSalidaConfirmada.RowID)
                            {
                                View.BtnConfirmarSalida.IsEnabled = false;
                                View.BtnGuardarSalida.IsEnabled = false;
                                View.PanelDatosOperacionSalida1.IsEnabled = false;
                                View.PanelDatosOperacionSalida2.IsEnabled = false;
                                View.PanelDatosOperacionSalida3Contado.IsEnabled = false;
                            }
                            //Si esta guardada
                            if (Documento.Salida.Estado.RowID == View.Model.StatusLlegadaSalidaGuardada.RowID)
                            {
                                View.BtnConfirmarSalida.IsEnabled = true;
                                View.BtnGuardarSalida.IsEnabled = true;
                                View.PanelDatosOperacionSalida1.IsEnabled = true;
                                View.PanelDatosOperacionSalida2.IsEnabled = true;
                                View.PanelDatosOperacionSalida3Contado.IsEnabled = true;
                                View.PanelDatosOperacionSalida3Contado.IsEnabled = false;
                            }
                        }
                    }

                }
                if (Documento.Aeronave != null)
                {
                    View.Aeronave.cargarValorEspecifico(Documento.Aeronave.Matricula, Documento.Aeronave.PBMOKG + "Kg - " + Documento.Aeronave.TipoAeronave + " - " + Documento.Aeronave.CapacidadPasajeros + "Pax");
                    View.Aeronave.Aeronaves = Documento.Aeronave;
                }
                if (Documento.Estado.RowID != 0)
                {
                    //Si la Operacion esta Liquidada
                    if (Documento.Estado.RowID == View.Model.StatusOperacionLiquidada.RowID)
                    {
                        try
                        {
                            OnCalcularValoresLiquidacion();
                        }
                        catch (Exception)
                        {
                        }
                        View.BotonFacturar.IsEnabled = true;
                    }
                }
                if (Documento.Facturado == true)
                {
                    View.BotonImprimirFactura.IsEnabled = true;
                    View.BotonFacturar.IsEnabled = false;
                    View.PanelDatosOperacionLlegada.IsEnabled = false;
                    View.PanelDatosOperacionSalida1.IsEnabled = false;
                    View.PanelDatosOperacionSalida2.IsEnabled = false;
                    View.PanelDatosOperacionSalida3Contado.IsEnabled = true;
                    if (Documento.Salida.TipoPosicionSalidaID != 0)
                    {
                        if (Documento.Salida.Tipo1.Codigo != "PUENTE")
                        {
                            View.PanelDatosOperacionSalidaPuenteContado.IsEnabled = false;
                        }
                    }
                    View.PanelDatosLiquidacionTasas.IsEnabled = false;
                    View.PanelDatosBomberos.IsEnabled = false;
                    View.PanelDatosCabecera.IsEnabled = false;
                    View.Btn_CerrarOperacion.IsEnabled = false;
                    this.cargarInfoDeFacturacion(Documento);

                    if (Documento.Tipo.Codigo == "CONTADO")
                    {
                        View.BtnGuardarSalida.IsEnabled = true;
                    }
                    else
                    {
                        //Si el rol es admin, master o JefeCecoa, le habilito el boton de abrir Operacion
                       // foreach (Usuario rol in App.curUser.Rol)
                       // {
                            if (App.curUser.Rol.Nombre == "Administrador" || App.curUser.Rol.Nombre == "MasterCecoa" || App.curUser.Rol.Nombre == "JefeCecoa")
                            {
                                View.BtnAbrirOperacion.IsEnabled = true;
                            }
                        //}
                    }
                }
                //Le asigno permisos al Boton de Anular Facturas
               // foreach (UserByRol rol in App.curUser.UserRols)
               // {
                    if (App.curUser.Rol.Nombre == "Administrador" || App.curUser.Rol.Nombre == "MasterCecoa" || App.curUser.Rol.Nombre == "JefeCecoa")
                    {
                        View.Btn_AnularFactura.Visibility = Visibility.Visible;
                        View.BtnAbrirOperacion.IsEnabled = true;
                    }
                    else
                    {
                        View.Btn_AnularFactura.Visibility = Visibility.Collapsed;
                        View.BtnAbrirOperacion.IsEnabled = false;
                    }
                //}


            }
            if (Presenter != null)
            {
                //Asigno el PresenterParente
                PresenterParent = Presenter;
            }
        }


        #region Adicionados

        #region servicioBomberos


        public void OnEliminarServicioBomberos(object sender, EventArgs e)
        {
            //Valido que exista el vuelo
            if (View.Model.Record.RowID == 0)
            {
                return;
            }
            foreach (Bombero ServicioBombero in View.ListaBomberos.SelectedItems)
            {
                if (ServicioBombero.Estado.Nombre == "Nuevo")
                {
                    db.Bombero.Remove(ServicioBombero);
                    db.SaveChanges();
                }
                else
                {
                    Util.ShowError("No puede eliminar este Servicio.");
                }
            }
            //View.Model.RegistroBomberosList = db.GetBomberos(new Bomberos { Operacion = View.Model.Record, Activo = true }).Where(f => f.Status.StatusType.Name == "ServicioBomberos").ToList();
            View.Model.RegistroBomberosList = db.Bombero.Where( f=>f.OperacionID == View.Model.Record.RowID && f.Activo == true && f.Estado.Tipo.Nombre == "ServicioBomberos").ToList();
        }

        public void OnGuardarServicioBomberos(object sender, EventArgs e)
        {
            //Valido que exista el vuelo
            if (View.Model.Record.RowID == 0)
            {
                Util.ShowError("Debe crear un vuelo");
                return;
            }
            //Si es registro nuevo
            //Valido que seleccione Tipo de servicio y Fecha
            if (View.ListaTipoServicio.SelectedIndex == -1)
            {
                Util.ShowError("Debe seleccionar un tipo de servicio");
                View.ListaTipoServicio.Focus();
                return;
            }
            if (View.fechaServicio.SelectedDate == null)
            {
                Util.ShowError("Debe seleccionar una fecha");
                View.fechaServicio.Focus();
                return;
            }
            if (View.FechaOperacion.SelectedDate != null)
            {
                if (View.fechaServicio.SelectedDate < View.FechaOperacion.SelectedDate)
                {
                    Util.ShowError("Fecha Servicio Bomberos no valida");
                    View.fechaServicio.Focus();
                    return;
                }
            }

            //Le asigno el vuelo actual
            View.Model.RecordBomberos.Operacion = View.Model.Record;
            View.Model.RecordBomberos.Activo = true;
            View.Model.RecordBomberos.Fecha = View.fechaServicio.SelectedDate;
            View.Model.RecordBomberos.TipoServicioBombID = ((Tipo)View.ListaTipoServicio.SelectedItem).RowID;
            View.Model.RecordBomberos.EstadoID = View.Model.StatusBomberosNuevo.RowID;
            string mensajeError = this.ObtenerValorServicio();
            if (!string.IsNullOrEmpty(mensajeError))
            {
                Util.ShowError(mensajeError);
                return;
            }

            //Asigno variables de Creacion
            View.Model.RecordBomberos.UsuarioCreacion = App.curUser.NombreUsuario;
            View.Model.RecordBomberos.FechaCreacion = DateTime.Now;
            //Guardo el registro
            db.Bombero.Add(View.Model.RecordBomberos);
            db.SaveChanges();
            Util.ShowMessage("Se registro exitosamente el Servicio Bomberos");
            View.ListaTipoServicio.Focus();
            //Actualizar Lista bomberos
            //View.Model.RegistroBomberosList = db.GetBomberos(new Bomberos { Operacion = View.Model.Record, Activo = true }).Where(f => f.Status.StatusType.Name == "ServicioBomberos").ToList();
            View.Model.RegistroBomberosList = db.Bombero.Where(f => f.OperacionID == View.Model.Record.RowID && f.Activo == true && f.Estado.Tipo.Nombre == "ServicioBomberos").ToList();
            this.CleanToCreateBomberos();

        }

        /// <summary>
        /// //Valido si para el dia seleccionado existe una tarifa para el servicio bomberos, y si es nacional, y si tiene permiso de explotacion 
        /// </summary>
        public string ObtenerValorServicio()
        {
            string mensajeError = "";
            // MetaMasterID 2135 = tarifas BOMBEROS
            // MetaMasterID 15 = vuelo INTERNACIONAL
            TarifaCecoa TarifaBomberos;
            // Verifico si existe una tarifa BOMBERO para la fecha seleccionada
            //if (db.GetTarifas(new Tarifas { FechaFiltro = View.fechaServicio.SelectedDate, TipoTarifa = new MMaster { Code = "BOMBEROS" }, TipoServicio = (MMaster)View.ListaTipoServicio.SelectedItem }).Count == 1)
            if (db.TarifaCecoa.Where(f=> f.Tipo1.Codigo == "BOMBEROS" && f.TipoServicioID == ((Tipo)View.ListaTipoServicio.SelectedItem).RowID  && (View.fechaServicio.SelectedDate.Value >= f.FechaInicial.Value && View.fechaServicio.SelectedDate.Value <= f.FechaFinal.Value  )).Count() == 1)
            {
                //TarifaBomberos = db.GetTarifas(new Tarifas { FechaFiltro = View.fechaServicio.SelectedDate, TipoTarifa = new MMaster { Code = "BOMBEROS" }, TipoServicio = (MMaster)View.ListaTipoServicio.SelectedItem }).First();
                TarifaBomberos = db.TarifaCecoa.FirstOrDefault( f=> f.Tipo1.Codigo == "BOMBEROS" );
                //Asigno el Valor del servicio
                View.Model.RecordBomberos.ValorServicio = TarifaBomberos.ValorCOP;
            }
            else
            {
                mensajeError = "No cuenta con una tarifa Bomberos disponible";
            }

            return mensajeError;
        }

        public void CleanToCreateBomberos()
        {
            View.fechaServicio.SelectedDate = View.Model.Record.FechaOP;
            View.ListaTipoServicio.SelectedIndex = -1;
            View.Model.RecordBomberos = new Bombero();
        }


        #endregion

        #endregion

        #region Operaciones


        /// <summary>
        /// Cuando el tipo de operacion es General o militar se tomara por defecto la placa de la aeronave como no. de vuelo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCambiarNoVuelo(object sender, EventArgs e)
        {
            this.AsignarNroVuelo();

        }



        /// <summary>
        /// Valido que la hora ingresada este en el rango de 00 a 24 y los minutos de 00 a 60
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnValidarRangoHora(object sender, EventArgs e)
        {
            if (!(View.horaAValidar.Contains("_")))
            {
                string[] arrayHora = View.horaAValidar.Split(':');

                if ((Int32.Parse(arrayHora[0]) < 0 || Int32.Parse(arrayHora[0]) >= 24) || (Int32.Parse(arrayHora[1]) < 0 || Int32.Parse(arrayHora[1]) >= 60))
                {
                    Util.ShowError("Hora no valida");
                }
            }
        }

        /// <summary>
        /// cargo los datos para Hora Programada y fecha Operacion que se traen del searchPlaneacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCargarDatosDesdePlaneacionLlegada(object sender, EventArgs e)
        {
            if ((Tipo)View.TipoFactura.SelectedItem != null)
            {
                //Si el vuelo es de tipo credito
                if (((Tipo)View.TipoFactura.SelectedItem).Codigo == "CREDITO" && View.FechaOperacion.SelectedDate != null)
                {
                    //if (db.GetPlaneacion(new Planeacion { NVueloEntrada = View.NumVuelo.Text, Fecha = View.FechaOperacion.SelectedDate }).Count() >= 1)
                    if (db.Planeacion.Where(f=> f.NVueloEntrada == View.NumVuelo.Text && f.Fecha.Value == View.FechaOperacion.SelectedDate.Value).Count() >= 1)
                    {
                        //Planeacion aux = db.GetPlaneacion(new Planeacion { NVueloEntrada = View.NumVuelo.Text, Fecha = View.FechaOperacion.SelectedDate }).First();
                        Planeacion aux = db.Planeacion.FirstOrDefault(f=> f.NVueloEntrada == View.NumVuelo.Text && f.Fecha.Value == View.FechaOperacion.SelectedDate.Value);
                        //View.FechaOperacion.SelectedDate = aux.Fecha != null ? aux.Fecha : null;
                        View.HoraProgramada.Text = !string.IsNullOrEmpty(aux.HoraEntrada) ? aux.HoraEntrada : "";
                        if (aux.OrigenID != 0)
                        {
                            View.Origen.Aeropuertos = aux.Aeropuerto1;
                            View.Origen.cargarValorEspecifico(aux.Aeropuerto1.SiglaIATA+"/"+ aux.Aeropuerto1.Ciudad);
                        }

                        if (aux.BandaID != 0)
                        {
                            View.Banda.SelectedValue = aux.Tipo.RowID;
                        }

                        if (!string.IsNullOrEmpty(aux.NVueloEntrada))
                        {
                            View.NumVuelo.Text = aux.NVueloEntrada.ToString();
                        }
                        //Si No se ha creado una salida, cargo la salida
                        if (View.Model.RecordSalida.RowID == 0)
                        {
                            View.FechaSalida.SelectedDate = aux.Fecha != null ? aux.Fecha : null;
                            if (!string.IsNullOrEmpty(aux.NVueloSalida))
                            {
                                View.NumVueloSalida.Text = aux.NVueloSalida.ToString();
                            }
                            if (aux.DestinoID != 0)
                            {
                                View.Destino.Aeropuertos = aux.Aeropuerto;
                                View.Destino.cargarValorEspecifico(aux.Aeropuerto.SiglaIATA + "/" + aux.Aeropuerto.Ciudad);
                            }
                            View.HoraProgramadaSalida.Text = !string.IsNullOrEmpty(aux.HoraSalida) ? aux.HoraSalida : "";
                            View.SalaSalida.Text = !string.IsNullOrEmpty(aux.Sala + "") ? aux.Sala + "" : "";
                        }
                    }
                }
            }
        }

        public void OnCargarDatosDesdePlaneacionSalida(object sender, EventArgs e)
        {
            if ((Tipo)View.TipoFactura.SelectedItem != null)
            {
                //Si el vuelo es de tipo credito
                if (((Tipo)View.TipoFactura.SelectedItem).Codigo == "CREDITO" && View.FechaSalida.SelectedDate != null)
                {
                    //Busco Planeacion con el Nro Vuelo de salida y la fecha de la Salida
                    //if (db.GetPlaneacion(new Planeacion { NVueloSalida = View.NumVueloSalida.Text, Fecha = View.FechaSalida.SelectedDate }).Count() >= 1)
                    if (db.Planeacion.Where(f=> f.NVueloSalida == View.NumVueloSalida.Text && f.Fecha.Value == View.FechaSalida.SelectedDate.Value).Count() >= 1)
                        {
                           // Planeacion aux = db.GetPlaneacion(new Planeacion { NVueloSalida = View.NumVueloSalida.Text, Fecha = View.FechaSalida.SelectedDate }).First();
                        Planeacion aux = db.Planeacion.FirstOrDefault(f=> f.NVueloSalida == View.NumVueloSalida.Text && f.Fecha.Value == View.FechaSalida.SelectedDate.Value);
                        //Cargo la hora programada de salida
                        View.HoraProgramadaSalida.Text = !string.IsNullOrEmpty(aux.HoraSalida) ? aux.HoraSalida : "";
                        //Cargo el destino
                        if (aux.DestinoID != 0)
                        {
                            View.Destino.Aeropuertos = aux.Aeropuerto;
                            View.Destino.cargarValorEspecifico(aux.Aeropuerto.SiglaIATA + "/" + aux.Aeropuerto.Ciudad);
                        }
                        View.SalaSalida.Text = String.IsNullOrEmpty(aux.Sala.ToString()) ? "0" : aux.Sala.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// cargo los datos para compañia factura y compañia explotadora que se traen del searchAeronaves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCargarDatosDesdeAeronave(object sender, EventArgs e)
        {
            if (View.SearchAeronavesCbx.AeronaveSeleccionada != null)
            {
                View.CompañiaFactura.Terceros = View.SearchAeronavesCbx.AeronaveSeleccionada.Tercero;
                View.CIAExplotadora.Text = View.SearchAeronavesCbx.AeronaveSeleccionada.Tercero1.Nombre;
                View.ClienteBomberos.Text = View.SearchAeronavesCbx.AeronaveSeleccionada.Tercero.Nombre;
                View.NIT.Text = View.SearchAeronavesCbx.AeronaveSeleccionada.Tercero.Identificacion.ToString();
                View.TipoOperacion.Text = View.SearchAeronavesCbx.AeronaveSeleccionada.Tipo.Nombre;
            }

        }

        /// <summary>
        /// Si el tipo de operacion es General o militar, asigno el Nro de vuelo como la matricula
        /// </summary>
        public void AsignarNroVuelo()
        {
            //Si ya selecciono una aeronave
            if (View.SearchAeronavesCbx.AeronaveSeleccionada != null)
            {
                if (View.TipoOperacion.SelectedItem != null)
                {
                    // Si selecciona militar o general
                    if (((Tipo)View.TipoOperacion.SelectedItem).Codigo.StartsWith("MILITAR") || ((Tipo)View.TipoOperacion.SelectedItem).Codigo.StartsWith("GENERAL"))
                    {
                        View.NumVuelo.Text = View.NumVueloSalida.Text = View.SearchAeronavesCbx.AeronaveSeleccionada.Matricula;
                    }
                    else
                    {
                        if (View.NumVuelo.Text == View.SearchAeronavesCbx.AeronaveSeleccionada.Matricula)
                        {
                            View.NumVuelo.Text = View.NumVueloSalida.Text = "";
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(View.SearchAeronavesCbx.txtDescripcion.Text))
            {
                // Si selecciona militar o general
                if (((Tipo)View.TipoOperacion.SelectedItem).Codigo.StartsWith("MILITAR") || ((Tipo)View.TipoOperacion.SelectedItem).Codigo.StartsWith("GENERAL"))
                {
                    View.NumVuelo.Text = View.NumVueloSalida.Text = View.SearchAeronavesCbx.Aeronaves.Matricula;
                }
                else
                {
                    if (View.NumVuelo.Text == View.SearchAeronavesCbx.txtDescripcion.Text)
                    {
                        View.NumVuelo.Text = View.NumVueloSalida.Text = "";
                    }
                    else //Si no es ninguna opcion entonces le asigno el numero de vuelo que guardo
                    {
                        View.NumVuelo.Text = View.Model.Record.Llegada.NVuelo;
                        View.NumVueloSalida.Text = View.Model.Record.Salida != null ? View.Model.Record.Salida.NVueloSalida : "";
                    }
                }
            }

        }



        public void OnCalcularEstado(object sender, EventArgs e)
        {
            this.onCalcularEstado();
        }


        public string CalcularEstadoLlegadaYSalida(String HoraAterrizaje, DateTime? FechaAterrizaje, String HoraProgramacion, DateTime? FechaProgramacion)
        {
            String Estado = "";
            if (!HoraAterrizaje.Contains("_") && FechaAterrizaje != null && !HoraProgramacion.Contains("_") && FechaProgramacion != null)
            {
                //Si la fecha de Programacion y de aterrizaje son las mismas (Es el mismo dia)
                if (FechaProgramacion == FechaAterrizaje)
                {
                    TimeSpan TNow = DateTime.Parse(HoraProgramacion) - DateTime.Parse(HoraAterrizaje);
                    if (TNow.TotalMinutes < -15)
                    { Estado = "D"; }
                    else if (TNow.TotalMinutes > 15)
                    { Estado = "A"; }
                    else
                    { Estado = "AT"; }
                }
                //Si la fecha de aterrizaje es mayor a la de operacion esta Demorado
                else if (FechaAterrizaje > FechaProgramacion)
                {
                    if (Int32.Parse(HoraProgramacion.Substring(0, 2)) == 23 && Int32.Parse(HoraAterrizaje.Substring(0, 2)) == 0)
                    {
                        Double minutos;
                        TimeSpan TNow2 = DateTime.Parse(HoraProgramacion) - DateTime.Parse(HoraAterrizaje);
                        minutos = TNow2.TotalMinutes;
                        minutos = minutos - 1440;
                        if (minutos < -15)
                        { Estado = "D"; }
                        else if (minutos > 15)
                        { Estado = "A"; }
                        else
                        { Estado = "AT"; }
                    }
                    else
                    { Estado = "D"; }
                }

                //Si la fecha de aterrizaje es menor a la de operacion esta adelantado
                else
                {
                    if (Int32.Parse(HoraProgramacion.Substring(0, 2)) == 0 && Int32.Parse(HoraAterrizaje.Substring(0, 2)) == 23)
                    {
                        Double minutos;
                        TimeSpan TNow2 = DateTime.Parse(HoraProgramacion) - DateTime.Parse(HoraAterrizaje);
                        minutos = TNow2.TotalMinutes;
                        minutos = minutos + 1440;
                        if (minutos < -15)
                        { Estado = "D"; }
                        else if (minutos > 15)
                        { Estado = "A"; }
                        else
                        { Estado = "AT"; }
                    }
                    else
                    { Estado = "A"; }
                }
            }

            return Estado;
        }

        public void onCalcularEstado()
        {

            View.EstadoVuelo.Text = CalcularEstadoLlegadaYSalida(View.HoraAterrizaje.Text, View.FechaAterrizaje.SelectedDate, View.HoraProgramada.Text, View.FechaProgramacionLlegada.SelectedDate);
            View.EstadoVueloSalida.Text = CalcularEstadoLlegadaYSalida(View.HoraDespegue.Text, View.FechaDespegue.SelectedDate, View.HoraProgramadaSalida.Text, View.FechaProgramacionSalida.SelectedDate);
        }
        //Al confirmar Llegada
        public void OnConfirmarRecordLlegada(object sender, EventArgs e)
        {
            //Valido que esten seteados los datos necesarios
            String mensaje = this.ValidarDatosVueloEntrada();
            if (mensaje != "")
            {
                Util.ShowError(mensaje);
                View.NumVuelo.Focus();
                return;
            }
            //Calculo el estado de los vuelos si estan seteadas las horas
            this.onCalcularEstado();
            //Status Confirmada Para Llegada
            View.Model.RecordLlegada.EstadoID = View.Model.StatusLlegadaSalidaConfirmada.RowID;
            //Datos de creacion para la llegada
            View.Model.RecordLlegada.FechaModificacion = DateTime.Now;
            View.Model.RecordLlegada.UsuarioModificacion = App.curUser.NombreUsuario;
            //db.UpdateLlegada(View.Model.RecordLlegada);
            db.SaveChanges();
            //Asigno Llegada a la Operacion
            View.Model.Record.Llegada = View.Model.RecordLlegada;
            View.Model.Record.FechaModificacion = DateTime.Now;
            View.Model.Record.UsuarioModificacion = App.curUser.NombreUsuario;
            //db.UpdateOperacion(View.Model.Record);
            db.SaveChanges();
            this.ConfirmarOperacion();
            View.BtnGuardarLlegada.IsEnabled = false;
            View.BtnConfirmarLlegada.IsEnabled = false;
            View.PanelDatosOperacionLlegada.IsEnabled = false;
            View.PanelDatosCabecera.IsEnabled = false;
            Util.ShowMessage("Se Confirmaron exitosamente los datos de llegada");
            View.NumVueloSalida.Focus();
        }
        //Al Confirmar Salida
        public void OnConfirmarRecordSalida(object sender, EventArgs e)
        {
            if (View.Model.Record.RowID != 0)
            {
                //Actualizar
                if (View.Model.RecordSalida.RowID != 0)
                {
                    //Status Guardada Para Salida
                    View.Model.RecordSalida.EstadoID = View.Model.StatusLlegadaSalidaConfirmada.RowID;
                    //Datos Actualizacion Salida
                    View.Model.RecordSalida.FechaModificacion = DateTime.Now;
                    View.Model.RecordSalida.UsuarioModificacion = App.curUser.NombreUsuario;
                    //Actualizo la Salida
                    //db.UpdateSalida(View.Model.RecordSalida);
                    db.SaveChanges();
                    View.Model.Record.Salida = View.Model.RecordSalida;

                    //Confirmo Operacion
                    this.ConfirmarOperacion();
                    //Actualizo el vuelo
                    View.BtnConfirmarSalida.IsEnabled = false;
                    View.BtnGuardarSalida.IsEnabled = false;
                    View.PanelDatosOperacionSalida1.IsEnabled = false;
                    View.PanelDatosOperacionSalida2.IsEnabled = false;
                    View.PanelDatosOperacionSalida3Contado.IsEnabled = false;

                    Util.ShowMessage("Se Confirmaron exitosamente los datos de salida");
                    View.ListaTipoServicio.Focus();
                }
                // this.asignarReglasPorCalamidadOExencion();
            }
            else
            {
                Util.ShowError("Debe crear una Llegada para guardar una Salida");
            }
            ////Si es nuevo registro -- Nunca Va a serlo

        }

        //Confirmo Operacion si ya esta confirmada la llegada y la salida
        public void ConfirmarOperacion()
        {
            if (View.Model.Record.Llegada == null || View.Model.Record.Salida == null)
            {
                return;
            }
            //Si la llegada y salida esta confirmada, confirmo el vuelo
            if (View.Model.Record.Llegada.Estado.Nombre == View.Model.StatusLlegadaSalidaConfirmada.Nombre && View.Model.Record.Salida.Estado.Nombre == View.Model.StatusLlegadaSalidaConfirmada.Nombre)
            {
                View.Model.Record.Estado = View.Model.StatusOperacionConfirmada;
                //Datos de modificacion Operacion
                View.Model.Record.FechaModificacion = DateTime.Now;
                View.Model.Record.UsuarioModificacion = App.curUser.NombreUsuario;
                //db.UpdateOperacion(View.Model.Record);
                db.SaveChanges();
                //IList<UserByRol> roles = App.curUser.UserRols;
                //foreach (UserByRol rol in roles)
                //{
                //    if (rol.Rol.Name == "Administrador" || rol.Rol.Name == "MasterCecoa" || rol.Rol.Name == "JefeCecoa")
                //    {
                //        View.BtnAbrirOperacion.IsEnabled = true;
                //    }
                //}
            }
        }


        public void cargarDatosParaGuardarLlegada()
        {
            //Cargo los comboBox
            View.Model.RecordLlegada.ClasificacionID = ((Tipo)View.TipoOperacion.SelectedItem).RowID;
            View.Model.RecordLlegada.TipoVueloID = ((Tipo)View.TipoVuelo.SelectedItem).RowID;
            View.Model.RecordLlegada.TipoDeclaracionID = ((Tipo)View.TipoDeclaracion.SelectedItem).RowID;
            View.Model.RecordLlegada.BandaID = ((Tipo)View.Banda.SelectedItem).RowID;
            View.Model.RecordLlegada.TipoPosicionID = ((Tipo)View.TipoPosicion.SelectedItem).RowID;
            View.Model.RecordLlegada.PosicionID = ((Tipo)View.Posicion.SelectedItem).RowID;
            View.Model.RecordLlegada.OrigenID = View.Origen.Aeropuertos.RowID;
            View.Model.RecordLlegada.CIAFacturaID = View.CompañiaFactura.Terceros.RowID;
            View.Model.RecordLlegada.HoraProgramadaLlegada = View.HoraProgramada.Text;
            View.Model.RecordLlegada.FechaAterrizaje = View.FechaAterrizaje.SelectedDate;
            View.Model.RecordLlegada.FechaLLegadaPlataforma = View.FechaLlegadaPlataforma.SelectedDate;
            View.Model.RecordLlegada.FechaLlegadaPuente = View.FechaLlegadaPuente.SelectedDate;
            View.Model.RecordLlegada.NVuelo = View.NumVuelo.Text;
        }

        public void cargarDatosParaGuardarSalida()
        {
            View.Model.RecordSalida.TipoVueloSalidaID = ((Tipo)View.TipoVueloSalida.SelectedItem).RowID;
            View.Model.RecordSalida.DestinoID = View.Destino.Aeropuertos.RowID;
            View.Model.RecordSalida.TipoPosicionSalidaID = ((Tipo)View.TipoPosicionSalida.SelectedItem).RowID;
            View.Model.RecordSalida.PosicionSalidaID = ((Tipo)View.PosicionSalida.SelectedItem).RowID;
            View.Model.RecordSalida.NVueloSalida = View.NumVueloSalida.Text;
            View.Model.RecordSalida.HoraProgramadaSalida = View.HoraProgramadaSalida.Text;
            View.Model.RecordSalida.FechaDespegue = View.FechaDespegue.SelectedDate;
            View.Model.RecordSalida.FechaSalidaPlataforma = View.FechaSalidaPlataforma.SelectedDate;
            View.Model.RecordSalida.FechaSalidaPuente = View.FechaSalidaPuente.SelectedDate;
        }


        public String ValidarDatosVueloEntrada()
        {
            String mensaje = "";
            if (View.TipoFactura.SelectedIndex == -1)
            {
                mensaje = "Debe seleccionar un tipo de factura";
            }
            else if (View.Aeronave.Aeronaves == null)
            {
                mensaje = "Debe seleccionar una aeronave";
            }
            else if (View.FechaOperacion.SelectedDate == null)
            {
                mensaje = "Debe seleccionar una Fecha de operacion";
            }
            else if (string.IsNullOrEmpty(View.NumVuelo.Text))
            {
                mensaje = "Debe ingresar un numero de vuelo.";
            }
            else if (View.HoraAterrizaje.Text == "__:__")
            {
                mensaje = "Debe ingresar una hora de Aterrizaje";
            }
            else if (View.HoraPlataforma.Text == "__:__")
            {
                mensaje = "Debe ingresar una hora de Llegada Plataforma";
            }

            else if (View.TipoVuelo.SelectedIndex == -1)
            {
                mensaje = "Debe Seleccionar el Tipo de vuelo";
            }
            else if (View.TipoDeclaracion.SelectedIndex == -1)
            {
                mensaje = "Debe Seleccionar la Declaracion del vuelo";
            }
            else if (View.TipoPosicion.SelectedIndex == -1)
            {
                mensaje = "Debe Seleccionar el Tipo de Posicion";
            }
            else if (View.Posicion.SelectedIndex == -1 && ((Tipo)(View.TipoPosicion.SelectedItem)).Codigo != "HANGAR")
            {
                mensaje = "Debe Seleccionar la Posicion de Llegada";
            }
            else if (View.HoraPuente.Text == "__:__" && ((Tipo)(View.TipoPosicion.SelectedItem)).Codigo == "PUENTE")
            {
                mensaje = "Debe ingresar una hora de llegada a Puente";
            }
            else if (View.Banda.SelectedIndex == -1)
            {
                mensaje = "Debe Seleccionar la Banda";
            }
            else if (View.TipoOperacion.SelectedIndex == -1)
            {
                mensaje = "Debe Seleccionar Clasificacion de la Operacion";
            }
            else if (View.CompañiaFactura.Terceros == null)
            {
                mensaje = "Debe Seleccionar una Compañia Factura.";
            }
            else if (View.Origen.Aeropuertos == null)
            {
                mensaje = "Debe Seleccionar un Origen.";
            }
            else if (string.IsNullOrEmpty(View.CargaEntrada.Text))
            {
                mensaje = "Debe digitar una cantidad en Carga.";
            }
            else if (string.IsNullOrEmpty(View.CorreoEntrada.Text))
            {
                mensaje = "Debe digitar una cantidad en Correo.";
            }
            else if (string.IsNullOrEmpty(View.Observacion.Text))
            {
                mensaje = "Debe ingresar una observacion.";
            }
            else if (((Tipo)View.TipoFactura.SelectedItem).Codigo != "CONTADO")
            {
                mensaje = validacionesCamposCredito();
            }

            return mensaje;
        }

        ///Solo para la salida
        public String validacionesCamposContado()
        {
            String mensaje = "";
            if (View.FechaOperacion.SelectedDate == null)
            {
                mensaje = "Debe ingreasar una hora de salida aproximada.";
            }
            else if (View.FechaOperacion.SelectedDate == null)
            {
                mensaje = "Debe ingresar una hora de salida a plataforma aproximada.";
            }
            else if (View.FechaOperacion.SelectedDate == null)
            {
                mensaje = "Debe ingresar una hora de despegue aproximada.";
            }
            return mensaje;
        }

        public String validacionesCamposCredito()
        {
            String mensaje = "";
            View.Sala.Text = string.IsNullOrEmpty(View.Sala.Text) ? 0 + "" : View.Sala.Text;
            if (View.HoraProgramada.Text.Contains("_"))
            {
                mensaje = "Debe ingresar una hora de programacion";
            }

            return mensaje;

        }

        public void OnCerrarTab(object sender, EventArgs e)
        {
            //Cierro el Tab seleccionado actualmente
            ((ListaOperacionesPresenter)PresenterParent).View.TabPadre.Items.Remove(((TabItem)((ListaOperacionesPresenter)PresenterParent).View.TabPadre.SelectedItem));

            //Selecciono por defecto el Tab con el listado de salidas de almacen
            ((TabItem)((ListaOperacionesPresenter)PresenterParent).View.TabPadre.Items[0]).Focus();
            //Actualizo la lista de vuelos
            //((ListaOperacionesPresenter)PresenterParent).View.Model.ListaOperaciones = db.GetOperacion(new Operacion { }).Take(200).ToList();
            ((ListaOperacionesPresenter)PresenterParent).View.Model.ListaOperaciones = db.Operacion.Take(200).ToList();
        }

        public void asignarReglasPorCalamidadOExencion()
        {
            if (View.TipoOperacion.SelectedItem != null && View.TipoDeclaracion.SelectedItem != null)
            {
                // Hago las restricciones correspondientes cuando selecciona clasificaciones exentas
                if (((Tipo)View.TipoOperacion.SelectedItem).Codigo.StartsWith("ESTADO(EXENTA)") || ((Tipo)View.TipoOperacion.SelectedItem).Codigo.StartsWith("MILITAR(EXENTA)"))
                {
                    cobraAerodromo = false;
                    cobraBomberos = false;
                    cobraParqueo = false;
                    cobraPuentes = false;
                }
                else if (((Tipo)View.TipoDeclaracion.SelectedItem).Codigo != "N/A")
                {
                    cobraAerodromo = false;
                    cobraPuentes = false;
                    cobraBomberos = true;
                    cobraParqueo = true;
                }
                else
                {
                    cobraAerodromo = true;
                    cobraBomberos = true;
                    cobraParqueo = true;
                    cobraPuentes = true;
                }
            }
        }
        #endregion

        #region Liquidacion

        public void onCalcularDatosLiquidacion(object sender, EventArgs e)
        {
            this.OnCalcularValoresLiquidacion();
        }

        /// <summary>
        /// Calculo los datos finales de liquidacion
        /// </summary>
        public void OnCalcularValoresLiquidacion()
        {
            if (View.Model.RecordSalida.RowID != 0)
            {
                //Para calcular info de pestaña liquidacion
                TarifaCecoa Tarifa;
                if (View.Model.RecordSalida.FechaSalida == null || View.Model.RecordSalida.Tipo2 == null)
                {
                    return;
                }
                //Buscar tarifa aerodromo que coincida con  la fecha Salida y que sea de tipo TASAS
                //if (db.GetTarifas(new Tarifas { FechaFiltro = View.Model.RecordSalida.FechaSalida, TipoTarifa = new Tipo { Code = "TASAS" } }).Count() != 0)
                if (db.TarifaCecoa.Where(f=> f.TipoTarifaID == db.Tipo.FirstOrDefault(t=> t.Codigo == "TASAS").RowID  && (View.Model.RecordSalida.FechaSalida.Value >= f.FechaInicial.Value && View.Model.RecordSalida.FechaSalida.Value <= f.FechaFinal.Value)).Count() != 0)
                {
                    //Tarifa = db.GetTarifas(new Tarifas { FechaFiltro = DateTime.Parse(View.Model.RecordSalida.FechaSalida.ToString()), TipoTarifa = new Tipo { Code = "TASAS" } }).First();
                    Tarifa = db.TarifaCecoa.FirstOrDefault(f => f.TipoTarifaID == db.Tipo.FirstOrDefault(t => t.Codigo == "TASAS").RowID && (View.Model.RecordSalida.FechaSalida.Value >= f.FechaInicial.Value && View.Model.RecordSalida.FechaSalida.Value <= f.FechaFinal.Value));
                    //Tarifa = db.TarifaCecoa.FirstOrDefault( f= > );
                    if (View.Model.RecordSalida.Tipo2.Codigo == "NACIONAL")
                    {
                        //*****ARREGLAR*********/View.Model.RecordTasas.TasaCOP = Tarifa.ValorCOP;
                    }
                    else
                    {
                        //*****ARREGLAR*********/View.Model.RecordTasas.TasaUSD = Tarifa.ValorUSD;
                        //*****ARREGLAR*********/View.Model.RecordTasas.TasaCOP = Tarifa.ValorUSD;
                    }
                }
                else
                //Si no tiene una tarifa de Aerodromo le dejo 0 por defecto
                {
                    Util.ShowError("No cuenta con una tarifa Tasas disponible");
                    //*****ARREGLAR*********/View.Model.RecordTasas.TasaCOP = 0;
                    //*****ARREGLAR*********/View.Model.RecordTasas.TasaUSD = 0;
                }
                //Sumo los exentos de todos los registros de tasas diferentes a CREDITO
                //View.Model.RecordTasas.Exentos = db.GetTasas(new Tasas { Operacion = View.Model.Record })
                //                                    .Where(f => f.TipoTasa.Codigo != "CREDITO")
                //                                    .Sum(f => f.Transitos + f.Tripulantes + f.Infantes + f.Otros + f.Militares);

                //*****ARREGLAR*********/View.Model.RecordTasas.Exentos = db.Tasas.Where( f=> f.Operacion.RowID == (View.Model.Record as Operacion).RowID)
                //*****ARREGLAR*********/.Where(f => f.Tipo.Codigo != "CREDITO")
                //*****ARREGLAR*********/.Sum(f => f.Transitos + f.Tripulantes + f.Infantes + f.Otros + f.Militares);

                //Resto los CREDITO
                //View.Model.RecordTasas.Exentos = View.Model.RecordTasas.Exentos - db.GetTasas(new Tasas { Operacion = View.Model.Record })
                //                                    .Where(f => f.TipoTasa.Codigo == "CREDITO")
                //                                    .Sum(f => f.Transitos + f.Tripulantes + f.Infantes + f.Otros + f.Militares);

                //*****ARREGLAR*********/View.Model.RecordTasas.Exentos = View.Model.RecordTasas.Exentos - db.Tasas.Where(f => f.Operacion.RowID == (View.Model.Record as Operacion).RowID)
                //*****ARREGLAR*********/.Where(f => f.Tipo.Codigo == "CREDITO")
                //*****ARREGLAR*********/.Sum(f => f.Transitos + f.Tripulantes + f.Infantes + f.Otros + f.Militares);



                //Sumo todos los registros de pagan tasa, que son diferentes a CREDITO
                //View.Model.RecordTasas.PaganTasa = db.GetTasas(new Tasas { Operacion = View.Model.Record })
                //                                    .Where(f => f.TipoTasa.Codigo != "CREDITO").Sum(f => f.PaganTasa);

                //*****ARREGLAR*********/View.Model.RecordTasas.PaganTasa = db.Tasas.Where(f => f.Operacion.RowID == (View.Model.Record as Operacion).RowID)
                //*****ARREGLAR*********/.Where(f => f.Tipo.Codigo != "CREDITO").Sum(f => f.PaganTasa);



                //Resto los CREDITO
                //View.Model.RecordTasas.PaganTasa = View.Model.RecordTasas.PaganTasa - db.GetTasas(new Tasas { Operacion = View.Model.Record })
                //                                    .Where(f => f.TipoTasa.Codigo == "CREDITO").Sum(f => f.PaganTasa);

                View.Model.RecordTasas.PaganTasa = View.Model.RecordTasas.PaganTasa - db.Tasas.Where(f => f.Operacion.RowID == (View.Model.Record as Operacion).RowID)
                                                   .Where(f => f.Tipo.Codigo == "CREDITO").Sum(f => f.PaganTasa);

                //*****ARREGLAR*********/View.Model.RecordTasas.PasajerosEmbarcados =
                //*****ARREGLAR*********/View.Model.RecordTasas.PaganTasa +
                //*****ARREGLAR*********/View.Model.RecordTasas.Exentos;

                if (View.Model.RecordSalida.Tipo2.Codigo == "NACIONAL")
                {
                    //*****ARREGLAR*********/View.Model.RecordTasas.TotalCOP =
                    //*****ARREGLAR*********/View.Model.RecordTasas.PaganTasa *
                    //*****ARREGLAR*********/View.Model.RecordTasas.TasaCOP;
                }
                else
                {
                    TRM Trm;
                   // if (db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).Count == 1)
                     if (db.TRM.Where(f=>  View.Model.Record.Salida.FechaSalida.Value >= f.FechaInicial.Value && View.Model.Record.Salida.FechaSalida.Value <= f.FechaFinal.Value).Count() == 1)
                    {
                        //Asigno la TRM vigente
                        //Trm = db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).First();
                        Trm = db.TRM.FirstOrDefault(f=> View.Model.Record.Salida.FechaSalida >= f.FechaInicial.Value  && View.Model.Record.Salida.FechaSalida <= f.FechaFinal.Value);

                        //*****ARREGLAR*********/View.Model.RecordTasas.TotalUSD =
                        //*****ARREGLAR*********/View.Model.RecordTasas.PaganTasa * Trm.Valor *
                        //*****ARREGLAR*********/View.Model.RecordTasas.TasaUSD;
                        //Para que se vea en pantalla
                        //*****ARREGLAR*********/View.Model.RecordTasas.TotalCOP = View.Model.RecordTasas.TotalUSD;

                    }
                    else
                    { Util.ShowError("No cuenta con una TRM disponible."); }
                }
            }
            else
            {
                Util.ShowError("No ha registrado un tipo de vuelo de salida");
            }
        }




        public void onGuardarDatosLiquidacion(object sender, EventArgs e)
        {
            //Asigno Vuelo
            View.Model.RecordTasas.Operacion = View.Model.Record;
            //Traigo el peso de la aeronave, por si se actualiza el peso de la aeronave despues de que se cargo el vuelo
            if (View.Model.Record.Aeronave != null)
            {
                //Suma de pasajeros totales sin infantes
                int suma = View.Model.RecordTasas.PaganTasa.Value + View.Model.RecordTasas.Tripulantes.Value + View.Model.RecordTasas.Militares.Value + View.Model.RecordTasas.Transitos.Value + View.Model.RecordTasas.Otros.Value;
                //Aeronave aeronaveActualizada = db.GetAeronaves(new Aeronaves { RowID = View.Model.Record.Aeronave.RowID }).First();
                Aeronave aeronaveActualizada = db.Aeronave.FirstOrDefault( f=> f.RowID == View.Model.Record.Aeronave.RowID);
                if (suma > aeronaveActualizada.CapacidadPasajeros)
                {
                    Util.ShowError("La cantidad de pasajeros supera la capacidad de la aeronave");
                    return;
                }
            }
            //Si es actualizar
            if (View.Model.RecordTasas.RowID != 0)
            {

                //Asigno las variables de modificacion
                View.Model.RecordTasas.UsuarioModificacion = App.curUser.NombreUsuario;
                View.Model.RecordTasas.FechaModificacion = DateTime.Now;
                View.Model.RecordTasas.Estado = View.Model.StatusTasasNueva;
                //Actualizo
                //db.UpdateTasas(View.Model.RecordTasas);
                db.SaveChanges();
                //View.Model.RegistroTasasList = db.GetTasas(new Tasas { Operacion = View.Model.Record });
                View.Model.RegistroTasasList = db.Tasas.Where(f=> f.OperacionID == ((Operacion)View.Model.Record).RowID).ToList();
                Util.ShowMessage("Datos Tasas Actualizados");
            }
            //Si es crear
            else
            {
                //Le asigno tipo
                //View.Model.RecordTasas.TipoTasa = db.GetTipo(new Tipo { Code = "NORMAL" }).First();
                View.Model.RecordTasas.Tipo = db.Tipo.FirstOrDefault(f => f.Codigo == "NORMAL");
                View.Model.RecordTasas.Fecha = DateTime.Now;
                //Asigno variables de Creacion
                View.Model.RecordTasas.UsuarioCreacion = App.curUser.NombreUsuario;
                View.Model.RecordTasas.FechaCreacion = DateTime.Now;
                View.Model.RecordTasas.Estado = View.Model.StatusTasasNueva;
                //Guardar
                //View.Model.RecordTasas = db.SaveTasas(View.Model.RecordTasas);
                db.Tasas.Add(View.Model.RecordTasas);
                db.SaveChanges();
                //View.Model.RegistroTasasList = db.GetTasas(new Tasas { Operacion = View.Model.Record });
                View.Model.RegistroTasasList = db.Tasas.Where(f => f.OperacionID == ((Operacion)View.Model.Record).RowID).ToList();
                Util.ShowMessage("Datos Tasas Guardados");
            }

            if (View.Model.Record.Salida != null)
            {
                //Calculos los datos que se mostraran el panel A cobrar
                this.OnCalcularValoresLiquidacion();
            }

        }

        #endregion

        #region Facturacion


        public void OnObteberListaFacturas(object sender, EventArgs e)
        {
            this.getListaFacturasAgrupadas();
        }
        public void getListaFacturasAgrupadas()
        {
            //Traigo todos los servicios de la Operacion agrupados por factura
            //View.Model.RecordServiciosAgrupadosList = db.GetServicios(new Servicios { Operacion = View.Model.Record }).Where(f => f.Factura != null).ToList();
            View.Model.RecordServiciosAgrupadosList = db.Servicios.Where( f => f.FacturaID != null && f.OperacionID == ((Operacion)View.Model.Record).RowID).ToList();
            if (View.Model.RecordServiciosAgrupadosList.Count >= 1)
            {
                View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList
                                .GroupBy(l => l.Facturas.RowID)
                                .SelectMany(cl => cl.Select(
                                    csLine => new Servicios
                                    {
                                        RowID = cl.First().Facturas.RowID,
                                        Facturas = cl.First().Facturas,
                                        Operacion = cl.First().Operacion,
                                        Cantidad = cl.Count(),
                                        Fecha = cl.First().Fecha,
                                        Valor = cl.Sum(c => c.Valor),
                                        UsuarioCreacion = cl.First().Facturas.UsuarioCreacion,
                                        Estado = cl.First().Facturas.Estado,
                                    })).Distinct().ToList();
                //Elimino los repetidos
                View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList.GroupBy(a => a.RowID).Select(grp => grp.First()).ToList();
            }
        }

        public void onCalcularFacturacionContadoConAdicionales(object sender, EventArgs e)
        {
            //Calculo Todo para el primer Adicional
            DateTime? fechaLlegada = null, fechaSalida = null;
            string horaLLegada = null, horaSalida = null;
            //Asigno reglas de exencion por calamidad
            this.asignarReglasPorCalamidadOExencion();
            //Status de Confirmado
            //Traigo el primer Adicional y calculo con ese
            //AdicionalesPyP primerAdicional = db.GetAdicionalesPyP(new AdicionalesPyP { Operacion = View.Model.Record, Status = View.Model.StatusAdicionalesConfirmado }).OrderBy(f => f.RowID).First();
            AdicionalesPyP primerAdicional = db.AdicionalesPyP.Where( f=> f.OperacionID == ((Operacion)View.Model.Record).RowID && f.EstadoID == ((Estado)View.Model.StatusAdicionalesConfirmado).RowID ).OrderBy(f => f.RowID).First();

            fechaLlegada = View.Model.RecordLlegada.FechaLLegadaPlataforma;
            fechaSalida = primerAdicional.FechaInicial;
            horaLLegada = View.Model.RecordLlegada.HoraPlataforma;
            horaSalida = primerAdicional.HoraLlegada;

            TarifaCecoa Tarifa;

            // Verifico si existe una tarifa AERODROMO para la fecha de operacion del vuelo
            //if (db.GetTarifas(new Tarifas { FechaFiltro = View.Model.Record.FechaOP, TipoTarifa = new Tipo { Code = "AERODROMO" } }).Count == 1)
            if (db.TarifaCecoa.Where(f=> f.Tipo1.Codigo == "AERODROMO" && (View.Model.Record.FechaOP >= f.FechaInicial.Value && View.Model.Record.FechaOP <= f.FechaFinal.Value)).Count() == 1)
            {
                // Tarifa = db.GetTarifas(new Tarifas { FechaFiltro = View.Model.Record.FechaOP, TipoTarifa = new Tipo { Code = "AERODROMO" } }).First();
                Tarifa = db.TarifaCecoa.Where(f=> f.Tipo1.Codigo == "AERODROMO" && (View.Model.Record.FechaOP >= f.FechaInicial.Value && View.Model.Record.FechaOP <= f.FechaFinal.Value)).First();
            }
            else
            {
                Util.ShowError("No cuenta con una tarifa de Aerodromo disponible");
                return;
            }
            //Si es Hangar, Hangar. no cobra parqueo
            //Tipo4 TipoPosicionLlegada
            if (View.Model.RecordLlegada.Tipo4.Codigo == "HANGAR" && primerAdicional.Tipo4.Codigo == "HANGAR")
            {
                this.cobraParqueo = false;
            }
            //Si llega en hangar no le cobro parqueo
            else if (View.Model.RecordLlegada.Tipo4.Codigo == "HANGAR")
            {
                this.cobraParqueo = false;
            }
            //Traigo el peso de la aeronave, por si se actualiza el peso de la aeronave despues de que se cargo el vuelo
            //Aeronave aeronaveActualizada = db.GetAeronaves(new Aeronaves { RowID = View.Model.Record.Aeronave.RowID }).First();
            Aeronave aeronaveActualizada = db.Aeronave.FirstOrDefault(f => f.RowID == View.Model.Record.Aeronave.RowID);

            //Verifico si el vuelo es nacional
            if (View.Model.RecordLlegada.Tipo5 != null)
            {
                if (View.Model.RecordLlegada.Tipo5.Codigo == "NACIONAL")
                {
                    if (aeronaveActualizada.FechaVencimientoMatricula < View.Model.Record.FechaOP && aeronaveActualizada.PermisoExplotacion == true)
                    {
                        Util.ShowError("Matricula vencida, Se liquidara SIN permiso de explotacion");
                        aeronaveActualizada.PermisoExplotacion = false;
                    }
                    //Valido si tiene matricula extranjera
                    if (aeronaveActualizada.Extranjera == true && aeronaveActualizada.PermisoExplotacion == false)
                    {
                        //Si no tiene permiso de explotacion le cobro internacional
                        //Si le envio TRM significa que es internacional
                        //this.calcularTarifasAerodromo(new TRM(), aeronaveActualizada.PBMOKG, Tarifa.ValorUSD, Tarifa.RecargoNocturnoUSD);
                        //Calculo el total para aerodromo
                        decimal ValorAerodromo = calcularTarifasAerodromo(new TRM(), aeronaveActualizada.PBMOKG.Value, Tarifa.ValorUSD.Value);
                        View.TotalAerodromo.Text = (Decimal.Round(ValorAerodromo)).ToString("N0");
                        //Calculo el recargo Nocturno
                        Decimal valorRecargoNoc = CalcularRecargoNocturno(Tarifa.RecargoNocturnoUSD.Value, ValorAerodromo, horaLLegada, horaSalida);
                        View.RecargoNocturno.Text = (Decimal.Round(valorRecargoNoc)).ToString("N0");
                        String posLlegada = "", posSalida = "";
                        //Valido si selecciono puente en la llegada y salida
                        if (View.Model.Record.Llegada.Tipo4.Codigo == "PUENTE")
                        {
                            posLlegada = View.Model.Record.Llegada.Tipo2.Codigo;
                        }
                        if (primerAdicional.Tipo4.Codigo == "PUENTE")
                        {
                            posSalida = primerAdicional.Tipo1.Codigo;
                        }
                        //Envio TRM porque es InterNacional
                        //Calculo el valor de los puentes
                        Decimal[] ValorPuentes = this.calcularPuentes(new TRM(), View.Model.Record.Salida.FechaSalida, View.Model.Record.Llegada.FechaLlegadaPuente, primerAdicional.FechaInicial, View.Model.Record.Llegada.Tipo4.Codigo, primerAdicional.Tipo4.Codigo, posLlegada, posSalida);
                        View.NumPuentes.Text = ValorPuentes[0] + "";
                        View.TotalPuente.Text = (ValorPuentes[1]).ToString("N0");

                        //Calculo el valor del parqueo
                        this.calcularParqueo(new TRM(), fechaLlegada, fechaSalida, horaLLegada, horaSalida);
                    }
                    else
                    //Sino tiene matricula extranjera y tiene permiso de explotacion le cobro nacional
                    {
                        //this.calcularTarifasAerodromo(null, aeronaveActualizada.PBMOKG, Tarifa.ValorCOP, Tarifa.RecargoNocturnoCOP);
                        //Calculo el total para aerodromo
                        decimal ValorAerodromo = calcularTarifasAerodromo(null, aeronaveActualizada.PBMOKG.Value, Tarifa.ValorCOP.Value);
                        View.TotalAerodromo.Text = (Decimal.Round(ValorAerodromo)).ToString("N0");
                        //Calculo el recargo Nocturno
                        Decimal valorRecargoNoc = CalcularRecargoNocturno(Tarifa.RecargoNocturnoCOP.Value, ValorAerodromo, horaLLegada, horaSalida);
                        View.RecargoNocturno.Text = (Decimal.Round(valorRecargoNoc)).ToString("N0");

                        String posLlegada = "", posSalida = "";
                        //Valido si selecciono puente en la llegada y salida
                        if (View.Model.Record.Llegada.Tipo4.Codigo == "PUENTE")
                        {
                            posLlegada = View.Model.Record.Llegada.Tipo2.Codigo;
                        }
                        if (primerAdicional.Tipo4.Codigo == "PUENTE")
                        {
                            posSalida = primerAdicional.Tipo1.Codigo;
                        }
                        //Envio Null porque es Nacional
                        //Calculo el valor de los puentes
                        Decimal[] ValorPuentes = this.calcularPuentes(null, View.Model.Record.Salida.FechaSalida, View.Model.Record.Llegada.FechaLlegadaPuente, primerAdicional.FechaInicial, View.Model.Record.Llegada.Tipo4.Codigo, primerAdicional.Tipo4.Codigo, posLlegada, posSalida);
                        View.NumPuentes.Text = ValorPuentes[0] + "";
                        View.TotalPuente.Text = (ValorPuentes[1]).ToString("N0");
                        //Envio Null porque es Nacional
                        this.calcularParqueo(null, fechaLlegada, fechaSalida, horaLLegada, horaSalida);
                    }
                }
                //Verifico si el vuelo es internacional
                else if (View.Model.RecordLlegada.Tipo5.Codigo == "INTERNACIONAL")
                {
                    //Si le envio TRM significa que es internacional
                    //this.calcularTarifasAerodromo(new TRM(), aeronaveActualizada.PBMOKG, Tarifa.ValorUSD, Tarifa.RecargoNocturnoUSD);

                    //Calculo el total para aerodromo
                    decimal ValorAerodromo = calcularTarifasAerodromo(new TRM(), aeronaveActualizada.PBMOKG.Value, Tarifa.ValorUSD.Value);
                    View.TotalAerodromo.Text = (Decimal.Round(ValorAerodromo)).ToString("N0");
                    //Calculo el recargo Nocturno
                    Decimal valorRecargoNoc = CalcularRecargoNocturno(Tarifa.RecargoNocturnoUSD.Value, ValorAerodromo, View.Model.RecordLlegada.HoraAterrizaje, View.Model.RecordSalida.HoraDespegue);
                    View.RecargoNocturno.Text = (Decimal.Round(valorRecargoNoc)).ToString("N0");
                    /////////////////////
                    String posLlegada = "", posSalida = "";
                    //Valido si selecciono puente en la llegada y salida
                    if (View.Model.Record.Llegada.Tipo4.Codigo == "PUENTE")
                    {
                        posLlegada = View.Model.Record.Llegada.Tipo2.Codigo;
                    }
                    if (View.Model.Record.Salida.Tipo1.Codigo == "PUENTE")
                    {
                        posSalida = View.Model.Record.Salida.Tipo.Codigo;
                    }
                    //Envio TRM porque es INTERNacional
                    //Calculo el valor de los puentes
                    Decimal[] ValorPuentes = this.calcularPuentes(new TRM(), View.Model.Record.Salida.FechaSalida, View.Model.Record.Llegada.FechaLlegadaPuente, primerAdicional.FechaInicial, View.Model.Record.Llegada.Tipo4.Codigo, primerAdicional.Tipo4.Codigo, posLlegada, posSalida);
                    View.NumPuentes.Text = ValorPuentes[0] + "";
                    View.TotalPuente.Text = (ValorPuentes[1]).ToString("N0");
                    ///////////////////////////
                    this.calcularParqueo(new TRM(), fechaLlegada, fechaSalida, horaLLegada, horaSalida);
                }
            }


            this.calcularServicioBomberos();
            this.calcularTasasFacturacion();
            this.calcularTotalFacturacion();
        }

        public void OnCalcularFacturacionContado(object sender, EventArgs e)
        {
            DateTime? fechaLlegada = null, fechaSalida = null;
            string horaLLegada = null, horaSalida = null;
            //Asigno reglas de exencion por calamidad
            this.asignarReglasPorCalamidadOExencion();
            if (this.cobraParqueo == true)
            {
                if (View.Model.RecordSalida.FechaSalidaPlataforma == null ||
                    View.Model.RecordLlegada.FechaLLegadaPlataforma == null ||
                    View.Model.RecordLlegada.HoraPlataforma.Contains("_") ||
                    View.Model.RecordSalida.HoraSalidaPlataforma.Contains("_"))
                {
                    Util.ShowError("No hay datos suficientes para calcular parqueo.");
                    View.ValorParqueo.Text = 0 + "";
                    View.CantHorasParqueo.Text = 0 + "";
                    this.cobraParqueo = false;
                }
                else
                {
                    fechaLlegada = View.Model.RecordLlegada.FechaLLegadaPlataforma;
                    fechaSalida = View.Model.RecordSalida.FechaSalidaPlataforma;
                    horaLLegada = View.Model.RecordLlegada.HoraPlataforma;
                    horaSalida = View.Model.RecordSalida.HoraSalidaPlataforma;
                }
            }


            TarifaCecoa Tarifa;

            // Verifico si existe una tarifa AERODROMO para la fecha de operacion del vuelo
            //if (db.GetTarifas(new Tarifas { FechaFiltro = View.Model.Record.FechaOP, TipoTarifa = new Tipo { Code = "AERODROMO" } }).Count == 1)
            if (db.TarifaCecoa.Where(f=> f.Tipo1.Codigo == "AERODROMO" &&  (View.Model.Record.FechaOP >= f.FechaInicial.Value && View.Model.Record.FechaOP <= f.FechaFinal.Value)).Count() == 1)
            {
                //Tarifa = db.GetTarifas(new Tarifas { FechaFiltro = View.Model.Record.FechaOP, TipoTarifa = new Tipo { Code = "AERODROMO" } }).First();
                Tarifa = db.TarifaCecoa.FirstOrDefault(f => f.Tipo1.Codigo == "AERODROMO" && (View.Model.Record.FechaOP >= f.FechaInicial.Value && View.Model.Record.FechaOP <= f.FechaFinal.Value));
            }
            else
            {
                Util.ShowError("No cuenta con una tarifa de Aerodromo disponible");
                return;
            }
            //Si es Hangar, Hangar. no cobra parqueo
            if (View.Model.RecordLlegada.Tipo4.Codigo == "HANGAR" && View.Model.RecordSalida.Tipo1.Codigo == "HANGAR")
            {
                this.cobraParqueo = false;
            }
            //Si llega en hangar no le cobro parqueo
            else if (View.Model.RecordLlegada.Tipo4.Codigo == "HANGAR")
            {
                this.cobraParqueo = false;
            }
            //Traigo el peso de la aeronave, por si se actualiza el peso de la aeronave despues de que se cargo el vuelo
            //Aeronave aeronaveActualizada = db.GetAeronaves(new Aeronaves { RowID = View.Model.Record.Aeronave.RowID }).First();
            Aeronave aeronaveActualizada = db.Aeronave.FirstOrDefault(f => f.RowID == View.Model.Record.Aeronave.RowID);

            //Verifico si el vuelo es nacional
            if (View.Model.RecordLlegada.Tipo5 != null && View.Model.RecordLlegada.Tipo5.Codigo == "NACIONAL")
            {
                if (aeronaveActualizada.FechaVencimientoMatricula < View.Model.Record.FechaOP && aeronaveActualizada.PermisoExplotacion == true)
                {
                    Util.ShowMessage("Matricula vencida, Se liquidara SIN permiso de explotacion");
                    aeronaveActualizada.PermisoExplotacion = false;
                }
                //Valido si tiene matricula extranjera
                if (aeronaveActualizada.Extranjera == true && aeronaveActualizada.PermisoExplotacion == false)
                {

                    //Si no tiene permiso de explotacion le cobro internacional
                    //Si le envio TRM significa que es internacional
                    //this.calcularTarifasAerodromo(new TRM(), aeronaveActualizada.PBMOKG, Tarifa.ValorUSD, Tarifa.RecargoNocturnoUSD);

                    //Calculo el total para aerodromo
                    decimal ValorAerodromo = calcularTarifasAerodromo(new TRM(), aeronaveActualizada.PBMOKG.Value, Tarifa.ValorUSD.Value);
                    View.TotalAerodromo.Text = (Decimal.Round(ValorAerodromo)).ToString("N0");
                    //Calculo el recargo Nocturno
                    Decimal valorRecargoNoc = CalcularRecargoNocturno(Tarifa.RecargoNocturnoUSD.Value, ValorAerodromo, View.Model.RecordLlegada.HoraAterrizaje, View.Model.RecordSalida.HoraDespegue);
                    View.RecargoNocturno.Text = (Decimal.Round(valorRecargoNoc)).ToString("N0");
                    /////////////////////
                    String posLlegada = "", posSalida = "";
                    //Valido si selecciono puente en la llegada y salida
                    if (View.Model.Record.Llegada.Tipo4.Codigo == "PUENTE")
                    {
                        posLlegada = View.Model.Record.Llegada.Tipo2.Codigo;
                    }
                    if (View.Model.Record.Salida.Tipo1.Codigo == "PUENTE")
                    {
                        posSalida = View.Model.Record.Salida.Tipo.Codigo;
                    }
                    //Envio TRM porque es INTERNacional
                    //Calculo el valor de los puentes
                    Decimal[] ValorPuentes = this.calcularPuentes(new TRM(), View.Model.Record.Salida.FechaSalida, View.Model.Record.Llegada.FechaLlegadaPuente, View.Model.Record.Salida.FechaSalidaPuente, View.Model.Record.Llegada.Tipo4.Codigo, View.Model.Record.Salida.Tipo1.Codigo, posLlegada, posSalida);
                    View.NumPuentes.Text = ValorPuentes[0] + "";
                    View.TotalPuente.Text = (ValorPuentes[1]).ToString("N0");
                    ///////////////////////////
                    this.calcularParqueo(new TRM(), fechaLlegada, fechaSalida, horaLLegada, horaSalida);
                }
                else
                //Sino tiene matricula extranjera y tiene permiso de explotacion le cobro nacional
                {
                    //this.calcularTarifasAerodromo(null, aeronaveActualizada.PBMOKG, Tarifa.ValorCOP, Tarifa.RecargoNocturnoCOP);
                    //Calculo el total para aerodromo
                    decimal ValorAerodromo = calcularTarifasAerodromo(null, aeronaveActualizada.PBMOKG.Value, Tarifa.ValorCOP.Value);
                    View.TotalAerodromo.Text = (Decimal.Round(ValorAerodromo)).ToString("N0");
                    //Calculo el recargo Nocturno
                    Decimal valorRecargoNoc = CalcularRecargoNocturno(Tarifa.RecargoNocturnoCOP.Value, ValorAerodromo, horaLLegada, horaSalida);
                    View.RecargoNocturno.Text = (Decimal.Round(valorRecargoNoc)).ToString("N0");
                    /////////////////////
                    String posLlegada = "", posSalida = "";
                    //Valido si selecciono puente en la llegada y salida
                    if (View.Model.Record.Llegada.Tipo4.Codigo == "PUENTE")
                    {
                        posLlegada = View.Model.Record.Llegada.Tipo2.Codigo;
                    }
                    if (View.Model.Record.Salida.Tipo1.Codigo == "PUENTE")
                    {
                        posSalida = View.Model.Record.Salida.Tipo.Codigo;
                    }
                    //Envio Null porque es Nacional
                    //Calculo el valor de los puentes
                    Decimal[] ValorPuentes = this.calcularPuentes(null, View.Model.Record.Salida.FechaSalida, View.Model.Record.Llegada.FechaLlegadaPuente, View.Model.Record.Salida.FechaSalidaPuente, View.Model.Record.Llegada.Tipo4.Codigo, View.Model.Record.Salida.Tipo1.Codigo, posLlegada, posSalida);
                    View.NumPuentes.Text = ValorPuentes[0] + "";
                    View.TotalPuente.Text = (ValorPuentes[1]).ToString("N0");
                    ///////////////////////////
                    //Envio Null porque es Nacional
                    this.calcularParqueo(null, fechaLlegada, fechaSalida, horaLLegada, horaSalida);
                }
            }
            //Verifico si el vuelo es internacional
            else if (View.Model.RecordLlegada.Tipo5 != null && View.Model.RecordLlegada.Tipo5.Codigo == "INTERNACIONAL")
            {
                //Si le envio TRM significa que es internacional
                //this.calcularTarifasAerodromo(new TRM(), aeronaveActualizada.PBMOKG, Tarifa.ValorUSD, Tarifa.RecargoNocturnoUSD);
                //Calculo el total para aerodromo
                decimal ValorAerodromo = calcularTarifasAerodromo(new TRM(), aeronaveActualizada.PBMOKG.Value, Tarifa.ValorUSD.Value);
                View.TotalAerodromo.Text = (Decimal.Round(ValorAerodromo)).ToString("N0");
                //Calculo el recargo Nocturno
                Decimal valorRecargoNoc = CalcularRecargoNocturno(Tarifa.RecargoNocturnoUSD.Value, ValorAerodromo, View.Model.RecordLlegada.HoraAterrizaje, View.Model.RecordSalida.HoraDespegue);
                View.RecargoNocturno.Text = (Decimal.Round(valorRecargoNoc)).ToString("N0");
                /////////////////////
                String posLlegada = "", posSalida = "";
                //Valido si selecciono puente en la llegada y salida
                if (View.Model.Record.Llegada.Tipo4.Codigo == "PUENTE")
                {
                    posLlegada = View.Model.Record.Llegada.Tipo2.Codigo;
                }
                if (View.Model.Record.Salida.Tipo1.Codigo == "PUENTE")
                {
                    posSalida = View.Model.Record.Salida.Tipo.Codigo;
                }
                //Envio TRM porque es INTERNacional
                //Calculo el valor de los puentes
                Decimal[] ValorPuentes = this.calcularPuentes(new TRM(), View.Model.Record.Salida.FechaSalida, View.Model.Record.Llegada.FechaLlegadaPuente, View.Model.Record.Salida.FechaSalidaPuente, View.Model.Record.Llegada.Tipo4.Codigo, View.Model.Record.Salida.Tipo1.Codigo, posLlegada, posSalida);
                View.NumPuentes.Text = ValorPuentes[0] + "";
                View.TotalPuente.Text = (ValorPuentes[1]).ToString("N0");
                ///////////////////////////
                this.calcularParqueo(new TRM(), fechaLlegada, fechaSalida, horaLLegada, horaSalida);
            }

            this.calcularServicioBomberos();
            this.calcularTasasFacturacion();
        }
        public void OnCalcularTotalFacturacion(object sender, EventArgs e)
        {
            this.calcularTotalFacturacion();
        }

        //Suma los vaores que esten cargados en los campos de facturacion
        public void calcularTotalFacturacion()
        {
            try
            {
                //TRM trmConvertir = db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).First();
                TRM trmConvertir = db.TRM.FirstOrDefault(f=> (View.Model.Record.Salida.FechaSalida >= f.FechaInicial.Value && View.Model.Record.Salida.FechaSalida <= f.FechaFinal.Value));
                // calculo el total
                if (!string.IsNullOrEmpty(View.TotalAerodromo.Text))
                {
                    View.TotalFacturacionContado.Text = (Double.Parse(View.TotalAerodromo.Text)).ToString("N0");
                    View.ValorAerodromoUSD.Text = (Double.Parse(View.TotalAerodromo.Text) / trmConvertir.Valor).Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                }
                if (!string.IsNullOrEmpty(View.RecargoNocturno.Text))
                {
                    View.TotalFacturacionContado.Text = (Double.Parse(View.TotalFacturacionContado.Text) + Double.Parse(View.RecargoNocturno.Text)).ToString("N0");
                    View.ValorRecargoUSD.Text = (Double.Parse(View.RecargoNocturno.Text) / trmConvertir.Valor).Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                }
                if (!string.IsNullOrEmpty(View.TotalPuente.Text))
                {
                    View.TotalFacturacionContado.Text = (Double.Parse(View.TotalFacturacionContado.Text) + Double.Parse(View.TotalPuente.Text)).ToString("N0");
                    View.ValorPuentesUSD.Text = (Double.Parse(View.TotalPuente.Text) / trmConvertir.Valor).Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                }
                if (!string.IsNullOrEmpty(View.ValorServBomberos.Text))
                {
                    View.TotalFacturacionContado.Text = (Double.Parse(View.TotalFacturacionContado.Text) + Double.Parse(View.ValorServBomberos.Text)).ToString("N0");
                    View.ValorBomberosUSD.Text = (Double.Parse(View.ValorServBomberos.Text) / trmConvertir.Valor).Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                }
                if (!string.IsNullOrEmpty(View.ValorTasas.Text))
                {
                    View.TotalFacturacionContado.Text = (Double.Parse(View.TotalFacturacionContado.Text) + Double.Parse(View.ValorTasas.Text)).ToString("N0");
                    View.ValorTasasUSD.Text = (Double.Parse(View.ValorTasas.Text) / trmConvertir.Valor).Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                }
                if (!string.IsNullOrEmpty(View.ValorParqueo.Text))
                {
                    View.TotalFacturacionContado.Text = (Double.Parse(View.TotalFacturacionContado.Text) + Double.Parse(View.ValorParqueo.Text)).ToString("N0");
                    View.ValorParqueoUSD.Text = (Double.Parse(View.ValorParqueo.Text) / trmConvertir.Valor).Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                }
                if (!string.IsNullOrEmpty(View.TotalFacturacionContado.Text))
                {
                    View.ValorTotalUSD.Text = (Double.Parse(View.TotalFacturacionContado.Text) / trmConvertir.Valor).Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                }
            }
            catch (Exception excp) { }
        }


        public decimal calcularTarifasAerodromo(TRM Trm, double peso, double valor)
        {
            decimal valorAerodromo = 0;
            if (this.cobraAerodromo == true)
            {
                //Si llega TRM es porque es INTERNACIONAL
                if (Trm != null)
                {
                    //Verifico si hay una TRM vigente para la operacion
                   // if (db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).Count == 1)
                    if (db.TRM.Where(f=> (View.Model.Record.Salida.FechaSalida.Value >= f.FechaInicial.Value && View.Model.Record.Salida.FechaSalida.Value <= f.FechaFinal.Value)).Count() == 1)
                    {
                        //Asigno la TRM vigente
                        //Trm = db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).First();
                        Trm = db.TRM.FirstOrDefault(f => (View.Model.Record.Salida.FechaSalida.Value >= f.FechaInicial.Value && View.Model.Record.Salida.FechaSalida.Value <= f.FechaFinal.Value));
                            //Calculo el Aerodromo = peso aeronave * tarifa AERODROMO INTERNACIONAL * trm
                            //Redondeo al siguiente entero
                            valorAerodromo = (Decimal.Round(Decimal.Parse(peso * valor * Trm.Valor + "")));
                    }
                    else
                    {
                        Util.ShowError("No cuenta con un TRM disponible");
                        View.TotalAerodromo.Text = 0 + "";
                    }
                }
                //Si NO llega TRM es porque es NACIONAL
                else
                {
                    //Calculo el Aerodromo = peso aeronave * tarifa AERODROMO NACIONAL
                    //Redondeo al siguiente entero
                    valorAerodromo = Decimal.Round(Decimal.Parse(peso * valor + ""));
                }
            }
            else
            {
                valorAerodromo = 0;
            }

            return valorAerodromo;
        }
        ///// <summary>
        ///// Calcula las tarifas Aerodromo y el recargo nocturno
        ///// </summary>
        ///// <param name="Trm">Si es nulo es porque es nacional, de lo contrario es porque es nacional</param>
        ///// <param name="peso">Peso de la aeronave</param>
        ///// <param name="valor">valor de la Tarifa Aerodromo</param>
        ///// <param name="recargo"></param>
        //public void calcularTarifasAerodromo(TRM Trm, double peso, double valor, double recargo)
        //{
        //    if (this.cobraAerodromo == true)
        //    {
        //        //Si llega TRM es porque es INTERNACIONAL
        //        if (Trm != null)
        //        {
        //            //Verifico si hay una TRM vigente para la operacion
        //            if (db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).Count == 1)
        //            {
        //                //Asigno la TRM vigente
        //                Trm = db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).First();
        //                //Calculo el Aerodromo = peso aeronave * tarifa AERODROMO INTERNACIONAL * trm
        //                //Redondeo al siguiente entero
        //                View.TotalAerodromo.Text = (Decimal.Round(Decimal.Parse(peso * valor * Trm.Valor + ""))).ToString("N0");
        //            }
        //            else
        //            {
        //                Util.ShowError("No cuenta con un TRM disponible");
        //                View.TotalAerodromo.Text = 0 + "";
        //            }
        //        }
        //        //Si NO llega TRM es porque es NACIONAL
        //        else
        //        {
        //            //Calculo el Aerodromo = peso aeronave * tarifa AERODROMO NACIONAL
        //            //Redondeo al siguiente entero
        //            View.TotalAerodromo.Text = (Decimal.Round(Decimal.Parse(peso * valor + ""))).ToString("N0");
        //        }
        //    }
        //    else
        //    {
        //        View.RecargoNocturno.Text = 0 + "";
        //        View.TotalAerodromo.Text = 0 + "";
        //    }
        //}

        public decimal CalcularRecargoNocturno(double PorcentajeRecargo, decimal valorAerodromo, string HoraLlegada, string HoraSalida)
        {
            decimal valorRecargo = 0;
            if (this.cobraAerodromo == true)
            {
                //Calculo el recargo Nocturno
                if (HoraLlegada != null && HoraSalida != null)
                {
                    //Obtengo la hora en que la aeronave aterrizo
                    int Hora = int.Parse(HoraLlegada.Substring(0, 2));
                    int minutos = int.Parse(HoraLlegada.Substring(3, 2));
                    //
                    int HoraDesp = int.Parse(HoraSalida.Substring(0, 2));
                    int minutosDesp = int.Parse(HoraSalida.Substring(3, 2));
                    //Si son las 6:01 ... se le cobra recargo?
                    if ((Hora >= 18 && minutos >= 0) || (Hora < 6 && minutos <= 59) || (Hora == 6 && minutos == 0))
                    {
                        //Calculo el Recargo Nocturno = (tarifa aerodromo * porcentaje del recargo ) / 100
                        valorRecargo = (valorAerodromo * Decimal.Parse(PorcentajeRecargo + "") / 100);
                        //Redondeo al siguiente entero
                        valorRecargo = (Decimal.Round(valorRecargo));
                    }
                    else if ((HoraDesp >= 18 && minutosDesp >= 0) || (HoraDesp < 6 && minutosDesp <= 59) || (HoraDesp == 6 && minutosDesp == 0))
                    {
                        //Calculo el Recargo Nocturno = (tarifa aerodromo * porcentaje del recargo ) / 100
                        valorRecargo = (valorAerodromo * Decimal.Parse(PorcentajeRecargo + "") / 100);
                        //Redondeo al siguiente entero
                        valorRecargo = (Decimal.Round(valorRecargo));
                    }
                    else
                    {
                        valorRecargo = 0;
                    }
                }
                else
                {
                    Util.ShowError("No hay datos suficientes para calcular Recargo Nocturno");
                    valorRecargo = 0;
                }
            }
            else
            {
                valorRecargo = 0;
            }
            return valorRecargo;
        }

        ///// <summary>
        ///// Calcula la cantidad de puentes y el valor que les va a cobrar
        ///// </summary>
        ///// <param name="Trm">Si es nulo es porque es nacional, de lo contrario es porque es nacional</param>
        //public void calcularPuentes(TRM Trm)
        //{
        //    if (this.cobraPuentes == true)
        //    {
        //        if (
        //            View.Model.RecordSalida.FechaSalidaPuente == null ||
        //            View.Model.RecordLlegada.Tipo4 == null ||
        //            View.Model.RecordSalida.Tipo1 == null ||
        //            View.Model.RecordLlegada.FechaLlegadaPuente == null
        //            )
        //        {
        //            Util.ShowError("No hay datos suficientes para calcular puentes.");
        //            View.TotalPuente.Text = 0 + "";
        //            View.NumPuentes.Text = 0 + "";
        //        }
        //        else
        //        {
        //            Tarifas Tarifa = null;
        //            //Verifico si cuenta con una tarifa de puentes disponible
        //            if (db.GetTarifas(new Tarifas { FechaFiltro = View.Model.RecordSalida.FechaSalida, TipoTarifa = new Tipo { Code = "PUENTES" } }).Count == 1)
        //            {
        //                Tarifa = db.GetTarifas(new Tarifas { FechaFiltro = View.Model.RecordSalida.FechaSalida, TipoTarifa = new Tipo { Code = "PUENTES" } }).First();
        //            }
        //            else
        //            {
        //                Util.ShowError("No cuenta con una tarifa de Puentes disponible");
        //                View.TotalPuente.Text = 0 + "";
        //                return;
        //            }

        //            int cantPuentes = 0;
        //            bool llegoEnPuente, SalioEnPuente;

        //            //Valido si Llego en puente
        //            llegoEnPuente = View.Model.RecordLlegada.Tipo4.Codigo == "PUENTE" ? true : false;
        //            SalioEnPuente = View.Model.RecordSalida.Tipo1.Codigo == "PUENTE" ? true : false;

        //            // Llego y salio por puente, los tipos son iguales.
        //            if ((llegoEnPuente && SalioEnPuente))
        //            {
        //                if ((View.Model.RecordLlegada.Posicion.Codigo == View.Model.RecordSalida.PosicionSalida.Codigo))
        //                {
        //                    //Fechas iguales
        //                    if (View.Model.RecordLlegada.FechaLlegadaPuente == View.Model.RecordSalida.FechaSalidaPuente)
        //                    {
        //                        cantPuentes = 1;
        //                    }
        //                    //Fechas diferentes
        //                    else
        //                    {
        //                        cantPuentes = 2;
        //                    }
        //                }
        //                else
        //                {
        //                    cantPuentes = 2;
        //                }

        //            }//No llego ni salio por puente
        //            else
        //            {
        //                cantPuentes = 0;
        //            }

        //            if ((llegoEnPuente && !SalioEnPuente) || (!llegoEnPuente && SalioEnPuente))
        //            {
        //                cantPuentes = 1;
        //            }

        //            //Si llega un trm es porque es internacional
        //            if (Trm != null)
        //            {
        //                //Verifico si hay una TRM vigente para la operacion
        //                if (db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).Count == 1)
        //                {
        //                    //Asigno la TRM vigente
        //                    Trm = db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).First();
        //                    //Calculo Puentes = Cantidad de puentes * tarifa Puentes INTERNACIONAL * TRM
        //                    View.TotalPuente.Text = "" + (cantPuentes * Tarifa.ValorUSD * Trm.Valor);
        //                    //Redondeo al siguiente entero
        //                    View.TotalPuente.Text = (Decimal.Round(Decimal.Parse(View.TotalPuente.Text))).ToString("N0");
        //                }
        //                else
        //                {
        //                    Util.ShowError("No hay TRM Vigente para la Operacion");
        //                }
        //            }
        //            else
        //            {
        //                //Calculo Puentes = Cantidad de puentes * tarifa Puentes NACIONAL
        //                View.TotalPuente.Text = "" + (cantPuentes * Tarifa.ValorCOP);
        //                //Redondeo al siguiente entero
        //                View.TotalPuente.Text = (Decimal.Round(Decimal.Parse(View.TotalPuente.Text))).ToString("N0");
        //            }
        //            View.NumPuentes.Text = "" + cantPuentes;
        //        }
        //    }
        //    else
        //    {
        //        View.NumPuentes.Text = "" + 0;
        //        View.TotalPuente.Text = "" + 0;
        //    }
        //}


        /// <summary>
        /// Calcula la cantidad de puentes y el valor que les va a cobrar
        /// </summary>
        /// <param name="Trm">Si es nulo es porque es nacional, de lo contrario es porque es nacional</param>
        public decimal[] calcularPuentes(TRM Trm, DateTime? FechaSalida, DateTime? FechaLlegadaPuente, DateTime? FechaSalidaPuente, String TipoPosicionLlegada, String TipoPosicionSalida, String PosicionLlegada, String PosicionSalida)
        {
            Decimal[] Valorespuentes = new Decimal[2];
            //Se usa para Valorespuentes[0] cantidad de puentes
            //Se usa para Valorespuentes[1] Valor de los Puentes
            if (this.cobraPuentes == true)
            {
                TarifaCecoa Tarifa = null;
                //Verifico si cuenta con una tarifa de puentes disponible
                //if (db.GetTarifas(new Tarifas { FechaFiltro = FechaSalidaPuente, TipoTarifa = new Tipo { Code = "PUENTES" } }).Count == 1)
                if (db.TarifaCecoa.Where(f=> f.Tipo1.Codigo == "PUENTES" && (FechaSalidaPuente.Value >= f.FechaInicial.Value && FechaSalidaPuente.Value <= f.FechaFinal.Value)).Count() == 1)
                {
                    //Tarifa = db.GetTarifas(new Tarifas { FechaFiltro = FechaSalidaPuente, TipoTarifa = new Tipo { Code = "PUENTES" } }).First();
                    Tarifa = db.TarifaCecoa.FirstOrDefault(f => f.Tipo1.Codigo == "PUENTES" && (FechaSalidaPuente.Value >= f.FechaInicial.Value && FechaSalidaPuente.Value <= f.FechaFinal.Value));
                }
                else
                {
                    Util.ShowError("No cuenta con una tarifa de Puentes disponible");
                    Valorespuentes[0] = 0;
                    Valorespuentes[1] = 0;
                    return Valorespuentes;
                }

                Valorespuentes[0] = 0;
                bool llegoEnPuente, SalioEnPuente;

                //Valido si Llego en puente
                llegoEnPuente = TipoPosicionLlegada == "PUENTE" ? true : false;
                SalioEnPuente = TipoPosicionSalida == "PUENTE" ? true : false;

                // Llego y salio por puente, los tipos son iguales.
                if ((llegoEnPuente && SalioEnPuente))
                {
                    if ((PosicionLlegada == PosicionSalida))
                    {
                        //Fechas iguales
                        if (FechaLlegadaPuente == FechaSalidaPuente)
                        { Valorespuentes[0] = 1; }
                        //Fechas diferentes
                        else
                        { Valorespuentes[0] = 2; }
                    }
                    else
                    { Valorespuentes[0] = 2; }

                }//No llego ni salio por puente
                else
                { Valorespuentes[0] = 0; }

                if ((llegoEnPuente && !SalioEnPuente) || (!llegoEnPuente && SalioEnPuente))
                { Valorespuentes[0] = 1; }

                //Si llega un trm es porque es internacional
                if (Trm != null)
                {
                    //Verifico si hay una TRM vigente para la operacion
                    //if (db.GetTRM(new TRM { FechaFiltro = FechaSalida }).Count == 1)
                    if (TraerTRM(FechaSalida.Value).Count() == 1)
                    {
                        //Asigno la TRM vigente
                        //Trm = db.GetTRM(new TRM { FechaFiltro = FechaSalida }).First();
                        Trm = TraerTRM(FechaSalida.Value).First();
                        //Calculo Puentes = Cantidad de puentes * tarifa Puentes INTERNACIONAL * TRM
                        //Redondeo al siguiente entero
                        Valorespuentes[1] = Decimal.Round(Valorespuentes[0] * Decimal.Parse((Tarifa.ValorUSD * Trm.Valor) + ""));
                    }
                    else
                    { Util.ShowError("No hay TRM Vigente para la Operacion"); }
                }
                else
                {
                    //Calculo Puentes = Cantidad de puentes * tarifa Puentes NACIONAL
                    //Redondeo al siguiente entero
                    Valorespuentes[1] = Decimal.Round(Valorespuentes[0] * Decimal.Parse((Tarifa.ValorCOP) + ""));
                }
            }
            else
            {
                Valorespuentes[0] = 0;
                Valorespuentes[1] = 0;
            }
            return Valorespuentes;
        }

        public List<TRM> TraerTRM(DateTime FechaFiltro)
        {
            List<TRM> Lista = new List<TRM>();
            if (FechaFiltro != DateTime.MinValue)
            {
                Lista = db.TRM.Where(f => (FechaFiltro >= f.FechaInicial.Value && FechaFiltro <= f.FechaFinal.Value)  ).ToList();
            }else
            {
                return null;
            }
            return Lista;
        }

        public List<TarifaCecoa> TraerTarifa(DateTime FechaFiltro, String Codigo)
        {
            List<TarifaCecoa> Lista = new List<TarifaCecoa>();
            if (FechaFiltro != DateTime.MinValue)
            {
                Lista = db.TarifaCecoa.Where(f => (FechaFiltro >= f.FechaInicial.Value && FechaFiltro <= f.FechaFinal.Value)).ToList();
            }
            else
            {
                Lista = db.TarifaCecoa.ToList();
            }

            if (!String.IsNullOrEmpty(Codigo))
            {
                Lista = Lista.Where(f => f.Tipo1.Codigo == Codigo).ToList();
            }
            return Lista;
        }


        /// <summary>
        /// Calculo el valor a pagar por servicio bomberos
        /// No identifico si es nacional o internacional, ya que al momento de crear el servicio 
        /// se valida esto y se asigna el valor correspondiente.
        /// </summary>
        public void calcularServicioBomberos()
        {
            if (this.cobraBomberos == true)
            {
                double valorBomberos = 0;
                int cantBomberos = 0;
                try
                {
                    //Recorrer los servicios que estan en la lista uno a uno, traer el valor, y sumar
                    foreach (Bombero bombero in View.Model.RegistroBomberosList)
                    {
                        if (bombero.Estado.Nombre == "Nuevo")
                        {
                            if (bombero.ValorServicio != 0)
                            {
                                valorBomberos += bombero.ValorServicio.Value;
                                cantBomberos++;
                            }
                        }
                    }
                    View.CantServBomberos.Text = cantBomberos + "";
                    View.ValorServBomberos.Text = (valorBomberos).ToString("N0");
                }
                catch (Exception exp)
                {
                    View.CantServBomberos.Text = 0 + "";
                    View.ValorServBomberos.Text = 0 + "";
                }

            }
            else
            {
                View.CantServBomberos.Text = 0 + "";
                View.ValorServBomberos.Text = 0 + "";
            }
        }

        public void calcularTasasFacturacion()
        {
            TarifaCecoa Tarifa;
            //Buscar tarifa aerodromo que coincida con  la fecha Salida y que sea de tipo TASAS
            int paganTasa = 0;
            //Sumo todos los registros de pagan tasa, que son diferentes a CREDITO
            paganTasa = db.Tasas.Where(f => f.OperacionID == View.Model.Record.RowID && f.Tipo.Codigo != "CREDITO").Sum(f => f.PaganTasa).Value;
            //Resto los CREDITO
            paganTasa = View.Model.RecordTasas.PaganTasa.Value - db.Tasas
                                                .Where(f => f.OperacionID == View.Model.Record.RowID && f.Tipo.Codigo == "CREDITO" ).Sum(f => f.PaganTasa).Value;

            //if (db.GetTarifas(new Tarifas { FechaFiltro = View.Model.RecordSalida.FechaSalida, TipoTarifa = new Tipo { Code = "TASAS" } }).Count() != 0)
            if (TraerTarifa(View.Model.RecordSalida.FechaSalida.Value, "TASAS").Count() != 0)
            {
                //Tarifa = db.GetTarifas(new Tarifas { FechaFiltro = DateTime.Parse(View.Model.RecordSalida.FechaSalida.ToString()), TipoTarifa = new Tipo { Code = "TASAS" } }).First();
                Tarifa = TraerTarifa(View.Model.RecordSalida.FechaSalida.Value, "TASAS").First();
                if (View.Model.RecordSalida.Tipo2.Codigo == "NACIONAL")
                {
                    View.ValorTasas.Text = (paganTasa * Tarifa.ValorCOP.Value).ToString("N0");
                    View.CantTasas.Text = paganTasa + "";
                }
                else
                {
                    TRM Trm;
                    //if (db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).Count == 1)
                    if (TraerTRM(View.Model.Record.Salida.FechaSalida.Value).Count == 1)
                    {
                        //Asigno la TRM vigente
                        //Trm = db.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).First();
                        Trm = TraerTRM(View.Model.Record.Salida.FechaSalida.Value).First();
                        View.ValorTasas.Text = (paganTasa * Trm.Valor.Value * Tarifa.ValorUSD.Value).ToString("N0");
                        View.CantTasas.Text = paganTasa + "";
                    }
                    else
                    { Util.ShowError("No cuenta con una TRM disponible."); }
                }
            }
            else
            {
                Util.ShowError("No cuenta con una tarifa Tasas disponible");
                View.ValorTasas.Text = "";
                View.CantTasas.Text = "";
            }
        }

        public void calcularParqueo(TRM Trm, DateTime? fechaLlegada, DateTime? fechaSalida, string horaLLegada, string horaSalida)
        {
            if (this.cobraParqueo == true)
            {
                // MetaMasterID 2137 = tarifas PARQUEO
                TarifaCecoa Tarifa = null;
                //Verifico si cuenta con una tarifa de parqueo disponible
                //if (db.GetTarifas(new Tarifas { FechaFiltro = fechaSalida, TipoTarifa = new Tipo { Code = "PARQUEO" } }).Count == 1)
                if (TraerTarifa(fechaSalida.Value, "PARQUEO").Count == 1)
                {
                    Tarifa = TraerTarifa(fechaSalida.Value, "PARQUEO").First();
                }
                else
                {
                    Util.ShowError("No cuenta con una tarifa de Parqueo disponible");
                    View.ValorParqueo.Text = "" + 0;
                    View.CantHorasParqueo.Text = "" + 0;
                    return;
                }

                int cantHoras = 0;
                int HoraLleg = int.Parse(horaLLegada.Substring(0, 2));
                int minutosLlg = int.Parse(horaLLegada.Substring(3, 2));
                int HoraSald = int.Parse(horaSalida.Substring(0, 2));
                int minutosSald = int.Parse(horaSalida.Substring(3, 2));

                //Si llego y salio el mismo dia
                if (fechaLlegada == fechaSalida)
                {
                    // Valido si se demoro mas de dos horas 
                    if (HoraSald - HoraLleg >= 3)
                    {
                        cantHoras = (HoraSald - HoraLleg) - 2;
                        if (minutosSald > minutosLlg)
                        {
                            cantHoras++;
                        }
                    }
                    //Si se demoro 2 horas, valido tambien minutos
                    else if ((HoraSald - HoraLleg == 2) && (minutosSald > minutosLlg))
                    { cantHoras = 1; }
                    else
                    { cantHoras = 0; }
                }
                //si llego un dia y salio otro
                else
                {
                    int horasPrimerDia, horasUltimoDia, diasEnMedio = 0;
                    //Calculo cuantos dias se quedo el avion
                    TimeSpan ts = fechaSalida.Value - fechaLlegada.Value;
                    switch (ts.Days)
                    {
                        case 1: diasEnMedio = 0;
                            break;
                        case 2: diasEnMedio = 1;
                            break;
                        //Si entra al default es porque son mas de 3
                        default:
                            diasEnMedio = ts.Days - 1;
                            break;
                    }
                    HoraLleg = 24 - HoraLleg;

                    //Cant horas de llegada menos las dos horas gratis
                    horasPrimerDia = HoraLleg;
                    horasUltimoDia = (minutosSald > minutosLlg) ? (HoraSald++) : (HoraSald);

                    //Le adiciono la hora de las 00:00 porque no la esta sumando
                    if (HoraSald > 0 && minutosSald > 0)
                    {
                        if (minutosSald > minutosLlg)
                        { horasUltimoDia++; }
                    }
                    //Calculo la cantidad de horas total, sumando las de todos los dias
                    cantHoras = horasPrimerDia + (diasEnMedio * 24) + horasUltimoDia;
                    //Cant horas menos las dos horas gratis
                    if (cantHoras >= 2)
                    { cantHoras = cantHoras - 2; }
                }

                //Si llega un trm es porque es internacional
                if (Trm != null)
                {
                    //Calculo el Parqueo = (tarifa aerodromo * porcentaje del Parqueo INTERNACIONAL * cantidad de horas) / 100
                    View.ValorParqueo.Text = "" + ((((Double.Parse(View.TotalAerodromo.Text)) * Tarifa.ValorUSD) / 100) * cantHoras);
                    //Redondeo al siguiente entero
                    View.ValorParqueo.Text = (Decimal.Round(Decimal.Parse(View.ValorParqueo.Text))).ToString("N0");
                }
                else
                {
                    //Calculo el Parqueo = (tarifa aerodromo * porcentaje del Parqueo NACIONAL * cantidad de horas) / 100
                    View.ValorParqueo.Text = "" + ((((Double.Parse(View.TotalAerodromo.Text)) * Tarifa.ValorCOP) / 100) * cantHoras);
                    //Redondeo al siguiente entero
                    View.ValorParqueo.Text = (Decimal.Round(Decimal.Parse(View.ValorParqueo.Text))).ToString("N0");
                }
                View.CantHorasParqueo.Text = "" + cantHoras;

                //Coloco esto aca porque necesito el valor del aerodromo para calcular el parqueo
                if (this.cobraAerodromo == false)
                {
                    View.TotalAerodromo.Text = 0 + "";
                }
            }
            else
            {
                View.ValorParqueo.Text = "" + 0;
                View.CantHorasParqueo.Text = "" + 0;
            }
        }


        //Si tiene datos de facturacion guardados se los muestro en pantalla
        public void cargarInfoDeFacturacion(Operacion operacion)
        {
            //Recorro las facturas creadas de esta operacion y de la que no este anulada muestro los servicios
            if (View.Model.RecordServiciosAgrupadosList != null)
            {
                if (View.Model.RecordServiciosAgrupadosList.Count != 0)
                {
                    foreach (Servicios auxServ in View.Model.RecordServiciosAgrupadosList)
                    {
                        if (auxServ.Estado.Nombre != "Anulada")
                        {
                            View.CantServBomberos.Text = 0 + "";
                            View.ValorServBomberos.Text = 0 + "";
                            //IList<Servicios> ListaServicios = db.GetServicios(new Servicios { Operacion = operacion, Factura = auxServ.Factura }).ToList();
                            IList<Servicios> ListaServicios = db.Servicios.Where(f => f.OperacionID == operacion.RowID && f.FacturaID == auxServ.FacturaID).ToList();                              
                            foreach (Servicios servicioDet in ListaServicios)
                            {
                                switch (servicioDet.Tipo.Codigo)
                                {
                                    case "AERODROMO": View.TotalAerodromo.Text = servicioDet.Valor.Value.ToString("N0");
                                        break;
                                    case "TASAS": View.CantTasas.Text = servicioDet.Cantidad.ToString();
                                        View.ValorTasas.Text = servicioDet.Valor.Value.ToString("N0");
                                        break;
                                    case "RECARGONOC": View.RecargoNocturno.Text = servicioDet.Valor.Value.ToString("N0");
                                        break;
                                    case "PUENTES": View.NumPuentes.Text = servicioDet.Cantidad.ToString();
                                        View.TotalPuente.Text = servicioDet.Valor.Value.ToString("N0");
                                        break;
                                    case "PARQUEO": View.CantHorasParqueo.Text = servicioDet.Cantidad.ToString();
                                        View.ValorParqueo.Text = servicioDet.Valor.Value.ToString("N0");
                                        break;
                                    case "ASISTENCIA":
                                        View.CantServBomberos.Text = (Int32.Parse(View.CantServBomberos.Text) + servicioDet.Cantidad) + "";
                                        View.ValorServBomberos.Text = (Double.Parse(View.ValorServBomberos.Text) + servicioDet.Valor).Value.ToString("N0");
                                        break;
                                    case "LIMPIEZA":
                                        View.CantServBomberos.Text = Int32.Parse(View.CantServBomberos.Text) + servicioDet.Cantidad + "";
                                        View.ValorServBomberos.Text = (Double.Parse(View.ValorServBomberos.Text) + servicioDet.Valor).Value.ToString("N0");
                                        break;
                                }

                            }
                        }
                    }
                }
                else /// Para las que son credito
                {
                    //IList<Servicios> ListaServicios = db.GetServicios(new Servicios { Operacion = operacion, Status = new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "Servicios" } } }).ToList();//Validar Estado
                    IList<Servicios> ListaServicios = db.Servicios.Where(f=>f.Operacion.RowID == operacion.RowID  && f.Estado.Nombre == "ParaFacturar" && f.Estado.Tipo.Nombre == "Servicios").ToList();
                    foreach (Servicios servicioDet in ListaServicios)
                    {
                        switch (servicioDet.Tipo.Codigo)
                        {
                            case "AERODROMO": View.TotalAerodromo.Text = servicioDet.Valor.Value.ToString("N0");
                                break;
                            case "TASAS": View.CantTasas.Text = servicioDet.Cantidad.ToString();
                                View.ValorTasas.Text = servicioDet.Valor.Value.ToString("N0");
                                break;
                            case "RECARGONOC": View.RecargoNocturno.Text = servicioDet.Valor.Value.ToString("N0");
                                break;
                            case "PUENTES": View.NumPuentes.Text = servicioDet.Cantidad.ToString();
                                View.TotalPuente.Text = servicioDet.Valor.Value.ToString("N0");
                                break;
                            case "PARQUEO": View.CantHorasParqueo.Text = servicioDet.Cantidad.ToString();
                                View.ValorParqueo.Text = servicioDet.Valor.Value.ToString("N0");
                                break;
                            case "ASISTENCIA":
                                View.CantServBomberos.Text = String.IsNullOrEmpty(View.CantServBomberos.Text) ? "0" : View.CantServBomberos.Text;
                                View.ValorServBomberos.Text = String.IsNullOrEmpty(View.ValorServBomberos.Text) ? "0" : View.ValorServBomberos.Text;
                                View.CantServBomberos.Text = (Int32.Parse(View.CantServBomberos.Text) + servicioDet.Cantidad) + "";
                                View.ValorServBomberos.Text = (Double.Parse(View.ValorServBomberos.Text) + servicioDet.Valor).Value.ToString("N0");
                                break;
                            case "LIMPIEZA":
                                View.CantServBomberos.Text = String.IsNullOrEmpty(View.CantServBomberos.Text) ? "0" : View.CantServBomberos.Text;
                                View.ValorServBomberos.Text = String.IsNullOrEmpty(View.ValorServBomberos.Text) ? "0" : View.ValorServBomberos.Text;
                                View.CantServBomberos.Text = Int32.Parse(View.CantServBomberos.Text) + servicioDet.Cantidad + "";
                                View.ValorServBomberos.Text = (Double.Parse(View.ValorServBomberos.Text) + servicioDet.Valor).Value.ToString("N0");
                                break;
                        }
                    }

                }
                this.calcularTotalFacturacion();

            }
        }
        #endregion
    }


}