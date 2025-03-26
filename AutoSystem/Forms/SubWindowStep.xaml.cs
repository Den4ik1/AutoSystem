using AutoSystem.Models;
using System.Windows;

namespace AutoSystem.Forms
{
    /// <summary>
    /// Interaction logic for SubWindowStep.xaml
    /// </summary>
    public partial class SubWindowStep : Window
    {
        public StepDataModel Step{ get; private set; }
        public SubWindowStep(StepDataModel step)
        {
            InitializeComponent();
            Step = step;
            DataContext = Step;
        }

        private void Accept_Step_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
