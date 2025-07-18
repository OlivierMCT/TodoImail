using Microsoft.AspNetCore.Components;
using TodoImail.BlazorApp.Models;
using TodoImail.BlazorApp.Services;

namespace TodoImail.BlazorApp.Components.Pages {
    public partial class ImailChat {
        [Inject]
        public required IImailChatRoom ChatRoom { get; set; }
        public bool IsConnected { get; set; }
        public string? Pseudo { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public List<string> SelectedPseudos { get; set; } = [];
        public string Senders => SelectedPseudos.Count != 0 ? string.Join(", ", SelectedPseudos) : "tout le monde";
        public void ToggleSelectedPseudo(string pseudo) {
            if(!SelectedPseudos.Remove(pseudo)) SelectedPseudos.Add(pseudo);
        }

        public IImailChatUser? ChatUser { get; private set; }
        public List<string> ConnectedPseudos { get; set; } = [];
        public List<ChatMessageEventArgs> Messages { get; set; } = [];

        public async Task Connect() {
            if (string.IsNullOrWhiteSpace(Pseudo)) return;
            try {
                ChatUser = await ChatRoom.Connect(Pseudo);
                
                ChatUser.ConnectedUser += ChatUser_ConnectedUser;
                ChatUser.DisconnectedUser += ChatUser_DisconnectedUser;
                ChatUser.NewMessage += ChatUser_NewMessage;

                IsConnected = true;
                SelectedPseudos.Clear();
                ConnectedPseudos.Clear();
                ConnectedPseudos.AddRange(ChatRoom.ConnectedUsers.Where(p => p != Pseudo));
                Messages.Clear();
            }catch(InvalidOperationException ex) {
                ErrorMessage = ex.Message;
            }
        }

        private void ChatUser_NewMessage(object sender, ChatMessageEventArgs args) {
            Messages.Add(args);
            InvokeAsync(StateHasChanged);
        }
        private void ChatUser_DisconnectedUser(object sender, ChatUserEventArgs args) {
            ConnectedPseudos.Remove(args.Pseudo);
            InvokeAsync(StateHasChanged);
        }
        private void ChatUser_ConnectedUser(object sender, ChatUserEventArgs args) {
            ConnectedPseudos.Add(args.Pseudo);
            InvokeAsync(StateHasChanged);
        }

        public async Task Disconnect() {
            if (ChatUser != null) {
                await ChatRoom.Disconnect(ChatUser);
                ChatUser = null;
            }
        }
        public void SendMessage() {
            if (ChatUser != null && !string.IsNullOrWhiteSpace(Message)) {
                if(SelectedPseudos.Count == 0) {
                    _ = ChatRoom.Send(ChatUser, Message);
                } else {
                    Parallel.ForEach(SelectedPseudos, p => ChatRoom.SendTo(ChatUser, p, Message));
                }
                Message = "";
            }
        }

    }
}
