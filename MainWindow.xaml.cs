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

        private void Border_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.Copy;
        }


    }
}
