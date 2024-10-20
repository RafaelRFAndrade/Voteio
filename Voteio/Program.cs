using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Voteio.Interfaces.Repository;
using Voteio.Repository;
using Voteio.Repository.Base;
using Voteio.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //grosso dos reposi
        builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddTransient<IIdeiasRepository, IdeiasRepository>();
        builder.Services.AddTransient<IComentarioRepository, ComentarioRepository>();
        builder.Services.AddTransient<IVotesRepository, VotesRepository>();

        //grosso dos services kkk
        builder.Services.AddTransient<UsuarioService>();
        builder.Services.AddTransient<TokenService>();
        builder.Services.AddTransient<IdeiasService>();  

        builder.Services.AddDbContext<RepositoryBase>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(9, 1, 0)) 
            )
        );

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });

        //isso talvez faça um inferno, comente se necessário
        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.UseCors("AllowSpecificOrigin");

        // tem mas to caganu pras pipes kkkkk.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        //esse tabm
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}