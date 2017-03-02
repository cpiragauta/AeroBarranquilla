using Microsoft.Practices.Unity;
using WMComposite.Events;
using WMComposite.Modularity;
using WMComposite.Regions;
using WpfFront.Common;

namespace WpfFront
{
    public class ShellPresenter : IShellPresenter
    {
        private readonly IUnityContainer container;
        private PopupWindow reportW;
        string curLook = "";
        InternalWindow window;

        public ShellPresenter(IUnityContainer container, IShellView shell)
        {
            this.container = container;
            Shell = shell;
            Shell.Model = this.container.Resolve<ShellPresenterModel>();
            Shell.ShowReports += new System.EventHandler<System.EventArgs>(Shell_ShowReports);
            //Shell.LoadInquiry += new System.EventHandler<DataEventArgs<string>>(Shell_LoadInquiry);

            //Menu = menu;
            //Menu.Model = this.container.Resolve<MainMenuPresenterModel>();
            //Menu.OpenModule += new EventHandler<DataEventArgs<ModuleRegion>>(this.OnOpenModule);

            window = new InternalWindow();

        }

        void Shell_ShowReports(object sender, System.EventArgs e)
        {
            //IReportPresenter presenter = container.Resolve<IReportPresenter>();

            //if (reportW != null)
            //{
            //    try { reportW.Close(); }
            //    catch { }
            //}
            

            //reportW = new PopupWindow();
            //reportW.Closing += new System.ComponentModel.CancelEventHandler(reportW_Closing);
            //reportW.ShowViewInShell(presenter.View, "WMS Express Reports");
            //reportW.WindowState = System.Windows.WindowState.Maximized;
            //reportW.Show();
        }


        void reportW_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public IShellView Shell { get; set; }

        //public IMainMenuView Menu { get; set; }

        public void ShowMenu()
        {
            //Shell.ShowViewInShell(Menu);
        }

        private void OnOpenModule(object sender, DataEventArgs<ModuleRegion> e)
        {
            //ShellWindow sw = new ShellWindow();
            //sw.DataContext = e.Value;
            //sw.Show();
        }
    }
}
