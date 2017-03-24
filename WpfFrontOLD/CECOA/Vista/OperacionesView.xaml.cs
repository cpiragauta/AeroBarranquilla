using System;
using System.Windows.Controls;
using Core.WPF;
using WMComposite.Events;
using System.Windows;
using WpfFront.Common;
using WpfFront.Controles;
using System.Windows.Input;
using System.Data;
using Assergs.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using WpfFront.Model;
using WpfFront.Modelo;


namespace WpfFront.Vista
{



    public partial class OperacionesView : UserControlBase, IOperacionesView
    {
        private  wmsEntities db;
        #region Metodos

        public event EventHandler<EventArgs> ConfirmarRecordLlegada;
        public event EventHandler<EventArgs> ConfirmarRecordSalida;
        public event EventHandler<EventArgs> CerrarTab;
        public event EventHandler<EventArgs> CalcularEstado;
        public event EventHandler<EventArgs> CargarDatosDesdeAeronave;
        public event EventHandler<EventArgs> CargarDatosDesdePlaneacionLlegada;
        public event EventHandler<EventArgs> CargarDatosDesdePlaneacionSalida;
        public event EventHandler<EventArgs> ValidarRangoHora;
        public event EventHandler<EventArgs> cambiarNoVuelo;
        public event EventHandler<DataEventArgs<Operacion>> ActualizarDatosOperacion;
        public event EventHandler<EventArgs> ObteberListaFacturas;
        


        public event EventHandler<EventArgs> EliminarServicioBomberos;
        public event EventHandler<EventArgs> guardarServicioBomberos;
        public event EventHandler<DataEventArgs<Bombero>> seleccionarListaServicioBomberos;
        //-
        public event EventHandler<EventArgs> GuardarDatosLiquidacion;
        public event EventHandler<EventArgs> CalcularDatosLiquidacion;
        public event EventHandler<EventArgs> CalcularFacturacionContado;
        public event EventHandler<EventArgs> CalcularFacturacionContadoConAdicionales;
        public event EventHandler<EventArgs> CalcularTotalFacturacion;


        
        #endregion

        #region Variables

        #region Operaciones

        public ComboBox TipoFactura
        {
            get { return this.cmb_TipoFactura; }
            set { this.cmb_TipoFactura = value; }
        }

