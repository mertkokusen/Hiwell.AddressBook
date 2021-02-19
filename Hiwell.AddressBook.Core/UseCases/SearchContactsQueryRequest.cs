using MediatR;
using System.Collections.Generic;
using Hiwell.AddressBook.Core.Dtos;
using System.Threading.Tasks;
using System.Threading;
using Hiwell.AddressBook.Core.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace Hiwell.AddressBook.Core.UseCases
{
    public class SearchContactsQueryRequest: IRequest<List<ContactDto>>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
    }

    class SearchContactsQueryHandler : BaseQueryHandler<SearchContactsQueryRequest, List<ContactDto>>
    {
        public SearchContactsQueryHandler(IAddressBookDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override Task<List<ContactDto>> Handle(SearchContactsQueryRequest request, CancellationToken cancellationToken)
        {
            var searchQueryStatement = this._context.Contacts.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                searchQueryStatement = searchQueryStatement.Where(c => c.Name.ToUpper().Contains(request.Name.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                searchQueryStatement = searchQueryStatement.Where(c => c.Address.ToUpper().Contains(request.Address.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                searchQueryStatement = searchQueryStatement.Where(c => c.Phone.Contains(request.Phone));
            }

            if (!string.IsNullOrWhiteSpace(request.MobilePhone))
            {
                searchQueryStatement = searchQueryStatement.Where(c => c.MobilePhone.Contains(request.MobilePhone));
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                searchQueryStatement = searchQueryStatement.Where(c => c.Email.ToUpper().Contains(request.Email.ToUpper()));
            }

            return searchQueryStatement.ProjectTo<ContactDto>(this._mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
