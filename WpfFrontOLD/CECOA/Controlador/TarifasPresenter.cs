//using System;
//using Assergs.Windows;
//using WMComposite.Events;
//using Microsoft.Practices.Unity;
//using WpfFront.Common;
//using System.Windows;
//using System.Linq;
//using WpfFront.Model;
//using WpfFront.Vista;
//using WpfFront.Modelo;

//namespace WpfFront.Controlador
//{

//    public interface ITarifasPresenter
//    {
//        ITarifasView View { get; set; }
//        ToolWindow Window { get; set; }
//    }


//    public class TarifasPresenter : ITarifasPresenter
//    {
//        public ITarifasView View { get; set; }
//        private readonly IUnityContainer container;
//        private readonly WMSServiceClient service;
//        public ToolWindow Window { get; set; }
//        //Variables Auxiliares 
//        public Connection Local;
//        public Object PresenterParent { get; set; }
//        const string AERODROMO = "Aerodromo";
//        const string AEROPORTUARIAS = "Aeroportuarias";
//        const string BOMBEROS = "Bomberos";
//        const string PUENTES = "Puentes";
//        const string PARQUEO = "Parqueo";

//        public TarifasPresenter(IUnityContainer container, ITarifasView view)
//        {
//            View = view;
//            this.container = container;
//            this.service = new WMSServiceClient();
//            View.Model = this.container.Resolve<TarifasModel>();

//            #region Definicion Metodos

//            #region Aerodromo y todos
//            view.BuscarTarifas += this.onBuscar;
//            view.AgregarTarifas += this.onAgregar;
//            view.SeleccionarTarifas += this.onSeleccionar;
//            view.NuevoRegistro += this.onNuevoRegistro;
//            view.ActualizarListaTarifas += this.onActualizarListaTarifas;
//            #endregion

//            #endregion

//            #region Datos

//            //Obtengo la conexion
//            try { Local = service.GetConnection(new Connection { Name = "LOCAL" }).First(); }
//            catch { }

//            //View.Model.ListaAerolineas = service.GetMetaMaster(new MetaMaster { MetaType = new MetaType { MetaTypeID = 12 } });
//            //view.Model.ListaTipoFactura = service.GetMMaster(new MMaster { MetaType = new MType { MetaMasterID = "TIPOFACTURACION" } });
//            //Cargo las opciones el comboBox
//            //MetaTypeId ServicioBomberos = 22
//            view.Model.ListaTipoServicio = service.GetMMaster(new MMaster { MetaType = new MType { Code = "ServicioBomberos" }, Active = true });

//            View.Tipo = "Todos";
//            //Cargo todas las listas
//            this.ActualizarListaTarifas();
//            View.Tipo = "";
//            #endregion
//        }

//        #region Metodos
//        public void onMostrarStack(object sender, EventArgs e)
//        {
//        }

//        public void onAgregar(object sender, EventArgs e)
//        {
//            switch (View.Tipo)
//            {
//                case AERODROMO:
//                    //Valido que esten seteados los datos
//                    if (string.IsNullOrEmpty(View.TXT_RecargoCOP.Text) || string.IsNullOrEmpty(View.TXT_RecargoUSD.Text) || string.IsNullOrEmpty(View.TXT_ValorCOP.Text) || string.IsNullOrEmpty(View.TXT_ValorUSD.Text) || string.IsNullOrEmpty(View.DTP_FechaInicial.Text) || View.DTP_FechaFinal.Text == "")
//                    {
//                        Util.ShowError("Todos Los Campos deben ser llenados");
//                        return;
//                    }
//                    //Valido que la tasa no exista en el mismo rango, con el mismo tipo y el mismo tiempo de validez
//                    foreach (TarifaCecoa TasaAu in View.Model.ListTarifas)
//                    {
//                        //Valido que no compare el rango con el mismo
//                        if (TasaAu.RowID != View.Model.Tarifas.RowID)
//                        {
//                            if (
//                                ((TasaAu.FechaFinal <= View.DTP_FechaInicial.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicial.SelectedDate) ||
//                                    (TasaAu.FechaInicial <= View.DTP_FechaFinal.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaFinal.SelectedDate) ||
//                                    (TasaAu.FechaInicial >= View.DTP_FechaInicial.SelectedDate && TasaAu.FechaFinal <= View.DTP_FechaFinal.SelectedDate) ||
//                                    (TasaAu.FechaInicial <= View.DTP_FechaInicial.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicial.SelectedDate)))
//                            {
//                                Util.ShowError("No se puede crear la nueva tasa porque hay una activa. Por favor revisar");
//                                return;
//                            }
//                        }
//                    }

