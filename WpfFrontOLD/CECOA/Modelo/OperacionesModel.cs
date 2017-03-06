using System.Collections.Generic;
using Core.BusinessEntity;
using System.Linq;
using WpfFront.Model;

namespace WpfFront.Modelo
{
    public interface IOperacionesModel
    {
        #region Declaracion
        Operacion Record { get; set; }
        Tasas RecordTasas { get; set; }
        Tasas RecordTasasAdicionales { get; set; }
        Servicios RecordServicios { get; set; }
        IList<Tipo> ListaTipoFacturacion { get; set; }
        IList<Tipo> ListaTipoOperacion { get; set; }
        IList<Tipo> ListaTipoVuelo { get; set; }
        IList<Tipo> ListaTipoPosicion { get; set; }
        IList<Tipo> ListaTipoPosicionSalida { get; set; }
        IList<Tipo> ListaPosicion { get; set; }
        IList<Tipo> ListaPosicionSalida { get; set; }
        IList<Tipo> ListaPosicionPyP { get; set; }
        IList<Tipo> ListaPosicionSalidaPyP { get; set; }
        IList<Tipo> ListaTipoDeclaracion { get; set; }
        IList<Tipo> ListaBanda { get; set; }
        IList<Tipo> ListaTipoServicio { get; set; }
        Bombero RecordBomberos { get; set; }
        Bombero RecordBomberosAdicional { get; set; }
        Llegada RecordLlegada { get; set; }
        Salida RecordSalida { get; set; }
        IList<Bombero> RegistroBomberosList { get; set; }
        IList<Bombero> RegistroBomberosAdicionalesList { get; set; }
        IList<Tasas> RegistroTasasList { get; set; }
        IList<Tasas> RegistroTasasAdicionalesList { get; set; }
        IList<Tipo> ListaTipoTasas { get; set; }
        IList<Servicios> RecordServiciosAgrupadosList { get; }
        #region Status
        Estado StatusFacturaEnviarERP { get; }
        Estado StatusServiciosParaFacturar { get; }
        Estado StatusServiciosEnviarERP { get; }
        Estado StatusServiciosAnulada { get; }
        Estado StatusFacturaAnulada { get; }
        Estado StatusLlegadaSalidaGuardada { get; }
        Estado StatusLlegadaSalidaConfirmada { get; }
        Estado StatusOperacionLiquidada { get; }
        Estado StatusOperacionConfirmada { get; }
        Estado StatusOperacionFacturada { get; }

        Estado StatusBomberosNuevo { get; }
        Estado StatusBomberosParaEnviarERP { get; }
        Estado StatusBomberosAnulado { get; }
        Estado StatusBomberosParaFacturar { get; }

        Estado StatusAdicionalesGuardado { get; }
        Estado StatusAdicionalesConfirmado { get; }
        Estado StatusAdicionalesParaFacturar { get; }
        Estado StatusAdicionalesParaEnviarERP { get; }
        Estado StatusAdicionalesFacturado { get; }
        Estado StatusAdicionalesAnulado { get; }

        Estado StatusTasasNueva { get; }
        Estado StatusTasasParaFacturar { get; }
        Estado StatusTasasParaEnviarERP { get; }
        Estado StatusTasasFacturada { get; }
        Estado StatusTasasAnulada { get; }
        #endregion





        #endregion
    }

    public class OperacionesModel : BusinessEntityBase, IOperacionesModel
    {

        private static readonly wmsEntities service = new wmsEntities();


        #region Status

        public static readonly Estado _StatusFacturaEnviarERP = service.Estado.FirstOrDefault(f => f.Nombre == "ParaEnviarERP" && f.Tipo.Nombre == "FacturaServicios");

        public Estado StatusFacturaEnviarERP
        {
            get { return _StatusFacturaEnviarERP; }
        }

        public static readonly Estado _StatusFacturaAnulada = service.Estado.FirstOrDefault(f => f.Nombre == "Anulada" && f.Tipo.Nombre == "FacturaServicios");
        public Estado StatusFacturaAnulada
        {
            get { return _StatusFacturaAnulada; }
        }


        public static readonly Estado _StatusServiciosParaFacturar = service.Estado.FirstOrDefault(f => f.Nombre == "ParaFacturar" && f.Tipo.Nombre == "Servicios");
        public Estado StatusServiciosParaFacturar
        {
            get { return _StatusServiciosParaFacturar; }
        }

        public static readonly Estado _StatusServiciosEnviarERP = service.Estado.FirstOrDefault(f => f.Nombre == "ParaEnviarERP" && f.Tipo.Nombre == "Servicios");
        public Estado StatusServiciosEnviarERP
        {
            get { return _StatusServiciosEnviarERP; }
        }

        public static readonly Estado _StatusServiciosAnulada = service.Estado.FirstOrDefault(f => f.Nombre == "Anulada" && f.Tipo.Nombre == "Servicios");
        public Estado StatusServiciosAnulada
        {
            get { return _StatusServiciosAnulada; }
        }

