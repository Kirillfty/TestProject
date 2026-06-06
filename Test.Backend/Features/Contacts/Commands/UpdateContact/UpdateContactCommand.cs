using MediatR;
using Microsoft.EntityFrameworkCore;
using test.Common.Exceptions;
using test.Database;
using test.Database.Entitty;

namespace test.Features.Contacts.Commands.UpdateContact;

public record UpdateContactCommand(
    int Id,
    string Name,
    string PhoneNumber,
    string JobTitle,
    DateTime BitrhDate
    ) : IRequest<Contact>;

public class UpdateContactCommandHandler:IRequestHandler<UpdateContactCommand,Contact>
{
    private readonly ApplicationDbContext _context;

    public UpdateContactCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Contact> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var oldContact = await _context.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        if (oldContact is null)
        {
            throw new NotFoundException<int>(nameof(Contact),request.Id);
        }

        var updateContact = new Contact
        {
            Id = oldContact.Id,
            Name =  request.Name,
            JobTitle = request.JobTitle,
            Phonenumber = request.PhoneNumber,
            BirthDate = request.BitrhDate
        };

        _context.Contacts.Update(updateContact);
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            return null;
        }
        return updateContact;
    }
}