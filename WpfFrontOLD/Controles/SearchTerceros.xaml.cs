using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using WpfFront.Model;


namespace WpfFront.Controles
{
    /// <summary>
    /// Interaction logic for SearchTerceros.xaml
    /// </summary>
    public partial class SearchTerceros : UserControl, INotifyPropertyChanged
    {
        public event EventHandler OnSelected;

        public SearchTerceros()
        {
            InitializeComponent();
            DataContext = this;
        }

        //Image envent    
        protected void imgLoad_FocusHandler(object sender, EventArgs e)
        {
            EventHandler temp = OnSelected;
            if (temp != null)
                temp(sender, e);
            txtData.Focus();
        }
        
        //Text Property
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String),
            typeof(SearchTerceros));

        public String Text
        {
            get { return this.txtData.Text; } //(String)GetValue(TextProperty);
            set { this.txtData.Text = value; }
        }

        private IList<Tercero> _DataList;
        public IList<Tercero> DataList
        {
            get { return _DataList; }
            set
            {
                _DataList = value;
                OnPropertyChanged("DataList");
            }
        }

        private IList<Tercero> _DefList;
        public IList<Tercero> DefaultList
        {
            get { return _DefList; }
            set { _DefList = value; }
        }

        public static DependencyProperty TercerosProperty = DependencyProperty.Register("Tercero", typeof(Tercero), typeof(SearchTerceros));

        public Tercero Terceros
        {
            get { return (Tercero)GetValue(TercerosProperty); }
            set
            {
                SetValue(TercerosProperty, value);
                this.cboData.Visibility = Visibility.Collapsed;
                txtDescripcion.Visibility = Visibility.Visible;
                if (Terceros!= null)
                {
                    if (!string.IsNullOrEmpty(Terceros.Nombre + Terceros.Apellidos))
                    {
                        txtData.Text = Terceros.Nombre + " " + Terceros.Apellidos;
                        txtDescripcion.Text = Terceros.Nombre + " " + Terceros.Apellidos;
                    }
                    else
                    {
                        txtDescripcion.Text = "";
                    }
                }
            }
        }

        #region INotifyPropertyChanged Members

        private event PropertyChangedEventHandler propertyChangedEvent;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { propertyChangedEvent += value; }
            remove { propertyChangedEvent -= value; }
        }

        protected void OnPropertyChanged(string prop)
        {
            if (propertyChangedEvent != null)
                propertyChangedEvent(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        //private void LoadDataList()
        //{
        //    if (!String.IsNullOrEmpty(txtData.Text))
        //    {
        //        DataList = DefaultList;
        //        //Obtengo solamente los tipo cliente(Cecoa)
        //        DataList = (new WMSServiceClient()).GetTerceros(new Terceros { Nombre = txtData.Text/*, Cliente=1, Empleado=0*/});
        //        return;
        //    }
        //    DataList = (new WMSServiceClient()).GetTerceros(new Terceros { /*Cliente = 1, Empleado = 0*/ });
        //}


        private void LoadDataList()
        {
            if (!String.IsNullOrEmpty(txtData.Text))
            {
                DataList = DefaultList;
                DataList = (new wmsEntities()).Tercero.Where(f => f.Nombre.ToUpper().Contains(txtData.Text.ToUpper())).Take(15).ToList();
                return;
            }
            DataList = (new wmsEntities()).Tercero.ToList();
        }

        //private void imgLook_Click(object sender, RoutedEventArgs e)
        //{
        //    //Search for a Records
        //    DataList = (new WMSServiceClient()).GetTerceros(new Terceros { /*Cliente = 1, Empleado = 0*/ });

        //    //Cargar la lista de Records
        //    this.cboData.Visibility = Visibility.Visible;
        //    this.cboData.IsDropDownOpen = true;

        //    if (DataList.Count == 1)
        //    {
        //        this.Terceros = DataList[0];
        //        imgLoad.Focus();
        //        //imgLoad.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, imgLoad));
        //    }
        //}

        private void imgLook_Click(object sender, RoutedEventArgs e)
        {
            //Search for a Records
            //Obtengo solamente los tipo cliente(Cecoa)
            DataList = (new wmsEntities()).Tercero.ToList();

            //Cargar la lista de Records
            this.cboData.Visibility = Visibility.Visible;
            this.cboData.IsDropDownOpen = true;

            if (DataList.Count == 1)
            {
                this.cboData.Visibility = Visibility.Collapsed;
                txtDescripcion.Visibility = Visibility.Visible;
                txtData.Text = DataList[0].Nombre +" "+ DataList[0].Apellidos;
                txtDescripcion.Text = DataList[0].Nombre + " " + DataList[0].Apellidos;
                this.Terceros = DataList[0];
                imgLoad.Focus();
                //imgLoad.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, imgLoad));
            }
        }

        private void txtData_KeyDown_1(object sender, KeyEventArgs e)
        {
             if (e.Key == Key.Tab)
            {
                return;
            }

            this.Terceros = new Tercero();

            LoadDataList();

            //Cargar la lista de Records
            this.cboData.Visibility = Visibility.Visible;
            this.cboData.IsDropDownOpen = true;

            if (DataList.Count == 1)
            {
                this.Terceros = DataList[0];
                imgLoad.Focus();
                //imgLoad.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, imgLoad));
            }
        }

        private void cboData_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
            Tercero data = ((ComboBox)sender).SelectedItem as Tercero;

            if (data == null)
                return;

            this.Terceros = data;
            imgLoad.Focus();
        }

        //Para controlar La navegacion con Tab
        private void NO_tab_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                return;
            }
        }
    }
}
