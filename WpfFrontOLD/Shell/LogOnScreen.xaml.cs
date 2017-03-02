using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WpfFront.Common;
using WpfFront.Utilidades;

namespace WpfFront
{
    public partial class LogOnScreen : Window, INotifyPropertyChanged
    {

        private Visibility hintVisibility;

        public LogOnScreen()
        {
            InitializeComponent();
            DataContext = this;
            HintVisibility = Visibility.Hidden;

            try
            {
                //xDomain.ItemsSource = db.GetDomainList();
                //Carga el cliente de TIGO-UNE
                //var lista_clientes = db.GetCustomersList().Where(f => f.DataKey.Equals("TIGO-UNE")).ToList();
                //lista_clientes.ForEach(s => s.DataKey = "ETB"); // Se reemplaza por ETB para utilizar las clases de TIGO-UNE

                //xDomain.ItemsSource = lista_clientes;
                //xDomain.SelectedIndex = 0;

                xUsername.Focus();
            }
            catch
            {
                throw; //new Exception( + ex.Message);
            }

        }

        private void DoLogonClick(object sender, RoutedEventArgs e)
        {

            try
            {
                Autenticacion obj = new Autenticacion();

                App.curUser = obj.ValidarUsuario(xUsername.Text, xPassword.Password);
                App.curCompany = App.curUser.Compañia;
                //App.currentLocation = ((ShowData)xDomain.SelectedItem).DataValue;

                DialogResult = true;
                Close();

            }
            catch (Exception ex)
            {
                Util.ShowError(ex.Message);
            }

        }


        public bool HintVisible
        {
            get { return HintVisibility == Visibility.Visible; }
            set
            {
                if (value)
                {
                    HintVisibility = Visibility.Visible;
                }
                else
                {
                    HintVisibility = Visibility.Hidden;
                }
            }
        }

        public Visibility HintVisibility
        {
            get { return hintVisibility; }
            set
            {
                if (value != hintVisibility)
                {
                    this.hintVisibility = value;
                    OnPropertyChanged("HintVisibility");
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


        private void DoCredentialsFocussed(object sender, RoutedEventArgs e)
        {
            TextBoxBase tb = sender as TextBoxBase;
            if (tb == null)
            {
                PasswordBox pwb = sender as PasswordBox;
                pwb.SelectAll();
            }
            else
            {
                tb.SelectAll();
            }
        }
    }

}
