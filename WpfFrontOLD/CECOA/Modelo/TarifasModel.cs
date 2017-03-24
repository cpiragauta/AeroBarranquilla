
using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Model;



namespace WpfFront.Modelo
{
    public interface ITarifasModel
    {
        IList<TarifaCecoa> ListTarifas { get; set; }
        IList<TarifaCecoa> ListTarifasArptrs { get; set; }
       // IList<Tarifas> ListTarifasPuents { get; set; }
        IList<TarifaCecoa> ListTarifasParqos { get; set; }

        TarifaCecoa Tarifas { get; set; }

        //Bomberos
        IList<Tipo> ListaTipoServicio { get; set; }

    }

    public class TarifasModel : BusinessEntityBase, ITarifasModel
    {

        #region Aerodromo
        private IList<TarifaCecoa> _ListTarifas;
        public IList<TarifaCecoa> ListTarifas
        {
            get { return _ListTarifas; }
            set
            {
                _ListTarifas = value;
                OnPropertyChanged("ListTarifas");
            }
        }
        private TarifaCecoa _Tarifas;
        public TarifaCecoa Tarifas
        {
            get { return _Tarifas; }
            set
            {
                _Tarifas = value;
                OnPropertyChanged("Tarifas");
            }
        }
        #endregion

        #region AeroPuertuaria
        private IList<TarifaCecoa> _ListTarifasArptrs;
        public IList<TarifaCecoa> ListTarifasArptrs
        {
            get { return _ListTarifasArptrs; }
            set
            {
                _ListTarifasArptrs = value;
                OnPropertyChanged("ListTarifasArptrs");
            }
        }
        private TarifaCecoa _TarifasArptrs;
        public TarifaCecoa TarifasArptrs
        {
            get { return _TarifasArptrs; }
            set
            {
                _TarifasArptrs = value;
                OnPropertyChanged("TarifasArptrs");
            }
        }
        #endregion

        #region Bomberos

        private IList<Tipo> _ListaTipoServicio;
        public IList<Tipo> ListaTipoServicio
        {
            get { return _ListaTipoServicio; }
            set
            {
                _ListaTipoServicio = value;
                OnPropertyChanged("ListaTipoServicio");
            }
        }

        private IList<TarifaCecoa> _ListTarifasBomber;
        public IList<TarifaCecoa> ListTarifasBomber
        {
            get { return _ListTarifasBomber; }
            set
            {
                _ListTarifasBomber = value;
                OnPropertyChanged("ListTarifasBomber");
            }
        }

        private TarifaCecoa _TarifasBomber;
        public TarifaCecoa TarifasBomber
        {
            get { return _TarifasBomber; }
            set
            {
                _TarifasBomber = value;
                OnPropertyChanged("TarifasBomber");
            }
        }
        #endregion

        #region Puentes
        private IList<TarifaCecoa> _ListTarifasPuents;
        public IList<TarifaCecoa> ListTarifasPuents
        {
            get { return _ListTarifasPuents; }
            set
            {
                _ListTarifasPuents = value;
                OnPropertyChanged("ListTarifasPuents");
            }
        }
        private TarifaCecoa _TarifasPuents;
        public TarifaCecoa TarifasPuents
        {
            get { return _TarifasPuents; }
            set
            {
                _TarifasPuents = value;
                OnPropertyChanged("TarifasPuents");
            }
        }
        #endregion

        #region Parqueo
        private IList<TarifaCecoa> _ListTarifasParqos;
        public IList<TarifaCecoa> ListTarifasParqos
        {
            get { return _ListTarifasParqos; }
            set
            {
                _ListTarifasParqos = value;
                OnPropertyChanged("ListTarifasParqos");
            }
        }
        private TarifaCecoa _TarifasParqos;
        public TarifaCecoa TarifasParqos
        {
            get { return _TarifasParqos; }
            set
            {
                _TarifasParqos = value;
                OnPropertyChanged("TarifasParqos");
            }
        }
        #endregion







    }
}