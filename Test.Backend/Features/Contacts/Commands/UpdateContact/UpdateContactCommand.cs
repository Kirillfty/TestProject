using MediatR;
using Microsoft.EntityFrameworkCore;
using Test.Backend.Common.Exceptions;
using Test.Backend.Database;
using Test.Backend.Database.Entitty;

namespace Test.Backend.Features.Contacts.Commands.UpdateContact;

public record UpdateContactCommand(
    int Id,
    string Name,
    string PhoneNumber,
    string JobTitle,
    DateTime BirthDate
) : IRequest<Contact>;

public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Contact>
{
    private readonly ApplicationDbContext _context;

    public UpdateContactCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Contact> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _context.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (contact is null)
        {
            throw new NotFoundException(nameof(Contact), request.Id.ToString());
        }
        
        contact.Name = request.Name;
        contact.JobTitle = request.JobTitle;
        contact.PhoneNumber = request.PhoneNumber;
        contact.BirthDate = request.BirthDate;

        _context.Contacts.Update(contact);
        
        await _context.SaveChangesAsync(cancellationToken);

        return contact;
    }
}