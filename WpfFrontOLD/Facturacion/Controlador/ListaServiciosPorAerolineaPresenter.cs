using System;
using WpfFront.Model;
using WpfFront.Modelo;
using WpfFront.Vista;
using Assergs.Windows;
using Microsoft.Practices.Unity;
using System.Windows.Controls;
using System.Linq;
using System.Data;


namespace WpfFront.Controlador
{

    public interface IListaServiciosPorAerolineaPresenter
    {
        IListaServiciosPorAerolineaView View { get; set; }
        ToolWindow Window { get; set; }

        void CargarDocumento(String Parametros, Object Presenter);
        void CargarDocumentoTasas(String Parametros, Object Presenter);

    }


    public class ListaServiciosPorAerolineaPresenter : IListaServiciosPorAerolineaPresenter
    {
        public IListaServiciosPorAerolineaView View { get; set; }
        private readonly IUnityContainer container;
        private readonly wmsEntities _db;
        public ToolWindow Window { get; set; }
        public Object PresenterParent { get; set; }

        public ListaServiciosPorAerolineaPresenter(IUnityContainer container, IListaServiciosPorAerolineaView view)
        {

            View = view;
            this.container = container;
            this._db = new wmsEntities();
            View.Model = this.container.Resolve<ListaServiciosPorAerolineaModel>();
            View.CerrarTab += this.OnCerrarTab;
            View.AbrirModal += this.OnAbrirModal;


        }

        public void funcionpreuebashdnjaks()
        {

        }
        public void CargarDocumento(String Parametros, Object Presenter)
        {
            String[] Array = Parametros.Split('^');

            int AerolineaID = Convert.ToInt32(Array[0]);
            if (AerolineaID != 0)
            {
                if (String.IsNullOrEmpty(Array[1]) && String.IsNullOrEmpty(Array[1]) && String.IsNullOrEmpty(Array[2]) )//Si no selecciono FECHAS
                {
                    //View.Model.RecordListAerodromo = _db.GetServicios(new Servicios
                    //{
                    //    Operacion = new Operacion
                    //    {
                    //        Aeronave = new Aeronaves { CompañiaFactura = new Terceros { RowID = AerolineaID } }
                    //    },
                    //    Status = new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "Servicios" } }
                    //    //Cuando filtro no tomo los servicios de tasas
                    //}).Where(f => f.Operacion.Tipo.Codigo != "CONTADO" && f.Tipo.Codigo!= "TASAS" && f.Tipo.Codigo!= "TASASDEBITO" && f.Tipo.Codigo!= "TASASCREDITO").ToList();
                    View.Model.RecordListAerodromo = _db.Servicios.Where(f=> f.Operacion.Aeronave.CompañiaFacturaID == AerolineaID  && f.Estado.Nombre == "ParaFacturar" && f.Estado.Tipo.Nombre == "Servicios")
                        .Where(f => f.Operacion.Tipo.Codigo != "CONTADO" && f.Tipo.Codigo != "TASAS" && f.Tipo.Codigo != "TASASDEBITO" && f.Tipo.Codigo != "TASASCREDITO").ToList();
                    //Cuando filtro no tomo los servicios de tasas

                }
                else if (!String.IsNullOrEmpty(Array[1]) && !String.IsNullOrEmpty(Array[2]) && String.IsNullOrEmpty(Array[3])) //Llega con fechas pero sin tipo
                {
                    DateTime FechaInicial = Convert.ToDateTime(Array[1]);
                    DateTime FechaFinal = Convert.ToDateTime(Array[2]);
                    //View.Model.RecordListAerodromo = _db.GetServicios(new Servicios
                    //{
                    //    Operacion = new Operacion
                    //    {
                    //        Aeronave = new Aeronaves { CompañiaFactura = new Terceros { RowID = AerolineaID } }
                    //    },
                    //    Status = new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "Servicios" } }
                    //    //Cuando filtro no tomo los servicios de tasas
                    //}).Where(f => f.Operacion.Salida.FechaSalida >= FechaInicial && f.Operacion.Salida.FechaSalida <= FechaFinal && f.Operacion.Tipo.Codigo != "CONTADO" && f.Tipo.Codigo!= "TASAS" && f.Tipo.Codigo!= "TASASDEBITO" && f.Tipo.Codigo!= "TASASCREDITO")
                    //.ToList();

                    View.Model.RecordListAerodromo = _db.Servicios.Where(f => f.Operacion.Aeronave.CompañiaFacturaID == AerolineaID && f.Estado.Nombre == "ParaFacturar" && f.Estado.Tipo.Nombre == "Servicios")
                        .Where(f => f.Operacion.Salida.FechaSalida >= FechaInicial && f.Operacion.Salida.FechaSalida <= FechaFinal && f.Operacion.Tipo.Codigo != "CONTADO" && f.Tipo.Codigo!= "TASAS" && f.Tipo.Codigo!= "TASASDEBITO" && f.Tipo.Codigo!= "TASASCREDITO")
                      .ToList();


                }
                else if (!String.IsNullOrEmpty(Array[1]) && !String.IsNullOrEmpty(Array[2]) && !String.IsNullOrEmpty(Array[3])) //Llega con fechas y con tipo
                {
                    DateTime FechaInicial = Convert.ToDateTime(Array[1]);
                    DateTime FechaFinal = Convert.ToDateTime(Array[2]);
                    String asd = Array[3];
                    //View.Model.RecordListAerodromo = _db.GetServicios(new Servicios
                    //{
                    //    Operacion = new Operacion
                    //    {
                    //        Llegada = new Llegada { TipoVuelo = new MMaster { Code = Array[3] } },
                    //        Aeronave = new Aeronaves { CompañiaFactura = new Terceros { RowID = AerolineaID } }
                    //    },
                    //    Status = new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "Servicios" } }
                    //    //Cuando filtro no tomo los servicios de tasas
                    //}).Where(f => f.Operacion.Salida.FechaSalida >= FechaInicial && f.Operacion.Salida.FechaSalida <= FechaFinal && f.Operacion.Tipo.Codigo != "CONTADO" && f.Tipo.Codigo!= "TASAS" && f.Tipo.Codigo!= "TASASDEBITO" && f.Tipo.Codigo!= "TASASCREDITO")
                    //.ToList();

                    View.Model.RecordListAerodromo = _db.Servicios.Where(f => f.Operacion.Aeronave.CompañiaFacturaID == AerolineaID && f.Operacion.Llegada.Tipo5.Codigo == Array[3] && f.Estado.Nombre == "ParaFacturar" && f.Estado.Tipo.Nombre == "Servicios")
                        .Where(f => f.Operacion.Salida.FechaSalida >= FechaInicial && f.Operacion.Salida.FechaSalida <= FechaFinal && f.Operacion.Tipo.Codigo != "CONTADO" && f.Tipo.Codigo != "TASAS" && f.Tipo.Codigo != "TASASDEBITO" && f.Tipo.Codigo != "TASASCREDITO")
                      .ToList();


                }
                View.NombreAerolinea.Text = Array[4];
                View.TxtTotal.Text = View.Model.RecordListAerodromo.Sum(f => f.Valor).Value.ToString("N0");
            }
            View.Model.RecordListAerodromo = View.Model.RecordListAerodromo.OrderBy(f => f.Operacion.RowID).ToList();//Organizo por rowidOp
            if (Presenter != null)
            {
                //Asigno el PresenterParente
                PresenterParent = Presenter;
            }
        }



