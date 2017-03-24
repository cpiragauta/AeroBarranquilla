using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Model;

namespace WpfFront.Modelo
{
    public interface IListaServiciosPorAerolineaModel
    {

        IList<Servicios> RecordListAerodromo { get; set; }

    }

    public class ListaServiciosPorAerolineaModel : BusinessEntityBase, IListaServiciosPorAerolineaModel
    {
        private IList<Servicios> _RecordListAerodromo;
        public IList<Servicios> RecordListAerodromo
        {
            get { return _RecordListAerodromo; }
            set
            {
                _RecordListAerodromo = value;
                OnPropertyChanged("RecordServiciosDetalladoList");
            }
        }
    }
}