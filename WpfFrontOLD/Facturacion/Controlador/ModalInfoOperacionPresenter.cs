using System;
using WpfFront.Models;
using WpfFront.Views;
using Assergs.Windows;
using WMComposite.Events;
using Microsoft.Practices.Unity;
using WpfFront.WMSBusinessService;
using WpfFront.Common;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using WpfFront.Services;
using System.Linq;
using System.Data;
using System.Reflection;
using WpfFront.Common.WFUserControls;
using Microsoft.Windows.Controls;
using WpfFront.Common.Windows;
using System.Windows.Input;
using System.Data.OleDb;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using WpfFront.Views;

namespace WpfFront.Presenters
{

    public interface IModalInfoOperacionPresenter
    {
        IModalInfoOperacionView View { get; set; }
        ToolWindow Window { get; set; }
    }


    public class ModalInfoOperacionPresenter : IModalInfoOperacionPresenter
    {
        public IModalInfoOperacionView View { get; set; }
        private readonly IUnityContainer container;
        private readonly WMSServiceClient service;
        public ToolWindow Window { get; set; }

        //Variables Auxiliares 
        public Connection Local;


        public ModalInfoOperacionPresenter(IUnityContainer container, IModalInfoOperacionView view, Operacion Documento)
        {
            View = view;
            this.container = container;
            this.service = new WMSServiceClient();
            View.Model = new ModalInfoOperacionModel();

            view.Model.ListaTipoFacturacion = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOFACTURACION" }, Active = true });
            view.Model.ListaTipoPosicion = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOPOSICION" }, Active = true });
            view.Model.ListaTipoPosicionSalida = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOPOSICION" }, Active = true });
            view.Model.ListaTipoOperacion = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOOPERACION" }, Active = true }).OrderBy(f => f.NumOrder).ToList();
            view.Model.ListaTipoVuelo = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOVUELO" }, Active = true });
            view.Model.ListaTipoDeclaracion = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPODECLARACION" }, Active = true }).OrderBy(f => f.NumOrder).ToList();
            view.Model.ListaBanda = service.GetMMaster(new MMaster { MetaType = new MType { Code = "BANDA" }, Active = true });
            view.Model.ListaPosicion = service.GetMMaster(new MMaster { Active = true }).OrderBy(f => f.NumOrder).ToList();
            view.Model.ListaPosicionSalida = service.GetMMaster(new MMaster { Active = true }).OrderBy(f => f.NumOrder).ToList();
            view.Model.ListaTipoServicio = service.GetMMaster(new MMaster { MetaType = new MType { Code = "ServicioBomberos" }, Active = true });
            view.Model.ListaTipoTasas = service.GetMMaster(new MMaster { MetaType = new MType { Code = "TIPOTASA" }, Active = true }).Where(f => f.Code != "NORMAL").ToList();

            this.CargarDocumento(Documento);

            #region Datos
            //Cargo la variable para las consultas directas
            try { Local = service.GetConnection(new Connection { Name = "LOCAL" }).First(); }
            catch { }

            #endregion
        }


