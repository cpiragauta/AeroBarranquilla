using System;
using System.Windows.Controls;
using Core.WPF;
using WMComposite.Events;
using System.Windows;
using System.Windows.Input;
using WpfFront.Model;
using WpfFront.Modelo;
using WpfFront.Controles;

namespace WpfFront.Vista
{
    /// <summary>

    /// </summary>
    public partial class ListaOperacionesView : UserControlBase, IListaOperacionesView
    {

        #region Metodos

        public event EventHandler<EventArgs> BuscarVuelos;
        public event EventHandler<EventArgs> ActualizarVuelos;
        public event EventHandler<EventArgs> NuevoVuelo;
        public event EventHandler<DataEventArgs<Operacion>> CargarDocumentoVuelo;

        #endregion

        public ListaOperacionesView()
        {
            InitializeComponent();
        }

        public ListaOperacionesModel Model
        {
            get
            { return this.DataContext as ListaOperacionesModel; }
            set
            { this.DataContext = value; }
        }


        #region Variables

        public TabControl TabPadre
        {
            get { return this.tabMenu; }
            set { this.tabMenu = value; }
        }

        public SearchAeropuertos SearchAeropuerto
        {
            get { return this.SearchAeropuertoOrigen; }
            set { this.SearchAeropuertoOrigen = value; }
        }

        public SearchAeronaves SearchAeronave
        {
            get { return this.SearchAeronaves; }
            set { this.SearchAeronaves = value; }
        }

        public SearchTerceros SearchTercero
        {
            get { return this.SearchTerceros; }
            set { this.SearchTerceros = value; }
        }

        public TextBox NumVuelo
        {
            get { return this.txt_Vuelo; }
            set { this.txt_Vuelo = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaVuelo
        {
            get { return this.DTP_FechaLlegada; }
            set { this.DTP_FechaLlegada = value; }
        }

        public ComboBox TipoFactura
        {
            get { return this.cmb_TipoFactura; }
            set { this.cmb_TipoFactura = value; }
        }
        public ComboBox TipoOP
        {
            get { return this.cmb_TipoOP; }
            set { this.cmb_TipoOP = value; }
        }

        
        


        //SearchTerceros

        #endregion

        #region Metodos

        /// <summary>
        /// Controla que solo se digiten numeros en el campo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void soloNumerosKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Decimal || e.Key == Key.Tab || e.Key == Key.OemPeriod)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void ListadoVuelos_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            CargarDocumentoVuelo(sender, new DataEventArgs<Operacion>((Operacion)ListadoVuelos.SelectedItem));
        }

        private void btnBuscarVuelos_Click_1(object sender, RoutedEventArgs e)
        {
            BuscarVuelos(sender, e);
        }

        private void btnNuevoRegistro_Click_1(object sender, RoutedEventArgs e)
        {
            NuevoVuelo(sender, e);
        }

        #endregion

        private void btnActualizarLista_Click_1(object sender, RoutedEventArgs e)
        {
            ActualizarVuelos(sender, e);
        }

        private void SearchAeropuertoOrigen_OnSelected_1(object sender, EventArgs e)
        {

        }

        private void cmb_TipoOP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_TipoOP.SelectedItem != null)
            {
                if(((Tipo)(cmb_TipoOP.SelectedItem)).Codigo =="LLEGADA"){
                    LblNroVuelo.Text = "Nro Vuelo L:";
                    LblOrigen.Text = "Origen:";
                    LblFechaLlegada.Text = "Fecha Llegada:";
                }else{
                    LblNroVuelo.Text = "Nro Vuelo S:";
                    LblOrigen.Text = "Destino:";
                    LblFechaLlegada.Text = "Fecha Salida:";
                }
            }
            else
            {
                LblNroVuelo.Text = "Nro Vuelo L:";
                LblOrigen.Text = "Origen:";
                LblFechaLlegada.Text = "Fecha Llegada:";
            }
        }

    }

    public interface IListaOperacionesView
    {
        //Clase Modelo
        ListaOperacionesModel Model { get; set; }

        #region Variables

        TabControl TabPadre { get; set; }
        SearchAeropuertos SearchAeropuerto { get; set; }
        SearchAeronaves SearchAeronave { get; set; }
        SearchTerceros SearchTercero { get; set; }
        TextBox NumVuelo { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaVuelo { get; set; }
        ComboBox TipoFactura { get; set; }
        ComboBox TipoOP { get; set; }

        #endregion

        #region Metodos
        event EventHandler<DataEventArgs<Operacion>> CargarDocumentoVuelo;
        event EventHandler<EventArgs> BuscarVuelos;
        event EventHandler<EventArgs> NuevoVuelo;
        event EventHandler<EventArgs> ActualizarVuelos;

        #endregion

    }
}