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
using Microsoft.Win32;
using System.Windows.Media.Imaging;

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
                OnPropertyChanged(nameof(SelectedProduct.Img));
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand NewProductCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SelectImageCommand { get; }
        public ICommand ImageDropCommand { get; }


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
                        Img = "",
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
                        Img = "",
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
                        Img = "",
                        //IsFavourite = true,
                        LongDescription = "Long description Toyota",
                        Price = 100,
                        ShortDescription = "Short description Toyota"
                    }
                };

            LoadCommand = new RelayCommand<object>(_ => LoadProducts());
            NewProductCommand = new RelayCommand<object>(_ => NewProduct());
            AddCommand = new RelayCommand<object>(_ => AddProduct());
            //UpdateCommand = new RelayCommand(async () => await UpdateProduct());
            SelectImageCommand = new RelayCommand<object>(_ => SelectImage());
            DeleteCommand = new RelayCommand<object>(async _ => await DeleteProduct());
            //ImageDropCommand = new RelayCommand<DragEventArgs>(OnImageDropped, _ => true);
            ImageDropCommand = new RelayCommand<DragEventArgs>(OnImageDropped);
            LoadProducts();
        }

        private void OnImageDropped(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && SelectedProduct is not null)
                {
                    string imagePath = files[0];
                    SelectedProduct.Img = imagePath;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }

        private void SelectImage()
        {
            if (SelectedProduct is null)
            {
                MessageBox.Show("Выберите товар для изменения изображения");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Изображения (*.png; *.jpg; *.jpeg) | *.png;*.jpg;*.jpeg",
                Title = "Выберите изображание"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedProduct.Img = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        private void LoadProducts()
        {
            OnPropertyChanged(nameof(Products));
            SelectedProduct = new Product();
        }



        private void NewProduct()
        {
            SelectedProduct = new Product();
            OnPropertyChanged(nameof(SelectedProduct));

        }

        private async Task AddProduct()
        {
            if (string.IsNullOrWhiteSpace(SelectedProduct?.CarModel))
            {
                MessageBox.Show("Заполните поля");
                return;
            }

            SelectedProduct.Id = Products.Count + 1;
            Products.Add(SelectedProduct);
            MessageBox.Show("Товар добавлен");
            LoadProducts();
        }


        private async Task DeleteProduct()
        {
            if (SelectedProduct == null) return;

            MessageBoxResult result = MessageBox.Show("Удалить этот товар?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;

            try
            {
                await Task.Delay(1000);
                Products.Remove(SelectedProduct);

                SelectedProduct = null;
                OnPropertyChanged(nameof (SelectedProduct));
                MessageBox.Show("Товар успешно удален.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления! {ex.Message}");
            }
        }
    }
}
