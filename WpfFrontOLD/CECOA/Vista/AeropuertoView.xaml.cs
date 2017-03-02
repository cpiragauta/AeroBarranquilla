using System;
using System.Windows.Controls;
using Core.WPF;
using WMComposite.Events;
using System.Windows;
using System.Windows.Input;
using WpfFront.Model;
using WpfFront.Modelo;

namespace WpfFront.Vista
{
    public partial class AeropuertoView : UserControlBase, IAeropuertoView
    {

        #region Definicion Metodos
        public event EventHandler<EventArgs> BuscarAeropuerto;
        public event EventHandler<DataEventArgs<Aeropuerto>> SeleccionarAeropuerto;
        public event EventHandler<EventArgs> AgregarAeropuerto;
        public event EventHandler<EventArgs> NuevoRegistro;
        public event EventHandler<EventArgs> ActualizarListaAeropuerto;
        #endregion

        public AeropuertoView()
        {
            InitializeComponent();
        }

        public AeropuertoModel Model
        {
            get
            { return this.DataContext as AeropuertoModel; }
            set
            { this.DataContext = value; }
        }


        #region Declaracion Variables
        public TextBox TXT_BuscarPais
        {
            get { return this.txt_Pais; }
            set { this.txt_Pais = value; }
        }
        public TextBox TXT_Pais
        {
            get { return this.txt_PaisR; }
            set { this.txt_PaisR = value; }
        }
        public TextBox TXT_BuscarSigla
        {
            get { return this.txt_Sigla; }
            set { this.txt_Sigla = value; }
        }
        public TextBox TXT_Sigla
        {
            get { return this.txt_SiglaIATAR; }
            set { this.txt_SiglaIATAR = value; }
        }
        public TextBox TXT_BuscarCiudad
        {
            get { return this.txt_Ciudad; }
            set { this.txt_Ciudad = value; }
        }
        public TextBox TXT_Ciudad
        {
            get { return this.txt_CiudadR; }
            set { this.txt_CiudadR = value; }
        }
        public TextBox TXT_Nombre
        {
            get { return this.txt_Nombre; }
            set { this.txt_Nombre = value; }
        }
        public TextBox TXT_SiglaOACI
        {
            get { return this.txt_SiglaOACI; }
            set { this.txt_SiglaOACI = value; }
        }
        public TextBox TXT_BuscarNombre
        {
            get { return this.txt_filtroNombre; }
            set { this.txt_filtroNombre = value; }
        }
        public TextBox TXT_BuscarSiglaOACI
        {
            get { return this.txt_FiltroSiglaOACI; }
            set { this.txt_FiltroSiglaOACI = value; }
        }
        public GroupBox PanelNuevoRegistro
        {
            get { return this.gb_NuevoRegistro; }
            set { this.gb_NuevoRegistro = value; }
        }
        /// <summary>
        ///ComboBox que toma valores de Nacional e Internacional
        /// </summary>
        public ComboBox cbxTipoAeropuerto
        {
            get { return this.cmb_TipoAeropuerto; }
            set { this.cmb_TipoAeropuerto = value; }
        }

        public ComboBox cbxFiltrarTipoAeropuerto
        {
            get { return this.cmb_FiltrarTipoAeropuerto; }
            set { this.cmb_FiltrarTipoAeropuerto = value; }
        }

        #endregion

        #region Declaracion Metodos
        private void btnAgregarAerpuerto_Click_1(object sender, RoutedEventArgs e)
        {
            AgregarAeropuerto(sender, e);
        }
        private void ListAeropuertos_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            SeleccionarAeropuerto(sender, new DataEventArgs<Aeropuerto>((Aeropuerto)ListAeropuertos.SelectedItem));
        }
        private void btnBuscarAerpuertos_Click_1(object sender, RoutedEventArgs e)
        {
            BuscarAeropuerto(sender, e);
        }
        private void btnNuevoRegistro_Click_1(object sender, RoutedEventArgs e)
        {
            NuevoRegistro(sender, e);
        }
        #endregion

        private void btnActualizarLista_Click_1(object sender, RoutedEventArgs e)
        {
            ActualizarListaAeropuerto(sender, e);
        }
    }

    public interface IAeropuertoView
    {
        //Clase Modelo
        AeropuertoModel Model { get; set; }

        #region Definicion Variables
        TextBox TXT_BuscarSigla { get; set; }
        TextBox TXT_Sigla { get; set; }
        TextBox TXT_BuscarCiudad { get; set; }
        TextBox TXT_Ciudad { get; set; }
        TextBox TXT_BuscarPais { get; set; }
        TextBox TXT_Pais { get; set; }
        TextBox TXT_Nombre { get; set; }
        TextBox TXT_BuscarNombre { get; set; }
        TextBox TXT_SiglaOACI { get; set; }
        TextBox TXT_BuscarSiglaOACI { get; set; }
        GroupBox PanelNuevoRegistro { get; set; }
        ComboBox cbxTipoAeropuerto { get; set; }
        ComboBox cbxFiltrarTipoAeropuerto { get; set; }

        #endregion

        #region Definicion Metodos
        event EventHandler<EventArgs> BuscarAeropuerto;
        event EventHandler<EventArgs> AgregarAeropuerto;
        event EventHandler<DataEventArgs<Aeropuerto>> SeleccionarAeropuerto;
        event EventHandler<EventArgs> NuevoRegistro;
        event EventHandler<EventArgs> ActualizarListaAeropuerto;

        #endregion

    }
}