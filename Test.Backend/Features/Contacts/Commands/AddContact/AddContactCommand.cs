using MediatR;
using test.Database;
using test.Database.Entitty;

namespace test.Features.Contacts.Commands;

public record AddContactCommand(int Id, string Name, string PhoneNumber, string JobTitle, DateTime BitrhDate)
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
            Id = request.Id,
            Name =  request.Name,
            JobTitle = request.JobTitle,
            Phonenumber =  request.PhoneNumber,
            BirthDate = request.BitrhDate
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