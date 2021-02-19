using AutoMapper;
using FluentValidation;
using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.Core.Extensions;
using Hiwell.AddressBook.Core.Interfaces;
using Hiwell.AddressBook.Core.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hiwell.AddressBook.Core.UseCases
{
    public class UpdateContactCommandRequest : IRequest<UpdateContactCommandResponse>, IMapTo<Contact>
    {
        [Required]
        public string UniqueId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
    }

    public class UpdateContactCommandRequestValidator : AbstractValidator<AddNewContactCommandRequest>
    {
        public UpdateContactCommandRequestValidator()
        {
            RuleFor(r => r.Email).MustBeValidEmail();
        }
    }

    public class UpdateContactCommandResponse : BaseCommandResult
    {
        private UpdateContactCommandResponse(string error = null) : base(error)
        {
        }

        public static UpdateContactCommandResponse Ok()
        {
            return new UpdateContactCommandResponse();
        }

        public static UpdateContactCommandResponse Fail(string error)
        {
            return new UpdateContactCommandResponse(error);
        }
    }

    public class UpdateContactCommandHandler : BaseCommandHandler<UpdateContactCommandRequest, UpdateContactCommandResponse>
    {
        public UpdateContactCommandHandler(IAddressBookDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<UpdateContactCommandResponse> Handle(UpdateContactCommandRequest request, CancellationToken cancellationToken)
        {
            var contact = await this._context.Contacts.FirstOrDefaultAsync(c => c.UniqueId == request.UniqueId);
            if (contact is null)
            {
                return UpdateContactCommandResponse.Fail("A contact with the given unique key does not exist.");
            }

            var newNameUnavailable = await this._context.Contacts.AnyAsync(c => c.UniqueId != request.UniqueId && c.Name == request.Name);
            if (newNameUnavailable)
            {
                return UpdateContactCommandResponse.Fail("A contact with the given name already exists.");
            }

            var updatedContact = this._mapper.Map(request,contact);
            
            this._context.Contacts.Update(updatedContact);

            var result = await this._context.SaveChangesAsync(cancellationToken);

            if (result == 1)
                return UpdateContactCommandResponse.Ok();
            else
                return UpdateContactCommandResponse.Fail("Couldn't update contact.");
        }
    }
}
