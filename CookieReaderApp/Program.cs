﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

var encryptionKey = builder.Configuration["CookieSettings:EncryptionKey"];
if (string.IsNullOrEmpty(encryptionKey))
{
    throw new Exception("La clave de cifrado no está configurada en appsettings.json");
}
var sharedKey = Convert.FromBase64String(encryptionKey);

var keyDirectory = new DirectoryInfo(@"C:\SharedKeys");
if (!keyDirectory.Exists)
{
    keyDirectory.Create();
}

// Configurar protección de datos
builder.Services.AddDataProtection()
    .SetApplicationName("SharedAuthApp")
    .PersistKeysToFileSystem(keyDirectory)
    .ProtectKeysWithDpapi();

// Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.Domain = "localhost";
    });

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CookieReader}/{action=Index}/{id?}"
);

app.Run();
