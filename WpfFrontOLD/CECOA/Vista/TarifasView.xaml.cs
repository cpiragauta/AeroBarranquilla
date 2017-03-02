//using System;
//using System.Windows.Controls;
//using Core.WPF;
//using WMComposite.Events;
//using System.Windows;
//using WpfFront.Common;
//using Microsoft.Windows.Controls;
//using WpfFront.Models;
//using System.Windows.Input;
//using System.Data;
//using WpfFront.Common.UserControls;
//using WpfFront.WMSBusinessService;
//using System.Windows.Media;
//using Assergs.Windows.Controls;

//namespace WpfFront.Vista
//{
//    /// <summary>

//    /// </summary>
//    public partial class TarifasView : UserControlBase, ITarifasView
//    {
//        const string AERODROMO = "Aerodromo";
//        const string AEROPORTUARIAS = "Aeroportuarias";
//        const string BOMBEROS = "Bomberos";
//        const string PUENTES = "Puentes";
//        const string PARQUEO = "Parqueo";

//        #region Metodos

//        #region Aerodromo y todos
//        public event EventHandler<EventArgs> BuscarTarifas;
//        public event EventHandler<DataEventArgs<Tarifas>> SeleccionarTarifas;
//        public event EventHandler<EventArgs> AgregarTarifas;
//        public event EventHandler<EventArgs> NuevoRegistro;
//        public event EventHandler<EventArgs> ActualizarListaTarifas;
//        #endregion

//        #endregion

//        public TarifasView()
//        {
//            InitializeComponent();
//        }




//        #region IMPLEMENTACION VARIABLES

//        public TarifasModel Model
//        {
//            get
//            { return this.DataContext as TarifasModel; }
//            set
//            { this.DataContext = value; }
//        }
//        //Variable para controlar internamente el tipo de tarifa que se va a manejar
//        private string _Tipo;
//        public String Tipo
//        {
//            get
//            { return this._Tipo; }
//            set
//            { this._Tipo = value; }
//        }

//        #region Aerodromo

//        public Microsoft.Windows.Controls.DatePicker DTP_FechaInicial
//        {
//            get { return this.DTP_FechaInicial1; }
//            set { this.DTP_FechaInicial1 = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_FechaFinal
//        {
//            get { return this.DTP_FechaFinal1; }
//            set { this.DTP_FechaFinal1 = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaInicial
//        {
//            get { return this.DTP_BuscarFechaInicial1; }
//            set { this.DTP_BuscarFechaInicial1 = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaFinal
//        {
//            get { return this.DTP_BuscarFechaFinal1; }
//            set { this.DTP_BuscarFechaFinal1 = value; }
//        }

//        public TextBox TXT_BuscarValorCOP
//        {
//            get { return this.txt_ValorCOP; }
//            set { this.txt_ValorCOP = value; }
//        }
//        public TextBox TXT_ValorCOP
//        {
//            get { return this.txt_ValorCOP1; }
//            set { this.txt_ValorCOP1 = value; }
//        }
//        public TextBox TXT_BuscarValorUSD
//        {
//            get { return this.txt_ValorUSD; }
//            set { this.txt_ValorUSD = value; }
//        }
//        public TextBox TXT_ValorUSD
//        {
//            get { return this.txt_ValorUSD1; }
//            set { this.txt_ValorUSD1 = value; }
//        }
//        public TextBox TXT_BuscarRecargoCOP
//        {
//            get { return this.txt_RecargoCOP; }
//            set { this.txt_RecargoCOP = value; }
//        }
//        public TextBox TXT_RecargoCOP
//        {
//            get { return this.txt_RecargoCOP1; }
//            set { this.txt_RecargoCOP1 = value; }
//        }
//        public TextBox TXT_RecargoUSD
//        {
//            get { return this.txt_RecargoUSD1; }
//            set { this.txt_RecargoUSD1 = value; }
//        }
//        public TextBox TXT_BuscarRecargoUSD
//        {
//            get { return this.txt_RecargoUSD; }
//            set { this.txt_RecargoUSD = value; }
//        }
//        public GroupBox PanelNuevoRegistro
//        {
//            get { return this.gb_nuevoRegistro; }
//            set { this.gb_nuevoRegistro = value; }
//        }

