using System.Collections.Generic;
using Core.BusinessEntity;
using WpfFront.Model;
   

namespace WpfFront.Modelo
{
    public interface ITRMModel
    {
        TRM TRM { get; set; }
        IList<TRM> ListTRM { get; set; }
    }

    public class TRMModel : BusinessEntityBase, ITRMModel
    {
        private IList<TRM> _ListTRM;
        public IList<TRM> ListTRM
        {
            get { return _ListTRM; }
            set
            {
                _ListTRM = value;
                OnPropertyChanged("ListTRM");
            }
        }




        private TRM _TRM;
        public TRM TRM
        {
            get { return _TRM; }
            set
            {
                _TRM = value;
                OnPropertyChanged("TRM");
            }
        }
    }
}