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
    /// Interaction logic for SearchAeropuertos.xaml
    /// </summary>
    public partial class SearchAeropuertos : UserControl, INotifyPropertyChanged
    {

        public event EventHandler OnSelected;

        public SearchAeropuertos()
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

        private void SearchAeropuertosLoaded(object sender, RoutedEventArgs e)
        {
            if (this.DefaultList != null && this.DefaultList.Count == 1)
            {
                DataList = DefaultList;
               // FireEvent(sender, e);
            }
        }
       

        //Text Property
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String),
            typeof(SearchAeropuertos));

        public String Text
        {
            get { return this.txtData.Text; } //(String)GetValue(TextProperty);
            set { this.txtData.Text = value; }
        }

        private IList<Aeropuerto> _DataList;
        public IList<Aeropuerto> DataList
        {
            get { return _DataList; }
            set
            {
                _DataList = value;
                OnPropertyChanged("DataList");
            }
        }

        private IList<Aeropuerto> _DefList;
        public IList<Aeropuerto> DefaultList
        {
            get { return _DefList; }
            set { _DefList = value; }
        }

        public static DependencyProperty AeropuertosProperty = DependencyProperty.Register("Aeropuerto", typeof(Aeropuerto), typeof(SearchAeropuertos));

        public Aeropuerto Aeropuertos
        {
            get { return (Aeropuerto)GetValue(AeropuertosProperty);    }
            set
            {
                SetValue(AeropuertosProperty, value);
            }
        }

        //-
        public void cargarValorEspecifico(string valor)
        {
            this.cboData.Visibility = Visibility.Collapsed;
            txtDescripcion.Visibility = Visibility.Visible;
            txtData.Text = valor;
            txtDescripcion.Text = valor;
            imgLoad.Focus();
        }
        public static DependencyProperty AccTypeProperty = DependencyProperty.Register("AccType", typeof(Int16), typeof(SearchAeropuertos));

        public Int16 AccType
        {
            get { return (Int16)GetValue(AccTypeProperty); }
            set
            {
                SetValue(AccTypeProperty, value);
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

        private void imgLook_Click(object sender, RoutedEventArgs e)
        {
            //Search for a Records
            DataList = (new wmsEntities()).Aeropuerto.ToList();

            //Cargar la lista de Records
            this.cboData.Visibility = Visibility.Visible;
            this.cboData.IsDropDownOpen = true;

            if (DataList.Count == 1)
            {
                this.cboData.Visibility = Visibility.Collapsed;
                txtDescripcion.Visibility = Visibility.Visible;
                txtData.Text = DataList[0].SiglaIATA + "/" + DataList[0].Ciudad;
                txtDescripcion.Text = DataList[0].SiglaIATA + "/" + DataList[0].Ciudad;
                this.Aeropuertos = DataList[0];
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
            LoadDataList();
            //Cargar la lista de Records
            this.cboData.Visibility = Visibility.Visible;
            this.cboData.IsDropDownOpen = true;

            if (DataList.Count == 1)
            {
                this.cboData.Visibility = Visibility.Collapsed;
                txtDescripcion.Visibility = Visibility.Visible;
                txtData.Text = DataList[0].SiglaIATA + "/" + DataList[0].Ciudad;
                txtDescripcion.Text = DataList[0].SiglaIATA + "/"+ DataList[0].Ciudad;
                this.Aeropuertos = DataList[0];
                imgLoad.Focus();
                //imgLoad.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, imgLoad));
            }
        }

        private void LoadDataList()
        {
            if (!String.IsNullOrEmpty(txtData.Text))
            {
                DataList = DefaultList;

                DataList = (new wmsEntities()).Aeropuerto.Where(f=> f.SiglaIATA.ToUpper().StartsWith(txtData.Text.ToUpper())).Take(15).ToList();


                return;
            }
            DataList = (new wmsEntities()).Aeropuerto.ToList();
        }


        private void cboData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Aeropuerto data = ((ComboBox)sender).SelectedItem as Aeropuerto;

            if (data == null)
                return;

            txtData.Text = data.SiglaIATA+"/"+data.Ciudad;
            txtDescripcion.Text = data.SiglaIATA + "/" + data.Ciudad;
            //Guardo el valor seleccionado en una variable temporal que se llamara desde OperacionesPresenter
            this.Aeropuertos = data;
            cboData.Visibility = Visibility.Collapsed;
            txtDescripcion.Visibility = Visibility.Visible;
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
