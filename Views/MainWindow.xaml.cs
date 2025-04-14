using System.Windows;
using WpfAdminPanel.ViewModels;
using System.Reflection;

namespace WpfAdminPanel.Views
{
    public partial class MainWindow : Window
    {
        private ProductViewModelWeb _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new ProductViewModelWeb();
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
            SetWindowTitle();
        }



        private async void CarsView_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadProductsAsync();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductViewModelWeb productViewModelWeb)
            {
                try
                {
                    await productViewModelWeb.LoadProductsAsync();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке товаров:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (DataContext is ProductViewModelLocal productViewModelLocal)
        //    {
        //        try
        //        {
        //            await productViewModelLocal.LoadProductsAsync();

        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Ошибка при загрузке товаров:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}

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
