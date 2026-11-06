using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QlyDaoTao.API.Admin.Middleware;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.BLL.Services;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories;
using QlyDaoTao.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddScoped<INienKhoaRepository, NienKhoaRepository>();
builder.Services.AddScoped<INienKhoaService, NienKhoaService>();
builder.Services.AddScoped<IHocKyRepository, HocKyRepository>();
builder.Services.AddScoped<IHocKyService, HocKyService>();
builder.Services.AddScoped<ILichHocRepository, LichHocRepository>();
builder.Services.AddScoped<ILichHocService, LichHocService>();
builder.Services.AddScoped<ILichThiRepository, LichThiRepository>();
builder.Services.AddScoped<ILichThiService, LichThiService>();
builder.Services.AddScoped<ILopHocPhanRepository, LopHocPhanRepository>();
builder.Services.AddScoped<ILopHocPhanService, LopHocPhanService>();
builder.Services.AddScoped<IDangKyHocPhanRepository,DangKyHocPhanRepository>();
builder.Services.AddScoped<IDangKyHocPhanService,DangKyHocPhanService>();
builder.Services.AddScoped<ILopHocPhanSinhVienRepository,LopHocPhanSinhVienRepository>();
builder.Services.AddScoped<ILopHocPhanSinhVienService,LopHocPhanSinhVienService>();
builder.Services.AddScoped<IDiemDanhRepository, DiemDanhRepository>();
builder.Services.AddScoped<IDiemDanhService, DiemDanhService>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(
            "http://127.0.0.1:5500",
            "http://localhost:5500")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors("Frontend");
// Đo thời gian xử lý request
app.UseMiddleware<RequestTimingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
