using System;
using WpfFront.Model;
using WpfFront.Vista;
using Assergs.Windows;
using WMComposite.Events;
using Microsoft.Practices.Unity;
using WpfFront.Common;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using WpfFront.Modelo;


namespace WpfFront.Controlador
{

    public interface IListaOperacionesPresenter
    {
        IListaOperacionesView View { get; set; }
        ToolWindow Window { get; set; }
    }

    public class ListaOperacionesPresenter : IListaOperacionesPresenter
    {
        public IListaOperacionesView View { get; set; }
        private readonly IUnityContainer container;
        private wmsEntities db;
        public ToolWindow Window { get; set; }
        //Variables Auxiliares 
        //public Object PresenterParent { get; set; }

        public ListaOperacionesPresenter(IUnityContainer container, IListaOperacionesView view)
        {
            View = view;
            this.container = container;
            this.db = new wmsEntities();
            View.Model = this.container.Resolve<ListaOperacionesModel>();
            view.Model.RecordBusqueda = new Operacion();

            #region Metodos

            view.BuscarVuelos += this.BuscarVuelos;
            view.NuevoVuelo += this.OnNuevoVuelo;
            view.CargarDocumentoVuelo += this.OnCargarDocumento;
            view.ActualizarVuelos += this.OnActualizarVuelos;

            #endregion

            #region Datos

            //Obtengo la conexion

            //Inicio las variables
            View.Model.ListaOperaciones = db.Operacion.Take(200).ToList();
            view.Model.ListaTipoFacturacion = db.Tipo.Where(f => f.Agrupacion.Codigo == "TIPOFACTURACION" && f.Activo == true).ToList();
            view.Model.ListaTipoOp = db.Tipo.Where(f => f.Agrupacion.Codigo == "TIPOVUELOLS" && f.Activo == true).ToList();


            #endregion
        }

        #region Metodos

        public void OnActualizarVuelos(Object sender, EventArgs e)
        {
            View.Model.ListaOperaciones = db.Operacion.Take(200).ToList();
            this.CleanToRefresh();

        }

        public void BuscarVuelos(Object sender, EventArgs e)
        {
            //Consulto Todo
            View.Model.ListaOperaciones = db.Operacion.ToList();

            //Inicio de Filtros
            if (View.TipoFactura.SelectedItem != null)
            {
                View.Model.ListaOperaciones = View.Model.ListaOperaciones.Where(f => f.TipoFacturacionID == ((Tipo)(View.TipoFactura.SelectedItem)).RowID).ToList();
            }

            if (View.SearchTercero.Terceros != null)
            {
                View.Model.ListaOperaciones = View.Model.ListaOperaciones.Where(f => f.Aeronave.CompañiaFacturaID == View.SearchTercero.Terceros.RowID).ToList();
            }

            if (View.SearchAeronave.Aeronaves != null)
            {
                View.Model.ListaOperaciones = View.Model.ListaOperaciones.Where(f => f.Aeronave.RowID == View.SearchAeronave.Aeronaves.RowID).ToList();
            }

            if (View.TipoOP.SelectedItem != null) //Si selecciono salida
            {
                if (((Tipo)(View.TipoOP.SelectedItem)).Codigo == "SALIDA")
                {
                    if (View.SearchAeropuerto.Aeropuertos != null)
                    {
                        View.Model.ListaOperaciones = View.Model.ListaOperaciones.Where(f => f.Salida.DestinoID == View.SearchAeropuerto.Aeropuertos.RowID).ToList();
                    }

                    if (!string.IsNullOrEmpty(View.NumVuelo.Text) && View.NumVuelo.Text != 0.ToString())
                    {
                        View.Model.ListaOperaciones = View.Model.ListaOperaciones.Where(f => f.Salida.NVueloSalida.StartsWith(View.NumVuelo.Text)).ToList();
                    }

                    return;
                }
            }
            //Si esta vacio o es llegada
           
            if (View.SearchAeropuerto.Aeropuertos != null)
            {
                View.Model.ListaOperaciones = View.Model.ListaOperaciones.Where(f => f.Llegada.OrigenID == View.SearchAeropuerto.Aeropuertos.RowID).ToList();
            }

            if (!string.IsNullOrEmpty(View.NumVuelo.Text) && View.NumVuelo.Text != 0.ToString())
            {
                View.Model.ListaOperaciones = View.Model.ListaOperaciones.Where(f => f.Llegada.NVuelo.StartsWith(View.NumVuelo.Text)).ToList();
            }
        }

        public void OnNuevoVuelo(Object sender, EventArgs e)
        {
            Operacion NewRecord = new Operacion();
            CargarDocumento(NewRecord);
        }


        private void OnCargarDocumento(object sender, DataEventArgs<Operacion> e)
        {
            CargarDocumento(e.Value);
        }

        public void CargarDocumento(Operacion RegistroVuelos)
        {
            if (RegistroVuelos != null)
            {
                //Variables Auxiliares
                TabItem NewTabItemPedido;
                IOperacionesPresenter ServicioPresenter;
                String nombreTab = "Registro de vuelo # ";
                if (RegistroVuelos.Llegada != null)
                { //Para que le muestre en el tab el Nro de Matricula
                    if (!string.IsNullOrEmpty(RegistroVuelos.Llegada.NVuelo))
                    {
                        nombreTab = nombreTab + RegistroVuelos.Llegada.NVuelo;
                    }
                    else
                    { nombreTab = nombreTab + RegistroVuelos.RowID; }
                }
                else
                { nombreTab = nombreTab + RegistroVuelos.RowID; }

                try
                {
                    //Creo los datos para el nuevo Tab
                    NewTabItemPedido = new TabItem
                    {
                        Header = nombreTab,
                        Name = "Tab_" + RegistroVuelos.RowID,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };

                    //Creo los datos para el UserControl que me controla los TakeOff
                    ServicioPresenter = container.Resolve<OperacionesPresenter>();

                    //Inicializo los datos del documento a cargar
                    ServicioPresenter.CargarDocumento(RegistroVuelos, this);

                    //Adiciono al Tab el StackPanel del TakeOff
                    NewTabItemPedido.Content = ServicioPresenter.View;

                    //Adiciono el nuevo Tab a la vista
                    View.TabPadre.Items.Add(NewTabItemPedido);

                    //Selecciono por defecto el nuevo Tab
                    NewTabItemPedido.Focus();
                    ServicioPresenter.View.TipoFactura.Focus();
                }
                catch (Exception Ex)
                {
                    Util.ShowError("Error cargando el documento. Error: " + Ex);
                }
            }
        }
        /// <summary>
        /// Limpio los controles de busqueda al actualizar
        /// </summary>
        public void CleanToRefresh()
        {
            View.SearchAeropuerto.Aeropuertos = new Aeropuerto();
            View.TipoFactura.SelectedIndex = -1;
            View.SearchAeropuerto.cargarValorEspecifico("");
            View.SearchTercero.Terceros = new Tercero();
            View.SearchTercero.txtDescripcion.Text = "";
            View.SearchTercero.txtData.Text = "";
            View.NumVuelo.Text = 0.ToString();
            View.SearchAeronave.Aeronaves = new Aeronave();
            View.SearchAeronave.cargarValorEspecifico("", "");
            View.FechaVuelo.Text = "";
            View.TipoOP.SelectedIndex = -1;
        }

        #endregion
    }
}