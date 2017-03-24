using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfFront.Common.UserControls;
using WpfFront.Models;
using WpfFront.WMSBusinessService;

namespace WpfFront.Views
{
    /// <summary>
    /// Lógica de interacción para ModalInfoOperacionView.xaml
    /// </summary>
    public partial class ModalInfoOperacionView : Window, IModalInfoOperacionView
    {
        public ModalInfoOperacionView()
        {
            InitializeComponent();
        }
  
        public ModalInfoOperacionModel Model
        {
            get { return this.DataContext as ModalInfoOperacionModel; }
            set { this.DataContext = value; }
        }
        public SearchTerceros CompañiaFactura
        {
            get { return this.SearchCompañiaFactura; }
            set { this.SearchCompañiaFactura = value; }
        }
        public ComboBox TipoVuelo
        {
            get { return this.cmb_TipoVuelo; }
            set { this.cmb_TipoVuelo = value; }
        }


        public TextBox NumVuelo
        {
            get { return this.txt_NumVuelo; }
            set { this.txt_NumVuelo = value; }
        }
        public TextBox NumVueloSalida
        {
            get { return this.txt_NumVueloSalida; }
            set { this.txt_NumVueloSalida = value; }
        }
        public SearchAeropuertos Origen
        {
            get { return this.SearchAeropuertoOrigen; }
            set { this.SearchAeropuertoOrigen = value; }
        }
        public SearchAeropuertos Destino
        {
            get { return this.SearchAeropuertoDestino; }
            set { this.SearchAeropuertoDestino = value; }
        }
        public ComboBox TipoPosicion
        {
            get { return this.cmb_TipoPosicion; }
            set { this.cmb_TipoPosicion = value; }
        }
        public ComboBox Posicion
        {
            get { return this.cmb_Posicion; }
            set { this.cmb_Posicion = value; }
        }
        public ComboBox PosicionSalida
        {
            get { return this.cbxPoscinSalid; }
            set { this.cbxPoscinSalid = value; }
        }
        public ComboBox TipoPosicionSalida
        {
            get { return this.cbxTipoPoscinSalid; }
            set { this.cbxTipoPoscinSalid = value; }
        }
        public ComboBox TipoVueloSalida
        {
            get { return this.cbxTipoVuelSalid; }
            set { this.cbxTipoVuelSalid = value; }
        }
        public SearchAeronaves Aeronave
        {
            get { return this.SearchAeronave; }
            set { this.SearchAeronave = value; }
        }
        public ComboBox TipoDeclaracion
        {
            get { return this.cmb_TipoDeclaracion; }
            set { this.cmb_TipoOperacion = value; }
        }

        public ComboBox TipoFactura
        {
            get { return this.cmb_TipoFactura; }
            set { this.cmb_TipoFactura = value; }
        }
        public TextBlock EstadoVuelo
        {
            get { return this.txtEstadoVuelo; }
            set { this.txtEstadoVuelo = value; }
        }

        public TextBlock EstadoVueloSalida
        {
            get { return this.txtEstadoVueloSalida; }
            set { this.txtEstadoVueloSalida = value; }
        }

        #region Facturacion

        public TextBlock TotalAerodromo
        {
            get { return this.txtTotalAerodromo; }
            set { this.txtTotalAerodromo = value; }
        }
        public TextBlock TotalPuente
        {
            get { return this.txtTotalPuente; }
            set { this.txtTotalPuente = value; }
        }

        public TextBlock NumPuentes
        {
            get { return this.txtNumPuentes; }
            set { this.txtNumPuentes = value; }
        }
        public TextBlock TotalFacturacionContado
        {
            get { return this.txtTotalFacturacion; }
            set { this.txtTotalFacturacion = value; }
        }

        public TextBlock CantServBomberos
        {
            get { return this.txtCantidadSerBomb; }
            set { this.txtCantidadSerBomb = value; }
        }
        public TextBlock ValorServBomberos
        {
            get { return this.txtValorSerBomb; }
            set { this.txtValorSerBomb = value; }
        }
        public TextBlock CantHorasParqueo
        {
            get { return this.txtCantHoras; }
            set { this.txtCantHoras = value; }
        }
        public TextBlock ValorParqueo
        {
            get { return this.txtTotalParqueo; }
            set { this.txtTotalParqueo = value; }
        }
        public TextBlock RecargoNocturno
        {
            get { return this.txtRecargoNocturno; }
            set { this.txtRecargoNocturno = value; }
        }

