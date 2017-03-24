using System;
using System.Windows.Controls;
using Core.WPF;
using WMComposite.Events;
using System.Windows;
using WpfFront.Common;
using WpfFront.Modelo;
using System.Windows.Input;
using WpfFront.Model;
using WpfFront.Controles;



namespace WpfFront.Vista
{
    /// <summary>

    /// </summary>
    public partial class AeronavesView : UserControlBase, IAeronavesView
    {

        #region Metodos

        public event EventHandler<EventArgs> BuscarAeronave;
        public event EventHandler<DataEventArgs<Tipo>> CargarDocumentoVuelo;
        public event EventHandler<EventArgs> GuardarAeronave;
        public event EventHandler<EventArgs> NewAeronave;
        public event EventHandler<EventArgs> DeleteAeronave;
        public event EventHandler<DataEventArgs<Aeronave>> CargarAeronave;
        public event EventHandler<EventArgs> NuevoRegistro;
        public event EventHandler<EventArgs> ActualizarListaAeronaves;

        #endregion

        public AeronavesView()
        {
            InitializeComponent();
        }

        public AeronavesModel Model
        {
            get
            { return this.DataContext as AeronavesModel; }
            set
            { this.DataContext = value; }
        }


        #region Variables

      

        public ComboBox ListaTipoOp
        {
            get { return this.CboListaOper; }
            set { this.CboListaOper = value; }
        }

        public ListView ListaAeronaves
        {
            get { return this.ListadoAeronaves; }
            set { this.ListadoAeronaves = value; }
        }

        public TextBox Txt_FiltroMatricula
        {
            get { return this.TxtFiltroMatricula; }
            set { this.TxtFiltroMatricula = value; }
        }
        public SearchTerceros SearchFiltroPropietaria
        {
            get { return this.SearchPropietariaFiltro; }
            set { this.SearchPropietariaFiltro = value; }
        }
        public ComboBox cbx_FiltroTipoOper
        {
            get { return this.CboFiltroTipoOper; }
            set { this.CboFiltroTipoOper = value; }
        }
        public TextBox Txt_FiltroCapacidad
        {
            get { return this.TxtFiltroCapacidad; }
            set { this.TxtFiltroCapacidad = value; }
        }

        //  public DatePickerCommands TxtFecham
        //{
        //    get { return this.Txt_FechaM; }
        //    set { this.Txt_FechaM = value; }
        //}

        public Microsoft.Windows.Controls.DatePicker DTP_FechaM
        {
            get { return this.Txt_FechaM; }
            set { this.Txt_FechaM = value; }
        }

        public GroupBox PanelNuevoRegistro
        {
            get { return this.gb_NuevoRegistro; }
            set { this.gb_NuevoRegistro = value; }
        }

        public SearchTerceros companiaFactura
        {
            get { return this.SearchCompañiaFactura; }
            set { this.SearchCompañiaFactura = value; }
        }

        public SearchTerceros companiaPropietaria
        {
            get { return this.SearchCompañiaPropietaria; }
            set { this.SearchCompañiaPropietaria = value; }
        }

        public StackPanel panelPermisoExplotacion
        {
            get { return this.stkPermisoExplotacion; }
            set { this.stkPermisoExplotacion = value; }
        }

        public StackPanel Panel_Matricula
        {
            get { return this.PanelMatricula; }
            set { this.PanelMatricula = value; }
        }


        #endregion

        #region Metodos


        /// <summary>
        /// Controla que solo se digiten numeros en el campo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void soloNumerosKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Decimal || e.Key == Key.Tab)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btn_Delete_Click_1(object sender, RoutedEventArgs e)
        {
            DeleteAeronave(sender, e);
        }

        private void ListadoAeronaves_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (ListaAeronaves.SelectedItem != null)
                CargarAeronave(sender, new DataEventArgs<Aeronave>((Aeronave)ListaAeronaves.SelectedItem));
        }