//        #endregion

//        #region Aeroportuarias

//        public Microsoft.Windows.Controls.DatePicker DTP_FechaInicialArptrs
//        {
//            get { return this.DTP_FechaInicial1Arptrs; }
//            set { this.DTP_FechaInicial1Arptrs = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_FechaFinalArptrs
//        {
//            get { return this.DTP_FechaFinal1Arptrs; }
//            set { this.DTP_FechaFinal1Arptrs = value; }
//        }

//        public Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaInicialArptrs
//        {
//            get { return this.DTP_BuscarFechaInicial1Arptrs; }
//            set { this.DTP_BuscarFechaInicial1Arptrs = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaFinalArptrs
//        {
//            get { return this.DTP_BuscarFechaFinal1Arptrs; }
//            set { this.DTP_BuscarFechaFinal1Arptrs = value; }
//        }
//        public TextBox TXT_BuscarValorCOPArptrs
//        {
//            get { return this.txt_ValorCOPArptrs; }
//            set { this.txt_ValorCOPArptrs = value; }
//        }
//        public TextBox TXT_ValorCOPArptrs
//        {
//            get { return this.txt_ValorCOP1Arptrs; }
//            set { this.txt_ValorCOP1Arptrs = value; }
//        }
//        public TextBox TXT_BuscarValorUSDArptrs
//        {
//            get { return this.txt_ValorUSDArptrs; }
//            set { this.txt_ValorUSDArptrs = value; }
//        }
//        public TextBox TXT_ValorUSDArptrs
//        {
//            get { return this.txt_ValorUSD1Arptrs; }
//            set { this.txt_ValorUSD1Arptrs = value; }
//        }
//        public GroupBox PanelNuevoRegistroArptrs
//        {
//            get { return this.gb_nuevoRegistroArptrs; }
//            set { this.gb_nuevoRegistroArptrs = value; }
//        }


//        #endregion

//        #region Bomberos

//        public Microsoft.Windows.Controls.DatePicker DTP_FechaInicialBomber
//        {
//            get { return this.DTP_FechaInicial1Bomber; }
//            set { this.DTP_FechaInicial1Bomber = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_FechaFinalBomber
//        {
//            get { return this.DTP_FechaFinal1Bomber; }
//            set { this.DTP_FechaFinal1Bomber = value; }
//        }

//        public Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaInicialBomber
//        {
//            get { return this.DTP_BuscarFechaInicial1Bomber; }
//            set { this.DTP_BuscarFechaInicial1Bomber = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaFinalBomber
//        {
//            get { return this.DTP_BuscarFechaFinal1Bomber; }
//            set { this.DTP_BuscarFechaFinal1Bomber = value; }
//        }

//        public TextBox TXT_BuscarValorCOPBomber
//        {
//            get { return this.txt_ValorCOPBomber; }
//            set { this.txt_ValorCOPBomber = value; }
//        }
//        public TextBox TXT_ValorCOPBomber
//        {
//            get { return this.txt_ValorCOP1Bomber; }
//            set { this.txt_ValorCOP1Bomber = value; }
//        }
//        public GroupBox PanelNuevoRegistroBomber
//        {
//            get { return this.gb_nuevoRegistroBomber; }
//            set { this.gb_nuevoRegistroBomber = value; }
//        }
//        public ComboBox cbxTipoServicio
//        {
//            get { return this.cbxServicio; }
//            set { this.cbxServicio = value; }
//        }



//        #endregion

//        #region Puentes

//        public Microsoft.Windows.Controls.DatePicker DTP_FechaInicialPuents
//        {
//            get { return this.DTP_FechaInicial1Puents; }
//            set { this.DTP_FechaInicial1Puents = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_FechaFinalPuents
//        {
//            get { return this.DTP_FechaFinal1Puents; }
//            set { this.DTP_FechaFinal1Puents = value; }
//        }

