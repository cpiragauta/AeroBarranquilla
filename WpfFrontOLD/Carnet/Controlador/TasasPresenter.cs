using System;
using WpfFront.Modelo;
using Assergs.Windows;
using Microsoft.Practices.Unity;
using WpfFront.Vista;

namespace WpfFront.Controlador
{

    public interface ITasasPresenter
    {
        ITasasView View { get; set; }
        ToolWindow Window { get; set; }
    }

    public class TasasPresenter : ITasasPresenter
    {
        public ITasasView View { get; set; }
        private readonly IUnityContainer container;
       // private readonly WMSServiceClient service;
        public ToolWindow Window { get; set; }
        //Variables Auxiliares 
        public Object PresenterParent { get; set; }

        public TasasPresenter(IUnityContainer container, ITasasView view)
        {
            View = view;
            this.container = container;
          //  this.service = new WMSServiceClient();
            View.Model = this.container.Resolve<TasasModel>();
        }

        //private void OnCargarRangoCarnetsFiltro(object sender, DataEventArgs<MMaster> e)
        //{
        //    //Evaluo que haya sido seleccionado un registro
        //    if (e.Value != null)
        //    {
        //        if (e.Value.Code == "PERSONA")
        //        {
        //            //Cargo las opciones de carnets dependiendo del tipo de carnet solicitado
        //            View.Model.ListaTiempoValidezFiltro = service.GetMMaster(new MMaster { MetaType = new MType { Code = "RANGOPERSONAL" } }).OrderBy(f => f.NumOrder).ToList();
        //        }
        //        else if (e.Value.Code == "VEHICULO")
        //        {
        //            //Cargo las opciones de carnets dependiendo del tipo de carnet solicitado
        //            View.Model.ListaTiempoValidezFiltro = service.GetMMaster(new MMaster { MetaType = new MType { Code = "RANGOVEHICULOS" } }).OrderBy(f => f.NumOrder).ToList();
        //        }
        //    }
        //}

    }
}