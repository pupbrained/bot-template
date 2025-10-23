# Discord Bot Template

A simple Discord bot template built with Discord.NET, designed as a starting point for creating your own Discord bots with slash commands.

## Features

- **Slash Commands**: Modern Discord interactions using Discord.Net's InteractionService
- **Modular Architecture**: Organize commands into separate modules for easy maintenance
- **Logging**: Integrated console logging with Microsoft.Extensions.Logging
- **Configuration**: Environment variable support with .env file loading
- **Dependency Injection**: Services registered in the main bot class and automatically injected into command modules

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- A Discord bot token (create one in the [Discord Developer Portal](https://discord.com/developers/applications))

## Setup

1. **Create a new repository from this template in GitHub**

2. **Create your Discord bot application:**
   - Go to the [Discord Developer Portal](https://discord.com/developers/applications)
   - Create a new application
   - Go to the "Bot" section and create a bot
   - Copy the bot token

3. **Invite the bot to your server:**
   - In the Developer Portal, go to "OAuth2" > "URL Generator"
   - Select scopes: `bot` and `applications.commands`
   - Select permissions: Send Messages, Use Slash Commands, etc.
   - Use the generated URL to invite the bot to your server

4. **Configure the bot:**
   - Create a `.env` file in the project root
   - Add your bot token: `BOT_TOKEN=your_bot_token_here`

5. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

## Running the Bot

```bash
dotnet run
```

The bot will connect to Discord and register its slash commands globally. You should see confirmation in the console.

## Usage

The template includes example commands in `Modules/GeneralModule.cs`:

- `/ping` - Basic ping command
- `/echo <message>` - Echoes back your message
- `/adminonly` - Admin-only command (requires administrator permissions)
- `/dm` - Command that only works in DMs

## Adding New Commands

1. Create a new class in the `Modules` folder that inherits from `InteractionModuleBase<SocketInteractionContext>`
2. Add methods decorated with `[SlashCommand("commandname", "description")]`
3. The bot will automatically register new commands when it starts

Modules can inject services via their constructors for dependencies like databases or APIs.

Example:
```c#
// SlashCommand takes the command name and description
[SlashCommand("hello", "Says hello")]
public async Task HelloCommand() =>
  await RespondAsync("Hello!");
```

## Configuration

- `BOT_TOKEN`: Your Discord bot token (required)
- The bot uses specific gateway intents. Modify in `DiscordBot.cs` if needed.

## Project Structure

- `DiscordBot.cs` - Main bot class, entry point, and dependency injection setup (registers client, interactions, logger)
- `Modules/` - Command modules (inherit from `InteractionModuleBase`, can inject services via constructors)
- `DiscordBot.csproj` - Project configuration and dependencies

## Dependencies

- Discord.Net 3.18.0 - Discord API wrapper
- DotNetEnv - Environment variable loading
- Microsoft.Extensions.Logging - Logging framework

## Contributing

This is a template project. Feel free to modify and extend it for your own Discord bots.

## License

This project is released under the [Unlicense](UNLICENSE), placing it in the public domain.