        public SearchAeronaves Aeronave
        {
            get { return this.SearchAeronave; }
            set { this.SearchAeronave = value; }
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

        public TextBox Sala
        {
            get { return this.txt_Sala; }
            set { this.txt_Sala = value; }
        }
        public TextBox SalaSalida
        {
            get { return this.txt_SalaSalida; }
            set { this.txt_SalaSalida = value; }
        }
        public TextBox CargaEntrada
        {
            get { return this.txtCargaEntrada; }
            set { this.txtCargaEntrada = value; }
        }

        public TextBox CorreoEntrada
        {
            get { return this.txtCorreoEntrada; }
            set { this.txtCorreoEntrada = value; }
        }

        public TextBox Observacion
        {
            get { return this.txtArObservaciones; }
            set { this.txtArObservaciones = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaAterrizaje
        {
            get { return this.DTP_FechaAterrizaje; }
            set { this.DTP_FechaAterrizaje = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaLlegadaPlataforma
        {
            get { return this.DTP_FechaLlegadaPlataforma; }
            set { this.DTP_FechaLlegadaPlataforma = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaLlegadaPuente
        {
            get { return this.DTP_FechaLlegadaPuente; }
            set { this.DTP_FechaLlegadaPuente = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaOperacion
        {
            get { return this.DTP_FechaOperacion; }
            set { this.DTP_FechaOperacion = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaSalida
        {
            get { return this.DTP_FechaSalida; }
            set { this.DTP_FechaSalida = value; }
        }

        ///Salida

        public Microsoft.Windows.Controls.DatePicker FechaDespegue
        {
            get { return this.DTP_FechaDespegue; }
            set { this.DTP_FechaDespegue = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaProgramacionLlegada
        {
            get { return this.DTP_FechaProgramadaLleg; }
            set { this.DTP_FechaProgramadaLleg = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaProgramacionSalida
        {
            get { return this.DTP_FechaProgramadaSal; }
            set { this.DTP_FechaProgramadaSal = value; }
        }


        

        public Microsoft.Windows.Controls.DatePicker FechaSalidaPlataforma
        {
            get { return this.DTP_FechaSalidaPlataforma; }
            set { this.DTP_FechaSalidaPlataforma = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaSalidaPuente
        {
            get { return this.DTP_FechaSalidaPuente; }
            set { this.DTP_FechaSalidaPuente = value; }
        }


        public Xceed.Wpf.Controls.MaskedTextBox HoraAterrizaje
        {
            get { return this.MaskedHoraAterrizaje; }
            set { this.MaskedHoraAterrizaje = value; }
        }

        public Xceed.Wpf.Controls.MaskedTextBox HoraPlataforma
        {
            get { return this.MaskedHoraPlataforma; }
            set { this.MaskedHoraPlataforma = value; }
        }

        public Xceed.Wpf.Controls.MaskedTextBox HoraPuente
        {
            get { return this.MaskedHoraPuente; }
            set { this.MaskedHoraPuente = value; }
        }

        public Xceed.Wpf.Controls.MaskedTextBox HoraProgramada
        {
            get { return this.MaskedHoraProgramada; }
            set { this.MaskedHoraProgramada = value; }
        }

        public Xceed.Wpf.Controls.MaskedTextBox HoraDespegue
        {
            get { return this.MaskedHoraDespegue; }
            set { this.MaskedHoraDespegue = value; }
        }


        public Xceed.Wpf.Controls.MaskedTextBox HoraPlataformaSalida
        {
            get { return this.MaskedHoraSalidaPlataforma; }
            set { this.MaskedHoraSalidaPlataforma = value; }
        }

        public Xceed.Wpf.Controls.MaskedTextBox HoraPuenteSalida
        {
            get { return this.MaskedHoraSalidaPuente; }
            set { this.MaskedHoraSalidaPuente = value; }
        }

        public Xceed.Wpf.Controls.MaskedTextBox HoraProgramadaSalida
        {
            get { return this.MaskedHoraProgramadaSalida; }
            set { this.MaskedHoraProgramadaSalida = value; }
        }
        public ComboBox TipoPosicion
        {
            get { return this.cmb_TipoPosicion; }
            set { this.cmb_TipoPosicion = value; }
        }

        //public SearchAeropuertos Origen
        //{
        //    get { return this.SearchOrigen; }
        //    set { this.SearchOrigen = value; }
        //}

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

        public SearchAeronaves SearchAeronavesCbx
        {
            get { return this.SearchAeronave; }
            set { this.SearchAeronave = value; }
        }

        public ComboBox TipoOperacion
        {
            get { return this.cmb_TipoOperacion; }
            set { this.cmb_TipoOperacion = value; }
        }

        public ComboBox TipoVuelo
        {
            get { return this.cmb_TipoVuelo; }
            set { this.cmb_TipoVuelo = value; }
        }

        public ComboBox TipoDeclaracion
        {
            get { return this.cmb_TipoDeclaracion; }
            set { this.cmb_TipoOperacion = value; }
        }

        public ComboBox Banda
        {
            get { return this.cmb_Banda; }
            set { this.cmb_Banda = value; }
        }

        public ComboBox Posicion
        {
            get { return this.cmb_Posicion; }
            set { this.cmb_Posicion = value; }
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

        public TextBlock CIAExplotadora
        {
            get { return this.lblCIAExplotadora; }
            set { this.lblCIAExplotadora = value; }
        }

        public SearchTerceros CompañiaFactura
        {
            get { return this.SearchCompañiaFactura; }
            set { this.SearchCompañiaFactura = value; }
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

        public TextBlock txtPaxNal
        {
            get { return this.txtPaxEmNal; }
            set { this.txtPaxEmNal = value; }
        }

        public TextBlock txtPaxIntrNal
        {
            get { return this.txtPaxEmIntrNal; }
            set { this.txtPaxEmIntrNal = value; }
        }

        public TextBlock txtTotalPagoCOP
        {
            get { return this.txt_TotalCOP; }
            set { this.txt_TotalCOP = value; }
        }

        public StackPanel PanelEstadoVueloLl
        {
            get { return this.panelEstadoVueloLlegada; }
            set { this.panelEstadoVueloLlegada = value; }
        }

        public StackPanel PanelEstadoVueloSal
        {
            get { return this.panelEstadoVueloSalida; }
            set { this.panelEstadoVueloSalida = value; }
        }

        public StackPanel PanelHoraProgLleg
        {
            get { return this.stkHoraProgLlegada; }
            set { this.stkHoraProgLlegada = value; }
        }

        public StackPanel PanelHoraProgSal
        {
            get { return this.stkHoraProgSalida; }
            set { this.stkHoraProgSalida = value; }
        }


        /// Variable para pasar al presenter y que valide si es valida

        private String _horaAValidar;
        public String horaAValidar
        {
            get { return this._horaAValidar; }
            set { this._horaAValidar = value; }
        }

        public TabItem TabFacturaContado
        {
            get { return this.TabItemFacturaContado; }
            set { this.TabItemFacturaContado = value; }
        }

        public StackPanel PanelSala
        {
            get { return this.stkPSala; }
            set { this.stkPSala = value; }
        }

        public StackPanel PanelDatosOperacionLlegada
        {
            get { return this.stkDatosOperacionLlegada; }
            set { this.stkDatosOperacionLlegada = value; }
        }
        public StackPanel PanelDatosOperacionSalida1
        {
            get { return this.stkDatosOperacionSalida1; }
            set { this.stkDatosOperacionSalida1 = value; }
        }
        public StackPanel PanelDatosOperacionSalida2
        {
            get { return this.stkDatosOperacionSalida2; }
            set { this.stkDatosOperacionSalida2 = value; }
        }

        public StackPanel PanelDatosOperacionSalida3Contado
        {
            get { return this.stkDatosOperacionSalida3Contado; }
            set { this.stkDatosOperacionSalida3Contado = value; }
        }

        public StackPanel PanelDatosOperacionSalidaPuenteContado
        {
            get { return this.stkHoraSalPuentReal; }
            set { this.stkHoraSalPuentReal = value; }
        }


        public StackPanel PanelDatosCabecera
        {
            get { return this.stkDatosCabecera; }
            set { this.stkDatosCabecera = value; }
        }

        public StackPanel PanelDatosLiquidacionTasas
        {
            get { return this.stkDatosLiquidacionTasas; }
            set { this.stkDatosLiquidacionTasas = value; }
        }

        public ImageButton BtnConfirmarLlegada
        {
            get { return this.btnConfirmarLlegada; }
            set { this.btnConfirmarLlegada = value; }
        }
        public ImageButton BtnConfirmarSalida
        {
            get { return this.btnConfirmarDatosSalida; }
            set { this.btnConfirmarDatosSalida = value; }
        }

        public ImageButton BtnGuardarLlegada
        {
            get { return this.btnGuardar2; }
            set { this.btnGuardar2 = value; }
        }
        public ImageButton BtnGuardarSalida
        {
            get { return this.btnGuardarDatosSalida; }
            set { this.btnGuardarDatosSalida = value; }
        }
        public ImageButton Btn_CerrarOperacion
        {
            get { return this.BtnCerrarOperacion; }
            set { this.BtnCerrarOperacion = value; }
        }
        public ImageButton Btn_AnularFactura
        {
            get { return this.btnAnularFactura; }
            set { this.btnAnularFactura = value; }
        }
        public ImageButton BtnAbrirOperacion
        {
            get { return this.Btn_AbrirOperacion; }
            set { this.Btn_AbrirOperacion = value; }
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

        public StackPanel PanelDatosBomberos
        {
            get { return this.stkDatosBomberos; }
            set { this.stkDatosBomberos = value; }
        }

        #endregion

        #region DatosBasicos
        public TabItem TabLiquidacionTasas
        {
            get { return this.TabLiquidacion; }
            set { this.TabLiquidacion = value; }
        }

        #endregion

        #region Adicionados

        #region servicioBomberos


        public ComboBox ListaTipoServicio
        {
            get { return this.cbxServicio; }
            set { this.cbxServicio = value; }
        }

        public TextBlock NIT
        {
            get { return this.lblNIT; }
            set { this.lblNIT = value; }
        }



        // Variable que va a guardar la compania factura de la Aeronave seleccionada

        public TextBlock ClienteBomberos
        {
            get { return this.txtNombreClienteBomberos; }
            set { this.txtNombreClienteBomberos = value; }
        }

        public Microsoft.Windows.Controls.DatePicker fechaServicio
        {
            get { return this.DTP_FechaServicio; }
            set { this.DTP_FechaServicio = value; }
        }

        public ListView ListaBomberos
        {
            get { return this.lvRegistroServicioBomberos; }
            set { this.lvRegistroServicioBomberos = value; }
        }

        public GroupBox GroupNotasAdicionales
        {
            get { return this.gbxNotas; }
            set { this.gbxNotas = value; }
        }

        public GroupBox GroupBomberosAdicionales
        {
            get { return this.gbxBomberos; }
            set { this.gbxBomberos = value; }
        }      
        


        #endregion

        #endregion

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

        public ImageButton BotonFacturar
        {
            get { return this.btnFacturar; }
            set { this.btnFacturar = value; }
        }

        public ImageButton BotonImprimirFactura
        {
            get { return this.btnImprimir; }
            set { this.btnImprimir = value; }
        }

        #endregion

        #endregion

        public OperacionesView()
        {
            InitializeComponent();
            this.db = new wmsEntities();
        }

        public OperacionesModel Model
        {
            get
            { return this.DataContext as OperacionesModel; }
            set
            { this.DataContext = value; }
        }

        #region METODOS

        #region Operacion


        /// Se ejecuta al cambiar el tipo de operacion, Si es General o militar tomara por defecto la placa de la aeronave como no. de vuelo.
        private void cmb_TipoOperacion_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
            {
            cambiarNoVuelo(sender, e);
            //cmb_TipoOperacion.Focus();
        }

        #region metodos validarHoras

        private void ValidarHorasLlegada_KeyDown_1(object sender, KeyEventArgs e)
    {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                string mensaje = validacionesHorasFechasLlegada();
                if (mensaje != "")
                {
                    Util.ShowError(mensaje);
                }
            }
            if (MaskedHoraPlataforma.IsFocused && e.Key == Key.LeftShift)
            {
                MaskedHoraAterrizaje.Focus();
            }
        }


        public string validacionesHorasFechasLlegada()
        {
            String mensaje = "";
            #region Validaciones de horas de Llegada

            if (!HoraPlataforma.Text.Contains("_") && !HoraAterrizaje.Text.Contains("_") && FechaAterrizaje.SelectedDate != null && FechaLlegadaPlataforma.SelectedDate != null)
            {

                int Validacion = DateTime.Compare(FechaAterrizaje.SelectedDate.Value, FechaLlegadaPlataforma.SelectedDate.Value);
                if (Validacion < 0) //Aterrizo un dia Y llego a plataforma en otro
                {
                    if (DateTime.Parse(HoraPlataforma.Text) >= DateTime.Parse(HoraAterrizaje.Text))
                    {
                        mensaje = "La Hora llegada a plataforma debe ser Menor a hora de Aterrizaje ";
                    }
                }
                else if (Validacion ==0) //Mismos Dias
                {
                    if (DateTime.Parse(HoraPlataforma.Text) <= DateTime.Parse(HoraAterrizaje.Text))
                    {
                        mensaje = "La Hora llegada a plataforma debe ser Mayor a hora de Aterrizaje ";
                    }
                } //Error
                else
                {
                    mensaje = "La Fecha Aterrizaje no puede ser mayor a la fecha de Llegada Plataforma ";
                }
            }


            if (!HoraPlataforma.Text.Contains("_") && !HoraPuente.Text.Contains("_") && FechaLlegadaPuente.SelectedDate != null && FechaLlegadaPlataforma.SelectedDate != null)
            {
                int Validacion = DateTime.Compare(FechaLlegadaPlataforma.SelectedDate.Value, FechaLlegadaPuente.SelectedDate.Value);
                if (Validacion < 0) // Dias diferentes
                {
                    if (DateTime.Parse(HoraPuente.Text) >= DateTime.Parse(HoraPlataforma.Text))
                    {
                        mensaje = "La Hora Llegada a Puente debe ser Menor a hora de Llegada Plataforma";
                    }
                }
                else if (Validacion == 0) //Mismos Dias
                {
                    if (DateTime.Parse(HoraPuente.Text) <= DateTime.Parse(HoraPlataforma.Text))
                    {
                        mensaje = "La Hora llegada a Puente debe ser Mayor a hora de Plataforma ";
                    }
                } //Error
                else
                {
                    mensaje = "La Fecha de Llegada Plataforma no puede ser mayor a la fecha de Llegada Puente ";
                }
            }



            if (!HoraAterrizaje.Text.Contains("_") && !HoraPuente.Text.Contains("_") && FechaAterrizaje.SelectedDate != null && FechaLlegadaPuente.SelectedDate != null)
            {
                int Validacion = DateTime.Compare(FechaAterrizaje.SelectedDate.Value, FechaLlegadaPuente.SelectedDate.Value);
                if (Validacion < 0) //Dias Diferentes
                {
                    if ( DateTime.Parse(HoraPuente.Text) >= DateTime.Parse(HoraAterrizaje.Text))
                    {
                        mensaje = "La Hora Llegada a Puente debe ser Menor a hora de Aterrizaje ";
                    }
                }
                else if (Validacion == 0) //Mismos Dias
                {
                    if (DateTime.Parse(HoraPuente.Text) <= DateTime.Parse(HoraAterrizaje.Text))
                    {
                        mensaje = "La Hora llegada a Puente debe ser Mayor a hora de Aterrizaje ";
                    }
                } //Error
                else
                {
                    mensaje = "La Fecha de Aterrizaje no puede ser mayor a la fecha de Llegada Puente ";
                }
            }

            #endregion
            return mensaje;
        }


        //public String ValidarHorasConFecha(DateTime? Fecha1, String Hora1, DateTime? Fecha2, String Hora2, String NombreHora1, String NombreHora2)
        //{
        //    String mensaje = "";

           
        //    return mensaje;

        //}

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

        //Selecciono por defecto las fechas de la operacion (Llegada)
        private void DTP_FechaOperacion_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DTP_FechaOperacion.SelectedDate == DateTime.MinValue)
            {
                DTP_FechaOperacion.SelectedDate = null;
                DTP_FechaOperacion.DisplayDate = DateTime.Now;
            }
            DTP_FechaAterrizaje.SelectedDate = DTP_FechaOperacion.SelectedDate;
            DTP_FechaLlegadaPuente.SelectedDate = DTP_FechaOperacion.SelectedDate;
            DTP_FechaLlegadaPlataforma.SelectedDate = DTP_FechaOperacion.SelectedDate;
            DTP_FechaServicio.SelectedDate = DTP_FechaOperacion.SelectedDate;
            DTP_FechaServicioAdicional.SelectedDate = DTP_FechaOperacion.SelectedDate;
            DTP_FechaInicialPyP.SelectedDate = DTP_FechaOperacion.SelectedDate;
            DTP_FechaProgramadaLleg.SelectedDate = DTP_FechaOperacion.SelectedDate;
        }
        //Selecciono por defecto las fechas Salida
        private void DTP_FechaSalida_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (FechaOperacion.SelectedDate != null)
            {
                if (FechaSalida.SelectedDate < FechaOperacion.SelectedDate)
                {
                    Util.ShowError("La fecha Salida debe ser mayor a la fecha de Llegada");
                    FechaSalida.SelectedDate = null;
                    DTP_FechaDespegue.SelectedDate = null;
                    DTP_FechaSalidaPuente.SelectedDate = null;
                    DTP_FechaSalidaPlataforma.SelectedDate = null;
                    return;
                }
            }
            DTP_FechaDespegue.SelectedDate = DTP_FechaSalida.SelectedDate;
            DTP_FechaSalidaPuente.SelectedDate = DTP_FechaSalida.SelectedDate;
            DTP_FechaSalidaPlataforma.SelectedDate = DTP_FechaSalida.SelectedDate;
            DTP_FechaProgramadaSal.SelectedDate = DTP_FechaSalida.SelectedDate;
            
        }


        private void validarHorasSalida(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                String mensaje = this.validarHorasFechasSalida();

                if (mensaje != "")
                {
                    Util.ShowError(mensaje);
                }
            }

        }

        public string validarHorasFechasSalida()
        {
            string mensaje = "";
            #region Validaciones de horas de Salida

            if (!HoraPlataformaSalida.Text.Contains("_") && !HoraDespegue.Text.Contains("_") && FechaSalidaPlataforma.SelectedDate != null && FechaDespegue.SelectedDate != null)
            {
                int condicion = DateTime.Compare(FechaSalidaPlataforma.SelectedDate.Value, FechaDespegue.SelectedDate.Value);
                if (condicion < 0) //Dia Diferente
                {
                    if (DateTime.Parse(HoraDespegue.Text) >= DateTime.Parse(HoraPlataformaSalida.Text))
                    {
                        mensaje = "La Hora de Plataforma debe ser Mayor a Hora de Despegue  ";
                    }
                }
                else if (condicion == 0) //Mismo Dia
                {
                    if (DateTime.Parse(HoraDespegue.Text) <= DateTime.Parse(HoraPlataformaSalida.Text))
                    {
                        mensaje = "La Hora de despegue debe ser Mayor a Hora de Salida a Plataforma  ";
                    }
                }
                else
                {
                    mensaje = "La Fecha Salida Plataforma no puede ser mayor a la fecha de Despegue ";
                }
            }

            if (!HoraPlataformaSalida.Text.Contains("_") && !HoraPuenteSalida.Text.Contains("_") && (FechaSalidaPuente.SelectedDate != null) && (FechaSalidaPlataforma.SelectedDate != null))
            {
                int condicion = DateTime.Compare(FechaSalidaPuente.SelectedDate.Value, FechaSalidaPlataforma.SelectedDate.Value);
                if (condicion < 0)//Dia Diferente
                {
                    //if (DateTime.Parse(HoraPlataformaSalida.Text) >= DateTime.Parse(HoraPuenteSalida.Text))
                    //{
                    //    mensaje = "La Hora Salida de Puente debe ser Menor a hora Salida de Plataforma";
                    //}
                }
                else if (condicion == 0) //Mismo Dia
                {
                    if (DateTime.Parse(HoraPlataformaSalida.Text) <= DateTime.Parse(HoraPuenteSalida.Text))
                    {
                        mensaje = "La Hora Salida de Plataforma debe ser Mayor a hora Salida de Puente ";
                    }
                }
                else
                {
                    mensaje = "La Fecha de Salida Puente no puede ser mayor a la fecha de Salida Plataforma ";
                }
            }



            if (!HoraDespegue.Text.Contains("_") && !HoraPuenteSalida.Text.Contains("_") && FechaSalidaPuente.SelectedDate != null && FechaDespegue.SelectedDate != null)
            {
                int condicion = DateTime.Compare(FechaSalidaPuente.SelectedDate.Value, FechaDespegue.SelectedDate.Value);
                if (condicion < 0)//Dia diferente
                {
                    if (DateTime.Parse(HoraDespegue.Text) >= DateTime.Parse(HoraPuenteSalida.Text))
                    {
                        mensaje = "La Hora Salida Puente debe ser Menor a hora de Despegue ";
                    }
                }
                else if (condicion == 0) //Mismo dia
                {
                    if (DateTime.Parse(HoraDespegue.Text) <= DateTime.Parse(HoraPuenteSalida.Text))
                    {
                        mensaje = "La Hora Despegue debe ser Mayor a hora de Salida a Puente ";
                    }
                }
                else
                {
                    mensaje = "La Fecha de de Salida Puente no puede ser mayor a la fecha Despegue";
                }
            }

            //Valido que si llego y salio el mismo dia las horas de salida sean mayores a las de llegada
            if(FechaOperacion.SelectedDate != null && FechaSalida.SelectedDate != null){
                if (FechaOperacion.SelectedDate == FechaSalida.SelectedDate)
                {
                    if (TipoPosicion.SelectedItem != null)
                    {
                        TextBox AuxHoraValidar = null;
                        //Si Llego en puente valido con la hora de puente
                        //Si no llego en puente valido con la hora plataforma
                        AuxHoraValidar = ((Tipo)TipoPosicion.SelectedItem).Codigo == "PUENTE" ? MaskedHoraPuente : MaskedHoraPlataforma;

                        if (!AuxHoraValidar.Text.Contains("_"))
                        {
                            if (!MaskedHoraSalidaPuente.Text.Contains("_"))
                            {
                                if (DateTime.Parse(MaskedHoraSalidaPuente.Text) <= DateTime.Parse(AuxHoraValidar.Text))
                                {
                                    mensaje = "Horas de salida incorrectas";
                                }
                            }
                            if (!MaskedHoraSalidaPlataforma.Text.Contains("_"))
                            {
                                if (DateTime.Parse(MaskedHoraSalidaPlataforma.Text) <= DateTime.Parse(AuxHoraValidar.Text))
                                {
                                    mensaje = "Horas de salida incorrectas";
                                }
                            }
                        }
                    }
                }
                if (FechaSalida.SelectedDate < FechaOperacion.SelectedDate)
                {
                    mensaje = "La fecha Salida debe ser mayor a la fecha de Llegada";
                }

            }


            #endregion

            return mensaje;
        }


        private void ValidarHoraDespegue_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (!ValidarRangoHoras(sender.ToString()))
            {
                MaskedHoraDespegue.Clear();
            }

            if (e.Key == Key.LeftShift)
            {
                MaskedHoraSalidaPlataforma.Focus();
            }
        }

        public void ValidarRangoHora_kEY(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
            {
                bool rango = ValidarRangoHoras(sender.ToString());
            }
        }

        public bool ValidarRangoHoras(String Hora)
        {
            bool rangoCorrecto = true;
            Hora = Hora.Substring(Hora.Length - 5, 5);
            if (!(Hora.Contains("_")) && Hora.Contains(":"))
            {
                string[] arrayHora = Hora.Split(':');

                if ((Int32.Parse(arrayHora[0]) < 0 || Int32.Parse(arrayHora[0]) >= 24) || (Int32.Parse(arrayHora[1]) < 0 || Int32.Parse(arrayHora[1]) >= 60))
                {
                    Util.ShowError("Hora no valida");
                    rangoCorrecto = false;
                }
            }
            return rangoCorrecto;
        }
        private void ValidarHoraSalidaPlataforma_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
            {
                horaAValidar = HoraPlataformaSalida.Text;
                ValidarRangoHora(sender, e);
            }
            if (e.Key == Key.LeftShift)
            {
                MaskedHoraSalidaPuente.Focus();
            }
        }
        private void ValidarHoraSalidaPuente_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
            {
                horaAValidar = HoraPuenteSalida.Text;
                ValidarRangoHora(sender, e);
            }
            if (e.Key == Key.LeftShift)
            {
                cbxPoscinSalid.Focus();
            }
        }

        #endregion

        private void SearchAeronave_OnSelected_1(object sender, EventArgs e)
        {
            CargarDatosDesdeAeronave(sender, e);
        }

        private void SearchOrigen_OnSelected_1(object sender, EventArgs e)
        {
            if (SearchAeropuertoOrigen.Aeropuertos != null)
            {
                cmb_TipoVuelo.SelectedValue = SearchAeropuertoOrigen.Aeropuertos.Tipo.RowID;
            }
        }

        private void SearchAeropuertoOrigen_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (SearchAeropuertoOrigen.Aeropuertos != null)
            {
                cmb_TipoVuelo.SelectedValue = SearchAeropuertoOrigen.Aeropuertos.Tipo.RowID;
            }
        }

        private void SearchDestino_OnSelected_1(object sender, EventArgs e)
        {
            if (SearchAeropuertoDestino.Aeropuertos != null)
            {
                cbxTipoVuelSalid.SelectedValue = SearchAeropuertoDestino.Aeropuertos.Tipo.RowID;
            }
        }

        private void SearchDestino_KeyUp_1(object sender, KeyEventArgs e)
       {
            if (SearchAeropuertoDestino.Aeropuertos != null)
            {
                cbxTipoVuelSalid.SelectedValue = SearchAeropuertoDestino.Aeropuertos.Tipo.RowID;
            }
        }

        // Se ejecuta al cambiar Tipo de Factura      
        private void cmb_TipoFactura_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // Muestro u oculto campos dependiendo de la opcion seleccionada 
            // Credito, contado, abonos por anticipado
            if (TipoFactura.SelectedItem != null)
            {
                if (((Tipo)TipoFactura.SelectedItem).Codigo != "CONTADO")
                {
                    // NumVuelo.Visibility = Visibility.Collapsed;
                    TabFacturaContado.Header = "Liquidación Credito";
                    btnFacturar.Visibility = Visibility.Collapsed;
                    Stk_PanelInfoFacturas.Visibility = Visibility.Collapsed;
                    btnCalcularFacturacion.Content = "Calcular Liquidación";
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

                    PanelHoraProgLleg.Visibility = Visibility.Visible;
                    PanelHoraProgSal.Visibility = Visibility.Visible;
                    PanelSala.Visibility = Visibility.Visible;
                    PanelEstadoVueloLl.Visibility = Visibility.Visible;
                    PanelEstadoVueloSal.Visibility = Visibility.Visible;
                    BtnCerrarOperacion.Visibility = Visibility.Visible;
                    GroupNotasAdicionales.Visibility = Visibility.Visible;
                    GroupBomberosAdicionales.Visibility = Visibility.Visible;

                    stkPSalaSalida.Visibility = Visibility.Visible;


                }
                else //Cuando es igual a contado
                {
                    //NumVuelo.Visibility = Visibility.Visible;
                    TabFacturaContado.Header = "Facturación Contado";
                    btnFacturar.Visibility = Visibility.Visible;
                    Stk_PanelInfoFacturas.Visibility = Visibility.Visible;
                    btnCalcularFacturacion.Content = "Calcular Facturación";


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

                    PanelHoraProgLleg.Visibility = Visibility.Collapsed;
                    PanelHoraProgSal.Visibility = Visibility.Collapsed;
                    PanelSala.Visibility = Visibility.Collapsed;
                    PanelEstadoVueloLl.Visibility = Visibility.Collapsed;
                    PanelEstadoVueloSal.Visibility = Visibility.Collapsed;
                    BtnCerrarOperacion.Visibility = Visibility.Collapsed;
                    GroupNotasAdicionales.Visibility = Visibility.Collapsed;
                    GroupBomberosAdicionales.Visibility = Visibility.Collapsed;

                    stkPSalaSalida.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// Controla que solo se digiten numeros en el campo
        private void soloNumerosKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Decimal || e.Key == Key.Tab || e.Key == Key.OemPeriod)
            {
                e.Handled = false;//lo dejo pasar
            }
            else
            {
                e.Handled = true;//No lo dejo escribir
            }
        }

        ///Busca el vuelo desde planeacion para llegada
        private void BuscarVuelosKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CargarDatosDesdePlaneacionLlegada(sender, e);
                //Para que cambie el tipo de vuelo de llegada
                SearchAeropuertoOrigen_KeyUp_1(sender, e);
                SearchDestino_OnSelected_1(sender, e);
            }
            NumVuelo.Focus();
        }

        private void BuscarVuelosSalidaKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CargarDatosDesdePlaneacionSalida(sender, e);
                SearchDestino_OnSelected_1(sender, e);
            }
            NumVueloSalida.Focus();
        }

        private void SearchAeronave_LostFocus_2(object sender, RoutedEventArgs e)
        {
            CargarDatosDesdeAeronave(sender, e);
        }

        private void cmb_TipoPosicion_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            String texto = "__:__";
            MaskedHoraPuente.IsEnabled = true;
            DTP_FechaLlegadaPuente.IsEnabled = true;
            stkPosicionLleg.IsEnabled = true;
            //Cargo las posiciones, dependiendo del tipo de posicion que seleccione
            if (((Tipo)TipoPosicion.SelectedItem).Codigo == "PUENTE")
            {
                //Model.ListaPosicion = service.GetTipo(new Tipo { MetaType = new MType { Code = "POSICIONPUENTE" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicion = db.Tipo.Where(f=> f.Agrupacion.Codigo == "POSICIONPUENTE").OrderBy(f => f.Orden).ToList();
            }
            else if (((Tipo)TipoPosicion.SelectedItem).Codigo == "REMOTA")
            {
                //Model.ListaPosicion = service.GetTipo(new Tipo { MetaType = new MType { Code = "POSICION" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicion = db.Tipo.Where(f => f.Agrupacion.Codigo == "POSICION").OrderBy(f => f.Orden).ToList();
                MaskedHoraPuente.IsEnabled = false;
                DTP_FechaLlegadaPuente.IsEnabled = false;
                MaskedHoraPuente.Text = texto;
            }
            else if (((Tipo)TipoPosicion.SelectedItem).Codigo == "CARGA")
            {
                //Model.ListaPosicion = service.GetTipo(new Tipo { MetaType = new MType { Code = "POSICIONCARGA" } }).OrderBy(f => f.Orden).ToList();
                Model.ListaPosicion = db.Tipo.Where(f => f.Agrupacion.Codigo == "POSICIONCARGA").OrderBy(f => f.Orden).ToList();
                MaskedHoraPuente.IsEnabled = false;
                DTP_FechaLlegadaPuente.IsEnabled = false;
                MaskedHoraPuente.Text = texto;
            }
            else if (((Tipo)TipoPosicion.SelectedItem).Codigo == "HELIRAMPA")
            {
                //Model.ListaPosicion = service.GetTipo(new Tipo { MetaType = new MType { Code = "POSICIONHR" } }).OrderBy(f => f.Orden).ToList();
                Model.ListaPosicion = db.Tipo.Where(f => f.Agrupacion.Codigo == "POSICIONHR").OrderBy(f => f.Orden).ToList();
                MaskedHoraPuente.IsEnabled = false;
                DTP_FechaLlegadaPuente.IsEnabled = false;
                MaskedHoraPuente.Text = texto;
            }
            else
            {
                stkPosicionLleg.IsEnabled = false;
                MaskedHoraPuente.IsEnabled = false;
                DTP_FechaLlegadaPuente.IsEnabled = false;
                MaskedHoraPuente.Text = texto;
                Posicion.SelectedIndex = -1;
            }
        }

        private void cbxTipPosLlegadaPyP_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //Valido que este seleccionada una opcion
            if (cbxTipPosLlegadaPyP.SelectedItem == null || cbxTipPosLlegadaPyP.SelectedIndex == -1)
            { return; }
            //Cargo las posiciones dependiendo del tipo de posicion que se seleccione
            cbxPosLlegadaPyP.IsEnabled = true;
            HoraLlegadaPteAdicional.Visibility = Visibility.Collapsed;
            if (((Tipo)cbxTipPosLlegadaPyP.SelectedItem).Codigo == "PUENTE")
            {
                //Model.ListaPosicionPyP = service.GetTipo(new MMaster { MetaType = new MType { Code = "POSICIONPUENTE" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionPyP = db.Tipo.Where( f=> f.Agrupacion.Codigo == "POSICIONPUENTE").OrderBy(f => f.Orden).ToList();
                HoraLlegadaPteAdicional.Visibility = Visibility.Visible;
            }
            else if (((Tipo)cbxTipPosLlegadaPyP.SelectedItem).Codigo == "REMOTA")
            {
                //Model.ListaPosicionPyP = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICION" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionPyP = db.Tipo.Where( f=> f.Agrupacion.Codigo == "POSICION").OrderBy(f => f.Orden).ToList();
            }
            else if (((Tipo)cbxTipPosLlegadaPyP.SelectedItem).Codigo == "CARGA")
            {
                //Model.ListaPosicionPyP = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICIONCARGA" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionPyP = db.Tipo.Where( f=> f.Agrupacion.Codigo == "POSICIONCARGA").OrderBy(f => f.Orden).ToList();
            }
            else if (((Tipo)cbxTipPosLlegadaPyP.SelectedItem).Codigo == "HELIRAMPA")
            {
                //Model.ListaPosicionPyP = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICIONHR" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionPyP = db.Tipo.Where( f=> f.Agrupacion.Codigo == "POSICIONHR").OrderBy(f => f.Orden).ToList();
            }
            else
            {
                cbxPosLlegadaPyP.SelectedIndex = -1;
                cbxPosLlegadaPyP.IsEnabled = false;
            }
        }

        private void cbxTipPosSalidaPyP_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //Valido que este seleccionada una opcion
            if (cbxTipPosLlegadaPyP.SelectedItem == null || cbxTipPosLlegadaPyP.SelectedIndex == -1)
            { return; }
            //Cargo las posiciones dependiendo del tipo de posicion que se seleccione
            cbxPosSalidaPyP.IsEnabled = true;
            HoraSalidaPteAdicional.Visibility = Visibility.Collapsed;
            if (((Tipo)cbxTipPosSalidaPyP.SelectedItem).Codigo == "PUENTE")
            {
                //Model.ListaPosicionSalidaPyP = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICIONPUENTE" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionSalidaPyP = db.Tipo.Where(f=> f.Codigo== "POSICIONPUENTE").OrderBy(f => f.Orden).ToList();
                HoraSalidaPteAdicional.Visibility = Visibility.Visible;
            }
            else if (((Tipo)cbxTipPosSalidaPyP.SelectedItem).Codigo == "REMOTA")
            {
                //Model.ListaPosicionSalidaPyP = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICION" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionSalidaPyP = db.Tipo.Where(f=> f.Codigo== "POSICION").OrderBy(f => f.Orden).ToList();
            }
            else if (((Tipo)cbxTipPosSalidaPyP.SelectedItem).Codigo == "CARGA")
            {
                //Model.ListaPosicionSalidaPyP = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICIONCARGA" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionSalidaPyP = db.Tipo.Where(f=> f.Codigo== "POSICIONCARGA").OrderBy(f => f.Orden).ToList();
            }
            else if (((Tipo)cbxTipPosSalidaPyP.SelectedItem).Codigo == "HELIRAMPA")
            {
                //Model.ListaPosicionSalidaPyP = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICIONHR" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionSalidaPyP = db.Tipo.Where(f=> f.Codigo== "POSICIONHR").OrderBy(f => f.Orden).ToList();
            }
            else
            {
                cbxPosSalidaPyP.SelectedIndex = -1;
                cbxPosSalidaPyP.IsEnabled = false;
            }
        }
        

        private void cbxTipoPoscinSalid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (TipoPosicionSalida.SelectedItem == null || TipoPosicionSalida.SelectedIndex == -1)
            { return; }

            String texto = "__:__";
            MaskedHoraSalidaPuente.IsEnabled = true;
            stkPosicionSalida.IsEnabled = true;
            DTP_FechaSalidaPuente.IsEnabled = true;
            //Cargo las posiciones, dependiendo del tipo de posicion que seleccione
            if (((Tipo)TipoPosicionSalida.SelectedItem).Codigo == "PUENTE")
            {
                //Model.ListaPosicionSalida = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICIONPUENTE" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionSalida = TraerTipos("POSICIONPUENTE");
            }
            else if (((Tipo)TipoPosicionSalida.SelectedItem).Codigo == "REMOTA")
            {
                //Model.ListaPosicionSalida = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICION" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionSalida = TraerTipos("POSICION");
                MaskedHoraSalidaPuente.IsEnabled = false;
                DTP_FechaSalidaPuente.IsEnabled = false;
                MaskedHoraSalidaPuente.Text = texto;
            }
            else if (((Tipo)TipoPosicionSalida.SelectedItem).Codigo == "CARGA")
            {
                //Model.ListaPosicionSalida = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICIONCARGA" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionSalida = TraerTipos("POSICIONCARGA");
                MaskedHoraSalidaPuente.IsEnabled = false;
                DTP_FechaSalidaPuente.IsEnabled = false;
                MaskedHoraSalidaPuente.Text = texto;
            }
            else if (((Tipo)TipoPosicionSalida.SelectedItem).Codigo == "HELIRAMPA")
            {
                //Model.ListaPosicionSalida = service.GetMMaster(new MMaster { MetaType = new MType { Code = "POSICIONHR" } }).OrderBy(f => f.NumOrder).ToList();
                Model.ListaPosicionSalida = TraerTipos("POSICIONHR");
                MaskedHoraSalidaPuente.IsEnabled = false;
                DTP_FechaSalidaPuente.IsEnabled = false;
                MaskedHoraSalidaPuente.Text = texto;
            }
            else
            {
                stkPosicionSalida.IsEnabled = false;
                MaskedHoraSalidaPuente.IsEnabled = false;
                DTP_FechaSalidaPuente.IsEnabled = false;
                MaskedHoraSalidaPuente.Text = texto;
                PosicionSalida.SelectedIndex = -1;
            }
        }

        public List<Tipo> TraerTipos(String codigoAgrupacion)
        {
            List<Tipo> Lista = new List<Tipo>();
            if (!String.IsNullOrEmpty(codigoAgrupacion))
            {
                Lista = db.Tipo.Where(f => f.Agrupacion.Codigo == codigoAgrupacion).OrderBy(f => f.Orden).ToList();
            }
            else
            {
                Lista = db.Tipo.ToList();
            }
            return Lista;
        }

        private void btnConfirmarLlegada_Click_1(object sender, RoutedEventArgs e)
        {
            string mensaje = validacionesHorasFechasLlegada();
            if (mensaje != "")
            {
                Util.ShowError(mensaje);
                txt_NumVuelo.Focus();
                return;
            }
            String mensaje2 = "";
            if (TipoFactura.SelectedIndex == -1)
            {
                mensaje = "Debe seleccionar un tipo de factura";
                cmb_TipoFactura.Focus();
            }
            else if (FechaOperacion.SelectedDate == null)
            {
                mensaje = "Debe seleccionar una Fecha de operacion";
                DTP_FechaOperacion.Focus();
            }

            if (mensaje != "")
            {
                Util.ShowError(mensaje2);
                txt_NumVuelo.Focus();
                return;
            }
            SearchOrigen_OnSelected_1(sender, null);
            int varaux = 0;
            //Cargo los datos que esten seteados al record
            Model.Record.FechaOP = FechaOperacion.SelectedDate.Value;
            Model.Record.Tipo = ((Tipo)TipoFactura.SelectedItem);
            //Guardo informacion de los combos si estan seleccionados
            Model.Record.Aeronave = Aeronave.Aeronaves != null ? Aeronave.Aeronaves : null;
            Model.RecordLlegada.ClasificacionID = TipoOperacion.SelectedItem != null ? ((Tipo)TipoOperacion.SelectedItem).RowID : varaux = 0;
            Model.RecordLlegada.TipoVueloID = TipoVuelo.SelectedItem != null ? ((Tipo)TipoVuelo.SelectedItem).RowID : varaux = 0;
            Model.RecordLlegada.TipoDeclaracionID = TipoDeclaracion.SelectedItem != null ? ((Tipo)TipoDeclaracion.SelectedItem).RowID : varaux = 0;
            Model.RecordLlegada.BandaID = Banda.SelectedItem != null ? ((Tipo)Banda.SelectedItem).RowID : varaux = 0; ;
            Model.RecordLlegada.TipoPosicionID = TipoPosicion.SelectedItem != null ? ((Tipo)TipoPosicion.SelectedItem).RowID : varaux = 0;
            Model.RecordLlegada.PosicionID = Posicion.SelectedItem != null ? ((Tipo)Posicion.SelectedItem).RowID : varaux = 0;
            Model.RecordLlegada.OrigenID = Origen.Aeropuertos != null ? Origen.Aeropuertos.RowID : varaux = 0;
            Model.RecordLlegada.CIAFacturaID = CompañiaFactura.Terceros != null ? CompañiaFactura.Terceros.RowID : varaux=0;
            Model.RecordLlegada.NVuelo = !string.IsNullOrEmpty(NumVuelo.Text) ? NumVuelo.Text : null;

            ConfirmarRecordLlegada(sender, e);           
        }

        private void btnGuardarDatosLlegada_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Si no tiene salida, ni operacion le valido que la ultima operacion tenga salida 
                if (Model.RecordSalida.RowID == 0 && Model.Record.RowID == 0)
                {
                    //List<Operacion> operacionesCon = service.GetOperacion(new Operacion { Aeronave = new Aeronaves { RowID = SearchAeronave.Aeronaves.RowID } }).Where(f => f.Salida == null).ToList();
                    List<Operacion> operacionesCon = db.Operacion.Where(f=> f.RowID == SearchAeronave.Aeronaves.RowID && f.Salida == null).ToList();
                    //Busco operaciones con la misma aeronave que no tengan salida
                    if (operacionesCon.Count >= 1)
                    {
                        Util.ShowError("Esta Aeronave registra un vuelo sin salida, por favor verificar.");
                        return;
                    }
                }
            }
            catch (Exception es) { }

            if (Model.Record != null)
            {
                String mensaje = "";
                if (TipoFactura.SelectedIndex == -1)
                {
                    mensaje = "Debe seleccionar un tipo de factura";
                }
                else if (FechaOperacion.SelectedDate == null)
                {
                    mensaje = "Debe seleccionar una Fecha de operacion";
                }
               

                if (mensaje != "")
                {
                    Util.ShowError(mensaje);
                    return;
                }

                //Cargo los datos que esten seteados al record
                Model.Record.FechaOP = FechaOperacion.SelectedDate.Value;
                Model.Record.TipoFacturacionID = ((Tipo)TipoFactura.SelectedItem).RowID;
                //Guardo informacion de los combos si estan seleccionados
                Model.Record.Aeronave = Aeronave.Aeronaves != null ? Aeronave.Aeronaves : null;
                Model.RecordLlegada.ClasificacionID = TipoOperacion.SelectedItem != null ? ((Tipo)TipoOperacion.SelectedItem).RowID : Model.RecordLlegada.ClasificacionID;
                Model.RecordLlegada.TipoVueloID = TipoVuelo.SelectedItem != null ? ((Tipo)TipoVuelo.SelectedItem).RowID : Model.RecordLlegada.TipoVueloID;
                Model.RecordLlegada.TipoDeclaracionID = TipoDeclaracion.SelectedItem != null ? ((Tipo)TipoDeclaracion.SelectedItem).RowID : Model.RecordLlegada.TipoDeclaracionID;
                Model.RecordLlegada.BandaID = Banda.SelectedItem != null ? ((Tipo)Banda.SelectedItem).RowID : Model.RecordLlegada.BandaID;
                Model.RecordLlegada.TipoPosicionID = TipoPosicion.SelectedItem != null ? ((Tipo)TipoPosicion.SelectedItem).RowID : Model.RecordLlegada.TipoPosicionID;
                Model.RecordLlegada.PosicionID = Posicion.SelectedItem != null ? ((Tipo)Posicion.SelectedItem).RowID : Model.RecordLlegada.PosicionID;
                Model.RecordLlegada.OrigenID = Origen.Aeropuertos != null ? Origen.Aeropuertos.RowID : Model.RecordLlegada.OrigenID;
                Model.RecordLlegada.CIAFacturaID = CompañiaFactura.Terceros != null ? CompañiaFactura.Terceros.RowID : Model.RecordLlegada.CIAFacturaID;
                Model.RecordLlegada.NVuelo = !string.IsNullOrEmpty(NumVuelo.Text) ? NumVuelo.Text : null;
                Model.RecordLlegada.HoraProgramadaLlegada = !string.IsNullOrEmpty(HoraProgramada.Text) ? HoraProgramada.Text : null;
                SearchOrigen_OnSelected_1(sender, null);

                //Si es actualizar
                if (Model.Record.RowID != 0)
                {
                    //Datos Actualizacion Salida
                    Model.RecordLlegada.FechaModificacion = DateTime.Now;
                    Model.RecordLlegada.UsuarioModificacion = App.curUser.NombreUsuario;
                    //Actualizar Salida
                    //service.UpdateLlegada(Model.RecordLlegada);
                    db.SaveChanges();
                    Model.Record.Llegada = Model.RecordLlegada;
                    //Model.Record.Estado =  service.GetStatus(new Status { Name = "Guardada", StatusType = new StatusType { Name = "OperacionCecoa" } }).First();
                    Model.Record.Estado =  db.Estado.FirstOrDefault(f=>f.Nombre == "Guardada" && f.Tipo.Nombre == "OperacionCecoa");
                    Model.Record.FechaModificacion = DateTime.Now;
                    Model.Record.UsuarioModificacion = App.curUser.NombreUsuario;
                    //Si actualiza datos de aeronave, tipo factura o fechaOP
                    //service.UpdateOperacion(Model.Record);
                    db.SaveChanges();

                    Util.ShowMessage("Se actualizaron exitosamente los datos de llegada");

                }
                //Si es nuevo registro
                else
                {
                    //Status Guardada Para Llegada
                    int estadoId;
                    estadoId = Model.StatusLlegadaSalidaGuardada.RowID;
                    Model.RecordLlegada.EstadoID = estadoId;
                    //Datos de Creacion de la Llegada
                    Model.RecordLlegada.FechaCreacion = DateTime.Now;
                    Model.RecordLlegada.UsuarioCreacion = App.curUser.NombreUsuario;
                    //Guardo Llegada
                    //Model.RecordLlegada = service.SaveLlegada(Model.RecordLlegada);
                    db.SaveChanges();
                    Model.Record.Llegada = new Llegada();
                    Model.Record.Llegada.RowID = Model.RecordLlegada.RowID;
                    //Status Guardada Para Operacion
                    //Model.Record.Estado = service.GetStatus(new Status { Name = "Guardada", StatusType = new StatusType { Name = "OperacionCecoa" } }).First();
                    //Model.Record.Estado =  db.Estado.FirstOrDefault(f=>f.Nombre == "Guardada" && f.Tipo.Nombre == "OperacionCecoa");
                    estadoId = Model.StatusOperacionGuardada.RowID;
                    Model.Record.EstadoID = estadoId;
                    //Datos de Creacion de la Operacion
                    Model.Record.FechaCreacion = DateTime.Now;
                    Model.Record.UsuarioCreacion = App.curUser.NombreUsuario;
                    ///a solicitud del aeropuerto se muestra la fecha de modificacion en la lista de vuelos, por eso cuando se crea inmediatamente se le ponen tambien
                    ///los datos de modificacion
                    Model.Record.FechaModificacion = DateTime.Now;
                    Model.Record.UsuarioModificacion = App.curUser.NombreUsuario;
                    //Guardo Operacion
                    //Model.Record = service.SaveOperacion(Model.Record);
                    Model.Record = db.Operacion.Add(Model.Record);
                    db.SaveChanges();
                    BtnConfirmarLlegada.IsEnabled = true;
                    Util.ShowMessage("Se registraron exitosamente los datos de llegada");
                }
                txt_NumVueloSalida.Focus();
            }
        }

        private void btnConfirmarDatosSalida_Click_1(object sender, RoutedEventArgs e)
        {
            string mensaje = ValidarDatosVueloSalida();
            if (mensaje != "")
            {
                Util.ShowError(mensaje);
                NumVueloSalida.Focus();
                return;
            }
            string mensaje2 = this.validarHorasFechasSalida();
            if (mensaje2 != "")
            {
                Util.ShowError(mensaje2);
                NumVueloSalida.Focus();
                return;
            }
            SearchDestino_KeyUp_1(sender, null);
            Model.RecordSalida.TipoVueloSalidaID = TipoVueloSalida.SelectedItem != null ? ((Tipo)TipoVueloSalida.SelectedItem).RowID : 0;
            Model.RecordSalida.DestinoID = Destino.Aeropuertos != null ? Destino.Aeropuertos.RowID : 0;
            Model.RecordSalida.TipoPosicionSalidaID = TipoPosicionSalida.SelectedItem != null ? ((Tipo)TipoPosicionSalida.SelectedItem).RowID : 0;
            Model.RecordSalida.PosicionSalidaID = PosicionSalida.SelectedItem != null ? ((Tipo)PosicionSalida.SelectedItem).RowID : 0;
            Model.RecordSalida.NVueloSalida = !string.IsNullOrEmpty(NumVueloSalida.Text) ? NumVueloSalida.Text : "";

            ConfirmarRecordSalida(sender, e);
        }

        private void btnGuardarDatosSalida_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.RowID != 0)
            {
                int var = 0;
                //Cargo los datos que esten seteados al record
                Model.Record.Aeronave = Aeronave.Aeronaves;
                Model.Record.FechaOP = FechaOperacion.SelectedDate.Value;
                Model.Record.TipoFacturacionID = ((Tipo)TipoFactura.SelectedItem).RowID;
                //Guardo informacion de los combos si estan seleccionados
                Model.RecordSalida.TipoVueloSalidaID = TipoVueloSalida.SelectedItem != null ? ((Tipo)TipoVueloSalida.SelectedItem).RowID : var=0;
                Model.RecordSalida.DestinoID = Destino.Aeropuertos != null ? Destino.Aeropuertos.RowID : var = 0;
                Model.RecordSalida.TipoPosicionSalidaID = TipoPosicionSalida.SelectedItem != null ? ((Tipo)TipoPosicionSalida.SelectedItem).RowID : var = 0;
                Model.RecordSalida.PosicionSalidaID = PosicionSalida.SelectedItem != null ? ((Tipo)PosicionSalida.SelectedItem).RowID : var = 0;
                Model.RecordSalida.NVueloSalida = !string.IsNullOrEmpty(NumVueloSalida.Text) ? NumVueloSalida.Text : "";
                Model.RecordSalida.HoraProgramadaSalida = !string.IsNullOrEmpty(HoraProgramadaSalida.Text) ? HoraProgramadaSalida.Text :"";
                SearchDestino_KeyUp_1(sender, null);

                //Actualizar
                if (Model.RecordSalida.RowID != 0)
                {
                    //Datos Actualizacion Salida
                    Model.RecordSalida.FechaModificacion = DateTime.Now;
                    Model.RecordSalida.UsuarioModificacion = App.curUser.NombreUsuario;
                    //Actualizar Salida
                    //service.UpdateSalida(Model.RecordSalida);
                    db.SaveChanges();
                    Model.Record.Salida = Model.RecordSalida;
                    Model.Record.FechaModificacion = DateTime.Now;
                    Model.Record.UsuarioModificacion = App.curUser.NombreUsuario;
                    if(Model.Record.Facturado == false){//Si la operacion no se ha facturado
                        //Model.Record.Estado = service.GetStatus(new Status { Name = "Guardada", StatusType = new StatusType { Name = "OperacionCecoa" } }).First();
                        Model.Record.Estado = db.Estado.First(f=> f.Nombre == "Guardada" && f.Tipo.Nombre == "OperacionCecoa");
                    }
                    //Si actualiza datos de aeronave, tipo factura o fechaOP
                    //service.UpdateOperacion(Model.Record);
                    db.SaveChanges();
                    Util.ShowMessage("Se actualizaron exitosamente los datos de salida");
                }
                //Crear Nuevo
                else
                {
                    Model.RecordSalida.Sala = String.IsNullOrEmpty(txt_SalaSalida.Text) ? 0 : Convert.ToInt32(txt_SalaSalida.Text);
                    //Status Guardada Para Llegada
                    Model.RecordSalida.Estado = Model.StatusLlegadaSalidaGuardada;
                    //Datos Creacion Salida
                    Model.RecordSalida.FechaCreacion = DateTime.Now;
                    Model.RecordSalida.UsuarioCreacion = App.curUser.NombreUsuario;
                    //Guardo Salida
                    //Model.RecordSalida = service.SaveSalida(Model.RecordSalida);
                    Model.RecordSalida = db.Salida.Add(Model.RecordSalida);
                    db.SaveChanges();
                    if (Model.Record.Facturado == false)
                    {//Si la operacion no se ha facturado
                        //Model.Record.Estado = service.GetStatus(new Status { Name = "Guardada", StatusType = new StatusType { Name = "OperacionCecoa" } }).First();
                        Model.Record.Estado = db.Estado.FirstOrDefault(f=> f.Nombre == "Guardada" && f.Tipo.Codigo == "OperacionCecoa");
                    }
                    Model.Record.Salida = Model.RecordSalida;                    
                    Model.Record.FechaModificacion = DateTime.Now;
                    Model.Record.UsuarioModificacion = App.curUser.NombreUsuario;
                    //Actualizo el vuelo
                    //service.UpdateOperacion(Model.Record);
                    db.SaveChanges();
                    BtnConfirmarSalida.IsEnabled = true;
                    Util.ShowMessage("Se registraron exitosamente los datos de salida");
                }
                ListaTipoServicio.Focus();
            }
            else
            {
                Util.ShowError("Debe crear una Llegada para guardar una Salida");
                NumVuelo.Focus();
            }
        }

