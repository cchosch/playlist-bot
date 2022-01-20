using System;
using System.Runtime.CompilerServices;
using Discord;
using Discord.WebSocket;

namespace playlistbot { 
    public class Global 
    {
        public static Bot bot = new Bot();
        public static string playlistPath = Directory.GetCurrentDirectory()+"\\playlist.json";
        public static string TOKEN = "";
        public static DiscordSocketConfig clientConfig = new DiscordSocketConfig { MessageCacheSize = 100 };
        public static Task Logger(LogMessage message){
            Console.WriteLine($"[{message.Severity}] {message}");
            return Task.CompletedTask;
        }
    }
}
