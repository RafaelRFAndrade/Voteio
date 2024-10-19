using Microsoft.EntityFrameworkCore;
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

        //grosso dos services kkk
        builder.Services.AddTransient<UsuarioService>();

        builder.Services.AddDbContext<RepositoryBase>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(9, 1, 0)) 
            )
        );


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
    }
}