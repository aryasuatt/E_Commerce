using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json; // Json ile çalışabilmek için gerekli kütüphane
using System.Threading.Tasks;


namespace WebUI.Services
{
    public class ApiService
    {
        //private readonly HttpClient _httpClient;

        //public ApiService(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        //public async Task<List<Product>> GetProductsAsync()
        //{
        //    var response = await _httpClient.GetAsync("products"); // API uç noktanız
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadFromJsonAsync<List<Product>>(); // Okuma yöntemi güncellendi
        //}
    }
}
