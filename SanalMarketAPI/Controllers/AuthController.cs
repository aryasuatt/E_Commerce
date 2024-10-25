using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SanalMarketAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SanalMarketAPI.Models; 
using System;


namespace SanalMarketAPI.Controllers
{
    //API controller yapısı, Auth işlemleri için bu cotroller kullanılacak.
    [ApiController]
    [Route("api/[controller]")]  //URL de "api/auth" pşarak erişilecek.
    public class AuthController : Controller
    {
        // Identity servislerini kullanarak kullanıcı yönetimi gerçekleştirmek için kullanılan bağımlılıklar
        private readonly UserManager<ApplicationUser> _userManager;         // Kullanıcı yönetimi
        private readonly SignInManager<ApplicationUser> _signInManager;     // Giriş işlemleri
        private readonly IConfiguration _configuration;                     // Uygulama ayarlarını okuma

        // Constructor'da bağımlılıklar dependency Injection yoluyla ekleniyor.
        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        //Kullanıcı kayıt işlemi (POST /api/auth/register)
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            // Yeni bir kullanıcı oluşturuluyor
            var user = new User
            {
                UserName = model.Username,   // Kullanıcı adı
                Email = model.Email,         // Email adresi
                FullName = model.FullName
            };

            // Identity framework ile kullanıcı oluşturma işlemi yapılıyor
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)   //  Eğer kullanıcı oluşturma başarılıysa
            {
                //Gerekirse burada kullanıcıya rol atama işlemi yapılabilir (opsiyonel)
                return Ok(new { message = "User registered successfully." }); //

            }
            // Kullanıcı oluşturma başarısızsa, hataları geri döner.
            return BadRequest(result.Errors);   //Hatalı istek yanıtı
        }


        //Satıcı kayıt işlemi (POST /api/auth/register)
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterSeller([FromBody] RegisterModel model)
        {
            // Yeni bir kullanıcı oluşturuluyor
            var seller = new Seller
            {
                UserName = model.Username,   // Kullanıcı adı
                Email = model.Email,         // Email adresi
                FullName = model.FullName
            };

            // Identity framework ile kullanıcı oluşturma işlemi yapılıyor
            var result = await _userManager.CreateAsync(seller, model.Password);

            if (result.Succeeded)   //  Eğer kullanıcı oluşturma başarılıysa
            {
                //Gerekirse burada kullanıcıya rol atama işlemi yapılabilir (opsiyonel)
                return Ok(new { message = "User registered successfully." }); //

            }
            // Kullanıcı oluşturma başarısızsa, hataları geri döner.
            return BadRequest(result.Errors);   //Hatalı istek yanıtı
        }



        // Kullanıcı giriş işlemi (POST /api/auth/login)
        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser([FromBody] LoginModel model)
        {
            // Kullanıcı adıyla kullanıcı aranıyor
            var user = await _userManager.FindByNameAsync(model.Username) as User;

            // Kullanıcı bulundu ve şifre doğruysa giriş yapar
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // JWT token üretme işlemi başlıyor
                var token = GenerateJwtToken(user); // Aşağıda tanımlı token üretme fonksiyonu çağrılıyor
                return Ok(new { Token = token });   // Token kullanıcıya geri gönderiliyor
            }

            // Eğer kullanıcı adı veya şifre yanlışsa Unauthorized (yetkisiz) hata döner.
            return Unauthorized();

        }

        // Satıcı  giriş işlemi (POST /api/auth/login)
        [HttpPost("login-seller")]
        public async Task<IActionResult> LoginSeller([FromBody] LoginModel model)
        {
            // Kullanıcı adıyla kullanıcı aranıyor
            var seller = await _userManager.FindByNameAsync(model.Username) as Seller;

            // Kullanıcı bulundu ve şifre doğruysa giriş yapar
            if (seller != null && await _userManager.CheckPasswordAsync(seller, model.Password))
            {
                // JWT token üretme işlemi başlıyor
                var token = GenerateJwtToken(seller); // Aşağıda tanımlı token üretme fonksiyonu çağrılıyor
                return Ok(new { Token = token });   // Token kullanıcıya geri gönderiliyor
            }

            // Eğer kullanıcı adı veya şifre yanlışsa Unauthorized (yetkisiz) hata döner.
            return Unauthorized();

        }




        // JWT Token üretme fonksiyonu
        private string GenerateJwtToken(ApplicationUser user)
        {
            // JWT handler nesnesi oluşturuluyor
            var tokenHandler = new JwtSecurityTokenHandler();

            // JWT'nin imzalanması için kullanılan key
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // appsettings.json dosyasındaki JWT Key

            // Token descriptor ile token'ın özellikleri belirleniyor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Token'a eklenecek Claims (Kullanıcı bilgileri gibi veriler)
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),         // Kullanıcı adı
                    new Claim(ClaimTypes.NameIdentifier, user.Id),     // Kullanıcı ID'si
                    // Eğer istenirse daha fazla claim eklenebilir
                }),

                NotBefore = DateTime.UtcNow,

                Expires = DateTime.UtcNow.AddHours(1), // Token'ın geçerlilik süresi 1 saat
                Issuer = _configuration["Jwt:Issuer"],      // Token'ı yayınlayan
                Audience = _configuration["Jwt:Audience"],  // Token'ı alacak olan (kimler kullanabilir)

                // Token'ın imzalanma işlemi
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            // Token oluşturuluyor
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Token string olarak geri döndürülüyor
            return tokenHandler.WriteToken(token);
        }
    }

    public class RegisterModel
    {
        public required string Username { get; set; }  // Kullanıcı adı
        public required string Email { get; set; }     // Email adresi
        public required string Password { get; set; }  // Şifre
        public string FullName { get; set; }
    }


    public class LoginModel
    {
        public  required string Username { get; set; }  // Kullanıcı adı
        public required string Password { get; set; }  // Şifre
    }


    public class User : ApplicationUser { }
    public class Seller : ApplicationUser { }

}


