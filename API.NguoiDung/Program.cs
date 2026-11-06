
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QlyDaoTao.BLL.Services;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories;
using QlyDaoTao.DAL.Repositories.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("========== KIEM TRA JWT ==========");
Console.WriteLine("Issuer: " + builder.Configuration["Jwt:Issuer"]);
Console.WriteLine("Audience: " + builder.Configuration["Jwt:Audience"]);
Console.WriteLine("Key co ton tai: " +
    !string.IsNullOrEmpty(builder.Configuration["Jwt:Key"]));
Console.WriteLine("==================================");
// Kết nối SQL Server
builder.Services.AddDbContext<QlyDaoTaoContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Đăng ký Repository và Service
builder.Services.AddScoped<ILichSuDiemDanhService, LichSuDiemDanhService>();
builder.Services.AddScoped<ILichSuDiemDanhRepository, LichSuDiemDanhRepository>();

builder.Services.AddScoped<ITyLeVangRepository, TyLeVangRepository>();
builder.Services.AddScoped<ITyLeVangService, TyLeVangService>();

builder.Services.AddScoped<ILopHocPhanSinhVienRepository, LopHocPhanSinhVienRepository>();
builder.Services.AddScoped<ILopHocPhanSinhVienService, LopHocPhanSinhVienService>();

builder.Services.AddScoped<ICanhBaoVangRepository, CanhBaoVangRepository>();
builder.Services.AddScoped<ICanhBaoVangService, CanhBaoVangService>();

builder.Services.AddScoped<ILichGiangDayRepository, LichGiangDayRepository>();
builder.Services.AddScoped<ILichGiangDayService, LichGiangDayService>();

builder.Services.AddScoped<IThongBaoRepository, ThongBaoRepository>();
builder.Services.AddScoped<IThongBaoService, ThongBaoService>();

builder.Services.AddScoped<IDiemRepository, DiemRepository>();
builder.Services.AddScoped<IDiemService, DiemService>();

builder.Services.AddScoped<IPhucKhaoRepository, PhucKhaoRepository>();
builder.Services.AddScoped<IPhucKhaoService, PhucKhaoService>();

// Đăng ký điểm danh
builder.Services.AddScoped<IDiemDanhRepository, DiemDanhRepository>();
builder.Services.AddScoped<IDiemDanhService, DiemDanhService>();

// Cấu hình xác thực JWT
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
                    builder.Configuration["Jwt:Key"]!
                )
            )
        };
    });

builder.Services.AddAuthorization();

// Controllers
builder.Services.AddControllers();

// Swagger có hỗ trợ JWT
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

// CORS
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
