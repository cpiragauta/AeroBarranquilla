using System;
using System.Collections.Generic;
using Core.BusinessEntity;
using Core.Validation;
using WpfFront.Common;
using WpfFront.WMSBusinessService;
using System.Data;

namespace WpfFront.Models
{
    public interface IModalInfoOperacionModel
    {

        IList<Servicios> RecordListAerodromo { get; set; }


        Operacion Record { get; set; }
        Tasas RecordTasas { get; set; }
        Tasas RecordTasasAdicionales { get; set; }
        Servicios RecordServicios { get; set; }
        IList<MMaster> ListaTipoFacturacion { get; set; }
        IList<MMaster> ListaTipoOperacion { get; set; }
        IList<MMaster> ListaTipoVuelo { get; set; }
        IList<MMaster> ListaTipoPosicion { get; set; }
        IList<MMaster> ListaTipoPosicionSalida { get; set; }
        IList<MMaster> ListaPosicion { get; set; }
        IList<MMaster> ListaPosicionSalida { get; set; }
        IList<MMaster> ListaPosicionPyP { get; set; }
        IList<MMaster> ListaPosicionSalidaPyP { get; set; }
        IList<MMaster> ListaTipoDeclaracion { get; set; }
        IList<MMaster> ListaBanda { get; set; }
        IList<MMaster> ListaTipoServicio { get; set; }
        Bomberos RecordBomberos { get; set; }
        Bomberos RecordBomberosAdicional { get; set; }
        Llegada RecordLlegada { get; set; }
        Salida RecordSalida { get; set; }
        AdicionalesPyP RecordAdicionalesPyP { get; set; }
        IList<AdicionalesPyP> RegistroAdicionalesPyPList { get; set; }
        IList<Bomberos> RegistroBomberosList { get; set; }
        IList<Bomberos> RegistroBomberosAdicionalesList { get; set; }
        IList<Tasas> RegistroTasasList { get; set; }
        IList<Tasas> RegistroTasasAdicionalesList { get; set; }
        IList<MMaster> ListaTipoTasas { get; set; }
        IList<Servicios> RecordServiciosAgrupadosList { get; }
    }

    public class ModalInfoOperacionModel : BusinessEntityBase, IModalInfoOperacionModel
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


        private Tasas _RecordTasasAdicionales;
        public Tasas RecordTasasAdicionales
        {
            get { return _RecordTasasAdicionales; }
            set
            {
                _RecordTasasAdicionales = value;
                OnPropertyChanged("RecordTasasAdicionales");
            }
        }

        private IList<MMaster> _ListaTipoTasas;
        public IList<MMaster> ListaTipoTasas
        {
            get { return _ListaTipoTasas; }
            set
            {
                _ListaTipoTasas = value;
                OnPropertyChanged("ListaTipoTasas");
            }
        }

        private IList<Tasas> _RegistroTasasAdicionalesList;
        public IList<Tasas> RegistroTasasAdicionalesList
        {
            get { return _RegistroTasasAdicionalesList; }
            set
            {
                _RegistroTasasAdicionalesList = value;
                OnPropertyChanged("RegistroTasasAdicionalesList");
            }
        }

        private IList<Tasas> _RegistroTasasList;
        public IList<Tasas> RegistroTasasList
        {
            get { return _RegistroTasasList; }
            set
            {
                _RegistroTasasList = value;
                OnPropertyChanged("RegistroTasasList");
            }
        }

        private Llegada recordLlegada;
        public Llegada RecordLlegada
        {
            get { return recordLlegada; }
            set
            {
                recordLlegada = value;
                OnPropertyChanged("recordLlegada");
            }
        }

        private Salida recordSalida;
        public Salida RecordSalida
        {
            get { return recordSalida; }
            set
            {
                recordSalida = value;
                OnPropertyChanged("recordSalida");
            }
        }

        private Tasas _RecordTasas;
        public Tasas RecordTasas
        {
            get { return _RecordTasas; }
            set
            {
                _RecordTasas = value;
                OnPropertyChanged("RecordTasas");
            }
        }


        #region Facturacion
        private Servicios _RecordServicios;
        public Servicios RecordServicios
        {
            get { return _RecordServicios; }
            set
            {
                _RecordServicios = value;
                OnPropertyChanged("RecordServicios");
            }
        }
        #endregion

        #region Adicionados

        #region Bomberos
        private Bomberos recordBomberos;
        public Bomberos RecordBomberos
        {
            get { return recordBomberos; }
            set
            {
                recordBomberos = value;
                OnPropertyChanged("recordBomberos");
            }
        }

        private Bomberos _RecordBomberosAdicional;
        public Bomberos RecordBomberosAdicional
        {
            get { return _RecordBomberosAdicional; }
            set
            {
                _RecordBomberosAdicional = value;
                OnPropertyChanged("RecordBomberosAdicional");
            }
        }

