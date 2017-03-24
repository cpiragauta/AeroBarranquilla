using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Model;

namespace WpfFront.Modelo
{
    public interface ITasasModel
    {
    }

    public class TasasModel : BusinessEntityBase, ITasasModel
    {

        private IList<Tipo> _ListaTiempoValidez;
        public IList<Tipo> ListaTiempoValidez
        {
            get { return _ListaTiempoValidez; }
            set
            {
                _ListaTiempoValidez = value;
                OnPropertyChanged("ListaTiempoValidez");
            }
        }

        private IList<Tipo> _ListaTiempoValidezFiltro;
        public IList<Tipo> ListaTiempoValidezFiltro
        {
            get { return _ListaTiempoValidez; }
            set
            {
                _ListaTiempoValidez = value;
                OnPropertyChanged("ListaTiempoValidezFiltro");
            }
        }

        private IList<Tipo> _ListaTipoTasa;
        public IList<Tipo> ListaTipoTasa
        {
            get { return _ListaTipoTasa; }
            set
            {
                _ListaTipoTasa = value;
                OnPropertyChanged("ListaTipoTasa");
            }
        }

        private IList<Tarifa> _ListaTarifas;
        public IList<Tarifa> ListaTarifas
        {
            get { return _ListaTarifas; }
            set
            {
                _ListaTarifas = value;
                OnPropertyChanged("ListaTarifas");
            }
        }

        private Tarifa _RecordTarifa;
        public Tarifa RecordTarifa
        {
            get { return _RecordTarifa; }
            set
            {
                _RecordTarifa = value;
                OnPropertyChanged("RecordTarifa");
            }
        }


        private Tarifa _RecordTarifaBusqueda;
        public Tarifa RecordTarifaBusqueda
        {
            get { return _RecordTarifaBusqueda; }
            set
            {
                _RecordTarifaBusqueda = value;
                OnPropertyChanged("RecordTarifaBusqueda");
            }
        }
    }
}