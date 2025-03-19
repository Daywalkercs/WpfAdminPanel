using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfAdminPanel.Helpers;
using WpfAdminPanel.Models;
using System.ComponentModel;

namespace WpfAdminPanel.ViewModels
{
    public class ProductViewModelLocal : INotifyPropertyChanged
    {
        public Product product = new Product();

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }



        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand NewProductCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }




        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string CarModel
        {
            get => product.CarModel;
            set
            {
                product.CarModel = value;
                OnPropertyChanged(nameof(CarModel));
            }
        }

        public string ShortDescription
        {
            get => product.ShortDescription;
            set
            {
                product.ShortDescription = value;
                OnPropertyChanged(nameof(ShortDescription));
            }
        }

        public string LongDescription
        {
            get => product.LongDescription;
            set
            {
                product.LongDescription = value;
                OnPropertyChanged(nameof(LongDescription));
            }
        }

        public string Img
        {
            get => product.Img;
            set
            {
                product.Img = value;
                OnPropertyChanged(nameof(Img));
            }
        }

        public decimal Price
        {
            get => product.Price;
            set
            {
                product.Price = value;
                OnPropertyChanged(nameof(Price));
            }
        }




        public ProductViewModelLocal()
        {
            Products = new ObservableCollection<Product>
                {
                    new Product {
                        CarModel = "BMW",
                        //Available = true,
                        //CategoryID = 1,
                        Id = 1,
                        Img = "Image BMW",
                        //IsFavourite = true,
                        LongDescription = "Long description BMW",
                        Price = 200,
                        ShortDescription = "Short description BMW"
                    },
                    new Product {
                        CarModel = "Mazda",
                        //Available = true,
                        //CategoryID = 1,
                        Id = 2,
                        Img = "Image Mazda",
                        //IsFavourite = true,
                        LongDescription = "Long description Mazda",
                        Price = 100,
                        ShortDescription = "Short description Mazda"
                    },
                        new Product {
                        CarModel = "Toyota",
                        //Available = true,
                        //CategoryID = 1,
                        Id = 3,
                        Img = "Image Toyota",
                        //IsFavourite = true,
                        LongDescription = "Long description Toyota",
                        Price = 100,
                        ShortDescription = "Short description Toyota"
                    }
                };

            LoadCommand = new RelayCommand(() => LoadProducts());
            NewProductCommand = new RelayCommand(() => NewProduct());
            AddCommand = new RelayCommand(() => AddProduct());
            UpdateCommand = new RelayCommand(async () => await UpdateProduct());
            //DeleteCommand = new RelayCommand(async () => await DeleteProduct());

            LoadProducts();
        }

        private async Task LoadProducts()
        {
            await Task.Delay(1000);

            var UpdatedProducts = new ObservableCollection<Product>(Products);
            Products.Clear();
            foreach (var product in UpdatedProducts)
            {
                Products.Add(product);
            }
            SelectedProduct = product;
        }

        private void NewProduct()
        {
            CarModel = string.Empty;
            ShortDescription = string.Empty;
            LongDescription = string.Empty;
            Img = string.Empty;
            Price = 0;

            SelectedProduct = product;
        }

        private async Task AddProduct()
        {
            var newProduct = new Product
            {
                Id = Products.Count + 1,
                CarModel = this.CarModel,
                ShortDescription = this.ShortDescription,
                LongDescription = this.LongDescription,
                Img = this.Img,
                Price = this.Price,
                //IsFavourite = IsFavourite,
                //Available = Available,
                //CategoryID = CategoryId
            };

            //Products.Add(SelectedProduct);
            Products.Add(newProduct);
            //SelectedProduct = newProduct;

            CarModel = string.Empty;
            ShortDescription = string.Empty;
            LongDescription = string.Empty;
            Img = string.Empty;
            Price = 0;
            //IsFavourite = false;
            //Available = false;
            //CategoryId = 0;

            MessageBox.Show("Товар добавлен");


            await LoadProducts();


        }

        private async Task UpdateProduct()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Выберите товар для обновления!");
                return;
            }

            Product productToUpdate = Products.FirstOrDefault(p => p.CarModel == SelectedProduct.CarModel);
            if (productToUpdate == null) { MessageBox.Show("Ошибка выбора товара!"); return; }

            //productToUpdate.CarModel = SelectedProduct.CarModel;
            //productToUpdate.ShortDescription = SelectedProduct.ShortDescription;
            //productToUpdate.LongDescription = SelectedProduct.LongDescription;
            //productToUpdate.Img = SelectedProduct.Img;
            //productToUpdate.Price = SelectedProduct.Price;

            OnPropertyChanged(nameof(Products));
        }

        //private async Task DeleteProduct()
        //{
        //    if (SelectedProduct == null) return;

        //    bool success = await _productService.DeleteAsync(SelectedProduct.Id);
        //    if (success)
        //    {
        //        MessageBox.Show("Товар удален!");
        //        await LoadProducts();
        //    }
        //}
    }
}
