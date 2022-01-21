# Update
So it turns out most Discord music bots ignore other bots for obvious reasons... command spamming, so I think what I'm going to do is scrap this repo and make a bot that looks just like a Discord user and does the same thing as this bot. Probably in pyhton. For now this bot will be scrapped and not supported anymore.

# Project Setup

First, create a `config.json` and a `playlists.json` file in `./bin/debug/net6.0/`.\
Then take your discord API token and put it in the `config.json` as follows:

```json
{
  "token": "MY API TOKEN"
}
```

This is the format for `playlists.json` you can manually add songs to it.

```json
[
  {
    "name": "placeholder guild name",
    "id": "1",
    "playlists": {
      "placeholder playlist": ["placeholder song 1"]
    }
  }
]
```

The program should be good to go!
