using AutoMapper;
using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Hiwell.AddressBook.Core.UseCases
{
    public class DeleteContactCommandRequest : IRequest<DeleteContactCommandResponse>
    {
        [Required]
        public string UniqueKey { get; set; }
    }

    public class DeleteContactCommandResponse : BaseCommandResult
    {
        private DeleteContactCommandResponse(string error = null) : base(error) { }
        public static DeleteContactCommandResponse Ok() => new DeleteContactCommandResponse();
        public static DeleteContactCommandResponse Fail(string error) => new DeleteContactCommandResponse(error);
    }

    public class DeleteContactCommandHandler : BaseCommandHandler<DeleteContactCommandRequest, DeleteContactCommandResponse>
    {
        public DeleteContactCommandHandler(IAddressBookDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<DeleteContactCommandResponse> Handle(DeleteContactCommandRequest request, CancellationToken cancellationToken)
        {
            var contact = await this._context.Contacts.FirstOrDefaultAsync(c => c.UniqueId == request.UniqueKey);
            if (contact is null)
            {
                return DeleteContactCommandResponse.Fail("A contact with the given unique key does not exist.");
            }

            this._context.Contacts.Remove(contact);

            var result = await this._context.SaveChangesAsync(cancellationToken);

            if (result == 1)
                return DeleteContactCommandResponse.Ok();
            else
                return DeleteContactCommandResponse.Fail("Couldn't delete contact.");
        }
    }
}
