using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace playlistbot
{
    public class SlashCommand
    {
        public Action<SocketSlashCommand> impl;
        public SlashCommandBuilder builder;
        public SlashCommand(){
            this.builder = new SlashCommandBuilder();
            impl = (SocketSlashCommand cmd) => { Global.Logger(new LogMessage(LogSeverity.Warning, "Slash Command", $"Nothing in {this.builder.Name} implementation")); };
        }
    }
}
