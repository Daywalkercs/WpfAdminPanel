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

namespace WpfAdminPanel.Views
{
    /// <summary>
    /// Логика взаимодействия для CarsView.xaml
    /// </summary>
    public partial class CarsView : Window
    {
        private CarsViewModel _carsViewModel;

        public CarsView()
        {
            InitializeComponent();

            _carsViewModel = new CarsViewModel();
            DataContext = _carsViewModel;
            Loaded += CarsView_Loaded;
        }

        private async void CarsView_Loaded(object sender, RoutedEventArgs e)
        {
            await _carsViewModel.LoadCarsAsync();
        }
    }
}
