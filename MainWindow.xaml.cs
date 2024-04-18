using DTCWaitingList.Interface;
using DTCWaitingList.Models;
using DTCWaitingList.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DTCWaitingList
{
    public partial class MainWindow : Window
    {
        private readonly IDataAccessService? _data;
        public IEnumerable<AppointmentView>? Results { get; set; }

        public MainWindow(IDataAccessService data)
        {
            _data = data;

            InitializeComponent();
            Results = _data.GetAppointments(null);
            listView.ItemsSource = Results;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void OrdenarNomeAscendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Nome", ListSortDirection.Ascending));
        }

        private void OrdenarNomeDescendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Nome", ListSortDirection.Descending));
        }

        private void OrdenarApelidoAscendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Apelido", ListSortDirection.Ascending));
        }

        private void OrdenarApelidoDescendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Apelido", ListSortDirection.Descending));
        }

        private void RemoverPaciente_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Appointment paciente = button.DataContext as Appointment;
            //MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja remover /*{paciente.Nome} {paciente.Apelido}*/ da lista?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
            //    //Results.Remove(paciente);
            //}
        }

        private void Procurar_Click(object sender, RoutedEventArgs e)
        {
            var resultadosFiltrados = new List<AppointmentView>();
            resultadosFiltrados = (List<AppointmentView>)Results;
            if (cmbDiasDisponiveis.SelectedItem != null ||
                cmbHoraDisponivel.SelectedItem != null ||
                cmbTipoConsulta.SelectedItem != null)
            {
                resultadosFiltrados = (List<AppointmentView>)Results;
            }
            listView.ItemsSource = resultadosFiltrados;
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AdicionarPaciente_Click(object sender, RoutedEventArgs e)
        {
            AppointmentView novoPaciente = new AppointmentView
            {
                //Nome = txtNome.Text,
                //Apelido = txtApelido.Text,
                //Email = txtEmail.Text,
                //Telefone = txtTelefone.Text,
                //DiasDisponiveis = cmbDiasDisponiveis.SelectedItem.ToString(),
                //HorasDisponíveis = cmbHorasDisponiveis.SelectedItem.ToString(),
                //TipoConsulta = cmbTipoConsulta.SelectedItem.ToString(),
                //NovoPaciente = chkNovoPaciente.IsChecked ?? false,
                //DataEntradaFilaEspera = DateTime.Now
            };

            Results.Append(novoPaciente);

            // Limpar os campos após adicionar o paciente
            LimparCamposAdicaoPaciente();
        }

        private void LimparCamposAdicaoPaciente()
        {
            txtNome.Text = "";
            txtApelido.Text = "";
            txtEmail.Text = "";
            txtTelefone.Text = "";
            cmbDiasDisponiveis.SelectedIndex = -1;
            cmbHorasDisponiveis.SelectedIndex = -1;
            cmbTipoConsulta.SelectedIndex = -1;
            chkNovoPaciente.IsChecked = false;
        }

        private void OrdenarEmailAscendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Email", ListSortDirection.Ascending));
        }


        private void OrdenarEmailDescendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Email", ListSortDirection.Descending));
        }

        private void OrdenarTelefoneAscendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Telefone", ListSortDirection.Ascending));
        }

        private void OrdenarTelefoneDescendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Telefone", ListSortDirection.Descending));
        }


        private void OrdenarDiasDisponiveisAscendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("DiasDisponiveis", ListSortDirection.Ascending));
        }

        private void OrdenarDiasDisponiveisDescendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("DiasDisponiveis", ListSortDirection.Descending));
        }


        private void OrdenarHorasDisponiveisAscendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("HorasDisponíveis", ListSortDirection.Ascending));
        }


        private void OrdenarHorasDisponiveisDescendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("HorasDisponíveis", ListSortDirection.Descending));
        }

        private void OrdenarTipoConsultaAscendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("TipoConsulta", ListSortDirection.Ascending));
        }


        private void OrdenarTipoConsultaDescendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("TipoConsulta", ListSortDirection.Descending));
        }

        private void OrdenarDataEntradaFilaEsperaDescendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("DataEntradaFilaEspera", ListSortDirection.Descending));
        }

        private void OrdenarNovoPacienteDescendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("NovoPaciente", ListSortDirection.Descending));
        }


        private void OrdenarNovoPacienteAscendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("NovoPaciente", ListSortDirection.Ascending));
        }

        private void OrdenarDataEntradaFilaEsperaAscendente_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("DataEntradaFilaEspera", ListSortDirection.Ascending));
        }




    }
}