//                    //Guardo Tarifa en BD
//                    this.OnAgregarTarifa();
//                    break;
//                case AEROPORTUARIAS:
//                    //Valido que esten seteados los datos
//                    if (string.IsNullOrEmpty(View.TXT_ValorCOPArptrs.Text) || string.IsNullOrEmpty(View.TXT_ValorUSDArptrs.Text) || string.IsNullOrEmpty(View.DTP_FechaInicialArptrs.Text) || View.DTP_FechaFinalArptrs.Text == "")
//                    {
//                        Util.ShowError("Todos Los Campos deben ser llenados");
//                        return;
//                    }
//                    //Valido que la tasa no exista en el mismo rango, con el mismo tipo y el mismo tiempo de validez
//                    foreach (Tarifas TasaAu in View.Model.ListTarifasArptrs)
//                    {
//                        //Valido que no compare el rango con el mismo
//                        if (TasaAu.RowID != View.Model.TarifasArptrs.RowID)
//                        {
//                            if (
//                                (TasaAu.FechaFinal <= View.DTP_FechaInicialArptrs.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicialArptrs.SelectedDate) ||
//                                    (TasaAu.FechaInicial <= View.DTP_FechaFinalArptrs.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaFinalArptrs.SelectedDate) ||
//                                    (TasaAu.FechaInicial >= View.DTP_FechaInicialArptrs.SelectedDate && TasaAu.FechaFinal <= View.DTP_FechaFinalArptrs.SelectedDate) ||
//                                    (TasaAu.FechaInicial <= View.DTP_FechaInicialArptrs.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicialArptrs.SelectedDate))
//                            {
//                                Util.ShowError("No se puede crear la nueva tasa porque hay una activa. Por favor revisar");
//                                return;
//                            }

//                        }
//                    }
//                    //Guardo Tarifa en BD
//                    this.OnAgregarTarifaArptrs();
//                    break;
//                case BOMBEROS:
//                    //Valido que esten seteados los datos
//                    if (string.IsNullOrEmpty(View.TXT_ValorCOPBomber.Text) || string.IsNullOrEmpty(View.DTP_FechaInicialBomber.Text) || View.DTP_FechaFinalBomber.Text == "" || View.cbxTipoServicio.SelectedItem == null)
//                    {
//                        Util.ShowError("Todos Los Campos deben ser llenados");
//                        return;
//                    }
//                    //Valido que la tasa no exista en el mismo rango, con el mismo tipo y el mismo tiempo de validez
//                    foreach (Tarifas TasaAu in View.Model.ListTarifasBomber)
//                    {
//                        //Valido que no compare el rango con el mismo
//                        if (TasaAu.RowID != View.Model.TarifasBomber.RowID)
//                        {
//                            if (
//                                ((TasaAu.FechaFinal <= View.DTP_FechaInicialBomber.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicialBomber.SelectedDate) ||
//                                    (TasaAu.FechaInicial <= View.DTP_FechaFinalBomber.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaFinalBomber.SelectedDate) ||
//                                    (TasaAu.FechaInicial >= View.DTP_FechaInicialBomber.SelectedDate && TasaAu.FechaFinal <= View.DTP_FechaFinalBomber.SelectedDate) ||
//                                    (TasaAu.FechaInicial <= View.DTP_FechaInicialBomber.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicialBomber.SelectedDate)) &&
//                                    TasaAu.TipoServicio.MetaMasterID == ((MMaster)View.cbxTipoServicio.SelectedItem).MetaMasterID)
//                            {
//                                Util.ShowError("No se puede crear la nueva tasa porque hay una activa. Por favor revisar");
//                                return;
//                            }
//                        }
//                    }
//                    //Guardo Tarifa en BD
//                    this.OnAgregarTarifaBomber();
//                    break;
//                //////////////PUENTES/////////////
//                case PUENTES:
//                    //Valido que esten seteados los datos
//                    if (string.IsNullOrEmpty(View.TXT_ValorCOPPuents.Text) || string.IsNullOrEmpty(View.TXT_ValorUSDPuents.Text) || string.IsNullOrEmpty(View.DTP_FechaInicialPuents.Text) || View.DTP_FechaFinalPuents.Text == "")
//                    {
//                        Util.ShowError("Todos Los Campos deben ser llenados");
//                        return;
//                    }
//                    //Valido que la tasa no exista en el mismo rango, con el mismo tipo y el mismo tiempo de validez
//                    foreach (Tarifas TasaAu in View.Model.ListTarifasPuents)
//                    {
//                        //Valido que no compare el rango con el mismo
//                        if (TasaAu.RowID != View.Model.TarifasPuents.RowID)
//                        {
//                            if (
//                                (TasaAu.FechaFinal <= View.DTP_FechaInicialPuents.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicialPuents.SelectedDate) ||
//                                    (TasaAu.FechaInicial <= View.DTP_FechaFinalPuents.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaFinalPuents.SelectedDate) ||
//                                    (TasaAu.FechaInicial >= View.DTP_FechaInicialPuents.SelectedDate && TasaAu.FechaFinal <= View.DTP_FechaFinalPuents.SelectedDate) ||
//                                    (TasaAu.FechaInicial <= View.DTP_FechaInicialPuents.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicialPuents.SelectedDate))
//                            {
//                                Util.ShowError("No se puede crear la nueva tasa porque hay una activa. Por favor revisar");
//                                return;
//                            }

