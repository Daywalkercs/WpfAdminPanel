using OnlineShop.Data.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

public class CarApiClient
{
    private readonly HttpClient _httpClient;

    public CarApiClient()
    {
        _httpClient = new HttpClient();

        // Локальный Uri
        _httpClient.BaseAddress = new Uri("https://localhost:44378/api/"); // Изменить при необходимости
    }

    public async Task<List<Car>> GetCarsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<Car>>("CarsApi") ?? new List<Car>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при загрузке списка машин: {ex.Message}");
            return new List<Car>();
        }
    }

    public async Task<bool> AddCarAsync(Car car) { return await Task.Run(() => true); }
    public async Task<bool> DeleteCarAsync(int id) { return await Task.Run(() => true); }
    public async Task<bool> UpdateCarAsync(Car car) { return await Task.Run(() => true); }
    public async Task<bool> DeleteAllCarsAsync() { return await Task.Run(() => true); }
}
