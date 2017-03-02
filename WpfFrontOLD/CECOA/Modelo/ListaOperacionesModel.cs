using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Model;

namespace WpfFront.Modelo
{
    public interface IListaOperacionesModel
    {
        //IList<Tipo> ListaTipoFactura { get; set; }
        IList<Operacion> ListaOperaciones { get; set; }
        Operacion RecordBusqueda { get; set; }
        IList<Tipo> ListaTipoFacturacion { get; set; }
        IList<Tipo> ListaTipoOp { get; set; }

    }

    public class ListaOperacionesModel : BusinessEntityBase, IListaOperacionesModel
    {

        private IList<Tipo> _ListaTipoOp;
        public IList<Tipo> ListaTipoOp
        {
            get { return _ListaTipoOp; }
            set
            {
                _ListaTipoOp = value;
                OnPropertyChanged("ListaTipoOp");
            }
        }

        private IList<Tipo> _ListaTipoFacturacion;
        public IList<Tipo> ListaTipoFacturacion
        {
            get { return _ListaTipoFacturacion; }
            set
            {
                _ListaTipoFacturacion = value;
                OnPropertyChanged("ListaTipoFacturacion");
            }
        }


        private IList<Operacion> _ListaOperaciones;
        public IList<Operacion> ListaOperaciones
        {
            get { return _ListaOperaciones; }
            set
            {
                _ListaOperaciones = value;
                OnPropertyChanged("ListaOperaciones");
            }
        }

        private Operacion _RecordBusqueda;
        public Operacion RecordBusqueda
        {
            get { return _RecordBusqueda; }
            set
            {
                _RecordBusqueda = value;
                OnPropertyChanged("RecordBusqueda");
            }
        }
    }
}