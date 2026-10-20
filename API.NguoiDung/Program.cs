using QlyDaoTao.BLL.Services;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Repositories;
using QlyDaoTao.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILichSuDiemDanhService,LichSuDiemDanhService>();
builder.Services.AddScoped<ILichSuDiemDanhRepository,LichSuDiemDanhRepository>();
builder.Services.AddScoped<ITyLeVangRepository, TyLeVangRepository>();
builder.Services.AddScoped<ITyLeVangService, TyLeVangService>();
builder.Services.AddScoped<ILopHocPhanSinhVienRepository,LopHocPhanSinhVienRepository>();
builder.Services.AddScoped<ILopHocPhanSinhVienService,LopHocPhanSinhVienService>();
builder.Services.AddScoped<ICanhBaoVangRepository, CanhBaoVangRepository>();
builder.Services.AddScoped<ICanhBaoVangService, CanhBaoVangService>();

builder.Services.AddDbContext<QlyDaoTaoContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
