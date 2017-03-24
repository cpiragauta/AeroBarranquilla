using System;
using System.Windows.Controls;
using Core.WPF;
using WMComposite.Events;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using WpfFront.Model;
using WpfFront.Modelo;
using WpfFront.Controles;


namespace WpfFront.Vista
{
    /// <summary>
    /// Interaction logic for FacturasBomberosView.xaml
    /// </summary>
    public partial class FacturasAgrupadasView : UserControlBase, IFacturasAgrupadasView
    {
        private readonly wmsEntities _db;
        public FacturasAgrupadasView()
        {
            InitializeComponent();
            this._db = new wmsEntities();
            DTP_FechaConsultaTarifas.SelectedDate = DateTime.Today;
        }

        //View Events
        public event EventHandler<EventArgs> SearchRecords;
        public event EventHandler<EventArgs> getListaSinFiltros;
        public event EventHandler<EventArgs> ProcessRecords;
        public event EventHandler<DataEventArgs<Facturas>> LoadFactura;
        public event EventHandler<DataEventArgs<Facturas>> RefreshProcess;
        public event EventHandler<DataEventArgs<Facturas>> OpenFileFiducia;
        public event EventHandler<EventArgs> SearchInvoiceList;
        public event EventHandler<EventArgs> CalculateFechaEntrega;
        public event EventHandler<EventArgs> CalculateFechaVencimiento;
        public event EventHandler<EventArgs> ReProcessRecords;
        public event EventHandler<EventArgs> EnviarERP;
        public event EventHandler<EventArgs> ConfirmacionEnvioERP;
        public event EventHandler<DataEventArgs<String>> CargarDatosServicios;
        public event EventHandler<DataEventArgs<Int32>> CargarDatosFactura;

        
        

        public FacturasAgrupadasModel Model
        {
            get
            { return this.DataContext as FacturasAgrupadasModel; }
            set
            { this.DataContext = value; }

        }

        #region Properties


        public TabControl TabPadre
        {
            get { return this.TabPadre2; }
            set { this.TabPadre2 = value; }
        }

        public TabControl TabPadreListaFacturas
        {
            get { return this.TabPadreFacturas; }
            set { this.TabPadreFacturas = value; }
        }

        public SearchTerceros CompaniaFactura
        {
            get { return this.SearchCompañiaFactura;  }
            set { this.SearchCompañiaFactura = value; }
        }

        public ComboBox TipoOperacionList
        {
            get { return this.cmb_TipoVuelo; }
            set { this.cmb_TipoVuelo = value; }
        }


        public Button ButtonProcesar
        {
            get { return this.btn_Procesar; }
            set { this.btn_Procesar = value; }
        }


        public TextBlock TRecord
        {
            get { return this.TxtRecords; }
            set { this.TxtRecords = value; }
        }

        public TextBlock TotalDetallado
        {
            get { return this.TxtTotalDetallado;}
            set {  this.TxtTotalDetallado = value;}
        }
        public TextBlock TotalAgrupado
        {
            get
            {return this.TxtTotalAgrupado;}
            set
            { this.TxtTotalDetallado = value;}
        }

        public Microsoft.Windows.Controls.DatePicker FechaEmision
        {
            get { return this.DTP_FechaEmision; }
            set { this.DTP_FechaEmision = value; ; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaInicial
        {
            get { return this.DTP_FechaInicial; }
            set { this.DTP_FechaInicial = value; ; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaFinal
        {
            get { return this.DTP_FechaFinal; }
            set { this.DTP_FechaFinal = value; ; }
        }

        public ListView listaFacturasSeleccionadas
        {
            get { return this.ListaFacturas; }
            set { this.ListaFacturas = value; ; }
        }

        


        #endregion


        #region ViewEvents

        private void btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            SearchRecords(sender, e);
        }

        private void btn_Procesar_Click(object sender, RoutedEventArgs e)
        {
            ProcessRecords(sender, e);
        }

        private void Txt_FechaExp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateFechaEntrega(sender, e);
        }

        private void Txt_FechaEnt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateFechaVencimiento(sender, e);
        }

        private void btn_ReProcesar_Click(object sender, RoutedEventArgs e)
        {
            ReProcessRecords(sender, e);
        }



        #endregion

        private void btnActualizarLista_Click(object sender, RoutedEventArgs e)
        {
            Txt_FechaInicio.SelectedDate = null;
            Txt_FechaFinal.SelectedDate = null;
            cmb_TipoVuelo.SelectedIndex = -1;
            SearchCompañiaFactura.Terceros = new Tercero();
            SearchCompañiaFactura.cboData.Text = "";
            SearchCompañiaFactura.txtDescripcion.Text = "";
            SearchCompañiaFactura.txtData.Text = "";
            ButtonProcesar.IsEnabled = true;

            getListaSinFiltros(sender, e);
            
        }

        private void btn_EnviarERP_Click(object sender, RoutedEventArgs e)
        {
            EnviarERP(sender, e);
        }
        private void ListaFacturas_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListaFacturas.SelectedItem != null)
            {
                CargarDatosFactura(sender, new DataEventArgs<Int32>(((Servicios)ListaFacturas.SelectedItem).Facturas.RowID));
            }
        }

