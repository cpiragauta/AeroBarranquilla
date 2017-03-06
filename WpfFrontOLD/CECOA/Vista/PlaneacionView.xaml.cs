using System;
using System.Windows.Controls;
using Core.WPF;
using WMComposite.Events;
using System.Windows;
using WpfFront.Common;
using System.Windows.Input;
using WpfFront.Model;
using WpfFront.Modelo;
using WpfFront.Controles;

namespace WpfFront.Vista
{
    /// <summary>

    /// </summary>
    public partial class PlaneacionView : UserControlBase, IPlaneacionView
    {

        #region Metodos

        public event EventHandler<EventArgs> SavePlaneacion;
        public event EventHandler<DataEventArgs<Planeacion>> CargarPlaneacion;
        public event EventHandler<EventArgs> BuscarPlaneacion;
        public event EventHandler<EventArgs> DeletePlaneacion;
        public event EventHandler<EventArgs> NewPlaneacion;
        public event EventHandler<EventArgs> NuevoRegistro;
        public event EventHandler<EventArgs> ActualizarListaPlaneacion;
        public event EventHandler<EventArgs> ValidarRangoHora;
        public event EventHandler<EventArgs> ExportarPlaneacionExcel;
        public event EventHandler<EventArgs> CerrarTab;


        #endregion

        public PlaneacionView()
        {
            InitializeComponent();
        }

        public PlaneacionModel Model
        {
            get
            { return this.DataContext as PlaneacionModel; }
            set
            { this.DataContext = value; }
        }


        #region Variables


        /// <summary>
        /// Variable para pasar al presenter y que valide si es valida
        /// </summary>
        private String _horaAValidar;
        public String horaAValidar
        {
            get { return this._horaAValidar; }
            set { this._horaAValidar = value; }
        }


        public ListView ListaPlaneacion
        {
            get { return this.ListadoPlaneacion; }
            set { this.ListadoPlaneacion = value; }
        }
        public GroupBox PanelNuevoRegistro
        {
            get { return this.gb_NuevoRegistro; }
            set { this.gb_NuevoRegistro = value; }
        }
        public TextBox TXT_NVuelo
        {
            get { return this.txt_NVueloEntrada; }
            set { this.txt_NVueloSalida = value; }
        }
        public TextBox TXT_FiltroNVuelo
        {
            get { return this.txtFiltroNVueloEntrada; }
            set { this.txtFiltroNVueloEntrada = value; }
        }
        public TextBox TXT_FiltroNVueloSalida
        {
            get { return this.txtFiltroNVueloSalida; }
            set { this.txtFiltroNVueloSalida = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaAExportar
        {
            get { return this.DTP_FechaAExportar; }
            set { this.DTP_FechaAExportar = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FiltroFecha_Operacion
        {
            get { return this.DTP_FiltroFechaOp; }
            set { this.DTP_FiltroFechaOp = value; }
        }

        public SearchTerceros SearchCompania
        {
            get { return this.SearchCompañia; }
            set { this.SearchCompañia = value; }
        }

        public SearchTerceros SrchFiltroCompania
        {
            get { return this.SearchFiltroCompañia; }
            set { this.SearchFiltroCompañia = value; }
        }

        public SearchAeropuertos Origen
        {
            get { return this.SearchOrigen; }
            set { this.SearchOrigen = value; }
        }

        public SearchAeropuertos Destino
        {
            get { return this.SearchDestino; }
            set { this.SearchDestino = value; }
        }

        public ComboBox cbxBanda
        {
            get { return this.cmb_Banda; }
            set { this.cmb_Banda = value; }
        }

        public GridView GridPlaneacion
        {
            get { return this.GridViewPlaneacion; }
            set { this.GridViewPlaneacion = value; }
        }

        public CheckBox CheckLlegada
        {
            get { return this.chkLlegada; }
            set { this.chkLlegada = value; }
        }

        public CheckBox CheckSalida
        {
            get { return this.chkSalida; }
            set { this.chkSalida = value; }
        }

        public StackPanel DatosLlegada
        {
            get { return this.stkLlegada; }
            set { this.stkLlegada = value; }
        }

        public StackPanel DatosSalida
        {
            get { return this.stkSalida; }
            set { this.stkSalida = value; }
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
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Decimal || e.Key == Key.Tab || e.Key == Key.OemPeriod)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }


        private void btnNuevoRegistro_Click_1(object sender, RoutedEventArgs e)
        {
            NuevoRegistro(sender, e);
            chkLlegada.IsChecked = false;
            chkSalida.IsChecked = false;
        }

        private void ListadoPlaneacion_MouseDoubleClick_2(object sender, MouseButtonEventArgs e)
        {
            if (ListadoPlaneacion.SelectedItem != null)
                CargarPlaneacion(sender, new DataEventArgs<Planeacion>((Planeacion)ListadoPlaneacion.SelectedItem));
        }

        private void btn_Save_Click_1(object sender, RoutedEventArgs e)
        {
            if (DTP_FechaEntrada.SelectedDate == null)
            {
                Util.ShowError("Por favor seleccione una Fecha.");
                return;
            }
            if (SearchCompañia.Terceros.RowID == 0)
            {
                Util.ShowError("Por favor seleccione una compañia");
                return;
            }

            if (string.IsNullOrEmpty(txt_TipoAeronave.Text))
            {
                Util.ShowError("Por favor digitar Tipo Aeronave.");
                return;
            }

            if (chkLlegada.IsChecked == true)
            {
                if (string.IsNullOrEmpty(txt_NVueloEntrada.Text))
                {
                    Util.ShowError("Por favor digitar No. de Vuelo Entrada.");
                    return;
                }
                if (MaskedHoraEntrada.Text.Contains("_"))
                {
                    Util.ShowError("Por favor digitar Hora programada entrada.");
                    return;
                }

                if (Origen.Aeropuertos.RowID == 0)
                {
                    Util.ShowError("Por favor seleccione un Origen");
                    return;
                }
                if (cbxBanda.SelectedIndex == -1)
                {
                    Util.ShowError("Por favor seleccione una banda");
                    return;
                }
            }

            if (chkSalida.IsChecked == true)
            {
                if (string.IsNullOrEmpty(txt_NVueloSalida.Text))
                {
                    Util.ShowError("Por favor digitar No. de Vuelo Salida.");
                    return;
                }
                if (MaskedHoraSalida.Text.Contains("_") || string.IsNullOrEmpty(MaskedHoraSalida.Text))
                {
                    Util.ShowError("Por Ingrese Hora de salida Programada.");
                    return;
                }

                if (Destino.Aeropuertos.RowID == 0)
                {
                    Util.ShowError("Por favor seleccione un Destino");
                    return;
                }
            }

            SavePlaneacion(sender, e);
        }

        private void btnBuscarPlaneacion_Click_1(object sender, RoutedEventArgs e)
        {
            BuscarPlaneacion(sender, e);
        }

        private void btn_Delete_Click_1(object sender, RoutedEventArgs e)
        {
            DeletePlaneacion(sender, e);
        }

        private void btnActualizarLista_Click_1(object sender, RoutedEventArgs e)
        {
            ActualizarListaPlaneacion(sender, e);
        }

        private void ExportarPlaneacion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ExportarPlaneacionExcel(sender, e);
        }
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            CerrarTab(sender, e);
        }

        #endregion


        private void chkLlegada_Click_1(object sender, RoutedEventArgs e)
        {
            if (chkLlegada.IsChecked == true)
            {
                stkLlegada.IsEnabled = true;
            }
            else
            {
                stkLlegada.IsEnabled = false;
            }
        }

        private void chkSalida_Click_1(object sender, RoutedEventArgs e)
        {
            if (chkSalida.IsChecked == true)
            {
                stkSalida.IsEnabled = true;
            }
            else
            {
                stkSalida.IsEnabled = false;
            }
        }

        private void chkLlegada_Checked_1(object sender, RoutedEventArgs e)
        {
            if (chkLlegada.IsChecked == true)
            {
                stkLlegada.IsEnabled = true;
            }
            else
            {
                stkLlegada.IsEnabled = false;
            }
        }


        private void ValidarHora(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
            {
                string horaValidar = ((TextBox)(sender)).Text;
                if (horaValidar != "__:__" && !(horaValidar.Contains("_")))
                {
                    string[] arrayHora = horaValidar.Split(':');
                    if ((Int32.Parse(arrayHora[0]) < 0 || Int32.Parse(arrayHora[0]) >= 24) || (Int32.Parse(arrayHora[1]) < 0 || Int32.Parse(arrayHora[1]) >= 60))
                    {
                        Util.ShowError("Hora no valida");
                        ((TextBox)(sender)).Clear();
                    }
                }
            }
        }



    }

