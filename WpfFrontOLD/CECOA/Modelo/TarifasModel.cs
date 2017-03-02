//using System;
//using System.Collections.Generic;
//using Core.BusinessEntity;
//using Core.Validation;
//using WpfFront.Common;
//using WpfFront.WMSBusinessService;
//using System.Data;

//namespace WpfFront.Models
//{
//    public interface ITarifasModel
//    {
//        IList<Tarifas> ListTarifas { get; set; }
//        IList<Tarifas> ListTarifasArptrs { get; set; }
//       // IList<Tarifas> ListTarifasPuents { get; set; }
//        IList<Tarifas> ListTarifasParqos { get; set; }

//        Tarifas Tarifas { get; set; }

//        //Bomberos
//        IList<MMaster> ListaTipoServicio { get; set; }

//    }

//    public class TarifasModel : BusinessEntityBase, ITarifasModel
//    {

//        #region Aerodromo
//        private IList<Tarifas> _ListTarifas;
//        public IList<Tarifas> ListTarifas
//        {
//            get { return _ListTarifas; }
//            set
//            {
//                _ListTarifas = value;
//                OnPropertyChanged("ListTarifas");
//            }
//        }
//        private Tarifas _Tarifas;
//        public Tarifas Tarifas
//        {
//            get { return _Tarifas; }
//            set
//            {
//                _Tarifas = value;
//                OnPropertyChanged("Tarifas");
//            }
//        }
//        #endregion

//        #region AeroPuertuaria
//        private IList<Tarifas> _ListTarifasArptrs;
//        public IList<Tarifas> ListTarifasArptrs
//        {
//            get { return _ListTarifasArptrs; }
//            set
//            {
//                _ListTarifasArptrs = value;
//                OnPropertyChanged("ListTarifasArptrs");
//            }
//        }
//        private Tarifas _TarifasArptrs;
//        public Tarifas TarifasArptrs
//        {
//            get { return _TarifasArptrs; }
//            set
//            {
//                _TarifasArptrs = value;
//                OnPropertyChanged("TarifasArptrs");
//            }
//        }
//        #endregion

//        #region Bomberos

//        private IList<MMaster> _ListaTipoServicio;
//        public IList<MMaster> ListaTipoServicio
//        {
//            get { return _ListaTipoServicio; }
//            set
//            {
//                _ListaTipoServicio = value;
//                OnPropertyChanged("ListaTipoServicio");
//            }
//        }

//        private IList<Tarifas> _ListTarifasBomber;
//        public IList<Tarifas> ListTarifasBomber
//        {
//            get { return _ListTarifasBomber; }
//            set
//            {
//                _ListTarifasBomber = value;
//                OnPropertyChanged("ListTarifasBomber");
//            }
//        }

//        private Tarifas _TarifasBomber;
//        public Tarifas TarifasBomber
//        {
//            get { return _TarifasBomber; }
//            set
//            {
//                _TarifasBomber = value;
//                OnPropertyChanged("TarifasBomber");
//            }
//        }
//        #endregion

//        #region Puentes
//        private IList<Tarifas> _ListTarifasPuents;
//        public IList<Tarifas> ListTarifasPuents
//        {
//            get { return _ListTarifasPuents; }
//            set
//            {
//                _ListTarifasPuents = value;
//                OnPropertyChanged("ListTarifasPuents");
//            }
//        }
//        private Tarifas _TarifasPuents;
//        public Tarifas TarifasPuents
//        {
//            get { return _TarifasPuents; }
//            set
//            {
//                _TarifasPuents = value;
//                OnPropertyChanged("TarifasPuents");
//            }
//        }
//        #endregion

//        #region Parqueo
//        private IList<Tarifas> _ListTarifasParqos;
//        public IList<Tarifas> ListTarifasParqos
//        {
//            get { return _ListTarifasParqos; }
//            set
//            {
//                _ListTarifasParqos = value;
//                OnPropertyChanged("ListTarifasParqos");
//            }
//        }
//        private Tarifas _TarifasParqos;
//        public Tarifas TarifasParqos
//        {
//            get { return _TarifasParqos; }
//            set
//            {
//                _TarifasParqos = value;
//                OnPropertyChanged("TarifasParqos");
//            }
//        }
//        #endregion







//    }
//}