using System;
using System.Windows.Controls;
using Core.WPF;
using WpfFront.Model;
using WpfFront.Modelo;
using System.Windows;

namespace WpfFront.Vista
{
    public partial class ListaServiciosPorAerolineaView : UserControlBase, IListaServiciosPorAerolineaView
    {

        #region Metodos
        public event EventHandler<EventArgs> CerrarTab;
        public event EventHandler<EventArgs> AbrirModal;

        
        #endregion

        public ListaServiciosPorAerolineaView()
        {
            InitializeComponent();
        }


        public ListaServiciosPorAerolineaModel Model
        {
            get
            { return this.DataContext as ListaServiciosPorAerolineaModel; }
            set
            { this.DataContext = value; }
        }
        #region Variables


        public TextBlock NombreAerolinea
        {
            get { return this.txtAerolinea; }
            set { this.txtAerolinea = value; }
        }

        public TextBlock TxtTotal
        {
            get { return this.TxtTotalDetallado; }
            set { this.TxtTotalDetallado = value; }
        }


        public ListView ListaAgrupados
        {
            get { return this.dgLista_Registros; }
            set { this.dgLista_Registros = value; }
        }
             
             


        public ModalInfoOperacionView modalGenerador
        {
            get { return this.modalGenerador_; }
            set { this.modalGenerador_ = value; }
        }

        #endregion

        #region ViewEvents
        private void btnCerrarTab_Click_1(object sender, RoutedEventArgs e)
        {
            CerrarTab(sender, e);
        }
       



        #endregion

        private void dgLista_Registros_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            QuitarEfectoGenerador(sender, e, this);
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




    }

    public interface IListaServiciosPorAerolineaView
    {
        //Clase Modelo
        ListaServiciosPorAerolineaModel Model { get; set; }

        //Definicion Variables
        TextBlock NombreAerolinea { get; set; }
        TextBlock TxtTotal { get; set; }

        ModalInfoOperacionView modalGenerador { get; set; }
        ListView ListaAgrupados { get; set; }

        //Eventos
        event EventHandler<EventArgs> CerrarTab;
        event EventHandler<EventArgs> AbrirModal;

       

    }
}