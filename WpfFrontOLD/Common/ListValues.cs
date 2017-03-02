using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfFront.Common
{


    public class ListValues
    {
        public enum ErrorCategory { Persistence, ErpConnection, Presentation, WcfService }
        public enum EventType { Info = 1, warn = 2, Error = 3, Fatal = 4 }


    }

    public static class SEntregasCarnets
    {
        public const int Nueva = 221;
        public const int Anulada = 222;
        public const int Impresa = 223;
        public const int Facturada = 224;
        public const int Exenta = 225;
    }

    public static class SSolicitudesCarnets
    {
        public const int Nueva = 241;
        public const int Confirmada = 242;
        public const int Anulada = 243;
        public const int Impresa = 244;
    }

    public static class SNotasCarnets
    {
        public const int Nueva = 231;
        public const int Enviada = 232;
    }

    public static class SSolicitudCarnetizacion
    {
        public const int Solicitada = 201;
        public const int EnPagos = 202;
        public const int EnEntregas = 203;
        public const int Cerrada = 204;
        public const int Impresa = 205;
        public const int Nota = 206;
    }

    public static class SPagosCarnets
    {
        public const int Nueva = 211;
        public const int Anulada = 212;
        public const int Impreso = 213;
        public const int Facturado = 214;
        public const int Traslado = 215;
    }


    public static class ShortAge
    {
        public const string BackOrder = "BACKORDER";
        public const string Cancel = "CANCEL";
    }


    public static class BasicProcess
    {
        public const string Picking = "Picking";
        public const string ReceiptAcknowledge = "ReceiptAcknowledge";
        public const string Shipping = "Shipping";
    }


    public static class ProcessActivityType
    {
        public const short Automatic = 1;
        public const short Manual = 2;
    }



    public static class DefaultBin
    {
        public const string MAIN = "MAIN";
        public const string PICKING = "PICKING";
        public const string PUTAWAY = "MAIN";
        public const string PROCESS = "PROCESS";
        public const string RETURN = "RETURN";
        public const string DAMAGE = "DAMAGE";

    }

    public static class BinType
    {
        public const short In_Out = 0;
        public const short In_Only = 1;
        public const short Out_Only = 2;
    }


    public static class OptionTypes
    {
        public const short Application = 1;
        public const short Report = 2;
        public const short Device = 3;
        public const short Control = 4;
    }

    public static class WmsSetupValues
    {

        public const int MaxBinLength = 10;
        public const int NumRegs = 50; //Maximo de registros en una consulta de busqueda
        public const int MinWindowWith = 1124;
        public const int SearchLength = 2;
        public const string IconPath48 = "/WpfFront;component/Images/Icons/48x48/";
        public const string IconPath16 = "/WpfFront;component/Images/Icons/48x48/";
        public const string DocumentViewURL = "http://localhost:29258/ShowDocument.aspx";
        public const string PrintReportDir = "PrintReport";
        public const string RdlTemplateDir = "RDL";
        public const string CustomUnit = "Custom";
        public const string Confirm_ReverseReceipt_CrossDock = "This receipt was created in a Cross Dock process.\nCancel this receipt will reverse the entire Cross Dock Process.\nDo you want to cancel the receipt anyway  ?.";
        public const string DefaultLabelTemplate = "LBL_ReceivingLabel_4x6.rdl";
        public const string AssemblyLabelTemplate = "LBL_ProductUniqueLabel_4x2.rdl";
        public const string ProductLabelTemplate = "LBL_ProductGeneric_4x2.rdl";
        public const string DEFAULT = "DEFAULT";
        public const string DefaultPackLabelTemplate = "DOC_CartonContent.rdl";
        public const string Language = "en-US";
        public const string AutoSerialNumber = "AUTOSERIAL";
        public const string DefaultMenuIcon = "Node.png";
        public const string DefaultPalletLabelTemplate = "DOC_PalletContent.rdl";
        public const string CountTicketTemplate = "DOC_CountInitial.rdl";
        //public const string CountTicketTemplate_Summary = "DOC_CountingTasks_Summary.rdl";
        //public const string CountTicketTemplate_Full = "DOC_CountingTasks_Full.rdl";
    }

    public static class BasicRol
    {
        public const int Manager = 2;
        public const int Picker = 3;
        public const int Admin = 1;
    }

    public static class EntityStatus
    {

        public const int Active = 1001;
        public const int Inactive = 1002;
        public const int Locked = 1003;
    }


    public static class NodeType
    {
        public const int PreLabeled = 1;
        public const int Received = 2;
        //public const int Labeled = 3;
        public const int Stored = 4;
        public const int Picked = 5;
        public const int Packed = 6;
        public const int Released = 7;
        public const int Voided = 8;
        public const int Process = 9;

        //Entidades de Workflow
        public const int Package = 100;
    }


    public static class SDocClass
    {

        public const short Receiving = 1;

        public const short Shipping = 2;

        public const short Inventory = 3;

        public const short Task = 4;

        public const short Posting = 5;

        public const short Label = 10;

        public const short Message = 11;

        public const short DocumentForm = 12;

        public const short Process = 13;
    }




    public static class SDocType
    {

        public const int PurchaseOrder = 101;

        public const int Return = 102;

        public const int WarehouseTransferReceipt = 103;



        public const int PurchaseReceipt = 501;

        public const int InventoryAdjustment = 502;

        public const int SalesShipment = 503;

        public const int ReceiptConfirmation = 504;



        public const int ReceivingTask = 401;

        public const int PickTicket = 402;

        public const int CrossDock = 403;

        public const int KitAssemblyTask = 404;

        public const int ReplenishPackTask = 405;

        public const int ChangeLocation = 406;

        public const int CountTask = 407;


        public const int SalesOrder = 201;

        public const int BackOrder = 202;

        public const int SalesInvoice = 203;

        public const int WarehouseTransferShipment = 204;

        public const int PurchaseReturn = 205;

        public const int MergedSalesOrder = 206;

        public const int FileProcess = 1302;

        public const int Route = 1000;
    }



    public static class GP_DocType
    {

        public const int IV_Adjustment = 1;

        public const int IV_Variance = 2;

        public const int PR_Shipment = 1;

        public const int PR_Shipment_Invoice = 3;
    }



    public static class GPBatchNumber
    {

        public const string Receipt = "WMS_RECEIPT";

        public const string Inventory = "WMS_INVENTORY";

        public const string Shipping = "WMS_SHIPPING";
    }



    public static class PickingMethod
    {

        public const int ZONE = 1;

        public const int FIFO = 2;

        public const int LIFO = 3;

        public const int FEFO = 4;
    }


    public static class AccType
    {
        public const int Customer = 1;
        public const int Vendor = 2;
        public const int Transportation = 3;
    }


    public static class SStatusType
    {

        public const int Document = 1;
        public const int Active = 10;

    }


    public static class EntityID
    {
        public const short Company = 1;
        public const short Account = 2;
        public const short AccountAddress = 3;
        public const short Product = 4;
        public const short Location = 5;
        public const short Document = 6;
        public const short Process = 7;
        public const short Bin = 8;
        public const short LogError = 9;
        public const short DocumentType = 10;
        public const short Label = 20;

    }

    public static class AccntType
    {
        public const int Customer = 1;
        public const int Vendor = 2;
        public const int Transportation = 3;
    }

    public static class CnnType
    {
        public const int GPeConnect = 1;
        public const int GPWebServices = 2;
        public const int Printer = 10;
        public const int File = 20;
    }

    public static class SDataTypes
    {
        public const short String = 1;
        public const short Number = 2;
        public const short Bool = 3;
        public const short DateTime = 4;
        public const short ReceivingAlert = 5;
        public const short ShippingAlert = 6;
        public const short ProductQuality = 7;

    }

    public static class KitType
    {
        public const int Custom = 100;  // kits personalizados
        public const int ERP = 2;
    }

    public static class ExplodeKit
    {
        public const int Always = 1;
        public const int IfNotStock = 2;
        public const int Caterpillar = 3;
        public const int CaterpillarKit = 5;
    }
}
