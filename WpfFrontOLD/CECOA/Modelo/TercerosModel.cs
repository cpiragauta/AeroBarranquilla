using Core.BusinessEntity;
using System.Collections.Generic;
using WpfFront.Model;
namespace WpfFront.Modelo
{
    public interface ITercerosModel
    {
        Tercero RecordTercero { get; set; }

    }

    public class TercerosModel : BusinessEntityBase, ITercerosModel
    {
        private IList<Tercero> _ListaTercero;
        public IList<Tercero> ListaTercero
        {
            get { return _ListaTercero; }
            set
            {
                _ListaTercero = value;
                OnPropertyChanged("ListaTercero");
            }
        }

        private Tercero _RecordTercero;
        public Tercero RecordTercero
        {
            get { return _RecordTercero; }
            set
            {
                _RecordTercero = value;
                OnPropertyChanged("RecordTercero");
            }
        }


        private Tercero _RecordBusqueda;
        public Tercero RecordBusqueda
        {
            get { return _RecordBusqueda; }
            set
            {
                _RecordBusqueda = value;
                OnPropertyChanged("RecordBusqueda");
            }
        }

        private IList<Tipo> listTipoTercero;
        public IList<Tipo> ListTipoTercero
        {
            get { return listTipoTercero; }
            set
            {
                listTipoTercero = value;
                OnPropertyChanged("ListTipoTercero");
            }
        }
    }
}
