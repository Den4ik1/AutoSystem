using AutoSystem.Models;
using System.Windows;


namespace AutoSystem.Forms
{
    /// <summary>
    /// Interaction logic for SubWindowMode.xaml
    /// </summary>
    public partial class SubWindowMode : Window
    {
        public ModeDataModel Mode { get; private set; }
        public SubWindowMode(ModeDataModel mode)
        {
            InitializeComponent();
            Mode = mode;
            DataContext= Mode;
        }

        private void Accept_Mode_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
