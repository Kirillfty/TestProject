using MediatR;
using Microsoft.EntityFrameworkCore;
using Test.Backend.Common.Exceptions;
using Test.Backend.Database;
using Test.Backend.Database.Entitty;

namespace Test.Backend.Features.Contacts.Commands.DeleteContact;

public record DeleteContactCommand(int Id) : IRequest;

public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteContactCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _context.Contacts.FirstOrDefaultAsync(
            u => u.Id == request.Id,
            cancellationToken: cancellationToken
        );

        if (contact is null)
        {
            throw new NotFoundException(nameof(Contact), request.Id.ToString());
        }

        _context.Contacts.Remove(contact);

        await _context.SaveChangesAsync(cancellationToken);
    }
}