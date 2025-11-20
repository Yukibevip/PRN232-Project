using Microsoft.AspNetCore.Mvc;
using Services; 

namespace PRN232_Project_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected Guid GetCurrentUserId()
        {
            // Read the ID from our static demo service
            var userId = DemoAuthService.CurrentUserId;

            if (userId == null)
            {
                // This will happen if you try to block a user *before* logging in
                throw new InvalidOperationException("No user is logged in for this demo. Please call the /api/users/login endpoint first.");
            }

            return userId.Value;
        }
    }
}