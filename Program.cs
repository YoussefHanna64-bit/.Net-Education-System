
using Education_System.Context;
using Education_System.Filters;
using Education_System.Middleware;
using Education_System.Models;
using Education_System.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;

namespace Education_System
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddDbContext<EduContext>(op =>
			{
				op.UseSqlServer(builder.Configuration.GetConnectionString("dev"));
			});

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
			{
				op.User.RequireUniqueEmail = true;

			}).AddEntityFrameworkStores<EduContext>();

			builder.Services.AddAuthentication(op =>
			{
				op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(op =>
			{
				op.SaveToken = true;
				op.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = "https://localhost:44360/",
					ValidateAudience = true,
					ValidAudience = "https://localhost:44360/",
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("ff996dbb717b65380d2bc55fc9a2b570eed98a8699715f400828a615bd1712c0"))
				};
			});

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			builder.Services.AddControllers(op =>
			{
				op.Filters.Add<ExceptionHandleFilter>();
				op.Filters.Add<CustomResultFilter>();
			});
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

			builder.Services.AddSwaggerGen();

			builder.Host.UseNLog();

			builder.Services.AddCors(op =>
			{
				op.AddPolicy("AllowAll", policy =>
				{
					policy.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
				});
			});

			var app = builder.Build();

			app.UseExceptionHandle();
			app.UseLogging();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseStaticFiles();

			app.UseCors("AllowAll");

			app.MapControllers();

			app.Run();
		}
	}
}