        public void CargarDocumentoTasas(String Parametros, Object Presenter)
        {
            String[] Array = Parametros.Split('^');

            int AerolineaID = Convert.ToInt32(Array[0]);
            if (AerolineaID != 0)
            {
                if (String.IsNullOrEmpty(Array[1]) && String.IsNullOrEmpty(Array[1]) && String.IsNullOrEmpty(Array[2]))//Si no selecciono FECHAS
                {
                    //View.Model.RecordListAerodromo = _db.GetServicios(new Servicios
                    //{
                    //    Operacion = new Operacion
                    //    {
                    //        Aeronave = new Aeronaves { CompañiaFactura = new Terceros { RowID = AerolineaID } }
                    //    },
                    //    Status = new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "Servicios" } }
                    //    //Cuando filtro no tomo los servicios de tasas
                    //}).Where(f => f.Operacion.Tipo.Codigo != "CONTADO" && (f.Tipo.Codigo== "TASAS" || f.Tipo.Codigo== "TASASDEBITO" || f.Tipo.Codigo== "TASASCREDITO")).ToList();
                    View.Model.RecordListAerodromo = _db.Servicios.Where(f => f.Operacion.Aeronave.CompañiaFacturaID == AerolineaID && f.Estado.Nombre == "ParaFacturar" && f.Estado.Tipo.Nombre == "Servicios")
                        .Where(f => f.Operacion.Tipo.Codigo != "CONTADO" && (f.Tipo.Codigo == "TASAS" || f.Tipo.Codigo == "TASASDEBITO" || f.Tipo.Codigo == "TASASCREDITO")).ToList();


                }
                else if (!String.IsNullOrEmpty(Array[1]) && !String.IsNullOrEmpty(Array[2]) && String.IsNullOrEmpty(Array[3])) //Llega con fechas pero sin tipo
                {
                    DateTime FechaInicial = Convert.ToDateTime(Array[1]);
                    DateTime FechaFinal = Convert.ToDateTime(Array[2]);
                    //View.Model.RecordListAerodromo = _db.GetServicios(new Servicios
                    //{
                    //    Operacion = new Operacion
                    //    {
                    //        Aeronave = new Aeronaves { CompañiaFactura = new Terceros { RowID = AerolineaID } }
                    //    },
                    //    Status = new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "Servicios" } }
                    //    //Cuando filtro no tomo los servicios de tasas
                    //}).Where(f => f.Operacion.Salida.FechaSalida >= FechaInicial && f.Operacion.Salida.FechaSalida <= FechaFinal && f.Operacion.Tipo.Codigo != "CONTADO" && ( f.Tipo.Codigo== "TASAS" || f.Tipo.Codigo== "TASASDEBITO" || f.Tipo.Codigo== "TASASCREDITO"))
                    //.ToList();

                    View.Model.RecordListAerodromo = _db.Servicios.Where(f => f.Operacion.Aeronave.CompañiaFacturaID == AerolineaID && f.Estado.Nombre == "ParaFacturar" && f.Estado.Tipo.Nombre == "Servicios")
                        .Where(f => f.Operacion.Salida.FechaSalida >= FechaInicial && f.Operacion.Salida.FechaSalida <= FechaFinal && f.Operacion.Tipo.Codigo != "CONTADO" && (f.Tipo.Codigo == "TASAS" || f.Tipo.Codigo == "TASASDEBITO" || f.Tipo.Codigo == "TASASCREDITO"))
                       .ToList();


                }
                else if (!String.IsNullOrEmpty(Array[1]) && !String.IsNullOrEmpty(Array[2]) && !String.IsNullOrEmpty(Array[3])) //Llega con fechas y con tipo
                {
                    DateTime FechaInicial = Convert.ToDateTime(Array[1]);
                    DateTime FechaFinal = Convert.ToDateTime(Array[2]);
                    String asd = Array[3];
                    //View.Model.RecordListAerodromo = _db.GetServicios(new Servicios
                    //{
                    //    Operacion = new Operacion
                    //    {
                    //        Llegada = new Llegada { TipoVuelo = new MMaster { Code = Array[3] } },
                    //        Aeronave = new Aeronaves { CompañiaFactura = new Terceros { RowID = AerolineaID } }
                    //    },
                    //    Status = new Status { Name = "ParaFacturar", StatusType = new StatusType { Name = "Servicios" } }
                    //    //Cuando filtro no tomo los servicios de tasas
                    //}).Where(f => f.Operacion.Salida.FechaSalida >= FechaInicial && f.Operacion.Salida.FechaSalida <= FechaFinal && f.Operacion.Tipo.Codigo != "CONTADO" && (f.Tipo.Codigo== "TASAS" || f.Tipo.Codigo== "TASASDEBITO" || f.Tipo.Codigo== "TASASCREDITO"))
                    //.ToList();
                    View.Model.RecordListAerodromo = _db.Servicios.Where(f => f.Operacion.Aeronave.CompañiaFacturaID == AerolineaID && f.Operacion.Llegada.Tipo5.Codigo == Array[3] && f.Estado.Nombre == "ParaFacturar" && f.Estado.Tipo.Nombre == "Servicios")
                        .Where(f => f.Operacion.Salida.FechaSalida >= FechaInicial && f.Operacion.Salida.FechaSalida <= FechaFinal && f.Operacion.Tipo.Codigo != "CONTADO" && (f.Tipo.Codigo == "TASAS" || f.Tipo.Codigo == "TASASDEBITO" || f.Tipo.Codigo == "TASASCREDITO"))
                    .ToList();

                }
                View.NombreAerolinea.Text = Array[4];
                View.TxtTotal.Text = View.Model.RecordListAerodromo.Sum(f => f.Valor).Value.ToString("N0");
            }
            View.Model.RecordListAerodromo = View.Model.RecordListAerodromo.OrderBy(f => f.Operacion.RowID).ToList();//Organizo por rowidOp
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
                ((FacturasAgrupadasPresenter)PresenterParent).View.TabPadre.Items.Remove(((TabItem)((FacturasAgrupadasPresenter)PresenterParent).View.TabPadre.SelectedItem));
                //Selecciono por defecto el Tab con el listado de facturas
                ((TabItem)((FacturasAgrupadasPresenter)PresenterParent).View.TabPadre.Items[0]).Focus();
            }
            catch (Exception)
            {}

            try
            {
                //Cierro el Tab seleccionado actualmente
                ((FacturasTasasPresenter)PresenterParent).View.TabPadre.Items.Remove(((TabItem)((FacturasTasasPresenter)PresenterParent).View.TabPadre.SelectedItem));
                //Selecciono por defecto el Tab con el listado de facturas
                ((TabItem)((FacturasTasasPresenter)PresenterParent).View.TabPadre.Items[0]).Focus();
            }
            catch (Exception)
            {}
        }

        public void OnAbrirModal(object sender, EventArgs e)
        {
            
        }

    }
}