    public interface IPlaneacionView
    {
        //Clase Modelo
        PlaneacionModel Model { get; set; }

        #region Declaracion Variables
        ListView ListaPlaneacion { get; set; }
        GroupBox PanelNuevoRegistro { get; set; }
        TextBox TXT_NVuelo { get; set; }
        TextBox TXT_FiltroNVuelo { get; set; }
        TextBox TXT_FiltroNVueloSalida { get; set; }
        SearchTerceros SearchCompania { get; set; }
        SearchTerceros SrchFiltroCompania { get; set; }
        SearchAeropuertos Origen { get; set; }
        SearchAeropuertos Destino { get; set; }
        ComboBox cbxBanda { get; set; }
        Microsoft.Windows.Controls.DatePicker FiltroFecha_Operacion { get; set; }
        String horaAValidar { get; set; }
        GridView GridPlaneacion { get; set; }
        CheckBox CheckLlegada { get; set; }
        CheckBox CheckSalida { get; set; }
        StackPanel DatosLlegada { get; set; }
        StackPanel DatosSalida { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaAExportar { get; set; }

        #endregion

        #region Metodos

        event EventHandler<DataEventArgs<Planeacion>> CargarPlaneacion;
        event EventHandler<EventArgs> SavePlaneacion;
        event EventHandler<EventArgs> NewPlaneacion;
        event EventHandler<EventArgs> BuscarPlaneacion;
        event EventHandler<EventArgs> DeletePlaneacion;
        event EventHandler<EventArgs> NuevoRegistro;
        event EventHandler<EventArgs> ActualizarListaPlaneacion;
        event EventHandler<EventArgs> ValidarRangoHora;
        event EventHandler<EventArgs> ExportarPlaneacionExcel;
        event EventHandler<EventArgs> CerrarTab;

        #endregion

    }
}