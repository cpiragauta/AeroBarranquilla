using System;
using WpfFront.Vista;
using Assergs.Windows;
using Microsoft.Practices.Unity;
using WpfFront.Modelo;
using WpfFront.Model;

namespace WpfFront.Controlador
{
    public interface IDatosCarnetsPresenter
    {
        IDatosCarnetsView View { get; set; }
        ToolWindow Window { get; set; }
        void CargarCarnet(Encabezado Documento, Object Presenter);
    }

    public class DatosCarnetsPresenter : IDatosCarnetsPresenter
    {
        public IDatosCarnetsView View { get; set; }
        private readonly IUnityContainer container;
      //  private readonly WMSServiceClient service;
        public ToolWindow Window { get; set; }

        //Variable que maneja los datos del Presenter Parent
        public Object PresenterParent { get; set; }

        public DatosCarnetsPresenter(IUnityContainer container, IDatosCarnetsView view)
        {
            View = view;
            this.container = container;
          //  this.service = new WMSServiceClient();
            View.Model = this.container.Resolve<DatosCarnetsModel>();
        }

        public void CargarCarnet(Encabezado Documento, Object Presenter)
        {
            if (Presenter != null)
            {
                //Asigno el PresenterParente
                PresenterParent = Presenter;
                View.PresenterParent = Presenter;
            }   

            if (Documento.RowID != 0)
            {
                View.Model.RecordEncabezado = Documento;
            }
            else
            {
                View.Model.RecordEncabezado = new Encabezado();
            }
        }

    }
}