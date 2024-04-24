using DTCWaitingList.Database.Models;
using DTCWaitingList.Interfaces;
using DTCWaitingList.Models;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Reflection;
using System.Windows;
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

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            notifyIcon!.Visible = true;
            notifyIcon!.BalloonTipTitle = "Aplicação Minimizada";
            notifyIcon!.BalloonTipText = "Sua aplicação foi minimizada para a bandeja.";
            notifyIcon!.ShowBalloonTip(3000);
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var columnHeader = e.OriginalSource as GridViewColumnHeader;
            if (columnHeader != null)
            {
                // Clear existing sorting
                foreach (var column in gridView.Columns)
                {
                    column.HeaderTemplate = null;
                }

                // Determine the current sort direction
                ListSortDirection newSortDirection;
                if (columnHeader.Tag == null)
                {
                    newSortDirection = ListSortDirection.Ascending;
                    columnHeader.Tag = newSortDirection;
                }
                else
                {
                    newSortDirection = (ListSortDirection)columnHeader.Tag;
                    columnHeader.Tag = newSortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }

                // Apply sorting to the column
                var propertyName = (columnHeader.Column.DisplayMemberBinding as System.Windows.Data.Binding)?.Path.Path;
                if (!string.IsNullOrEmpty(propertyName))
                {
                    var view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(propertyName, newSortDirection));
                }

                // Set the sort arrow icon
                columnHeader.ContentTemplate = Resources[newSortDirection == ListSortDirection.Ascending ? "SortAscendingTemplate" : "SortDescendingTemplate"] as DataTemplate;
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

        private void AdicionarPaciente_Click(object sender, RoutedEventArgs e)
        {         
            //Results.Append(novoPaciente);

            // Limpar os campos após adicionar o paciente
            LimparCamposAdicaoPaciente();
        }

        private void RemoverPaciente_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            PatientView patient = button.DataContext as PatientView;
            MessageBoxResult result = System.Windows.MessageBox.Show($"Tem certeza que deseja remover {patient.FullName} da lista?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                List<PatientView> patients = Results.ToList();
                patients.Remove(patient);
                listView.ItemsSource = patients;
            }
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
    }
}

