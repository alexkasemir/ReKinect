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
    /// Interaction logic for ScoreWindow.xaml
    /// </summary>
    public partial class ScoreWindow : Window
    {
        float score;

        public ScoreWindow()
        {
            InitializeComponent();
            score = 0;
        }

        public void setScore(float newScore)
        {
            score = newScore;
            this.label_score.Content = score;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("hello");
            if (e.Key == Key.R)
            {
                TitleWindow window = new TitleWindow();
                window.Width = this.ActualWidth;
                window.Height = this.ActualHeight;
                if (this.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Maximized;
                }
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //window.setScore(this.gestureDetector.GestureResultView.Overall_Score);

                window.Show();
                this.Close();
            }
        }

    }

}
