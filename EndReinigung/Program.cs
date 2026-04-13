using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace EndReinigung
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //8_Y&gVbIe6bh7cok
            // Add localization services
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddControllersWithViews()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            var app = builder.Build();

            // Configure supported cultures
            var supportedCultures = new[] { new CultureInfo("de"), new CultureInfo("en") };
            var locOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("de"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            locOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            app.UseRequestLocalization(locOptions);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Add AVIF MIME type support
            var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings[".avif"] = "image/avif";
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = contentTypeProvider
            });

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Attribute routing for SEO-friendly URLs
            app.MapControllers();

            app.Run();
        }
    }
}
