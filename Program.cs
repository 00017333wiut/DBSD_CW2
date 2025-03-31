using CW2.DAL.Entities;
using CW2.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
/*using CW2.DAL.Ef;
using CW2.DAL.Repositories;*/

var builder = WebApplication.CreateBuilder(args);

IConfiguration conf = builder.Configuration;

string connStr = conf.GetConnectionString("ArtRentLocalDB");
connStr = connStr.Replace("|DbDir|", builder.Environment.ContentRootPath);

builder.Services.AddSingleton<IStaffRepository>
    (s => new AdoNetStaffRepository(connStr));

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
    pattern: "{controller=Staff}/{action=Index}/{id?}");

app.Run();