//        public Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaInicialPuents
//        {
//            get { return this.DTP_BuscarFechaInicial1Puents; }
//            set { this.DTP_BuscarFechaInicial1Puents = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaFinalPuents
//        {
//            get { return this.DTP_BuscarFechaFinal1Puents; }
//            set { this.DTP_BuscarFechaFinal1Puents = value; }
//        }
//        public TextBox TXT_BuscarValorCOPPuents
//        {
//            get { return this.txt_ValorCOPPuents; }
//            set { this.txt_ValorCOPPuents = value; }
//        }
//        public TextBox TXT_ValorCOPPuents
//        {
//            get { return this.txt_ValorCOP1Puents; }
//            set { this.txt_ValorCOP1Puents = value; }
//        }
//        public TextBox TXT_BuscarValorUSDPuents
//        {
//            get { return this.txt_ValorUSDPuents; }
//            set { this.txt_ValorUSDPuents = value; }
//        }
//        public TextBox TXT_ValorUSDPuents
//        {
//            get { return this.txt_ValorUSD1Puents; }
//            set { this.txt_ValorUSD1Puents = value; }
//        }
//        public GroupBox PanelNuevoRegistroPuents
//        {
//            get { return this.gb_nuevoRegistroPuents; }
//            set { this.gb_nuevoRegistroPuents = value; }
//        }


//        #endregion

//        #region Parqueos

//        public Microsoft.Windows.Controls.DatePicker DTP_FechaInicialParqos
//        {
//            get { return this.DTP_FechaInicial1Parqos; }
//            set { this.DTP_FechaInicial1Parqos = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_FechaFinalParqos
//        {
//            get { return this.DTP_FechaFinal1Parqos; }
//            set { this.DTP_FechaFinal1Parqos = value; }
//        }

//        public Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaInicialParqos
//        {
//            get { return this.DTP_BuscarFechaInicial1Parqos; }
//            set { this.DTP_BuscarFechaInicial1Parqos = value; }
//        }
//        public Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaFinalParqos
//        {
//            get { return this.DTP_BuscarFechaFinal1Parqos; }
//            set { this.DTP_BuscarFechaFinal1Parqos = value; }
//        }
//        public TextBox TXT_BuscarValorCOPParqos
//        {
//            get { return this.txt_ValorCOPParqos; }
//            set { this.txt_ValorCOPParqos = value; }
//        }
//        public TextBox TXT_ValorCOPParqos
//        {
//            get { return this.txt_ValorCOP1Parqos; }
//            set { this.txt_ValorCOP1Parqos = value; }
//        }
//        public TextBox TXT_BuscarValorUSDParqos
//        {
//            get { return this.txt_ValorUSDParqos; }
//            set { this.txt_ValorUSDParqos = value; }
//        }
//        public TextBox TXT_ValorUSDParqos
//        {
//            get { return this.txt_ValorUSD1Parqos; }
//            set { this.txt_ValorUSD1Parqos = value; }
//        }
//        public GroupBox PanelNuevoRegistroParqos
//        {
//            get { return this.gb_nuevoRegistroParqos; }
//            set { this.gb_nuevoRegistroParqos = value; }
//        }


//        #endregion


//        #endregion

//        #region IMPLEMENTACION METODOS

//        #region Aerodromo
//        /// <summary>
//        /// Controla que solo se digiten numeros en el campo
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void soloNumerosKeyDownEvent(object sender, KeyEventArgs e)
//        {
//            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Decimal || e.Key == Key.Tab || e.Key == Key.OemPeriod)
//            {
//                e.Handled = false;
//            }
//            else
//            {
//                e.Handled = true;
//            }
//        }

