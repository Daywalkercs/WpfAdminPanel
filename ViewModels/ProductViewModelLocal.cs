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
using System.IO;
using System.Text.Json;
using System.Windows.Diagnostics;
//using static System.Net.WebRequestMethods;

namespace WpfAdminPanel.ViewModels
{
    public class ProductViewModelLocal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SelectImageCommand { get; }
        public ICommand ImageDropCommand { get; }

        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }

        private const string COMBO_BOX_TEXT = "Выберите товар или начните вводить";

        private string? _currentFilePath;

        private string _comboBoxText = "Выберите товар или начните вводить";
        public string ComboBoxText
        {
            get => _comboBoxText;
            set
            {
                _comboBoxText = value;
                OnPropertyChanged(nameof(ComboBoxText));
            }
        }

        private ObservableCollection<Product> _products = new();
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private Product? _selectedProduct;
        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                OnPropertyChanged(nameof(SelectedProduct.Img));
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
            AddCommand = new RelayCommand<object>(_ => AddProduct());
            SelectImageCommand = new RelayCommand<object>(_ => SelectImage());
            DeleteCommand = new RelayCommand<object>(async _ => await DeleteProduct());
            ImageDropCommand = new RelayCommand<DragEventArgs>(OnImageDropped);

            OpenCommand = new RelayCommand<object>(_ => OpenFile());
            SaveCommand = new RelayCommand<object>(_ => Save());
            SaveAsCommand = new RelayCommand<object>(_ => SaveAsFile());
            LoadProducts();
        }

        private void Save()
        {
            string directory = "Data";
            string filePath = Path.Combine(directory, "SavedFile.json");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(Products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            _currentFilePath = filePath;
            MessageBox.Show("Файл сохранен", "Сохранение файла");
        }

        private void SaveAsFile()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*",
                Title = "Сохранить как"
            };

            if (dialog.ShowDialog() == true)
            {
                _currentFilePath = dialog.FileName;
                SaveToFile(_currentFilePath);
            }
        }

        private void SaveToFile(string failPath)
        {

            try
            {
                string json = JsonSerializer.Serialize(Products, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(failPath, json);
                MessageBox.Show("Файл сохранен.", "Успешное сохранение");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении {ex.Message}", "Ошибка сохранения");
            }
        }

        private void OpenFile()
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "Json файлы (*.json)|*.json|Все файлы (*.*) | *.*",
                Title = "Открыть файл"
            };

            if (openDialog.ShowDialog() == true)
            {
                LoadFromFile(openDialog.FileName);
            }
        }

        private void LoadFromFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<ObservableCollection<Product>>(json);

                if (products != null)
                {
                    Products.Clear();
                    foreach (var product in products)
                    {
                        Products.Add(product);
                    }
                    MessageBox.Show("Файл успешно загружен", "Успешная загрузка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка загрузки файла!");
            }
        }

        private void LoadProducts()
        {
            ComboBoxText = COMBO_BOX_TEXT;
            SelectedProduct = null;
            OnPropertyChanged(nameof(SelectedProduct));
            SelectedProduct = new Product();
            OnPropertyChanged(nameof(Products));
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


        private void AddProduct()
        {
            //await Task.Delay(100);

            if (string.IsNullOrWhiteSpace(SelectedProduct?.CarModel))
            {
                MessageBox.Show("Поле \"Модель\" обязательно для заполнения!");
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
                await Task.Delay(100);
                Products.Remove(SelectedProduct);

                SelectedProduct = null;
                OnPropertyChanged(nameof(SelectedProduct));
                LoadProducts();
                MessageBox.Show("Товар успешно удален.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления! {ex.Message}");
            }
        }
    }
}
