using MediatR;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using test.Common.Exceptions;
using test.Database;
using test.Database.Entitty;

namespace test.Features.Contacts.Commands.DeleteContact;

public record DeleteContactCommand(int id) : IRequest<Contact>;
public class DeleteContactCommandHandler: IRequestHandler<DeleteContactCommand, Contact>
{
    private readonly ApplicationDbContext _context;

    public DeleteContactCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<Contact> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _context.Contacts.FirstOrDefaultAsync(u => u.Id == request.id);
        
        if (contact is null)
        {
            throw new NotFoundException<int>(nameof(Contact), request.id);
        }
        
        _context.Contacts.Remove(contact);

        try
        {
            _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return null;
        }
        
        return contact;
    }
}
