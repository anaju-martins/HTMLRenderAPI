
namespace HtmlRenderAPI
{
    using HtmlRenderAPI.Services;
    public class Program
    {
        public static void Main(string[] args)
        {
           

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddSingleton<HtmlRenderService>(); //instanciado apenas uma vez
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Program.cs
            builder.Services.AddCors(opts =>
            {
                opts.AddDefaultPolicy(policy =>
                  policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            var app = builder.Build();
            app.UseCors();

          
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
}
