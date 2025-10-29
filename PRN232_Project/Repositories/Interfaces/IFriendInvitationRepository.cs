using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IFriendInvitationRepository
    {
        Task CreateInvitation(Guid senderId, Guid receiverId);
        Task AcceptInvitation(int invitationId);
    }
}
