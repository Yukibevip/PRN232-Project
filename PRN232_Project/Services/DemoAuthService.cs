using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class DemoAuthService
    {
        public static Guid? CurrentUserId { get; private set; }

        public static void Login(Guid userId)
        {
            // When a user logs in, we "remember" their ID.
            CurrentUserId = userId;
        }

        public static void Logout()
        {
            CurrentUserId = null;
        }
    }
}
