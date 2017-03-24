using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Model;

namespace WpfFront.Modelo
{
    public interface IListaServiciosPorFacturaModel
    {

        IList<Servicios> RecordServiciosDetalladoList { get; set; }

    }

    public class ListaServiciosPorFacturaModel : BusinessEntityBase, IListaServiciosPorFacturaModel
    {
        private IList<Servicios> _RecordServiciosDetalladoList;
        public IList<Servicios> RecordServiciosDetalladoList
        {
            get { return _RecordServiciosDetalladoList; }
            set
            {
                _RecordServiciosDetalladoList = value;
                OnPropertyChanged("RecordServiciosDetalladoList");
            }
        }
    }
}