using Microsoft.AspNetCore.Builder;
using Smart_Invoice.Utility;
using System.Configuration;

namespace Smart_Invoice
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
           
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
