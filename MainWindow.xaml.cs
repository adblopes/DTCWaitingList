﻿using DTCWaitingList.Interface;
using DTCWaitingList.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;

namespace DTCWaitingList
{
    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon;

        public class Paciente
        private readonly IDataAccessService? _data;
        public IEnumerable<Patient>? Results { get; set; }

        public MainWindow(IDataAccessService data)
        {
            _data = data;
            InitializeMainWindow();
        }

        private async void InitializeMainWindow()
            InitializeNotifyIcon();
            await SetResultsAsync();
            DataContext = this;
        }

    private async Task SetResultsAsync()
    {
        Results = _data!.GetPatients();

        // prevent different thread exception by blocking calling thread
        Dispatcher.Invoke(() =>
        {
            listView.ItemsSource = Results;
        });
    }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
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
            Button button = sender as Button;
            Patient paciente = button.DataContext as Patient;
            //MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja remover /*{paciente.Nome} {paciente.Apelido}*/ da lista?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
            //    //Results.Remove(paciente);
            //}
            MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja remover {paciente.Nome} {paciente.Apelido} da lista?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Resultados.Remove(paciente);
            }
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
            Patient novoPaciente = new Patient
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

