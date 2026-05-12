
using Education_System.Context;
using Education_System.Filters;
using Education_System.Middleware;
using Education_System.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

			//app.UseAuthorization();
			app.UseStaticFiles();

			app.UseCors("AllowAll");

			app.MapControllers();

			app.Run();
		}
	}
}
