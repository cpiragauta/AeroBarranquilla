using System;
using System.Windows.Controls;
using Core.WPF;
using WMComposite.Events;
using System.Windows;
using WpfFront.Modelo;
using WpfFront.Model;
using System.Windows.Input;
using WpfFront.Controles;




namespace WpfFront.Vista
{
    /// <summary>
    
    /// </summary>
    public partial class ListaCarnetsView : UserControlBase, IListaCarnetsView
    {

        #region Definicion Metodos
        public event EventHandler<EventArgs> BuscarCarnets;
        public event EventHandler<EventArgs> NuevoCarnet;
        public event EventHandler<DataEventArgs<Solicitud>> CargarCarnet;
        public event EventHandler<EventArgs> ActualizarLista;
        #endregion

        public ListaCarnetsView()
        {
            InitializeComponent();
        }

        public ListaCarnetsModel Model
        {
            get
            { return this.DataContext as ListaCarnetsModel; }
            set
            { this.DataContext = value; }
        }

        #region Variables

        public TextBox NoDocumento_Placa
        { get { return this.txt_NoDocumento_Placa; }
            set { this.txt_NoDocumento_Placa = value; }
        }

        public TextBox Nombres_Marca
        {
            get { return this.txt_Nombres_Marca; }
            set { this.txt_Nombres_Marca = value; }
        }

        public SearchTerceros TXT_Terceros
        {
            get { return this.TerceroSeleccionado; }
            set { this.TerceroSeleccionado = value; }
        }
        public TabControl TabPadre
        {
            get { return this.tabMenu; }
            set { this.tabMenu = value; }
        }
        #endregion

        #region Metodos

        private void btnBuscarCarnets_Click_1(object sender, RoutedEventArgs e)
        {
            BuscarCarnets(sender, e);
        }
        
        private void SearchTerceros_OnSelected_1(object sender, EventArgs e)
        {
           
        }

        private void ListadoCarnets_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (ListadoCarnets.SelectedItem != null)
            {
            CargarCarnet(sender, new DataEventArgs<Solicitud>((Solicitud)ListadoCarnets.SelectedItem));
            }
        }

        private void btnNuevoRegistro_Click_1(object sender, RoutedEventArgs e)
        {
            NuevoCarnet(sender, e);
        }
        private void btnActualizarLista_Click_1(object sender, RoutedEventArgs e)
        {
            ActualizarLista(sender, e);
        }

        #endregion

       

  
    }

    public interface IListaCarnetsView
    {
        //Clase Modelo
        ListaCarnetsModel Model { get; set; }

        #region Variables
        TextBox Nombres_Marca { get; set; }
        TextBox NoDocumento_Placa { get; set; }
        SearchTerceros TXT_Terceros { get; set; }
        TabControl TabPadre { get; set; }
        #endregion

        #region Metodos
        event EventHandler<EventArgs> BuscarCarnets;
        event EventHandler<EventArgs> NuevoCarnet;
        event EventHandler<DataEventArgs<Solicitud>> CargarCarnet;
        event EventHandler<EventArgs> ActualizarLista;

        #endregion

    }
}