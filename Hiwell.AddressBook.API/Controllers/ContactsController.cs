using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hiwell.AddressBook.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IAddressBookDbContext dbContent;
        public ContactsController(AddressBook.Core.Interfaces.IAddressBookDbContext dbContent)
        {
            this.dbContent = dbContent;
        }

        [HttpGet]
        public async Task<IEnumerable<Contact>> GetAll()
        {
            return await this.dbContent.Contacts.ToListAsync();
        }
    }
}
