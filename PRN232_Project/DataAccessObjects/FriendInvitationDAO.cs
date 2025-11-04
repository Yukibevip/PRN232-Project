using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class FriendInvitationDAO
    {
        private readonly CallioTestContext _context;
        public FriendInvitationDAO(CallioTestContext context) { _context = context; }

        public Task Add(FriendInvitation invitation)
        {
            _context.FriendInvitations.Add(invitation);
            return _context.SaveChangesAsync();
        }

        public Task<FriendInvitation?> GetById(int invitationId)
        {
            return _context.FriendInvitations.FindAsync(invitationId).AsTask();
        }

        public Task Remove(FriendInvitation invitation)
        {
            _context.FriendInvitations.Remove(invitation);
            return _context.SaveChangesAsync();
        }

        public Task<bool> Exists(Guid senderId, Guid receiverId)
        {
            return _context.FriendInvitations.AnyAsync(i =>
                (i.SenderId == senderId && i.ReceiverId == receiverId) ||
                (i.SenderId == receiverId && i.ReceiverId == senderId));
        }
    }
}