//        private void btnGuardarAerpuerto_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = AERODROMO;
//            AgregarTarifas(sender, e);
//        }
//        private void ListTarifass_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
//        {
//            Tipo = AERODROMO;
//            SeleccionarTarifas(sender, new DataEventArgs<Tarifas>((Tarifas)ListTarifas.SelectedItem));
//        }
//        private void btnBuscarAerpuertos_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = AERODROMO;
//            BuscarTarifas(sender, e);
//        }
//        private void btnMostrarAgregar_Click(object sender, RoutedEventArgs e)
//        {
//            Tipo = AERODROMO;
//            NuevoRegistro(sender, e);
//        }
//        private void btnActualizarLista_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = AERODROMO;
//            ActualizarListaTarifas(sender, e);
//        }

//        #endregion

//        #region Aeroportuarias

//        private void btnGuardarAerpuertoArptrs_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = AEROPORTUARIAS;
//            AgregarTarifas(sender, e);
//        }
//        private void ListTarifass_MouseDoubleArptrsClick_1(object sender, MouseButtonEventArgs e)
//        {
//            Tipo = AEROPORTUARIAS;
//            SeleccionarTarifas(sender, new DataEventArgs<Tarifas>((Tarifas)ListTarifasArptrs.SelectedItem));
//        }
//        private void btnBuscarAeropuertosArptrs_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = AEROPORTUARIAS;
//            BuscarTarifas(sender, e);
//        }
//        private void btnMostrarAgregarArptrs_Click(object sender, RoutedEventArgs e)
//        {
//            Tipo = AEROPORTUARIAS;
//            NuevoRegistro(sender, e);
//        }
//        private void btnActualizarListaArptrs_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = AEROPORTUARIAS;
//            ActualizarListaTarifas(sender, e);
//        }

//        #endregion

//        #region Bomberos

//        private void btnGuardarAerpuertoBomber_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = BOMBEROS;
//            AgregarTarifas(sender, e);
//        }
//        private void ListTarifass_MouseDoubleBomberClick_1(object sender, MouseButtonEventArgs e)
//        {
//            Tipo = BOMBEROS;
//            SeleccionarTarifas(sender, new DataEventArgs<Tarifas>((Tarifas)ListTarifasBomber.SelectedItem));
//        }
//        private void btnBuscarAeropuertosBomber_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = BOMBEROS;
//            BuscarTarifas(sender, e);
//        }
//        private void btnMostrarAgregarBomber_Click(object sender, RoutedEventArgs e)
//        {
//            Tipo = BOMBEROS;
//            NuevoRegistro(sender, e);
//        }
//        private void btnActualizarListaBomber_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = BOMBEROS;
//            ActualizarListaTarifas(sender, e);
//        }

//        #endregion

//        #region Puentes

//        private void btnGuardarAerpuertoPuents_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = PUENTES;
//            AgregarTarifas(sender, e);
//        }
//        private void ListTarifass_MouseDoublePuentsClick_1(object sender, MouseButtonEventArgs e)
//        {
//            Tipo = PUENTES;
//            SeleccionarTarifas(sender, new DataEventArgs<Tarifas>((Tarifas)ListTarifasPuents.SelectedItem));
//        }
//        private void btnBuscarAeropuertosPuents_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = PUENTES;
//            BuscarTarifas(sender, e);
//        }
//        private void btnMostrarAgregarPuents_Click(object sender, RoutedEventArgs e)
//        {
//            Tipo = PUENTES;
//            NuevoRegistro(sender, e);
//        }
//        private void btnActualizarListaPuents_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = PUENTES;
//            ActualizarListaTarifas(sender, e);
//        }

//        #endregion

//        #region Parqueo

//        private void btnGuardarAerpuertoParqos_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = PARQUEO;
//            AgregarTarifas(sender, e);
//        }
//        private void ListTarifass_MouseDoubleParqosClick_1(object sender, MouseButtonEventArgs e)
//        {
//            Tipo = PARQUEO;
//            SeleccionarTarifas(sender, new DataEventArgs<Tarifas>((Tarifas)ListTarifasParqos.SelectedItem));
//        }
//        private void btnBuscarAeropuertosParqos_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = PARQUEO;
//            BuscarTarifas(sender, e);
//        }
//        private void btnMostrarAgregarParqos_Click(object sender, RoutedEventArgs e)
//        {
//            Tipo = PARQUEO;
//            NuevoRegistro(sender, e);
//        }
//        private void btnActualizarListaParqos_Click_1(object sender, RoutedEventArgs e)
//        {
//            Tipo = PARQUEO;
//            ActualizarListaTarifas(sender, e);
//        }