        public static readonly Estado _StatusLlegadaSalidaGuardada = service.Estado.FirstOrDefault(f => f.Nombre == "Guardada" && f.Tipo.Nombre == "LlegadaSalidaCecoa");
        public Estado StatusLlegadaSalidaGuardada
        {
            get { return _StatusLlegadaSalidaGuardada; }
        }

        private static readonly Estado _StatusLlegadaSalidaConfirmada = service.Estado.FirstOrDefault(f => f.Nombre == "Confirmada" && f.Tipo.Nombre == "LlegadaSalidaCecoa");
        public Estado StatusLlegadaSalidaConfirmada
        {
            get { return _StatusLlegadaSalidaConfirmada; }
        }

        public static readonly Estado _StatusOperacionLiquidada = service.Estado.FirstOrDefault(f => f.Nombre == "Liquidada" && f.Tipo.Nombre == "OperacionCecoa");
        public Estado StatusOperacionLiquidada
        {
            get { return _StatusOperacionLiquidada; }
        }

        public static readonly Estado _StatusOperacionConfirmada = service.Estado.FirstOrDefault(f => f.Nombre == "Confirmada" && f.Tipo.Nombre == "OperacionCecoa");
        public Estado StatusOperacionConfirmada
        {
            get { return _StatusOperacionConfirmada; }
        }

        public static readonly Estado _StatusOperacionFacturada = service.Estado.FirstOrDefault(f => f.Nombre == "Facturada" && f.Tipo.Nombre == "OperacionCecoa");
        public Estado StatusOperacionFacturada
        {
            get { return _StatusOperacionFacturada; }
        }

        public static readonly Estado _StatusBomberosNuevo = service.Estado.FirstOrDefault(f => f.Nombre == "Nuevo" && f.Tipo.Nombre == "ServicioBomberos");
        public Estado StatusBomberosNuevo
        {
            get { return _StatusBomberosNuevo; }
        }

        public static readonly Estado _StatusBomberosParaEnviarERP = service.Estado.FirstOrDefault(f => f.Nombre == "ParaEnviarERP" && f.Tipo.Nombre == "ServicioBomberos");
        public Estado StatusBomberosParaEnviarERP
        {
            get { return _StatusBomberosParaEnviarERP; }
        }

        public static readonly Estado _StatusBomberosAnulado = service.Estado.FirstOrDefault(f => f.Nombre == "Anulado" && f.Tipo.Nombre == "ServicioBomberos");
        public Estado StatusBomberosAnulado
        {
            get { return _StatusBomberosAnulado; }
        }

        public static readonly Estado _StatusBomberosParaFacturar = service.Estado.FirstOrDefault(f => f.Nombre == "ParaFacturar" && f.Tipo.Nombre == "ServicioBomberos");
        public Estado StatusBomberosParaFacturar
        {
            get { return _StatusBomberosParaFacturar; }
        }

        #endregion

        #region Puentes Parqueo Adicionales

        public static readonly Estado _StatusAdicionalesGuardado = service.Estado.FirstOrDefault(f => f.Nombre == "Guardado" && f.Tipo.Nombre == "Adicionales");
        public Estado StatusAdicionalesGuardado
        {
            get { return _StatusAdicionalesGuardado; }
        }

        public static readonly Estado _StatusAdicionalesConfirmado = service.Estado.FirstOrDefault(f => f.Nombre == "Confirmado" && f.Tipo.Nombre == "Adicionales");
        public Estado StatusAdicionalesConfirmado
        {
            get { return _StatusAdicionalesConfirmado; }
        }

        public static readonly Estado _StatusAdicionalesParaFacturar = service.Estado.FirstOrDefault(f => f.Nombre == "ParaFacturar" && f.Tipo.Nombre == "Adicionales");
        public Estado StatusAdicionalesParaFacturar
        {
            get { return _StatusAdicionalesParaFacturar; }
        }

        public static readonly Estado _StatusAdicionalesParaEnviarERP = service.Estado.FirstOrDefault(f => f.Nombre == "ParaEnviarERP" && f.Tipo.Nombre == "Adicionales");
        public Estado StatusAdicionalesParaEnviarERP
        {
            get { return _StatusAdicionalesParaEnviarERP; }
        }

        public static readonly Estado _StatusAdicionalesFacturado = service.Estado.FirstOrDefault(f => f.Nombre == "Facturado" && f.Tipo.Nombre == "Adicionales");
        public Estado StatusAdicionalesFacturado
        {
            get { return _StatusAdicionalesFacturado; }
        }

        public static readonly Estado _StatusAdicionalesAnulado = service.Estado.FirstOrDefault(f => f.Nombre == "Anulado" && f.Tipo.Nombre == "Adicionales");
        public Estado StatusAdicionalesAnulado
        {
            get { return _StatusAdicionalesAnulado; }
        }


        #endregion

        #region Tasas

        public static readonly Estado _StatusTasasNueva = service.Estado.FirstOrDefault(f => f.Nombre == "Nueva" && f.Tipo.Nombre == "Tasas");
        public Estado StatusTasasNueva
        {
            get { return _StatusTasasNueva; }
        }

