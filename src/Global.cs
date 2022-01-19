using System;
using Discord;
using Discord.WebSocket;

namespace playlistbot { 
    public class Global 
    {
        public static Bot bot = new Bot();
        public static String TOKEN = "";
        public static DiscordSocketConfig clientConfig = new DiscordSocketConfig { MessageCacheSize = 100 };
        public static Task Logger(LogMessage message){
            Console.WriteLine($"[{message.Severity}] {message}");
            return Task.CompletedTask;
        }

    }
}
