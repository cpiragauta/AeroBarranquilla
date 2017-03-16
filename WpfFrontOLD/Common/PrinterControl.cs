using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Data;
using System.Globalization;
using WpfFront.Model;

namespace WpfFront.Common
{
    public class PrinterControl
    {

        private static string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string batFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\PRINT.BAT";
        //private static LocalReport localReport = null;

        private static IList<Stream> m_streams = null;
        private static int m_currentPageIndex = 0;


        //public static void imprimirReporteCarnet(Solicitud solicitudCarnet)
        //{
        //    //Variables Auxiliares
        //    DataTable dtHeader = new DataTable("Header");
        //    DataTable dtDetails = new DataTable("DataSet1");
        //    DataSet dsReporte = new DataSet();
        //    String Printer;
        //    Int32 Control = 0;

        //    //Creo los nombres de los datos a mostrar, deben ser iguales a los utilizados en el reporte
        //    dtHeader.Columns.Add("Header");
        //    dtDetails.Columns.Add("C_Empresa");
        //    dtDetails.Columns.Add("C_Foto");
        //    dtDetails.Columns.Add("C_Nombres");
        //    dtDetails.Columns.Add("C_Apellidos");
        //    dtDetails.Columns.Add("C_Cedula");
        //    dtDetails.Columns.Add("C_Cargo");
        //    dtDetails.Columns.Add("C_FechaExp");
        //    dtDetails.Columns.Add("C_FechaVen");
        //    dtDetails.Columns.Add("C_A1");
        //    dtDetails.Columns.Add("C_A2");
        //    dtDetails.Columns.Add("C_A3");
        //    dtDetails.Columns.Add("C_A4");

        //    //Creo el registro de los datos del header y los asigno

        //    dtHeader.Rows.Add(dtHeader.NewRow());
        //    dtHeader.Rows[0]["Header"] = "";
        //    //Creo los registros de los datos de los detalles
        //    dtDetails.Rows.Add(dtDetails.NewRow());

        //    try { dtDetails.Rows[Control]["C_Nombres"] = solicitudCarnet.Nombres_Marca; }
        //    catch { dtDetails.Rows[Control]["C_Nombres"] = ""; }

        //    try { dtDetails.Rows[Control]["C_Empresa"] = solicitudCarnet.Encabezado.TerceroSolicita.NombreCompleto; }
        //    catch { dtDetails.Rows[Control]["C_Empresa"] = ""; }

        //    try { dtDetails.Rows[Control]["C_Apellidos"] = solicitudCarnet.Apellidos_Modelo; }
        //    catch { dtDetails.Rows[Control]["C_Apellidos"] = ""; }

        //    try { dtDetails.Rows[Control]["C_Cedula"] = solicitudCarnet.NoDocumento_Placa; }
        //    catch { dtDetails.Rows[Control]["C_Cedula"] = ""; }

        //    try { dtDetails.Rows[Control]["C_Cargo"] = solicitudCarnet.Cargo_NoMotor; }
        //    catch { dtDetails.Rows[Control]["C_Cargo"] = ""; }

        //    try { dtDetails.Rows[Control]["C_FechaExp"] = solicitudCarnet.FechaInicio.Value.ToString("dd/MM/yyyy"); }
        //    catch { dtDetails.Rows[Control]["C_FechaExp"] = ""; }

        //    try { dtDetails.Rows[Control]["C_FechaVen"] = solicitudCarnet.FechaFinal.Value.ToString("dd/MM/yyyy"); }
        //    catch { dtDetails.Rows[Control]["C_FechaVen"] = ""; }

        //    try { dtDetails.Rows[Control]["C_A1"] = solicitudCarnet.Area1.Code2; }
        //    catch { dtDetails.Rows[Control]["C_A1"] = ""; }

        //    try { dtDetails.Rows[Control]["C_A2"] = solicitudCarnet.Area2.Code2; }
        //    catch { dtDetails.Rows[Control]["C_A2"] = ""; }

        //    try { dtDetails.Rows[Control]["C_A3"] = solicitudCarnet.Area3_Puerta1.Code2; }
        //    catch { dtDetails.Rows[Control]["C_A3"] = ""; }

        //    try { dtDetails.Rows[Control]["C_A4"] = solicitudCarnet.Area4_Puerta2.Code2; }
        //    catch { dtDetails.Rows[Control]["C_A4"] = ""; }

        //    if (((MMaster)solicitudCarnet.TipoCarnet).Code != "VEHICULO")
        //    {
        //        try { dtDetails.Rows[Control]["C_Foto"] = "File:" + solicitudCarnet.Foto; }
        //        catch { dtDetails.Rows[Control]["C_Foto"] = ""; }
        //    }
        //    else
        //    {
        //        try { dtDetails.Rows[Control]["C_Foto"] = "File:" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\LogoPixeladoConLetras.PNG"; }
        //        catch { dtDetails.Rows[Control]["C_Foto"] = ""; }
        //    }

        //    dsReporte.Tables.Add(dtHeader);
        //    dsReporte.Tables.Add(dtDetails);

        //    //Obtengo los datos de la impresora
        //    Printer = (new WMSServiceClient()).GetConfigOption(new ConfigOption { Code = "PRINTREPORTING" }).First().DefValue;
        //    ViewDocument Reporte1 = null;
        //    if (solicitudCarnet.TipoCarnet != null)
        //    {
        //        //Si es persona de area publica tiene otro diseno de carnet
        //        if (solicitudCarnet.TipoCarnet.Code == "PERSONA(AP)")
        //        {
        //            Reporte1 = new ViewDocument(dsReporte, "CARNET_AREA_PUBLICA.rdl");
        //        }
        //        else
        //        {
        //            Reporte1 = new ViewDocument(dsReporte, "CARNET.rdl");
        //        }
        //    }
        //    else
        //    {
        //        //Muestro en pantalla el comprobante para luego imprimirlo
        //        Reporte1 = new ViewDocument(dsReporte, "CARNET.rdl");
        //    }
        //    Reporte1.Show();
        //}

