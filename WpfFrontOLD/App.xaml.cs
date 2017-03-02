using System.Windows;
using System;
using System.Collections.Generic;
using WpfFront.Common;
using Assergs.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Linq;
using System.Collections;
using WpfFront.Model;

namespace WpfFront
{
    public partial class App : Application
    {
        public static Usuario curUser;
        public static Compañia curCompany;
        public static IList<Menu> curMenuOptions;
        public static Type curPresenter;
        public static Dictionary<string, string> configOptions;
        public static string curAuthUser;
        public static string langCode;
        public static string currentLocation;
        public static Hashtable BinDirectionList;
        public static string curVersion;
        public static bool showingReports = false;

        wmsEntities db = new wmsEntities();

        public App()
        {
            this.DispatcherUnhandledException += this.App_DispatcherUnhandledException;
            this.Resources = new OfficeStyle();

            try
            {
                if (LogonActive() == true)
                    StartupContainer();
                else
                    Shutdown(1);
            }
            catch (Exception ex)
            {
                Util.ShowError(Util.GetTechMessage(ex));
                Shutdown(1);
            }
        }

        private bool? LogonActive()
        {
            Splash splash = new Splash();
            splash.DataContext = new SplashModel();
            splash.Show();

            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            LogOnScreen logon = new LogOnScreen();

//#if DEBUG
//            logon.HintVisible = true;
//#endif

            splash.Close();
            return logon.ShowDialog();
        }

        private void StartupContainer()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Splash splash = new Splash();
            splash.DataContext = new SplashModel();
            splash.Show();

            //Carga la configuracion del usuario validado
            App.curVersion = db.Configuracion.FirstOrDefault(f => f.Codigo == "VERSION").Valor;

            Application.Current.MainWindow = null;

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            splash.Close();
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (BrowserInteropHelper.IsBrowserHosted)
                throw e.Exception;

            else
            {
                Exception ex = e.Exception;
                string message = ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message = ex.Message + "\r\n\r\n" + message;
                }

                //Util.ShowError("Application: " + message);
                Util.ShowMessage(message);
                e.Handled = true;
            }

        }
    }
}