        private void btn_AuxConfirmar_Click(object sender, RoutedEventArgs e)
        {
            ConfirmacionEnvioERP(sender, e);
        }

        private void Txt_FechaConsultaTarifas_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(DTP_FechaConsultaTarifas.Text))
            {
                //Limpio los campos
                lblTRM.Text = lblTRM.Text.Split(':')[0] + ": ";
                lblAerodromo.Text = lblAerodromo.Text.Split(':')[0] + ": ";
                lblRecargo.Text = lblRecargo.Text.Split(':')[0] + ": ";
                lblPuentes.Text = lblPuentes.Text.Split(':')[0] + ": ";
                lblParqueo.Text = lblParqueo.Text.Split(':')[0] + ": ";
                lblAsistencia.Text = lblAsistencia.Text.Split(':')[0] + ": ";
                lblLimpieza.Text = lblLimpieza.Text.Split(':')[0] + ": ";
                lblTasas.Text = lblTasas.Text.Split(':')[0] + ": ";

                DateTime FechaBusqueda =  Convert.ToDateTime(DTP_FechaConsultaTarifas.Text);
                //TRM trm = _db.GetTRM(new TRM { FechaFiltro = FechaBusqueda }).FirstOrDefault();
                TRM trm = TraerTRM(FechaBusqueda).FirstOrDefault();
                if (trm != null)
                {
                    lblTRM.Text = lblTRM.Text.Split(':')[0] + ": " + trm.Valor;
                }

                //List<TarifaCecoa> tarifas = _db.GetTarifas(new Tarifas { FechaFiltro = FechaBusqueda }).ToList();
                List<TarifaCecoa> tarifas = TraerTarifa(FechaBusqueda, "").ToList();
                if (tarifas.Count != 0)
                {
                    foreach (TarifaCecoa item in tarifas)
                    {
                        switch (item.Tipo1.Codigo)
                        {
                            case "AERODROMO":
                                lblAerodromo.Text = lblAerodromo.Text.Split(':')[0] + ": " + item.ValorCOP + " - " + item.ValorUSD;
                                lblRecargo.Text = lblRecargo.Text.Split(':')[0] + ": " + item.RecargoNocturnoCOP + " - " + item.RecargoNocturnoUSD;
                                break;
                            case "PUENTES": lblPuentes.Text = lblPuentes.Text.Split(':')[0] + ": " + item.ValorCOP + " - " + item.ValorUSD;
                                break;
                            case "PARQUEO": lblParqueo.Text = lblParqueo.Text.Split(':')[0] + ": " + item.ValorCOP + " - " + item.ValorUSD;
                                break;
                            case "BOMBEROS":
                                if (item.Tipo.Codigo == "ASISTENCIA")
                                {
                                    lblAsistencia.Text = lblAsistencia.Text.Split(':')[0] + ": " + item.ValorCOP + " - " + item.ValorUSD;

                                }
                                if (item.Tipo.Codigo == "LIMPIEZA")
                                {
                                    lblLimpieza.Text = lblLimpieza.Text.Split(':')[0] + ": " + item.ValorCOP + " - " + item.ValorUSD;
                                }
                                break;
                            case "TASAS": lblTasas.Text = lblTasas.Text.Split(':')[0] + ": " + item.ValorCOP + " - " + item.ValorUSD;
                                break;
                        }
                    }
                }
                else
                {
                    lblAerodromo.Text = lblAerodromo.Text.Split(':')[0] + ": ";
                    lblRecargo.Text = lblRecargo.Text.Split(':')[0] + ": ";
                    lblPuentes.Text = lblPuentes.Text.Split(':')[0] + ": ";
                    lblParqueo.Text = lblParqueo.Text.Split(':')[0] + ": ";
                    lblAsistencia.Text = lblAsistencia.Text.Split(':')[0] + ": ";
                    lblLimpieza.Text = lblLimpieza.Text.Split(':')[0] + ": ";
                    lblTasas.Text = lblTasas.Text.Split(':')[0] + ": ";
                }


            }
        }

        public List<TarifaCecoa> TraerTarifa(DateTime FechaFiltro, String Codigo)
        {
            List<TarifaCecoa> Lista = new List<TarifaCecoa>();
            if (FechaFiltro != DateTime.MinValue)
            {
                Lista = _db.TarifaCecoa.Where(f => (FechaFiltro >= f.FechaInicial.Value && FechaFiltro <= f.FechaFinal.Value)).ToList();
            }
            else
            {
                Lista = _db.TarifaCecoa.ToList();
            }

            if (!String.IsNullOrEmpty(Codigo))
            {
                Lista = Lista.Where(f => f.Tipo1.Codigo == Codigo).ToList();
            }
            return Lista;
        }

        public List<TRM> TraerTRM(DateTime FechaFiltro)
        {
            List<TRM> Lista = new List<TRM>();
            if (FechaFiltro != DateTime.MinValue)
            {
                Lista = _db.TRM.Where(f => (FechaFiltro >= f.FechaInicial.Value && FechaFiltro <= f.FechaFinal.Value)).ToList();
            }
            else
            {
                return null;
            }
            return Lista;
        }

        private void lvRegistrosVuelo_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvRegistrosVuelo.SelectedItem != null)
            {
                String Parametro = "";
                Parametro = ((Servicios)(lvRegistrosVuelo.SelectedItem)).Operacion.Aeronave.CompañiaFacturaID + "^" + DTP_FechaInicial.Text + "^" + DTP_FechaFinal + "^";
                if (cmb_TipoVuelo.SelectedItem != null)
                {
                    Parametro = Parametro + ((Tipo)(cmb_TipoVuelo.SelectedItem)).Codigo;
                }
                Parametro = Parametro +"^"+ ((Servicios)(lvRegistrosVuelo.SelectedItem)).Operacion.Aeronave.Tercero.Nombre;
                CargarDatosServicios(sender, new DataEventArgs<String>(Parametro));
            }
        }



    }

    public interface IFacturasAgrupadasView
    {
        //Clase Modelo
        FacturasAgrupadasModel Model { get; set; }



        //Definicion Variables

        TabControl TabPadre { get; set; }
        TabControl TabPadreListaFacturas { get; set; }
        Button ButtonProcesar { get; set; }
        TextBlock TRecord { get; set; }
        //ListView LvRegVuelo { get; set; }
        SearchTerceros CompaniaFactura { get; set; }
        ComboBox TipoOperacionList { get; set; }
        TextBlock TotalDetallado { get; set; }
        TextBlock TotalAgrupado { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaEmision { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaInicial { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaFinal { get; set; }
        ListView listaFacturasSeleccionadas { get; set; }


        //Eventos
        event EventHandler<EventArgs> SearchRecords;
        event EventHandler<EventArgs> getListaSinFiltros;
        event EventHandler<EventArgs> ProcessRecords;
        event EventHandler<EventArgs> CalculateFechaEntrega;
        event EventHandler<EventArgs> CalculateFechaVencimiento;
        event EventHandler<EventArgs> EnviarERP;
        event EventHandler<EventArgs> ConfirmacionEnvioERP;
        event EventHandler<DataEventArgs<String>> CargarDatosServicios;

        event EventHandler<DataEventArgs<Int32>> CargarDatosFactura;

    }
}