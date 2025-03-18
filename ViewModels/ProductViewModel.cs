using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using WpfAdminPanel.Models;
using WpfAdminPanel.Services;
using System.Windows.Input;
using WpfAdminPanel.Helpers;

namespace WpfAdminPanel.ViewModels
{
    public class ProductViewModel
    {
        private readonly ApiService<Product> _productService;
        public ObservableCollection<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }


        public ProductViewModel()
        {
            _productService = new ApiService<Product>("https://localhost:5001/api/products");
            Products = new ObservableCollection<Product>();


            LoadCommand = new RelayCommand(async () => await LoadProducts());
            AddCommand = new RelayCommand(async () => await AddProduct());
            UpdateCommand = new RelayCommand(async () => await UpdateProduct());
            DeleteCommand = new RelayCommand(async () => await DeleteProduct());

            LoadProducts();
        }

        private async Task LoadProducts()
        {
            var products = await _productService.GetAllAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        private async Task AddProduct()
        {
            var newProduct = new Product {
                         Id = 1,
                         CarModel = "Новый товар",
                         ShortDescription = "Новое описание",
                         LongDescription = "Новое подробное описание",
                         Img = "New Image",
                         Price = 100,
                         //IsFavourite = true,
                         //Available = true,
                         //CategoryID = 1,
                                         };
            bool success = await _productService.CreateAsync(newProduct);
            if (success)
            {
                MessageBox.Show("Товар добавлен!");
                await LoadProducts();
            }
        }

        private async Task UpdateProduct()
        {
            if (SelectedProduct == null) return;

            SelectedProduct.CarModel += " (Обновлен)";
            bool success = await _productService.UpdateAsync(SelectedProduct.Id, SelectedProduct);
            if (success)
            {
                MessageBox.Show("Товар обновлен!");
                await LoadProducts();
            }
        }

        private async Task DeleteProduct()
        {
            if (SelectedProduct == null) return;

            bool success = await _productService.DeleteAsync(SelectedProduct.Id);
            if (success)
            {
                MessageBox.Show("Товар удален!");
                await LoadProducts();
            }
        }
    }
}
