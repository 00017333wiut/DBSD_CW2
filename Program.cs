using CW2.DAL.EF;
using CW2.DAL.Entities;
using CW2.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
/*using CW2.DAL.Ef;
using CW2.DAL.Repositories;*/

var builder = WebApplication.CreateBuilder(args);

IConfiguration conf = builder.Configuration;

builder.Services.AddAutoMapper(typeof(Program));

string connStr = conf.GetConnectionString("ArtRentLocalDB");
connStr = connStr.Replace("|DbDir|", builder.Environment.ContentRootPath);


builder.Services.AddDbContext<ArtRentDbContext>(options =>
    options.UseSqlServer(connStr)
               .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole().AddDebug()))

    );

//AdoNet
//builder.Services.AddSingleton<IArtworkRepository>
//    (s => new AdoNetArtworkRepository(connStr));

//DapperStoredProcedures
builder.Services.AddSingleton<IArtworkRepository>(
    s => new DapperStoredProcArtworkRepository(connStr)
    );

//Dapper
/*builder.Services.AddSingleton<IArtworkRepository>(
    s => new DapperArtworkRepository(connStr)
    );*/


//DbContext
builder.Services.AddDbContext<ArtRentDbContext>(options =>
    options.UseSqlServer(connStr)
);


//builder.Services.AddScoped<IArtworkRepository, EfArtworkRepository>();


builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();

//builder.Services.AddScoped<IArtworkRepository, EfArtworkRepository>();

builder.Services.AddControllersWithViews();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Artwork}/{action=Index}/{id?}");

app.Run();
