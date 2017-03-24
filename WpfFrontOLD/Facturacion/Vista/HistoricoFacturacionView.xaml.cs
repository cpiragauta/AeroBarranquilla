using System;
using System.Windows.Controls;
using Core.WPF;
using WpfFront.WMSBusinessService;
using WpfFront.Models;
using WpfFront.Common.UserControls;
using WMComposite.Events;
using Xceed.Wpf.DataGrid;
using System.Windows;
using Microsoft.Windows.Controls;
using WpfFront.Common;
using Xceed.Wpf.DataGrid.Settings;
using System.Collections.Generic;

namespace WpfFront.Views
{
    /// <summary>
    /// Interaction logic for FacturasBomberosView.xaml
    /// </summary>
    public partial class HistoricoFacturacionView : UserControlBase, IHistoricoFacturacionView
    {
        public HistoricoFacturacionView()
        {
            InitializeComponent();
        }

        //View Events
        public event EventHandler<EventArgs> SearchRecords;
        public event EventHandler<DataEventArgs<Facturas>> LoadFactura;
        public event EventHandler<DataEventArgs<Facturas>> RefreshProcess;
        public event EventHandler<DataEventArgs<Facturas>> OpenFileFiducia;
        public event EventHandler<EventArgs> SearchInvoiceList;
        public event EventHandler<EventArgs> CalculateFechaEntrega;
        public event EventHandler<EventArgs> CalculateFechaVencimiento;
        public event EventHandler<DataEventArgs<Int32>> CargarDatosFactura;
        public event EventHandler<EventArgs> ExportarPlaneacionExcel;


        public HistoricoFacturacionModel Model
        {
            get
            { return this.DataContext as HistoricoFacturacionModel; }
            set
            { this.DataContext = value; }

        }

        #region Properties


        public SearchTerceros CompaniaFactura
        {
            get { return this.SearchCompañiaFactura;  }
            set { this.SearchCompañiaFactura = value; }
        }

        public ComboBox TipoFactura
        {
            get { return this.cmb_TipoFactura; }
            set { this.cmb_TipoFactura = value; }
        }

        public ComboBox GetFacturasList
        {
            get { return this.Lst_Facturas; }
            set { this.Lst_Facturas = value; }
        }

        public Button BtnReProcesar
        {
            get { return this.btn_ReProcesar; }
            set { this.btn_ReProcesar = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaEmision
        {
            get { return this.DtP_FechaEmision; }
            set { this.DtP_FechaEmision = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaInicio
        {
            get { return this.DtP_FechaInicio; }
            set { this.DtP_FechaInicio = value; }
        }

        public Microsoft.Windows.Controls.DatePicker FechaFinal
        {
            get { return this.DtP_FechaFinal; }
            set { this.DtP_FechaFinal = value; }
        }

        public TabControl TabPadre
        {
            get { return this.TabPadreControl; }
            set { this.TabPadreControl = value; }
        }

        #endregion


        #region ViewEvents

        private void btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            SearchRecords(sender, e);
        }

        private void btn_Procesar_Click(object sender, RoutedEventArgs e)
        {
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
        }



        #endregion

        private void btnActualizarLista_Click(object sender, RoutedEventArgs e)
        {
            DtP_FechaEmision.SelectedDate = null;
            DtP_FechaInicio.SelectedDate = null;
            DtP_FechaFinal.SelectedDate = null;
            cmb_TipoFactura.SelectedIndex = -1;
            SearchCompañiaFactura.Terceros = new Terceros();
            SearchCompañiaFactura.cboData.Text = "";
            SearchCompañiaFactura.txtDescripcion.Text = "";
            SearchCompañiaFactura.txtData.Text = "";
            btn_Buscar_Click(sender, e);
        }

        private void ListaFacturas_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListaFacturas.SelectedItem != null)
            {
                CargarDatosFactura(sender, new DataEventArgs<Int32>(((Servicios)ListaFacturas.SelectedItem).Factura.RowID));
            }
        }

        private void ExportarPlaneacion_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ExportarPlaneacionExcel(sender, e);
        }



    }

    public interface IHistoricoFacturacionView
    {
        //Clase Modelo
        HistoricoFacturacionModel Model { get; set; }

        //Definicion Variables
        ComboBox GetFacturasList { get; set; }
        SearchTerceros CompaniaFactura { get; set; }
        ComboBox TipoFactura { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaEmision { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaInicio { get; set; }
        Microsoft.Windows.Controls.DatePicker FechaFinal { get; set; }
        TabControl TabPadre { get; set; }


        //Eventos
        event EventHandler<EventArgs> SearchRecords;
        event EventHandler<EventArgs> CalculateFechaEntrega;
        event EventHandler<EventArgs> CalculateFechaVencimiento;
        event EventHandler<DataEventArgs<Int32>> CargarDatosFactura;
        event EventHandler<EventArgs> ExportarPlaneacionExcel;


    }
}