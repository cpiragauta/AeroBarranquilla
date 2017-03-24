using System;
using Assergs.Windows;
using WMComposite.Events;
using Microsoft.Practices.Unity;
using WpfFront.Common;
using System.Windows;
using System.Linq;
using WpfFront.Model;
using WpfFront.Modelo;
using WpfFront.Vista;
using System.Globalization;



namespace WpfFront.Controlador
{

    public interface ITRMPresenter
    {
        ITRMView View { get; set; }
        ToolWindow Window { get; set; }
    }


    public class TRMPresenter : ITRMPresenter
    {
        wmsEntities db = new wmsEntities();

        public ITRMView View { get; set; }
        private readonly IUnityContainer container;
        public ToolWindow Window { get; set; }
        //Variables Auxiliares 
    
        public Object PresenterParent { get; set; }

        public TRMPresenter(IUnityContainer container, ITRMView view)
        {
            View = view;
            this.container = container;
           
            View.Model = this.container.Resolve<TRMModel>();

            #region Definicion Metodos

            View.AgregarTRM += new EventHandler<EventArgs>(this.onAgregar);
            View.BuscarTRM += new EventHandler<EventArgs>(this.onBuscar);
            View.SeleccionarTRM += new EventHandler<DataEventArgs<TRM>>(this.onSeleccionar);
            View.NuevoRegistroTRM += new EventHandler<EventArgs>(this.onNuevoRegistro);
            View.ActualizarTRM += new EventHandler<EventArgs>(this.onActualizar);
            #endregion

            #region Datos

            //Obtengo la conexion
        

            //View.Model.ListaAerolineas = service.GetMetaMaster(new MetaMaster { MetaType = new MetaType { MetaTypeID = 12 } });
            //view.Model.ListaTipoFactura = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOFACTURACION" } });

            //Inicio las variables
            CleanToCreate();

            this.actualizarListaTRM();
            #endregion
        }

        #region Declaracion Metodos

        public void onAgregar(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(View.DTP_FechaInicial.Text) || string.IsNullOrEmpty(View.DTP_FechaFinal.Text) || string.IsNullOrEmpty(View.TXT_Valor.Text))
            {
                Util.ShowError("Todos Los Campos deben ser llenados");
                return;
            }
            //Valido que no exista una tasa con el mismo rango de fechas
            string mesajeError = validarFechasTasa();
            if(!string.IsNullOrEmpty(mesajeError)){
                 Util.ShowError(mesajeError);
                return;
            }

            if (View.Model.TRM != null)
            {
                View.Model.TRM.UsuarioModificacion = App.curUser.NombreUsuario;
                View.Model.TRM.FechaModificacion = DateTime.Now;
                //Actualizar registro si existe
                //service.UpdateTRM(View.Model.TRM);
                db.SaveChanges();
                //Actualizar Vista
                this.actualizarListaTRM();
                CleanToCreate();
                this.controlarPanelNuevoRegistro(false);
            }
            else
            {
                //Llenar Registro si es uno nuevo
                TRM TRM = new TRM();
                TRM.FechaInicial = DateTime.Parse(View.DTP_FechaInicial.Text);
                TRM.FechaFinal = DateTime.Parse(View.DTP_FechaFinal.Text);
                TRM.Valor = Convert.ToDouble(View.TXT_Valor.Text, CultureInfo.CreateSpecificCulture("en-US"));//////ffsfsfsfsssssssssssssssssssssssssssssss
                TRM.UsuarioCreacion = App.curUser.NombreUsuario;
                TRM.FechaCreacion = DateTime.Now;
                //Crear Registro
                //TRM = service.SaveTRM(TRM);
                TRM = db.TRM.Add(TRM);
                db.SaveChanges();
                
                //Actualizar Vista
                this.actualizarListaTRM();
                CleanToCreate();
                this.controlarPanelNuevoRegistro(false);
            }
        }


