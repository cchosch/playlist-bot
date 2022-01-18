// See https://aka.ms/new-console-template for more information
using Discord;
using Discord.WebSocket;


namespace playlistbot
{
    public class Bot
    {
        DiscordSocketClient client = new DiscordSocketClient(Global.clientConfig);
        public List<SlashCommand> slashCommands = new List<SlashCommand>();

        public static void Main(String[] args) => new Bot().MainAsync().GetAwaiter().GetResult();
        
        public async Task MainAsync()
        {
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
                            /*WRITE TO FILE 


                            guild HCP = new guild() { 
                                guildid = 334,
                                playlists = new Dictionary<string, string[]>()
                                {
                                    ["bricker"] = new string[] { "tribe", "one time 4 your mind", "left" },
                                }
                            };
                            guild DMR = new guild() { 
                                guildid = 334,
                                playlists = new Dictionary<string, string[]>()
                                {
                                    ["bricker"] = new string[] { "the world is yours", "ichi banger", "fuck seb" },
                                }
                            };
                            List<guild> servers = new List<guild> { HCP, DMR };
                            var options = new JsonSerializerOptions { WriteIndented = true };
                            string jsondone = JsonSerializer.Serialize(servers, options);
                            string dir = Directory.GetCurrentDirectory()+"\\playlist.json";
                            File.WriteAllTextAsync(dir, jsondone).GetAwaiter().GetResult();
                            /**/
                            /*READ FROM FILE
                            string dir = Directory.GetCurrentDirectory()+"\\playlist.json";
                            string jsondata = File.ReadAllText(dir);
                            List<guild> guilds = JsonSerializer.Deserialize<List<guild>>(jsondata);
                            /**/

                                try{
                                    var playlist = cmd.Data.Options.ElementAt(0).ToString().ToLower();
                                    var song = cmd.Data.Options.ElementAt(1).ToString().ToLower();
                                    String artist = null;
                                    try { artist = cmd.Data.Options.ElementAt(2).ToString().ToLower(); }catch(Exception e) { }
                                }catch(Exception e){
                                    await Global.Logger(new LogMessage(LogSeverity.Error, "Command " + cmd.CommandName, e.Message));
                                }
                            };
                            break;
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