//                        }
//                    }
//                    //Guardo Tarifa en BD
//                    this.OnAgregarTarifaPuents();
//                    break;
//                ///////////PARQUEO////////////////
//                case PARQUEO:
//                    //Valido que esten seteados los datos
//                    if (string.IsNullOrEmpty(View.TXT_ValorCOPParqos.Text) || string.IsNullOrEmpty(View.TXT_ValorUSDParqos.Text) || string.IsNullOrEmpty(View.DTP_FechaInicialParqos.Text) || View.DTP_FechaFinalParqos.Text == "")
//                    {
//                        Util.ShowError("Todos Los Campos deben ser llenados");
//                        return;
//                    }
//                    //Valido que la tasa no exista en el mismo rango, con el mismo tipo y el mismo tiempo de validez
//                    foreach (Tarifas TasaAu in View.Model.ListTarifasParqos)
//                    {
//                        //Valido que no compare el rango con el mismo
//                        if (TasaAu.RowID != View.Model.TarifasParqos.RowID)
//                        {
//                            if (
//                                (TasaAu.FechaFinal <= View.DTP_FechaInicialParqos.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicialParqos.SelectedDate) ||
//                                    (TasaAu.FechaInicial <= View.DTP_FechaFinalParqos.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaFinalParqos.SelectedDate) ||
//                                    (TasaAu.FechaInicial >= View.DTP_FechaInicialParqos.SelectedDate && TasaAu.FechaFinal <= View.DTP_FechaFinalParqos.SelectedDate) ||
//                                    (TasaAu.FechaInicial <= View.DTP_FechaInicialParqos.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicialParqos.SelectedDate))
//                            {
//                                Util.ShowError("No se puede crear la nueva tasa porque hay una activa. Por favor revisar");
//                                return;
//                            }

//                        }
//                    }
//                    //Guardo Tarifa en BD
//                    this.OnAgregarTarifaParqos();
//                    break;
//            }

//        }

//        public void OnAgregarTarifa()
//        {
//            if (View.Model.Tarifas.RowID != 0)
//            {
//                //Actualizar registro si existe
//                View.Model.Tarifas.ModDate = DateTime.Now;
//                View.Model.Tarifas.ModifiedBy = App.curUser.UserName;
//                service.UpdateTarifas(View.Model.Tarifas);

//                //Actualizar Vista
//                this.ActualizarListaTarifas();
//                this.controlarPanelNuevoRegistro(false);
//                CleanToCreate();
//            }
//            else
//            {
//                //Llenar Registro si es uno nuevo
//                Tarifas Registro = CargarRegistro();
//                Registro.CreationDate = DateTime.Now;
//                Registro.CreatedBy = App.curUser.UserName;
//                //Crear Registro
//                service.SaveTarifas(Registro);
//                //Actualizar Vista
//                this.ActualizarListaTarifas();
//                //Oculto el panel de Nuevo Registro
//                this.controlarPanelNuevoRegistro(false);
//                this.CleanToCreate();
//            }


//        }

//        public void OnAgregarTarifaArptrs()
//        {
//            if (View.Model.TarifasArptrs.RowID != 0)
//            {
//                //Actualizar registro si existe
//                View.Model.TarifasArptrs.ModDate = DateTime.Now;
//                View.Model.TarifasArptrs.ModifiedBy = App.curUser.UserName;
//                service.UpdateTarifas(View.Model.TarifasArptrs);
//                //Actualizar Vista
//                this.ActualizarListaTarifas();
//                this.controlarPanelNuevoRegistro(false);
//                CleanToCreate();
//            }
//            else
//            {
//                //Llenar Registro si es uno nuevo
//                Tarifas Registro = CargarRegistro();
//                Registro.CreationDate = DateTime.Now;
//                Registro.CreatedBy = App.curUser.UserName;
//                //Crear Registro
//                service.SaveTarifas(Registro);
//                //Actualizar Vista
//                this.ActualizarListaTarifas();
//                //Oculto el panel de Nuevo Registro
//                this.controlarPanelNuevoRegistro(false);
//                this.CleanToCreate();
//            }
//        }

//        public void OnAgregarTarifaBomber()
//        {
//            if (View.Model.TarifasBomber.RowID != 0)
//            {
//                //Actualizar registro
//                View.Model.TarifasBomber.ModDate = DateTime.Now;
//                View.Model.TarifasBomber.ModifiedBy = App.curUser.UserName;
//                View.Model.TarifasBomber.TipoServicio = ((MMaster)View.cbxTipoServicio.SelectedItem);
//                service.UpdateTarifas(View.Model.TarifasBomber);
//                //Actualizar Vista
//                this.ActualizarListaTarifas();
//                this.controlarPanelNuevoRegistro(false);
//                CleanToCreate();
//            }
//            else
//            {
//                //Llenar Registro si es uno nuevo
//                Tarifas Registro = CargarRegistro();
//                Registro.CreationDate = DateTime.Now;
//                Registro.CreatedBy = App.curUser.UserName;
//                //Guardar Registro
//                service.SaveTarifas(Registro);
//                //Actualizar Vista
//                this.ActualizarListaTarifas();
//                //Oculto el panel de Nuevo Registro
//                this.controlarPanelNuevoRegistro(false);
//                this.CleanToCreate();
//                //  View.Model.TarifasBomber = new Tarifas();
//            }
//        }

