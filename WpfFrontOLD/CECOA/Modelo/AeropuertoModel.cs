using System;
using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Model;

namespace WpfFront.Modelo
{
    public interface IAeropuertoModel
    {
        Aeropuerto Aeropuerto { get; set; }
        IList<Aeropuerto>ListAeropuertos{ get; set; }
        IList<Tipo> ListaTipoAeropuerto { get; set; }
    }

    public class AeropuertoModel : BusinessEntityBase, IAeropuertoModel
    {
       
        private IList<Aeropuerto> _ListAeropuertos;
        public IList<Aeropuerto> ListAeropuertos
        {
            get { return _ListAeropuertos; }
            set
            {
                _ListAeropuertos = value;
                OnPropertyChanged("ListAeropuertos");
            }
        }

        private Aeropuerto _Aeropuerto;
        public Aeropuerto Aeropuerto
        {
            get { return _Aeropuerto; }
            set
            {
                _Aeropuerto = value;
                OnPropertyChanged("Aeropuerto");
            }
        }

        private IList<Tipo> _ListaTipoAeropuerto;
        public IList<Tipo> ListaTipoAeropuerto
        {
            get { return _ListaTipoAeropuerto; }
            set
            {
                _ListaTipoAeropuerto = value;
                OnPropertyChanged("ListaTipoAeropuerto");
            }
        }
    }
}