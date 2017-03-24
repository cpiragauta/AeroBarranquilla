using System.Collections.Generic;
using Core.BusinessEntity;
using System.Data;
using WpfFront.Model;

namespace WpfFront.Modelo
{
    public interface IFacturasTasasModel
    {

        IList<Tipo> AerolineaList { get; set; }
        Facturas RecordFacturas { get; set; }
        IList<Facturas> FacturasList { get; set; }
        IList<Servicios> RecordListAerodromo { get; set; }
        IList<Servicios> RecordListAerodromoAgrupada { get; set; }
        IList<Tipo> ListaTipoOperacion { get; set; }
        IList<Tipo> ListaTipoTasa { get; set; }
        IList<Servicios> RecordServiciosAgrupadosList { get; set; }

    }

    public class FacturasTasasModel : BusinessEntityBase, IFacturasTasasModel
    {

        private IList<Servicios> _RecordServiciosAgrupadosList;
        public IList<Servicios> RecordServiciosAgrupadosList
        {
            get { return _RecordServiciosAgrupadosList; }
            set
            {
                _RecordServiciosAgrupadosList = value;
                OnPropertyChanged("RecordServiciosAgrupadosList");
            }
        }

        private IList<Tipo> _ListaTipoOperacion;
        public IList<Tipo> ListaTipoOperacion
        {
            get { return _ListaTipoOperacion; }
            set
            {
                _ListaTipoOperacion = value;
                OnPropertyChanged("ListaTipoOperacion");
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


        private IList<Servicios> _RecordListAerodromo;
        public IList<Servicios> RecordListAerodromo
        {
            get { return _RecordListAerodromo; }
            set
            {
                _RecordListAerodromo = value;
                OnPropertyChanged("RecordListAerodromo");
            }
        }

        private IList<Servicios> _RecordListAerodromoAgrupada;
        public IList<Servicios> RecordListAerodromoAgrupada
        {
            get { return _RecordListAerodromoAgrupada; }
            set
            {
                _RecordListAerodromoAgrupada = value;
                OnPropertyChanged("RecordListAerodromoAgrupada");
            }
        }


        private Facturas _Factura;
        public Facturas Factura
        {
            get { return _Factura; }
            set
            {
                _Factura = value;
                OnPropertyChanged("Factura");
            }
        }

        private DataTable _RecordList;
        public DataTable RecordList
        {
            get { return _RecordList; }
            set
            {
                _RecordList = value;
                OnPropertyChanged("RecordList");
            }
        }

        private IList<Tipo> _AerolineaList;
        public IList<Tipo> AerolineaList
        {
            get { return _AerolineaList; }
            set
            {
                _AerolineaList = value;
                OnPropertyChanged("AerolineaList");
            }
        }

        private Facturas _RecordFacturas;
        public Facturas RecordFacturas
        {
            get { return _RecordFacturas; }
            set
            {
                _RecordFacturas = value;
                OnPropertyChanged("RecordFacturas");
            }
        }

        private IList<Facturas> _FacturasList;
        public IList<Facturas> FacturasList
        {
            get { return _FacturasList; }
            set
            {
                _FacturasList = value;
                OnPropertyChanged("FacturasList");
            }
        }




    }
}