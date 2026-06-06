using MediatR;
using Test.Backend.Database;
using Test.Backend.Database.Entitty;

namespace Test.Backend.Features.Contacts.Commands.AddContact;

public record AddContactCommand(string Name, string PhoneNumber, string JobTitle, DateTime BirthDate)
    : IRequest<Contact>;

public class AddContactCommandHandler : IRequestHandler<AddContactCommand, Contact>
{
    private readonly ApplicationDbContext _db;
    public AddContactCommandHandler(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<Contact> Handle(AddContactCommand request, CancellationToken cancellationToken)
    {
        var entity = new Contact
        {
            Name =  request.Name,
            JobTitle = request.JobTitle,
            PhoneNumber =  request.PhoneNumber,
            BirthDate = request.BirthDate
        };
        
        await _db.AddAsync(entity);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return null;
        }

        return entity;
    }
}