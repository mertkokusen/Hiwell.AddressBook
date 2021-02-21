using MediatR;
using System.Collections.Generic;
using Hiwell.AddressBook.Core.Dtos;
using System.Threading.Tasks;
using System.Threading;
using Hiwell.AddressBook.Core.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace Hiwell.AddressBook.Core.UseCases
{
    public class GetAllContactsQuery: IRequest<List<ContactDto>>
    {
    }

    public class GetAllContactsQueryHandler : BaseQueryHandler<GetAllContactsQuery, List<ContactDto>>
    {
        public GetAllContactsQueryHandler(IAddressBookDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override Task<List<ContactDto>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        {
            return this._context.Contacts.AsNoTracking().ProjectTo<ContactDto>(this._mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
