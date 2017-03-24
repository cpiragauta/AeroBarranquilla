using System;
using Core.WPF;
using System.Windows;
using WpfFront.Common;
using System.Windows.Input;
using System.Linq;
using WpfFront.Modelo;
using WpfFront.Model;



namespace WpfFront.Vista
{
    public partial class TercerosView : UserControlBase, ITercerosView

        
    {
        wmsEntities db = new wmsEntities();

        public TercerosView()
        {
            InitializeComponent();

        }

        public TercerosModel Model
        {
            get
            { return this.DataContext as TercerosModel; }
            set
            { this.DataContext = value; }
        }

        #region Metodos

        private void DockPanel_Loaded_1(object sender, RoutedEventArgs e)
        {
            // Model.ListaTercero = service.GetTerceros(new Tercero { });
            Model.ListaTercero = db.Tercero.ToList();
            Model.RecordBusqueda = new Tercero();
            //Model.ListTipoTercero = service.GetMMaster(new Tipo { RowID = new MType { Code = "TIPOTERCERO" } });
            Model.ListTipoTercero = db.Tipo.Where(f=> f.Agrupacion.Codigo == "TIPOTERCERO").ToList();
        }

        private void Btn_guardarCliente_Click_1(object sender, RoutedEventArgs e)
        {
            if ( String.IsNullOrEmpty(txt_Identificacion.Text))
            {
                if (txt_Identificacion.Text == "0")
                {
                    Util.ShowError("Debe ingresar la identificacion");
                    return;
                }
                Util.ShowError("Debe ingresar la identificacion");
                return;
            }
            if (rbtJuridico.IsChecked == true)
            {
                if (string.IsNullOrEmpty(txt_RazonSocial.Text))
                {
                    Util.ShowError("Debe ingresar la razon social");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txt_Nombres.Text))
                {
                    Util.ShowError("Debe ingresar el nombre");
                    return;
                }
                if (string.IsNullOrEmpty(txt_Apellidos.Text))
                {
                    Util.ShowError("Debe ingresar el apellido");
                    return;
                }
            
            }
            if (string.IsNullOrEmpty(txt_Direccion.Text))
            {
                Util.ShowError("Debe ingresar la direccion");
                return;
            }
            if (string.IsNullOrEmpty(txt_Telefono.Text))
            {
                Util.ShowError("Debe ingresar el telefono");
                return;
            }

            if (Model.RecordTercero.RowID == 0)
            {
                if (rbtJuridico.IsChecked == true)
                {
                    Model.RecordTercero.Nombre = txt_RazonSocial.Text;
                    //Model.RecordTercero.TipoTerceroID = service.GetMMaster(new Tipo { Code = "JURIDICA" }).First();
                    Model.RecordTercero.TipoTerceroID = db.Tipo.FirstOrDefault(f=> f.Codigo == "JURIDICA").RowID;
                }
                else
                {
                    Model.RecordTercero.Nombre = txt_Nombres.Text;
                    Model.RecordTercero.Apellidos = txt_Apellidos.Text;
                    //Model.RecordTercero.TipoTerceroID = service.GetMMaster(new Tipo { Code = "NATURAL" }).First();
                    Model.RecordTercero.TipoTerceroID = db.Tipo.FirstOrDefault(f => f.Codigo == "NATURAL").RowID;
                }
                Model.RecordTercero.Direccion = txt_Direccion.Text;
                Model.RecordTercero.Telefono = txt_Telefono.Text;
                Model.RecordTercero.Sincronizado = false;
                Model.RecordTercero.FechaCreacion = DateTime.Now;
                Model.RecordTercero.UsuarioCreacion = App.curUser.NombreUsuario;
                //service.SaveTerceros(Model.RecordTercero);
                db.Tercero.Add(Model.RecordTercero);
                db.SaveChanges();
                Util.ShowMessage("Se almaceno exitosamente");
            }
            else
            {
                //service.UpdateTerceros(Model.RecordTercero);
                db.SaveChanges();
                Util.ShowMessage("Se actualizo exitosamente");
            }

            panelDatosTercero.Visibility = Visibility.Collapsed;
        }

        private void rbtNatural_Checked(object sender, RoutedEventArgs e)
        {
            panel_RazonSocial.Visibility = Visibility.Collapsed;
            panel_Nombres.Visibility = Visibility.Visible;
            panel_Apellidos.Visibility = Visibility.Visible;
            panel_Digito.Visibility = Visibility.Collapsed;
        }

        private void rbtJuridico_Checked(object sender, RoutedEventArgs e)
        {
            panel_RazonSocial.Visibility = Visibility.Visible;
            panel_Nombres.Visibility = Visibility.Collapsed;
            panel_Apellidos.Visibility = Visibility.Collapsed;
            panel_Digito.Visibility = Visibility.Visible;
        }

        private void BuscarTercero_Click(object sender, RoutedEventArgs e)
        {
            // Model.ListaTercero = service.GetTerceros(new Tercero { TipoTerceroID = (Tipo)cb_Tipo_Cliente.SelectedItem ,Identificacion = Model.RecordBusqueda.Identificacion, Nombre = Model.RecordBusqueda.Nombre});
            // Model.ListaTercero = db.Tercero.Where(f => f.TipoTerceroID== ((Tipo)cb_Tipo_Cliente.SelectedItem).RowID  && f.Identificacion == Model.RecordBusqueda.Identificacion && f.Nombre == Model.RecordBusqueda.Nombre).ToList();
            Model.ListaTercero = db.Tercero//.Where(f => cb_Tipo_Cliente.SelectedItem == null ? cb_Tipo_Cliente.SelectedItem == null: f.TipoTerceroID == ((Tipo)cb_Tipo_Cliente.SelectedItem).RowID)
                .Where(f => Model.RecordBusqueda.Identificacion == null ? Model.RecordBusqueda.Identificacion == null : f.Identificacion == Model.RecordBusqueda.Identificacion).
                Where(f => Model.RecordBusqueda.Nombre == null ? Model.RecordBusqueda.Nombre == null : f.Nombre.Contains(Model.RecordBusqueda.Nombre)).ToList();
            Model.RecordBusqueda = new Tercero();
        }

        #endregion

        private void NuevaTercero_Click(object sender, RoutedEventArgs e)
        {
            panelDatosTercero.Visibility = Visibility.Visible;
            rbtNatural.IsChecked = true;
            Model.RecordTercero = new Tercero();
            
        }

        private void btnActualizarLista_Click(object sender, RoutedEventArgs e)
        {
            cb_Tipo_Cliente.SelectedIndex = -1;
            Model.RecordBusqueda = new Tercero();
            //Model.ListaTercero = service.GetTerceros(new Tercero { });
            Model.ListaTercero = db.Tercero.ToList();

        }

        private void ListaTerceros_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (ListaTerceros.SelectedItem == null)
            { return; }

            Model.RecordTercero = (Tercero)ListaTerceros.SelectedItem;
            panelDatosTercero.Visibility = Visibility.Visible;

            if (Model.RecordTercero.Tipo.Codigo == "JURIDICA")
            {
                rbtJuridico.IsChecked = true;
            }
            else
            {
                rbtNatural.IsChecked = true;
            }
        }

}

    public interface ITercerosView
    {
        TercerosModel Model { get; set; }
    }
}