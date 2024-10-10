using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SanalMarketAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {
            var user = new ApplicationUser
            {
                UserName = "aryasuatt", //Kullanıcı adı
                Email = "mail.aryasuat@gmail.com" // E-Posta adresi.
            };

            //Kullanıcıya bir şifre ataması
            string password = "DenemePassword123";

            //Kullanıcı oluşturuyoruz
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return Ok("Kullanıcı oluşturuldu!");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        public class ApplicationUser : IdentityUser
        {
            // Ek özellikler ekleyebilirsiniz
        }


    }
}
