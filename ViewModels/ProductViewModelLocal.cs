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
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        private string _carModel;
        private string _shortDescription;
        private string _longDescription;
        private string _img;
        private decimal _price;
        private bool _isFavourite;
        private bool _available;
        private int _categoryId;


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string CarModel
        {
            get => _carModel;
            set
            {
                _carModel = value;
                OnPropertyChanged(nameof(CarModel));
            }
        }

        public string ShortDescription
        {
            get => _shortDescription;
            set
            {
                _shortDescription = value;
                OnPropertyChanged(nameof(ShortDescription));
            }
        }

        public string LongDescription
        {
            get => _longDescription;
            set
            {
                _longDescription = value;
                OnPropertyChanged(nameof(LongDescription));
            }
        }

        public string Img
        {
            get => _img;
            set
            {
                _img = value;
                OnPropertyChanged(nameof(Img));
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public bool IsFavourite
        {
            get => _isFavourite;
            set
            {
                _isFavourite = value;
                OnPropertyChanged(nameof(IsFavourite));
            }
        }

        public bool Available
        {
            get => _available;
            set
            {
                _available = value;
                OnPropertyChanged(nameof(Available));
            }
        }

        public int CategoryId
        {
            get => _categoryId;
            set
            {
                _categoryId = value;
                OnPropertyChanged(nameof(CategoryId));
            }
        }


        public ProductViewModelLocal()
        {
            Products = new ObservableCollection<Product>
                {
                    new Product {
                        CarModel = "BMW",
                        Available = true,
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
                        Available = true,
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
                        Available = true,
                        //CategoryID = 1,
                        Id = 3,
                        Img = "Image Toyota",
                        //IsFavourite = true,
                        LongDescription = "Long description Toyota",
                        Price = 100,
                        ShortDescription = "Short description Toyota"
                    }
                };

            LoadCommand = new RelayCommand( () => LoadProducts());
            AddCommand = new RelayCommand( () =>  AddProduct());
            //UpdateCommand = new RelayCommand(async () => await UpdateProduct());
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
            
        }


        private async Task AddProduct()
        {
            var newProduct = new Product
            {
                Id = Products.Count + 1,
                CarModel = CarModel,
                ShortDescription = ShortDescription,
                LongDescription = LongDescription,
                Img = Img,
                Price = Price,
                //IsFavourite = IsFavourite,
                Available = Available,
                //CategoryID = CategoryId
            };

            Products.Add(newProduct);
            SelectedProduct = newProduct;
            MessageBox.Show("Товар добавлен");


            CarModel = string.Empty;
            ShortDescription = string.Empty;
            LongDescription = string.Empty;
            Img = string.Empty;
            Price = 0;
            IsFavourite = false;
            Available = false;
            CategoryId = 0;
            await LoadProducts();


        }

        //private async Task UpdateProduct()
        //{
        //    if (Products == null) return;

        //    SelectedProduct.CarModel += " (Обновлен)";
        //    bool success = await _productService.UpdateAsync(SelectedProduct.Id, SelectedProduct);
        //    if (success)
        //    {
        //        MessageBox.Show("Товар обновлен!");
        //        await LoadProducts();
        //    }
        //}

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