        //public static void imprimirReporteStiker(Solicitud solicitudCarnet)
        //{
        //    //Variables Auxiliares
        //    DataTable dtHeader = new DataTable("Header");
        //    DataTable dtDetails = new DataTable("DataSet1");
        //    DataSet dsReporte = new DataSet();
        //    String Printer;
        //    Int32 Control = 0;

        //    //Creo los nombres de los datos a mostrar, deben ser iguales a los utilizados en el reporte
        //    dtHeader.Columns.Add("Header");
        //    dtDetails.Columns.Add("Consecutivo");
        //    dtDetails.Columns.Add("Nombres_Marca");
        //    dtDetails.Columns.Add("Apellidos_Modelo");
        //    dtDetails.Columns.Add("NoDocumento_Placa");
        //    dtDetails.Columns.Add("Cargo_NoMotor");
        //    dtDetails.Columns.Add("TerceroSolicita");
        //    dtDetails.Columns.Add("Area1");
        //    dtDetails.Columns.Add("Area2");
        //    dtDetails.Columns.Add("Area3_Puerta1");
        //    dtDetails.Columns.Add("Area4_Puerta2");
        //    dtDetails.Columns.Add("FechaInicio");
        //    dtDetails.Columns.Add("FechaFinal");
        //    dtDetails.Columns.Add("TipoEmpleado");
        //    dtDetails.Columns.Add("Licencia");
        //    dtDetails.Columns.Add("Foto");

        //    //Creo el registro de los datos del header y los asigno

        //    dtHeader.Rows.Add(dtHeader.NewRow());
        //    dtHeader.Rows[0]["Header"] = "";
        //    //Creo los registros de los datos de los detalles
        //    dtDetails.Rows.Add(dtDetails.NewRow());

        //    try { dtDetails.Rows[Control]["Consecutivo"] = solicitudCarnet.RowID; }
        //    catch { dtDetails.Rows[Control]["Consecutivo"] = ""; }

        //    try { dtDetails.Rows[Control]["Nombres_Marca"] = solicitudCarnet.Nombres_Marca; }
        //    catch { dtDetails.Rows[Control]["Nombres_Marca"] = ""; }

        //    try { dtDetails.Rows[Control]["Apellidos_Modelo"] = solicitudCarnet.Apellidos_Modelo; }
        //    catch { dtDetails.Rows[Control]["Apellidos_Modelo"] = ""; }

        //    try { dtDetails.Rows[Control]["NoDocumento_Placa"] = solicitudCarnet.NoDocumento_Placa; }
        //    catch { dtDetails.Rows[Control]["NoDocumento_Placa"] = ""; }

        //    try { dtDetails.Rows[Control]["Cargo_NoMotor"] = solicitudCarnet.Cargo_NoMotor; }
        //    catch { dtDetails.Rows[Control]["Cargo_NoMotor"] = ""; }

        //    try { dtDetails.Rows[Control]["TerceroSolicita"] = solicitudCarnet.Encabezado.TerceroSolicita.NombreCompleto; }
        //    catch { dtDetails.Rows[Control]["TerceroSolicita"] = ""; }

        //    try { dtDetails.Rows[Control]["Area1"] = solicitudCarnet.Area1.Code; }
        //    catch { dtDetails.Rows[Control]["Area1"] = ""; }

        //    try { dtDetails.Rows[Control]["Area2"] = solicitudCarnet.Area2.Code; }
        //    catch { dtDetails.Rows[Control]["Area2"] = ""; }

        //    try { dtDetails.Rows[Control]["Area3_Puerta1"] = solicitudCarnet.Area3_Puerta1.Code; }
        //    catch { dtDetails.Rows[Control]["Area3_Puerta1"] = ""; }

        //    try { dtDetails.Rows[Control]["Area4_Puerta2"] = solicitudCarnet.Area4_Puerta2.Code; }
        //    catch { dtDetails.Rows[Control]["Area4_Puerta2"] = ""; }

        //    try { dtDetails.Rows[Control]["FechaInicio"] = solicitudCarnet.FechaInicio.Value.ToString("dd/MM/yyyy"); }
        //    catch { dtDetails.Rows[Control]["FechaInicio"] = ""; }

        //    try { dtDetails.Rows[Control]["FechaFinal"] = solicitudCarnet.FechaFinal.Value.ToString("dd/MM/yyyy"); }
        //    catch { dtDetails.Rows[Control]["FechaFinal"] = ""; }

        //    if (solicitudCarnet.TipoEmpleado.Code == "CONTRATISTA" || solicitudCarnet.TipoEmpleado.Code == "VISITA")
        //    {
        //        try { dtDetails.Rows[Control]["TipoEmpleado"] = solicitudCarnet.TipoEmpleado.Code; }
        //        catch { dtDetails.Rows[Control]["TipoEmpleado"] = ""; }
        //    }

        //    if (solicitudCarnet.Adicional != null)
        //    {
        //        try { dtDetails.Rows[Control]["Licencia"] = solicitudCarnet.Adicional.Code; }
        //        catch { dtDetails.Rows[Control]["Licencia"] = ""; }
        //    }

