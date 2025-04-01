using System.Windows;
using WpfAdminPanel.ViewModels;
using System.Reflection;

namespace WpfAdminPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new ProductViewModel();
            DataContext = new ProductViewModelLocal();
            SetWindowTitle();
        }

        private void Border_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.Copy;
        }

        private void SetWindowTitle()
        {
            string? version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            this.Title = $"Админ-панель v{version ?? "Неизвестная версия"}";
        }
    }
}
