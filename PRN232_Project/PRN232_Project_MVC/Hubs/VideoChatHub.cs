using BusinessObjects;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.Collections;

namespace PRN232_Project_MVC.Hubs
{
    public class VideoChatHub : Hub
    {
        private static Dictionary<Guid, string> UserList = new Dictionary<Guid, string>();
        private static List<Guid> WaitingList = new List<Guid>();
        private static List<Guid[]> ActiveList = new List<Guid[]>();
        private readonly APIService _apiService;

        public VideoChatHub(APIService apiService)
        {
            _apiService = apiService;
        }

        public async Task SendOffer(string targetConnectionId, string offer) 
            => await Clients.Client(targetConnectionId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);

        public async Task SendAnswer(string targetConnectionId, string answer)
            => await Clients.Client(targetConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);

        public async Task SendIceCandidate(string targetConnectionId, string candidate)
            => await Clients.Client(targetConnectionId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);

        //public async Task UserConnect(string userId)
        //{
        //    User user = await _apiService.GetUserByIdAsync(userId);

        //    if (user != null)
        //    {
        //        UserList.Add(user);
        //    }
        //}

        public async Task SearchOthers(string userId)
        {
            var userIdGuid = Guid.Parse(userId);

            if (userIdGuid == Guid.Empty) return;

            foreach (var activePair in ActiveList)
            {
                if (activePair.Contains(userIdGuid))
                {
                    // Người dùng đã trong danh sách hoạt động, không cần tìm kiếm nữa
                    return;
                }
            }

            if (WaitingList.Count > 0)
            {
                Guid partnerId = WaitingList.FirstOrDefault(uid => uid != userIdGuid);

                if ( partnerId == Guid.Empty) return;

                UserList.TryGetValue(partnerId, out string fromId);

                // Gửi offer đến partner
                Clients.Client(fromId).SendAsync("ReceivePartnerId", Context.ConnectionId , partnerId);

                ActiveList.Add(new Guid[] { userIdGuid, partnerId });

            } else
            {
                WaitingList.Add(userIdGuid);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];

            if (StringValues.IsNullOrEmpty(userId)) return;

            UserList.Add(Guid.Parse(userId), Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];
            if (StringValues.IsNullOrEmpty(userId)) return;
            Guid userIdGuid = Guid.Parse(userId);
            UserList.Remove(userIdGuid);
            WaitingList.Remove(userIdGuid);
            foreach (var pair in ActiveList.ToList())
            {
                if (pair.Contains(userIdGuid))
                {
                    Guid partnerId = pair[0] == userIdGuid ? pair[1] : pair[0];
                    if (UserList.TryGetValue(partnerId, out string partnerConnectionId))
                    {
                        await Clients.Client(partnerConnectionId).SendAsync("PartnerDisconnected");
                    }
                    ActiveList.Remove(pair);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

    }
}
