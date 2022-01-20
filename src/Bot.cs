// See https://aka.ms/new-console-template for more information
using Discord;
using Discord.WebSocket;
using System.Text.Json;
using System.IO;
using System.Diagnostics;

namespace playlistbot
{
    public class Bot
    {
        public DiscordSocketClient client = new DiscordSocketClient();
        public List<SlashCommand> slashCommands = new List<SlashCommand>();
        public List<Guild> cachedGuilds = new List<Guild>();

        public static void Main(String[] args) => new Bot().MainAsync().GetAwaiter().GetResult();
        
        public async Task MainAsync()
        {
            client = new DiscordSocketClient(Global.clientConfig);
            #pragma warning disable CS8602
            try { Global.TOKEN = JsonSerializer.Deserialize<Dictionary<string,string>>(File.ReadAllText("config.json"))["token"]; }
            catch (Exception ex) { await Global.Logger(new LogMessage(LogSeverity.Error, "JSON Deserialization", ex.Message)); System.Environment.Exit(0); }
            #pragma warning restore CS8602
            Global.bot = this;
            await Global.Logger(new LogMessage(LogSeverity.Info,"Bot","Starting Bot..."));

            await client.LoginAsync(TokenType.Bot, Global.TOKEN);
            await client.StartAsync();

            client.Log += Global.Logger;
            client.SlashCommandExecuted += Commands.commandHandler;

            client.MessageUpdated += MessageUpdated;
            client.Ready += Ready;

            await Task.Delay(-1);
        }

        public async Task readGuilds(){
            var path = Directory.GetCurrentDirectory() + "\\playlists.json";
            try { cachedGuilds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guild>>(File.ReadAllText(path)); }
            catch(FileNotFoundException e) { File.Create(path); cachedGuilds = new List<Guild>();  }
            if(cachedGuilds == null){
                await Global.Logger(new LogMessage(LogSeverity.Warning,$"readGuilds()/{new StackFrame(1,true).GetFileLineNumber()}",path+" gave a null List<Guild>"));
                cachedGuilds = new List<Guild>();
            }
        }

        public async Task updateGuilds()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsondone = JsonSerializer.Serialize(cachedGuilds, options);
            await File.WriteAllTextAsync(Global.playlistPath, jsondone);
        }

        public async Task Ready()
        {
            var numCommands = 1;
            for(int i = 0; i < numCommands; i++){
                slashCommands.Add(new SlashCommand()); 
                switch (i)
                {
                    case 0: slashCommands[i].builder
                            .WithName("add")
                            .WithDescription("Add song to playlist")
                            .AddOption("playlist", ApplicationCommandOptionType.String,"Playlist to add to",isRequired:true)
                            .AddOption("song", ApplicationCommandOptionType.String, "Song to add t nameo playlist", isRequired:true)
                            .AddOption("artist", ApplicationCommandOptionType.String, "Artist name");
                            slashCommands[i].impl = async (SocketSlashCommand cmd) => {
                                await readGuilds();
                                try{
                                    #pragma warning disable CS8600
                                    #pragma warning disable CS8602
                                    string playlist = cmd.Data.Options.ElementAt(0).Value!.ToString();
                                    string song = cmd.Data.Options.ElementAt(1).Value.ToString();
                                    SocketGuild guild = (cmd.Channel as SocketGuildChannel).Guild;
                                    #pragma warning restore CS8600
                                    #pragma warning restore CS8602
                                    if(playlist == null || song == null) { return;}
                                    
                                    try { song += cmd.Data.Options.ElementAt(2).Value.ToString(); }catch(Exception e) { e = new Exception(); }
                                    bool found = false;
                                    foreach (Guild g in cachedGuilds){
                                        if(guild.Id == Convert.ToUInt64(g.id)){
                                            found = true;
                                            if (g.playlists.ContainsKey(playlist))
                                                g.playlists[playlist].Add(song);
                                            else
                                                g.playlists.Add(playlist, new List<string>() { song }) ;
                                            Console.WriteLine(g.ToString());
                                        }
                                    }
                                    if (!found)
                                    {
                                        cachedGuilds.Add(new Guild(guild.Name, guild.Id.ToString(), new Dictionary<string, List<string>>() { { playlist , new List<string>()} }));
                                    }
                                    foreach(Guild g in cachedGuilds)
                                    {
                                        await Global.Logger(new LogMessage(LogSeverity.Debug, "GUILDS", g.ToString()));
                                    }
                                    await updateGuilds();
                                }catch(Exception e){
                                    await Global.Logger(new LogMessage(LogSeverity.Error, "Command " + cmd.CommandName, e.Message));
                                }
                            };
                            break;
                    default: await Global.Logger(new LogMessage(LogSeverity.Warning, "Ready()",""));break;
                }
                try { await client.CreateGlobalApplicationCommandAsync(slashCommands[i].builder.Build()); }
                catch (Exception ex) { await Global.Logger(new LogMessage(LogSeverity.Error, ex.Source, ex.Message, exception: ex)); }
            }
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {   
            // If the message was not in the cache, downloading it will result in getting a copy of `after`.
            var message = await before.GetOrDownloadAsync();
            Console.WriteLine($"{message} -> {after}");
        }
    }
}