        private void btnCerrarTab_Click_1(object sender, RoutedEventArgs e)
        {
            CerrarTab(sender, e);
        }

        public String ValidarDatosVueloSalida()
        {
            String mensaje = "";
            if (string.IsNullOrEmpty(NumVueloSalida.Text))
            {
                mensaje = "Debe ingresar un numero de Vuelo Salida.";
            }
            else if (DTP_FechaSalida.SelectedDate == null)
            {
                mensaje = "Debe seleccionar una Fecha de Salida";
            }
            else if (Destino.Aeropuertos == null)
            {
                mensaje = "Debe Seleccionar un Destino.";
            }
            else if (string.IsNullOrEmpty(txtCargaSalida.Text))
            {
                mensaje = "Debe digitar una cantidad en Carga de Salida.";
            }
            else if (string.IsNullOrEmpty(txtCorreoSalida.Text))
            {
                mensaje = "Debe digitar una cantidad en Correo de Salida.";
            }

            else if (TipoVueloSalida.SelectedIndex == -1)
            {
                mensaje = "Debe Seleccionar el Tipo de vuelo Salida.";
            }
            else if (TipoPosicionSalida.SelectedIndex == -1)
            {
                mensaje = "Debe Seleccionar el Tipo de Posicion de Salida.";
            }
            else if (stkPosicionSalida.IsEnabled == true && PosicionSalida.SelectedIndex == -1)
            {
                mensaje = "Debe Seleccionar la Posicion de Salida.";
            }
            else if (((Tipo)TipoPosicionSalida.SelectedItem).Codigo == "PUENTE" && HoraPuenteSalida.Text.Contains("_"))
            {
                mensaje = "Debe ingresar una hora de Salida a Puente";
            }
            else if (((Tipo)TipoFactura.SelectedItem).Codigo != "CONTADO")
            {
                mensaje = validacionesCamposCreditoSalida();
            }
            else if (((Tipo)TipoFactura.SelectedItem).Codigo == "CONTADO" )
            {
                mensaje = validacionesCamposContadoSalida();
            }
            return mensaje;



        }

        public String validacionesCamposCreditoSalida()
        {
            String mensaje = "";
            if (HoraProgramadaSalida.Text.Contains("_"))
            {
                mensaje = "Debe ingresar una hora programada de Salida";
            }
            else if (MaskedHoraSalidaPlataforma.Text.Contains("_"))
            {
                mensaje = "Debe ingresar una hora salida Plataforma";
            }
            else if (MaskedHoraDespegue.Text.Contains("_"))
            {
                mensaje = "Debe ingresar una hora Despegue";
            }
            else if (((Tipo)TipoPosicionSalida.SelectedItem).Codigo == "PUENTE" && (HoraPuenteSalida.Text.Contains("_")))
            {
                mensaje = "Debe ingresar una hora de Salida a Puente";
            }
            else if (DTP_FechaProgramadaSal.SelectedDate == null)
            {
                mensaje = "Seleccione fecha de programación Salida";
            }
            return mensaje;
        }

        public String validacionesCamposContadoSalida()
        {
            String mensaje = "";
            if (((Tipo)TipoPosicionSalida.SelectedItem).Codigo == "PUENTE" && (HoraPuenteSalida.Text.Contains("_")))
            {
                mensaje = "Debe ingresar una hora de Salida a Puente Aproximada";
            }
            else if (MaskedHoraSalidaPlataforma.Text.Contains("_"))
            {
                mensaje = "Debe ingresar una hora salida Plataforma Aproximada";
            }
            else if (MaskedHoraDespegue.Text.Contains("_"))
            {
                mensaje = "Debe ingresar una hora Despegue Aproximada";
            }
            return mensaje;
        }