        private void btnGuardarRegistro_Click_1(object sender, RoutedEventArgs e)
        {
            //Evaluo que haya sido digitada la matricula
            if (String.IsNullOrEmpty(txtMatricula.Text))
            {
                Util.ShowError("Por favor ingresar la matricula.");
                return;
            }

            //Evaluo que haya sido digitado el Peso
            if (String.IsNullOrEmpty(txtPeso.Text))
            {                
                Util.ShowError("Por favor digitar el PBMO KG.");
                return;
            }

            if (String.IsNullOrEmpty(txtCapPasajeros.Text))
            {
                Util.ShowError("Por favor ingresar Capacidad pasajeros.");
                return;
            }
            if (String.IsNullOrEmpty(txtTipoAeronave.Text))
            {
                Util.ShowError("Por favor ingresar Tipo de Aeronave.");
                return;
            }

            if (companiaPropietaria.Terceros == null)
            {
                Util.ShowError("Por favor seleccinar la Compañia Propietaria.");
                return;
            }

            if (companiaFactura.Terceros == null)
            {
                Util.ShowError("Por favor seleccinar la compañia Factura.");
                return;
            }

            if (ListaTipoOp.SelectedIndex == -1)
            {
                Util.ShowError("Por favor seleccinar el tipo de operacion.");
                return;
            }

            if (chkExtranjera.IsChecked == true && chkPermisoExplotacion.IsChecked == true)
            {
                if (string.IsNullOrEmpty(DTP_FechaM.Text))
                {
                    Util.ShowError("Por favor seleccinar F. Venc Matrícula.");
                    return;
                }
            }
            GuardarAeronave(sender, e);
        }

        private void btnBuscarAeronaves_Click_1(object sender, RoutedEventArgs e)
        {
            BuscarAeronave(sender, e);
        }

        private void btnNuevoRegistro_Click_1(object sender, RoutedEventArgs e)
        {
            //NewAeronave(sender, e);
            NuevoRegistro(sender, e);
        }

        private void btnActualizarLista_Click_1(object sender, RoutedEventArgs e)
        {
            ActualizarListaAeronaves(sender, e);
        }

        private void chkExtranjera_Click_1(object sender, RoutedEventArgs e)
        {
            if (chkExtranjera.IsChecked==true)
            {
                stkPermisoExplotacion.Visibility = Visibility.Visible;
            }
            else
            {
                chkPermisoExplotacion.IsChecked= false;
                stkPermisoExplotacion.Visibility = Visibility.Collapsed;
            }
            if (chkPermisoExplotacion.IsChecked == true)
            {
                PanelMatricula.Visibility = Visibility.Visible;
            }
            else
            {
                PanelMatricula.Visibility = Visibility.Collapsed;
            }
        }

        private void chkPermisoExplotacion_Click_1(object sender, RoutedEventArgs e)
        {
            if (chkPermisoExplotacion.IsChecked == true && chkExtranjera.IsChecked == true)
            {
                PanelMatricula.Visibility = Visibility.Visible;
            }
            else
            {
                PanelMatricula.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        private void txtPeso_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtPeso.Text) && txtPeso.Text != "0")
            {
                double numero = Convert.ToDouble(txtPeso.Text);
                txtPeso.Text = numero.ToString("N0");
                txtPeso.SelectionStart = txtPeso.Text.Length;
            }
        }



    }

    public interface IAeronavesView
    {
        //Clase Modelo
        AeronavesModel Model { get; set; }

        #region Definicion Variables
        ComboBox ListaTipoOp { get; set; }
        Microsoft.Windows.Controls.DatePicker DTP_FechaM { get; set; }
        TextBox Txt_FiltroMatricula { get; set; }
        SearchTerceros SearchFiltroPropietaria { get; set; }
        ComboBox cbx_FiltroTipoOper { get; set; }
        TextBox Txt_FiltroCapacidad { get; set; }
        GroupBox PanelNuevoRegistro { get; set; }
        SearchTerceros companiaFactura { get; set; }
        SearchTerceros companiaPropietaria { get; set; }
        StackPanel panelPermisoExplotacion { get; set; }
        StackPanel Panel_Matricula { get; set; }


        #endregion

        #region Definicion Metodos
        event EventHandler<DataEventArgs<Tipo>> CargarDocumentoVuelo;
        event EventHandler<EventArgs> BuscarAeronave;
        event EventHandler<EventArgs> GuardarAeronave;
        event EventHandler<EventArgs> NewAeronave;
        event EventHandler<EventArgs> DeleteAeronave;
        event EventHandler<DataEventArgs<Aeronave>> CargarAeronave;
        event EventHandler<EventArgs> NuevoRegistro;
        event EventHandler<EventArgs> ActualizarListaAeronaves;

        #endregion

    }
}