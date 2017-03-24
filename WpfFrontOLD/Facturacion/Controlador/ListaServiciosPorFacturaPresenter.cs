using System;
using WpfFront.Modelo;
using WpfFront.Model;
using WpfFront.Vista;
using Assergs.Windows;
using Microsoft.Practices.Unity;
using System.Windows.Controls;
using System.Linq;
using System.Data;


namespace WpfFront.Controlador
{

    public interface IListaServiciosPorFacturaPresenter
    {
        IListaServiciosPorFacturaView View { get; set; }
        ToolWindow Window { get; set; }

        void CargarDocumento( Int32 FacturaID , Object Presenter);

    }


    public class ListaServiciosPorFacturaPresenter : IListaServiciosPorFacturaPresenter
    {
        public IListaServiciosPorFacturaView View { get; set; }
        private readonly IUnityContainer container;
        private readonly wmsEntities _db;
        public ToolWindow Window { get; set; }
        public Object PresenterParent { get; set; }

        public ListaServiciosPorFacturaPresenter(IUnityContainer container, IListaServiciosPorFacturaView view)
        {

            View = view;
            this.container = container;
            this._db = new wmsEntities();
            View.Model = this.container.Resolve<ListaServiciosPorFacturaModel>();
            View.CerrarTab += this.OnCerrarTab;


        }


        public void CargarDocumento(Int32 FacturaID, Object Presenter)
        {

            if (FacturaID != 0)
            {
                //View.Model.RecordServiciosDetalladoList = _db.GetServicios(new Servicios
                //{
                //    Facturas = new Facturas { RowID = FacturaID},
                //    Operacion = new Operacion
                //    {                        
                //        TipoFacturacion = new MMaster { Code = "CREDITO" }
                //    }
                //}).Where(f => f.Factura != null).ToList();
                View.Model.RecordServiciosDetalladoList = _db.Servicios.Where(f => f.Facturas != null)
                    .Where(f=> f.Facturas.RowID == FacturaID && f.Operacion.Tipo.Codigo == "CREDITO").ToList();


                Servicios aux = View.Model.RecordServiciosDetalladoList.First();
                View.FechaEmision.Text = aux.Facturas.FechaEmision.ToString();
                View.FechaInicial.Text = aux.Facturas.FechaInicio.ToString();
                View.FechaFinal.Text = aux.Facturas.FechaFinal.ToString();
                View.Aerolinea.Text = aux.Operacion.Aeronave.Tercero.Nombre.ToString();
                View.Tipo.Text = aux.Tipo.Codigo2;
                View.ValorTotal.Text = View.Model.RecordServiciosDetalladoList.Sum(f => f.Valor).Value.ToString("N0");
                View.TxtTotal.Text = View.Model.RecordServiciosDetalladoList.Sum(f => f.Valor).Value.ToString("N0");
                View.CreadaPor.Text = aux.Facturas.UsuarioCreacion;

            }
            if (Presenter != null)
            {
                //Asigno el PresenterParente
                PresenterParent = Presenter;
            }
        }

        public void OnCerrarTab(object sender, EventArgs e)
        {
            try
            {
                //Cierro el Tab seleccionado actualmente
                ((HistoricoFacturacionPresenter)PresenterParent).View.TabPadre.Items.Remove(((TabItem)((HistoricoFacturacionPresenter)PresenterParent).View.TabPadre.SelectedItem));
                //Selecciono por defecto el Tab con el listado de facturas
                ((TabItem)((HistoricoFacturacionPresenter)PresenterParent).View.TabPadre.Items[0]).Focus();
            }
            catch (Exception)
            {}

            try
            {
                //Cierro el Tab seleccionado actualmente
                ((FacturasAgrupadasPresenter)PresenterParent).View.TabPadreListaFacturas.Items.Remove(((TabItem)((FacturasAgrupadasPresenter)PresenterParent).View.TabPadreListaFacturas.SelectedItem));
                //Selecciono por defecto el Tab con el listado de facturas
                ((TabItem)((FacturasAgrupadasPresenter)PresenterParent).View.TabPadreListaFacturas.Items[0]).Focus();
            }
            catch (Exception)
            { }

            try
            {
                //Cierro el Tab seleccionado actualmente
                ((FacturasTasasPresenter)PresenterParent).View.TabPadreListaFacturas.Items.Remove(((TabItem)((FacturasTasasPresenter)PresenterParent).View.TabPadreListaFacturas.SelectedItem));
                //Selecciono por defecto el Tab con el listado de facturas
                ((TabItem)((FacturasTasasPresenter)PresenterParent).View.TabPadreListaFacturas.Items[0]).Focus();
            }
            catch (Exception)
            {}
            //Actualizo la lista de Facturas
            //((ListaOperacionesPresenter)PresenterParent).View.Model.ListaOperaciones = service.GetOperacion(new Operacion { });
        }

    }
}