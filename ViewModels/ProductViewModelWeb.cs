using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfAdminPanel.Helpers;
using WpfAdminPanel.Models;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using OnlineShop.Data.Models;
//using static System.Net.WebRequestMethods;

namespace WpfAdminPanel.ViewModels
{
    public class ProductViewModelWeb : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CleanAllCommand { get; }
        public ICommand SelectImageCommand { get; }
        public ICommand ImageDropCommand { get; }

        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }

        private const string COMBO_BOX_TEXT = "Выберите товар или начните вводить";

        private string? _currentFilePath;

        private readonly CarApiClient _client = new CarApiClient();

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

        //private ObservableCollection<Product> _products = new();
        //public ObservableCollection<Product> Products
        //{
        //    get { return _products; }
        //    set
        //    {
        //        _products = value;
        //        OnPropertyChanged(nameof(Products));
        //    }
        //}

        private ObservableCollection<Car> _products = new();
        public ObservableCollection<Car> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private Car? _selectedProduct;
        public Car? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                OnPropertyChanged(nameof(SelectedProduct.Img));
            }
        }

        public ProductViewModelWeb()
        {
            LoadCommand = new RelayCommand<object>(async _ => await LoadProductsAsync());
            AddCommand = new RelayCommand<object>(async _ => await AddProduct());
            SelectImageCommand = new RelayCommand<object>(_ => SelectImage());
            DeleteCommand = new RelayCommand<object>(async _ => await DeleteProduct());
            CleanAllCommand = new RelayCommand<object>(async _ => await CleanAll());
            ImageDropCommand = new RelayCommand<DragEventArgs>(OnImageDropped);

            OpenCommand = new RelayCommand<object>(_ => OpenFile());
            SaveCommand = new RelayCommand<object>(_ => Save());
            SaveAsCommand = new RelayCommand<object>(_ => SaveAsFile());
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

        private void SaveToFile(string filePath)
        {
            try
            {
                string json = JsonSerializer.Serialize(Products, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                MessageBox.Show("Файл сохранен.", "Успешное сохранение");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
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
                var products = JsonSerializer.Deserialize<ObservableCollection<Car>>(json);

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

        public async Task LoadProductsAsync()
        {
            var cars = await _client.GetCarsAsync();
            foreach (var car in cars)
            {
                Products.Add(car);
            }
            //await Task.Delay(100);
            ComboBoxText = COMBO_BOX_TEXT;
            SelectedProduct = null;
            OnPropertyChanged(nameof(SelectedProduct));
            SelectedProduct = new Car();
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
                MessageBox.Show("Выберите товар для изменения изображения.");
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

        private async Task AddProduct()
        {
            await Task.Delay(100);

            if (string.IsNullOrWhiteSpace(SelectedProduct?.CarModel))
            {
                MessageBox.Show("Поле \"Модель\" обязательно для заполнения!");
                return;
            }

            SelectedProduct.Id = Products.Count + 1;
            Products.Add(SelectedProduct);
            OnPropertyChanged(nameof(Products));
            MessageBox.Show("Товар добавлен");
            await LoadProductsAsync();
        }


        private async Task DeleteProduct()
        {
            if (SelectedProduct == null || _products.Count == 0)
            {
                MessageBox.Show("Список товаров пуст. Необходимо выбрать товар перед удалением.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (string.IsNullOrWhiteSpace(SelectedProduct?.CarModel))
            {
                MessageBox.Show("Перед удалением необходимо добавить либо выбрать товар.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Удалить этот товар?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;

            try
            {
                await Task.Delay(100);
                Products.Remove(SelectedProduct);

                SelectedProduct = null;
                OnPropertyChanged(nameof(SelectedProduct));
                OnPropertyChanged(nameof(Products));
                _ = LoadProductsAsync();
                MessageBox.Show("Товар успешно удален.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления! {ex.Message}");
            }
        }

        private async Task CleanAll()
        {
            try
            {
                if (Products.Count == 0)
                {
                    MessageBox.Show("Список товаров пуст", "Внимание");
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Удалить все товары?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No) return;

                // Очистка коллекции из UI потока!
                await Application.Current.Dispatcher.InvokeAsync(() => Products.Clear());

                MessageBox.Show("Все товары удалены.", "Удаление товаров");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления");
            }
        }
    }
}
