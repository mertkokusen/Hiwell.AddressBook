using AutoMapper;
using FluentValidation;
using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.Core.Extensions;
using Hiwell.AddressBook.Core.Interfaces;
using Hiwell.AddressBook.Core.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Hiwell.AddressBook.Core.UseCases
{
    public class AddNewContactCommandRequest : IRequest<AddNewContactCommandResponse>, IMapTo<Contact>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
    }

    public class AddNewContactCommandRequestValidator : AbstractValidator<AddNewContactCommandRequest>
    {
        public AddNewContactCommandRequestValidator()
        {
            RuleFor(r => r.Email).MustBeValidEmail();
        }
    }

    public class AddNewContactCommandResponse : BaseCommandResult
    {
        private AddNewContactCommandResponse(string error = null) : base(error)
        {
        }

        public static AddNewContactCommandResponse Ok()
        {
            return new AddNewContactCommandResponse();
        }

        public static AddNewContactCommandResponse Fail(string error)
        {
            return new AddNewContactCommandResponse(error);
        }
    }

    public class AddNewContactCommandHandler : BaseCommandHandler<AddNewContactCommandRequest, AddNewContactCommandResponse>
    {
        public AddNewContactCommandHandler(IAddressBookDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<AddNewContactCommandResponse> Handle(AddNewContactCommandRequest request, CancellationToken cancellationToken)
        {
            var contactAlreadyExists = await this._context.Contacts.AnyAsync(c => c.Name == request.Name);
            if (contactAlreadyExists)
            {
                return AddNewContactCommandResponse.Fail("A contact with the same name already exists.");
            }

            var newContact = this._mapper.Map<Contact>(request);
            newContact.Active = true;
            newContact.UniqueId = Guid.NewGuid().ToString("N");

            this._context.Contacts.Add(newContact);

            var result = await this._context.SaveChangesAsync(cancellationToken);

            if (result == 1)
                return AddNewContactCommandResponse.Ok();
            else
                return AddNewContactCommandResponse.Fail("Couldn't create contact.");
        }
    }
}
