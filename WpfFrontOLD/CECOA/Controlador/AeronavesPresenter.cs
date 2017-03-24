using System;
using WpfFront.Modelo;
using Assergs.Windows;
using WMComposite.Events;
using Microsoft.Practices.Unity;
using WpfFront.Vista;
using WpfFront.Common;
using System.Windows;
using System.Linq;
using WpfFront.Model;


namespace WpfFront.Controlador
{

    public interface IAeronavesPresenter
    {
        IAeronavesView View { get; set; }
        ToolWindow Window { get; set; }
    }


    public class AeronavesPresenter : IAeronavesPresenter
    {

        wmsEntities db = new wmsEntities();
        public IAeronavesView View { get; set; }
        private readonly IUnityContainer container;
      //  private readonly WMSServiceClient service;
        public ToolWindow Window { get; set; }
        //Variables Auxiliares 
     
        public Object PresenterParent { get; set; }
        
        public AeronavesPresenter(IUnityContainer container, IAeronavesView view)

        {

            View = view;
            this.container = container;
           // this.service = new WMSServiceClient();
            View.Model = this.container.Resolve<AeronavesModel>();

            #region Metodos
            View.BuscarAeronave += new EventHandler<EventArgs>(this.onBuscar);

            View.CargarAeronave += this.OnCargarAeronave;
            view.GuardarAeronave += this.OnGuardarAeronave;
            view.DeleteAeronave += this.OnDeleteAeronave;
            view.NewAeronave += this.OnNewAeronave;
            View.NuevoRegistro += new EventHandler<EventArgs>(this.onNuevoRegistro);
            View.ActualizarListaAeronaves += new EventHandler<EventArgs>(this.onActualizarListaAeronaves);
            #endregion

            #region Datos

           

            this.ActualizarListaAeronaves();
            View.Model.RecordAeronaves = new Aeronave();
           // View.Model.TipoOperacion = service.GetMMaster(new { MetaType = new MType { Code = "TIPOOPERACION" } });
            View.Model.TipoOperacion = db.Tipo.Where(f => f.Agrupacion.Codigo == "TIPOOPERACION").ToList();
            CleanToCreate();

            #endregion
        }

        #region Metodos


        public void onBuscar(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(View.Txt_FiltroCapacidad.Text))
            {
                //View.Model.ListadoAeronaves = service.GetAeronaves(new Aeronaves { Matricula = View.Txt_FiltroMatricula.Text, Propietario = View.SearchFiltroPropietaria.Terceros, Clasificacion = (MMaster)View.cbx_FiltroTipoOper.SelectedItem }).ToList();
                View.Model.ListadoAeronaves = db.Aeronave.Where(f => f.Matricula == View.Txt_FiltroMatricula.Text && f.PropietarioID == View.SearchFiltroPropietaria.Terceros.RowID && f.ClasificacionID == ((Tipo)View.cbx_FiltroTipoOper.SelectedItem).RowID).ToList();

            }
            else
            {
               // View.Model.ListadoAeronaves = service.GetAeronaves(new Aeronave { Matricula = View.Txt_FiltroMatricula.Text, CapacidadPasajeros = Int32.Parse(View.Txt_FiltroCapacidad.Text), Propietario = View.SearchFiltroPropietaria.Terceros, Clasificacion = (MMaster)View.cbx_FiltroTipoOper.SelectedItem }).ToList();
                View.Model.ListadoAeronaves = db.Aeronave.Where(f => f.Matricula == View.Txt_FiltroMatricula.Text && f.CapacidadPasajeros == Int32.Parse(View.Txt_FiltroCapacidad.Text) && f.PropietarioID == View.SearchFiltroPropietaria.Terceros.RowID && f.ClasificacionID == ((Tipo)View.cbx_FiltroTipoOper.SelectedItem).RowID).ToList();
            }

        }
        public void OnNewAeronave(Object sender, EventArgs e)
        {
            CleanToCreate();
        }

