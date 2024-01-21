using Booking.Auth.Application.Features.AuthFeatures;
using Booking.Auth.WebAPI.Models;
using Booking.Auth.WebAPI.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Auth.WebAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(IJwtTokenGenerator jwtTokenGenerator, IMediator mediator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<TokenDto>> Login(AuthenticateRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return response.success ? Ok(_jwtTokenGenerator.GenerateToken(request.Email, response.roleName)) : Unauthorized();
    }
}