        //Cuando es credito
        private void CerrarOperacion_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.Llegada == null || Model.Record.Salida == null)
            {
                Util.ShowError("Debe Crear Llegada y Salida.");
                return;
            }
            //else if (service.GetTRM(new TRM { FechaFiltro = FechaSalida.SelectedDate }).Count == 0)
            else if (TraerTRM(FechaSalida.SelectedDate.Value).Count == 0)
            {
                Util.ShowError("No existe TRM para la fecha seleccionada");
                return;
            }
            else if (Model.Record.Llegada.Estado.RowID != Model.StatusLlegadaSalidaConfirmada.RowID || Model.Record.Salida.Estado.RowID != Model.StatusLlegadaSalidaConfirmada.RowID)
            {
                Util.ShowError("Debe confirmar Llegada y Salida.");
                return;
            }
            else if (Model.RecordTasas.RowID == 0)
            {
                if (UtilWindow.ConfirmOK("No tiene tasas registradas, Desea continuar?") == false)
                {
                    return;
                }
            }


            //Valido que haya Liquidado
            Estado statusOPliquidada = Model.StatusOperacionLiquidada;
            if (Model.Record.Estado.RowID != statusOPliquidada.RowID)
            {
                Util.ShowError("Debe Liquidar la Operacion.");
                return;
            }

            //Si La operacion tiene adicionales de Puentes y parqueo
            //if (service.GetAdicionalesPyP(new AdicionalesPyP { Operacion = Model.Record, Status = Model.StatusAdicionalesConfirmado }).Count() >= 1)
            if (db.AdicionalesPyP.Where(f=> f.OperacionID == Model.Record.RowID && f.EstadoID == Model.StatusAdicionalesConfirmado.RowID).Count() >= 1)
            {
                //Para calcular el primer adicional
                CalcularFacturacionContadoConAdicionales(sender, e);
                //Para calcular el resto de adicionales y sumarlos
                this.CalcularYSumarAdicionales();
                //Sumo los valores que estan en pantalla
                CalcularTotalFacturacion(sender, e);
            }
            else
            {
                CalcularFacturacionContado(sender, e);
            }
            Estado aux = Model.StatusServiciosParaFacturar;
            //Estado auxBomberos = service.GetStatus(new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "ServicioBomberos" } }).First();
            Estado auxBomberos = db.Estado.FirstOrDefault(f=> f.Nombre == "ParaFacturar" && f.Tipo.Nombre == "ServicioBomberos");
            //Le envio el status y false para que no genere factura
            this.facturarServicios(aux, auxBomberos, false);
            Btn_CerrarOperacion.IsEnabled = false;
        }

        private void btnActualizarOperacion_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.RowID == 0)
            {
                Util.ShowError("Debe Crear una Operacion");
            }
            ActualizarDatosOperacion(sender, new DataEventArgs<Operacion>(Model.Record));

        }

        private void BtnAbrirOperacion_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.Facturado == true)
            {
                if (Model.Record.Tipo != null)
                {
                    if (Model.Record.Tipo.Codigo != "CONTADO")
                    {
                        //Validar que el usuario con el que esta ingresando sea MasterCecoa, JefeCecoa, o Admin
                        //foreach (UserByRol rol in App.curUser.UserRols)
                        //{
                            if (App.curUser.Rol.Nombre == "Administrador" || App.curUser.Rol.Nombre == "MasterCecoa" || App.curUser.Rol.Nombre == "JefeCecoa")//VAlido Rol
                            {
                                if (UtilWindow.ConfirmOK("Está seguro de que quiere Abrir la Operación?") == false)
                                {
                                    return;
                                }

                            //IList<Servicios> ListaServicios = service.GetServicios(new Servicios { Operacion = Model.Record }).ToList();
                            //IList<Servicios> ListaServicios = service.GetServicios(new Servicios { Operacion = Model.Record }).Where(f => f.Estado.Nombre != "Anulada").ToList();
                            IList<Servicios> ListaServicios = db.Servicios.Where(f => f.Operacion == Model.Record && f.Estado.Nombre != "Anulada").ToList();

                                //Verificar si tiene servicios creados con estado diferente a ParaFacturar
                                foreach (Servicios servicio1 in ListaServicios)
                                {
                                    if (servicio1.Estado.RowID == Model.StatusServiciosParaFacturar.RowID || servicio1.Estado.RowID == Model.StatusServiciosAnulada.RowID)
                                    { 
                                    
                                    }
                                    else
                                    {
                                        Util.ShowError("No puede Abrir esta Operación, ya se facturó uno de sus servicios");
                                        return;
                                    }
                                }


                                //Cambio Status de los Servicios a Anulado
                                foreach (Servicios servicio in ListaServicios)
                                {

                                    if (servicio.Estado.RowID == Model.StatusServiciosParaFacturar.RowID)
                                    {
                                        servicio.Estado = Model.StatusServiciosAnulada;
                                        //service.UpdateServicios(servicio);
                                        db.SaveChanges();
                                    }
                                }

                                //Le cambio el estado a las tasas 
                                this.cambiarStatusTasas(Model.StatusTasasNueva);

                                //Cambio el status de los bomberos a Nuevo
                                if (Model.Record.Tipo.Codigo != "CONTADO")
                                {
                                    this.cambiarStatusYFacturaBomberos(Model.StatusBomberosNuevo, Model.StatusTasasParaFacturar.Nombre);
                                }
                                else
                                {
                                    this.cambiarStatusYFacturaBomberos(Model.StatusBomberosNuevo, Model.StatusBomberosParaEnviarERP.Nombre);

                                }

                                //Actualizo El vuelo A Facturado = False
                                Model.Record.Facturado = false;
                            //Model.Record.Estado = service.GetStatus(new Status { Name = "Guardada", StatusType = new StatusType { Name = "OperacionCecoa" } }).First();
                                Model.Record.Estado = db.Estado.FirstOrDefault(f => f.Nombre == "Guardada"  && f.Tipo.Nombre == "OperacionCecoa");
                                //service.UpdateOperacion(Model.Record);
                                db.SaveChanges();
                                //Para abrir la operacion y que puedan modificar
                                Model.RecordLlegada.Estado = Model.StatusLlegadaSalidaGuardada;
                                //service.UpdateLlegada(Model.RecordLlegada);
                                db.SaveChanges();
                                Model.RecordSalida.Estado = Model.StatusLlegadaSalidaGuardada;
                                //service.UpdateSalida(Model.RecordSalida);
                                db.SaveChanges();
                                //Habilito los campos para que los pueda editar
                                BotonFacturar.IsEnabled = true;
                                PanelDatosOperacionLlegada.IsEnabled = true;
                                PanelDatosOperacionSalida1.IsEnabled = true;
                                PanelDatosOperacionSalida2.IsEnabled = true;
                                PanelDatosOperacionSalida3Contado.IsEnabled = true;
                                PanelDatosCabecera.IsEnabled = true;
                                PanelDatosLiquidacionTasas.IsEnabled = true;
                                BtnGuardarLlegada.IsEnabled = true;
                                BtnGuardarSalida.IsEnabled = true;
                                BtnConfirmarLlegada.IsEnabled = true;
                                BtnConfirmarSalida.IsEnabled = true;
                                PanelDatosBomberos.IsEnabled = true;
                                BtnCerrarOperacion.IsEnabled = true;
                                Util.ShowMessage("La operación se abrió correctamente");


                            }
                            else
                            {
                                Util.ShowError("No puede Abrir esta Operación.");
                                return;
                            }
                        //}
                    }
                    else
                    {
                        Util.ShowError("No puede Abrir una Operación Facturada.");
                        return;
                    }
                }
                return;
            }
            if (Model.Record.Llegada == null)
            {
                Util.ShowError("Debe Crear Llegada");
                return;
            }
            else if (Model.Record.Llegada.Estado == null)
            {         return;       }

            //Para la llegada
            if (Model.Record.Llegada.Estado.Nombre == Model.StatusLlegadaSalidaConfirmada.Nombre)
            {
                //Para abrir la operacion y que puedan modificar
                Model.RecordLlegada.Estado = Model.StatusLlegadaSalidaGuardada;
                //service.UpdateLlegada(Model.RecordLlegada);
                                db.SaveChanges();
                //Para Saber quien fue la ultima persona que modifico la operacion
                Model.Record.FechaModificacion = DateTime.Now;
                Model.Record.UsuarioModificacion = App.curUser.NombreUsuario;
                //service.UpdateOperacion(Model.Record);
                                db.SaveChanges();
                ActualizarDatosOperacion(sender, new DataEventArgs<Operacion>(Model.Record));
            }
            if (Model.Record.Salida != null)
            {
                if (Model.Record.Salida.Estado == null)
                { return; }
                //Para la salida
                if (Model.Record.Salida.Estado.Nombre == Model.StatusLlegadaSalidaConfirmada.Nombre)
                {
                    //Para abrir la operacion y que puedan modificar
                    Model.RecordSalida.Estado = Model.StatusLlegadaSalidaGuardada;
                    //service.UpdateSalida(Model.RecordSalida);
                                db.SaveChanges();
                    //Para Saber quien fue la ultima persona que modifico la operacion
                    //Model.Record.Estado = service.GetStatus(new Status { Name = "Guardada", StatusType = new StatusType { Name = "OperacionCecoa" } }).First();
                    Model.Record.Estado = db.Estado.FirstOrDefault(f => f.Nombre == "Guardada"  && f.Tipo.Nombre == "OperacionCecoa");
                    Model.Record.FechaModificacion = DateTime.Now;
                    Model.Record.UsuarioModificacion = App.curUser.NombreUsuario;
                    //service.UpdateOperacion(Model.Record);
                    db.SaveChanges();
                    ActualizarDatosOperacion(sender, new DataEventArgs<Operacion>(Model.Record));
                }
            }
            if (Model.RegistroAdicionalesPyPList.Count != 0)
            {
                foreach(AdicionalesPyP adicional in Model.RegistroAdicionalesPyPList){
                    if (adicional.Estado.Nombre == Model.StatusAdicionalesConfirmado.Nombre)
                    {
                        //Para abrir los adicionales y que puedan modificar
                        adicional.Estado = Model.StatusAdicionalesGuardado;
                        //Para Saber quien fue la ultima persona que modifico la operacion
                        adicional.FechaModificacion = DateTime.Now;
                        adicional.UsuarioModificacion = App.curUser.NombreUsuario;
                        //service.UpdateAdicionalesPyP(adicional);
                        db.SaveChanges();

                    }
                }
            }


        }

        private void cmb_TipoOperacion_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                SearchCompañiaFactura.Focusable = true;
                SearchCompañiaFactura.Focus();
            }
        }

        private void SearchCompañiaFactura_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                SearchAeropuertoOrigen.txtData.Focus();
            }
        }

        #endregion

        #region Adicionados
        //-
        #region servicioBomberos


        private void btnGuardarServicioBomberos_Click(object sender, RoutedEventArgs e)
        {
            guardarServicioBomberos(sender, e);
        }


        private void btnEliminarServicioBomberos_Click(object sender, RoutedEventArgs e)
        {
            EliminarServicioBomberos(sender, e);
        }


        //Servicios Bomberos Adicionales



        #endregion

        #region ServiciosBomberos Adicionales

        private void btnGuardarServicioBomberosAdicional_Click(object sender, RoutedEventArgs e)
        {
            //Valido que exista el vuelo
            if (Model.Record.RowID == 0)
            {
                Util.ShowError("Primero debe crear un vuelo");
                return;
            }
            if (Model.Record.Facturado == false)
            {
                Util.ShowError("Solo puede registrar BOMBEROS Adicionales cuando el vuelo ya se facturo");
                return;
            }
            //Si es registro nuevo
            //Valido que seleccione Tipo de servicio y Fecha
            if (tipoBomberoAdicional.SelectedIndex == -1)
            {
                Util.ShowError("Debe seleccionar un tipo de servicio");
                return;
            }
            if (DTP_FechaServicioAdicional.SelectedDate == null)
            {
                Util.ShowError("Debe seleccionar una fecha");
                return;
            }
            if (DTP_FechaServicioAdicional.SelectedDate < Model.Record.FechaOP)
            {
                Util.ShowError("Fecha Servicio Bomberos Adicionales no valida");
                DTP_FechaServicioAdicional.Focus();
                return;
            }
            TarifaCecoa TarifaBomberos;
            // Verifico si existe una tarifa BOMBERO para la fecha seleccionada
            //if (service.GetTarifas(new Tarifas { FechaFiltro = DTP_FechaServicioAdicional.SelectedDate, TipoTarifa = new MMaster { Code = "BOMBEROS" }, TipoServicio = (Tipo)tipoBomberoAdicional.SelectedItem }).Count == 1)
            if (TraerTarifa(DTP_FechaServicioAdicional.SelectedDate.Value, "BOMBEROS").Where(f => f.TipoServicioID == ((Tipo)tipoBomberoAdicional.SelectedItem).RowID).Count() == 1)
            {
                //TarifaBomberos = service.GetTarifas(new Tarifas { FechaFiltro = DTP_FechaServicioAdicional.SelectedDate, TipoTarifa = new MMaster { Code = "BOMBEROS" }, TipoServicio = (Tipo)tipoBomberoAdicional.SelectedItem }).First();
                TarifaBomberos = TraerTarifa(DTP_FechaServicioAdicional.SelectedDate.Value, "BOMBEROS").Where(f => f.TipoServicioID == ((Tipo)tipoBomberoAdicional.SelectedItem).RowID).First();
                //Asigno el Valor del servicio
                Model.RecordBomberosAdicional.ValorServicio = TarifaBomberos.ValorCOP;
            }
            else
            {
                Util.ShowError("No cuenta con una tarifa Bomberos disponible");
                return;
            }
                
            //Le asigno el vuelo actual
            Model.RecordBomberosAdicional.Operacion = Model.Record;
            Model.RecordBomberosAdicional.Fecha = DTP_FechaServicioAdicional.SelectedDate;
            Model.RecordBomberosAdicional.TipoServicioBombID = ((Tipo)tipoBomberoAdicional.SelectedItem).RowID;
            //Model.RecordBomberosAdicional.Estado = service.GetStatus(new Status { Name = "Nuevo", StatusType = new StatusType { Name = "ServicioBomberosAdicional" } }).First();
            Model.RecordBomberosAdicional.Estado = db.Estado.FirstOrDefault(f=> f.Nombre == "Nuevo" && f.Tipo.Nombre == "ServicioBomberosAdicional");
            Model.RecordBomberosAdicional.Activo = true;
            //Asigno variables de Creacion
            Model.RecordBomberosAdicional.UsuarioCreacion = App.curUser.NombreUsuario;
            Model.RecordBomberosAdicional.FechaCreacion = DateTime.Now;
            //Guardo el registro
            //service.SaveBomberos(Model.RecordBomberosAdicional);
            db.Bombero.Add(Model.RecordBomberosAdicional);
            db.SaveChanges();
            Util.ShowMessage("Se registro exitosamente el Servicio Bomberos");
            //Actualizar Lista bomberos
            //Model.RegistroBomberosAdicionalesList = service.GetBomberos(new Bomberos { Operacion = Model.Record }).Where(f => f.Status.StatusType.Nombre == "ServicioBomberosAdicional").ToList();
            Model.RegistroBomberosAdicionalesList = db.Bombero.Where(f=> f.OperacionID == Model.Record.RowID && f.Tipo.Nombre == "ServicioBomberosAdicional").ToList();
            DTP_FechaServicioAdicional.SelectedDate = Model.Record.FechaOP;
            tipoBomberoAdicional.SelectedIndex = -1;
            Model.RecordBomberosAdicional = new Bombero();
        }

        private void btnEliminarBomberoAdicional_Click_1(object sender, RoutedEventArgs e)
        {
            //Valido que exista el vuelo
            if (Model.Record.RowID == 0)
            {
                return;
            }
            foreach (Bombero ServicioBombero in lvRegistroServicioBomberosAdicionales.SelectedItems)
            {
                if (ServicioBombero.Estado.Nombre == "Nuevo")
                {
                    //service.DeleteBomberos(ServicioBombero);
                    db.Bombero.Remove(ServicioBombero);
                    db.SaveChanges();
                }
                else
                {
                    Util.ShowError("No puede eliminar este registro");
                }
            }
            //Model.RegistroBomberosAdicionalesList = service.GetBomberos(new Bomberos { Operacion = Model.Record }).Where(f => f.Status.StatusType.Nombre == "ServicioBomberosAdicional").ToList();
            Model.RegistroBomberosAdicionalesList = db.Bombero.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Nombre == "ServicioBomberosAdicional").ToList();
        }

        

        #endregion

        #region FacturacionAdicional

        private void FacturarServiciosAdicionales_Click_1(object sender, RoutedEventArgs e)
        {
            if (Model.Record.RowID == 0)
            {
                Util.ShowError("Debe Crear una Operacion");
                return;
            }
            if (Model.Record.Facturado == false)
            {
                Util.ShowError("Solo puede Facturar Adicionales cuando el vuelo ya se facturo");
                return;
            }
            //Si es credito o abonos no le asocio Factura
            if (Model.Record.Tipo.Codigo != "CONTADO")
            {
                GenerarFacturacionServiciosAdicionales(Model.StatusServiciosParaFacturar);
            }
        }

        public void GenerarFacturacionServiciosAdicionales(Estado NuevoStatus)
        {
            Facturas NroFactura = null; //Factura que se le va a asociar a los servicios
            int cantAsistencia = 0, cantLimpieza = 0, cantTasas = 0, cantPyP = 0;
            //Recorro los registros para ver si hay algo para facturar
            cantAsistencia = Model.RegistroBomberosAdicionalesList.Where(f => f.Tipo.Codigo == "ASISTENCIA" && f.Estado.Nombre == "Nuevo").Count();
            cantLimpieza = Model.RegistroBomberosAdicionalesList.Where(f => f.Tipo.Codigo == "LIMPIEZA" && f.Estado.Nombre == "Nuevo").Count();
            cantTasas = Model.RegistroTasasAdicionalesList.Where(f => f.Estado.Nombre == Model.StatusTasasNueva.Nombre).Count();
            //cantPyP = Model.RegistroAdicionalesPyPList.Where(f => f.Estado.Nombre == Model.StatusAdicionalesConfirmado.Nombre).Count();
            //Si no hay bomberos, ni tasas registrados 
            if (cantAsistencia == 0 && cantLimpieza == 0 && cantTasas == 0)
            {
                Util.ShowError("No hay Datos a Facturar");
                return;
            }
                        //Creo servicio para tasas
            if (cantTasas != 0)
            {
                //Agrupo Tasas por tipo Debito o credito
                int cantCredito = 0, cantDebito = 0;
                cantCredito = Model.RegistroTasasAdicionalesList.Where(f => f.Estado.Nombre == Model.StatusTasasNueva.Nombre && f.Tipo.Codigo == "CREDITO").Count();
                cantDebito = Model.RegistroTasasAdicionalesList.Where(f => f.Estado.Nombre == Model.StatusTasasNueva.Nombre && f.Tipo.Codigo == "DEBITO").Count();
                if (cantCredito != 0)
                {
                    Servicios tasas = new Servicios();
                    //int paganTasa = Model.RegistroTasasAdicionalesList.Where(f => f.Estado.Nombre == Model.StatusTasasNueva.Nombre && f.Tipo.Codigo == "CREDITO").Sum(f => f.PaganTasa);
                    int paganTasa = Model.RegistroTasasAdicionalesList.Where(f => f.Estado.Nombre == Model.StatusTasasNueva.Nombre && f.Tipo.Codigo == "CREDITO").Sum(f => f.PaganTasa).Value;
                    tasas.Fecha = DateTime.Now;
                    //Los que pagan tasa de tipo Credito
                    tasas.Cantidad = paganTasa;
                    //Si es Nacional le asigno la tarifa Nacional
                    if (Model.RecordSalida.Tipo2.Codigo == "NACIONAL")
                    {
                        //*****ARREGLAR*********/ tasas.Valor = paganTasa * Model.RecordTasas.TasaCOP;
                    }
                    //Si es InterNacional le asigno la tarifa Inter
                    else
                    {
                        //TRM trm = service.GetTRM(new TRM { FechaFiltro = Model.Record.Salida.FechaSalida }).First();
                        TRM trm = TraerTRM(Model.Record.Salida.FechaSalida.Value).FirstOrDefault();
                        //*****ARREGLAR*********/tasas.Valor = paganTasa * Model.RecordTasas.TasaUSD * trm.Valor.Value;
                    }
                    //Si es Credito le asigno tipo TASAS
                    //tasas.TipoServicioID = service.GetMMaster(new MMaster { Code = "TASASCREDITO" }).First();
                    tasas.TipoServicioID = db.Tipo.FirstOrDefault(f => f.Codigo == "TASASCREDITO").RowID;
                    tasas.Operacion = Model.Record;
                    tasas.FacturaID = NroFactura.RowID;
                    tasas.Estado = NuevoStatus;
                    tasas.UsuarioCreacion = App.curUser.NombreUsuario;
                    tasas.FechaCreacion = DateTime.Now;
                    //service.SaveServicios(tasas);
                    db.Servicios.Add(tasas);
                    db.SaveChanges();

                }
                if (cantDebito != 0)
                {
                    Servicios tasas = new Servicios();
                    int paganTasa = Model.RegistroTasasAdicionalesList.Where(f => f.Estado.Nombre == Model.StatusTasasNueva.Nombre && f.Tipo.Codigo == "DEBITO").Sum(f => f.PaganTasa).Value;
                    tasas.Fecha = DateTime.Now;
                    tasas.Cantidad = paganTasa;
                    //Si es Nacional le asigno la tarifa Nacional
                    if (Model.RecordSalida.Tipo2.Codigo == "NACIONAL")
                    {
                       //*****ARREGLAR*********/ tasas.Valor = paganTasa * Model.RecordTasas.TasaCOP;
                    }
                    //Si es InterNacional le asigno la tarifa Inter
                    else
                    {
                        //TRM trm = service.GetTRM(new TRM { FechaFiltro = Model.Record.Salida.FechaSalida }).First();
                        TRM trm = TraerTRM(Model.Record.Salida.FechaSalida.Value).First();
                        //*****ARREGLAR*********/ tasas.Valor = paganTasa * Model.RecordTasas.TasaUSD * trm.Valor;
                    }
                    //Si es Debito le asigno tipo TASASDEBITO
                    //tasas.TipoServicioID = service.GetMMaster(new MMaster { Code = "TASASDEBITO" }).First();
                    tasas.TipoServicioID = db.Tipo.FirstOrDefault(f => f.Codigo == "TASASDEBITO").RowID;
                    tasas.Operacion = Model.Record;
                    tasas.FacturaID = NroFactura.RowID;
                    tasas.Estado = NuevoStatus;
                    tasas.UsuarioCreacion = App.curUser.NombreUsuario;
                    tasas.FechaCreacion = DateTime.Now;
                    //service.SaveServicios(tasas);
                    db.Servicios.Add(tasas);
                    db.SaveChanges();
                }
            }
            //Creo servicio para bomberos asistencia
            if (cantAsistencia != 0)
            {
                double acumAsistencia = Model.RegistroBomberosAdicionalesList.Where(f => f.Tipo.Codigo == "ASISTENCIA" && f.Estado.Nombre == "Nuevo").Sum(f => f.ValorServicio).Value;
                Servicios asistenciaBomberos = new Servicios();
                asistenciaBomberos.Fecha = DateTime.Now;
                asistenciaBomberos.Cantidad = cantAsistencia;
                asistenciaBomberos.Valor = acumAsistencia;
                //asistenciaBomberos.TipoServicio = service.GetMMaster(new MMaster { Code = "ASISTENCIA" }).First();
                asistenciaBomberos.TipoServicioID = db.Tipo.FirstOrDefault(f => f.Codigo == "ASISTENCIA").RowID;
                asistenciaBomberos.Operacion = Model.Record;
                asistenciaBomberos.Facturas = NroFactura;
                asistenciaBomberos.Estado = NuevoStatus;
                asistenciaBomberos.UsuarioCreacion = App.curUser.NombreUsuario;
                asistenciaBomberos.FechaCreacion = DateTime.Now;
                //service.SaveServicios(asistenciaBomberos);
                db.Servicios.Add(asistenciaBomberos);
                db.SaveChanges();
            }
            //Creo servicio para bomberos limpieza
            if (cantLimpieza != 0)
            {
                double acumLimpieza = Model.RegistroBomberosAdicionalesList.Where(f => f.Tipo.Codigo == "LIMPIEZA" && f.Estado.Nombre == "Nuevo").Sum(f => f.ValorServicio).Value;
                Servicios limpiezaBomberos = new Servicios();
                limpiezaBomberos.Fecha = DateTime.Now;
                limpiezaBomberos.Cantidad = cantLimpieza;
                limpiezaBomberos.Valor = acumLimpieza;
                //limpiezaBomberos.TipoServicio = service.GetMMaster(new MMaster { Code = "LIMPIEZA" }).First();
                limpiezaBomberos.TipoServicioID = db.Tipo.FirstOrDefault(f => f.Codigo == "LIMPIEZA").RowID;
                limpiezaBomberos.Operacion = Model.Record;
                limpiezaBomberos.Facturas = NroFactura;
                limpiezaBomberos.Estado = NuevoStatus;
                limpiezaBomberos.UsuarioCreacion = App.curUser.NombreUsuario;
                limpiezaBomberos.FechaCreacion = DateTime.Now;
                //service.SaveServicios(limpiezaBomberos);
                db.Servicios.Add(limpiezaBomberos);
                db.SaveChanges();
            }

            //Cambio el status a las Tasas 
            if (cantTasas != 0)
            {
                foreach (Tasas tasas in lvRegistroServicioTasaAdicional.Items)
                {
                    //Si el estado es nueva
                    if (tasas.Estado.Nombre == Model.StatusTasasNueva.Nombre)
                    {
                        tasas.Estado = Model.StatusTasasParaFacturar; // Le asigno para Facturar
                        //service.UpdateTasas(tasas);
                        db.SaveChanges();
                    }
                }
                //Model.RegistroTasasAdicionalesList = service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo != "NORMAL").ToList();
                Model.RegistroTasasAdicionalesList = db.Tasas.Where(f=> f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "NORMAL").ToList();
            }

            //Cambio el Status de Bomberos Adicionales
            //Estado auxBomberos = service.GetStatus(new Status { Name = "ParaEnviarERP", StatusType = new StatusType { Name = "ServicioBomberosAdicional" } }).First();
            Estado auxBomberos = db.Estado.FirstOrDefault(f=> f.Nombre == "ParaEnviarERP" && f.Tipo.Nombre == "ServicioBomberosAdicional");
            foreach (Bombero ServicioBombero in lvRegistroServicioBomberosAdicionales.Items)
            {
                if (ServicioBombero.Estado.Nombre == "Nuevo")
                {
                    //ServicioBombero.Estado = service.GetStatus(new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "ServicioBomberosAdicional" } }).First();
                    ServicioBombero.Estado = db.Estado.FirstOrDefault(f=> f.Nombre == "ParaEnviarERP" && f.Tipo.Nombre == "ServicioBomberosAdicional");
                    //service.UpdateBomberos(ServicioBombero);
                    db.SaveChanges();
                }
                //Model.RegistroBomberosAdicionalesList = service.GetBomberos(new Bomberos { Operacion = Model.Record, Activo = true }).Where(f => f.Status.StatusType.Nombre == "ServicioBomberosAdicional").ToList();
                Model.RegistroBomberosAdicionalesList = db.Bombero.Where(f =>f.OperacionID == Model.Record.RowID && f.Activo == true &&  f.Estado.Tipo.Nombre == "ServicioBomberosAdicional").ToList();
            }
            Util.ShowMessage("Adicionales Procesados Correctamente");
        }


        #endregion

        #region Tasas Adicionales

        private void btn_GuardarLiquidacionAdicional_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.Facturado == true)
            {
                //Valido si existe un servicio de tasas que este paraFacturar 
                //Servicios ServicioTasas = service.GetServicios(new Servicios { Operacion = new Operacion { RowID = Model.Record.RowID }, Status = new Status { StatusID = Model.StatusServiciosParaFacturar.RowID }, TipoServicio = new MMaster { Code = "TASAS"} }).FirstOrDefault();
                Servicios ServicioTasas = db.Servicios.FirstOrDefault(f=> f.OperacionID == Model.Record.RowID && f.Estado.RowID == Model.StatusServiciosParaFacturar.RowID && f.Tipo.Codigo == "TASAS");
                if (ServicioTasas.Facturas == null)
                {
                    Util.ShowError("Solo puede registrar NOTAS cuando el vuelo ya se facturo");
                    return;
                }
                //if (service.GetTarifas(new TarifaCecoa { FechaFiltro = Model.Record.Salida.FechaSalida , TipoTarifa = new MMaster { Code = "TASAS"} }).Count() ==0)
                if (TraerTarifa( Model.Record.Salida.FechaSalida.Value, "TASAS").Count() ==0)
                {
                    Util.ShowError("No hay una tarifa TASAS Activa para la fecha seleccionada.");
                    return;
                }
                if(Model.Record.Salida.Tipo2.Codigo == "INTERNACIONAL"){
                    //if (service.GetTRM(new TRM { FechaFiltro = Model.Record.Salida.FechaSalida }).Count() == 0)
                    if (TraerTRM(Model.Record.Salida.FechaSalida.Value).Count() == 0)
                    {
                        Util.ShowError("No hay un TRM Activo para la fecha de seleccionada.");
                        return;
                    }
                }
                //Valido que seleccione el tipo
                if (TipoTasaAdicional.SelectedIndex == -1)
                {
                    Util.ShowError("Debe seleccionar un tipo de Tasa");
                    return;
                }
                string mensaje = validacionesTasasAdicionales();
                if (mensaje != "")
                {
                    Util.ShowError(mensaje);
                    return;
                }
                //Busco Tasas Adicionales que esten en status Nueva
                //if (service.GetTasas(new Tasas { Operacion = Model.Record}).Where(f => f.TipoTasa.Codigo != "NORMAL").ToList().Count >= 1)
                if (db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "NORMAL").ToList().Count >= 1)
                {
                    Util.ShowError("Solo puede Ingresar una nota por operacion");
                    return;
                }
                //Le asigno el vuelo actual
                Model.RecordTasasAdicionales.Operacion = Model.Record;
                Model.RecordTasasAdicionales.TipoTasaID = ((Tipo)TipoTasaAdicional.SelectedItem).RowID;
                Model.RecordTasasAdicionales.Fecha = DateTime.Now;
                Model.RecordTasasAdicionales.Estado = Model.StatusTasasNueva;
                //Asigno variables de Creacion
                Model.RecordTasasAdicionales.UsuarioCreacion = App.curUser.NombreUsuario;
                Model.RecordTasasAdicionales.FechaCreacion = DateTime.Now;
                //Guardo el registro
                //service.SaveTasas(Model.RecordTasasAdicionales);
                db.Tasas.Add(Model.RecordTasasAdicionales);
                db.SaveChanges();
                Util.ShowMessage("Se registro Exitosamente");
                //Actualizar Lista bomberos
                //Model.RegistroTasasAdicionalesList = service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo != "NORMAL").ToList();
                Model.RegistroTasasAdicionalesList = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "NORMAL").ToList();
                //Model.RegistroTasasList = service.GetTasas(new Tasas { Operacion = Model.Record });
                Model.RegistroTasasList = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID).ToList();
                TipoTasaAdicional.SelectedIndex = -1;
                Model.RecordTasasAdicionales = new Tasas();
                CalcularDatosLiquidacion(sender, e);
            }
            else
            {
                Util.ShowError("Solo puede registrar NOTAS cuando el vuelo ya se facturo");
            }
        }

        private void btnEliminarTasaAdicional_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.RowID == 0)
            {
                return;
            }
            foreach (Tasas TasaAdicional in lvRegistroServicioTasaAdicional.SelectedItems)
            {
                //Si el estado es nuevo lo elimino
                if (TasaAdicional.Estado.Nombre == Model.StatusTasasNueva.Nombre)
                {
                    //service.DeleteTasas(TasaAdicional);
                    db.Tasas.Remove(TasaAdicional);
                    db.SaveChanges();
                }
                else {
                    Util.ShowError("No puede eliminar este registro.");
                }
            }
            //Model.RegistroTasasAdicionalesList = service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo != "NORMAL").ToList();
            Model.RegistroTasasAdicionalesList = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "NORMAL").ToList();
            //Model.RegistroTasasList = service.GetTasas(new Tasas { Operacion = Model.Record });
            Model.RegistroTasasList = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID).ToList();
            CalcularDatosLiquidacion(sender, e);
        }


        public string validacionesTasasAdicionales()
        {
            string mensaje = "";
            if (((Tipo)TipoTasaAdicional.SelectedItem).Codigo == "CREDITO")
            {
                int suma = 0;
                //Valido que la cantidad de pagan tasas sea menos o igual a la existente
                //suma = service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo != "CREDITO").Sum(f => f.PaganTasa) - service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo == "CREDITO").Sum(f => f.PaganTasa);
                suma = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "CREDITO").Sum(f => f.PaganTasa).Value - db.Tasas.Where(f => f.OperacionID == Model.Record.RowID  && f.Tipo.Codigo == "CREDITO").Sum(f => f.PaganTasa).Value;
                if ((Model.RecordTasasAdicionales.PaganTasa > suma) && Model.RecordTasasAdicionales.PaganTasa != 0)
                {
                    mensaje = "Cantidad de Pagan Tasa no valida";
                }
                //Valido que la cantidad de exentos a guardar sea menor o igual a la existente
                //suma = service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo != "CREDITO").Sum(f => f.Infantes) - service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo == "CREDITO").Sum(f => f.Infantes);
                suma = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "CREDITO").Sum(f => f.Infantes).Value - db.Tasas.Where(f => f.OperacionID == Model.Record.RowID  && f.Tipo.Codigo == "CREDITO").Sum(f => f.Infantes).Value;
                if (Model.RecordTasasAdicionales.Infantes > suma && Model.RecordTasasAdicionales.Infantes != 0)
                {
                    mensaje = "Cantidad de Infantes no valida";
                }
                //suma = service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo != "CREDITO").Sum(f => f.Tripulantes) - service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo == "CREDITO").Sum(f => f.Tripulantes);
                suma = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "CREDITO").Sum(f => f.Tripulantes).Value - db.Tasas.Where(f => f.OperacionID == Model.Record.RowID  && f.Tipo.Codigo == "CREDITO").Sum(f => f.Tripulantes).Value;
                if (Model.RecordTasasAdicionales.Tripulantes > suma && Model.RecordTasasAdicionales.Tripulantes != 0)
                {
                    mensaje = "Cantidad de Tripadi no valida";
                }
                //suma = service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo != "CREDITO").Sum(f => f.Militares) - service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo == "CREDITO").Sum(f => f.Militares);
                suma = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "CREDITO").Sum(f => f.Militares).Value - db.Tasas.Where(f => f.OperacionID == Model.Record.RowID  && f.Tipo.Codigo == "CREDITO").Sum(f => f.Militares).Value;
                if (Model.RecordTasasAdicionales.Militares > suma && Model.RecordTasasAdicionales.Militares != 0)
                {
                    mensaje = "Cantidad de Militares no valida";
                }
                //suma = service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo != "CREDITO").Sum(f => f.Transitos) - service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo == "CREDITO").Sum(f => f.Transitos);
                suma = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "CREDITO").Sum(f => f.Transitos).Value - db.Tasas.Where(f => f.OperacionID == Model.Record.RowID  && f.Tipo.Codigo == "CREDITO").Sum(f => f.Transitos).Value;
                if (Model.RecordTasasAdicionales.Transitos > suma && Model.RecordTasasAdicionales.Transitos != 0)
                {
                    mensaje = "Cantidad de Transitos no valida";
                }
                //suma = service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo != "CREDITO").Sum(f => f.Otros) - service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo == "CREDITO").Sum(f => f.Otros);
                suma = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "CREDITO").Sum(f => f.Otros).Value - db.Tasas.Where(f => f.OperacionID == Model.Record.RowID  && f.Tipo.Codigo == "CREDITO").Sum(f => f.Otros).Value;
                if (Model.RecordTasasAdicionales.Otros > suma && Model.RecordTasasAdicionales.Otros != 0)
                {
                    mensaje = "Cantidad de Otros no valida";
                }
            }
            //NORMAL y CREDITO
            else
            {
                //Suma de pasajeros totales
                int suma = Model.RecordTasasAdicionales.PaganTasa.Value + Model.RecordTasasAdicionales.Tripulantes.Value + Model.RecordTasasAdicionales.Militares.Value + Model.RecordTasasAdicionales.Transitos.Value + Model.RecordTasasAdicionales.Otros.Value;
                //Le adiciono a la suma los debitos menos los CREDITO
                //suma = suma + service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo != "CREDITO").Sum(f => f.PaganTasa) -
                //              service.GetTasas(new Tasas { Operacion = Model.Record }).Where(f => f.TipoTasa.Codigo == "CREDITO").Sum(f => f.PaganTasa);
                suma = suma + db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo != "CREDITO").Sum(f => f.PaganTasa.Value) -
                             db.Tasas.Where(f => f.OperacionID == Model.Record.RowID && f.Tipo.Codigo == "CREDITO").Sum(f => f.PaganTasa.Value);
                if (Model.Record.Aeronave != null)
                {
                    //Traigo el peso de la aeronave, por si se actualiza el peso de la aeronave despues de que se cargo el vuelo
                    //Aeronave aeronaveActualizada = service.GetAeronaves(new Aeronaves { RowID = Model.Record.Aeronave.RowID }).First();
                    Aeronave aeronaveActualizada = db.Aeronave.First(f => f.RowID == Model.Record.Aeronave.RowID);
                    if (suma > aeronaveActualizada.CapacidadPasajeros)
                    {
                        mensaje = "La cantidad de pasajeros supera la capacidad de la aeronave";
                    }
                }
            }
            return mensaje;

        }

        #endregion

        #endregion

        #region Liquidacion

        private void btn_GuardarLiquidacion_Click_1(object sender, RoutedEventArgs e)
        {
            if(Model.Record.RowID == 0){
                Util.ShowError("Debe crear un vuelo");
                return;
            }
            //Liquidacion Tasas
            GuardarDatosLiquidacion(sender, e);
            
           
        }

        private void cbxTipoVuelSalid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cbxTipoVuelSalid.SelectedIndex == -1 || cbxTipoVuelSalid.SelectedItem == null)
            {  return;  }
            if (((Tipo)(cbxTipoVuelSalid.SelectedItem)).Codigo == "NACIONAL")
            {
                EncabezadoTasasNacional.Visibility = Visibility.Visible;
                EncabezadoTasasInternacional.Visibility = Visibility.Collapsed;
                stkEmbarcadosNal.Visibility = Visibility.Visible;
                stkEmbarcadosInternal.Visibility = Visibility.Collapsed;
                lblUSD.Visibility = Visibility.Collapsed;
                lblCOP.Visibility = Visibility.Visible;
            }
            else
            {
                EncabezadoTasasNacional.Visibility = Visibility.Collapsed;
                EncabezadoTasasInternacional.Visibility = Visibility.Visible;
                stkEmbarcadosNal.Visibility = Visibility.Collapsed;
                stkEmbarcadosInternal.Visibility = Visibility.Visible;
                lblUSD.Visibility = Visibility.Visible;
                lblCOP.Visibility = Visibility.Collapsed;
            }
        }


        #endregion

        #region Facturacion Contado


        private void btnCalcularFacturacion_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.Llegada == null || Model.Record.Salida == null)
            {
                Util.ShowError("Debe confirmar Llegada y Salida.");
                return;
            }
            //else if (service.GetTRM(new TRM { FechaFiltro = FechaSalida.SelectedDate}).Count == 0)
            else if (TraerTRM(FechaSalida.SelectedDate.Value).Count == 0)
            {
                Util.ShowError("No existe TRM para la fecha seleccionada");
                return;
            }
            else if(Model.Record.Facturado == true){
                if (Model.Record.Tipo.Codigo != "CONTADO")
                {
                    Util.ShowError("Operacion ya Liquidada");
                    return;
                }
                else
                {
                    Util.ShowError("Operacion ya Facturada");
                    return;
                }
            }
            Estado statusConfirmada = Model.StatusLlegadaSalidaConfirmada;
            Estado statusOPliquidada = Model.StatusOperacionLiquidada;
            if (Model.Record.Llegada.Estado.RowID == statusConfirmada.RowID && Model.Record.Salida.Estado.RowID == statusConfirmada.RowID)
            {
                //Busco si tiene adicionales que esten confirmados y con despegado = true
                //if (service.GetAdicionalesPyP(new AdicionalesPyP { Operacion = Model.Record, Status = Model.StatusAdicionalesConfirmado, Despego = true }).Count() >= 1)
                if (db.AdicionalesPyP.Where(f=>f.OperacionID == Model.Record.RowID && f.EstadoID == Model.StatusAdicionalesConfirmado.RowID && f.Despego == true).Count() >= 1)
                {
                    //Para calcular el primer adicional
                    CalcularFacturacionContadoConAdicionales(sender, e);
                    //Para calcular el resto de adicionales y sumarlos
                    this.CalcularYSumarAdicionales();
                    //Sumo los valores que estan en pantalla
                }
                else
                {
                    CalcularFacturacionContado(sender, e);
                }
                CalcularTotalFacturacion(sender, e);

                //Actualizo el status de la operacion a Liquidada
                Model.Record.Estado = statusOPliquidada;

                BotonImprimirFactura.IsEnabled = true;
                //Si no ha facturado le habilito el boton para facturar
                if (Model.Record.Facturado == false)
                {
                    BotonFacturar.IsEnabled = true;
                }

                Model.Record.FechaModificacion = DateTime.Now;
                Model.Record.UsuarioModificacion = App.curUser.NombreUsuario;
                //service.UpdateOperacion(Model.Record);
                db.SaveChanges();
            }
            else
            {
                Util.ShowError("Debe confirmar Llegada y Salida.");
            }
        }

        private void btnFacturar_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.Llegada == null || Model.Record.Salida == null)
            {
                Util.ShowError("Debe Crear Llegada y Salida.");
                return;
            }

            Estado statusConfirmada = Model.StatusLlegadaSalidaConfirmada;
            if (Model.Record.Llegada.Estado.RowID == statusConfirmada.RowID && Model.Record.Salida.Estado.RowID == statusConfirmada.RowID)
            {
                //Valido que haya Liquidado
                Estado statusOPliquidada = Model.StatusOperacionLiquidada;
                if (Model.Record.Estado.RowID == statusOPliquidada.RowID)
                {
                    Estado aux = Model.StatusServiciosEnviarERP;
                    //Estado auxBomberos = service.GetStatus(new Status { Name = "ParaEnviarERP", StatusType = new StatusType { Name = "ServicioBomberos" } }).First();
                    Estado auxBomberos = db.Estado.FirstOrDefault(f => f.Nombre == "ParaEnviarERP" && f.Tipo.Nombre == "ServicioBomberos");
                    this.facturarServicios(aux, auxBomberos, true);
                    ObteberListaFacturas(sender, e);
                    //Para imprimir la factura de Contado
                    //IList<Servicios> listaServicios = service.GetServicios(new Servicios { Operacion = Model.Record, Status = aux }).ToList();
                    IList<Servicios> listaServicios = db.Servicios.Where(f => f.OperacionID == Model.Record.RowID && f.EstadoID == aux.RowID).ToList();
                    PrinterControl.imprimirFactura(listaServicios);
                    stkDatosOperacionSalida3Contado.IsEnabled = true;
                    if (Model.Record.Salida.TipoPosicionSalidaID != null)
                    {
                        if (Model.Record.Salida.Tipo1.Codigo != "PUENTE")
                        {
                            PanelDatosOperacionSalidaPuenteContado.IsEnabled = false;
                        }
                    }

                    BtnGuardarSalida.IsEnabled = true;
                }
                else
                {
                    Util.ShowError("Debe Calcular Facturación de la Operación.");
                }
            }
            else
            {
                Util.ShowError("Debe confirmar Llegada y Salida.");
            }
        }

        public String NuevoPrefijoFactura()
        {
            //Tipo pre = service.GetMMaster(new MMaster { Code = "PREFIJOCONTADO" }).FirstOrDefault();
            Tipo pre = db.Tipo.FirstOrDefault(f => f.Codigo == "PREFIJOCONTADO");
            String prefijoActual = pre.Codigo2;
            String Limite = pre.Valor;
            String ConsecutivoActual = pre.Nombre;
            int ConsecutivoI = Convert.ToInt32(ConsecutivoActual);
            int limiteI = Convert.ToInt32(Limite);
            ConsecutivoI = ConsecutivoI + 1;
            if (ConsecutivoI > limiteI)
            {
                Util.ShowError("Ha superado el limite de consecutivos " + limiteI);
                return "";
            }
            else
            {
                String NuevoPrefijo = prefijoActual + " " + ConsecutivoI;
                pre.Nombre = ConsecutivoI.ToString();
                //service.UpdateMMaster(pre);
                db.SaveChanges();
                return NuevoPrefijo;
            }
        }

        public void facturarServicios(Estado NuevoStatus, Estado NuevoStatusBomberos, bool AsociarFactura)
        {
            if (Model.Record.RowID != 0)
            {
                Facturas NroFactura = null;
                if (AsociarFactura)
                {
                    //Creo una factura para asignarle a los servicios
                    Facturas facturaServicios = new Facturas();
                    facturaServicios.FechaEmision = DateTime.Now;
                    facturaServicios.FechaInicio = DateTime.Now;
                    facturaServicios.FechaFinal = DateTime.Now;
                    facturaServicios.Estado = Model.StatusFacturaEnviarERP;
                    facturaServicios.UsuarioCreacion = App.curUser.NombreUsuario;
                    facturaServicios.FechaCreacion = DateTime.Now;
                    facturaServicios.NumeroFactura = NuevoPrefijoFactura();
                    //NroFactura = service.SaveFacturas(facturaServicios);
                    NroFactura = db.Facturas.Add(facturaServicios);
                    db.SaveChanges();

                }
                //Creo Servicio Aerodromo
                if (!string.IsNullOrEmpty(txtTotalAerodromo.Text) && txtTotalAerodromo.Text != "0")
                {
                    Servicios aerodromo = new Servicios();
                    aerodromo.Fecha = DateTime.Now;
                    aerodromo.Cantidad = 1;
                    aerodromo.Valor = Double.Parse(txtTotalAerodromo.Text);
                   // aerodromo.TipoServicio = service.GetMMaster(new MMaster { Code = "AERODROMO" }).First();
                    aerodromo.TipoServicioID = db.Tipo.First(f => f.Codigo == "AERODROMO").RowID;
                    aerodromo.Facturas = NroFactura;
                    aerodromo.Estado = NuevoStatus;
                    aerodromo.UsuarioCreacion = App.curUser.NombreUsuario;
                    aerodromo.Operacion = Model.Record;
                    aerodromo.FechaCreacion = DateTime.Now;
                    //service.SaveServicios(aerodromo);
                    db.Servicios.Add(aerodromo);
                    db.SaveChanges();
                }
                //Creo Servicio Recargo Nocturno
                if (!string.IsNullOrEmpty(txtRecargoNocturno.Text) && txtRecargoNocturno.Text != "0")
                {
                    Servicios recargo = new Servicios();
                    recargo.Fecha = DateTime.Now;
                    recargo.Cantidad = 1;
                    recargo.Valor = Double.Parse(txtRecargoNocturno.Text);
                    //recargo.TipoServicio = service.GetMMaster(new MMaster { Code = "RECARGONOC" }).First();
                    recargo.TipoServicioID = db.Tipo.First(f => f.Codigo == "RECARGONOC").RowID;
                    recargo.Operacion = Model.Record;
                    recargo.Facturas = NroFactura;
                    recargo.Estado = NuevoStatus;
                    recargo.UsuarioCreacion = App.curUser.NombreUsuario;
                    recargo.FechaCreacion = DateTime.Now;
                    //service.SaveServicios(recargo);
                    db.Servicios.Add(recargo);
                    db.SaveChanges();
                }
                //Creo Servicio Puentes
                if (!string.IsNullOrEmpty(txtTotalPuente.Text) && txtTotalPuente.Text != "0")
                {
                    Servicios puentes = new Servicios();
                    puentes.Fecha = DateTime.Now;
                    puentes.Cantidad = Int32.Parse(txtNumPuentes.Text);
                    puentes.Valor = Double.Parse(txtTotalPuente.Text);
                    //puentes.TipoServicio = service.GetMMaster(new MMaster { Code = "PUENTES" }).First();
                    puentes.TipoServicioID = db.Tipo.First(f => f.Codigo == "PUENTES").RowID;
                    puentes.Operacion = Model.Record;
                    puentes.Facturas = NroFactura;
                    puentes.Estado = NuevoStatus;
                    puentes.UsuarioCreacion = App.curUser.NombreUsuario;
                    puentes.FechaCreacion = DateTime.Now;
                    //service.SaveServicios(puentes);
                    db.Servicios.Add(puentes);
                    db.SaveChanges();
                }
                //Creo Servicio Parqueo
                if (!string.IsNullOrEmpty(txtTotalParqueo.Text) && txtTotalParqueo.Text != "0")
                {
                    Servicios parqueo = new Servicios();
                    parqueo.Fecha = DateTime.Now;
                    parqueo.Cantidad = Int32.Parse(txtCantHoras.Text);
                    parqueo.Valor = Double.Parse(txtTotalParqueo.Text);
                    //parqueo.TipoServicioID = service.GetMMaster(new MMaster { Code = "PARQUEO" }).First();
                    parqueo.TipoServicioID = db.Tipo.First(f=> f.Codigo == "PARQUEO").RowID;
                    parqueo.Operacion = Model.Record;
                    parqueo.Facturas = NroFactura;
                    parqueo.Estado = NuevoStatus;
                    parqueo.UsuarioCreacion = App.curUser.NombreUsuario;
                    parqueo.FechaCreacion = DateTime.Now;
                    //service.SaveServicios(parqueo);
                    db.Servicios.Add(parqueo);
                    db.SaveChanges();
                }
                //Creo Servicio Tasas
                if (!string.IsNullOrEmpty(txtValorTasas.Text) && txtValorTasas.Text != "0")
                {
                    Servicios tasas = new Servicios();
                    tasas.Fecha = DateTime.Now;
                    tasas.Cantidad = Int32.Parse(txtcantPasajeros.Text);
                    tasas.Valor = Double.Parse(txtValorTasas.Text);
                    //tasas.TipoServicio = service.GetMMaster(new MMaster { Code = "TASAS" }).First();
                    tasas.TipoServicioID = db.Tipo.First(f => f.Codigo == "TASAS").RowID;
                    tasas.Operacion = Model.Record;
                    tasas.Facturas = NroFactura;
                    tasas.Estado = NuevoStatus;
                    tasas.UsuarioCreacion = App.curUser.NombreUsuario;
                    tasas.FechaCreacion = DateTime.Now;
                    //service.SaveServicios(tasas);
                    db.Servicios.Add(tasas);
                    db.SaveChanges();
                }
                //Creo 2 Servicios, uno agrupando asistencia y otro agrugando limpieza
                int cantAsistencia = 0, cantLimpieza = 0;
                double acumAsistencia = 0, acumLimpieza = 0;
                if (!string.IsNullOrEmpty(txtCantidadSerBomb.Text) && txtCantidadSerBomb.Text != "0")
                {
                    foreach (Bombero ServicioBombero in ListaBomberos.Items)
                    {
                        if (ServicioBombero.Estado.Nombre == "Nuevo")
                        {
                            if (ServicioBombero.Tipo.Codigo == "ASISTENCIA")
                            {
                                cantAsistencia++;
                                acumAsistencia += ServicioBombero.ValorServicio.Value;
                            }
                            else if (ServicioBombero.Tipo.Codigo == "LIMPIEZA")
                            {
                                cantLimpieza++;
                                acumLimpieza += ServicioBombero.ValorServicio.Value;
                            }
                        }
                    }
                }
                else
                {
                    cantAsistencia = cantLimpieza = 0;
                }

                if (cantAsistencia != 0)
                {
                    Servicios asistenciaBomberos = new Servicios();
                    asistenciaBomberos.Fecha = DateTime.Now;
                    asistenciaBomberos.Cantidad = cantAsistencia;
                    asistenciaBomberos.Valor = acumAsistencia;
                    //asistenciaBomberos.TipoServicio = service.GetMMaster(new MMaster { Code = "ASISTENCIA" }).First();
                    asistenciaBomberos.TipoServicioID = db.Tipo.First(f => f.Codigo == "ASISTENCIA").RowID;
                    asistenciaBomberos.Operacion = Model.Record;
                    asistenciaBomberos.Facturas = NroFactura;
                    asistenciaBomberos.Estado = NuevoStatus;
                    asistenciaBomberos.UsuarioCreacion = App.curUser.NombreUsuario;
                    asistenciaBomberos.FechaCreacion = DateTime.Now;
                    //service.SaveServicios(asistenciaBomberos);
                    db.Servicios.Add(asistenciaBomberos);
                    db.SaveChanges();
                }

                if (cantLimpieza != 0)
                {
                    Servicios limpiezaBomberos = new Servicios();
                    limpiezaBomberos.Fecha = DateTime.Now;
                    limpiezaBomberos.Cantidad = cantLimpieza;
                    limpiezaBomberos.Valor = acumLimpieza;
                    //limpiezaBomberos.TipoServicio = service.GetMMaster(new MMaster { Code = "LIMPIEZA" }).First();
                    limpiezaBomberos.TipoServicioID = db.Tipo.First(f => f.Codigo == "LIMPIEZA").RowID;
                    limpiezaBomberos.Operacion = Model.Record;
                    limpiezaBomberos.FacturaID = NroFactura.RowID;
                    limpiezaBomberos.Estado = NuevoStatus;
                    limpiezaBomberos.UsuarioCreacion = App.curUser.NombreUsuario;
                    limpiezaBomberos.FechaCreacion = DateTime.Now;
                    //service.SaveServicios(limpiezaBomberos);
                    db.Servicios.Add(limpiezaBomberos);
                    db.SaveChanges();
                }
                //Le asigno status de Facturada a la operacionbtnFacturar_Click
                if (Model.Record.Tipo != null)
                {
                    if (Model.Record.Tipo.Codigo != "CONTADO")
                    {
                        Model.Record.Estado = Model.StatusOperacionLiquidada;
                    }
                    else
                    {
                        Model.Record.Estado = Model.StatusOperacionFacturada;
                    }
                }                
                Model.Record.Facturado = true;
                //Datos de modificacion Operacion
                Model.Record.FechaModificacion = DateTime.Now;
                Model.Record.UsuarioModificacion = App.curUser.NombreUsuario;
                //service.UpdateOperacion(Model.Record);
                db.SaveChanges();
                //Para que No facture otra vez
                btnFacturar.IsEnabled = false;
                //Para que pueda imprimir
                BotonImprimirFactura.IsEnabled = true;
                //Para que no pueda modificar informacion de la operacion
                PanelDatosLiquidacionTasas.IsEnabled = false;
                PanelDatosOperacionLlegada.IsEnabled = false;
                PanelDatosOperacionSalida1.IsEnabled = false;
                PanelDatosOperacionSalida2.IsEnabled = false;
                PanelDatosOperacionSalida3Contado.IsEnabled = false;
                PanelDatosCabecera.IsEnabled = false;
                PanelDatosBomberos.IsEnabled = false;

                //Asigno la factura y el status a los servicios bomberos
                this.cambiarStatusYFacturaBomberos(NuevoStatusBomberos, "Nuevo");
                //Le cambio el estado a las tasas 
                if (Model.Record.Tipo != null)
                {
                    if (Model.Record.Tipo.Codigo != "CONTADO")
                    {
                        this.cambiarStatusTasas(Model.StatusTasasParaFacturar);
                    }
                    else
                    {
                        this.cambiarStatusTasas(Model.StatusTasasParaEnviarERP);
                    }
                } 

                if (Model.Record.Tipo.Codigo != "CONTADO")
                {
                    Util.ShowMessage("Operacion Terminada correctamente");
                }
                else
                {
                    Util.ShowMessage("Facturacion generada correctamente");
                }
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (ListaFacturas.SelectedItem != null)
            {
                if (((Servicios)ListaFacturas.SelectedItem).Estado.Nombre == "Anulada")
                {
                    Util.ShowError("Factura Anulada, no se puede imprimir");
                    return;
                }
                //IList<Servicios> listaServicios = service.GetServicios(new Servicios { Operacion = Model.Record, Facturas = new Facturas { RowID = ((Servicios)ListaFacturas.SelectedItem).RowID } });
                IList<Servicios> listaServicios = db.Servicios.Where(f => f.OperacionID == Model.Record.RowID && f.FacturaID == ((Servicios)ListaFacturas.SelectedItem).RowID).ToList();
                PrinterControl.imprimirFactura(listaServicios);
            }
            else
            {
                Util.ShowError("Seleccione una Factura a Imprimir");

            }
        }

        //Pone el nuevoStatus a la condicion que digan
        public void cambiarStatusYFacturaBomberos(Estado NuevoStatus, String Condicion)
        {
            foreach (Bombero ServicioBombero in ListaBomberos.Items)
            {
                if (ServicioBombero.Estado.Nombre == Condicion)
                {
                    ServicioBombero.Estado = NuevoStatus;
                    //service.UpdateBomberos(ServicioBombero);
                    db.SaveChanges();
                }
            }
            //Model.RegistroBomberosList = service.GetBomberos(new Bomberos { Operacion = Model.Record, Activo = true }).Where(f => f.Status.StatusType.Nombre == "ServicioBomberos").ToList();
            Model.RegistroBomberosList = db.Bombero.Where(f => f.OperacionID == Model.Record.RowID && f.Activo == true && f.Estado.Tipo.Nombre == "ServicioBomberos").ToList();
        }

        public void cambiarStatusTasas(Estado NuevoStatus)
        {
            //Le cambio el estado a las tasas 
            if (Model.RecordTasas.RowID != 0)
            {
                try
                {
                    //Asigno las variables de modificacion
                    Model.RecordTasas.UsuarioModificacion = App.curUser.NombreUsuario;
                    Model.RecordTasas.FechaModificacion = DateTime.Now;
                    Model.RecordTasas.Estado = NuevoStatus;
                    //Actualizo
                    //service.UpdateTasas(Model.RecordTasas);
                    db.SaveChanges();
                    //Model.RegistroTasasList = service.GetTasas(new Tasas { Operacion = Model.Record });
                    Model.RegistroTasasList = db.Tasas.Where(f => f.OperacionID == Model.Record.RowID).ToList();
                }
                catch (Exception)
                {
                }
            }
        }



        private void btnAnularFactura_Click_1(object sender, RoutedEventArgs e)
        {
            if (ListaFacturas.SelectedItem != null)
            {
                if (((Servicios)ListaFacturas.SelectedItem).Estado.Nombre == "Anulada")
                {
                    Util.ShowError("Esta Factura ya esta Anulada");
                    return;
                }
                if (((Servicios)ListaFacturas.SelectedItem).Estado.Nombre != "ParaEnviarERP")
                {
                    Util.ShowError("Solo Puede Anular Facturas en Status ParaEnviarERP");
                    return;
                }
                //Cambio Status de los Servicios a Anulado
                //IList<Servicios> ListaServicios = service.GetServicios(new Servicios { Operacion = Model.Record, Facturas = new Facturas { RowID = ((Servicios)ListaFacturas.SelectedItem).RowID } , Status = Model.StatusServiciosEnviarERP }).ToList();
                IList<Servicios> ListaServicios = db.Servicios.Where(f=> f.OperacionID == Model.Record.RowID && f.FacturaID == ((Servicios)ListaFacturas.SelectedItem).RowID && f.Estado.RowID == Model.StatusServiciosEnviarERP.RowID).ToList() ;
                foreach (Servicios servicio in ListaServicios)
                {
                    servicio.Estado = Model.StatusServiciosAnulada;
                    //service.UpdateServicios(servicio);
                    db.SaveChanges();
                }
                //Cambio Status de la Factura a Anulada
                //Facturas Factura = service.GetFacturas(new Facturas { RowID = ((Servicios)ListaFacturas.SelectedItem).RowID }).First();
                Facturas Factura = db.Facturas.First(f => f.RowID == ((Servicios)ListaFacturas.SelectedItem).RowID);
                Factura.Estado = Model.StatusFacturaAnulada;
                //service.UpdateFacturas(Factura);
                db.SaveChanges();
                //Cambio el status de los bomberos a Nuevo
                this.cambiarStatusYFacturaBomberos(Model.StatusBomberosNuevo, Model.StatusBomberosParaEnviarERP.Nombre);
                //Cambio el estado para las tasas a nuevo
                this.cambiarStatusTasas(Model.StatusTasasNueva);
                //Actualizar Lista
                ObteberListaFacturas(sender, e);

                //Actualizo El vuelo A Facturado = False
                Model.Record.Facturado = false;
                //Model.Record.Estado = service.GetStatus(new Status { Name = "Guardada", StatusType = new StatusType { Name = "OperacionCecoa" } }).First();
                Model.Record.Estado =  db.Estado.FirstOrDefault(f=>f.Nombre == "Guardada" && f.Tipo.Nombre == "OperacionCecoa");
                //service.UpdateOperacion(Model.Record);
                db.SaveChanges();
                //Para abrir la operacion y que puedan modificar
                Model.RecordLlegada.Estado = Model.StatusLlegadaSalidaGuardada;
                //service.UpdateLlegada(Model.RecordLlegada);
                db.SaveChanges();
                Model.RecordSalida.Estado = Model.StatusLlegadaSalidaGuardada;
                //service.UpdateSalida(Model.RecordSalida);
                db.SaveChanges();

                //Habilito los campos para que los pueda editar
                BotonFacturar.IsEnabled = true;
                PanelDatosOperacionLlegada.IsEnabled = true;
                PanelDatosOperacionSalida1.IsEnabled = true;
                PanelDatosOperacionSalida2.IsEnabled = true;
                PanelDatosOperacionSalida3Contado.IsEnabled = true;
                PanelDatosCabecera.IsEnabled = true;
                PanelDatosLiquidacionTasas.IsEnabled = true;
                BtnGuardarLlegada.IsEnabled = true;
                BtnGuardarSalida.IsEnabled = true;
                BtnConfirmarLlegada.IsEnabled = true;
                BtnConfirmarSalida.IsEnabled = true;
                PanelDatosBomberos.IsEnabled = true;
                Util.ShowMessage("Factura Anulada Correctamente");

            }
            else
            {
                Util.ShowError("Selecciona una factura a Anular");
            }
        }

        #endregion

        #region AdicionalesPyP

        private void GuardarAdicionales_Click_1(object sender, RoutedEventArgs e)
        {
             if (Model.Record.RowID != 0)
            {
                if (Model.Record.Facturado == true)
                {
                    Util.ShowError("No puede agregar Adicionales");
                    return;
                }
                if (Model.Record.Llegada.RowID != 0)
                {
                    if (Model.RecordAdicionalesPyP.Estado != null)
                    {
                        if (Model.RecordAdicionalesPyP.Estado.Nombre != "Guardado"){
                            Util.ShowError("No se puede modificar este registro");
                            return;
                        }
                    }
                    int var = 0;
                    Model.RecordAdicionalesPyP.Operacion = Model.Record;
                    Model.RecordAdicionalesPyP.FechaInicial = DTP_FechaInicialPyP.SelectedDate != null ? DTP_FechaInicialPyP.SelectedDate : null;
                    Model.RecordAdicionalesPyP.FechaFinal = DTP_FechaFinalPyP.SelectedDate != null ? DTP_FechaFinalPyP.SelectedDate : null;
                    Model.RecordAdicionalesPyP.TipoPosicionLlegadaID = cbxTipPosLlegadaPyP.SelectedItem != null ? ((Tipo)(cbxTipPosLlegadaPyP.SelectedItem)).RowID : var = 0;
                    Model.RecordAdicionalesPyP.TipoPosicionSalidaID = cbxTipPosSalidaPyP.SelectedItem != null ? ((Tipo)(cbxTipPosSalidaPyP.SelectedItem)).RowID : var = 0;
                    Model.RecordAdicionalesPyP.TipoLlegadaID = cbxTipoLlegada.SelectedItem != null ? ((Tipo)(cbxTipoLlegada.SelectedItem)).RowID : var = 0;
                    Model.RecordAdicionalesPyP.PosicionLlegadaID = cbxPosLlegadaPyP.SelectedItem != null ? ((Tipo)(cbxPosLlegadaPyP.SelectedItem)).RowID : var = 0;
                    Model.RecordAdicionalesPyP.PosicionSalidaID = cbxPosSalidaPyP.SelectedItem != null ? ((Tipo)(cbxPosSalidaPyP.SelectedItem)).RowID : var = 0;
                    Model.RecordAdicionalesPyP.Estado = Model.StatusAdicionalesGuardado;
                  
                    if (Model.RecordAdicionalesPyP.RowID == 0)
                    {
                       Model.RecordAdicionalesPyP.UsuarioCreacion = App.curUser.NombreUsuario;
                       Model.RecordAdicionalesPyP.FechaCreacion = DateTime.Now;
                        // Model.RecordAdicionalesPyP = service.SaveAdicionalesPyP(Model.RecordAdicionalesPyP);
                        db.AdicionalesPyP.Add(Model.RecordAdicionalesPyP);
                        db.SaveChanges();
                    }
                    else
                    {
                        Model.RecordAdicionalesPyP.UsuarioModificacion = App.curUser.NombreUsuario;
                        Model.RecordAdicionalesPyP.FechaModificacion = DateTime.Now;
                        //service.UpdateAdicionalesPyP(Model.RecordAdicionalesPyP);
                        db.SaveChanges();
                    }
                    limpiarAdcionales();
                    panelHoraDespegue.Visibility = Visibility.Collapsed;
                    //panelIngresoAdicionales.Visibility = Visibility.Collapsed;
                    //Model.RegistroAdicionalesPyPList = service.GetAdicionalesPyP(new AdicionalesPyP { Operacion = Model.Record });
                    Model.RegistroAdicionalesPyPList = db.AdicionalesPyP.Where(f => f.OperacionID == Model.Record.RowID).ToList();
                    Util.ShowMessage("Guardado Correctamente");
                }
                else
                {
                    Util.ShowError("Debe crear una LLegada");
                }
            }
             else
             {
                 Util.ShowError("Debe crear una Operacion");
             }
        }

        private void ConfirmarAdicionalButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (Model.RecordAdicionalesPyP.Estado == null)
            {  return;    }
            if (Model.RecordAdicionalesPyP.Estado.Nombre == "Guardado")
            {
                string mensaje = this.validacionesAdicionalesPyP();
                if (mensaje != "")
                {
                    Util.ShowError(mensaje);
                    return;
                }
                Model.RecordAdicionalesPyP.Operacion = Model.Record;
                Model.RecordAdicionalesPyP.FechaInicial = DTP_FechaInicialPyP.SelectedDate;
                Model.RecordAdicionalesPyP.FechaFinal = DTP_FechaFinalPyP.SelectedDate;
                Model.RecordAdicionalesPyP.TipoPosicionLlegadaID = ((Tipo)(cbxTipPosLlegadaPyP.SelectedItem)).RowID;
                Model.RecordAdicionalesPyP.TipoPosicionSalidaID = ((Tipo)(cbxTipPosSalidaPyP.SelectedItem)).RowID;
                Model.RecordAdicionalesPyP.TipoLlegadaID = ((Tipo)(cbxTipoLlegada.SelectedItem)).RowID;
                Model.RecordAdicionalesPyP.PosicionLlegadaID = ((Tipo)(cbxPosLlegadaPyP.SelectedItem)).RowID;
                Model.RecordAdicionalesPyP.PosicionSalidaID = ((Tipo)(cbxPosSalidaPyP.SelectedItem)).RowID;
                Model.RecordAdicionalesPyP.Estado = Model.StatusAdicionalesConfirmado;
                Model.RecordAdicionalesPyP.UsuarioModificacion = App.curUser.NombreUsuario;
                Model.RecordAdicionalesPyP.FechaModificacion = DateTime.Now;
                //service.UpdateAdicionalesPyP(Model.RecordAdicionalesPyP);
                db.SaveChanges();
                if (Model.RecordAdicionalesPyP.Despego == true)
                {
                    //Cargo los datos del adicional en los datos de salida del vuelo
                    this.cargarDatosAdicionalASalida();
                    panelIngresoAdicionales.IsEnabled = false;
                    btnAgregarPyp.IsEnabled = false;
                }
                //Model.RegistroAdicionalesPyPList = service.GetAdicionalesPyP(new AdicionalesPyP { Operacion = Model.Record });
                Model.RegistroAdicionalesPyPList = db.AdicionalesPyP.Where(f => f.OperacionID == Model.Record.RowID).ToList();
                //Cargo los datos del adicional en los datos de salida del vuelo
                this.limpiarAdcionales();
                panelHoraDespegue.Visibility = Visibility.Collapsed;
                //panelIngresoAdicionales.Visibility = Visibility.Collapsed;
            }
            else
            {
                Util.ShowError("Este registro ya se confirmo");
            }
      
               
        }

        private void lvRegistroSAdicionalesPyp_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

            if (lvRegistroSAdicionalesPyp.SelectedItem == null)
            {  return; }
            limpiarAdcionales();
            //panelIngresoAdicionales.Visibility = Visibility.Visible;
            Model.RecordAdicionalesPyP = (AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem;
            //Si ya esta confirmado no lo puede editar
            if (((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).Estado.Nombre != Model.StatusAdicionalesGuardado.Nombre)
            {
                panelIngresoAdicionales.IsEnabled = false;
            }
            else
            {         panelIngresoAdicionales.IsEnabled = true; }
            //Si esta seleccionado despego, muestro la hora de despegue
            if (((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).Despego.Value)
            {
                panelHoraDespegue.Visibility = Visibility.Visible;
            }
           //Tipos de posiciones
            cbxTipPosLlegadaPyP.SelectedValue = ((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).TipoPosicionLlegadaID != null ? ((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).TipoPosicionLlegadaID : 0;
            cbxTipPosSalidaPyP.SelectedValue = ((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).TipoPosicionSalidaID != null ? ((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).TipoPosicionSalidaID : 0;
            //Posiciones
            cbxPosLlegadaPyP.SelectedValue = ((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).PosicionLlegadaID != null ? ((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).PosicionLlegadaID : 0;
            cbxPosSalidaPyP.SelectedValue = ((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).PosicionSalidaID != null ? ((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).PosicionSalidaID : 0;
            cbxTipoLlegada.SelectedValue = ((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).TipoLlegadaID != null ? ((AdicionalesPyP)lvRegistroSAdicionalesPyp.SelectedItem).TipoLlegadaID : 0;
        }

        public void limpiarAdcionales() {
            Model.RecordAdicionalesPyP = new AdicionalesPyP();
            //Asigno por defecto la fecha de operacion como fecha inicial
            DTP_FechaInicialPyP.SelectedDate = Model.Record.FechaOP;
            cbxTipPosLlegadaPyP.SelectedIndex = -1;
            cbxTipPosSalidaPyP.SelectedIndex = -1;
            cbxPosLlegadaPyP.SelectedIndex = -1;
            cbxPosSalidaPyP.SelectedIndex = -1;
            //Si esta seleccionado el tipo de vuelo en la llegada se lo asigno por defecto al control
            if (Model.Record.Llegada.TipoVueloID != null)
            {
                ((Tipo)cbxTipoLlegada.SelectedItem).RowID = Model.Record.Llegada.TipoVueloID.Value;
            }
            else
            {
                cbxTipoLlegada.SelectedIndex = -1;
            }
           
        }

        private void btnAgregarPyp_Click(object sender, RoutedEventArgs e)
        {
            panelIngresoAdicionales.Visibility = Visibility.Visible;
            panelIngresoAdicionales.IsEnabled = true;
            Model.RecordAdicionalesPyP = new AdicionalesPyP();
            this.limpiarAdcionales();
        }

        private void btnEliminarPyp_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.RowID == 0)
            {
                return;
            }
            foreach (AdicionalesPyP adicional in lvRegistroSAdicionalesPyp.SelectedItems)
            {
                //Solo se puede borrar el registro si esta guardado
                if (adicional.Estado.Nombre == "Guardado" )
                {
                    if (adicional.Despego == true)
                    {
                        panelIngresoAdicionales.IsEnabled = true;
                        btnAgregarPyp.IsEnabled = true;
                    }
                    if (adicional.RowID == Model.RecordAdicionalesPyP.RowID)
                    {
                        this.limpiarAdcionales();
                    }
                    panelIngresoAdicionales.IsEnabled = true;
                    //service.DeleteAdicionalesPyP(adicional);
                    db.AdicionalesPyP.Remove(adicional);
                    db.SaveChanges();
                }
            }
            //Model.RegistroAdicionalesPyPList = service.GetAdicionalesPyP(new AdicionalesPyP { Operacion = Model.Record });
            Model.RegistroAdicionalesPyPList = db.AdicionalesPyP.Where(f=> f.OperacionID == Model.Record.RowID).ToList();
        }

        private void btnCalcularAdicionalesPyp_Click(object sender, RoutedEventArgs e)
        {
            if (Model.RegistroAdicionalesPyPList != null)
            {
                Decimal[] Valores = this.calcularAdicionalesPyP();
            }
            else
            {
                Util.ShowError("No tiene Adicionales para calcular");
            }
        }

        public void CalcularYSumarAdicionales()
        {
            Decimal[] Valores = this.calcularAdicionalesPyP();
            if (Valores == null) { return; }
            //Si esta vacio le pongo cero
            txtTotalParqueo.Text = string.IsNullOrEmpty(txtTotalParqueo.Text) ? "0" : txtTotalParqueo.Text;
            txtCantHoras.Text = string.IsNullOrEmpty(txtCantHoras.Text) ? "0" : txtCantHoras.Text;
            txtNumPuentes.Text = string.IsNullOrEmpty(txtNumPuentes.Text) ? "0" : txtNumPuentes.Text;
            txtTotalPuente.Text = string.IsNullOrEmpty(txtTotalPuente.Text) ? "0" : txtTotalPuente.Text;
            ///Sumo lo que este en los campos mas los calculos que hice internos
            txtNumPuentes.Text = ((Int32.Parse(txtNumPuentes.Text)) + Valores[0]).ToString("N0");
            txtTotalPuente.Text = ((Decimal.Parse(txtTotalPuente.Text)) + Valores[1]).ToString("N0");
            txtCantHoras.Text = ((Int32.Parse(txtCantHoras.Text)) + Valores[2]).ToString("N0");
            txtTotalParqueo.Text = ((Decimal.Parse(txtTotalParqueo.Text)) + Valores[3]).ToString("N0");
        }

        public Decimal[] calcularAdicionalesPyP()
        {
            int cantPuentes = 0, cantPuentesFinales = 0;
            int cantHoras = 0, cantHorasFinales = 0;
            Decimal valorFinalPuentes = 0, valorFinalParqueo = 0;

            if (Model.RegistroAdicionalesPyPList.Count == 0)
            {
                Util.ShowError("No hay Adicionales para facturar");
                return null;
            }

            if (Model.RegistroAdicionalesPyPList.Count(f=> f.Estado.Nombre == Model.StatusAdicionalesConfirmado.Nombre) == 0 )
            {
                Util.ShowError("No hay Adicionales confirmados para facturar");
                return null;
            }

            foreach (AdicionalesPyP adicional in Model.RegistroAdicionalesPyPList.OrderBy( f=> f.RowID))
            {
                
                Decimal valorPuentes = 0, ValorParqueo = 0, totalAerodromo = 0;

                cantPuentes = CalcularCantPuentes(adicional);

                //Verifico si cuenta con una tarifa de puentes disponible
                TarifaCecoa TarifaPuente = null;
                //if (service.GetTarifas(new Tarifas { FechaFiltro = adicional.FechaFinal, TipoTarifa = new MMaster { Code = "PUENTES" } }).Count == 1)
                if (TraerTarifa(adicional.FechaFinal.Value, "PUENTES").Count == 1)
                {
                    //TarifaPuente = service.GetTarifas(new Tarifas { FechaFiltro = adicional.FechaFinal, TipoTarifa = new MMaster { Code = "PUENTES" } }).First();
                    TarifaPuente = TraerTarifa(adicional.FechaFinal.Value, "PUENTES").FirstOrDefault();
                }
                else
                {
                    Util.ShowError("No cuenta con una tarifa de Puentes disponible " + adicional.RowID);
                    cantPuentes = 0;
                }

                cantHoras = CalcularCantHoras(adicional);

                //Verifico si cuenta con una tarifa de parqueo disponible
                TarifaCecoa TarifaParqueo = null;
                //if (service.GetTarifas(new Tarifas { FechaFiltro = adicional.FechaFinal, TipoTarifa = new MMaster { Code = "PARQUEO" } }).Count == 1)
                if (TraerTarifa(adicional.FechaFinal.Value, "PARQUEO").Count == 1)
                {
                    //TarifaParqueo = service.GetTarifas(new Tarifas { FechaFiltro = adicional.FechaFinal, TipoTarifa = new MMaster { Code = "PARQUEO" } }).First();
                    TarifaParqueo = TraerTarifa(adicional.FechaFinal.Value, "PARQUEO").FirstOrDefault();
                }
                else
                {
                    Util.ShowError("No cuenta con una tarifa de Parqueo disponible");
                    cantHoras = 0;
                    ValorParqueo = 0;
                }

                TarifaCecoa TarifaAerodromo = null;
                // Verifico si existe una tarifa AERODROMO para la fecha de operacion del vuelo
                //if (service.GetTarifas(new Tarifas { FechaFiltro = adicional.FechaInicial, TipoTarifa = new MMaster { Code = "AERODROMO" } }).Count == 1)
                if (TraerTarifa(adicional.FechaFinal.Value, "AERODROMO").Count == 1)
                {
                    //TarifaAerodromo = service.GetTarifas(new Tarifas { FechaFiltro = adicional.FechaFinal, TipoTarifa = new MMaster { Code = "AERODROMO" } }).First();
                    TarifaAerodromo = TraerTarifa(adicional.FechaFinal.Value, "AERODROMO").FirstOrDefault();
                }
                else
                {
                    Util.ShowError("No cuenta con una tarifa de Aerodromo disponible");
                    cantHoras = 0;
                    ValorParqueo = 0;
                }

                //Calcular el valor total puentes dependiendo del tipo de llegada
                if (adicional.Tipo2.Codigo == "NACIONAL")
                {
                    if (TarifaPuente != null)
                    {
                        //Calculo Puentes = Cantidad de puentes * tarifa Puentes NACIONAL
                        valorPuentes = Decimal.Parse((cantPuentes * TarifaPuente.ValorCOP) + "");
                        //Redondeo al siguiente entero
                        valorPuentes = Decimal.Round(valorPuentes);
                    }
                    if (TarifaParqueo != null && TarifaAerodromo != null)
                    {
                        //Calculo el Aerodromo = peso aeronave * tarifa AERODROMO NACIONAL
                        //Redondeo al siguiente entero
                        totalAerodromo = Decimal.Round(Decimal.Parse((Model.Record.Aeronave.PBMOKG * TarifaAerodromo.ValorCOP) + ""));
                        //Calculo el Parqueo = (tarifa aerodromo * porcentaje del Parqueo NACIONAL  / 100 )* cantidad de horas
                        ValorParqueo = Decimal.Parse((((totalAerodromo * Decimal.Parse(TarifaParqueo.ValorCOP + "")) / 100) * cantHoras) + "");
                        //Redondeo al siguiente entero
                        ValorParqueo = Decimal.Round(ValorParqueo);
                    }
                }
                else
                {
                    TRM Trm;
                    //Verifico si hay una TRM vigente para la operacion
                    //if (service.GetTRM(new TRM { FechaFiltro = adicional.FechaFinal }).Count == 1)
                        if (TraerTRM(adicional.FechaFinal.Value).Count == 1)
                    {
                        //Asigno la TRM vigente
                        //Trm = service.GetTRM(new TRM { FechaFiltro = adicional.FechaFinal }).First();
                        Trm = TraerTRM(adicional.FechaFinal.Value).FirstOrDefault();
                        if (TarifaPuente != null)
                        {
                            //Calculo Puentes = Cantidad de puentes * tarifa Puentes INTERNACIONAL * TRM
                            valorPuentes = Decimal.Parse((cantPuentes * TarifaPuente.ValorUSD * Trm.Valor) + "");
                            //Redondeo al siguiente entero
                            valorPuentes = Decimal.Round(valorPuentes);
                        }
                        if (TarifaParqueo != null)
                        {
                            //Calculo el Aerodromo = peso aeronave * tarifa AERODROMO INTERNACIONAL * TRM
                            //Redondeo al siguiente entero
                            totalAerodromo = Decimal.Round(Decimal.Parse((Model.Record.Aeronave.PBMOKG * TarifaAerodromo.ValorUSD * Trm.Valor) + ""));
                            //Calculo el Parqueo = (tarifa aerodromo * porcentaje del Parqueo INTERNACIONAL  / 100 )* cantidad de horas
                            ValorParqueo = Decimal.Parse((((totalAerodromo * Decimal.Parse(TarifaParqueo.ValorUSD + "")) / 100) * cantHoras) + "");
                            //Redondeo al siguiente entero
                            ValorParqueo = Decimal.Round(ValorParqueo);
                        }
                    }
                    else
                    {
                        Util.ShowError("No hay TRM Vigente para la fecha " + adicional.FechaFinal);
                    }
                }
                valorFinalPuentes = valorFinalPuentes + valorPuentes;
                valorFinalParqueo = valorFinalParqueo + ValorParqueo;
               // Util.ShowMessage("Cantidad de puentes " + cantPuentes + " Valor " + valorPuentes + " Cantidad de Horas " + cantHoras + " Valor " + ValorParqueo);
                cantHorasFinales = cantHorasFinales + cantHoras;
                cantPuentesFinales = cantPuentesFinales + cantPuentes;
            }
           // Util.ShowMessage("valor total puentes " + cantPuentesFinales + ": " + valorFinalPuentes + " valor total Parqueo " + cantHorasFinales + ": " + valorFinalParqueo);
            
            //Para saber en pantalla cuanto se esta calculando
            txtPuentesCantAdiconalesTotal.Text = cantPuentesFinales.ToString();
            txtPuentesValorAdiconalesTotal.Text = valorFinalPuentes.ToString();
            txtParqueoHrsAdiconalesTotal.Text = cantHorasFinales.ToString();
            txtParqueoValorAdiconalesTotal.Text = valorFinalParqueo.ToString();

            Decimal[] Valores = new Decimal[4];
            Valores[0] = cantPuentesFinales;
            Valores[1] = valorFinalPuentes;
            Valores[2] = cantHorasFinales;
            Valores[3] = valorFinalParqueo;
            return Valores;
        }

        public int CalcularCantPuentes( AdicionalesPyP adicional)
        {
            int cantPuentes = 0;
            bool llegoEnPuente, SalioEnPuente;
            
            //Valido si Llego en puente
            llegoEnPuente = adicional.Tipo4.Codigo == "PUENTE" ? true : false;
            SalioEnPuente = adicional.Tipo3.Codigo == "PUENTE" ? true : false;
            // Llego y salio por puente, los tipos son iguales.
            if ((llegoEnPuente && SalioEnPuente))
            {
                if ((adicional.Tipo == adicional.Tipo1)) // Si la posicion de llegada y salida son las misamas
                {
                    //Fechas iguales
                    if (adicional.FechaInicial == adicional.FechaFinal)
                    { cantPuentes = 1; }
                    //Fechas diferentes
                    else
                    { cantPuentes = 2; }
                }
                else
                { cantPuentes = 2; }

            }//No llego ni salio por puente
            else
            { cantPuentes = 0; }

            if ((llegoEnPuente && !SalioEnPuente) || (!llegoEnPuente && SalioEnPuente))
            { cantPuentes = 1; }
            //Si es Hangar, Hangar. no cobra parqueo ni puente
           
            return cantPuentes;
        }

        public int CalcularCantHoras(AdicionalesPyP adicional)
        {
            int cantHoras = 0;

            //Si llego en Hangar no cobra parqueo 
            if (adicional.Tipo4.Codigo == "HANGAR")
            {
                return 0;
            }

            int HoraLleg = int.Parse(adicional.HoraLlegada.Substring(0, 2));
            int minutosLlg = int.Parse(adicional.HoraLlegada.Substring(3, 2));
            int HoraSald = int.Parse(adicional.HoraSalida.Substring(0, 2));
            int minutosSald = int.Parse(adicional.HoraSalida.Substring(3, 2));

            //Si llego y salio el mismo dia
            if (adicional.FechaInicial == adicional.FechaFinal)
            {
                cantHoras = HoraSald - HoraLleg;
                if (minutosSald > minutosLlg)
                { cantHoras++; }

                ////si aun no le he cobrado las 2 horas de parqueo
                if ((HoraLleg < ((int.Parse(Model.RecordLlegada.HoraPlataforma.Substring(0, 2))) + 2)))
                {
                    cantHoras = cantHoras - 2;
                    if (cantHoras < 0)
                    {
                        cantHoras = 0; 
                        if (minutosSald > (int.Parse(Model.RecordLlegada.HoraPlataforma.Substring(3, 2))))
                        { cantHoras++; } 
                    }
                }

                
            }
            //si llego un dia y salio otro
            else
            {
                int horasPrimerDia, horasUltimoDia, diasEnMedio = 0;
                //Calculo cuantos dias se quedo el avion
                TimeSpan ts = adicional.FechaFinal.Value - adicional.FechaInicial.Value;
                switch (ts.Days)
                {
                    case 1: diasEnMedio = 0;
                        break;
                    case 2: diasEnMedio = 1;
                        break;
                    //Si entra al default es porque son mas de 3
                    default:
                        diasEnMedio = ts.Days - 1;
                        break;
                }
                HoraLleg = 24 - HoraLleg;
                //Cant horas de llegada menos las dos horas gratis
                horasPrimerDia = (HoraLleg);
                horasUltimoDia = (minutosSald > minutosLlg) ? (HoraSald++) : (HoraSald);
                //Le adiciono la hora de las 00:00 porque no la esta sumando
                if (HoraSald > 0 && minutosSald > 0)
                {
                    if (minutosSald > minutosLlg)
                    {
                        horasUltimoDia++;
                    }
                }
                //Calculo la cantidad de horas total, sumando las de todos los dias
                cantHoras = horasPrimerDia + (diasEnMedio * 24) + horasUltimoDia;
            }
            
            return cantHoras;
        }

        

        public string validacionesAdicionalesPyP()
        {
            string mensaje = "";


            if (DTP_FechaInicialPyP.SelectedDate == null)
            {
                mensaje = "Seleccione la fecha inicial";
            }
            else if (DTP_FechaFinalPyP.SelectedDate == null)
            {
                mensaje = "Seleccione la fecha final";
            }
            else if (cbxTipPosLlegadaPyP.SelectedIndex == -1)
            {
                mensaje = "Seleccione el tipo de Posicion de Llegada";
            }
            else if (cbxTipPosSalidaPyP.SelectedIndex == -1)
            {
                mensaje = "Seleccione el tipo Posicion de Salida";
            }
            else if (((Tipo)cbxTipPosLlegadaPyP.SelectedItem).Codigo != "HANGAR" && cbxPosLlegadaPyP.SelectedIndex == -1)
            {
                mensaje = "Seleccione Posicion de Llegada";
            }
            else if (((Tipo)cbxTipPosSalidaPyP.SelectedItem).Codigo != "HANGAR" && cbxPosSalidaPyP.SelectedIndex == -1)
            {
                mensaje = "Seleccione Posicion de Salida";
            }

            else if (HoraLlegadaAdicionalesPyP.Text.Contains("_"))
            {
                mensaje = "Ingrese Hora de Llegada";
            }
            else if (HoraSalidaAdicionalesPyP.Text.Contains("_"))
            {
                mensaje = "Ingrese Hora de Salida";
            }

            else if (DTP_FechaInicialPyP.SelectedDate > DTP_FechaFinalPyP.SelectedDate)
            {
                mensaje = "La fecha Inicial no puede ser mayor a la Final";
            }
            else if (chkDespegoPyP.IsChecked == true)
            {
                if (HoraDespegueAdicionalesPyP.Text.Contains("_"))
                {
                    mensaje = "Ingrese Hora de Despegue";
                }
                else if (Convert.ToDateTime(HoraDespegueAdicionalesPyP.Text) < Convert.ToDateTime(HoraSalidaAdicionalesPyP.Text))
                {
                    mensaje = "Hora de despegue incorrecta.";
                }
            }

            return mensaje;
        }

        private void ClickDepegoAdicionalesPyP(object sender, RoutedEventArgs e)
        {
            if (chkDespegoPyP.IsChecked == true)
            {
                panelHoraDespegue.Visibility = Visibility.Visible;
            }
            else
            {
                panelHoraDespegue.Visibility = Visibility.Collapsed;
            }
        }

        public void cargarDatosAdicionalASalida()
        {

            
            //Limpio los campos
            MaskedHoraSalidaPlataforma.Text = "";
            FechaSalida.SelectedDate = null;
            TipoPosicionSalida.SelectedValue = 0;
            PosicionSalida.SelectedValue = 0;
            //TipoVueloSalida.SelectedValue = 0;
            HoraDespegue.Text = "";

            //Cargo los datos del adicional en los datos de salida del vuelo
            MaskedHoraSalidaPlataforma.Text = Model.RecordAdicionalesPyP.HoraSalida;
            MaskedHoraSalidaPuente.Text = String.IsNullOrEmpty(Model.RecordAdicionalesPyP.HoraSalidaPuente) ? "" : Model.RecordAdicionalesPyP.HoraSalidaPuente ;
            Model.RecordSalida.FechaSalida = Model.RecordAdicionalesPyP.FechaFinal;
            TipoPosicionSalida.SelectedValue = Model.RecordAdicionalesPyP.TipoPosicionSalidaID;
            PosicionSalida.SelectedValue = Model.RecordAdicionalesPyP.PosicionSalidaID != null ? Model.RecordAdicionalesPyP.PosicionSalidaID : 0;
            //TipoVueloSalida.SelectedValue = Model.RecordAdicionalesPyP.TipoLlegada.RowID;
            HoraDespegue.Text = Model.RecordAdicionalesPyP.HoraDespegue;


            Model.RecordSalida.HoraSalidaPlataforma = Model.RecordAdicionalesPyP.HoraSalida;
            Model.RecordSalida.PosicionSalidaID = Model.RecordAdicionalesPyP.PosicionSalidaID;
            Model.RecordSalida.PosicionSalidaID = Model.RecordAdicionalesPyP.PosicionSalidaID != null ? Model.RecordAdicionalesPyP.PosicionSalidaID : null;
            Model.RecordSalida.HoraDespegue = Model.RecordAdicionalesPyP.HoraDespegue;

            if (((Tipo)Model.RecordAdicionalesPyP.Tipo3).Codigo == "PUENTE")
            {
                String asd = (Convert.ToDateTime(Model.RecordAdicionalesPyP.HoraSalida).AddMinutes(-1)).ToShortTimeString();
                Model.RecordSalida.HoraSalidaPuente = asd.Length == 4 ? "0" + asd : asd;
                MaskedHoraSalidaPuente.Text = asd.Length == 4 ? "0" + asd : asd;
            }
            else
            {
                MaskedHoraSalidaPuente.Clear();
                Model.RecordSalida.HoraSalidaPuente = null;
            }
            
            

            //Guardo la salida
            Model.RecordSalida.FechaModificacion = DateTime.Now;
            Model.RecordSalida.UsuarioModificacion = App.curUser.NombreUsuario;
            //Actualizo la Salida
            //service.UpdateSalida(Model.RecordSalida);
            db.SaveChanges();
        }

        #endregion

        private void btnImprimirDetalle_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.RowID != 0)
            {
                
                if (Model.Record.Facturado == false)
                {
                    if (Model.Record.Tipo.Codigo != "CONTADO")
                    {
                        Util.ShowError("Esta operación no se ha terminado.");
                    }
                    else
                    {
                        Util.ShowError("Esta Operacion no se ha facturado.");
                    }
                    return;
                }
                //listo los servicios que no estan anulados para mostrarlos en el informe
                //List<Servicios> ListaServicios =  service.GetServicios(new Servicios { Operacion = Model.Record}).Where(f => f.Estado.Nombre != Model.StatusServiciosAnulada.Nombre).ToList();
                List<Servicios> ListaServicios =  db.Servicios.Where(f => f.OperacionID == Model.Record.RowID && f.Estado.Nombre != Model.StatusServiciosAnulada.Nombre).ToList();
                //Busco el trm con la fecha de salida para mostrarlo en el informe
                double valorTRM=0;
                try
                {
                    //TRM trm = service.GetTRM(new TRM { FechaFiltro = Model.Record.Salida.FechaSalida }).First();
                    TRM trm = TraerTRM(Model.Record.Salida.FechaSalida.Value).FirstOrDefault();
                    valorTRM = trm.Valor.Value;
                }
                catch (Exception exp) { }
                string tiempo = ObtenerTiempoHoras();

                PrinterControl.imprimirDetalleOperacion(Model.Record,
                        db.Tasas.Where(f => f.OperacionID == Model.Record.RowID).ToList(), 
                        ListaServicios,
                        valorTRM, tiempo);
            }
                       else
            {
                Util.ShowError("Debe Crear una operacion");

            }
        }

        public List<TRM> TraerTRM(DateTime FechaFiltro)
        {
            List<TRM> Lista = new List<TRM>();
            if (FechaFiltro != DateTime.MinValue)
            {
                Lista = db.TRM.Where(f => (FechaFiltro >= f.FechaInicial.Value && FechaFiltro <= f.FechaFinal.Value)).ToList();
            }
            else
            {
                return null;
            }
            return Lista;
        }

        public List<TarifaCecoa> TraerTarifa(DateTime FechaFiltro, String Codigo)
        {
            List<TarifaCecoa> Lista = new List<TarifaCecoa>();
            if (FechaFiltro != DateTime.MinValue)
            {
                Lista = db.TarifaCecoa.Where(f => (FechaFiltro >= f.FechaInicial.Value && FechaFiltro <= f.FechaFinal.Value)).ToList();
            }
            else
            {
                Lista = db.TarifaCecoa.ToList();
            }

            if (!String.IsNullOrEmpty(Codigo))
            {
                Lista = Lista.Where(f => f.Tipo1.Codigo == Codigo).ToList();
            }
            return Lista;
        }

        public string ObtenerTiempoHoras()
        {
            string tiempo = "";
            int hora = 0, minuto =0;
            //Si llego y salio el mismo dia
            if (Model.Record.Llegada.FechaLLegadaPlataforma == Model.Record.Salida.FechaSalidaPlataforma)
            {
                TimeSpan TNow = DateTime.Parse(Model.Record.Salida.HoraSalidaPlataforma) - DateTime.Parse(Model.Record.Llegada.HoraPlataforma);
                if (TNow.TotalMinutes >= 60)
                {
                    hora = Convert.ToInt32(TNow.TotalMinutes / 60);
                    minuto = Convert.ToInt32((TNow.TotalMinutes) - (Convert.ToInt32(hora) * 60));
                    if (hora <= 2)
                    {
                        hora = 0;
                    }
                }
                else {
                    hora = 0;
                    minuto = Convert.ToInt32(TNow.TotalMinutes) ;
                }
                tiempo = hora + ":" + minuto;

            }
            //si llego un dia y salio otro
            else 
            {
                int horasPrimerDia, horasUltimoDia, diasEnMedio = 0;
                int cantHoras = 0;
                int HoraLleg = int.Parse(Model.Record.Llegada.HoraPlataforma.Substring(0, 2));
                int minutosLlg = int.Parse(Model.Record.Llegada.HoraPlataforma.Substring(3, 2));
                int HoraSald = int.Parse(Model.Record.Salida.HoraSalidaPlataforma.Substring(0, 2));
                int minutosSald = int.Parse(Model.Record.Salida.HoraSalidaPlataforma.Substring(3, 2));
                TimeSpan ts = Model.Record.Salida.FechaSalidaPlataforma.Value - Model.Record.Llegada.FechaLLegadaPlataforma.Value;
                switch (ts.Days)
                {
                    case 1: diasEnMedio = 0;
                        break;
                    case 2: diasEnMedio = 1;
                        break;
                    //Si entra al default es porque son mas de 3
                    default:
                        diasEnMedio = ts.Days - 1;
                        break;
                }
                HoraLleg = 24 - HoraLleg;

                //Cant horas de llegada menos las dos horas gratis
                horasPrimerDia = HoraLleg;
                horasUltimoDia = (minutosSald > minutosLlg) ? (HoraSald++) : (HoraSald);

                //Le adiciono la hora de las 00:00 porque no la esta sumando
                if (HoraSald > 0 && minutosSald > 0)
                {
                    if (minutosSald > minutosLlg)
                    {
                        horasUltimoDia++;
                    }
                    else
                    {
                        minuto = (60 - minutosLlg) + minutosSald;
                    }
                }
                //Calculo la cantidad de horas total, sumando las de todos los dias
                cantHoras = (horasPrimerDia + (diasEnMedio * 24) + horasUltimoDia) - 2;
                tiempo = cantHoras + ":" + minuto;
            }
                
                
            return tiempo;
        }

        #endregion

        private void btnLimpiarDatosSAlida_Click(object sender, RoutedEventArgs e)
        {
            if (Model.Record.Facturado == true || Model.Record.Salida == null)
            {
                return;
            }
            //NumVueloSalida.Text = "";
            FechaSalida.SelectedDate = null;
            TipoPosicionSalida.SelectedIndex = -1;
            PosicionSalida.SelectedIndex = -1;
            HoraPuenteSalida.Text = "";
            HoraPlataformaSalida.Text = "";
            HoraDespegue.Text = "";
            //TipoVueloSalida.SelectedIndex = -1;
            //Destino.Aeropuertos = null;
            //Destino.txtData.Text = "";
            //Destino.txtDescripcion.Text = "";
            txtCargaSalida.Text = "0";
            txtCorreoSalida.Text = "0";

        }

        private void cmb_TipoVuelo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_TipoVuelo.SelectedItem == null || cmb_TipoVuelo.SelectedIndex == -1) { return; }
            cbxTipoLlegada.SelectedValue = ((Tipo)cmb_TipoVuelo.SelectedItem).RowID;
            
        }

        private void MaskedHoraProgramadaSalida_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Model != null)
            {
                CalcularEstado(sender, e);
            }
        }

        private void NO_tab_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                return;
            }
        }

        private void DTP_FechaAterrizaje_GotFocus(object sender, RoutedEventArgs e)
        {
            MaskedHoraPlataforma.Focus();
        }

        private void DTP_FechaSalidaPuente_GotFocus(object sender, RoutedEventArgs e)
        {
            MaskedHoraSalidaPlataforma.Focus();
        }

        private void DTP_FechaSalidaPlataforma_GotFocus(object sender, RoutedEventArgs e)
        {
            MaskedHoraDespegue.Focus();
        }

        private void DTP_FechaLlegadaPuente_GotFocus(object sender, RoutedEventArgs e)
        {
            cmb_TipoDeclaracion.Focus();
        }

        private void cmb_TipoDeclaracion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key  == Key.LeftShift)
            {
                MaskedHoraPuente.Focus();
            }
        }

        private void DTP_FechaLlegadaPlataforma_GotFocus(object sender, RoutedEventArgs e)
        {
            cmb_TipoPosicion.Focus();
        }

        private void DTP_FechaDespegue_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchAeropuertoDestino.Focus();
        }

        private void SeleccionarCampo_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)(sender)).Select(0, ((TextBox)(sender)).Text.Length);
        }

        private void NavegarEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((TextBox)(sender)).Text = ((TextBox)(sender)).Text + Key.Tab;
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

        private void BuscarProgramacionLlegada_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_NumVuelo.Text))
            {
                CargarDatosDesdePlaneacionLlegada(sender, e);
            }
        }

        private void BuscarProgramacionSalida_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_NumVueloSalida.Text))
            {
                CargarDatosDesdePlaneacionSalida(sender, e);
            }
        }
    }

    public interface IOperacionesView
    {
        //Clase Modelo
        OperacionesModel Model { get; set; }

        #region Variables

        #region Operacion

        ComboBox TipoFactura { get; set; }
        //TextBox TipoFactura { get; set; }
        SearchAeronaves Aeronave { get; set; }
        TextBox NumVuelo { get; set; }
        TextBox NumVueloSalida { get; set; }
        TextBox Sala { get; set; }
        TextBox SalaSalida { get; set; }
        TextBox CargaEntrada { get; set; }
        TextBox CorreoEntrada { get; set; }
        TextBox Observacion { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaOperacion { get; set; }
        //Llegada
        Xceed.Wpf.Controls.MaskedTextBox HoraAterrizaje { get; set; }
        Xceed.Wpf.Controls.MaskedTextBox HoraPlataforma { get; set; }
        Xceed.Wpf.Controls.MaskedTextBox HoraPuente { get; set; }
        Xceed.Wpf.Controls.MaskedTextBox HoraProgramada { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaLlegadaPlataforma { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaLlegadaPuente { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaAterrizaje { get; set; }


        //Salida
        Xceed.Wpf.Controls.MaskedTextBox HoraDespegue { get; set; }
        Xceed.Wpf.Controls.MaskedTextBox HoraPlataformaSalida { get; set; }
        Xceed.Wpf.Controls.MaskedTextBox HoraPuenteSalida { get; set; }
        Xceed.Wpf.Controls.MaskedTextBox HoraProgramadaSalida { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaSalidaPlataforma { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaSalidaPuente { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaSalida { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaDespegue { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaProgramacionLlegada { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaProgramacionSalida { get; set; }

        

        ComboBox TipoPosicion { get; set; }
        SearchAeropuertos Origen { get; set; }
        SearchAeropuertos Destino { get; set; }
        ComboBox TipoOperacion { get; set; }
        ComboBox TipoVuelo { get; set; }
        ComboBox TipoDeclaracion { get; set; }
        ComboBox Banda { get; set; }
        ComboBox Posicion { get; set; }
        ComboBox PosicionSalida { get; set; }
        ComboBox TipoPosicionSalida { get; set; }
        ComboBox TipoVueloSalida { get; set; }
        SearchTerceros CompañiaFactura { get; set; }
        TextBlock EstadoVuelo { get; set; }
        TextBlock EstadoVueloSalida { get; set; }
        TextBlock CIAExplotadora { get; set; }
        TextBlock txtTotalPagoCOP { get; set; }
        TextBlock txtPaxIntrNal { get; set; }
        TextBlock txtPaxNal { get; set; }
        SearchAeronaves SearchAeronavesCbx { get; set; }
        String horaAValidar { get; set; }
        TabItem TabFacturaContado { get; set; }
        StackPanel PanelSala { get; set; }
        StackPanel PanelEstadoVueloLl { get; set; }
        StackPanel PanelEstadoVueloSal { get; set; }
        StackPanel PanelHoraProgLleg { get; set; }
        StackPanel PanelHoraProgSal { get; set; }
        StackPanel PanelDatosOperacionLlegada { get; set; }
        StackPanel PanelDatosOperacionSalida1 { get; set; }
        StackPanel PanelDatosOperacionSalida2 { get; set; }
        StackPanel PanelDatosOperacionSalida3Contado { get; set; }
        StackPanel PanelDatosOperacionSalidaPuenteContado { get; set; }
        StackPanel PanelDatosCabecera { get; set; }
        StackPanel PanelDatosLiquidacionTasas { get; set; }
        StackPanel PanelDatosBomberos { get; set; }
        ImageButton BtnConfirmarLlegada { get; set; }
        ImageButton BtnConfirmarSalida { get; set; }
        ImageButton BtnGuardarLlegada { get; set; }
        ImageButton BtnGuardarSalida { get; set; }
        ImageButton Btn_CerrarOperacion { get; set; }
        ImageButton Btn_AnularFactura { get; set; }
        ImageButton BtnAbrirOperacion { get; set; }
        TextBlock ValorAerodromoUSD { get; set; }
        TextBlock ValorRecargoUSD { get; set; }
        TextBlock ValorTasasUSD { get; set; }
        TextBlock ValorPuentesUSD { get; set; }
        TextBlock ValorParqueoUSD { get; set; }
        TextBlock ValorBomberosUSD { get; set; }
        TextBlock ValorTotalUSD { get; set; }

       

        #endregion

        #region Liquidacion

        #region DatosBasicos
        TabItem TabLiquidacionTasas { get; set; }

        #endregion

        #endregion

        #region Adicionados

        #region servicioBomberos

        ComboBox ListaTipoServicio { get; set; }
        TextBlock NIT { get; set; }
        Microsoft.Windows.Controls.DatePicker fechaServicio { get; set; }
        //Variable que va a guardar la compania factura de la Aeronave seleccionada
        TextBlock ClienteBomberos { get; set; }
        ListView ListaBomberos { get; set; }

        GroupBox GroupNotasAdicionales { get; set; }
        GroupBox GroupBomberosAdicionales { get; set; }

        //Button BotonMostrarBomberos { get; set; }

        #endregion

        #endregion

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
        ImageButton BotonFacturar { get; set; }
        ImageButton BotonImprimirFactura { get; set; }


        #endregion

        #endregion

        #region Metodos

        #region Liquidacion
        //-
        event EventHandler<EventArgs> GuardarDatosLiquidacion;
        event EventHandler<EventArgs> CalcularDatosLiquidacion;

        #endregion

        #region Operacion

        event EventHandler<EventArgs> CerrarTab;
        event EventHandler<EventArgs> ConfirmarRecordLlegada;
        event EventHandler<EventArgs> ConfirmarRecordSalida;
        event EventHandler<EventArgs> CalcularEstado;
        event EventHandler<EventArgs> CargarDatosDesdeAeronave;
        event EventHandler<EventArgs> CargarDatosDesdePlaneacionLlegada;
        event EventHandler<EventArgs> CargarDatosDesdePlaneacionSalida;
        event EventHandler<EventArgs> ValidarRangoHora;
        event EventHandler<EventArgs> cambiarNoVuelo;
        event EventHandler<DataEventArgs<Operacion>> ActualizarDatosOperacion;
        event EventHandler<EventArgs> ObteberListaFacturas;


        #endregion

        #region Adicionados

        #region servicioBomberos

        event EventHandler<EventArgs> EliminarServicioBomberos;
        event EventHandler<EventArgs> guardarServicioBomberos;


        #endregion


        #endregion

        #region Facturacion
        event EventHandler<EventArgs> CalcularFacturacionContado;
        event EventHandler<EventArgs> CalcularFacturacionContadoConAdicionales;
        event EventHandler<EventArgs> CalcularTotalFacturacion;
        #endregion

        #endregion

    }
}