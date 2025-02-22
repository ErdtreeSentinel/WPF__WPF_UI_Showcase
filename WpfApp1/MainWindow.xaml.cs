using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {


            InitializeComponent();
            ApplicationThemeManager.Apply(
                ApplicationTheme.Dark,
                WindowBackdropType.Mica,
                true
            );
        }

        private void DestroyUSAButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("jfldsjlfdsjl");
        }
    }
}