using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Model;


namespace WpfFront.Modelo
{
    public interface IPlaneacionModel
    {
        //IList<Tipo> ListaTipoFactura { get; set; }
        IList<Tipo> ListaBanda { get; set; }
        Planeacion RecordBusquedaPlaneacion { get; set; }

    }

    public class PlaneacionModel : BusinessEntityBase, IPlaneacionModel
    {
        private IList<Planeacion> _ListadoPlaneacion;
        public IList<Planeacion> ListadoPlaneacion
        {
            get { return _ListadoPlaneacion; }
            set
            {
                _ListadoPlaneacion = value;
                OnPropertyChanged("ListadoPlaneacion");
            }
        }

        private Planeacion _RecordPlaneacion;
        public Planeacion RecordPlaneacion
        {
            get { return _RecordPlaneacion; }
            set
            {
                _RecordPlaneacion = value;
                OnPropertyChanged("RecordPlaneacion");
            }
        }

        private Planeacion _RecordBusquedaPlaneacion;
        public Planeacion RecordBusquedaPlaneacion
        {
            get { return _RecordBusquedaPlaneacion; }
            set
            {
                _RecordBusquedaPlaneacion = value;
                OnPropertyChanged("RecordBusquedaPlaneacion");
            }
        }

        private IList<Tipo> _ListaBanda;
        public IList<Tipo> ListaBanda
        {
            get { return _ListaBanda; }
            set
            {
                _ListaBanda = value;
                OnPropertyChanged("ListaBanda");
            }
        }

    }
}