using TodoImail.BlazorApp.Services;

namespace TodoImail.BlazorApp.Models {

    public delegate void ImailChatUserHandler(object sender, ChatUserEventArgs args);
    public delegate void ImailChatMessageHandler(object sender, ChatMessageEventArgs args);


    public record ChatEventArgs{
        public required string Message { get; init; }
        public required DateTime Timestamp { get; init; }
    }

    public record ChatUserEventArgs : ChatEventArgs {        
        public required string Pseudo { get; init; }
    }

    public enum ChatMessageType { Public, Private }
    public record ChatMessageEventArgs : ChatEventArgs {
        public required ChatMessageType MessageType { get; init; }
        public required string From { get; init; }
    }
}