        public void CargarDocumento(Operacion Documento)
        {
            View.Model.Record = Documento;
            View.Model.RecordSalida = Documento.Salida == null ? new Salida() : Documento.Salida;
            View.Model.RecordLlegada = Documento.Llegada == null ? new Llegada() : Documento.Llegada;

            if (Documento.RowID != 0)
            {
                //Cargo los servicios bomberos que tenga
                try
                {
                    View.Model.RegistroBomberosList = service.GetBomberos(new Bomberos { Operacion = Documento, Activo = true }).Where(f => f.Status.StatusType.Name == "ServicioBomberos").ToList();
                }
                catch { }
                //Cargo los servicios bomberos Adicionales que tenga
                try
                { View.Model.RegistroBomberosAdicionalesList = service.GetBomberos(new Bomberos { Operacion = Documento, Activo = true }).Where(f => f.Status.StatusType.Name == "ServicioBomberosAdicional").ToList(); }
                catch { }
                //Cargo las Tasas
                try
                { View.Model.RegistroTasasList = service.GetTasas(new Tasas { Operacion = Documento }); }
                catch { }
                //Cargo las Tasas Adicionales
                try
                { View.Model.RegistroTasasAdicionalesList = service.GetTasas(new Tasas { Operacion = Documento }).Where(f => f.TipoTasa.Code != "NORMAL").ToList(); }
                catch { }
                //Cargo los datos de la pestaña liquidacion 
                try
                { View.Model.RecordTasas = service.GetTasas(new Tasas { Operacion = Documento }).First(); }
                catch { }
                //Cargo la lista de factura Agrupadas
                try
                { 
                    //getListaFacturasAgrupadas(); 
                }
                catch { }

                //Cargo Adicionales
                try
                { View.Model.RegistroAdicionalesPyPList = service.GetAdicionalesPyP(new AdicionalesPyP { Operacion = Documento }); }
                catch { }
                if (Documento.Llegada != null)
                {
                    //Cargo los comboBox de tipo de posicion y posicion de llegada y salida
                    if (Documento.Llegada.RowID != 0)
                    {
                        //Si tiene llegada habilito el boton de confirmar
                        //Cargo la compania Factura
                        if (Documento.Llegada.CIAFactura != null)
                        {
                            View.CompañiaFactura.Terceros = Documento.Llegada.CIAFactura;
                            View.CompañiaFactura.txtDescripcion.Text = Documento.Llegada.CIAFactura.NombreCompleto;
                            View.CompañiaFactura.txtData.Text = Documento.Llegada.CIAFactura.NombreCompleto;
                        }
                        //Cargo los comboBox de tipo vuelo llegada 
                        View.TipoFactura.SelectedValue = Documento.TipoFacturacion != null ? Documento.TipoFacturacion.MetaMasterID : -1;
                        View.TipoVuelo.SelectedValue = Documento.Llegada.TipoVuelo != null ? Documento.Llegada.TipoVuelo.MetaMasterID : -1;
                        View.TipoDeclaracion.SelectedValue = Documento.Llegada.TipoDeclaracion != null ? Documento.Llegada.TipoDeclaracion.MetaMasterID : -1;
                        View.TipoPosicion.SelectedValue = Documento.Llegada.TipoPosicion != null ? Documento.Llegada.TipoPosicion.MetaMasterID : -1;
                        View.Posicion.SelectedValue = Documento.Llegada.Posicion != null ? Documento.Llegada.Posicion.MetaMasterID : -1;
                        View.PosicionSalida.SelectedValue = Documento.Salida.PosicionSalida != null ? Documento.Salida.PosicionSalida.MetaMasterID : -1;

                        View.EstadoVuelo.Text = CalcularEstadoLlegadaYSalida(Documento.Llegada.HoraAterrizaje, Documento.Llegada.FechaAterrizaje, Documento.Llegada.HoraProgramadaLlegada, Documento.Llegada.FechaProgramacion);
                        View.EstadoVueloSalida.Text = CalcularEstadoLlegadaYSalida(Documento.Salida.HoraDespegue, Documento.Salida.FechaDespegue, Documento.Salida.HoraProgramadaSalida, Documento.Salida.FechaProgramacion);
                        
                        //Cargo NVueloLlegada
                        View.NumVuelo.Text = Documento.Llegada.NVuelo;
                        //Cargo los datos de los controles y comboBox si estan seteados
                        if (Documento.Llegada.Origen != null)
                        {
                            View.Origen.cargarValorEspecifico(Documento.Llegada.Origen.Ubicacion);
                            View.Origen.Aeropuertos = Documento.Llegada.Origen;
                        }
                        if (Documento.Llegada.TipoPosicion != null)
                        {
                            if (Documento.Llegada.TipoPosicion.Code != "HANGAR")
                            {
                                View.Posicion.SelectedValue = Documento.Llegada.Posicion != null ? Documento.Llegada.Posicion.MetaMasterID : -1;
                            }
                        }

                    }

                }


                if (Documento.Salida != null)
                {
                    if (Documento.Salida.Destino != null)
                    {
                        View.Destino.cargarValorEspecifico(Documento.Salida.Destino.Ubicacion);
                        View.Destino.Aeropuertos = Documento.Salida.Destino;
                    }
                    if (Documento.Salida.RowID != 0)
                    {
                        //Si tiene llegada habilito el boton de confirmar
                        if (Documento.Salida.FechaSalida != null && Documento.Salida.TipoVueloSalida != null)
                        {
                            try
                            {
                                this.OnCalcularValoresLiquidacion();
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (Documento.Salida.TipoPosicionSalida != null)
                        {
                            if (Documento.Salida.TipoPosicionSalida.Code != "HANGAR")
                            {
                                View.PosicionSalida.SelectedValue = Documento.Salida.PosicionSalida != null ? Documento.Salida.PosicionSalida.MetaMasterID : -1;
                            }
                        }
                        View.TipoPosicionSalida.SelectedValue = Documento.Salida.TipoPosicionSalida != null ? Documento.Salida.TipoPosicionSalida.MetaMasterID : -1;
                        View.TipoVueloSalida.SelectedValue = Documento.Salida.TipoVueloSalida != null ? Documento.Salida.TipoVueloSalida.MetaMasterID : -1;

                    }

                }
                if (Documento.Aeronave != null)
                {
                    View.Aeronave.cargarValorEspecifico(Documento.Aeronave.Matricula, Documento.Aeronave.PBMOKG + "Kg - " + Documento.Aeronave.TipoAeronave + " - " + Documento.Aeronave.CapacidadPasajeros + "Pax");
                    View.Aeronave.Aeronaves = Documento.Aeronave;
                }
               
                try 
	{	        
		this.cargarInfoDeFacturacion(Documento);
	}
	catch (Exception)
	{
		
	}
            }
           
        }

        public string CalcularEstadoLlegadaYSalida(String HoraAterrizaje, DateTime? FechaAterrizaje, String HoraProgramacion, DateTime? FechaProgramacion)
        {
            String Estado = "";
            if (!HoraAterrizaje.Contains("_") && FechaAterrizaje != null && !HoraProgramacion.Contains("_") && FechaProgramacion != null)
            {
                //Si la fecha de Programacion y de aterrizaje son las mismas (Es el mismo dia)
                if (FechaProgramacion == FechaAterrizaje)
                {
                    TimeSpan TNow = DateTime.Parse(HoraProgramacion) - DateTime.Parse(HoraAterrizaje);
                    if (TNow.TotalMinutes < -15)
                    { Estado = "D"; }
                    else if (TNow.TotalMinutes > 15)
                    { Estado = "A"; }
                    else
                    { Estado = "AT"; }
                }
                //Si la fecha de aterrizaje es mayor a la de operacion esta Demorado
                else if (FechaAterrizaje > FechaProgramacion)
                {
                    if (Int32.Parse(HoraProgramacion.Substring(0, 2)) == 23 && Int32.Parse(HoraAterrizaje.Substring(0, 2)) == 0)
                    {
                        Double minutos;
                        TimeSpan TNow2 = DateTime.Parse(HoraProgramacion) - DateTime.Parse(HoraAterrizaje);
                        minutos = TNow2.TotalMinutes;
                        minutos = minutos - 1440;
                        if (minutos < -15)
                        { Estado = "D"; }
                        else if (minutos > 15)
                        { Estado = "A"; }
                        else
                        { Estado = "AT"; }
                    }
                    else
                    { Estado = "D"; }
                }

                //Si la fecha de aterrizaje es menor a la de operacion esta adelantado
                else
                {
                    if (Int32.Parse(HoraProgramacion.Substring(0, 2)) == 0 && Int32.Parse(HoraAterrizaje.Substring(0, 2)) == 23)
                    {
                        Double minutos;
                        TimeSpan TNow2 = DateTime.Parse(HoraProgramacion) - DateTime.Parse(HoraAterrizaje);
                        minutos = TNow2.TotalMinutes;
                        minutos = minutos + 1440;
                        if (minutos < -15)
                        { Estado = "D"; }
                        else if (minutos > 15)
                        { Estado = "A"; }
                        else
                        { Estado = "AT"; }
                    }
                    else
                    { Estado = "A"; }
                }
            }

            return Estado;
        }
        public void OnCalcularValoresLiquidacion()
        {
            if (View.Model.RecordSalida.RowID != 0)
            {
                //Para calcular info de pestaña liquidacion
                Tarifas Tarifa;
                if (View.Model.RecordSalida.FechaSalida == null || View.Model.RecordSalida.TipoVueloSalida == null)
                {
                    return;
                }
                //Buscar tarifa aerodromo que coincida con  la fecha Salida y que sea de tipo TASAS
                if (service.GetTarifas(new Tarifas { FechaFiltro = View.Model.RecordSalida.FechaSalida, TipoTarifa = new MMaster { Code = "TASAS" } }).Count() != 0)
                {
                    Tarifa = service.GetTarifas(new Tarifas { FechaFiltro = DateTime.Parse(View.Model.RecordSalida.FechaSalida.ToString()), TipoTarifa = new MMaster { Code = "TASAS" } }).First();
                    if (View.Model.RecordSalida.TipoVueloSalida.Code == "NACIONAL")
                    {
                        View.Model.RecordTasas.TasaCOP = Tarifa.ValorCOP;
                    }
                    else
                    {
                        View.Model.RecordTasas.TasaUSD = Tarifa.ValorUSD;
                        View.Model.RecordTasas.TasaCOP = Tarifa.ValorUSD;
                    }
                }
                else
                //Si no tiene una tarifa de Aerodromo le dejo 0 por defecto
                {
                    Util.ShowError("No cuenta con una tarifa Tasas disponible");
                    View.Model.RecordTasas.TasaCOP = 0;
                    View.Model.RecordTasas.TasaUSD = 0;
                }
                //Sumo los exentos de todos los registros de tasas diferentes a CREDITO
                View.Model.RecordTasas.Exentos = service.GetTasas(new Tasas { Operacion = View.Model.Record })
                                                    .Where(f => f.TipoTasa.Code != "CREDITO")
                                                    .Sum(f => f.Transitos + f.Tripulantes + f.Infantes + f.Otros + f.Militares);
                //Resto los CREDITO
                View.Model.RecordTasas.Exentos = View.Model.RecordTasas.Exentos - service.GetTasas(new Tasas { Operacion = View.Model.Record })
                                                    .Where(f => f.TipoTasa.Code == "CREDITO")
                                                    .Sum(f => f.Transitos + f.Tripulantes + f.Infantes + f.Otros + f.Militares);
                //Sumo todos los registros de pagan tasa, que son diferentes a CREDITO
                View.Model.RecordTasas.PaganTasa = service.GetTasas(new Tasas { Operacion = View.Model.Record })
                                                    .Where(f => f.TipoTasa.Code != "CREDITO").Sum(f => f.PaganTasa);
                //Resto los CREDITO
                View.Model.RecordTasas.PaganTasa = View.Model.RecordTasas.PaganTasa - service.GetTasas(new Tasas { Operacion = View.Model.Record })
                                                    .Where(f => f.TipoTasa.Code == "CREDITO").Sum(f => f.PaganTasa);

                View.Model.RecordTasas.PasajerosEmbarcados =
                View.Model.RecordTasas.PaganTasa +
                View.Model.RecordTasas.Exentos;

                if (View.Model.RecordSalida.TipoVueloSalida.Code == "NACIONAL")
                {
                    View.Model.RecordTasas.TotalCOP =
                    View.Model.RecordTasas.PaganTasa *
                    View.Model.RecordTasas.TasaCOP;
                }
                else
                {
                    TRM Trm;
                    if (service.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).Count == 1)
                    {
                        //Asigno la TRM vigente
                        Trm = service.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).First();

                        View.Model.RecordTasas.TotalUSD =
                        View.Model.RecordTasas.PaganTasa * Trm.Valor *
                        View.Model.RecordTasas.TasaUSD;
                        //Para que se vea en pantalla
                        View.Model.RecordTasas.TotalCOP = View.Model.RecordTasas.TotalUSD;

                    }
                    else
                    { Util.ShowError("No cuenta con una TRM disponible."); }
                }
            }
            else
            {
                Util.ShowError("No ha registrado un tipo de vuelo de salida");
            }
        }

