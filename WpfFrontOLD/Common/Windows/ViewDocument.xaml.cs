using System;
using System.Windows;
using System.Windows.Forms.Integration;
using Microsoft.Reporting.WinForms;
using System.Data;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace WpfFront.Common
{
    /// <summary>
    /// Interaction logic for FormatsView.xaml
    /// </summary>
    public partial class ViewDocument : Window
    {

        private readonly ReportViewer pivotView = new ReportViewer();
        private string printerName;
        private int documentID;

        public ViewDocument(DataSet ds, String ReportName)
        {
            string flag = "";

            if (ReportName == null)
            {
                Util.ShowError("Report could not be found.");
                return;
            }

            if (ds == null)
                return;

            flag = "Initialize";

            InitializeComponent();
            printerName = "";



            #region Windows Form Host

            //if (showBtnPrint)
            //    btnPrintBatch.Visibility = Visibility.Visible;
            //else
            //    btnPrintBatch.Visibility = Visibility.Collapsed;

            //Create a Windows Forms Host to host a form
            WindowsFormsHost host = new WindowsFormsHost();

            //Report ddimensions
            host.HorizontalAlignment = HorizontalAlignment.Stretch;
            host.VerticalAlignment = VerticalAlignment.Stretch;

            //pivotView.Width = 900;
            //pivotView.Height = 700;

            pivotView.Margin = new System.Windows.Forms.Padding { All = 5 };

            //Add the component to the host
            host.Child = pivotView;
            gridP.Children.Add(host);

            #endregion

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string idpallet = "";
            string date = DateTime.Now.ToString("ddMMyyy-HHmmss");
            try
            {
                //Report File exists
                string reportPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    WmsSetupValues.RdlTemplateDir + "\\" + ReportName);

                if (!File.Exists(reportPath))
                {
                    Util.ShowError("Archivo del reporte no existe.");
                    return;
                }

                //Rendering Report
                flag = "Rendering";
                this.pivotView.ProcessingMode = ProcessingMode.Local;
                this.pivotView.LocalReport.ReportPath = reportPath;

                try { this.pivotView.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted)); }
                catch { }

                this.pivotView.LocalReport.EnableExternalImages = true;
                try { this.pivotView.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence); }
                catch { }
                try { this.pivotView.LocalReport.AddTrustedCodeModuleInCurrentAppDomain("Barcode, Version=1.0.5.40001, Culture=neutral, PublicKeyToken=6dc438ab78a525b3"); }
                catch { }
                try { this.pivotView.LocalReport.AddTrustedCodeModuleInCurrentAppDomain("System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"); }
                catch { }

                flag = "Process";
                pivotView.LocalReport.DataSources.Add(new ReportDataSource("Header", ds.Tables[0]));
                pivotView.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[1]));

                idpallet = ds.Tables[1].Rows[0][4].ToString();

                //Showing
                flag = "Showing";
                pivotView.ShowFindControls = false;
                pivotView.ShowExportButton = false;
                pivotView.ShowRefreshButton = false;
                pivotView.Show();
                pivotView.LocalReport.Refresh();
                pivotView.RefreshReport();

            }// End try
            catch (Exception ex)
            {
                Util.ShowError("Error mostrando el reporte: " + flag + ", " + ex);
            }
            finally
            {
                string filePath = "";
                // Asigno el nombre del archivo e.g.  RES-A0215544, Fecha 09102015-0941009  
                //(La fecha tiene el siguiente formato "ddMMyyy-HHmmss").
                string nameFile = idpallet + ", Fecha " + date;
                try
                {
                // Especifico un nombre para la ruta.
                filePath = @"c:\Habladores";
                // Asignamos la extensión del archivo.
                nameFile += ".PDF";
                // Evaluo si la ruta ya existe para no volver a crearla.
                if (!System.IO.File.Exists(filePath))
                {
                    // Creo el directorio especificado.
                    System.IO.Directory.CreateDirectory(filePath);
                }
                // Combino la ruta con el nombre del archivo.
                filePath = System.IO.Path.Combine(filePath, nameFile);
                // Creo un arreglo de bytes para almacenar allí el archivo con la información
                // que se guardará.
                byte[] bytes = this.pivotView.LocalReport.Render("pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                // Creo el archivo en la ruta especificada.
                FileStream fs = new FileStream(filePath, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                //Cierro el objeto FileStream.
                fs.Close();
                // Libero la memoria que fue ocupada por el objeto.
                fs.Dispose();
                // mostramos la información de que se guardo satisfactoriamente el archivo y mostramos la ruta.
                Util.ShowMessage("Reporte exportado en la siguiente ruta: " + filePath);
                }
                catch (Exception e)
                {
                    Util.ShowError(e.Message);
                }
            }// End finally
        }// End ViewDocument(dataset, string)
    }// End partial class
}// end namespace