        public string validarFechasTasa()
        {
            string mensajeError = "";
            //Cuando es actualizar 
            if (View.Model.TRM != null)
            {
                

                //Valido que la tasa no exista en el mismo rango, con el mismo tipo y el mismo tiempo de validez
                foreach (TRM TasaAu in View.Model.ListTRM)
                {
                    //Valido que no compare el rango con el mismo
                    if (TasaAu.RowID != View.Model.TRM.RowID)
                    {
                        //Valido que no compare el rango con el mismo
                        if (
                            ((TasaAu.FechaFinal <= View.DTP_FechaInicial.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicial.SelectedDate) ||
                                (TasaAu.FechaInicial <= View.DTP_FechaFinal.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaFinal.SelectedDate) ||
                                (TasaAu.FechaInicial >= View.DTP_FechaInicial.SelectedDate && TasaAu.FechaFinal <= View.DTP_FechaFinal.SelectedDate) ||
                                (TasaAu.FechaInicial <= View.DTP_FechaInicial.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicial.SelectedDate)))
                        {
                            mensajeError = "No se puede crear el nuevo TRM porque hay uno activo. Por favor revisar";
                        }
                    }
                }
            }
            //Cuando es un nuevo registro
            else
            {
                if (View.DTP_FechaFinal.SelectedDate < View.DTP_FechaInicial.SelectedDate)
                {
                    mensajeError = "La fecha Final debe ser Mayor a la Inicial";
                }
                //Valido que la tasa no exista en el mismo rango, con el mismo tipo y el mismo tiempo de validez
                foreach (TRM TasaAu in View.Model.ListTRM)
                {
                    //Valido que no compare el rango con el mismo
                    if (
                        ((TasaAu.FechaFinal <= View.DTP_FechaInicial.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicial.SelectedDate) ||
                            (TasaAu.FechaInicial <= View.DTP_FechaFinal.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaFinal.SelectedDate) ||
                            (TasaAu.FechaInicial >= View.DTP_FechaInicial.SelectedDate && TasaAu.FechaFinal <= View.DTP_FechaFinal.SelectedDate) ||
                            (TasaAu.FechaInicial <= View.DTP_FechaInicial.SelectedDate && TasaAu.FechaFinal >= View.DTP_FechaInicial.SelectedDate)))
                    {
                        mensajeError = "No se puede crear el nuevo TRM porque hay uno activo. Por favor revisar";
                    }
                }
            }
            return mensajeError;
        }