         public void cargarInfoDeFacturacion(Operacion operacion)
        {
             //Recorro los servicios que no estan anulados
            IList<Servicios> ListaServicios = service.GetServicios(new Servicios { Operacion = operacion, Status = new Status { StatusType = new StatusType { Name = "Servicios" } } }).Where(f=>f.Status.Name != "Anulada").ToList();//Validar Estado
            foreach (Servicios servicioDet in ListaServicios)
            {
                switch (servicioDet.TipoServicio.Code)
                {
                    case "AERODROMO": View.TotalAerodromo.Text = servicioDet.Valor.ToString("N0");
                        break;
                    case "TASAS": View.CantTasas.Text = servicioDet.Cantidad.ToString();
                        View.ValorTasas.Text = servicioDet.Valor.ToString("N0");
                        break;
                    case "RECARGONOC": View.RecargoNocturno.Text = servicioDet.Valor.ToString("N0");
                        break;
                    case "PUENTES": View.NumPuentes.Text = servicioDet.Cantidad.ToString();
                        View.TotalPuente.Text = servicioDet.Valor.ToString("N0");
                        break;
                    case "PARQUEO": View.CantHorasParqueo.Text = servicioDet.Cantidad.ToString();
                        View.ValorParqueo.Text = servicioDet.Valor.ToString("N0");
                        break;
                    case "ASISTENCIA":
                        View.CantServBomberos.Text= String.IsNullOrEmpty(View.CantServBomberos.Text)?"0": View.CantServBomberos.Text;
                        View.ValorServBomberos.Text = String.IsNullOrEmpty(View.ValorServBomberos.Text) ? "0" : View.ValorServBomberos.Text;
                        View.CantServBomberos.Text = (Int32.Parse(View.CantServBomberos.Text) + servicioDet.Cantidad) + "";
                        View.ValorServBomberos.Text = (Double.Parse(View.ValorServBomberos.Text) + servicioDet.Valor).ToString("N0");
                        break;
                    case "LIMPIEZA":
                        View.CantServBomberos.Text= String.IsNullOrEmpty(View.CantServBomberos.Text)?"0": View.CantServBomberos.Text;
                        View.ValorServBomberos.Text = String.IsNullOrEmpty(View.ValorServBomberos.Text) ? "0" : View.ValorServBomberos.Text;
                        View.CantServBomberos.Text = Int32.Parse(View.CantServBomberos.Text) + servicioDet.Cantidad + "";
                        View.ValorServBomberos.Text = (Double.Parse(View.ValorServBomberos.Text) + servicioDet.Valor).ToString("N0");
                        break;
                }
                this.calcularTotalFacturacion();
            }
        }