        public TextBlock CantTasas
        {
            get { return this.txtcantPasajeros; }
            set { this.txtcantPasajeros = value; }
        }
        public TextBlock ValorTasas
        {
            get { return this.txtValorTasas; }
            set { this.txtValorTasas = value; }
        }

        public TextBlock ValorAerodromoUSD
        {
            get { return this.lblAerodromoDolares; }
            set { this.lblAerodromoDolares = value; }
        }

        public TextBlock ValorRecargoUSD
        {
            get { return this.lblRecargoDolares; }
            set { this.lblRecargoDolares = value; }
        }

        public TextBlock ValorTasasUSD
        {
            get { return this.lblTasasDolares; }
            set { this.lblTasasDolares = value; }
        }

        public TextBlock ValorPuentesUSD
        {
            get { return this.lblPuentesDolares; }
            set { this.lblPuentesDolares = value; }
        }

        public TextBlock ValorParqueoUSD
        {
            get { return this.lblParqueoDolares; }
            set { this.lblParqueoDolares = value; }
        }

        public TextBlock ValorBomberosUSD
        {
            get { return this.lblBomberosDolares; }
            set { this.lblBomberosDolares = value; }
        }
        public TextBlock ValorTotalUSD
        {
            get { return this.lblTotalDolares; }
            set { this.lblTotalDolares = value; }
        }

        #endregion

