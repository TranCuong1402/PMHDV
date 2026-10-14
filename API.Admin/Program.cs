using Microsoft.EntityFrameworkCore;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.BLL.Services;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories;
using QlyDaoTao.DAL.Repositories.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QlyDaoTaoContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddScoped<ISinhVienRepository, SinhVienRepository>();
builder.Services.AddScoped<ISinhVienService, SinhVienService>();
builder.Services.AddScoped<IGiangVienRepository, GiangVienRepository>();
builder.Services.AddScoped<IGiangVienService, GiangVienService>();
builder.Services.AddScoped<IMonHocRepository, MonHocRepository>();
builder.Services.AddScoped<IMonHocService, MonHocService>();
builder.Services.AddScoped<ILopRepository, LopRepository>();
builder.Services.AddScoped<ILopService, LopService>();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();