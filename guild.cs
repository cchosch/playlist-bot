using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;


namespcae playlistbot{
  public class Guild{
    
    public string name = "";
    public string id = "";
    public HashMap<string,string[]> playlists;
    
    public Guild(string name, string id, HashMap<string,string[]> playlists){
      this.name = name;
      this.id = id;
      this.playlists = playlists;
    }
  }
}


