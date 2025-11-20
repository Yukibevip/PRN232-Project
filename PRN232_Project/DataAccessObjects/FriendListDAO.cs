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
    public class FriendListDAO
    {
        private readonly CallioTestContext _context;
        private readonly IMapper _mapper;
        public FriendListDAO(CallioTestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<bool> AreUsersFriends(Guid userId1, Guid userId2)
        {
            return _context.FriendLists.AnyAsync(f =>
                (f.UserId1 == userId1 && f.UserId2 == userId2) ||
                (f.UserId1 == userId2 && f.UserId2 == userId1));
        }
        public Task<List<Guid>> GetFriendIdsForUser(Guid userId)
        {
            return _context.FriendLists
                .Where(f => f.UserId1 == userId)
                .Select(f => f.UserId2)
                .ToListAsync();
        }
        public Task<List<FriendList>> GetFriendshipRecords(Guid userId1, Guid userId2)
        {
            return _context.FriendLists.Where(f =>
                (f.UserId1 == userId1 && f.UserId2 == userId2) ||
                (f.UserId1 == userId2 && f.UserId2 == userId1))
                .ToListAsync();
        }
        public void RemoveFriendshipRange(List<FriendList> friendshipRecords)
        {
            _context.FriendLists.RemoveRange(friendshipRecords);
        }
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
        public Task AddFriendship(FriendList friendship)
        {
            _context.FriendLists.Add(friendship);
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FriendListDto>> GetFriendLists()
        {
            var result = await _context.FriendLists.Include(f => f.UserId1Navigation).Include(f => f.UserId2Navigation).ToListAsync();
            var dto = _mapper.Map<IEnumerable<FriendList>, IEnumerable<FriendListDto>>(result);
            return dto;
        }

        public async Task<bool> RemoveFriendShip(Guid userId1, Guid userId2)
        {
            var friendships = await GetFriendshipRecords(userId1, userId2);
            if (friendships == null || friendships.Count == 0)
            {
                return false;
            }
            RemoveFriendshipRange(friendships);
            await SaveChangesAsync();
            return true;
        }
    }
}
