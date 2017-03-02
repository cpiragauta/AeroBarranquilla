using System;
using Assergs.Windows;
using WMComposite.Events;
using Microsoft.Practices.Unity;
using WpfFront.Common;
using System.Windows;
using System.Linq;
using WpfFront.Model;
using WpfFront.Vista;
using WpfFront.Modelo;


namespace WpfFront.Controlador
{

    public interface IAeropuertoPresenter
    {
        IAeropuertoView View { get; set; }
        ToolWindow Window { get; set; }
    }


    public class AeropuertoPresenter : IAeropuertoPresenter
    {
        public IAeropuertoView View { get; set; }
        private readonly IUnityContainer container;
        public ToolWindow Window { get; set; }
        public Object PresenterParent { get; set; }

        wmsEntities db = new wmsEntities();

        public AeropuertoPresenter(IUnityContainer container, IAeropuertoView view)
        {
            View = view;
            this.container = container;
            View.Model = this.container.Resolve<AeropuertoModel>();

            #region Definicion Metodos

            View.AgregarAeropuerto += new EventHandler<EventArgs>(this.onAgregar);
            View.BuscarAeropuerto += new EventHandler<EventArgs>(this.onBuscar);
            View.SeleccionarAeropuerto += new EventHandler<DataEventArgs<Aeropuerto>>(this.onSeleccionar);
            View.NuevoRegistro += new EventHandler<EventArgs>(this.onNuevoRegistro);
            View.ActualizarListaAeropuerto += new EventHandler<EventArgs>(this.onActualizarListaAeropuerto);
            #endregion

            #region Datos

            //View.Model.ListaAerolineas = db.GetMetaMaster(new MetaMaster { MetaType = new MetaType { MetaTypeID = 12 } });
            //view.Model.ListaTipoFactura = db.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOFACTURACION" } });

            view.Model.ListaTipoAeropuerto = db.Tipo.Where(f => f.Agrupacion.Codigo == "TIPOOPERACION").ToList();//db.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOVUELO" } });


            //Inicio las variables
            CleanToCreate();
            this.ActualizarListaAeropuertos();
            #endregion
        }

        #region Declaracion Metodos