        public void onBuscar(object sender, EventArgs e)
        {

            bool fechaFinal = string.IsNullOrEmpty(View.DTP_FechaFinalB.Text);
            bool valorBusqueda = string.IsNullOrEmpty(View.TXT_ValorBusqueda.Text);
            bool fechaInicial = string.IsNullOrEmpty(View.DTP_FechaInicialB.Text);
            if (!fechaFinal || !fechaInicial || !valorBusqueda)
            {
                DateTime FechaInicialVista =  String.IsNullOrEmpty(View.DTP_FechaInicialB.Text)? DateTime.MinValue : DateTime.Parse(View.DTP_FechaInicialB.Text);
                DateTime FechaFinalVista = String.IsNullOrEmpty(View.DTP_FechaFinalB.Text) ? DateTime.MinValue : DateTime.Parse(View.DTP_FechaFinalB.Text);
                double ValorVista = String.IsNullOrEmpty(View.TXT_ValorBusqueda.Text) ? 0 : double.Parse(View.TXT_ValorBusqueda.Text);
                //buscar por fecha inicial
                if (fechaFinal && valorBusqueda)
                {
                    //  View.Model.ListTRM = service.GetTRM(new TRM { FechaInicial = DateTime.Parse(View.DTP_FechaInicialB.Text) });
                    View.Model.ListTRM = db.TRM.Where(f=> f.FechaInicial.Value == FechaInicialVista).ToList();
                }
                //buscar por fecha final
                else if (fechaInicial && valorBusqueda)
                {
                   // View.Model.ListTRM = service.GetTRM(new TRM { FechaFinal = DateTime.Parse(View.DTP_FechaFinalB.Text) });
                    View.Model.ListTRM = db.TRM.Where(f => f.FechaFinal.Value == FechaFinalVista).ToList();

                }
                //buscar por valor
                else if (fechaInicial && fechaFinal)
                {
                    //View.Model.ListTRM = service.GetTRM(new TRM { Valor = double.Parse(View.TXT_ValorBusqueda.Text) });
                    View.Model.ListTRM = db.TRM.Where(f => f.Valor.Value == ValorVista).ToList();

                }
                //buscar por fecha inicial y final
                else if (valorBusqueda)
                {
                    // View.Model.ListTRM = service.GetTRM(new TRM { FechaFinal = DateTime.Parse(View.DTP_FechaFinalB.Text), FechaInicial = DateTime.Parse(View.DTP_FechaInicialB.Text) });
                    View.Model.ListTRM = db.TRM.Where(f=> f.FechaFinal.Value == FechaFinalVista && f.FechaInicial.Value == FechaInicialVista).ToList();

                }
                //buscar por fecha inicial y valor
                else if (fechaFinal)
                {
                    // View.Model.ListTRM = service.GetTRM(new TRM { FechaInicial = DateTime.Parse(View.DTP_FechaInicialB.Text), Valor = double.Parse(View.TXT_ValorBusqueda.Text) });
                    View.Model.ListTRM = db.TRM.Where(f=>f.FechaInicial.Value == FechaInicialVista && f.Valor.Value== ValorVista).ToList();
                }
                //buscar por fecha final y valor
                else if (fechaInicial)
                {
                   // View.Model.ListTRM = service.GetTRM(new TRM { Valor = double.Parse(View.TXT_ValorBusqueda.Text), FechaFinal = DateTime.Parse(View.DTP_FechaFinalB.Text) });
                    View.Model.ListTRM = db.TRM.Where(f=>f.Valor.Value == ValorVista && f.FechaFinal == FechaFinalVista).ToList();
                }
                //buscar por todos
                else
                {
                    // View.Model.ListTRM = service.GetTRM(new TRM { Valor = double.Parse(View.TXT_ValorBusqueda.Text), FechaFinal = DateTime.Parse(View.DTP_FechaFinalB.Text), FechaInicial = DateTime.Parse(View.DTP_FechaInicialB.Text) });
                    View.Model.ListTRM = db.TRM.Where(f => f.Valor.Value == ValorVista && f.FechaFinal== FechaFinalVista && f.FechaInicial== FechaInicialVista).ToList();
                }
            }
            else
            {
                this.actualizarListaTRM();
            }
        }

        public void onSeleccionar(object sender, DataEventArgs<TRM> trm)
        {
            View.Model.TRM = trm.Value; //Cargo los datos en el panel de crear registro
            this.controlarPanelNuevoRegistro(true);
        }

        public void onNuevoRegistro(object sender, EventArgs e)
        {
            this.controlarPanelNuevoRegistro(true);
            this.CleanToCreate();
        }

        public void onActualizar(object sender, EventArgs e)
        {
            this.actualizarListaTRM();
            this.CleanToRefresh();
        }

        /**
       * Actualiza la lista de TRM 
       * */
        public void actualizarListaTRM()
        {
            //View.Model.ListTRM = service.GetTRM(new TRM { });
            View.Model.ListTRM = db.TRM.ToList();

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

            View.Model.TRM = null;
            View.DTP_FechaFinal.Text = "";
            View.DTP_FechaInicial.Text = "";
            View.TXT_Valor.Text = "";
        }

        public void CleanToRefresh()
        {

            View.DTP_FechaFinalB.Text = "";
            View.DTP_FechaInicialB.Text = "";
            View.TXT_ValorBusqueda.Text = "";
            View.Model.TRM = null;
        }

        #endregion
    }
}