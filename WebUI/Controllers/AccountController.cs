using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        // Constructor'da HttpClient bağımlılığı alınıyor
        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Login işlemi
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Giriş verilerini JSON formatına çevir
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // API'ye POST isteği gönder
            var response = await _httpClient.PostAsync("https://localhost:5001/api/Auth/login", jsonContent);

            // Eğer istek başarılıysa
            if (response.IsSuccessStatusCode)
            {
                // Yanıtı JSON olarak oku
                var result = await response.Content.ReadAsStringAsync();

                // Yanıtı TokenResponse modeline dönüştür
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Büyük küçük harf duyarlılığına dikkat
                });

                // Token varsa session'a kaydet
                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.Token))
                {
                    HttpContext.Session.SetString("JWTToken", tokenResponse.Token);
                    return RedirectToAction("Index", "Home"); // Başarılı giriş sonrası ana sayfaya yönlendirme
                }
            }

            // Hatalı giriş durumunda tekrar login sayfasına yönlendirme
            ModelState.AddModelError(string.Empty, "Giriş başarısız.");
            return View("user_login");
        }
    }

    // API'den dönecek olan token yanıtını temsil eden sınıf
    public class TokenResponse
    {
        public string Token { get; set; } // API'den dönen token
    }

    // Login model sınıfı
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