        //    if (((MMaster)solicitudCarnet.TipoCarnet).Code != "VEHICULO")
        //    {
        //        try { dtDetails.Rows[Control]["Foto"] = "File:" + solicitudCarnet.Foto; }
        //        catch { dtDetails.Rows[Control]["Foto"] = ""; }
        //    }
        //    else
        //    {
        //        try { dtDetails.Rows[Control]["Foto"] = "File:" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\LogoPixeladoConLetras.PNG"; }
        //        catch { dtDetails.Rows[Control]["Foto"] = ""; }
        //    }


        //    dsReporte.Tables.Add(dtHeader);
        //    dsReporte.Tables.Add(dtDetails);

        //    //Muestro en pantalla el comprobante para luego imprimirlo
        //    ViewDocument Reporte1 = new ViewDocument(dsReporte, "STICKER.rdl");
        //    Reporte1.Show();
        //}

        public static void imprimirFactura(IList<Servicios> ListaServicios)
        {
            ////Variables Auxiliares
            //DataTable dtHeader = new DataTable("Header");
            //DataTable dtDetails = new DataTable("Detalle");
            //DataSet dsReporte = new DataSet();
            //Int32 Control = 0;//Variable de control para el numero de filas que lleva.

            ////Creo los nombres de los datos a mostrar en el encabezado, deben ser iguales a los utilizados en el reporte
            //dtHeader.Columns.Add("Fecha");
            //dtHeader.Columns.Add("FacturaNo");
            //dtHeader.Columns.Add("Identificacion");
            //dtHeader.Columns.Add("Valor");
            //dtHeader.Columns.Add("Iva");
            //dtHeader.Columns.Add("Total");
            ////Creo los nombres de los datos a mostrar en el detalle, deben ser iguales a los utilizados en el reporte
            //dtDetails.Columns.Add("Descripcion");
            //dtDetails.Columns.Add("Cantidad");
            //dtDetails.Columns.Add("Subtotal");

            ////Creo el registro de los datos del header y los asigno
            //dtHeader.Rows.Add(dtHeader.NewRow());

            //dtHeader.Rows[0]["Fecha"] = DateTime.Now;
            //dtHeader.Rows[0]["FacturaNo"] = ListaServicios.Where(f => f.Facturas != null).First().Facturas.RowID + " " + ListaServicios.Where(f => f.Facturas != null).First().Facturas.NumeroFactura;
            //dtHeader.Rows[0]["Identificacion"] = ListaServicios.First().Operacion.Aeronave.CompañiaFactura.IdentificacionCompleto;
            //double valor = ListaServicios.Sum(f => f.Valor.Value);
            //dtHeader.Rows[0]["Valor"] = valor;
            //dtHeader.Rows[0]["Iva"] = 0;
            //dtHeader.Rows[0]["Total"] = valor; //+ (valor * 0.16);


            //foreach (Servicios item in ListaServicios)
            //{
            //    //Creo los registros de los datos de los detalles
            //    dtDetails.Rows.Add(dtDetails.NewRow());

            //    try { dtDetails.Rows[Control]["Descripcion"] = item.TipoServicio.Code; }
            //    catch { dtDetails.Rows[Control]["Descripcion"] = ""; }

            //    try { dtDetails.Rows[Control]["Cantidad"] = item.Cantidad; }
            //    catch { dtDetails.Rows[Control]["Cantidad"] = ""; }

            //    try { dtDetails.Rows[Control]["Subtotal"] = item.Valor; }
            //    catch { dtDetails.Rows[Control]["Subtotal"] = ""; }

            //    Control++;
            //}

            //dsReporte.Tables.Add(dtHeader);
            //dsReporte.Tables.Add(dtDetails);

            ////Muestro en pantalla el comprobante para luego imprimirlo
            //ViewDocument Reporte1 = new ViewDocument(dsReporte, "FACTURA_AEROPUERTOBARRANQUILLA.rdl");
            //Reporte1.Show();
        }

        //public static void imprimirFacturaCarnet(IList<Solicitud> ListaServicios, Pago registroPago)
        //{
        //    //Variables Auxiliares
        //    DataTable dtHeader = new DataTable("Header");
        //    DataTable dtDetails = new DataTable("Detalle");
        //    DataSet dsReporte = new DataSet();
        //    Int32 Control = 0;//Variable de control para el numero de filas que lleva.

        //    //Creo los nombres de los datos a mostrar en el encabezado, deben ser iguales a los utilizados en el reporte
        //    dtHeader.Columns.Add("Fecha");
        //    dtHeader.Columns.Add("FacturaNo");
        //    dtHeader.Columns.Add("Identificacion");
        //    dtHeader.Columns.Add("Valor");
        //    dtHeader.Columns.Add("Iva");
        //    dtHeader.Columns.Add("Total");
        //    //Creo los nombres de los datos a mostrar en el detalle, deben ser iguales a los utilizados en el reporte
        //    dtDetails.Columns.Add("Descripcion");
        //    dtDetails.Columns.Add("Cantidad");
        //    dtDetails.Columns.Add("Subtotal");

        //    //Creo el registro de los datos del header y los asigno
        //    dtHeader.Rows.Add(dtHeader.NewRow());
        //    dtHeader.Rows[0]["Fecha"] = DateTime.Now;
        //    dtHeader.Rows[0]["FacturaNo"] = registroPago.RowID;
        //    dtHeader.Rows[0]["Identificacion"] = registroPago.TerceroFactura.IdentificacionCompleto;

        //    double valor = registroPago.Valor;
        //    dtHeader.Rows[0]["Valor"] = valor;
        //    dtHeader.Rows[0]["Iva"] = 0;//valor * 0.16;
        //    dtHeader.Rows[0]["Total"] = valor;

