using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Printing; // The PrinterSettings class is located inside the Printing namespace
using System.Management;
using System.Reflection;
using System.Collections.Specialized;
using System.DirectoryServices;
using System.Runtime.Remoting.Messaging;
using System.Windows.Media.Imaging;
using Assergs.Windows;
using System.Collections;
using System.Linq;
using WMComposite.Modularity;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;
using Xceed.Wpf.DataGrid.Settings;
using WMComposite.Regions;
using System.Windows.Controls;
using System.Data;
using System.Media;
using WpfFront.Model;

namespace WpfFront.Common
{
    public partial class Util
    {
        

        public static void ShowError(string customMsg)
        {
            //MessageBox.Show(customMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            UtilWindow.ShowError(customMsg);
            SystemSounds.Hand.Play();
        }

        public static void ShowMessage(string msg)
        {
            //MessageBox.Show(msg, "Process Done", MessageBoxButton.OK, MessageBoxImage.Information);
            UtilWindow.ShowMessage(msg);
            SystemSounds.Asterisk.Play();
        }


        public static string CryptPasswd(string data, string cryptKey)
        {
            if (string.IsNullOrEmpty(data))
                return "";


            Crypto cpt = new Crypto(Crypto.SymmProvEnum.DES);
            return cpt.Encrypting(data, cryptKey);

        }


        public static string DeCryptPasswd(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            Crypto cpt = new Crypto(Crypto.SymmProvEnum.DES);
            return cpt.Decrypting(value, key);

        }

        public static string GetPlainTextString(Stream stream)
        {
            int streamLength = Convert.ToInt32(stream.Length);
            byte[] file = new byte[streamLength];
            stream.Read(file, 0, streamLength);
            stream.Close();
            return Encoding.ASCII.GetString(file);
        }

        //Metodo que apoya a ToShowData para obtener los dato de un objeto usando Reflection
        private static string GetValue(PropertyInfo property, object obj, int level)
        {
            try
            {
                if (level == 0)
                {
                    string fType = property.PropertyType.Name;

                    if (fType == "String" || fType.Contains("Decimal") || fType.Contains("Double") || fType.Contains("Nullable") || fType.Contains("Int") || fType.Contains("DateTime") || fType.Contains("Bool"))
                        return property.GetValue(obj, null).ToString();
                    else
                        return GetValue(property, property.GetValue(obj, null), 1);
                }
                else
                {
                    return obj.GetType().GetProperty("Name").GetValue(obj, null).ToString();
                }
            }
            catch { }

            return "";

        }

        // Devuelve la cadena enviada con un formato especial, según el tipo.
        public static string FormatValue(string type, string value)
        {
            string result = value;
            if (type.Contains("Date"))
                result = DateTime.Parse(value).ToShortDateString();
            else
                if (type.Contains("Int"))
                    result = int.Parse(value).ToString("###,###,###");
                else
                    if (type.Contains("Double"))
                        result = Double.Parse(value).ToString("###,###,###.##");

            return result;
        }

        public static byte[] GetImageByte(Stream stream)
        {
            int streamLength = Convert.ToInt32(stream.Length);
            byte[] image = new byte[streamLength];
            stream.Read(image, 0, streamLength);
            stream.Close();
            return image;
        }

        //public static void LoadServiceMasters()
        //{
        //    WMSServiceClient db = new WMSServiceClient();

        //    //Status
        //    App.DocStatusList = db.GetStatus(new Status());
        //    App.EntityStatusList = App.DocStatusList.Where(f => f.StatusType.StatusTypeID == SStatusType.Active).ToList();
        //    App.DocStatusList = App.DocStatusList.Where(f => f.StatusType.StatusTypeID == SStatusType.Document).ToList();

        //    //Locations
        //    App.LocationList = db.GetLocation(new Location { Status = new Status { StatusID = EntityStatus.Active } })
        //        .OrderBy(f => f.Name).ToList();

        //    //Companies
        //    App.CompanyList = db.GetCompany(new Company());

        //    //Bin Directions
        //    Hashtable binDirections = new Hashtable();
        //    binDirections.Add(2, "Out only");
        //    binDirections.Add(1, "In only");
        //    binDirections.Add(0, "In/Out");
        //    App.BinDirectionList = binDirections;

        //    //Connection Printers
        //    App.PrinterConnectionList = db.GetConnection(new Connection { ConnectionType = new ConnectionType { RowID = CnnType.Printer } });

        //}

        public static string GetTechMessage(Exception ex)
        {
            if (ex == null)
                return "";

            string msg = ex.Message;
            Exception tmpEx = ex.InnerException;

            while (tmpEx != null)
            {
                msg += "\n" + tmpEx.Message;
                tmpEx = tmpEx.InnerException;
            }

            return msg;
        }

