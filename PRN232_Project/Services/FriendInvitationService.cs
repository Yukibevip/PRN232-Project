using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FriendInvitationService : IFriendInvitationService
    {
        private readonly IFriendInvitationRepository _friendInvitationRepository;
        public FriendInvitationService(IFriendInvitationRepository friendInvitationRepository)
        {
            _friendInvitationRepository = friendInvitationRepository;
        }
    }
}
