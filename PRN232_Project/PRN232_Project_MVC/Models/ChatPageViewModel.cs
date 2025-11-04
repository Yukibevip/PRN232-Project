
namespace PRN232_Project_MVC.Models
{
    public class ChatPageViewModel
    {
        // 1. The full list of friends for the sidebar
        public List<FriendViewModel> Friends { get; set; } = new List<FriendViewModel>();

        // 2. The friend you are currently chatting with
        public FriendViewModel? ChattingWith { get; set; }

        // 3. The message history for this chat
        public List<MessageViewModel> ConversationHistory { get; set; } = new List<MessageViewModel>();
        public bool IsChatBlocked { get; set; } = false;
    }
}
