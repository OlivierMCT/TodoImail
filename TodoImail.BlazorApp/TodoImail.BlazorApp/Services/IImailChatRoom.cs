using TodoImail.BlazorApp.Models;

namespace TodoImail.BlazorApp.Services;
public interface IImailChatRoom {
    Task<IImailChatUser> Connect(string pseudo);
    Task Disconnect(IImailChatUser user);

    Task Send(IImailChatUser from, string message);
    Task SendTo(IImailChatUser from, string to, string message);

    public List<string> ConnectedUsers { get; }
}

public interface IImailChatUser {
    public string Pseudo { get; }
    public event ImailChatMessageHandler? NewMessage;
    public event ImailChatUserHandler? ConnectedUser;
    public event ImailChatUserHandler? DisconnectedUser;
}

public class ImailChatRoomServiceDefaultImplementation : IImailChatRoom {
    private class ChatUser : IImailChatUser {
        public required string Pseudo { get; init; }
        public event ImailChatMessageHandler? NewMessage;
        public event ImailChatUserHandler? ConnectedUser;
        public event ImailChatUserHandler? DisconnectedUser;

        public void RaiseNewMessage(object sender, ChatMessageEventArgs args)  => NewMessage?.Invoke(sender, args);
        public void RaiseConnectedUser(object sender, ChatUserEventArgs args)  => ConnectedUser?.Invoke(sender, args);
        public void RaiseDisconnectedUser(object sender, ChatUserEventArgs args)  => DisconnectedUser?.Invoke(sender, args);
    }

    private List<ChatUser> ChatUsers { get; set; } = [];
    public List<string> ConnectedUsers => [.. ChatUsers.Select(u => u.Pseudo)];

    public Task<IImailChatUser> Connect(string pseudo) {
        if (ConnectedUsers.Contains(pseudo))
            throw new InvalidOperationException($"le pseudo '{pseudo}' est déjà utilisé");

        ChatUser user = new () { Pseudo = pseudo };
        ChatUsers.Add(user);
        Parallel.ForEach(ChatUsers, userTo => userTo.RaiseConnectedUser(this, new() { Message = $"{pseudo} vient de se connecter", Pseudo = pseudo, Timestamp = DateTime.Now }));

        return Task.FromResult<IImailChatUser>(user);
    }

    public Task Disconnect(IImailChatUser user) {
        int index = ChatUsers.FindIndex(u => u.Pseudo == user.Pseudo);
        if (index >= 0) {
            ChatUsers.RemoveAt(index);
            string pseudo = user.Pseudo;
            Parallel.ForEach(ChatUsers, userTo => userTo.RaiseDisconnectedUser(this, new() { Message = $"{pseudo} vient de se déconnecter", Pseudo = pseudo, Timestamp = DateTime.Now }));
        }
        return Task.CompletedTask;
    }

    public async Task Send(IImailChatUser from, string message) {
        var userFrom = ChatUsers.Find(u => u.Pseudo == from.Pseudo) ?? throw new InvalidOperationException($"{from.Pseudo} n'est pas connecté");
        await Parallel.ForEachAsync(ChatUsers, (userTo, _) => Send(userTo, userFrom, message));
    }

    public async Task SendTo(IImailChatUser from, string to, string message) {
        var userFrom = ChatUsers.Find(u => u.Pseudo == from.Pseudo) ?? throw new InvalidOperationException($"{from.Pseudo} n'est pas connecté");
        var userTo = ChatUsers.Find(u => u.Pseudo == to) ?? throw new InvalidOperationException($"{to} n'est pas connecté");
        await Send(userTo, userFrom, message, ChatMessageType.Private);
    }

    private ValueTask Send(ChatUser to, ChatUser from, string message, ChatMessageType type = ChatMessageType.Public) {
        to.RaiseNewMessage(this, new() {
            From = from.Pseudo,
            Message = message,
            MessageType = type,
            Timestamp = DateTime.Now
        });
        return ValueTask.CompletedTask;
    }

}