//        public void OnAgregarTarifaPuents()
//        {
//            if (View.Model.TarifasPuents.RowID != 0)
//            {
//                //Actualizar registro si existe
//                View.Model.TarifasPuents.ModDate = DateTime.Now;
//                View.Model.TarifasPuents.ModifiedBy = App.curUser.UserName;
//                service.UpdateTarifas(View.Model.TarifasPuents);
//                //Actualizar Vista
//                this.ActualizarListaTarifas();
//                this.controlarPanelNuevoRegistro(false);
//                CleanToCreate();
//            }
//            else
//            {
//                //Llenar Registro si es uno nuevo
//                Tarifas Registro = CargarRegistro();
//                Registro.CreationDate = DateTime.Now;
//                Registro.CreatedBy = App.curUser.UserName;
//                //Crear Registro
//                service.SaveTarifas(Registro);
//                //Actualizar Vista
//                this.ActualizarListaTarifas();
//                //Oculto el panel de Nuevo Registro
//                this.controlarPanelNuevoRegistro(false);
//                this.CleanToCreate();
//            }
//        }

//        public void OnAgregarTarifaParqos()
//        {
//            if (View.Model.TarifasParqos.RowID != 0)
//            {
//                //Actualizar registro si existe
//                View.Model.TarifasParqos.ModDate = DateTime.Now;
//                View.Model.TarifasParqos.ModifiedBy = App.curUser.UserName;
//                service.UpdateTarifas(View.Model.TarifasParqos);
//                //Actualizar Vista
//                this.ActualizarListaTarifas();
//                this.controlarPanelNuevoRegistro(false);
//                CleanToCreate();
//            }
//            else
//            {
//                //Llenar Registro si es uno nuevo
//                Tarifas Registro = CargarRegistro();
//                Registro.CreationDate = DateTime.Now;
//                Registro.CreatedBy = App.curUser.UserName;
//                //Crear Registro
//                service.SaveTarifas(Registro);
//                //Actualizar Vista
//                this.ActualizarListaTarifas();
//                //Oculto el panel de Nuevo Registro
//                this.controlarPanelNuevoRegistro(false);
//                this.CleanToCreate();
//            }
//        }



//        public Tarifas CargarRegistro()
//        {
//            // MetaMasterID 2133 = tarifas AERODROMO
//            // MetaMasterID 2134 = tarifas AEROPORTUARIAS
//            // MetaMasterID 2135 = tarifas BOMBEROS
//            // MetaMasterID 2136 = tarifas PUENTES
//            // MetaMasterID 2137 = tarifas PARQUEO
//            Tarifas Registro = new Tarifas();
//            switch (View.Tipo)
//            {
//                case AERODROMO:
//                    Registro.ValorCOP = Double.Parse(View.TXT_ValorCOP.Text.Replace(".", ",")); //Solo hace conversion a Double si llega con ,
//                    Registro.ValorUSD = Double.Parse(View.TXT_ValorUSD.Text.Replace(".", ",")); //Solo hace conversion a Double si llega con ,
//                    Registro.RecargoNocturnoCOP = Double.Parse(View.TXT_RecargoCOP.Text.Replace(".", ","));
//                    Registro.RecargoNocturnoUSD = Double.Parse(View.TXT_RecargoUSD.Text.Replace(".", ","));
//                    Registro.FechaInicial = DateTime.Parse(View.DTP_FechaInicial.Text);
//                    Registro.FechaFinal = DateTime.Parse(View.DTP_FechaFinal.Text);
//                    Registro.TipoTarifa = service.GetMMaster(new MMaster { Code = "AERODROMO" }).First();
//                    break;
//                case AEROPORTUARIAS:
//                    Registro.ValorCOP = Double.Parse(View.TXT_ValorCOPArptrs.Text.Replace(".", ",")); //Solo hace conversion a Double si llega con ,
//                    Registro.ValorUSD = Double.Parse(View.TXT_ValorUSDArptrs.Text.Replace(".", ",")); //Solo hace conversion a Double si llega con ,
//                    Registro.FechaInicial = DateTime.Parse(View.DTP_FechaInicialArptrs.Text);
//                    Registro.FechaFinal = DateTime.Parse(View.DTP_FechaFinalArptrs.Text);
//                    Registro.TipoTarifa = service.GetMMaster(new MMaster { Code = "TASAS" }).First();
//                    break;
//                case BOMBEROS:
//                    Registro.ValorCOP = Double.Parse(View.TXT_ValorCOPBomber.Text.Replace(".", ",")); //Solo hace conversion a Double si llega con ,
//                    Registro.FechaInicial = DateTime.Parse(View.DTP_FechaInicialBomber.Text);
//                    Registro.FechaFinal = DateTime.Parse(View.DTP_FechaFinalBomber.Text);
//                    Registro.TipoTarifa = service.GetMMaster(new MMaster { Code = "BOMBEROS" }).First();
//                    Registro.TipoServicio = ((MMaster)View.cbxTipoServicio.SelectedItem);
//                    break;
//                case PUENTES:
//                    Registro.ValorCOP = Double.Parse(View.TXT_ValorCOPPuents.Text.Replace(".", ",")); //Solo hace conversion a Double si llega con ,
//                    Registro.ValorUSD = Double.Parse(View.TXT_ValorUSDPuents.Text.Replace(".", ",")); //Solo hace conversion a Double si llega con ,
//                    Registro.FechaInicial = DateTime.Parse(View.DTP_FechaInicialPuents.Text);
//                    Registro.FechaFinal = DateTime.Parse(View.DTP_FechaFinalPuents.Text);
//                    Registro.TipoTarifa = service.GetMMaster(new MMaster { Code = "PUENTES" }).First();
//                    break;
//                case PARQUEO:
//                    Registro.ValorCOP = Double.Parse(View.TXT_ValorCOPParqos.Text.Replace(".", ",")); //Solo hace conversion a Double si llega con ,
//                    Registro.ValorUSD = Double.Parse(View.TXT_ValorUSDParqos.Text.Replace(".", ",")); //Solo hace conversion a Double si llega con ,
//                    Registro.FechaInicial = DateTime.Parse(View.DTP_FechaInicialParqos.Text);
//                    Registro.FechaFinal = DateTime.Parse(View.DTP_FechaFinalParqos.Text);
//                    Registro.TipoTarifa = service.GetMMaster(new MMaster { Code = "PARQUEO" }).First();
//                    break;
//            }

