using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Model;


namespace WpfFront.Modelo
{
    public interface IListaCarnetsModel
    {
    }

    public class ListaCarnetsModel : BusinessEntityBase, IListaCarnetsModel
    {

        #region General

        private IList<Solicitud> entitylist;
        public IList<Solicitud> EntityList
        {
            get { return entitylist; }
            set
            {
                entitylist = value;
                OnPropertyChanged("EntityList");
            }
        }

        #endregion

    }
}