        private void cmb_TipoFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Muestro u oculto campos dependiendo de la opcion seleccionada 
            // Credito, contado, abonos por anticipado
            if (TipoFactura.SelectedItem != null)
            {
                if (((MMaster)TipoFactura.SelectedItem).Code != "CONTADO")
                {
                    // NumVuelo.Visibility = Visibility.Collapsed;
                    TabItemFacturaContado.Header = "Liquidación Credito";
                    Stk_PanelInfoFacturas.Visibility = Visibility.Collapsed;
                    //Oculto todos los labels de informacion aproximada ya que nose una en credito
                    stkHoraDespegueReal.Visibility = Visibility.Collapsed;
                    lblHoraSalPuentContado.Visibility = Visibility.Collapsed;
                    lblHoraSalPuentCredito.Visibility = Visibility.Visible;
                    stkHoraSalPuentReal.Visibility = Visibility.Collapsed;

                    lblHoraSalPlatContado.Visibility = Visibility.Collapsed;
                    lblHoraSalPlatCredito.Visibility = Visibility.Visible;
                    stkHoraSalPlatCreditoReal.Visibility = Visibility.Collapsed;

                    lblDespegueContado.Visibility = Visibility.Collapsed;
                    lblDespegueCredito.Visibility = Visibility.Visible;

                    stkHoraProgLlegada.Visibility = Visibility.Visible;
                    stkHoraProgSalida.Visibility = Visibility.Visible;
                    stkPSala.Visibility = Visibility.Visible;
                    panelEstadoVueloLlegada.Visibility = Visibility.Visible;
                    panelEstadoVueloSalida.Visibility = Visibility.Visible;
                    gbxNotas.Visibility = Visibility.Visible;
                    gbxBomberos.Visibility = Visibility.Visible;

                    stkPSalaSalida.Visibility = Visibility.Visible;


                }
                else //Cuando es igual a contado
                {
                    //NumVuelo.Visibility = Visibility.Visible;
                    TabItemFacturaContado.Header = "Facturación Contado";
                    Stk_PanelInfoFacturas.Visibility = Visibility.Visible;


                    //Oculto todos los labels de informacion aproximada ya que nose una en credito
                    stkHoraDespegueReal.Visibility = Visibility.Visible;
                    lblHoraSalPuentContado.Visibility = Visibility.Visible;
                    lblHoraSalPuentCredito.Visibility = Visibility.Collapsed;
                    stkHoraSalPuentReal.Visibility = Visibility.Visible;

                    lblHoraSalPlatContado.Visibility = Visibility.Visible;
                    lblHoraSalPlatCredito.Visibility = Visibility.Collapsed;
                    stkHoraSalPlatCreditoReal.Visibility = Visibility.Visible;

                    lblDespegueContado.Visibility = Visibility.Visible;
                    lblDespegueCredito.Visibility = Visibility.Collapsed;

                    stkHoraProgLlegada.Visibility = Visibility.Collapsed;
                    stkHoraProgSalida.Visibility = Visibility.Collapsed;
                    stkPSala.Visibility = Visibility.Collapsed;
                    panelEstadoVueloLlegada.Visibility = Visibility.Collapsed;
                    panelEstadoVueloSalida.Visibility = Visibility.Collapsed;
                    gbxNotas.Visibility = Visibility.Collapsed;
                    gbxBomberos.Visibility = Visibility.Collapsed;

                    stkPSalaSalida.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void DTP_FechasLlegada_MouseLeave_1(object sender, MouseEventArgs e)
        {
            DTP_FechaAterrizaje.ToolTip = String.Format("{0:d}", DTP_FechaAterrizaje.SelectedDate);
            DTP_FechaLlegadaPuente.ToolTip = String.Format("{0:d}", DTP_FechaLlegadaPuente.SelectedDate);
            DTP_FechaLlegadaPlataforma.ToolTip = String.Format("{0:d}", DTP_FechaLlegadaPlataforma.SelectedDate);
            DTP_FechaProgramadaLleg.ToolTip = String.Format("{0:d}", DTP_FechaProgramadaLleg.SelectedDate);

        }
        private void MouseLeaveMostrarFecha(object sender, MouseEventArgs e)
        {
            DTP_FechaSalidaPlataforma.ToolTip = String.Format("{0:d}", DTP_FechaSalidaPlataforma.SelectedDate);
            DTP_FechaSalidaPuente.ToolTip = String.Format("{0:d}", DTP_FechaSalidaPuente.SelectedDate);
            DTP_FechaDespegue.ToolTip = String.Format("{0:d}", DTP_FechaDespegue.SelectedDate);
            DTP_FechaProgramadaSal.ToolTip = String.Format("{0:d}", DTP_FechaProgramadaSal.SelectedDate);
        }

    }
    public interface IModalInfoOperacionView
    {
        ModalInfoOperacionModel Model { get; set; }
        SearchTerceros CompañiaFactura { get; set; }
        ComboBox TipoVuelo { get; set; }
        ComboBox TipoDeclaracion { get; set; }
        ComboBox TipoPosicion { get; set; }
        ComboBox Posicion { get; set; }
        ComboBox PosicionSalida { get; set; }
        ComboBox TipoPosicionSalida { get; set; }
        TextBox NumVuelo { get; set; }
        TextBox NumVueloSalida { get; set; }
        SearchAeropuertos Origen { get; set; }
        SearchAeropuertos Destino { get; set; }
        ComboBox TipoVueloSalida { get; set; }
        SearchAeronaves Aeronave { get; set; }
        ComboBox TipoFactura { get; set; }
        TextBlock EstadoVuelo { get; set; }
        TextBlock EstadoVueloSalida { get; set; }

        #region Facturacion
        TextBlock TotalAerodromo { get; set; }
        TextBlock TotalPuente { get; set; }
        TextBlock NumPuentes { get; set; }
        TextBlock TotalFacturacionContado { get; set; }
        TextBlock CantServBomberos { get; set; }
        TextBlock ValorServBomberos { get; set; }
        TextBlock CantHorasParqueo { get; set; }
        TextBlock ValorParqueo { get; set; }
        TextBlock CantTasas { get; set; }
        TextBlock ValorTasas { get; set; }
        TextBlock RecargoNocturno { get; set; }


        TextBlock ValorAerodromoUSD { get; set; }
        TextBlock ValorRecargoUSD { get; set; }
        TextBlock ValorTasasUSD { get; set; }
        TextBlock ValorPuentesUSD { get; set; }
        TextBlock ValorParqueoUSD { get; set; }
        TextBlock ValorBomberosUSD { get; set; }
        TextBlock ValorTotalUSD { get; set; }

        #endregion

    }

     
}
