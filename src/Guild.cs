using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;


namespace playlistbot{
    public class Guild{
    
        public string id = "";
        public string name = "";
        public Dictionary<string,List<string>> playlists;

        public override string ToString()
        {
            return $"{{\"id\": \"{id}\" \n \"name\": \"{name}\" \n \"playlists\": [\n {string.Join(", \n",playlists)}\n]}}";
        }

        public Guild(string name, string id, Dictionary<string,List<string>> playlists){
            this.name = name;
            this.id = id;
            this.playlists = playlists;
        }
    }
}