        public static readonly Estado _StatusTasasParaFacturar = service.Estado.FirstOrDefault(f => f.Nombre == "ParaFacturar" && f.Tipo.Nombre == "Tasas");
        public Estado StatusTasasParaFacturar
        {
            get { return _StatusTasasParaFacturar; }
        }

        public static readonly Estado _StatusTasasParaEnviarERP = service.Estado.FirstOrDefault(f => f.Nombre == "ParaEnviarERP" && f.Tipo.Nombre == "Tasas");
        public Estado StatusTasasParaEnviarERP
        {
            get { return _StatusTasasParaEnviarERP; }
        }

        public static readonly Estado _StatusTasasAnulada = service.Estado.FirstOrDefault(f => f.Nombre == "Anulada" && f.Tipo.Nombre == "Tasas");
        public Estado StatusTasasAnulada
        {
            get { return _StatusTasasAnulada; }
        }

        public static readonly Estado _StatusTasasFacturada = service.Estado.FirstOrDefault(f => f.Nombre == "Facturada" && f.Tipo.Nombre == "Tasas");
        public Estado StatusTasasFacturada
        {
            get { return _StatusTasasFacturada; }
        }

        #endregion


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

        private IList<Tipo> _ListaTipoTasas;
        public IList<Tipo> ListaTipoTasas
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
        private Bombero recordBomberos;
        public Bombero RecordBomberos
        {
            get { return recordBomberos; }
            set
            {
                recordBomberos = value;
                OnPropertyChanged("recordBomberos");
            }
        }

        private Bombero _RecordBomberosAdicional;
        public Bombero RecordBomberosAdicional
        {
            get { return _RecordBomberosAdicional; }
            set
            {
                _RecordBomberosAdicional = value;
                OnPropertyChanged("RecordBomberosAdicional");
            }
        }

        private IList<Tipo> _ListaTipoServicio;
        public IList<Tipo> ListaTipoServicio
        {
            get { return _ListaTipoServicio; }
            set
            {
                _ListaTipoServicio = value;
                OnPropertyChanged("ListaTipoServicio");
            }
        }

        private IList<Bombero> registroBomberosList;
        public IList<Bombero> RegistroBomberosList
        {
            get { return registroBomberosList; }
            set
            {
                registroBomberosList = value;
                OnPropertyChanged("RegistroBomberosList");
            }
        }


        private IList<Bombero> _RegistroBomberosAdicionalesList;
        public IList<Bombero> RegistroBomberosAdicionalesList
        {
            get { return _RegistroBomberosAdicionalesList; }
            set
            {
                _RegistroBomberosAdicionalesList = value;
                OnPropertyChanged("RegistroBomberosAdicionalesList");
            }
        }



        #endregion

        #endregion




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

        private IList<Tipo> _ListaTipoDeclaracion;
        public IList<Tipo> ListaTipoDeclaracion
        {
            get { return _ListaTipoDeclaracion; }
            set
            {
                _ListaTipoDeclaracion = value;
                OnPropertyChanged("ListaTipoDeclaracion");
            }
        }

        private IList<Tipo> _ListaTipoPosicion;
        public IList<Tipo> ListaTipoPosicion
        {
            get { return _ListaTipoPosicion; }
            set
            {
                _ListaTipoPosicion = value;
                OnPropertyChanged("ListaTipoPosicion");
            }
        }

        private IList<Tipo> _ListaTipoPosicionSalida;
        public IList<Tipo> ListaTipoPosicionSalida
        {
            get { return _ListaTipoPosicionSalida; }
            set
            {
                _ListaTipoPosicionSalida = value;
                OnPropertyChanged("ListaTipoPosicionSalida");
            }
        }

        private IList<Tipo> _ListaPosicion;
        public IList<Tipo> ListaPosicion
        {
            get { return _ListaPosicion; }
            set
            {
                _ListaPosicion = value;
                OnPropertyChanged("ListaPosicion");
            }
        }

        private IList<Tipo> _ListaPosicionSalida;
        public IList<Tipo> ListaPosicionSalida
        {
            get { return _ListaPosicionSalida; }
            set
            {
                _ListaPosicionSalida = value;
                OnPropertyChanged("ListaPosicionSalida");
            }
        }

        private IList<Tipo> _ListaPosicionPyP;
        public IList<Tipo> ListaPosicionPyP
        {
            get { return _ListaPosicionPyP; }
            set
            {
                _ListaPosicionPyP = value;
                OnPropertyChanged("ListaPosicionPyP");
            }
        }

        private IList<Tipo> _ListaPosicionSalidaPyP;
        public IList<Tipo> ListaPosicionSalidaPyP
        {
            get { return _ListaPosicionSalidaPyP; }
            set
            {
                _ListaPosicionSalidaPyP = value;
                OnPropertyChanged("ListaPosicionSalidaPyP");
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

        private IList<Tipo> _ListaTipoVuelo;
        public IList<Tipo> ListaTipoVuelo
        {
            get { return _ListaTipoVuelo; }
            set
            {
                _ListaTipoVuelo = value;
                OnPropertyChanged("ListaTipoVuelo");
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
