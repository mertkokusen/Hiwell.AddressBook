using AutoMapper;
using Hiwell.AddressBook.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hiwell.AddressBook.Core.UseCases
{
    public abstract class BaseQueryHandler<TReq, TRes> : IRequestHandler<TReq, TRes>
        where TReq : IRequest<TRes>
    {
        protected readonly IAddressBookDbContext _context;
        protected readonly IMapper _mapper;

        public BaseQueryHandler(
            IAddressBookDbContext context,
            IMapper mapper
            )
        {
            this._context = context;
            this._mapper = mapper;
        }

        public abstract Task<TRes> Handle(TReq request, CancellationToken cancellationToken);
    }
}
