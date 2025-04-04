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


builder.Services.AddDbContext<DbContext>(options =>
    options.UseSqlServer(connStr)
               .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole().AddDebug()))

    );

//builder.Services.AddSingleton<IArtworkRepository>
//    (s => new AdoNetArtworkRepository(connStr));

builder.Services.AddSingleton<IArtworkRepository>(
    s => new DapperStoredProcArtworkRepository(connStr)
    );

//builder.Services.AddSingleton<IArtworkRepository>(
//    s => new DapperArtworkRepository(connStr)
//    );


// builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();


// Configure the HTTP request pipeline.
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
