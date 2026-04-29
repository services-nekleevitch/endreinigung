using EndReinigung.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using System.Globalization;

namespace EndReinigung
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add localization services
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddControllersWithViews()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            // Contact form email
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();

            // DB wiring is disabled for now.
            // End goal: share the same MySQL database with the legacy EndreinigungZurich project.
            // To re-enable, do NOT un-comment the previous EndReinigung/DataAccess/ context — instead:
            //   1. Move EndReinigung/DataAccess/Entities/Booking.cs into the existing DbAccess project
            //      (..\DbAccess\Entities\Booking.cs) and add DbSet<Booking> to DbAccess/AppDbContext.cs.
            //   2. Add a ProjectReference to ..\DbAccess\DbAccess.csproj in EndReinigung.csproj.
            //   3. Swap Microsoft.EntityFrameworkCore.Sqlite for Pomelo.EntityFrameworkCore.MySql.
            //   4. Register the shared AppDbContext here using UseMySql + the same connection string
            //      format as EndreinigungZurich/appsettings.json.
            //   5. Add EF migrations (dotnet ef migrations add AddBookings -p ..\DbAccess) so schema
            //      changes are tracked across both projects.
            //   6. Restore the AppDbContext dependency in CalculatorController and the save logic.
            //   7. Delete EndReinigung/DataAccess/ once everything has moved.

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

            // Canonicalize host: 301 redirect www.zurich-endreinigung.ch -> zurich-endreinigung.ch.
            // Keeps sitemap/canonical tags (non-www) aligned with what Google indexes.
            app.UseRewriter(new RewriteOptions().Add(ctx =>
            {
                var req = ctx.HttpContext.Request;
                if (req.Host.Host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                {
                    var newHost = req.Host.Host.Substring(4);
                    var newHostString = req.Host.Port.HasValue
                        ? new HostString(newHost, req.Host.Port.Value)
                        : new HostString(newHost);
                    var newUrl = $"{req.Scheme}://{newHostString}{req.PathBase}{req.Path}{req.QueryString}";
                    ctx.HttpContext.Response.StatusCode = StatusCodes.Status301MovedPermanently;
                    ctx.HttpContext.Response.Headers.Location = newUrl;
                    ctx.Result = RuleResult.EndResponse;
                }
            }));

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
