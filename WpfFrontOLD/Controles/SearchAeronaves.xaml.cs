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
    /// Interaction logic for SearchAeronaves.xaml
    /// </summary>
    public partial class SearchAeronaves : UserControl, INotifyPropertyChanged
    {

        public event EventHandler OnSelected;

        public SearchAeronaves()
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
            typeof(SearchAeronaves));

        //-
        private Aeronave _AeronaveSeleccionada;
        public Aeronave AeronaveSeleccionada
         {
            get { return _AeronaveSeleccionada; } //(String)GetValue(TextProperty);
             set { _AeronaveSeleccionada = value; }
        }

        public String Text
        {
            get { return this.txtData.Text; } //(String)GetValue(TextProperty);
            set { this.txtData.Text = value; }
        }

        private IList<Aeronave> _DataList;
        public IList<Aeronave> DataList
        {
            get { return _DataList; }
            set
            {
                _DataList = value;
                OnPropertyChanged("DataList");
            }
        }

        private IList<Aeronave> _DefList;
        public IList<Aeronave> DefaultList
        {
            get { return _DefList; }
            set { _DefList = value; }
        }

        public static DependencyProperty AeronavesProperty = DependencyProperty.Register("Aeronave", typeof(Aeronave), typeof(SearchAeronaves));

        public Aeronave Aeronaves
        {
            get { return (Aeronave)GetValue(AeronavesProperty); }
            set { SetValue(AeronavesProperty, value);     }
        }

        //-
        public void cargarValorEspecifico(string valortxt, string valorDescripcion)
        {
            this.cboData.Visibility = Visibility.Collapsed;
            txtDescripcion.Visibility = Visibility.Visible;
            txtData.Text = valortxt;
            txtDescripcion.Text = valorDescripcion;
            imgLoad.Focus();
        }

        public static DependencyProperty AccTypeProperty = DependencyProperty.Register("AccType", typeof(Int16), typeof(SearchAeronaves));

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
            //LoadDataList()

            DataList = (new wmsEntities()).Aeronave.ToList();

            //Cargar la lista de Records
            this.cboData.Visibility = Visibility.Visible;
            this.cboData.IsDropDownOpen = true;

            if (DataList.Count == 1)
            {
                this.cboData.Visibility = Visibility.Collapsed;
                this.txtDescripcion.Visibility = Visibility.Visible;
                txtData.Text = DataList[0].Matricula;
                txtDescripcion.Text = DataList[0].PBMOKG + "Kg - " + DataList[0].TipoAeronave + " - " + DataList[0].CapacidadPasajeros+"Pax";
                this.Aeronaves = DataList[0];
                //Guardo el valor seleccionado en una variable temporal que se llamara desde OperacionesPresenter
                AeronaveSeleccionada = DataList[0];

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
                this.Aeronaves = null;

                LoadDataList();

                //Cargar la lista de Records
                this.cboData.Visibility = Visibility.Visible;
                this.cboData.IsDropDownOpen = true;

                if (DataList.Count == 1)
                {
                    this.cboData.Visibility = Visibility.Collapsed;
                    txtDescripcion.Visibility = Visibility.Visible;
                    txtData.Text = DataList[0].Matricula;
                    txtDescripcion.Text = DataList[0].PBMOKG + "Kg - " + DataList[0].TipoAeronave + " - " + DataList[0].CapacidadPasajeros + "Pax";
                    //Guardo el valor seleccionado en una variable temporal que se llamara desde OperacionesPresenter
                    AeronaveSeleccionada = DataList[0];

                    this.Aeronaves = DataList[0];
                    imgLoad.Focus();
                    //imgLoad.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, imgLoad));
                }
            //}
        }

        private void LoadDataList()
        {
            if (!String.IsNullOrEmpty(txtData.Text))
            {
                DataList = DefaultList;

                DataList = (new wmsEntities()).Aeronave.Where(f=>f.Matricula == txtData.Text).Take(15).ToList();
                return;
            }
            DataList = (new wmsEntities()).Aeronave.ToList();
        }
       

        private void cboData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Aeronave data = ((ComboBox)sender).SelectedItem as Aeronave;
            if (data == null)
                return;
            txtData.Text = data.Matricula;
            txtDescripcion.Text = DataList[0].PBMOKG + "Kg - " + DataList[0].TipoAeronave + " - " + DataList[0].CapacidadPasajeros+"Pax";
            //Guardo el valor seleccionado en una variable temporal que se llamara desde OperacionesPresenter
            AeronaveSeleccionada = data;
            this.Aeronaves = data;
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