//            return Registro;
//        }

//        public void onBuscar(object sender, EventArgs e)
//        {
//            // MetaMasterID 2133 = tarifas AERODROMO
//            // MetaMasterID 2134 = tarifas AEROPORTUARIAS
//            // MetaMasterID 2135 = tarifas BOMBEROS
//            // MetaMasterID 2136 = tarifas PUENTES
//            // MetaMasterID 2137 = tarifas PARQUEO
//            DateTime FechaIni = DateTime.MinValue;
//            DateTime FechaFin = DateTime.MinValue;
//            switch (View.Tipo)
//            {
//                case AERODROMO:
//                    //Si vienen vacios los campos, les asigno por defecto cero (0)
//                    if (String.IsNullOrEmpty(View.TXT_BuscarRecargoCOP.Text))
//                    {
//                        View.TXT_BuscarRecargoCOP.Text = 0.ToString();
//                    }
//                    if (String.IsNullOrEmpty(View.TXT_BuscarRecargoUSD.Text))
//                    {
//                        View.TXT_BuscarRecargoUSD.Text = 0.ToString();
//                    }
//                    if (String.IsNullOrEmpty(View.TXT_BuscarValorCOP.Text))
//                    {
//                        View.TXT_BuscarValorCOP.Text = 0.ToString();
//                    }
//                    if (String.IsNullOrEmpty(View.TXT_BuscarValorUSD.Text))
//                    {
//                        View.TXT_BuscarValorUSD.Text = 0.ToString();
//                    }

//                    if (!string.IsNullOrEmpty(View.DTP_BuscarFechaInicial.Text))
//                    {
//                        FechaIni = Convert.ToDateTime(View.DTP_BuscarFechaInicial.Text);
//                    }
//                    if (!string.IsNullOrEmpty(View.DTP_BuscarFechaFinal.Text))
//                    {
//                        FechaFin = Convert.ToDateTime(View.DTP_BuscarFechaFinal.Text);
//                    }
//                    // Hago la busqueda
//                    View.Model.ListTarifas = service.GetTarifas(new Tarifas
//                    {
//                        RecargoNocturnoCOP = Double.Parse(View.TXT_BuscarRecargoCOP.Text),
//                        RecargoNocturnoUSD = Double.Parse(View.TXT_BuscarRecargoUSD.Text),
//                        FechaInicial = FechaIni,
//                        FechaFinal = FechaFin,
//                        ValorCOP = Double.Parse(View.TXT_BuscarValorCOP.Text),
//                        ValorUSD = Double.Parse(View.TXT_BuscarValorUSD.Text),
//                        TipoTarifa = service.GetMMaster(new MMaster { Code = "AERODROMO" }).First()
//                    });

//                    break;
//                case AEROPORTUARIAS:
//                    //Si vienen vacios los campos, les asigno por defecto cero (0)
//                    if (String.IsNullOrEmpty(View.TXT_BuscarValorCOPArptrs.Text))
//                    {
//                        View.TXT_BuscarValorCOPArptrs.Text = 0.ToString();
//                    }
//                    if (String.IsNullOrEmpty(View.TXT_BuscarValorUSDArptrs.Text))
//                    {
//                        View.TXT_BuscarValorUSDArptrs.Text = 0.ToString();
//                    }

