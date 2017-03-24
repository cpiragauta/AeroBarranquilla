using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Model;
using WpfFront.Vista;
using WpfFront.Controlador;


namespace WpfFront.Modelo
{
    public interface IAeronavesModel
    {
        //IList<MMaster> ListaTipoFactura { get; set; }
    }

    public class AeronavesModel : BusinessEntityBase, IAeronavesModel
    {
        private IList<Aeronave> _ListadoAeronaves;
        public IList<Aeronave> ListadoAeronaves
        {
            get { return _ListadoAeronaves; }
            set
            {
                _ListadoAeronaves = value;
                OnPropertyChanged("ListadoAeronaves");
            }
        }

        private Aeronave _RecordAeronaves;
        public Aeronave RecordAeronaves
        {
            get { return _RecordAeronaves; }
            set
            {
                _RecordAeronaves = value;
                OnPropertyChanged("RecordAeronaves");
            }
        }

       

        private IList<Tipo> _TipoOperacion;
        public IList<Tipo> TipoOperacion
        {
            get { return _TipoOperacion; }
            set
            {
                _TipoOperacion = value;
                OnPropertyChanged("TipoOperacion");
            }
        }

    }
}