        //    if (registroPago.TipoPago.Code == "GENERAL")
        //    {
        //        foreach (Solicitud item in ListaServicios)
        //        {
        //            //Creo los registros de los datos de los detalles
        //            dtDetails.Rows.Add(dtDetails.NewRow());

        //            try { dtDetails.Rows[Control]["Cantidad"] = 1; }
        //            catch { dtDetails.Rows[Control]["Cantidad"] = ""; }

        //            if (item.TipoSolicitud.Code == "NORMAL")
        //            {
        //                try { dtDetails.Rows[Control]["Descripcion"] = "Carnet " + item.RangoCarnet.Name; }
        //                catch { dtDetails.Rows[Control]["Descripcion"] = ""; }

        //                try { dtDetails.Rows[Control]["Subtotal"] = item.Tarifa.Valor; }
        //                catch { dtDetails.Rows[Control]["Subtotal"] = ""; }
        //            }
        //            else//Si entra es porque es reexpedicion no atribuible
        //            {
        //                try { dtDetails.Rows[Control]["Descripcion"] = "Reexpedición Carnet " + item.RangoCarnet.Name; }
        //                catch { dtDetails.Rows[Control]["Descripcion"] = ""; }

        //                try { dtDetails.Rows[Control]["Subtotal"] = item.Tarifa.ValorReexpedicion; }
        //                catch { dtDetails.Rows[Control]["Subtotal"] = ""; }
        //            }
        //            Control++;
        //        }

        //    }
        //    else//Si entra es porque el pago es unitario
        //    {
        //        //Creo los registros de los datos de los detalles
        //        dtDetails.Rows.Add(dtDetails.NewRow());

        //        try { dtDetails.Rows[Control]["Cantidad"] = 1; }
        //        catch { dtDetails.Rows[Control]["Cantidad"] = ""; }

        //        if (registroPago.Solicitud.TipoSolicitud.Code == "NORMAL")
        //        {
        //            try { dtDetails.Rows[Control]["Descripcion"] = "Carnet " + registroPago.Solicitud.RangoCarnet.Name; }
        //            catch { dtDetails.Rows[Control]["Descripcion"] = ""; }

        //            try { dtDetails.Rows[Control]["Subtotal"] = registroPago.Solicitud.Tarifa.Valor; }
        //            catch { dtDetails.Rows[Control]["Subtotal"] = ""; }
        //        }
        //        else//Si entra es porque es reexpedicion no atribuible
        //        {
        //            try { dtDetails.Rows[Control]["Descripcion"] = "Reexpedición Carnet " + registroPago.Solicitud.RangoCarnet.Name; }
        //            catch { dtDetails.Rows[Control]["Descripcion"] = ""; }

        //            try { dtDetails.Rows[Control]["Subtotal"] = registroPago.Solicitud.Tarifa.ValorReexpedicion; }
        //            catch { dtDetails.Rows[Control]["Subtotal"] = ""; }
        //        }

        //    }

        //    dsReporte.Tables.Add(dtHeader);
        //    dsReporte.Tables.Add(dtDetails);

        //    //Muestro en pantalla el comprobante para luego imprimirlo
        //    ViewDocument Reporte1 = new ViewDocument(dsReporte, "FACTURA_AEROPUERTOBARRANQUILLA.rdl");
        //    Reporte1.Show();
        //}

