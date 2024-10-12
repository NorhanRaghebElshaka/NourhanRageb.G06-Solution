using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NourhanRageb.G06.BLL.interfaces;
using NourhanRageb.G06.BLL.Repositories;
using NourhanRageb.G06.BLL;
using NourhanRageb.G06.DAL.Data.Contexts;
using NourhanRageb.G06.DAL.Models;
using NourhanRageb.G06.PL.Mapping.Employees;
using System.Reflection;

namespace NourhanRageb.G06.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddScoped<AppDbContext>(); // Allow Dependancy Ingecation For AppDbContext

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();//Allow Dependancy Ingecation For DepartmentRepository
          //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();//Allow Dependancy Ingecation For EmployeeRepository
            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
             
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddDefaultTokenProviders();//Allow Dependancy Ingecation For Identity Role
            
            builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            // Life Time
            //builder.Services.AddScoped();    // LifeTime Rar Request , Object UnReachable
            //builder.Services.AddTransient(); // LifeTime Rar Operations
            //builder.Services.AddSingleton(); // LifeTime Rar Application

            //builder.Services.AddScoped<IScopedServices , ScopedServices>();         // Par Request
            //builder.Services.AddTransient<ITranientServices, TranientServices>();   // Par Operations
            //builder.Services.AddSingleton<ISingletonServices, SingletonServices>(); // Par App

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/sIgnIn";
            });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute
            (
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}
