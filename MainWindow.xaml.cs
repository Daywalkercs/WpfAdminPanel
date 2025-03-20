using System.Windows;
using WpfAdminPanel.ViewModels;

namespace WpfAdminPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new ProductViewModel();
            DataContext = new ProductViewModelLocal();
        }
    }
}
