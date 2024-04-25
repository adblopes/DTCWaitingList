using DTCWaitingList.Interfaces;
using DTCWaitingList.Models;
using System.ComponentModel;
using System.Windows.Controls;
using DTCWaitingList.Views;
using System.Windows.Forms;
using System.Windows.Data;
using System.Reflection;
using Wpf.Ui.Controls;
using System.Windows;
using System.IO;
using MessageBox = Wpf.Ui.Controls.MessageBox;
using Button = System.Windows.Controls.Button;
using MessageBoxResult = Wpf.Ui.Controls.MessageBoxResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DTCWaitingList
{
    public partial class MainWindow : Window
    {
        private readonly IDataAccessService? _data;
        public IEnumerable<PatientView>? Results { get; set; }
        public NotifyIcon? notifyIcon { get; set; }

        public MainWindow()
        {
            _data = App.Current.Services.GetService<IDataAccessService>();
            InitializeMainWindow();
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

        private void InitializeMainWindow()
        {
            InitializeNotifyIcon();
            InitializeComponent();
            PopulateComboBoxes();
            SetResults();
        }

        private void SetResults()
        {
            Results = _data!.GetPatients();
            listView.ItemsSource = Results;
        }

        private void PopulateComboBoxes()
        {
            var days = _data!.GetDays();
            var times = _data!.GetTimes();
            var types = _data!.GetReasons();

            cmbDays.ItemsSource = days;
            lb2Days.ItemsSource = days;
            cmbTimes.ItemsSource = times;
            lb2Times.ItemsSource = times;
            cmbTypes.ItemsSource = types;
            cmb2Types.ItemsSource = types;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            SortColumns(e.OriginalSource as GridViewColumnHeader);
        }

        private void SearchPatients_Click(object sender, RoutedEventArgs e)
        {
            //var resultadosFiltrados = new List<Patient>();
            //resultadosFiltrados = (List<Patient>)Results;
            //if (cmbDiasDisponiveis.SelectedItem != null ||
            //    cmbHoraDisponivel.SelectedItem != null ||
            //    cmbTipoConsulta.SelectedItem != null)
            //{
            //    resultadosFiltrados = (List<Patient>)Results;
            //}
            //listView.ItemsSource = resultadosFiltrados;
        }

        private void AddPacient_Click(object sender, RoutedEventArgs e)
        {
            var newPatient = new PatientView()
            {
                FullName = txtFullName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                PatientDays = (IList<string>)lb2Days.SelectedItems,
                PatientTimes = (IList<string>)lb2Times.SelectedItems,
                Reason = (string)cmb2Types.SelectedItem,
                IsClient = chkNewPatient.IsChecked!.Value,
            };

            Results!.Append(newPatient);
            ClearAddPatientFields();
        }

        private async void RemovePatient_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            PatientView patient = (PatientView)button!.DataContext;

            await RemovePatientMessageBox(patient);
        }

        private async Task RemovePatientMessageBox(PatientView patient)
        {
            object content = $"Are you sure you want to remove {patient.FullName} from the waiting list?";
            var messageBox = new MessageBox()
            {
                IsPrimaryButtonEnabled = true,
                PrimaryButtonAppearance = ControlAppearance.Info,
                PrimaryButtonText = "REMOVE",
                CloseButtonText = "Cancel",
                Content = content,
                ShowTitle = true,
                Title = "Remove Patient",
            };

            MessageBoxResult result = await messageBox.ShowDialogAsync(true, CancellationToken.None);

            if (result == MessageBoxResult.Primary)
            {
                List<PatientView> patients = Results!.ToList();

                _data!.RemovePatient((int)patient.PatientId!);
                patients.Remove(patient);
                Results = patients;
                listView.ItemsSource = Results;
            }
        }

        private void ClearAddPatientFields()
        {
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            cmbDays.SelectedIndex = -1;
            cmbTimes.SelectedIndex = -1;
            cmbTypes.SelectedIndex = -1;
            chkNewPatient.IsChecked = false;
        }

        private void SortColumns(GridViewColumnHeader? columnHeader)
        {
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

                    if (Enum.GetNames(typeof(SortCriteria)).Contains(propertyName))
                    {
                        ListCollectionView listView = (ListCollectionView)view;
                        listView.CustomSort = new PatientComparer(Enum.Parse<SortCriteria>(propertyName));
                    }
                    else
                    {
                        view.SortDescriptions.Add(new SortDescription(propertyName, newSortDirection));
                    }
                }

                // Set the sort arrow icon
                columnHeader.ContentTemplate = Resources[newSortDirection == ListSortDirection.Ascending ? "SortAscendingTemplate" : "SortDescendingTemplate"] as DataTemplate;
            }
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

