using AutoSystem.Contexts;
using AutoSystem.Models;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace AutoSystem.Forms
{
    public partial class DataWindow : Window
    {
        private ApplicationContext db = new ApplicationContext();
        public DataWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();

            db.Modes.Load();
            db.Steps.Load();

            DataContext = new WindowDataContext
            {
                Modes = db.Modes.Local.ToObservableCollection(),
                Steps = db.Steps.Local.ToObservableCollection()
            };
        }

        private void Add_Mode_Click(object sender, RoutedEventArgs e)
        {
            SubWindowMode subModeWindow = new SubWindowMode(new ModeDataModel());
            try
            {
                if (subModeWindow.ShowDialog() == true)
                {
                    ModeDataModel mode = subModeWindow.Mode;
                    db.Modes.Add(mode);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Mode_Click(object sender, RoutedEventArgs e)
        {
            ModeDataModel? mode = modeList.SelectedItem as ModeDataModel;
            if (mode is null) return;

            try
            {


                SubWindowMode subModeWindow = new SubWindowMode(new ModeDataModel
                {
                    Id = mode.Id,
                    Name = mode.Name,
                    MaxBottleNumber = mode.MaxBottleNumber,
                    MaxUsedTips = mode.MaxUsedTips
                });

                if (subModeWindow.ShowDialog() == true)
                {
                    mode = db.Modes.Find(subModeWindow.Mode.Id);
                    if (mode != null)
                    {
                        mode.Name = subModeWindow.Mode.Name;
                        mode.MaxBottleNumber = subModeWindow.Mode.MaxBottleNumber;
                        mode.MaxUsedTips = subModeWindow.Mode.MaxUsedTips;
                        db.SaveChanges();
                        modeList.Items.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Mode_Click(object sender, RoutedEventArgs e)
        {
            ModeDataModel? mode = modeList.SelectedItem as ModeDataModel;
            if (mode is null) return;

            try
            {
                db.Modes.Remove(mode);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Step_Click(object sender, RoutedEventArgs e)
        {
            SubWindowStep subStepWindow = new SubWindowStep(new StepDataModel());
            try
            {
                if (subStepWindow.ShowDialog() == true)
                {
                    StepDataModel step = subStepWindow.Step;
                    db.Steps.Add(step);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Step_Click(object sender, RoutedEventArgs e)
        {
            StepDataModel? step = stepList.SelectedItem as StepDataModel;
            if (step is null) return;

            try
            {
                SubWindowStep subStepWindow = new SubWindowStep(new StepDataModel
                {
                    Id = step.Id,
                    Destination = step.Destination,
                    ModeId = step.ModeId,
                    Speed = step.Speed,
                    Timer = step.Timer,
                    Type = step.Type,
                    Volume = step.Volume
                });

                if (subStepWindow.ShowDialog() == true)
                {
                    step = db.Steps.Find(subStepWindow.Step.Id);
                    if (step != null)
                    {
                        step.Destination = subStepWindow.Step.Destination;
                        step.ModeId = subStepWindow.Step.ModeId;
                        step.Speed = subStepWindow.Step.Speed;
                        step.Timer = subStepWindow.Step.Timer;
                        step.Type = subStepWindow.Step.Type;
                        step.Volume = subStepWindow.Step.Volume;
                        db.SaveChanges();
                        stepList.Items.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Step_Click(object sender, RoutedEventArgs e)
        {
            StepDataModel? step = stepList.SelectedItem as StepDataModel;
            if (step is null) return;
            try
            {
                db.Steps.Remove(step);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Open_Excel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel Files|*.xls;*.xlsx",
                    Title = "Select an Excel File"

                };
                if (openFileDialog.ShowDialog() == true)
                {
                    ProcessExcelFile(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }


        private List<ModeDataModel> ReadMode(DataTable modesTable)
        {
           
            var newModes = new List<ModeDataModel>();
            foreach (DataRow row in modesTable.Rows)
            {

                var Id = row["Id"].ToString();
                var Name = row["Name"].ToString();
                var MaxBottleNumber = row["MaxBottleNumber"].ToString();
                var MaxUsedTips = row["MaxUsedTips"].ToString();

                newModes.Add(new ModeDataModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    MaxBottleNumber = Convert.ToInt32(row["MaxBottleNumber"]),
                    MaxUsedTips = Convert.ToInt32(row["MaxUsedTips"])
                });
            }
            return newModes;
        }

        private List<StepDataModel> ReadStep(DataTable stepsTable)
        {
            var newSteps = new List<StepDataModel>();
            var modes = modeList.ItemsSource as List<ModeDataModel>;

            foreach (DataRow row in stepsTable.Rows)
            {
                var step = new StepDataModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    ModeId = Convert.ToInt32(row["ModeId"]),
                    Timer = Convert.ToInt32(row["Timer"]),
                    Destination = row["Destination"].ToString(),
                    Speed = Convert.ToInt32(row["Speed"]),
                    Type = row["Type"].ToString(),
                    Volume = Convert.ToInt32(row["Volume"])
                };

                if (modes != null)
                {
                    step.Mode = modes.FirstOrDefault(m => m.Id == step.ModeId);
                }

                newSteps.Add(step);
            }
            return newSteps;
        }


        private void ProcessExcelFile(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        if (result.Tables.Contains("Modes"))
                        {
                            var modesTable = result.Tables["Modes"];
                            
                            var modes = modeList.ItemsSource as List<ModeDataModel> ?? new List<ModeDataModel>();
                      
                            var newModes = ReadMode(modesTable);

                            db.Modes.AddRange(newModes);
                            db.SaveChanges();
                            modes.AddRange(newModes);
                            modeList.ItemsSource = modes;
                        }

                        if (result.Tables.Contains("Steps"))
                        {
                            var stepsTable = result.Tables["Steps"];
                            var steps = stepList.ItemsSource as List<StepDataModel> ?? new List<StepDataModel>();

                            var newSteps = ReadStep(stepsTable);

                            db.Steps.AddRange(newSteps);
                            db.SaveChanges();
                            steps.AddRange(newSteps);
                            stepList.ItemsSource = steps;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
