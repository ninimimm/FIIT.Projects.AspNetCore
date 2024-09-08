using Microsoft.EntityFrameworkCore;
using Service.ApplicationDbContext;
using Service.Comments;
using Service.Passports;
using Service.SessionsNumber;
using Service.Users;
using Telegram.Bot;

namespace Api
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddScoped<CommentsRepository>();
            services.AddScoped<PassportsRepository>();
            services.AddScoped<SessionsNumberRepository>();
            services.AddScoped<UsersRepository>();
            services.AddScoped<TelegramBot.TelegramBot>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<ITelegramBotClient>(_ =>
                new TelegramBotClient("6599160966:AAEPk4mP04rI5jzHQJr65a4xlyHRQIUCygk"));
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}