//        #endregion


//        #endregion

//    }

//    public interface ITarifasView
//    {

//        #region Definicion VARIABLES

//        //Clase Modelo
//        TarifasModel Model { get; set; }
//        String Tipo { get; set; }

//        #region Aerodromo
//        Microsoft.Windows.Controls.DatePicker DTP_FechaInicial { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_FechaFinal { get; set; }
//        TextBox TXT_BuscarValorCOP { get; set; }
//        TextBox TXT_ValorCOP { get; set; }
//        TextBox TXT_BuscarValorUSD { get; set; }
//        TextBox TXT_ValorUSD { get; set; }
//        TextBox TXT_BuscarRecargoCOP { get; set; }
//        TextBox TXT_RecargoCOP { get; set; }
//        TextBox TXT_BuscarRecargoUSD { get; set; }
//        TextBox TXT_RecargoUSD { get; set; }
//        GroupBox PanelNuevoRegistro { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaInicial { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaFinal { get; set; }
//        #endregion

//        #region Aeroportuarias

//        Microsoft.Windows.Controls.DatePicker DTP_FechaInicialArptrs { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_FechaFinalArptrs { get; set; }
//        TextBox TXT_BuscarValorCOPArptrs { get; set; }
//        TextBox TXT_ValorCOPArptrs { get; set; }
//        TextBox TXT_BuscarValorUSDArptrs { get; set; }
//        TextBox TXT_ValorUSDArptrs { get; set; }
//        GroupBox PanelNuevoRegistroArptrs { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaInicialArptrs { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaFinalArptrs { get; set; }

//        #endregion

//        #region Bomberos

//        Microsoft.Windows.Controls.DatePicker DTP_FechaInicialBomber { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_FechaFinalBomber { get; set; }
//        TextBox TXT_BuscarValorCOPBomber { get; set; }
//        TextBox TXT_ValorCOPBomber { get; set; }
//        GroupBox PanelNuevoRegistroBomber { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaInicialBomber { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaFinalBomber { get; set; }
//        ComboBox cbxTipoServicio { get; set; }


//        #endregion

//        #region Puentes

//        Microsoft.Windows.Controls.DatePicker DTP_FechaInicialPuents { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_FechaFinalPuents { get; set; }
//        TextBox TXT_BuscarValorCOPPuents { get; set; }
//        TextBox TXT_ValorCOPPuents { get; set; }
//        TextBox TXT_BuscarValorUSDPuents { get; set; }
//        TextBox TXT_ValorUSDPuents { get; set; }
//        GroupBox PanelNuevoRegistroPuents { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaInicialPuents { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaFinalPuents { get; set; }

//        #endregion

//        #region Parqueos

//        Microsoft.Windows.Controls.DatePicker DTP_FechaInicialParqos { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_FechaFinalParqos { get; set; }
//        TextBox TXT_BuscarValorCOPParqos { get; set; }
//        TextBox TXT_ValorCOPParqos { get; set; }
//        TextBox TXT_BuscarValorUSDParqos { get; set; }
//        TextBox TXT_ValorUSDParqos { get; set; }
//        GroupBox PanelNuevoRegistroParqos { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaInicialParqos { get; set; }
//        Microsoft.Windows.Controls.DatePicker DTP_BuscarFechaFinalParqos { get; set; }

//        #endregion


//        #endregion

//        #region METODOS

//        #region Aerodromo y Todos

//        event EventHandler<EventArgs> BuscarTarifas;
//        event EventHandler<DataEventArgs<Tarifas>> SeleccionarTarifas;
//        event EventHandler<EventArgs> AgregarTarifas;
//        event EventHandler<EventArgs> NuevoRegistro;
//        event EventHandler<EventArgs> ActualizarListaTarifas;

//        #endregion

//        #endregion

//    }
//}