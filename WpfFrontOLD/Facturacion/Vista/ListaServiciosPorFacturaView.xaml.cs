using System;
using System.Windows.Controls;
using Core.WPF;
using WpfFront.Model;
using WpfFront.Modelo;
using WpfFront.Controlador;
using System.Windows;

namespace WpfFront.Vista
{
    public partial class ListaServiciosPorFacturaView : UserControlBase, IListaServiciosPorFacturaView
    {

        #region Metodos
        public event EventHandler<EventArgs> CerrarTab;
        #endregion

        public ListaServiciosPorFacturaView()
        {
            InitializeComponent();
        }


        public ListaServiciosPorFacturaModel Model
        {
            get
            { return this.DataContext as ListaServiciosPorFacturaModel; }
            set
            { this.DataContext = value; }
        }
        public ModalInfoOperacionView modalGenerador
        {
            get { return this.modalGenerador_; }
            set { this.modalGenerador_ = value; }
        }

        public ListView ListaAgrupados
        {
            get { return this.dgLista_Registros; }
            set { this.dgLista_Registros = value; }
        }

        #region Variables
        public TextBlock FechaEmision
        {
            get { return this.txtFechaEmision; }
            set { this.txtFechaEmision = value; }
        }

        public TextBlock FechaInicial
        {
            get { return this.txtFechaInicial; }
            set { this.txtFechaInicial = value; }
        }

        public TextBlock FechaFinal
        {
            get { return this.txtFechaFinal; }
            set { this.txtFechaFinal = value; }
        }

        public TextBlock Aerolinea
        {
            get { return this.txtAerolinea; }
            set { this.txtAerolinea = value; }
        }

        public TextBlock Tipo
        {
            get { return this.txtTipo; }
            set { this.txtTipo = value; }
        }

        public TextBlock ValorTotal
        {
            get { return this.txtValorTotal; }
            set { this.txtValorTotal = value; }
        }

        public TextBlock CreadaPor
        {
            get { return this.txtCreadaPor; }
            set { this.txtCreadaPor = value; }
        }

        public TextBlock TxtTotal
        {
            get { return this.TxtTotalDetallado; }
            set { this.TxtTotalDetallado = value; }
        }

        #endregion

        #region ViewEvents
        private void btnCerrarTab_Click_1(object sender, RoutedEventArgs e)
        {
            CerrarTab(sender, e);
        }


        private void dgLista_Registros_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //QuitarEfectoGenerador(sender, e, this);
            modalGenerador = new ModalInfoOperacionView();

            if (ListaAgrupados.SelectedItem != null)
            {
                ModalInfoOperacionPresenter lo = new ModalInfoOperacionPresenter(null, modalGenerador, ((Servicios)(ListaAgrupados.SelectedItem)).Operacion);
            }
            //AplicarEfecto(this);
            modalGenerador.ShowDialog();
            bool? resultado = modalGenerador.DialogResult;
            if (resultado != null)
            {
                if (resultado == false)
                {
                    //  QuitarEfectoGenerador(sender, e, this);
                }
            }
        }

        ModalInfoOperacionView modalGenerador_ = new ModalInfoOperacionView();
        private void QuitarEfectoGenerador(object sender, EventArgs e, ListaServiciosPorAerolineaView DGView)
        {
            DGView.Effect = null;
        }
        private void AplicarEfecto(ListaServiciosPorAerolineaView DGView)
        {
            DGView.Effect = new System.Windows.Media.Effects.BlurEffect
            {
                Radius = 5.0
            };
        }

        private void QuitarEfecto(ListaServiciosPorAerolineaView DGView)
        {
            DGView.Effect = null;
        }
        #endregion

       


    }

    public interface IListaServiciosPorFacturaView
    {
        //Clase Modelo
        ListaServiciosPorFacturaModel Model { get; set; }

        //Definicion Variables
        TextBlock FechaEmision { get; set; }
        TextBlock FechaInicial { get; set; }
        TextBlock FechaFinal { get; set; }
        TextBlock Aerolinea { get; set; }
        TextBlock Tipo { get; set; }
        TextBlock ValorTotal { get; set; }
        TextBlock CreadaPor { get; set; }
        TextBlock TxtTotal { get; set; }
        //Eventos
        event EventHandler<EventArgs> CerrarTab;

    }
}