        public static void imprimirDetalleOperacion(Operacion Operacion, List<Tasas> tasas, List<Servicios> servicios, double TRM, string tiempo)
        {
            ////Variables Auxiliares
            //DataTable dtHeader = new DataTable("Header");
            //DataTable dtDetails = new DataTable("Detalle");
            //DataSet dsReporte = new DataSet();
            //Int32 Control = 0;//Variable de control para el numero de filas que lleva.

            //dtHeader.Columns.Add("Header");

            //////Creo los nombres de los datos a mostrar en el encabezado, deben ser iguales a los utilizados en el reporte
            //dtDetails.Columns.Add("CONSECUTIVO");
            //dtDetails.Columns.Add("MODALIDAD");
            //dtDetails.Columns.Add("TRM");
            //dtDetails.Columns.Add("RECARGONOCTURNO");
            //dtDetails.Columns.Add("MATRICULA");
            //dtDetails.Columns.Add("COMPAÑIA");
            //dtDetails.Columns.Add("PBMO");
            //dtDetails.Columns.Add("INFORMACIONLLEGADAORIGEN");
            //dtDetails.Columns.Add("INFORMACIONLLEGADATIPOVUELOLLEG");
            //dtDetails.Columns.Add("INFORMACIONLLEGADAVUELOLLEGADA");
            //dtDetails.Columns.Add("INFORMACIONLLEGADAFECHA/HORAATERR");
            //dtDetails.Columns.Add("INFORMACIONLLEGADAFECHA/HORAPLATA");
            //dtDetails.Columns.Add("INFORMACIONLLEGADAPOSICIONLLEGADA");
            //dtDetails.Columns.Add("INFORMACIONLLEGADAFECHA/HORAPTE");
            //dtDetails.Columns.Add("INFORMACIONSALIDADESTINO");
            //dtDetails.Columns.Add("INFORMACIONSALIDATIPODEVUELO");
            //dtDetails.Columns.Add("INFORMACIONSALIDAVUELOSALIDA");
            //dtDetails.Columns.Add("INFORMACIONSALIDAPOSICIONSALIDA");
            //dtDetails.Columns.Add("INFORMACIONSALIDAFECHA/HORAPTE");
            //dtDetails.Columns.Add("INFORMACIONSALIDAFECHA/HORAPLATA");
            //dtDetails.Columns.Add("INFORMACIONSALIDAFECHA/HORADESP");
            //dtDetails.Columns.Add("INFORMACIONSALIDATIEMPO(HH-MM)");
            //dtDetails.Columns.Add("PASAJERO");
            //dtDetails.Columns.Add("CARGA");
            //dtDetails.Columns.Add("CORREO1");
            //dtDetails.Columns.Add("PAGANTASA");
            //dtDetails.Columns.Add("TRIPADI");
            //dtDetails.Columns.Add("INFANTES");
            //dtDetails.Columns.Add("EXENTOS");
            //dtDetails.Columns.Add("TRANSITOS");
            //dtDetails.Columns.Add("CONEXIONES");
            //dtDetails.Columns.Add("TILEMBARCADO");
            //dtDetails.Columns.Add("CARGAR");
            //dtDetails.Columns.Add("CORREO2");
            //dtDetails.Columns.Add("DERECHOSNACIONALESTASAS");
            //dtDetails.Columns.Add("DERECHOSNACIONALESAERODROMO");
            //dtDetails.Columns.Add("DERECHOSNACIONALESRECNOC.AERODROMO");
            //dtDetails.Columns.Add("DERECHOSNACIONALESPUENTES");
            //dtDetails.Columns.Add("DERECHOSNACIONALESESTACIONAMIENTO");
            //dtDetails.Columns.Add("DERECHOSNACIONALESBOMBEROSASIST");
            //dtDetails.Columns.Add("DERECHOSNACIONALESBOMBEROSLIMPIEZA");
            //dtDetails.Columns.Add("DERECHOSNACIONALESTOTALAFACTURARCOP");
            //dtDetails.Columns.Add("DERECHOSINTERNACIONALESTASAS");
            //dtDetails.Columns.Add("DERECHOSINTERNACIONALESAERODROMO");
            //dtDetails.Columns.Add("DERECHOSINTERNACIONALESRECNOC.AERÓDROMO");
            //dtDetails.Columns.Add("DERECHOSINTERNACIONALESPUENTES");
            //dtDetails.Columns.Add("DERECHOSINTERNACIONALESESTACIONAMIENTO");
            //dtDetails.Columns.Add("DERECHOSINTERNACIONALESBOMBEROSASIST");
            //dtDetails.Columns.Add("DERECHOSINTERNACIONALESBOMBEROS_LIMPIEZA");
            //dtDetails.Columns.Add("DERECHOSINTERNACIONALESTOTALAFACTURARCOP");
            //dtDetails.Columns.Add("DERECHOSINTERNACIONALESTOTALAFACTURARUSD");
            //dtDetails.Columns.Add("SubTotalCOP");
            //dtDetails.Columns.Add("SubTotalUSD");
            //dtDetails.Columns.Add("LIQUIDADOPOR");
            //dtDetails.Columns.Add("FACTURANo");
            //dtDetails.Columns.Add("Fechaimpresion");

            //dtHeader.Rows.Add(dtHeader.NewRow());
            //dtHeader.Rows[0]["Header"] = "";

            //dtDetails.Rows.Add(dtDetails.NewRow());

            //try { dtDetails.Rows[Control]["TRM"] = TRM; }
            //catch { dtDetails.Rows[Control]["TRM"] = "-"; }

            //try
            //{
            //    if (servicios.Where(f => f.TipoServicio.Code == "RECARGONOC").Count() >= 1)
            //    {
            //        dtDetails.Rows[Control]["RECARGONOCTURNO"] = "SI";
            //    }
            //    else
            //    {
            //        dtDetails.Rows[Control]["RECARGONOCTURNO"] = "NO";

            //    }
            //}
            //catch { dtDetails.Rows[Control]["RECARGONOCTURNO"] = "-"; }

            //try { dtDetails.Rows[Control]["CONSECUTIVO"] = Operacion.RowID; }
            //catch { dtDetails.Rows[Control]["CONSECUTIVO"] = "-"; }

            //try { dtDetails.Rows[Control]["MODALIDAD"] = Operacion.Tipo.Code; }
            //catch { dtDetails.Rows[Control]["MODALIDAD"] = "-"; }

            //try { dtDetails.Rows[Control]["MATRICULA"] = Operacion.Aeronave.Matricula; }
            //catch { dtDetails.Rows[Control]["MATRICULA"] = "-"; }

            //try { dtDetails.Rows[Control]["COMPAÑIA"] = Operacion.Aeronave.CompañiaFactura.NombreCompleto; }
            //catch { dtDetails.Rows[Control]["COMPAÑIA"] = "-"; }

            //try { dtDetails.Rows[Control]["PBMO"] = Operacion.Aeronave.PBMOKG; }
            //catch { dtDetails.Rows[Control]["PBMO"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONLLEGADAORIGEN"] = Operacion.Llegada.Origen.SiglaIATA; }
            //catch { dtDetails.Rows[Control]["INFORMACIONLLEGADAORIGEN"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONLLEGADATIPOVUELOLLEG"] = Operacion.Llegada.TipoVuelo.Code; }
            //catch { dtDetails.Rows[Control]["INFORMACIONLLEGADATIPOVUELOLLEG"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONLLEGADAVUELOLLEGADA"] = Operacion.Llegada.NVuelo; }
            //catch { dtDetails.Rows[Control]["INFORMACIONLLEGADAVUELOLLEGADA"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONLLEGADAFECHA/HORAATERR"] = String.Format("{0:MM/dd/yyyy}", Operacion.Llegada.FechaAterrizaje.Value) + " - " + Operacion.Llegada.HoraAterrizaje; }
            //catch { dtDetails.Rows[Control]["INFORMACIONLLEGADAFECHA/HORAATERR"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONLLEGADAFECHA/HORAPLATA"] = String.Format("{0:MM/dd/yyyy}", Operacion.Llegada.FechaLLegadaPlataforma.Value) + " - " + Operacion.Llegada.HoraPlataforma; }
            //catch { dtDetails.Rows[Control]["INFORMACIONLLEGADAFECHA/HORAPLATA"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONLLEGADAPOSICIONLLEGADA"] = Operacion.Llegada.TipoPosicion.Name + " " + Operacion.Llegada.Posicion.Name; }
            //catch { dtDetails.Rows[Control]["INFORMACIONLLEGADAPOSICIONLLEGADA"] = "-"; }

            //try
            //{
            //    if (Operacion.Llegada.TipoPosicion.Code == "PUENTE")
            //    {
            //        dtDetails.Rows[Control]["INFORMACIONLLEGADAFECHA/HORAPTE"] = String.Format("{0:MM/dd/yyyy}", Operacion.Llegada.FechaLlegadaPuente.Value) + " - " + Operacion.Llegada.HoraPuente;
            //    }
            //    else
            //    {
            //        dtDetails.Rows[Control]["INFORMACIONLLEGADAFECHA/HORAPTE"] = "-";
            //    }
            //}
            //catch { dtDetails.Rows[Control]["INFORMACIONLLEGADAFECHA/HORAPTE"] = "-"; }

            //try { dtDetails.Rows[Control]["PASAJERO"] = Operacion.Llegada.CantPax; }
            //catch { dtDetails.Rows[Control]["PASAJERO"] = "-"; }

            //try { dtDetails.Rows[Control]["CARGA"] = Operacion.Llegada.CargaEntrada; }
            //catch { dtDetails.Rows[Control]["CARGA"] = "-"; }

            //try { dtDetails.Rows[Control]["CORREO1"] = Operacion.Llegada.CorreoEntrada; }
            //catch { dtDetails.Rows[Control]["CORREO1"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONSALIDADESTINO"] = Operacion.Salida.Destino.SiglaIATA; }
            //catch { dtDetails.Rows[Control]["INFORMACIONSALIDADESTINO"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONSALIDATIPODEVUELO"] = Operacion.Salida.TipoVueloSalida.Code; }
            //catch { dtDetails.Rows[Control]["INFORMACIONSALIDATIPODEVUELO"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONSALIDAVUELOSALIDA"] = Operacion.Salida.NVueloSalida; }
            //catch { dtDetails.Rows[Control]["INFORMACIONSALIDAVUELOSALIDA"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONSALIDAPOSICIONSALIDA"] = Operacion.Salida.TipoPosicionSalida.Name + " " + Operacion.Salida.PosicionSalida.Name; }
            //catch { dtDetails.Rows[Control]["INFORMACIONSALIDAPOSICIONSALIDA"] = "-"; }

            //try
            //{
            //    if (Operacion.Salida.TipoPosicionSalida.Code == "PUENTE")
            //    {
            //        dtDetails.Rows[Control]["INFORMACIONSALIDAFECHA/HORAPTE"] = String.Format("{0:MM/dd/yyyy}", Operacion.Salida.FechaSalidaPuente.Value) + " - " + Operacion.Salida.HoraSalidaPuente;
            //    }
            //    else
            //    {
            //        dtDetails.Rows[Control]["INFORMACIONSALIDAFECHA/HORAPTE"] = "";
            //    }
            //}
            //catch { dtDetails.Rows[Control]["INFORMACIONSALIDAFECHA/HORAPTE"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONSALIDAFECHA/HORAPLATA"] = String.Format("{0:MM/dd/yyyy}", Operacion.Salida.FechaSalidaPlataforma.Value) + " - " + Operacion.Salida.HoraSalidaPlataforma; }
            //catch { dtDetails.Rows[Control]["INFORMACIONSALIDAFECHA/HORAPLATA"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONSALIDAFECHA/HORADESP"] = String.Format("{0:MM/dd/yyyy}", Operacion.Salida.FechaDespegue.Value) + " - " + Operacion.Salida.HoraDespegue; }
            //catch { dtDetails.Rows[Control]["INFORMACIONSALIDAFECHA/HORADESP"] = "-"; }

            //try { dtDetails.Rows[Control]["INFORMACIONSALIDATIEMPO(HH-MM)"] = servicios.Where(f => f.TipoServicio.Code == "PARQUEO").First().Cantidad; }
            //catch { dtDetails.Rows[Control]["INFORMACIONSALIDATIEMPO(HH-MM)"] = "-"; }

            //try { dtDetails.Rows[Control]["CARGAR"] = Operacion.Salida.CargaSalida; }
            //catch { dtDetails.Rows[Control]["CARGAR"] = "-"; }

            //try { dtDetails.Rows[Control]["CORREO2"] = Operacion.Salida.CorreoSalida; }
            //catch { dtDetails.Rows[Control]["CORREO2"] = "-"; }

            //try
            //{
            //    dtDetails.Rows[Control]["PAGANTASA"] = tasas.Where(f => f.TipoTasa.Code != "CREDITO").Sum(f => f.PaganTasa) - tasas.Where(f => f.TipoTasa.Code == "CREDITO").Sum(f => f.PaganTasa);
            //}
            //catch { dtDetails.Rows[Control]["PAGANTASA"] = "-"; }

            //try
            //{
            //    dtDetails.Rows[Control]["TRIPADI"] = tasas.Where(f => f.TipoTasa.Code != "CREDITO").Sum(f => f.Tripulantes) - tasas.Where(f => f.TipoTasa.Code == "CREDITO").Sum(f => f.Tripulantes);
            //}
            //catch { dtDetails.Rows[Control]["TRIPADI"] = "-"; }

            //try
            //{
            //    dtDetails.Rows[Control]["INFANTES"] = tasas.Where(f => f.TipoTasa.Code != "CREDITO").Sum(f => f.Infantes) - tasas.Where(f => f.TipoTasa.Code == "CREDITO").Sum(f => f.Infantes);
            //}
            //catch { dtDetails.Rows[Control]["INFANTES"] = "-"; }

            //try
            //{
            //    dtDetails.Rows[Control]["EXENTOS"] = tasas.Where(f => f.TipoTasa.Code != "CREDITO").Sum(f => f.Otros + f.Militares) - tasas.Where(f => f.TipoTasa.Code == "CREDITO").Sum(f => f.Otros + f.Militares);
            //}
            //catch { dtDetails.Rows[Control]["EXENTOS"] = "-"; }

            //try
            //{
            //    dtDetails.Rows[Control]["TRANSITOS"] = tasas.Where(f => f.TipoTasa.Code != "CREDITO").Sum(f => f.Transitos) - tasas.Where(f => f.TipoTasa.Code == "CREDITO").Sum(f => f.Transitos);
            //}
            //catch { dtDetails.Rows[Control]["TRANSITOS"] = "-"; }

            //try { dtDetails.Rows[Control]["CONEXIONES"] = "0"; }
            //catch { dtDetails.Rows[Control]["CONEXIONES"] = "-"; }

            //try
            //{
            //    dtDetails.Rows[Control]["TILEMBARCADO"] = tasas.Where(f => f.TipoTasa.Code != "CREDITO").Sum(f => f.PasajerosEmbarcados) - tasas.Where(f => f.TipoTasa.Code == "CREDITO").Sum(f => f.PasajerosEmbarcados);
            //}
            //catch { dtDetails.Rows[Control]["TILEMBARCADO"] = "-"; }

            //double acumNacional = 0, acumInternacional = 0;
            //foreach (Servicios servicio in servicios)
            //{

            //    //Evaluo Tasas con la salida
            //    if (Operacion.Salida.TipoVueloSalida.Code == "NACIONAL")
            //    {
            //        switch (servicio.TipoServicio.Code)
            //        {
            //            case "TASAS":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSNACIONALESTASAS"] = servicio.Valor;
            //                    acumNacional = acumNacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSNACIONALESTASAS"] = "-"; }
            //                break;
            //            case "TASASDEBITO":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSNACIONALESTASAS"] = Convert.ToDouble(dtDetails.Rows[Control]["DERECHOSNACIONALESTASAS"]) + servicio.Valor;
            //                    acumNacional = acumNacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSNACIONALESTASAS"] = "-"; }
            //                break;
            //            case "TASASCREDITO":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSNACIONALESTASAS"] = Convert.ToDouble(dtDetails.Rows[Control]["DERECHOSNACIONALESTASAS"]) - servicio.Valor;
            //                    acumNacional = acumNacional - servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSNACIONALESTASAS"] = "-"; }
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        switch (servicio.TipoServicio.Code)
            //        {
            //            case "TASAS":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSINTERNACIONALESTASAS"] = servicio.Valor;
            //                    acumInternacional = acumInternacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESTASAS"] = "-"; }
            //                break;
            //            case "TASASDEBITO":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSINTERNACIONALESTASAS"] = Convert.ToDouble(dtDetails.Rows[Control]["DERECHOSNACIONALESTASAS"]) + servicio.Valor;
            //                    acumInternacional = acumInternacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESTASAS"] = "-"; }
            //                break;
            //            case "TASASCREDITO":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSINTERNACIONALESTASAS"] = Convert.ToDouble(dtDetails.Rows[Control]["DERECHOSNACIONALESTASAS"]) - servicio.Valor;
            //                    acumInternacional = acumInternacional - servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESTASAS"] = "-"; }
            //                break;

            //        }
            //    }
            //    //Evaluo lo demas con la llegada
            //    if (Operacion.Llegada.TipoVuelo.Code == "NACIONAL")
            //    {
            //        switch (servicio.TipoServicio.Code)
            //        {
            //            case "AERODROMO":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSNACIONALESAERODROMO"] = servicio.Valor;
            //                    acumNacional = acumNacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSNACIONALESAERODROMO"] = "-"; }
            //                break;
            //            case "RECARGONOC":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSNACIONALESRECNOC.AERODROMO"] = servicio.Valor;
            //                    acumNacional = acumNacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSNACIONALESRECNOC.AERODROMO"] = "-"; }
            //                break;
            //            case "PUENTES":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSNACIONALESPUENTES"] = servicio.Valor;
            //                    acumNacional = acumNacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSNACIONALESPUENTES"] = "-"; }
            //                break;
            //            case "PARQUEO":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSNACIONALESESTACIONAMIENTO"] = servicio.Valor;
            //                    acumNacional = acumNacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSNACIONALESESTACIONAMIENTO"] = "-"; }
            //                break;
            //            case "ASISTENCIA":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSNACIONALESBOMBEROSASIST"] = dtDetails.Rows[Control]["DERECHOSNACIONALESBOMBEROSASIST"] != DBNull.Value ? Convert.ToDouble(dtDetails.Rows[Control]["DERECHOSNACIONALESBOMBEROSASIST"]) + servicio.Valor : servicio.Valor;
            //                    acumNacional = acumNacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSNACIONALESBOMBEROSASIST"] = "-"; }
            //                break;
            //            case "LIMPIEZA":
            //                try
            //                {
            //                    if (dtDetails.Rows[Control]["DERECHOSNACIONALESBOMBEROSLIMPIEZA"] != DBNull.Value)
            //                    {
            //                        dtDetails.Rows[Control]["DERECHOSNACIONALESBOMBEROSLIMPIEZA"] = Convert.ToDouble(dtDetails.Rows[Control]["DERECHOSNACIONALESBOMBEROSLIMPIEZA"]) + servicio.Valor;
            //                        acumNacional = acumNacional + servicio.Valor;
            //                    }
            //                    else
            //                    {
            //                        dtDetails.Rows[Control]["DERECHOSNACIONALESBOMBEROSLIMPIEZA"] = servicio.Valor;
            //                        acumNacional = acumNacional + servicio.Valor;
            //                    }

            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSNACIONALESBOMBEROSLIMPIEZA"] = "-"; }
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        switch (servicio.TipoServicio.Code)
            //        {
            //            case "AERODROMO":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSINTERNACIONALESAERODROMO"] = servicio.Valor;
            //                    acumInternacional = acumInternacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESAERODROMO"] = "-"; }
            //                break;
            //            case "RECARGONOC":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSINTERNACIONALESRECNOC.AERÓDROMO"] = servicio.Valor;
            //                    acumInternacional = acumInternacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESRECNOC.AERÓDROMO"] = "-"; }
            //                break;
            //            case "PUENTES":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSINTERNACIONALESPUENTES"] = servicio.Valor;
            //                    acumInternacional = acumInternacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESPUENTES"] = "-"; }
            //                break;
            //            case "PARQUEO":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSINTERNACIONALESESTACIONAMIENTO"] = servicio.Valor;
            //                    acumInternacional = acumInternacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESESTACIONAMIENTO"] = "-"; }
            //                break;
            //            case "ASISTENCIA":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSINTERNACIONALESBOMBEROSASIST"] = dtDetails.Rows[Control]["DERECHOSINTERNACIONALESBOMBEROSASIST"] != DBNull.Value ? Convert.ToDouble(dtDetails.Rows[Control]["DERECHOSINTERNACIONALESBOMBEROSASIST"]) + servicio.Valor : servicio.Valor;
            //                    acumInternacional = acumInternacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESBOMBEROSASIST"] = "-"; }
            //                break;
            //            case "LIMPIEZA":
            //                try
            //                {
            //                    dtDetails.Rows[Control]["DERECHOSINTERNACIONALESBOMBEROS_LIMPIEZA"] = dtDetails.Rows[Control]["DERECHOSINTERNACIONALESBOMBEROS_LIMPIEZA"] != DBNull.Value ? Convert.ToDouble(dtDetails.Rows[Control]["DERECHOSINTERNACIONALESBOMBEROS_LIMPIEZA"]) + servicio.Valor : servicio.Valor;
            //                    acumInternacional = acumInternacional + servicio.Valor;
            //                }
            //                catch { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESBOMBEROS_LIMPIEZA"] = "-"; }
            //                break;
            //        }
            //    }

            //}




            //try { dtDetails.Rows[Control]["SubTotalCOP"] = acumNacional; }
            //catch { dtDetails.Rows[Control]["SubTotalCOP"] = "-"; }


            //try { dtDetails.Rows[Control]["SubTotalUSD"] = (acumInternacional / TRM).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")); }
            //catch { dtDetails.Rows[Control]["SubTotalUSD"] = "-"; }

            //try
            //{
            //    dtDetails.Rows[Control]["DERECHOSNACIONALESTOTALAFACTURARCOP"] = acumNacional + acumInternacional;
            //}
            //catch { dtDetails.Rows[Control]["DERECHOSNACIONALESTOTALAFACTURARCOP"] = "-"; }

            //try { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESTOTALAFACTURARUSD"] = ((acumInternacional + acumNacional) / TRM).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")); }
            //catch { dtDetails.Rows[Control]["DERECHOSINTERNACIONALESTOTALAFACTURARUSD"] = "-"; }



            //try
            //{
            //    dtDetails.Rows[Control]["LIQUIDADOPOR"] = Operacion.CreatedBy;
            //    //Si tiene servicios con facturas en null quiere decir que es credito/Abonos y por lo cual no tiene una factura asociada
            //    if (servicios.Where(f => f.Factura == null).Count() >= 1)
            //    {
            //        dtDetails.Rows[Control]["LIQUIDADOPOR"] = servicios.First().CreatedBy;
            //    }
            //    else
            //    {
            //        dtDetails.Rows[Control]["LIQUIDADOPOR"] = servicios.First().Factura.CreatedBy;
            //    }

            //}
            //catch { dtDetails.Rows[Control]["LIQUIDADOPOR"] = "-"; }

            //try
            //{
            //    //Si tiene servicios con facturas en null quiere decir que es credito/Abonos y por lo cual no tiene una factura asociada
            //    if (servicios.Where(f => f.Factura == null).Count() >= 1)
            //    {
            //        dtDetails.Rows[Control]["FACTURANo"] = "-";
            //    }
            //    else
            //    {
            //        dtDetails.Rows[Control]["FACTURANo"] = servicios.Where(f => f.Factura != null).First().Factura.RowID;
            //    }
            //}

            //catch { dtDetails.Rows[Control]["FACTURANo"] = "--"; }

            //try { dtDetails.Rows[Control]["Fechaimpresion"] = DateTime.Now.ToString("D", CultureInfo.CreateSpecificCulture("es-ES")); }
            //catch { dtDetails.Rows[Control]["Fechaimpresion"] = "-"; }



            //dsReporte.Tables.Add(dtHeader);
            //dsReporte.Tables.Add(dtDetails);

            ////Muestro en pantalla el comprobante para luego imprimirlo
            //ViewDocument Reporte1 = new ViewDocument(dsReporte, "INFORME_DETALLE_OPERACION.rdl");

            //Reporte1.Show();

        }

    }


}
