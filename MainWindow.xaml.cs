using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;

namespace DTCWaitingList
{
    public partial class PacientesEmListaDeEspera : Window
    {
        private NotifyIcon notifyIcon;

        public class Paciente
        {
            public string Nome { get; set; }
            public string Apelido { get; set; }
            public string Email { get; set; }
            public string Telefone { get; set; }
            public string DiasDisponiveis { get; set; }
            public string HorasDisponíveis { get; set; }
            public string TipoConsulta { get; set; }
            public bool NovoPaciente { get; set; }
            public DateTime DataEntradaFilaEspera { get; set; }
        }

        public ObservableCollection<Paciente> Resultados { get; set; }

        public PacientesEmListaDeEspera()
        {
            InitializeComponent();
            Resultados = new ObservableCollection<Paciente>();
            MockResultados();
            InitializeNotifyIcon();
            listView.ItemsSource = Resultados;
            DataContext = this;
        }

        private void MockResultados()
        {
            Resultados.Add(new Paciente
            {
                Nome = "João",
                Apelido = "Silva",
                Email = "joao.silva@example.com",
                Telefone = "123456789",
                DiasDisponiveis = "Segunda-feira, Terça-feira",
                TipoConsulta = "Checkup",
                NovoPaciente = true,
                DataEntradaFilaEspera = DateTime.Now.AddDays(-5)
            });
            Resultados.Add(new Paciente
            {
                Nome = "Maria",
                Apelido = "Santos",
                Email = "maria.santos@example.com",
                Telefone = "987654321",
                DiasDisponiveis = "Quarta-feira, Sexta-feira",
                TipoConsulta = "Cleaning",
                NovoPaciente = false,
                DataEntradaFilaEspera = DateTime.Now.AddDays(-7)
            });
            Resultados.Add(new Paciente
            {
                Nome = "Carlos",
                Apelido = "Oliveira",
                Email = "carlos.oliveira@example.com",
                Telefone = "567891234",
                DiasDisponiveis = "Segunda-feira, Quarta-feira",
                HorasDisponíveis = "Manhã",
                TipoConsulta = "Checkup",
                NovoPaciente = true,
                DataEntradaFilaEspera = DateTime.Now.AddDays(-10)
            });
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
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            Paciente paciente = button.DataContext as Paciente;
            MessageBoxResult result = System.Windows.MessageBox.Show($"Tem certeza que deseja remover {paciente.Nome} {paciente.Apelido} da lista?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Resultados.Remove(paciente);
            }
        }

        private void Procurar_Click(object sender, RoutedEventArgs e)
        {
            var resultadosFiltrados = new ObservableCollection<Paciente>();
            resultadosFiltrados = Resultados;
            if (cmbDiasDisponiveis.SelectedItem != null ||
                cmbHoraDisponivel.SelectedItem != null ||
                cmbTipoConsulta.SelectedItem != null)
            {
                resultadosFiltrados = Resultados;
            }
            listView.ItemsSource = resultadosFiltrados;
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AdicionarPaciente_Click(object sender, RoutedEventArgs e)
        {
            Paciente novoPaciente = new Paciente
            {
                Nome = txtNome.Text,
                Apelido = txtApelido.Text,
                Email = txtEmail.Text,
                Telefone = txtTelefone.Text,
                DiasDisponiveis = cmbDiasDisponiveis.SelectedItem.ToString(),
                HorasDisponíveis = cmbHorasDisponiveis.SelectedItem.ToString(),
                TipoConsulta = cmbTipoConsulta.SelectedItem.ToString(),
                NovoPaciente = chkNovoPaciente.IsChecked ?? false,
                DataEntradaFilaEspera = DateTime.Now
            };

            Resultados.Add(novoPaciente);

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


        private void InitializeNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon("agenda.ico"); // Especifique o caminho para o ícone
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
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipTitle = "Aplicação Minimizada";
            notifyIcon.BalloonTipText = "Sua aplicação foi minimizada para a bandeja.";
            notifyIcon.ShowBalloonTip(3000);
        }
    }


}

