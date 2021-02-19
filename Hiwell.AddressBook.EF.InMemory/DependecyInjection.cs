using Hiwell.AddressBook.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Hiwell.AddressBook.EF.Sqlite
{
    public static class DependecyInjection
    {
        public static void AddSqlite(this IServiceCollection services)
        {
            services.AddDbContext<AddressBookSqliteDbContext>();

            services.AddScoped<IAddressBookDbContext>(provider => provider.GetService<AddressBookSqliteDbContext>());
        }

        public static void EnsureDbCreated(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AddressBookSqliteDbContext>();
                context.Database.EnsureCreated();

                var hasData = context.Contacts.Count() > 0;

                if (!hasData)
                {
                    context.Contacts.Add(new Core.Entities.Contact()
                    {
                        Name = "Contact 1",
                        Address = "Address 1",
                        Email = "contact1@email.com",
                        Phone = "568498752",
                        MobilePhone = "5551112356",
                        Active = true
                    });


                    context.SaveChanges();
                }
            }
        }
    }
}
