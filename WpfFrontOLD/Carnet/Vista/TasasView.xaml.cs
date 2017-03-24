using System;
using System.Windows.Controls;
using Core.WPF;
using System.Windows;
using WpfFront.Common;
using WpfFront.Model;
using System.Windows.Input;
using System.Data;
using WpfFront.Modelo;
using System.Linq;

namespace WpfFront.Vista
{
    /// <summary>

    /// </summary>
    public partial class TasasView : UserControlBase, ITasasView
    {

        wmsEntities db = new wmsEntities();
        // private readonly WMSServiceClient service = new WMSServiceClient();

        public TasasView()
        {
            InitializeComponent();
        }

        public TasasModel Model
        {
            get
            { return this.DataContext as TasasModel; }
            set
            { this.DataContext = value; }
        }

        #region Metodos

        private void Tasas_Loaded_1(object sender, RoutedEventArgs e)
        {
        }

        private void DockPanel_Loaded_1(object sender, RoutedEventArgs e)
        {
            Model.RecordTarifaBusqueda = new Tarifa();
            // Model.ListaTarifas = service.GetTarifa(new Tarifa { });
            Model.ListaTarifas = db.Tarifa.ToList();
            // Model.ListaTipoTasa = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOTARIFACARNET" }, Active = true });
            Model.ListaTipoTasa = db.Tipo.Where(f=>f.Agrupacion.Codigo == "TIPOTARIFACARNET" && f.Activo==true).ToList();
        }

        private void ListTasas_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            //SeleccionarTasas(sender, new DataEventArgs<Tarifa>((Tarifa)ListTasas.SelectedItem));
            Model.RecordTarifa = (Tarifa)ListaTarifas.SelectedItem;
            PanelNuevoRegistro.Visibility = Visibility.Visible;
        }

        private void btnGuardarRegistro_Click_1(object sender, RoutedEventArgs e)
        {
            if (FechaInicio.SelectedDate == null)
            {
                Util.ShowError("Debe seleccionar una fecha de inicio para la tasa");
                return;
            }
            if (FechaFinal.SelectedDate == null)
            {
                Util.ShowError("Debe seleccionar una fecha final para la tasa");
                return;
            }
            if (Tipo.SelectedIndex == -1)
            {
                Util.ShowError("Debe seleccionar un tipo de tasa");
                return;
            }
            if (Rango.SelectedIndex == -1)
            {
                Util.ShowError("Debe seleccionar un tiempo de validez para la tasa");
                return;
            }
            if (string.IsNullOrEmpty(Valor.Text))
            {
                Util.ShowError("Debe ingresar el valor de la tasa");
                return;
            }

            //Valido que la tasa no exista en el mismo rango, con el mismo tipo y el mismo tiempo de validez
            foreach (Tarifa TasaAu in Model.ListaTarifas)
            {
                if (TasaAu.RowID != Model.RecordTarifa.RowID)
                {
                    if (
                        (
                        (TasaAu.FechaInicio <= FechaInicio.SelectedDate && TasaAu.FechaFinal >= FechaInicio.SelectedDate) ||
                        (TasaAu.FechaInicio <= FechaFinal.SelectedDate && TasaAu.FechaFinal >= FechaFinal.SelectedDate) ||
                        (TasaAu.FechaInicio >= FechaInicio.SelectedDate && TasaAu.FechaFinal <= FechaFinal.SelectedDate)
                        ) &&
                            TasaAu.TipoID == ((Tipo)Tipo.SelectedItem).RowID &&
                            TasaAu.RangoID == ((Tipo)Rango.SelectedItem).RowID)
                    {
                        Util.ShowError("No se puede crear la nueva tasa porque hay una activa. Por favor revisar");
                        return;
                    }
                }
            }

            if (Model.RecordTarifa.RowID != 0)
            {
                PanelNuevoRegistro.Visibility = Visibility.Collapsed;
                //  service.UpdateTarifa(Model.RecordTarifa);
                db.SaveChanges();
                CleanToCreate();
                Util.ShowMessage("Se actualizo exitosamente la tasa");
            }
            else
            {
                Model.RecordTarifa.FechaInicio = FechaInicio.SelectedDate.Value;
                Model.RecordTarifa.FechaFinal = FechaFinal.SelectedDate.Value;
                Model.RecordTarifa.Tipo = (Tipo)Tipo.SelectedItem;
                Model.RecordTarifa.RangoID = ((Tipo)Rango.SelectedItem).RowID;
                Model.RecordTarifa.Valor = Double.Parse(Valor.Text);
                Model.RecordTarifa.ValorReexpedicion = string.IsNullOrWhiteSpace(ValorReexpedicion.Text)? 0 : Double.Parse(ValorReexpedicion.Text);
                Model.RecordTarifa.UsuarioCreacion = App.curUser.NombreUsuario;
                Model.RecordTarifa.FechaCreacion = DateTime.Now;
               // service.SaveTarifa(Model.RecordTarifa);

                CleanToCreate();
                Util.ShowMessage("Se registro exitosamente la tasa");
                ConsultarTarifas();
                PanelNuevoRegistro.Visibility = Visibility.Collapsed;
            }
        }

