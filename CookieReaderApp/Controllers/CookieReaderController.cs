using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CookieReaderApp.Controllers
{
    public class CookieReaderController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (authResult.Succeeded && authResult.Principal != null)
            {
                var user = authResult.Principal;
                var name = user.FindFirst(ClaimTypes.Name)?.Value ?? "Desconocido";
                var role = user.FindFirst(ClaimTypes.Role)?.Value ?? "Sin rol";

                return Content($"✅ Cookie encontrada y autenticada.\nUsuario autenticado: {name}, Rol: {role}");
            }

            return Content("❌ La cookie no se autenticó correctamente.");
        }
    }
}