         public void calcularTotalFacturacion()
         {
             try
             {
                 TRM trmConvertir = service.GetTRM(new TRM { FechaFiltro = View.Model.Record.Salida.FechaSalida }).First();
                 // calculo el total
                 if (!string.IsNullOrEmpty(View.TotalAerodromo.Text))
                 {
                     View.TotalFacturacionContado.Text = (Double.Parse(View.TotalAerodromo.Text)).ToString("N0");
                     View.ValorAerodromoUSD.Text = (Double.Parse(View.TotalAerodromo.Text) / trmConvertir.Valor).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                 }
                 if (!string.IsNullOrEmpty(View.RecargoNocturno.Text))
                 {
                     View.TotalFacturacionContado.Text = (Double.Parse(View.TotalFacturacionContado.Text) + Double.Parse(View.RecargoNocturno.Text)).ToString("N0");
                     View.ValorRecargoUSD.Text = (Double.Parse(View.RecargoNocturno.Text) / trmConvertir.Valor).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                 }
                 if (!string.IsNullOrEmpty(View.TotalPuente.Text))
                 {
                     View.TotalFacturacionContado.Text = (Double.Parse(View.TotalFacturacionContado.Text) + Double.Parse(View.TotalPuente.Text)).ToString("N0");
                     View.ValorPuentesUSD.Text = (Double.Parse(View.TotalPuente.Text) / trmConvertir.Valor).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                 }
                 if (!string.IsNullOrEmpty(View.ValorServBomberos.Text))
                 {
                     View.TotalFacturacionContado.Text = (Double.Parse(View.TotalFacturacionContado.Text) + Double.Parse(View.ValorServBomberos.Text)).ToString("N0");
                     View.ValorBomberosUSD.Text = (Double.Parse(View.ValorServBomberos.Text) / trmConvertir.Valor).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                 }
                 if (!string.IsNullOrEmpty(View.ValorTasas.Text))
                 {
                     View.TotalFacturacionContado.Text = (Double.Parse(View.TotalFacturacionContado.Text) + Double.Parse(View.ValorTasas.Text)).ToString("N0");
                     View.ValorTasasUSD.Text = (Double.Parse(View.ValorTasas.Text) / trmConvertir.Valor).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                 }
                 if (!string.IsNullOrEmpty(View.ValorParqueo.Text))
                 {
                     View.TotalFacturacionContado.Text = (Double.Parse(View.TotalFacturacionContado.Text) + Double.Parse(View.ValorParqueo.Text)).ToString("N0");
                     View.ValorParqueoUSD.Text = (Double.Parse(View.ValorParqueo.Text) / trmConvertir.Valor).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                 }
                 if (!string.IsNullOrEmpty(View.TotalFacturacionContado.Text))
                 {
                     View.ValorTotalUSD.Text = (Double.Parse(View.TotalFacturacionContado.Text) / trmConvertir.Valor).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) + " USD";
                 }
             }
             catch (Exception excp) { }
         }

    }
}