using MediatR;
using Microsoft.AspNetCore.Mvc;
using test.Database.Entitty;
using test.Features.Contacts.Commands;
using test.Features.Contacts.Commands.DeleteContact;
using test.Features.Contacts.Commands.UpdateContact;
using test.Features.Contacts.Queries.GetAll;
using test.Features.Contacts.Queries.GetById;

namespace test.Controller;

[ApiController]
[Route("[controller]")]
public class ContactController:ControllerBase
{
    
    private readonly ISender _sender;

    public ContactController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    public async Task<IReadOnlyList<Contact>> GetAll(CancellationToken cancellationToken)
    {
        return await _sender.Send(new GetAllContactsQuery(),cancellationToken);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddContactCommand request, CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(request, cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<Contact> GetById([FromRoute]int id, CancellationToken cancellationToken)
    {
        return await _sender.Send(new GetContactByIdQuery(id),cancellationToken);
    }
    
    [HttpPut]
    public async Task<Contact> Update([FromBody] UpdateContactCommand request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request,cancellationToken);
    }

    [HttpDelete("{id:int}")]
    public async Task<Contact> Delete([FromRoute]int id,CancellationToken cancellationToken)
    {
        return await _sender.Send(new DeleteContactCommand(id), cancellationToken);
    }
}