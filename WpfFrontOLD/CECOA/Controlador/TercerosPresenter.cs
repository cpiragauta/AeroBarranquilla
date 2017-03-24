
using Assergs.Windows;
using Microsoft.Practices.Unity;
using WpfFront.Modelo;
using WpfFront.Model;
using WpfFront.Vista;

namespace WpfFront.Controlador
{
    public interface ITercerosPresenter
    {
        ITercerosView View { get; set; }
        ToolWindow Window { get; set; }
    }

    public class TercerosPresenter : ITercerosPresenter
    {
        public ITercerosView View { get; set; }
        private readonly IUnityContainer container;
       // private readonly WMSServiceClient service;
        public ToolWindow Window { get; set; }

        public TercerosPresenter(IUnityContainer container, ITercerosView view)
        {
            View = view;
            this.container = container;
          //  this.service = new WMSServiceClient();
            View.Model = this.container.Resolve<TercerosModel>();

        }
    }
}