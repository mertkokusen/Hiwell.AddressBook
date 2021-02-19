using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Hiwell.AddressBook.Core.Dtos;
using Hiwell.AddressBook.Core.UseCases;

namespace Hiwell.AddressBook.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ContactsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<List<ContactDto>> GetAll()
        {
            return await this.mediator.Send(new GetAllContactsQuery());
        }

        [HttpPost]
        public async Task<AddNewContactCommandResponse> AddNewContact(AddNewContactCommandRequest request)
        {
            return await this.mediator.Send(request);
        }

        [HttpDelete]
        public async Task<DeleteContactCommandResponse> DeleteContact(DeleteContactCommandRequest request)
        {
            return await this.mediator.Send(request);
        }
    }
}
