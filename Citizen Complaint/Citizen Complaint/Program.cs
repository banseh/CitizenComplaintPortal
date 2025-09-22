using System.Text;
using Citizen_Complaint.BL.Extentions;
using Citizen_Complaint.BL.Validators;
using Citizen_Complaint.DAL;
using Citizen_Complaint.DAL.Extentions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Citizen_Complaint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DALExtention
            builder.Services.AddDALExtention();
            #endregion

            #region BLExtention
            builder.Services.AddBLExtention();
            #endregion

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(option =>
             {
                 var jwtSettings = builder.Configuration.GetSection("Jwt");
                 var secretKey = jwtSettings["Key"];
                 var secretKeyInByte = Encoding.UTF8.GetBytes(secretKey);
                 var key = new SymmetricSecurityKey(secretKeyInByte);
                 option.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     IssuerSigningKey = key,
                 };
             });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();
            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
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
        }
    }
}
