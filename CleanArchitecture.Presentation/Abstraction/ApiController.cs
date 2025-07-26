using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Abstraction;

//Her Controller bunları içereceği için temel abstract bunu oluşturduk.
//Controller oluşturduğumuzda bunu inherit ederiz

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
	public readonly IMediator _mediator;
	protected ApiController(IMediator mediator)
	{
		_mediator = mediator;
	}
}
