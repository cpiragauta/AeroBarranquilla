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
    /// <summary>

    /// </summary>
    public partial class TRMView : UserControlBase, ITRMView
    {

        #region Definicion Metodos
        public event EventHandler<EventArgs> BuscarTRM;
        public event EventHandler<DataEventArgs<TRM>> SeleccionarTRM;
        public event EventHandler<EventArgs> AgregarTRM;
        public event EventHandler<EventArgs> NuevoRegistroTRM;
        public event EventHandler<EventArgs> ActualizarTRM;

        #endregion

        public TRMView()
        {
            InitializeComponent();
        }

        public TRMModel Model
        {
            get
            { return this.DataContext as TRMModel; }
            set
            { this.DataContext = value; }
        }
        #region Definicion Variables


        public Microsoft.Windows.Controls.DatePicker DTP_FechaInicialB
        {
            get { return this.DTP_FechaInicialBusqueda; }
            set { this.DTP_FechaInicialBusqueda = value; }
        }
        public Microsoft.Windows.Controls.DatePicker DTP_FechaFinalB
        {
            get
            {
                // string fecha = string.Format(DTP_FechaFinalBusqueda.ToString(), "MM-dd-yyyy");
                return this.DTP_FechaFinalBusqueda;
            }
            // return string.Format(DTP_FechaFinalBusqueda.ToString(), "MM-dd-yyyy");
            set { this.DTP_FechaFinalBusqueda = value; }
        }
        public Microsoft.Windows.Controls.DatePicker DTP_FechaInicial
        {
            get { return this.DTP_FechaInicial1; }
            set { this.DTP_FechaInicial1 = value; }
        }
        public Microsoft.Windows.Controls.DatePicker DTP_FechaFinal
        {
            get { return this.DTP_FechaFinal1; }
            set { this.DTP_FechaFinal1 = value; }
        }

        public TextBox TXT_ValorBusqueda
        {
            get { return this.txt_Valor; }
            set { this.txt_Valor = value; }
        }
        public TextBox TXT_Valor
        {
            get { return this.txt_ValorR; }
            set { this.txt_ValorR = value; }
        }
        public GroupBox PanelNuevoRegistro
        {
            get { return this.gb_NuevoRegistro; }
            set { this.gb_NuevoRegistro = value; }
        }

        #endregion

        #region Declaracion Metodos
        private void ListTRMs_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            SeleccionarTRM(sender, new DataEventArgs<TRM>((TRM)ListTRMs.SelectedItem));
        }
        private void btnBuscarTRM_Click(object sender, RoutedEventArgs e)
        {
            BuscarTRM(sender, e);
        }
        private void btnAgregarTRM_Click(object sender, RoutedEventArgs e)
        {
            AgregarTRM(sender, e);
        }
        private void btnActualizarLista_Click_1(object sender, RoutedEventArgs e)
        {
            ActualizarTRM(sender, e);
        }
        private void btnNuevoRegistro_Click(object sender, EventArgs e)
        {
            NuevoRegistroTRM(sender, e);
        }


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

        #endregion

        private void ListTRMs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public interface ITRMView
    {
        //Clase Modelo
        TRMModel Model { get; set; }

        #region Variables
        Microsoft.Windows.Controls.DatePicker DTP_FechaInicialB { get; set; }
        Microsoft.Windows.Controls.DatePicker DTP_FechaFinalB { get; set; }
        Microsoft.Windows.Controls.DatePicker DTP_FechaInicial { get; set; }
        Microsoft.Windows.Controls.DatePicker DTP_FechaFinal { get; set; }
        TextBox TXT_Valor { get; set; }
        TextBox TXT_ValorBusqueda { get; set; }
        GroupBox PanelNuevoRegistro { get; set; }
        #endregion

        #region Metodos
        event EventHandler<EventArgs> BuscarTRM;
        event EventHandler<DataEventArgs<TRM>> SeleccionarTRM;
        event EventHandler<EventArgs> AgregarTRM;
        event EventHandler<EventArgs> NuevoRegistroTRM;
        event EventHandler<EventArgs> ActualizarTRM;

        #endregion

    }
}