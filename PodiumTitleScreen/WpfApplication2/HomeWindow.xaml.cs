using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        private void button_startinterview_Click(object sender, RoutedEventArgs e)
        {
            InterviewWindow window = new InterviewWindow();
            window.Width = this.ActualWidth;
            window.Height = this.ActualHeight;
            if (this.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Maximized;
            }
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            window.Show();
            this.Close();
        }

    }
}