        private IList<MMaster> _ListaTipoServicio;
        public IList<MMaster> ListaTipoServicio
        {
            get { return _ListaTipoServicio; }
            set
            {
                _ListaTipoServicio = value;
                OnPropertyChanged("ListaTipoServicio");
            }
        }

        private IList<Bomberos> registroBomberosList;
        public IList<Bomberos> RegistroBomberosList
        {
            get { return registroBomberosList; }
            set
            {
                registroBomberosList = value;
                OnPropertyChanged("RegistroBomberosList");
            }
        }


        private IList<Bomberos> _RegistroBomberosAdicionalesList;
        public IList<Bomberos> RegistroBomberosAdicionalesList
        {
            get { return _RegistroBomberosAdicionalesList; }
            set
            {
                _RegistroBomberosAdicionalesList = value;
                OnPropertyChanged("RegistroBomberosAdicionalesList");
            }
        }



        #endregion

        #region AdicionalesPyP
        private AdicionalesPyP recordAdicionalesPyP;
        public AdicionalesPyP RecordAdicionalesPyP
        {
            get { return recordAdicionalesPyP; }
            set
            {
                recordAdicionalesPyP = value;
                OnPropertyChanged("recordAdicionalesPyP");
            }
        }

        private IList<AdicionalesPyP> registroAdicionalesPyPList;
        public IList<AdicionalesPyP> RegistroAdicionalesPyPList
        {
            get { return registroAdicionalesPyPList; }
            set
            {
                registroAdicionalesPyPList = value;
                OnPropertyChanged("RegistroAdicionalesPyPList");
            }
        }

        #endregion

        #endregion




        private IList<MMaster> _ListaTipoFacturacion;
        public IList<MMaster> ListaTipoFacturacion
        {
            get { return _ListaTipoFacturacion; }
            set
            {
                _ListaTipoFacturacion = value;
                OnPropertyChanged("ListaTipoFacturacion");
            }
        }

        private IList<MMaster> _ListaTipoDeclaracion;
        public IList<MMaster> ListaTipoDeclaracion
        {
            get { return _ListaTipoDeclaracion; }
            set
            {
                _ListaTipoDeclaracion = value;
                OnPropertyChanged("ListaTipoDeclaracion");
            }
        }

        private IList<MMaster> _ListaTipoPosicion;
        public IList<MMaster> ListaTipoPosicion
        {
            get { return _ListaTipoPosicion; }
            set
            {
                _ListaTipoPosicion = value;
                OnPropertyChanged("ListaTipoPosicion");
            }
        }

        private IList<MMaster> _ListaTipoPosicionSalida;
        public IList<MMaster> ListaTipoPosicionSalida
        {
            get { return _ListaTipoPosicionSalida; }
            set
            {
                _ListaTipoPosicionSalida = value;
                OnPropertyChanged("ListaTipoPosicionSalida");
            }
        }

        private IList<MMaster> _ListaPosicion;
        public IList<MMaster> ListaPosicion
        {
            get { return _ListaPosicion; }
            set
            {
                _ListaPosicion = value;
                OnPropertyChanged("ListaPosicion");
            }
        }

        private IList<MMaster> _ListaPosicionSalida;
        public IList<MMaster> ListaPosicionSalida
        {
            get { return _ListaPosicionSalida; }
            set
            {
                _ListaPosicionSalida = value;
                OnPropertyChanged("ListaPosicionSalida");
            }
        }

        private IList<MMaster> _ListaPosicionPyP;
        public IList<MMaster> ListaPosicionPyP
        {
            get { return _ListaPosicionPyP; }
            set
            {
                _ListaPosicionPyP = value;
                OnPropertyChanged("ListaPosicionPyP");
            }
        }

        private IList<MMaster> _ListaPosicionSalidaPyP;
        public IList<MMaster> ListaPosicionSalidaPyP
        {
            get { return _ListaPosicionSalidaPyP; }
            set
            {
                _ListaPosicionSalidaPyP = value;
                OnPropertyChanged("ListaPosicionSalidaPyP");
            }
        }

        private IList<MMaster> _ListaTipoOperacion;
        public IList<MMaster> ListaTipoOperacion
        {
            get { return _ListaTipoOperacion; }
            set
            {
                _ListaTipoOperacion = value;
                OnPropertyChanged("ListaTipoOperacion");
            }
        }

        private IList<MMaster> _ListaTipoVuelo;
        public IList<MMaster> ListaTipoVuelo
        {
            get { return _ListaTipoVuelo; }
            set
            {
                _ListaTipoVuelo = value;
                OnPropertyChanged("ListaTipoVuelo");
            }
        }

        private IList<MMaster> _ListaBanda;
        public IList<MMaster> ListaBanda
        {
            get { return _ListaBanda; }
            set
            {
                _ListaBanda = value;
                OnPropertyChanged("ListaBanda");
            }
        }

        private Operacion _Record;
        public Operacion Record
        {
            get { return _Record; }
            set
            {
                _Record = value;
                OnPropertyChanged("Record");
            }
        }
    }
}