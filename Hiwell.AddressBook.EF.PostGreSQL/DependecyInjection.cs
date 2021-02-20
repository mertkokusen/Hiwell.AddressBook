using Hiwell.AddressBook.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Hiwell.AddressBook.EF.PostGreSQL
{
    public static class DependecyInjection
    {
        public static void AddPostGreSql(this IServiceCollection services)
        {
            services.AddDbContext<AddressBookPostGreDbContext>();

            services.AddScoped<IAddressBookDbContext>(provider => provider.GetService<AddressBookPostGreDbContext>());
        }

        public static void EnsurePostGreDbCreated(this IApplicationBuilder app)
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AddressBookPostGreDbContext>();
                context.Database.EnsureCreated();

                var hasData = context.Contacts.Count() > 0;

                //TODO: Generate random data with BOGUS 
                //https://github.com/bchavez/Bogus
                if (!hasData)
                {
                    context.Contacts.Add(new Core.Entities.Contact()
                    {
                        Name = "Contact 1",
                        Address = "Address 1",
                        Email = "contact1@email.com",
                        Phone = "568498752",
                        MobilePhone = "5551112356",
                        Active = true,
                        UniqueId = Guid.NewGuid().ToString("N")
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
