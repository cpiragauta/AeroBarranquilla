using System;
using System.Collections.Generic;
using Core.BusinessEntity;
using Core.Validation;
using WpfFront.Common;
using WpfFront.WMSBusinessService;
using System.Data;

namespace WpfFront.Models
{
    public interface IHistoricoFacturacionModel
    {

        IList<MMaster> AerolineaList { get; set; }
        Facturas RecordFacturas { get; set; }
        IList<Facturas> FacturasList { get; set; }
        IList<Servicios> RecordListAerodromo { get; set; }
        IList<Servicios> RecordListAerodromoAgrupada { get; set; }
        IList<MMaster> ListaTipoFactura { get; set; }
        IList<Servicios> RecordServiciosAgrupadosList { get; set; }
        IList<Servicios> RecordServiciosDetalladoList { get; set; }

    }

    public class HistoricoFacturacionModel : BusinessEntityBase, IHistoricoFacturacionModel
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

        private IList<MMaster> _ListaTipoFactura;
        public IList<MMaster> ListaTipoFactura
        {
            get { return _ListaTipoFactura; }
            set
            {
                _ListaTipoFactura = value;
                OnPropertyChanged("ListaTipoFactura");
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

        private IList<MMaster> _AerolineaList;
        public IList<MMaster> AerolineaList
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