        internal static IList<ModuleRegion> GetMenuOptionsV2(StartModules module)
        {
            IList<ModuleRegion> menuList = new List<ModuleRegion>();
            IList<MenuRol> optionsList = null;
            wmsEntities db = new wmsEntities();

            //Crea el menu principal Con Los tipos de menu que debe Haber

            IList<Tipo> typeList = db.Tipo.Where(f => f.Agrupacion.Codigo == "MENU").ToList();

            Rol rolAdmin = db.Rol.FirstOrDefault(f => f.Codigo == "ADMINISTRADOR"); 

            //Si es admin tiene permiso a todas las funcionalidades
            if (App.curUser.Rol.RowID == rolAdmin.RowID)
            {
                //Guarda las opciones del menu en la session para reuso
                App.curMenuOptions = db.Menu.Where(f => f.Activo == true).ToList();
            }
            else
            {
                optionsList = db.MenuRol.Where(f => f.RolID == App.curUser.RolID && f.Activo == true).OrderBy(f => f.Menu.Orden).ToList();

                //Guarda las opciones del menu en la session para reuso
                App.curMenuOptions = optionsList.Select(f => f.Menu).ToList();
            }

            ModuleRegion menuChild = null;

            foreach (Tipo mType in typeList)
            {

                menuChild = new ModuleRegion();
                menuChild.Name = mType.Nombre;

                if (App.curUser.Rol.RowID == rolAdmin.RowID)    
                {
                    // recorremos lista de opciones del rol, para organizar los menus
                    foreach (Model.Menu mOption in App.curMenuOptions.Where(f => f.Tipo.RowID == mType.RowID).OrderBy(f => f.Orden))
                    {
                        menuChild.Options.Add(new ModuleSubmenu
                        {
                            Name = mOption.Nombre,
                            Module = module,
                            Image = GetImage("/WpfFront;component/Images/Icons/48x48/" + mOption.Imagen),
                            PresenterType = Type.GetType("WpfFront.Controlador." + mOption.Ruta + ""),
                            IconPath = "/WpfFront;component/Images/Icons/48x48/" + mOption.Imagen
                        });

                    }
                }
                else
                {
                    // recorremos lista de opciones del rol, para organizar los menus
                    foreach (MenuRol mOption in optionsList.Where(f => f.Menu.Tipo.RowID == mType.RowID).OrderBy(f => f.Menu.Orden))
                    {
                        menuChild.Options.Add(new ModuleSubmenu
                        {
                            Name = mOption.Menu.Nombre,
                            Module = module,
                            Image = GetImage("/WpfFront;component/Images/Icons/48x48/" + mOption.Menu.Imagen),
                            PresenterType = Type.GetType("WpfFront.Controlador." + mOption.Menu.Ruta + ""),
                            IconPath = "/WpfFront;component/Images/Icons/48x48/" + mOption.Menu.Imagen
                        });

                    }
                }

                menuList.Add(menuChild);
            }

            return menuList;
        }

        public static Byte[] GetImage(String suri)
        {
            Uri uri = null;
            try { uri = new Uri(suri, UriKind.Relative); }
            catch { uri = new Uri(WmsSetupValues.IconPath48 + WmsSetupValues.DefaultMenuIcon, UriKind.Relative); }


            Stream ms = Application.GetResourceStream(uri).Stream;
            Byte[] image = new Byte[ms.Length];
            ms.Read(image, 0, Convert.ToInt32(ms.Length - 1));
            return image;
        }

        public static ImageSource GetImageSource(byte[] imageByte)
        {

            BitmapImage img = new BitmapImage();

            using (MemoryStream stream = new MemoryStream(imageByte))
                img.StreamSource = stream;

            return img;
        }

        public static string XmlSerializer(Object obj)
        {
            MemoryStream myMemStream = new MemoryStream();
            XmlSerializer mySerializer = new XmlSerializer(obj.GetType());
            mySerializer.Serialize(myMemStream, obj);
            myMemStream.Position = 0;

            // Load the serialized eConnect document object into an XML document object
            XmlTextReader xmlreader = new XmlTextReader(myMemStream);
            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.Load(xmlreader);
            return myXmlDocument.OuterXml;
        }



        public static SettingsRepository XmlDeSerializer(string objString)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(objString));
            XmlSerializer mySerializer = new XmlSerializer(typeof(SettingsRepository));
            SettingsRepository obj = mySerializer.Deserialize(ms) as SettingsRepository;
            return obj;

        }

        public static string GetConfigOption(string key)
        {
            try { return App.configOptions[key]; }
            catch { Util.ShowError("Option " + key + " not defined."); }
            return "";
        }

        public static InternalWindow GetInternalWindow(Panel parent, string wName)
        {
            InternalWindow window = new InternalWindow();
            window.Parent = parent;
            window.CanResize = true;
            window.ShowStatusBar = false;
            window.Header = wName;
            window.StartPosition = ToolWindowStartPosition.CenterParent;
            window.Height = SystemParameters.FullPrimaryScreenHeight - 150;
            window.Width = SystemParameters.FullPrimaryScreenWidth - 10;
            return window;
        }

        public static void WriteEventLog(string techError)
        {
            try
            {
                string sSource;
                string sLog;
                string sEvent;

                sSource = "WMS 3.0 Server";
                sLog = "WpfFront";
                sEvent = techError;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                //EventLog.WriteEntry(sSource, sEvent);
                EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Error, 301);
            }
            catch { }
        }

        internal static string ExtractFileName(string file)
        {
            try
            {
                string[] fname = file.Split("\\".ToCharArray());
                return fname[fname.Length - 1];
            }
            catch { return ""; }
        }

        //Return a dataset from a XML string  document
        public static DataSet GetDataSet(string xmlData)
        {
            XmlDocument myXmlOut = new XmlDocument();
            myXmlOut.LoadXml(xmlData);

            // convert to dataset in two lines
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlNodeReader(myXmlOut));

            return ds;
        }

        internal static bool AllowOption(string option)
        {
            return App.curMenuOptions.Any(f => f.Nombre == option);
        }
    }
}