//                    if (!string.IsNullOrEmpty(View.DTP_BuscarFechaInicialArptrs.Text))
//                    {
//                        FechaIni = Convert.ToDateTime(View.DTP_BuscarFechaInicialArptrs.Text);
//                    }
//                    if (!string.IsNullOrEmpty(View.DTP_BuscarFechaFinalArptrs.Text))
//                    {
//                        FechaFin = Convert.ToDateTime(View.DTP_BuscarFechaFinalArptrs.Text);
//                    }
//                    // Hago la busqueda
//                    View.Model.ListTarifasArptrs = service.GetTarifas(new Tarifas
//                    {
//                        ValorCOP = Double.Parse(View.TXT_BuscarValorCOPArptrs.Text),
//                        ValorUSD = Double.Parse(View.TXT_BuscarValorUSDArptrs.Text),
//                        FechaInicial = FechaIni,
//                        FechaFinal = FechaFin,
//                        TipoTarifa = service.GetMMaster(new MMaster { MetaMasterID = 2134 }).First()
//                    });
//                    break;
//                case BOMBEROS:
//                    //Si vienen vacios los campos, les asigno por defecto cero (0)
//                    if (String.IsNullOrEmpty(View.TXT_BuscarValorCOPBomber.Text))
//                    {
//                        View.TXT_BuscarValorCOPBomber.Text = 0.ToString();
//                    }

//                    if (!string.IsNullOrEmpty(View.DTP_BuscarFechaInicialBomber.Text))
//                    {
//                        FechaIni = Convert.ToDateTime(View.DTP_BuscarFechaInicialBomber.Text);
//                    }
//                    if (!string.IsNullOrEmpty(View.DTP_BuscarFechaFinalBomber.Text))
//                    {
//                        FechaFin = Convert.ToDateTime(View.DTP_BuscarFechaFinalBomber.Text);
//                    }
//                    // Hago la busqueda
//                    View.Model.ListTarifasBomber = service.GetTarifas(new Tarifas
//                    {
//                        ValorCOP = Double.Parse(View.TXT_BuscarValorCOPBomber.Text),
//                        FechaInicial = FechaIni,
//                        FechaFinal = FechaFin,
//                        TipoTarifa = service.GetMMaster(new MMaster { Code = "BOMBEROS" }).First()
//                    });
//                    break;
//                case PUENTES:
//                    //Si vienen vacios los campos, les asigno por defecto cero (0)
//                    if (String.IsNullOrEmpty(View.TXT_BuscarValorCOPPuents.Text))
//                    {
//                        View.TXT_BuscarValorCOPPuents.Text = 0.ToString();
//                    }
//                    if (String.IsNullOrEmpty(View.TXT_BuscarValorUSDPuents.Text))
//                    {
//                        View.TXT_BuscarValorUSDPuents.Text = 0.ToString();
//                    }

//                    if (!string.IsNullOrEmpty(View.DTP_BuscarFechaInicialPuents.Text))
//                    {
//                        FechaIni = Convert.ToDateTime(View.DTP_BuscarFechaInicialPuents.Text);
//                    }
//                    if (!string.IsNullOrEmpty(View.DTP_BuscarFechaFinalPuents.Text))
//                    {
//                        FechaFin = Convert.ToDateTime(View.DTP_BuscarFechaFinalPuents.Text);
//                    }
//                    // Hago la busqueda
//                    View.Model.ListTarifasPuents = service.GetTarifas(new Tarifas
//                    {
//                        ValorCOP = Double.Parse(View.TXT_BuscarValorCOPPuents.Text),
//                        ValorUSD = Double.Parse(View.TXT_BuscarValorUSDPuents.Text),
//                        FechaInicial = FechaIni,
//                        FechaFinal = FechaFin,
//                        TipoTarifa = service.GetMMaster(new MMaster { Code = "PUENTES" }).First()
//                    });
//                    break;
//                case PARQUEO:
//                    //Si vienen vacios los campos, les asigno por defecto cero (0)
//                    if (String.IsNullOrEmpty(View.TXT_BuscarValorCOPParqos.Text))
//                    {
//                        View.TXT_BuscarValorCOPParqos.Text = 0.ToString();
//                    }
//                    if (String.IsNullOrEmpty(View.TXT_BuscarValorUSDParqos.Text))
//                    {
//                        View.TXT_BuscarValorUSDParqos.Text = 0.ToString();
//                    }

//                    if (!string.IsNullOrEmpty(View.DTP_BuscarFechaInicialParqos.Text))
//                    {
//                        FechaIni = Convert.ToDateTime(View.DTP_BuscarFechaInicialParqos.Text);
//                    }
//                    if (!string.IsNullOrEmpty(View.DTP_BuscarFechaFinalParqos.Text))
//                    {
//                        FechaFin = Convert.ToDateTime(View.DTP_BuscarFechaFinalParqos.Text);
//                    }
//                    // Hago la busqueda
//                    View.Model.ListTarifasParqos = service.GetTarifas(new Tarifas
//                    {
//                        ValorCOP = Double.Parse(View.TXT_BuscarValorCOPParqos.Text),
//                        ValorUSD = Double.Parse(View.TXT_BuscarValorUSDParqos.Text),
//                        FechaInicial = FechaIni,
//                        FechaFinal = FechaFin,
//                        TipoTarifa = service.GetMMaster(new MMaster { Code = "PARQUEO" }).First()
//                    });
//                    break;
//            }
//        }

