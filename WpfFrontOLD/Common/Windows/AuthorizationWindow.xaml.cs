using System;
using System.Windows;
using System.Windows.Input;
using WpfFront.Utilidades;
using WpfFront.Model;

namespace WpfFront.Common
{
    public partial class AuthorizationWindow : Window
    {
        string menuOption;

        public AuthorizationWindow(string MenuOption)
        {
            InitializeComponent();
            menuOption = MenuOption;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            ValidateUser();
        }

        private void ValidateUser()
        {
            try
            {
                Autenticacion obj = new Autenticacion();

                Usuario newUser = obj.ValidarUsuario(txtUsername.Text, txtPassword.Password);
            }
            catch (Exception ex)
            {
                Util.ShowError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Util.AllowOption(menuOption))
            {
                DialogResult = true;
                Close();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ValidateUser();
        }
    }
}
