// See https://aka.ms/new-console-template for more information
using Discord;
using Discord.WebSocket;
using System.Text.Json;
using System.IO;


namespace playlistbot
{
    public class Bot
    {
        public DiscordSocketClient client = new DiscordSocketClient();
        public List<SlashCommand> slashCommands = new List<SlashCommand>();

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
                                File.WriteAllTextAsy.ToLnc(dir, jsondone).GetAwaiter().GetResult();
                                /**/
                                /*READ FROM FILE*/
                                #pragma warning disable CS8600
                                var options = new JsonSerializerOptions { WriteIndented = true };
                                var path = Directory.GetCurrentDirectory() + "\\playlists.json";
                                List<Guild> guilds;
                                try { guilds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guild>>(File.ReadAllText(path)); }
                                catch(Exception e) { await Global.Logger(new LogMessage(LogSeverity.Error, e.Source, e.Message)); return; }
                                if(guilds == null) { return; }
                                #pragma warning restore CS8600
                                /**/
                                string playlist;
                                string song;
                                SocketGuild guild;

                                try{
                                    #pragma warning disable CS8600
                                    playlist = cmd.Data.Options.ElementAt(0).Value.ToString();
                                    song = cmd.Data.Options.ElementAt(1).Value.ToString();
                                    var tguild = cmd.Channel as SocketGuildChannel;
                                    guild = tguild.Guild;
                                    #pragma warning restore CS8600
                                    
                                    try { song += cmd.Data.Options.ElementAt(2).Value.ToString(); }catch(Exception e) { e = new Exception(); }
                                    bool[] found = new bool[2] { false, false };
                                    foreach (Guild g in guilds){
                                        if(guild.Id == Convert.ToUInt64(g.id)){
                                            found[0] = true;
                                            if (g.playlists.ContainsKey(playlist)){
                                                found[1] = true;
                                                g.playlists[playlist].Add(song);
                                                foreach(string s in g.playlists[playlist]) { 
                                                Console.WriteLine(s);}
                                            }
                                        }
                                    }
                                    foreach(Guild g in guilds)
                                    {
                                        await Global.Logger(new LogMessage(LogSeverity.Debug, "GUILDS", g.ToString()));
                                    }
                                    string jsondone = JsonSerializer.Serialize(guilds, options);
                                    await File.WriteAllTextAsync(path, jsondone);
                                    await cmd.RespondAsync(jsondone.ToString());
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


