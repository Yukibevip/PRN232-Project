using AutoMapper;
using BusinessObjects;
using BusinessObjects.Dto;
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
        private readonly IMapper _mapper;
        public FriendInvitationDAO(CallioTestContext context, IMapper mapper) 
        {
            _context = context; 
            _mapper = mapper;
        }

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

        public async Task<IEnumerable<FriendInvitationDto>> GetFriendInvitations()
        {
            var result = await _context.FriendInvitations.Include(fi => fi.Sender).Include(fi => fi.Receiver).ToListAsync();
            var dto = _mapper.Map<IEnumerable<FriendInvitation>, IEnumerable<FriendInvitationDto>>(result);
            return dto;
        }

        public async Task<bool> RemoveFriendInvitation(Guid userId1, Guid userId2)
        {
            var friendInvitation= await _context.FriendInvitations.FirstOrDefaultAsync(fi => (fi.SenderId == userId1 && fi.ReceiverId == userId2) ||
                                                                 (fi.SenderId == userId2 && fi.ReceiverId == userId1));
            _context.FriendInvitations.Remove(friendInvitation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
