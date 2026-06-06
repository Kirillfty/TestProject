using MediatR;
using Microsoft.EntityFrameworkCore;
using Test.Backend.Database;
using Test.Backend.Database.Entitty;

namespace Test.Backend.Features.Contacts.Queries.GetAll;

public class GetAllContactsQuery : IRequest<IReadOnlyList<Contact>>;

public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, IReadOnlyList<Contact>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllContactsQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Contact>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Contacts.AsNoTracking().ToListAsync(cancellationToken);
    }
}