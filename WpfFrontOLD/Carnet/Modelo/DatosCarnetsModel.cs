using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Controlador;
using WpfFront.Model;


namespace WpfFront.Modelo
{
    public interface IDatosCarnetsModel
    {

    }

    public class DatosCarnetsModel : BusinessEntityBase, IDatosCarnetsModel
    {
        #region Encabeado
        private Encabezado recordEncabezado;
        public Encabezado RecordEncabezado
        {
            get { return recordEncabezado; }
            set
            {
                recordEncabezado = value;
                OnPropertyChanged("RecordEncabezado");
            }
        }
        #endregion

        #region Solicitud
        private Solicitud recordSolicitud;
        public Solicitud RecordSolicitud
        {
            get { return recordSolicitud; }
            set
            {
                recordSolicitud = value;
                OnPropertyChanged("RecordSolicitud");
            }
        }

        private IList<Tipo> listTipoSolicitud;
        public IList<Tipo> ListTipoSolicitud
        {
            get { return listTipoSolicitud; }
            set
            {
                listTipoSolicitud = value;
                OnPropertyChanged("ListTipoSolicitud");
            }
        }

        private IList<Tipo> listTipoCarnet;
        public IList<Tipo> ListTipoCarnet
        {
            get { return listTipoCarnet; }
            set
            {
                listTipoCarnet = value;
                OnPropertyChanged("ListTipoCarnet");
            }
        }

        private IList<Tipo> listRangoCarnet;
        public IList<Tipo> ListRangoCarnet
        {
            get { return listRangoCarnet; }
            set
            {
                listRangoCarnet = value;
                OnPropertyChanged("ListRangoCarnet");
            }
        }

        private IList<Tipo> listTipoEmpleado;
        public IList<Tipo> ListTipoEmpleado
        {
            get { return listTipoEmpleado; }
            set
            {
                listTipoEmpleado = value;
                OnPropertyChanged("ListTipoEmpleado");
            }
        }

        private IList<Tipo> listAdicional;
        public IList<Tipo> ListAdicional
        {
            get { return listAdicional; }
            set
            {
                listAdicional = value;
                OnPropertyChanged("ListAdicional");
            }
        }

        private IList<Tipo> listAreasPersonal;
        public IList<Tipo> ListAreasPersonal
        {
            get { return listAreasPersonal; }
            set
            {
                listAreasPersonal = value;
                OnPropertyChanged("ListAreasPersonal");
            }
        }

        private IList<Tipo> listAreasVehiculos;
        public IList<Tipo> ListAreasVehiculos
        {
            get { return listAreasVehiculos; }
            set
            {
                listAreasVehiculos = value;
                OnPropertyChanged("ListAreasVehiculos");
            }
        }

        private IList<Solicitud> solicitudesPersona;
        public IList<Solicitud> SolicitudesPersona
        {
            get { return solicitudesPersona; }
            set
            {
                solicitudesPersona = value;
                OnPropertyChanged("SolicitudesPersona");
            }
        }

        private IList<Solicitud> solicitudesVehiculo;
        public IList<Solicitud> SolicitudesVehiculo
        {
            get { return solicitudesVehiculo; }
            set
            {
                solicitudesVehiculo = value;
                OnPropertyChanged("SolicitudesVehiculo");
            }
        }
        #endregion

        #region Pago

        private IList<Tipo> listTipoPago;
        public IList<Tipo> ListTipoPago
        {
            get { return listTipoPago; }
            set
            {
                listTipoPago = value;
                OnPropertyChanged("ListTipoPago");
            }
        }

        private IList<Tipo> listFormaPago;
        public IList<Tipo> ListFormaPago
        {
            get { return listFormaPago; }
            set
            {
                listFormaPago = value;
                OnPropertyChanged("ListFormaPago");
            }
        }

        private Pago recordPago;
        public Pago RecordPago
        {
            get { return recordPago; }
            set
            {
                recordPago = value;
                OnPropertyChanged("RecordPago");
            }
        }

        private IList<Solicitud> solicitudesParaPago;
        public IList<Solicitud> SolicitudesParaPago
        {
            get { return solicitudesParaPago; }
            set
            {
                solicitudesParaPago = value;
                OnPropertyChanged("SolicitudesParaPago");
            }
        }

        private IList<Pago> pagos;
        public IList<Pago> Pagos
        {
            get { return pagos; }
            set
            {
                pagos = value;
                OnPropertyChanged("Pagos");
            }
        }

        #endregion

        #region Entrega

        private IList<Tipo> listTipoEntrega;
        public IList<Tipo> ListTipoEntrega
        {
            get { return listTipoEntrega; }
            set
            {
                listTipoEntrega = value;
                OnPropertyChanged("ListTipoEntrega");
            }

        }


        private Entrega recordEntrega;
        public Entrega RecordEntrega
        {
            get { return recordEntrega; }
            set
            {
                recordEntrega = value;
                OnPropertyChanged("RecordEntrega");
            }
        }

        private IList<Pago> pagosParaEntrega;
        public IList<Pago> PagosParaEntrega
        {
            get { return pagosParaEntrega; }
            set
            {
                pagosParaEntrega = value;
                OnPropertyChanged("PagosParaEntrega");
            }
        }

        private IList<Entrega> entregas;
        public IList<Entrega> Entregas
        {
            get { return entregas; }
            set
            {
                entregas = value;
                OnPropertyChanged("Entregas");
            }
        }

        #endregion

    }
}