using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using Hiwell.AddressBook.Core.Interfaces;

namespace Hiwell.AddressBook.Core.UseCases
{
    public abstract class BaseCommandHandler<TReq, TRes> : IRequestHandler<TReq, TRes>
        where TReq : IRequest<TRes>
        where TRes : BaseCommandResult
    {
        protected readonly IAddressBookDbContext _context;
        protected readonly IMapper _mapper;

        public BaseCommandHandler(
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
