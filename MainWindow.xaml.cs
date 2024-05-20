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
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = Wpf.Ui.Controls.MessageBox;
using Button = System.Windows.Controls.Button;
using MessageBoxResult = Wpf.Ui.Controls.MessageBoxResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DTCWaitingList.Database.Models;

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

            lbDays.ItemsSource = days;
            lb2Days.ItemsSource = days;
            lbTimes.ItemsSource = times;
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
            var conditions = new Dictionary<string, object>();

            if (lbDays.SelectedItems.Count != 0)
            {
                var firstDay = (Database.Models.Day)lbDays.SelectedItems[0]!;
                if (firstDay.NameOfDay != "Any Day")
                {
                    var dayList = new List<Database.Models.Day>();
                    foreach (var item in lbDays.SelectedItems)
                    {
                        dayList.Add((Database.Models.Day)item);
                    }

                    conditions.Add("PatientDays", dayList);
                }
            }
            if (lbTimes.SelectedItems.Count != 0)
            {
                var firstTime = (Time)lbTimes.SelectedItems[0]!;
                if (firstTime.TimeOfDay != "Any Time")
                {
                    var timeList = new List<Time>();
                    foreach (var item in lbTimes.SelectedItems)
                    {
                        timeList.Add((Time)item);
                    }

                    conditions.Add("PatientTimes", timeList);
                }
            }
            if (cmbTypes.SelectedItem != null)
            {
                conditions.Add("Reason", cmbTypes.SelectedItem);
            }

            Results = _data!.SearchPatients(conditions);
            listView.ItemsSource = Results;
        }

        private void DaySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox? listBox = sender as ListBox;

            if (listBox != null && listBox.SelectedItems.Count > 0)
            {
                if (e.AddedItems.Count > 0)
                {
                    var selectedItem = e.AddedItems[0] as Database.Models.Day;

                    if (selectedItem!.NameOfDay == "Any Day")
                    {
                        foreach (var item in listBox.Items)
                        {
                            if (item != selectedItem)
                            {
                                listBox.SelectedItems.Remove(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (Database.Models.Day day in listBox.Items)
                        {
                            if (day.NameOfDay == "Any Day")
                            {
                                listBox.SelectedItems.Remove(day);
                            }
                        }

                    }
                }
            }
        }

        private void TimeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox? listBox = sender as ListBox;

            if (listBox != null && listBox.SelectedItems.Count > 0)
            {
                if (e.AddedItems.Count > 0)
                {
                    var selectedItem = e.AddedItems[0] as Time;

                    if (selectedItem!.TimeOfDay == "Any Time")
                    {
                        foreach (var item in listBox.Items)
                        {
                            if (item != selectedItem)
                            {
                                listBox.SelectedItems.Remove(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (Time time in listBox.Items)
                        {
                            if (time.TimeOfDay == "Any Time")
                            {
                                listBox.SelectedItems.Remove(time);
                            }
                        }

                    }
                }
            }
        }

        private void AddPacient_Click(object sender, RoutedEventArgs e)
        {
            if (txtFullName.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || chkNewPatient.IsChecked == null
                || lb2Days.SelectedItems.Count == 0 || lb2Times.SelectedItems.Count == 0 || cmb2Types.SelectedItem == null)
            {
                errorMessage.Visibility = Visibility.Visible;
                successMessage.Visibility = Visibility.Hidden;
            }
            else
            {
                errorMessage.Visibility = Visibility.Hidden;
                successMessage.Visibility = Visibility.Hidden;

                var newPatient = new PatientView()
                {
                    FullName = txtFullName.Text,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    PatientDays = new List<string>(),
                    PatientTimes = new List<string>(),
                    IsClient = chkNewPatient.IsChecked!.Value,
                    CreatedDate = DateTime.Now,
                };

                foreach (var item in lb2Days.SelectedItems)
                {
                    var itemDay = (Database.Models.Day)item;
                    newPatient.PatientDays.Add(itemDay!.NameOfDay!);
                }

                foreach (var item in lb2Times.SelectedItems)
                {
                    var itemTime = (Time)item;
                    newPatient.PatientTimes.Add(itemTime!.TimeOfDay!);
                }

                var reason = (Reason)cmb2Types.SelectedItem!;
                newPatient.Reason = reason.ReasonName;
                newPatient.FullReason = reason.ReasonName;

                try
                {
                    _data!.AddPatient(newPatient);
                    ClearAddPatientFields();
                }
                catch (Exception ex)
                {
                    throw new DbUpdateException($"Wasn't able to add patient to database, please check your connection and try again later. Error: {ex.Message}");
                }
                finally
                {
                    Results = _data!.GetPatients();
                    listView.ItemsSource = Results;
                    successMessage.Visibility = Visibility.Visible;
                }
            } 
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

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            lbDays.SelectedIndex = -1;
            lbTimes.SelectedIndex = -1;
            cmbTypes.SelectedIndex = -1;
        }

        private void ClearAddPatientFields()
        {
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            lb2Days.SelectedIndex = -1;
            lb2Times.SelectedIndex = -1;
            cmb2Types.SelectedIndex = -1;
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

            notifyIcon.MouseClick += NotifyIcon_MouseClick!;
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip menuStrip = new ContextMenuStrip();
                menuStrip.Items.Add("Open DTC WL", null, OpenMenuItem_Click!);
                menuStrip.Items.Add("Exit", null, ExitMenuItem_Click!);
                notifyIcon!.ContextMenuStrip = menuStrip;
            }
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}