//        public void onSeleccionar(object sender, DataEventArgs<Tarifas> TarifaAerodromo)
//        {
//            switch (View.Tipo)
//            {
//                case AERODROMO:
//                    View.Model.Tarifas = TarifaAerodromo.Value;
//                    break;
//                case AEROPORTUARIAS:
//                    View.Model.TarifasArptrs = TarifaAerodromo.Value;
//                    break;
//                case BOMBEROS:
//                    View.Model.TarifasBomber = TarifaAerodromo.Value;
//                    View.cbxTipoServicio.SelectedItem = TarifaAerodromo.Value.TipoServicio;
//                    break;
//                case PUENTES:
//                    View.Model.TarifasPuents = TarifaAerodromo.Value;
//                    break;
//                case PARQUEO:
//                    View.Model.TarifasParqos = TarifaAerodromo.Value;
//                    break;

//            }
//            this.controlarPanelNuevoRegistro(true);
//        }

//        public void onNuevoRegistro(object sender, EventArgs e)
//        {
//            this.controlarPanelNuevoRegistro(true);
//            CleanToCreate();
//        }


//        public void onActualizarListaTarifas(object sender, EventArgs e)
//        {
//            this.ActualizarListaTarifas();
//            this.cleanToRefresh();
//        }

//        public void cleanToRefresh()
//        {
//            switch (View.Tipo)
//            {
//                case AERODROMO:
//                    View.TXT_BuscarRecargoCOP.Text = 0.ToString();
//                    View.TXT_BuscarRecargoUSD.Text = 0.ToString();
//                    View.TXT_BuscarValorCOP.Text = 0.ToString();
//                    View.TXT_BuscarValorUSD.Text = 0.ToString();
//                    View.DTP_BuscarFechaInicial.Text = "";
//                    View.DTP_BuscarFechaFinal.Text = "";
//                    break;
//                case AEROPORTUARIAS:
//                    View.TXT_BuscarValorCOPArptrs.Text = 0.ToString();
//                    View.TXT_BuscarValorUSDArptrs.Text = 0.ToString();
//                    View.DTP_BuscarFechaInicialArptrs.Text = "";
//                    View.DTP_BuscarFechaFinalArptrs.Text = "";
//                    break;
//                case BOMBEROS:
//                    View.TXT_BuscarValorCOPBomber.Text = 0.ToString();
//                    View.DTP_BuscarFechaInicialBomber.Text = "";
//                    View.DTP_BuscarFechaFinalBomber.Text = "";
//                    View.cbxTipoServicio.SelectedIndex = -1;
//                    break;
//                case PUENTES:
//                    View.TXT_BuscarValorCOPPuents.Text = 0.ToString();
//                    View.TXT_BuscarValorUSDPuents.Text = 0.ToString();
//                    View.DTP_BuscarFechaInicialPuents.Text = "";
//                    View.DTP_BuscarFechaFinalPuents.Text = "";
//                    break;
//                case PARQUEO:
//                    View.TXT_BuscarValorCOPParqos.Text = 0.ToString();
//                    View.TXT_BuscarValorUSDParqos.Text = 0.ToString();
//                    View.DTP_BuscarFechaInicialParqos.Text = "";
//                    View.DTP_BuscarFechaFinalParqos.Text = "";
//                    break;
//            }
//        }


