using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WpfAdminPanel.Services
{
    public class ApiService<T>
    {
        private readonly HttpClient _httpClient;  // HTTP-клиент для запросов
        private readonly string _baseUrl;         // Базовый URL API

        public ApiService(string baseUrl)
        {
            _httpClient = new HttpClient(); // Инициализируем HttpClient
            _baseUrl = baseUrl;             // Сохраняем URL API
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<T>>(_baseUrl);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<T>($"{_baseUrl}/{id}");
        }

        public async Task<bool> CreateAsync(T item)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, item);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, T item)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", item);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}

