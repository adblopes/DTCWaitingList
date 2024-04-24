using DTCWaitingList.Interfaces;
using DTCWaitingList.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Reflection;
using System.IO;

namespace DTCWaitingList
{
    public partial class MainWindow : Window
    {
        private readonly IDataAccessService? _data;
        public IEnumerable<PatientView>? Results { get; set; }
        public NotifyIcon? notifyIcon { get; set; }

        public MainWindow(IDataAccessService data)
        {
            _data = data;
            InitializeMainWindow();
        }

        private void InitializeMainWindow()
        { 
            InitializeNotifyIcon();
            InitializeComponent();
            SetResults();
        }

        private void SetResults()
        {
            Results = _data!.GetPatients();

            listView.ItemsSource = Results;
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
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            Patient paciente = button.DataContext as Patient;
            //MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja remover /*{paciente.Nome} {paciente.Apelido}*/ da lista?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
            //    //Results.Remove(paciente);
            //}
            //MessageBoxResult result = System.Windows.MessageBox.Show($"Tem certeza que deseja remover {paciente.Nome} {paciente.Apelido} da lista?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
            //    Resultados.Remove(paciente);
            //}
        }

        private void Procurar_Click(object sender, RoutedEventArgs e)
        {
            var resultadosFiltrados = new List<Patient>();
            resultadosFiltrados = (List<Patient>)Results;
            if (cmbDiasDisponiveis.SelectedItem != null ||
                cmbHoraDisponivel.SelectedItem != null ||
                cmbTipoConsulta.SelectedItem != null)
            {
                resultadosFiltrados = (List<Patient>)Results;
            }
            listView.ItemsSource = resultadosFiltrados;
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AdicionarPaciente_Click(object sender, RoutedEventArgs e)
        {
            PatientView novoPaciente = new PatientView
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

        private void RemoverPaciente_Click(object sender, RoutedEventArgs e)
        {

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


        private void InitializeNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, @"Resources\agenda.ico"));
            notifyIcon.Visible = false;
            notifyIcon.DoubleClick += (s, args) =>
            {
                this.Show();
                this.WindowState = WindowState.Normal;
                notifyIcon.Visible = false;
            };
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            notifyIcon!.Visible = true;
            notifyIcon!.BalloonTipTitle = "Aplicação Minimizada";
            notifyIcon!.BalloonTipText = "Sua aplicação foi minimizada para a bandeja.";
            notifyIcon!.ShowBalloonTip(3000);
        }
    }
}

