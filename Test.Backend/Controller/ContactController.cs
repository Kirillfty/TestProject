using MediatR;
using Microsoft.AspNetCore.Mvc;
using Test.Backend.Database.Entitty;
using Test.Backend.Features.Contacts.Commands.AddContact;
using Test.Backend.Features.Contacts.Commands.DeleteContact;
using Test.Backend.Features.Contacts.Commands.UpdateContact;
using Test.Backend.Features.Contacts.Queries.GetAll;
using Test.Backend.Features.Contacts.Queries.GetById;

namespace Test.Backend.Controller;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly ISender _sender;

    public ContactController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IReadOnlyList<Contact>> GetAll(CancellationToken cancellationToken)
    {
        return await _sender.Send(new GetAllContactsQuery(), cancellationToken);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddContactCommand request, CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(request, cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<Contact> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        return await _sender.Send(new GetContactByIdQuery(id), cancellationToken);
    }

    [HttpPut]
    public async Task<Contact> Update([FromBody] UpdateContactCommand request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    public async Task Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteContactCommand(id), cancellationToken);
    }
}