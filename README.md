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
