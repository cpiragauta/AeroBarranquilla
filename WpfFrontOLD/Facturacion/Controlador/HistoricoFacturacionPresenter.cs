using System;
using WpfFront.Models;
using WpfFront.Views;
using Assergs.Windows;
using WMComposite.Events;
using Microsoft.Practices.Unity;
using WpfFront.WMSBusinessService;
using WpfFront.Common;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using WpfFront.Services;
using System.Globalization;
using Xceed.Wpf.DataGrid.Settings;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace WpfFront.Presenters
{

    public interface IHistoricoFacturacionPresenter
    {
        IHistoricoFacturacionView View { get; set; }
        ToolWindow Window { get; set; }
    }


    public class HistoricoFacturacionPresenter : IHistoricoFacturacionPresenter
    {
        public IHistoricoFacturacionView View { get; set; }
        private readonly IUnityContainer container;
        private readonly WMSServiceClient service;
        public ToolWindow Window { get; set; }
        Connection local;
        int diasVen = 0;
        int diasEntrega = 0;


        public HistoricoFacturacionPresenter(IUnityContainer container, IHistoricoFacturacionView view)
        {

            View = view;
            this.container = container;
            this.service = new WMSServiceClient();
            View.Model = this.container.Resolve<HistoricoFacturacionModel>();

            View.SearchRecords += new EventHandler<EventArgs>(this.OnSearchRecords);
            View.ExportarPlaneacionExcel += this.OnExportarPLaneacionExcel;

            view.CargarDatosFactura += this.OnCargarDatosFactura;

            View.Model.AerolineaList = service.GetMMaster(new MMaster { MetaType = new MType { MetaTypeID = 12 } });
            view.Model.ListaTipoFactura = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOTARIFA" }, Active = true }).Where(f =>f.Name != "Bomberos").OrderBy(f => f.NumOrder).ToList();
            view.Model.ListaTipoFactura.Add(service.GetMMaster(new MMaster { Code = "ASISTENCIA" }).First());
            view.Model.ListaTipoFactura.Add(service.GetMMaster(new MMaster { Code = "LIMPIEZA" }).First());

            getListaFacturasAgrupadas();
            View.Model.Factura = new Facturas();
            try { local = service.GetConnection(new Connection { Name = "LOCAL" }).First(); }
            catch { }
        }

        private void OnSearchRecords(object sender, EventArgs e)
        {
            this.getListaFacturasAgrupadas();
        }


        public void getListaFacturasAgrupadas()
        {
            int idCompania = 0;
            string TipoFactura = "";
            if (View.CompaniaFactura.Terceros != null)
            {
                idCompania = View.CompaniaFactura.Terceros.RowID;
            }
            if (View.TipoFactura.SelectedItem != null)
            {
                TipoFactura = ((MMaster)View.TipoFactura.SelectedItem).Code;
            }

            //Traigo todos los servicios de la Operacion agrupados por factura
            View.Model.RecordServiciosAgrupadosList = service.GetServicios(new Servicios
            {
                Status = new Status { Name = "Facturada", StatusType = new StatusType { Name = "Servicios" } }
            }).Where(f => (f.Factura != null) && f.Operacion.TipoFacturacion.Code != "CONTADO" ).OrderByDescending(f=>f.RowID).OrderByDescending(f=>f.RowID).ToList();

            //Realizo los filtros dependiendo de lo que se seleccione en los controles 
            if(View.FechaEmision.SelectedDate != null)
            {
                View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList.Where(f => f.Factura.FechaEmision.ToString().Substring(0, 10) == View.FechaEmision.SelectedDate.ToString().Substring(0, 10)).OrderByDescending(f=>f.RowID).ToList();
            }

            if (View.FechaInicio.SelectedDate != null)
            {
                View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList.Where(f => f.Factura.FechaInicio.ToString().Substring(0, 10) == View.FechaInicio.SelectedDate.ToString().Substring(0, 10)).OrderByDescending(f=>f.RowID).ToList();
            }

            if (View.FechaFinal.SelectedDate != null)
            {
                View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList.Where(f => f.Factura.FechaFinal.ToString().Substring(0, 10) == View.FechaFinal.SelectedDate.ToString().Substring(0, 10)).OrderByDescending(f=>f.RowID).ToList();
            }

            if (idCompania != 0)
            {
                View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList.Where(f => f.Operacion.Aeronave.CompañiaFactura.RowID == idCompania).OrderByDescending(f=>f.RowID).ToList();
            }

            if (!string.IsNullOrEmpty(TipoFactura))
            {
               View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList.Where(f => f.TipoServicio.Code2 == TipoFactura).OrderByDescending(f=>f.RowID).ToList();
            }
            if (View.Model.RecordServiciosAgrupadosList.Count >= 1)
            {
                View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList
                                .GroupBy(l => l.Factura.RowID)
                                .SelectMany(cl => cl.Select(
                                    csLine => new Servicios
                                    {
                                        RowID = cl.First().Factura.RowID,
                                        Cantidad = cl.Count(),
                                        Factura = cl.First().Factura,
                                        Operacion = cl.First().Operacion,
                                        TipoServicio = cl.First().TipoServicio,
                                        Valor = cl.Sum(c => c.Valor),
                                        CreatedBy = cl.First().Factura.CreatedBy,
                                        Status = cl.First().Factura.Status,
                                    })).Distinct().OrderByDescending(f=>f.RowID).ToList();
                //Elimino los repetidos
                View.Model.RecordServiciosAgrupadosList = View.Model.RecordServiciosAgrupadosList.GroupBy(a => a.RowID).Select(grp => grp.First()).OrderBy(f => f.RowID).OrderBy(f => f.RowID).OrderByDescending(f=>f.RowID).ToList();
            }
        }

        private void OnExportarPLaneacionExcel(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = null;
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.Worksheet ws = null;
            Microsoft.Office.Interop.Excel.Range rng = null;
            Microsoft.Office.Interop.Excel.Range encabezado = null;

            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                wb = excel.Workbooks.Add();
                ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet;

                //Rango para el titulo
                ws.get_Range("A1", "F1").Cells.MergeCells = true;
                ws.get_Range("A1", "F1").Font.Size = 12;
                ws.get_Range("A1", "F1").Font.Bold = true;
                ws.get_Range("A1", "F1").VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ws.get_Range("A1", "F1").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ws.get_Range("A1", "F1").Value = "HISTORICO FACTURACION";


                //Rango de la fecha 
                ws.get_Range("G1", "J1").Merge(true);
                ws.get_Range("G1", "J1").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ws.get_Range("G1", "J1").Value = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("es-ES"));
                ws.get_Range("G1", "J1").Font.Bold = true;
                ws.get_Range("G1", "J1").Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;


                //Rango para el encabezado
                encabezado = ws.get_Range("A3", "J3");
                encabezado.Characters.Font.Bold = true;
                encabezado.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                encabezado.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

                ws.Range["A3"].Offset[0, 0].Value = "Factura Nro";
                ws.Range["A3"].Offset[0, 1].Value = "Cant Items";
                ws.Range["A3"].Offset[0, 2].Value = "Fecha Emision";
                ws.Range["A3"].Offset[0, 3].Value = "Fecha Inicio ";
                ws.Range["A3"].Offset[0, 4].Value = "Fecha Final";
                ws.Range["A3"].Offset[0, 5].Value = "Aerolinea";
                ws.Range["A3"].Offset[0, 6].Value = "Concepto";
                ws.Range["A3"].Offset[0, 7].Value = "Valor";
                ws.Range["A3"].Offset[0, 8].Value = "Creada Por";
                ws.Range["A3"].Offset[0, 9].Value = "Status";


                int i = 4;
                double total = 0;
                IList<Servicios> listaPlaneacionFiltrada = View.Model.RecordServiciosAgrupadosList;
                foreach (Servicios aux in listaPlaneacionFiltrada)
                {
                    string[] datos = new String[13];
                    datos[0] = aux.RowID.ToString();
                    datos[1] = aux.Cantidad.ToString();
                    datos[2] = aux.Factura.FechaEmision.ToString();
                    datos[3] = aux.Factura.FechaInicio.ToString();
                    datos[4] = aux.Factura.FechaFinal.ToString();
                    datos[5] = aux.Operacion.Aeronave.CompañiaFactura.Nombre;
                    datos[6] = aux.TipoServicio.Code2;
                    datos[7] = aux.Valor.ToString("N0");
                    total = total + aux.Valor;
                    datos[8] = aux.CreatedBy;
                    datos[9] = aux.Status.Name;
                    ws.Range["A" + (i)].Offset[0].Resize[1, 10].Value = datos;
                    ws.Range["A" + (i)].Offset[0].Resize[1, 10].Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    i++;
                }
                ws.Range["I" + (i)].Offset[0].Value = "Total:";
                ws.Range["J" + (i)].Offset[0].Value = total.ToString("N2");
                ws.Range["I" + (i)].Offset[0].Font.Bold = true;
                ws.Range["J" + (i)].Offset[0].Font.Bold = true;                
                ws.Columns.AutoFit();
                excel.Visible = true;
                wb.Activate();
            }
            catch (Exception ex)
            { Util.ShowMessage("Problema al exportar la información a Excel: " + ex.ToString()); }


        }



        private void OnSearchInvoiceList(object sender, EventArgs e)
        {
            //Evaluo si la fecha inicio fue seleccionada
            if (String.IsNullOrEmpty(View.Model.Factura.FechaInicio.ToString()))
            {
                Util.ShowError("Por favor seleccionar una fecha de inicio para la busqueda de facturas");
                return;
            }

            //Evaluo si la fecha final fue seleccionada
            if (String.IsNullOrEmpty(View.Model.Factura.FechaFinal.ToString()))
            {
                Util.ShowError("Por favor seleccionar una fecha final para la busqueda de facturas");
                return;
            }
        }

        void View_ReProcessRecords(object sender, EventArgs e)
        {
            //  service.DirectSQLQuery("EXEC spProcesarFacturas 8", "", "FACT", local);
            Util.ShowMessage("Documentos en Proceso, espere mientras se ingresan al ERP.");
        }

        private void OnCargarDatosFactura(object sender, DataEventArgs<Int32> FacturaID)
        {
            CargarDatosFactura(FacturaID.Value);
        }

        public void CargarDatosFactura(Int32 FacturaID)
        {
            if (FacturaID != null)
            {
                //Variables Auxiliares
                TabItem NewTabItemFactura;
                IListaServiciosPorFacturaPresenter ServicioPresenter;
                try
                {
                    // Creo los datos para el nuevo Tab
                    NewTabItemFactura = new TabItem
                    {
                        Header = "Registro de Factura # " + FacturaID,
                        Name = "Tab_" + FacturaID,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };

                    //Creo los datos para el UserControl que me controla los TakeOff
                    ServicioPresenter = container.Resolve<ListaServiciosPorFacturaPresenter>();

                    //Inicializo los datos del documento a cargar
                    ServicioPresenter.CargarDocumento(FacturaID, this);

                    //Adiciono al Tab el StackPanel del TakeOff
                    NewTabItemFactura.Content = ServicioPresenter.View;

                    //Adiciono el nuevo Tab a la vista
                    View.TabPadre.Items.Add(NewTabItemFactura);

                    //Selecciono por defecto el nuevo Tab
                    NewTabItemFactura.Focus();
                }
                catch (Exception Ex)
                {
                    Util.ShowError("Error cargando el documento. Error: " + Ex);
                }
            }
        }


    }
}