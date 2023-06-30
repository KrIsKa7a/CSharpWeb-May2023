namespace HouseRentingSystem.Web
{
    using Microsoft.EntityFrameworkCore;
    
    using Data;
    using Data.Models;
    using Infrastructure.Extensions;
    using Infrastructure.ModelBinders;
    using Services.Data.Interfaces;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString = 
                builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services.AddDbContext<HouseRentingDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount =
                        builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
                    options.Password.RequireLowercase = 
                        builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
                    options.Password.RequireUppercase =
                        builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
                    options.Password.RequireNonAlphanumeric =
                        builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
                    options.Password.RequiredLength =
                        builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
                })
                .AddEntityFrameworkStores<HouseRentingDbContext>();

            builder.Services.AddApplicationServices(typeof(IHouseService));

            builder.Services
                .AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
                });

            WebApplication app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapDefaultControllerRoute();
            app.MapRazorPages();

            app.Run();
        }
    }
}