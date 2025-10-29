using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PRN232_Project_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected Guid GetCurrentUserId()
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            //if (userIdClaim == null)
            //{
            //    throw new InvalidOperationException("User ID claim not found in token. Ensure the endpoint is protected with [Authorize].");
            //}
            //return Guid.Parse(userIdClaim.Value);
            return new Guid("exmaple"); // - A - REAL - USER - ID - FROM - YOUR - DATABASE - HERE"
            // It finds the 'NameIdentifier' claim that the middleware
            // extracted from the JWT for this specific request.
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            // It returns the dynamic ID from the token.
            //return Guid.Parse(userIdClaim.Value);
        }
    }
}