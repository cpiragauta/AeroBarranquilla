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

    public interface IListaCarnetsPresenter
    {
        IListaCarnetsView View { get; set; }
        ToolWindow Window { get; set; }
    }

   
    public class ListaCarnetsPresenter : IListaCarnetsPresenter
    {

        wmsEntities  db = new wmsEntities();
        public IListaCarnetsView View { get; set; }
        private readonly IUnityContainer container;
       // private readonly WMSServiceClient service;
        public ToolWindow Window { get; set; }
        //Variables Auxiliares 
      //  public Connection Local;
        //public Object PresenterParent { get; set; }

        public ListaCarnetsPresenter(IUnityContainer container, IListaCarnetsView view)
        {
            View = view;
            this.container = container;
           // this.service = new WMSServiceClient();
            View.Model = this.container.Resolve<ListaCarnetsModel>();

            #region Metodos
            view.BuscarCarnets += this.onBuscarCarnets;
            view.CargarCarnet += this.onCargarCarnet;
            view.NuevoCarnet += this.onNuevoCarnet;
            view.ActualizarLista += this.onActualizarListaCarnet;

            #endregion

            #region Datos

            //View.Model.EntityList = new  IList<CarnetBase>();
            //Inicio las variables
            //View.Model.EntityList = service.GetCarnetBase(new CarnetBase()).Where(f => !String.IsNullOrEmpty(f.NoFactura)).ToList();
            //  View.Model.EntityList = service.GetSolicitud(new Solicitud { });
            View.Model.EntityList = db.Solicitud.ToList();
            // View.Model.EntityList = service.GetCarnetBase(new CarnetBase ()).Where(f => !String.IsNullOrEmpty(f.NoFactura)).ToList();
            //view.Model.ListAeropuertos = service.GetAeropuertos(new Aeropuertos { }).ToList();
            #endregion
        }

        #region Metodos
        public void onBuscarCarnets(object sender, EventArgs e)
        {
            // View.Model.EntityList = service.GetSolicitud(new Solicitud { NoDocumento_Placa = View.NoDocumento_Placa.Text, Nombres_Marca = View.Nombres_Marca.Text }).ToList();
            View.Model.EntityList = db.Solicitud.Where(f=>f.NoDocumento_Placa == View.NoDocumento_Placa.Text && f.Nombres_Marca == View.Nombres_Marca.Text).ToList();

            return;
        }

        private void onCargarCarnet(object sender, DataEventArgs<Solicitud> e)
        {
            CargarCarnet(e.Value.Encabezado);
        }

        private void onNuevoCarnet(object sender, EventArgs e)
        {
            CargarCarnet(new Encabezado());
        }

        private void onActualizarListaCarnet(object sender, EventArgs e)
        {
            this.ActualizarListaCarnets();
        }

        public void ActualizarListaCarnets()
        {
            //View.Model.EntityList = service.GetSolicitud(new Solicitud { });
            View.Model.EntityList = db.Solicitud.ToList();
            View.NoDocumento_Placa.Text = "";
            View.Nombres_Marca.Text = "";
            View.TXT_Terceros.Terceros = new Tercero();
        }

        public void CargarCarnet(Encabezado Carnet)
        {
            //Variables Auxiliares
            TabItem NewTabItemPedido;
            IDatosCarnetsPresenter ServicioPresenter;

            try
            {
                //Creo los datos para el nuevo Tab
                NewTabItemPedido = new TabItem
                {
                    Header = "Carnet # " + Carnet.RowID,
                    Name = "Tab_" + Carnet.RowID,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };

                //Creo los datos para el UserControl que me controla los TakeOff
                ServicioPresenter = container.Resolve<DatosCarnetsPresenter>();

                //Inicializo los datos del documento a cargar
                ServicioPresenter.CargarCarnet(Carnet, this);

                //Adiciono al Tab el StackPanel del TakeOff
                NewTabItemPedido.Content = ServicioPresenter.View;

                //Adiciono el nuevo Tab a la vista
                View.TabPadre.Items.Add(NewTabItemPedido);

                //Selecciono por defecto el nuevo Tab
                NewTabItemPedido.Focus();
            }
            catch (Exception Ex)
            {
                Util.ShowError("Error cargando el documento. Error: " + Ex);
            }
        }

        #endregion
    }
}