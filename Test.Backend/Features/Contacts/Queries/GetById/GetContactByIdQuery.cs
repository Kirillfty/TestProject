using MediatR;
using Microsoft.EntityFrameworkCore;
using test.Common.Exceptions;
using test.Database;
using test.Database.Entitty;

namespace test.Features.Contacts.Queries.GetById;

public record GetContactByIdQuery(int Id) : IRequest<Contact>;


public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, Contact>
{
    private readonly ApplicationDbContext _context;

    public GetContactByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Contact> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        var contact = await _context.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(u=>u.Id == request.Id,cancellationToken);

        if (contact is null)
        {
            throw new NotFoundException<int>(nameof(Contact),request.Id);
        }
        
        return contact;
    }
}


