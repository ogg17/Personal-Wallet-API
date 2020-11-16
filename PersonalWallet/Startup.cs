using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PersonalWallet.Models;
 
namespace PersonalWallet {
    public class Startup {
        private IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration) => Configuration = configuration; 
        
        public void ConfigureServices(IServiceCollection services) {
            // set data context
            services.AddDbContext<UsersContext>(options => 
                options.UseSqlite(Configuration.GetConnectionString("DefaultDataBasePath")));
            
            // use controllers without views
            services.AddControllers(); 
        }
 
        public void Configure(IApplicationBuilder app) {
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            // connect routing to controllers
            app.UseEndpoints(endpoints => {endpoints.MapControllers();});
        }
    }
}