        public void ConsultarTarifas()
        {
           // Model.ListaTarifas = service.GetTarifa(new Tarifa { });
            Model.ListaTarifas = db.Tarifa.ToList();
        }

        private void btnActualizarLista_Click_1(object sender, RoutedEventArgs e)
        {
            ConsultarTarifas();
        }

        private void btnNuevoRegistro_Click_1(object sender, RoutedEventArgs e)
        {
            PanelNuevoRegistro.Visibility = Visibility.Visible;
            Model.RecordTarifa = new Tarifa();
        }

        #endregion



        private void cmb_TipoTasa_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (Tipo.SelectedItem != null)
            {
                if (((Tipo)Tipo.SelectedItem).Codigo == "PERSONA")
                {
                    //Cargo las opciones de carnets dependiendo del tipo de carnet solicitado
                    //  Model.ListaTiempoValidez = service.GetMMaster(new MMaster { MetaType = new MType { Code = "RANGOPERSONAL" } }).OrderBy(f => f.NumOrder).ToList();
                    Model.ListaTiempoValidez = db.Tipo.Where(f=>f.Agrupacion.Codigo == "RANGOPERSONAL").OrderBy(f => f.Orden).ToList();
                }
                else if (((Tipo)Tipo.SelectedItem).Codigo == "VEHICULO")
                {
                    //Cargo las opciones de carnets dependiendo del tipo de carnet solicitado
                   // Model.ListaTiempoValidez = service.GetMMaster(new MMaster { MetaType = new MType { Code = "RANGOVEHICULOS" } }).OrderBy(f => f.NumOrder).ToList();
                    Model.ListaTiempoValidez = db.Tipo.Where(f=>f.Agrupacion.Codigo == "RANGOVEHICULOS").OrderBy(f => f.Orden).ToList();

                }
            }
        }

        private void btnBuscarTasas_Click_1(object sender, RoutedEventArgs e)
        {
            Model.RecordTarifaBusqueda.Tipo = FiltroTipo.SelectedItem == null ? null : (Tipo)FiltroTipo.SelectedItem;
            Model.RecordTarifaBusqueda.Tipo = FiltroRango == null ? null : (Tipo)FiltroRango.SelectedItem;
            //Model.RecordTarifaBusqueda.Valor = valor
            //Model.ListaTarifas = service.GetTarifa(Model.RecordTarifaBusqueda);
            Model.ListaTarifas = db.Tarifa.ToList();
            Model.RecordTarifaBusqueda = new Tarifa();
        }

        public void CleanToCreate()
        {
            Model.RecordTarifa = new Tarifa();
        }

        private void FiltroTipo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (FiltroTipo.SelectedItem != null)
            {
                if (((Tipo)FiltroTipo.SelectedItem).Codigo == "PERSONA")
                {
                    //Cargo las opciones de carnets dependiendo del tipo de carnet solicitado
                   // Model.ListaTiempoValidezFiltro = service.GetMMaster(new MMaster { MetaType = new MType { Code = "RANGOPERSONAL" } }).OrderBy(f => f.NumOrder).ToList();
                    Model.ListaTiempoValidezFiltro = db.Tipo.Where(f=>f.Agrupacion.Codigo == "RANGOPERSONAL").OrderBy(f=>f.Orden).ToList();
                }
                else if (((Tipo)FiltroTipo.SelectedItem).Codigo == "VEHICULO")
                {
                    //Cargo las opciones de carnets dependiendo del tipo de carnet solicitado
                 //   Model.ListaTiempoValidezFiltro = service.GetMMaster(new MMaster { MetaType = new MType { Code = "RANGOVEHICULOS" } }).OrderBy(f => f.NumOrder).ToList();
                    Model.ListaTiempoValidezFiltro = db.Tipo.Where(f => f.Agrupacion.Codigo == "RANGOVEHICULOS").OrderBy(f=>f.Orden).ToList();
                }
            }
        }
    }

    public interface ITasasView
    {
        //Clase Modelo
        TasasModel Model { get; set; }
    }
}
