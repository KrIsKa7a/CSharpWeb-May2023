namespace ChatApp.ViewModels.Chat
{
    public class ChatViewModel
    {
        public ChatViewModel()
        {
            this.AllMessages = new HashSet<MessageViewModel>();
        }

        public MessageViewModel CurrentMessage { get; set; } = null!;

        public ICollection<MessageViewModel> AllMessages { get; set; }
    }
}
