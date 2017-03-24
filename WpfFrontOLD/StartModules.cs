using WpfFront.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using WMComposite.Modularity;
using WMComposite.Regions;
using System.Collections.ObjectModel;
using Assergs.Windows;
using WpfFront.Controlador;
using WpfFront.Modelo;
using WpfFront.Vista;

namespace WpfFront
{
    public class StartModules : IModule
    {
        private readonly IUnityContainer container;
        private readonly IShellPresenter regionManager;

        public StartModules(IUnityContainer container, IShellPresenter regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
            moduleRegions = new ObservableCollection<ModuleRegion>();
        }

        public void Initialize()
        {
            Xceed.Wpf.DataGrid.Licenser.LicenseKey = "DGP31-C4MY0-XX36W-LGNA";

            RegisterViewsAndServices();
            regionManager.Shell.ShowAllContent(false);
            regionManager.Shell.Model.Modules.Add(this);
            regionManager.Shell.ShowAllContent(true);
        }

        private ObservableCollection<ModuleRegion> moduleRegions;

        public ObservableCollection<ModuleRegion> ModuleRegions
        {
            get
            {
                if (moduleRegions == null)
                    moduleRegions = new ObservableCollection<ModuleRegion>();
                return moduleRegions;
            }
        }

        protected void RegisterViewsAndServices()
        {

            container.RegisterType<IAeropuertoView, AeropuertoView>();
            container.RegisterType<IAeropuertoPresenter, AeropuertoPresenter>();
            container.RegisterType<IAeropuertoModel, AeropuertoModel>();

            container.RegisterType<IListaOperacionesView, ListaOperacionesView>();
            container.RegisterType<IListaOperacionesPresenter, ListaOperacionesPresenter>();
            container.RegisterType<IListaOperacionesModel, ListaOperacionesModel>();

            container.RegisterType<IOperacionesView, OperacionesView>();
            container.RegisterType<IOperacionesPresenter, OperacionesPresenter>();
            container.RegisterType<IOperacionesModel, OperacionesModel>();

            container.RegisterType<IPlaneacionView, PlaneacionView>();
            container.RegisterType<IPlaneacionPresenter, PlaneacionPresenter>();
            container.RegisterType<IPlaneacionModel, PlaneacionModel>();

            container.RegisterType<IAeronavesView, AeronavesView>();
            container.RegisterType<IAeronavesPresenter, AeronavesPresenter>();
            container.RegisterType<IAeronavesModel, AeronavesModel>();

            container.RegisterType<ITercerosView, TercerosView>();
            container.RegisterType<ITercerosPresenter, TercerosPresenter>();
            container.RegisterType<ITercerosModel, TercerosModel>();

            container.RegisterType<ITarifasView, TarifasView>();
            container.RegisterType<ITarifasPresenter, TarifasPresenter>();
            container.RegisterType<ITarifasModel, TarifasModel>();

            container.RegisterType<ITRMView, TRMView>();
            container.RegisterType<ITRMPresenter, TRMPresenter>();
            container.RegisterType<ITRMModel, TRMModel>();

            IList<ModuleRegion> menuOptions = Util.GetMenuOptionsV2(this);

            foreach (ModuleRegion modreg in menuOptions)
                moduleRegions.Add(modreg);
        }

        #region IModule Members

        public Object Execute(Type typePresenter, ToolWindow window)
        {
            switch (typePresenter.Name)
            {
                case "AeropuertoPresenter":
                    AeropuertoPresenter AeropuertoPresenter = container.Resolve<AeropuertoPresenter>();
                    AeropuertoPresenter.Window = window;
                    return AeropuertoPresenter.View;

                case "ListaOperacionesPresenter":
                    ListaOperacionesPresenter ListaOperacionesPresenter = container.Resolve<ListaOperacionesPresenter>();
                    ListaOperacionesPresenter.Window = window;
                    return ListaOperacionesPresenter.View;

                case "PlaneacionPresenter":
                    PlaneacionPresenter PlaneacionPresenter = container.Resolve<PlaneacionPresenter>();
                    PlaneacionPresenter.Window = window;
                    return PlaneacionPresenter.View;
                case "AeronavesPresenter":
                    AeronavesPresenter AeronavesPresenter = container.Resolve<AeronavesPresenter>();
                    AeronavesPresenter.Window = window;
                    return AeronavesPresenter.View;
                case "TercerosPresenter":
                    TercerosPresenter TercerosPresenter = container.Resolve<TercerosPresenter>();
                    TercerosPresenter.Window = window;
                    return TercerosPresenter.View;
                case "TarifasPresenter":
                    TarifasPresenter TarifasPresenter = container.Resolve<TarifasPresenter>();
                    TarifasPresenter.Window = window;
                    return TarifasPresenter.View;
                case "TRMPresenter":
                    TRMPresenter TRMPresenter = container.Resolve<TRMPresenter>();
                    TRMPresenter.Window = window;
                    return TRMPresenter.View;

            }

            return null;
        }

        #endregion

        #region IModule Members


        public object ExecuteWebUrl(Type typePresenter, ToolWindow window, string WebUrl)
        {
            return new object();
        }

        #endregion


    }
}