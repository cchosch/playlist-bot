# Project Setup

First, create a `config.json` and a `playlists.json` file in `./bin/debug/net6.0/`.\
Then take your discord API token and put it in the `config.json` as follows:

```json
{
  "token": "MY API TOKEN"
}
```

This is the format for `playlists.json` you can

```json
[
  {
    "name": "placeholder guild", // guild name
    "id": "1", // guild id
    "playlists": {
      // playlists for that guild
      "placeholder playlist": ["placeholder song 1"] // actual individual playlist
    }
  }
]
```

The program should be good to go!