//        /**
//       * Actualiza la lista de Tarifas aerodromo 
//       * */
//        public void ActualizarListaTarifas()
//        {
//            // MetaMasterID 2133 = tarifas AERODROMO
//            // MetaMasterID 2134 = tarifas AEROPORTUARIAS
//            // MetaMasterID 2135 = tarifas BOMBEROS
//            // MetaMasterID 2136 = tarifas PUENTES
//            // MetaMasterID 2137 = tarifas PARQUEO
//            switch (View.Tipo)
//            {
//                case AERODROMO:
//                    View.Model.ListTarifas = service.GetTarifas(new Tarifas { TipoTarifa = new MMaster { Code = "AERODROMO" } });
//                    break;
//                case AEROPORTUARIAS:
//                    View.Model.ListTarifasArptrs = service.GetTarifas(new Tarifas { TipoTarifa = new MMaster { Code = "TASAS" } });
//                    break;
//                case BOMBEROS:
//                    View.Model.ListTarifasBomber = service.GetTarifas(new Tarifas { TipoTarifa = new MMaster { Code = "BOMBEROS" } });
//                    break;
//                case PUENTES:
//                    View.Model.ListTarifasPuents = service.GetTarifas(new Tarifas { TipoTarifa = new MMaster { Code = "PUENTES" } });
//                    break;
//                case PARQUEO:
//                    View.Model.ListTarifasParqos = service.GetTarifas(new Tarifas { TipoTarifa = new MMaster { Code = "PARQUEO" } });
//                    break;
//                case "Todos":
//                    View.Model.ListTarifas = service.GetTarifas(new Tarifas { TipoTarifa = new MMaster { Code = "AERODROMO" } });
//                    View.Model.ListTarifasArptrs = service.GetTarifas(new Tarifas { TipoTarifa = new MMaster { Code = "TASAS" } });
//                    View.Model.ListTarifasBomber = service.GetTarifas(new Tarifas { TipoTarifa = new MMaster { Code = "BOMBEROS" } });
//                    View.Model.ListTarifasPuents = service.GetTarifas(new Tarifas { TipoTarifa = new MMaster { Code = "PUENTES" } });
//                    View.Model.ListTarifasParqos = service.GetTarifas(new Tarifas { TipoTarifa = new MMaster { Code = "PARQUEO" } });
//                    break;
//            }
//        }
//        /**
//         * Oculta o muestra el panel de nuevo registro
//         * true= mostrar
//         * false= ocultar
//         * */
//        public void controlarPanelNuevoRegistro(bool status)
//        {
//            if (status)
//            {
//                switch (View.Tipo)
//                {
//                    case AERODROMO:
//                        View.PanelNuevoRegistro.Visibility = Visibility.Visible;
//                        break;
//                    case AEROPORTUARIAS:
//                        View.PanelNuevoRegistroArptrs.Visibility = Visibility.Visible;
//                        break;
//                    case BOMBEROS:
//                        View.PanelNuevoRegistroBomber.Visibility = Visibility.Visible;
//                        break;
//                    case PUENTES:
//                        View.PanelNuevoRegistroPuents.Visibility = Visibility.Visible;
//                        break;
//                    case PARQUEO:
//                        View.PanelNuevoRegistroParqos.Visibility = Visibility.Visible;
//                        break;
//                }
//            }
//            else
//            {
//                switch (View.Tipo)
//                {
//                    case AERODROMO:
//                        View.PanelNuevoRegistro.Visibility = Visibility.Collapsed;
//                        break;
//                    case AEROPORTUARIAS:
//                        View.PanelNuevoRegistroArptrs.Visibility = Visibility.Collapsed;
//                        break;
//                    case BOMBEROS:
//                        View.PanelNuevoRegistroBomber.Visibility = Visibility.Collapsed;
//                        break;
//                    case PUENTES:
//                        View.PanelNuevoRegistroPuents.Visibility = Visibility.Collapsed;
//                        break;
//                    case PARQUEO:
//                        View.PanelNuevoRegistroParqos.Visibility = Visibility.Collapsed;
//                        break;
//                }
//            }
//        }

//        public void CleanToCreate()
//        {
//            switch (View.Tipo)
//            {
//                case AERODROMO:
//                    View.Model.Tarifas = new Tarifas();
//                    View.TXT_ValorCOP.Text = "";
//                    View.TXT_ValorUSD.Text = "";
//                    View.DTP_FechaInicial.SelectedDate = DateTime.Now;
//                    View.DTP_FechaFinal.SelectedDate = DateTime.Now;
//                    View.TXT_RecargoCOP.Text = "";
//                    View.TXT_RecargoUSD.Text = "";
//                    View.DTP_FechaInicial.Text = "";
//                    View.DTP_FechaFinal.Text = "";
//                    break;
//                case AEROPORTUARIAS:
//                    View.Model.TarifasArptrs = new Tarifas();
//                    View.TXT_ValorCOPArptrs.Text = "";
//                    View.TXT_ValorUSDArptrs.Text = "";
//                    View.DTP_FechaInicialArptrs.SelectedDate = DateTime.Now;
//                    View.DTP_FechaFinalArptrs.SelectedDate = DateTime.Now;
//                    View.DTP_FechaInicialArptrs.Text = "";
//                    View.DTP_FechaFinalArptrs.Text = "";
//                    break;
//                case BOMBEROS:
//                    View.Model.TarifasBomber = new Tarifas();
//                    View.TXT_ValorCOPBomber.Text = "";
//                    View.DTP_FechaInicialBomber.SelectedDate = DateTime.Now;
//                    View.DTP_FechaFinalBomber.SelectedDate = DateTime.Now;
//                    View.DTP_FechaInicialBomber.Text = "";
//                    View.DTP_FechaFinalBomber.Text = "";
//                    View.cbxTipoServicio.SelectedIndex = -1;
//                    break;
//                case PUENTES:
//                    View.Model.TarifasPuents = new Tarifas();
//                    View.TXT_ValorCOPPuents.Text = "";
//                    View.TXT_ValorUSDPuents.Text = "";
//                    View.DTP_FechaInicialPuents.SelectedDate = DateTime.Now;
//                    View.DTP_FechaFinalPuents.SelectedDate = DateTime.Now;
//                    View.DTP_FechaInicialPuents.Text = "";
//                    View.DTP_FechaFinalPuents.Text = "";
//                    break;
//                case PARQUEO:
//                    View.Model.TarifasParqos = new Tarifas();
//                    View.TXT_ValorCOPParqos.Text = "";
//                    View.TXT_ValorUSDParqos.Text = "";
//                    View.DTP_FechaInicialParqos.SelectedDate = DateTime.Now;
//                    View.DTP_FechaFinalParqos.SelectedDate = DateTime.Now;
//                    View.DTP_FechaInicialParqos.Text = "";
//                    View.DTP_FechaFinalParqos.Text = "";
//                    break;
//            }
//            View.Tipo = "";
//        }

//        #endregion
//    }
//}