        public void OnGuardarAeronave(Object sender, EventArgs e)
        {
            string message = "";
            //Valido que no exista una aeronave con la misma matricula
            if (//service.GetAeronaves(new Aeronave { Matricula = View.Model.RecordAeronaves.Matricula.ToUpper() }).Where(f => f.RowID != View.Model.RecordAeronaves.RowID).Count() >= 1)
                db.Aeronave.Where(f=>f.Matricula== View.Model.RecordAeronaves.Matricula.ToUpper() && f.RowID != (View.Model.RecordAeronaves.RowID)).Count() >=1)
                {
                    Util.ShowError("Ya existe una Aeronave con la misma matricula");
                return;
            }
          
            
            try
            {

                //Asigno los valores a la variable para guardar
                View.Model.RecordAeronaves.Matricula = View.Model.RecordAeronaves.Matricula.ToUpper();
                View.Model.RecordAeronaves.TipoAeronave = View.Model.RecordAeronaves.TipoAeronave.ToUpper();
                View.Model.RecordAeronaves.Tercero1 = View.companiaPropietaria.Terceros;
                View.Model.RecordAeronaves.Tercero = View.companiaFactura.Terceros;
                View.Model.RecordAeronaves.ClasificacionID = ((Tipo)View.ListaTipoOp.SelectedItem).RowID;
            }
            catch { }

            if (View.Model.RecordAeronaves.RowID == 0)
            {
                View.Model.RecordAeronaves.FechaCreacion = DateTime.Now;
                View.Model.RecordAeronaves.UsuarioCreacion = App.curUser.NombreUsuario;
               // service.SaveAeronaves(View.Model.RecordAeronaves);
                db.Aeronave.Add(View.Model.RecordAeronaves);
                db.SaveChanges();
                
                
                message = "Registro Creado";

            }
            else
            {

                message = "Registro Actualizado.";
                View.Model.RecordAeronaves.FechaModificacion = DateTime.Now;
                View.Model.RecordAeronaves.UsuarioModificacion = App.curUser.NombreUsuario;
                // service.UpdateAeronaves(View.Model.RecordAeronaves);
                db.SaveChanges();
               
            }
            this.ActualizarListaAeronaves();
            this.controlarPanelNuevoRegistro(false);
            CleanToCreate();
            
            Util.ShowMessage(message);
        }
        

        private void OnCargarAeronave(object sender, DataEventArgs<Aeronave> e)
        {
            //Cargo la pantalla con el documento creado
            CargarAeronave(e.Value);
        }
        public void CargarAeronave(Aeronave RecordAeronave)
        {
            if (RecordAeronave != null)
            {
                View.Model.RecordAeronaves = RecordAeronave;

                View.companiaFactura.Terceros = RecordAeronave.Tercero;
                View.companiaFactura.txtDescripcion.Text = RecordAeronave.Tercero.Nombre;
                View.companiaFactura.txtData.Text = RecordAeronave.Tercero.Nombre;


                View.companiaPropietaria.Terceros = RecordAeronave.Tercero1;
                View.companiaPropietaria.txtDescripcion.Text = RecordAeronave.Tercero1.Nombre;
                View.companiaPropietaria.txtData.Text = RecordAeronave.Tercero1.Nombre;

                View.panelPermisoExplotacion.Visibility = (RecordAeronave.Extranjera == true || RecordAeronave.PermisoExplotacion == true )? Visibility.Visible : Visibility.Collapsed;

                View.Panel_Matricula.Visibility = RecordAeronave.PermisoExplotacion == true ? Visibility.Visible : Visibility.Collapsed;


                try { View.ListaTipoOp.SelectedValue = View.Model.RecordAeronaves.Tipo.RowID; }
                catch { }
                this.controlarPanelNuevoRegistro(true);
            }
        }


        public void OnDeleteAeronave(Object sender, EventArgs e)
        {
            if (View.Model.RecordAeronaves.RowID == 0)
            {
                return;
            }
            //service.DeleteAeronaves(View.Model.RecordAeronaves);
            db.Aeronave.Remove(View.Model.RecordAeronaves);
            db.SaveChanges();
            this.ActualizarListaAeronaves();
            this.controlarPanelNuevoRegistro(false);
            CleanToCreate();
        }
        public void onNuevoRegistro(Object sender, EventArgs e)
        {
            this.controlarPanelNuevoRegistro(true);
            CleanToCreate();
        }
        public void onActualizarListaAeronaves(object sender, EventArgs e)
        {
            this.ActualizarListaAeronaves();
            //Limpio los campos al actualizar
            View.Txt_FiltroMatricula.Text = "";
            View.Txt_FiltroCapacidad.Text = "";
            View.SearchFiltroPropietaria.Terceros = null;
            View.SearchFiltroPropietaria.txtDescripcion.Text = "";
            View.SearchFiltroPropietaria.txtData.Text = "";
            View.cbx_FiltroTipoOper.SelectedIndex = -1;

        }


        /**
       * Actualiza la lista de Aeronaves 
       * */
        public void ActualizarListaAeronaves()
        {
           // View.Model.ListadoAeronaves = service.GetAeronaves(new Aeronave { });
            db.SaveChanges();
        }
        /**
         * Oculta o muestra el panel de nuevo registro
         * true= mostrar
         * false= ocultar
         * */
        public void controlarPanelNuevoRegistro(bool status)
        {
            if (status)
            {
                View.PanelNuevoRegistro.Visibility = Visibility.Visible;
            }
            else
            {
                View.PanelNuevoRegistro.Visibility = Visibility.Collapsed;
            }
        }
        public void CleanToCreate()
        {
            View.Model.RecordAeronaves = new Aeronave();
            View.ListaTipoOp.SelectedIndex = -1;


            View.companiaFactura.Terceros = new Tercero();
            View.companiaFactura.txtDescripcion.Text = "";
            View.companiaFactura.txtData.Text = "";


            View.companiaPropietaria.Terceros = new Tercero();
            View.companiaPropietaria.txtDescripcion.Text = "";
            View.companiaPropietaria.txtData.Text = "";
        }

        #endregion
    }
}