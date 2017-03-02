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

            container.RegisterType<IPlaneacionView, PlaneacionView>();
            container.RegisterType<IPlaneacionPresenter, PlaneacionPresenter>();
            container.RegisterType<IPlaneacionModel, PlaneacionModel>();

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