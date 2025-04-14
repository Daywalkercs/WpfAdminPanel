using OnlineShop.Data.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class CarApiClient
{
    private readonly HttpClient _httpClient;

    public CarApiClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:44378/api/"); // замени при необходимости
    }

    public async Task<List<Car>> GetCarsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<Car>>("carsapi") ?? new List<Car>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при загрузке списка машин: {ex.Message}");
            return new List<Car>();
        }
    }
}