        public void onAgregar(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(View.TXT_Ciudad.Text) || string.IsNullOrEmpty(View.TXT_Sigla.Text) ||
                string.IsNullOrEmpty(View.TXT_Pais.Text) || string.IsNullOrEmpty(View.TXT_Nombre.Text) || string.IsNullOrEmpty(View.TXT_SiglaOACI.Text) || View.cbxTipoAeropuerto.SelectedItem == null)
            {
                Util.ShowError("Todos Los Campos deben ser llenados");
                return;
            }
            foreach (Aeropuerto item in View.Model.ListAeropuertos)
            {
                //Valido que no sea el mismo
                if (item.RowID != View.Model.Aeropuerto.RowID)
                {
                    if (item.SiglaOACI.ToUpper() == View.Model.Aeropuerto.SiglaOACI.ToUpper())
                    {
                        Util.ShowError("Ya existe un Aeropuerto con la misma Sigla OASI");
                        return;
                    }
                    if (item.SiglaIATA.ToUpper() == View.Model.Aeropuerto.SiglaIATA.ToUpper())
                    {
                        Util.ShowError("Ya existe un Aeropuerto con la misma Sigla IATA");
                        return;
                    }
                }
            }
            if (View.Model.Aeropuerto.RowID != 0)
            {
                //Actualizar registro si existe
                View.Model.Aeropuerto.Pais = View.TXT_Pais.Text.ToUpper();
                View.Model.Aeropuerto.Nombre = View.TXT_Nombre.Text.ToUpper();
                View.Model.Aeropuerto.Ciudad = View.TXT_Ciudad.Text.ToUpper();
                View.Model.Aeropuerto.SiglaIATA = View.TXT_Sigla.Text.ToUpper();
                View.Model.Aeropuerto.SiglaOACI = View.TXT_SiglaOACI.Text.ToUpper();
                View.Model.Aeropuerto.FechaModificacion = DateTime.Now;
                View.Model.Aeropuerto.UsuarioModificacion = App.curUser.NombreUsuario;
                View.Model.Aeropuerto.TipoAeropuertoID = ((Tipo)View.cbxTipoAeropuerto.SelectedItem).RowID;

                db.SaveChanges();

                //Actualizar Vista
                this.ActualizarListaAeropuertos();
                this.controlarPanelNuevoRegistro(false);
                CleanToCreate();
            }
            else
            {
                //Llenar Registro si es uno nuevo
                Aeropuerto Registro = new Aeropuerto();
                Registro.Ciudad = View.TXT_Ciudad.Text.ToUpper();
                Registro.Pais = View.TXT_Pais.Text.ToUpper();
                Registro.SiglaIATA = View.TXT_Sigla.Text.ToUpper();
                Registro.Nombre = View.TXT_Nombre.Text.ToUpper();
                Registro.SiglaOACI = View.TXT_SiglaOACI.Text.ToUpper();
                Registro.TipoAeropuertoID = ((Tipo)View.cbxTipoAeropuerto.SelectedItem).RowID;
                Registro.FechaCreacion = DateTime.Now;
                Registro.UsuarioCreacion = App.curUser.NombreUsuario;
                //Crear Registro
                Registro = db.Aeropuerto.Add(Registro);
                db.SaveChanges();
                //Actualizar Vista
                this.ActualizarListaAeropuertos();
                //cerrando panel de nuevo registro               
                this.controlarPanelNuevoRegistro(false);
                CleanToCreate();
            }



        }
        public void onBuscar(object sender, EventArgs e)
        {
            if (View.cbxFiltrarTipoAeropuerto.SelectedItem != null)
            {
                int mmasterID = ((Tipo)View.cbxFiltrarTipoAeropuerto.SelectedItem).RowID;
            }

            View.Model.ListAeropuertos = db.Aeropuerto.ToList() ;

            if (!String.IsNullOrEmpty(View.TXT_BuscarNombre.Text))
            {
                View.Model.ListAeropuertos = View.Model.ListAeropuertos.Where(F => F.Nombre.ToUpper().Contains(View.TXT_BuscarNombre.Text.ToUpper())).ToList();
            }
            if (!String.IsNullOrEmpty(View.TXT_BuscarSigla.Text))
            {
                View.Model.ListAeropuertos = View.Model.ListAeropuertos.Where(F => F.SiglaIATA.ToUpper().Contains(View.TXT_BuscarSigla.Text.ToUpper())).ToList();
            }
            if (!String.IsNullOrEmpty(View.TXT_BuscarSiglaOACI.Text))
            {
                View.Model.ListAeropuertos = View.Model.ListAeropuertos.Where(F => F.SiglaOACI.ToUpper().Contains(View.TXT_BuscarSiglaOACI.Text.ToUpper())).ToList();
            }
            if (!String.IsNullOrEmpty(View.TXT_BuscarCiudad.Text))
            {
                View.Model.ListAeropuertos = View.Model.ListAeropuertos.Where(F => F.Ciudad.ToUpper().Contains(View.TXT_BuscarCiudad.Text.ToUpper())).ToList();
            }
            if (!String.IsNullOrEmpty(View.TXT_BuscarPais.Text))
            {
                View.Model.ListAeropuertos = View.Model.ListAeropuertos.Where(F => F.Pais.ToUpper().Contains(View.TXT_BuscarPais.Text.ToUpper())).ToList();
            }
            if (View.cbxFiltrarTipoAeropuerto.SelectedIndex != -1)
            {
                View.Model.ListAeropuertos = View.Model.ListAeropuertos.Where(F => F.TipoAeropuertoID == ((Tipo)View.cbxFiltrarTipoAeropuerto.SelectedItem).RowID).ToList();
            }
        }
        public void onSeleccionar(object sender, DataEventArgs<Aeropuerto> Aeropuerto)
        {
            View.Model.Aeropuerto = Aeropuerto.Value;
            this.controlarPanelNuevoRegistro(true);

        }
        public void onNuevoRegistro(object sender, EventArgs e)
        {
            this.controlarPanelNuevoRegistro(true);
            this.CleanToCreate();
        }
        public void onActualizarListaAeropuerto(object sender, EventArgs e)
        {
            this.ActualizarListaAeropuertos();
            this.clear();
        }

        /**
       * Actualiza la lista de Aeropuertos 
       * */
        public void ActualizarListaAeropuertos()
        {
            View.Model.ListAeropuertos = db.Aeropuerto.ToList();
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
        public void clear()
        {
            View.TXT_BuscarCiudad.Text = "";
            View.TXT_BuscarNombre.Text = "";
            View.TXT_BuscarPais.Text = "";
            View.TXT_BuscarSigla.Text = "";
            View.TXT_BuscarSiglaOACI.Text = "";
            View.cbxFiltrarTipoAeropuerto.SelectedIndex = -1;
        }
        public void CleanToCreate()
        {
            View.TXT_Sigla.Clear();
            View.TXT_Pais.Clear();
            View.TXT_Ciudad.Clear();
            View.TXT_BuscarSigla.Clear();
            View.TXT_BuscarPais.Clear();
            View.TXT_BuscarCiudad.Clear();
            View.Model.Aeropuerto = new Aeropuerto();
        }

        #endregion
    }
}