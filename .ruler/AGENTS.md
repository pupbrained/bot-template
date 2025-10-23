# AGENTS.md

## Project Overview

This is a C# Discord bot template using Discord.NET, designed for AI agents to modify and extend. It provides a basic structure for building Discord bots with slash commands (interactions). The bot is a console application targeting .NET 9.0.

Key features:
- Uses Discord.WebSocket for real-time communication
- Implements slash commands via InteractionService
- Supports modular command organization
- Includes logging and dependency injection
- Loads configuration from environment variables or .env file

When modifying this project, maintain the modular architecture and interaction-based command system.

## Setup and Running

Assume the user has created a Discord bot application in the Discord Developer Portal, obtained its token, and invited the bot to a server with appropriate permissions (including the `applications.commands` scope for slash commands).

To run the project:
- Ensure .NET 9.0 SDK is installed.
- Create a `.env` file in the project root with `BOT_TOKEN=your_discord_bot_token_here`.
- Run `dotnet restore` to restore dependencies.
- Execute `dotnet run` to start the bot.

The bot connects to Discord and registers slash commands globally. Commands can be tested in servers where the bot is invited.

## Code Structure

- `DiscordBot.cs`: Main entry point. Initializes the Discord client, interaction service, logging, and event handlers. Dependency injection is set up here with `ServiceCollection`, registering services like the client, interactions, and logger. The `IServiceProvider` is used by `AddModulesAsync` and `ExecuteCommandAsync` for resolving dependencies. Do not modify core setup logic unless necessary.
- `Modules/`: Contains command modules. Each module inherits from `InteractionModuleBase<SocketInteractionContext>` and defines slash commands with `[SlashCommand]` attributes. Add new commands here. Inject services via constructors if needed.
- `DiscordBot.csproj`: Project file with dependencies. Add new packages here if needed.

When adding features, create new modules in the `Modules` folder and register them automatically via `AddModulesAsync`.

## Coding Guidelines

When editing this codebase, follow these patterns established in the existing code:

- **Naming**: Use PascalCase for classes, methods, and properties (e.g., `DiscordBot`, `MainAsync`, `HandleInteraction`).
- **Asynchronous Programming**: Use `async`/`await` for all I/O operations. Return `Task` or `Task<T>` from async methods.
- **Attributes**: Use Discord.Net attributes like `[SlashCommand]`, `[DefaultMemberPermissions]`, `[CommandContextType]` to define command behavior.
- **Error Handling**: Wrap interaction handling in try-catch blocks and log exceptions. Delete original responses on errors for application commands.
- **Logging**: Use structured logging with `ILogger`. Map Discord.Net's `LogSeverity` to Microsoft.Extensions.Logging's `LogLevel`.
- **Dependency Injection**: Register services in `ServiceCollection` in `DiscordBot.cs` (e.g., client, interactions, logger as singletons). The `IServiceProvider` is passed to `AddModulesAsync` for module instantiation and `ExecuteCommandAsync` for command execution. Inject services into module constructors when adding dependencies like databases or APIs.
- **Configuration**: Load sensitive data like tokens from environment variables or `.env` file using DotNetEnv. Never hardcode secrets.
- **Modularity**: Organize commands into separate modules in the `Modules` folder. Keep the main bot class focused on setup and event handling.
- **Intents**: Configure gateway intents appropriately for your bot's needs (current setup includes message content and excludes scheduled events/invites).
- **Nullability**: Nullable reference types are enabled; use `?` for nullable types and check for null where necessary.

Always validate changes by running the bot and testing commands. Use logging to debug issues.

## Testing

When modifying code, test commands in a development Discord server. Ensure the bot has necessary permissions. Use console logging output to identify and fix issues.

## Deployment

For production deployments, ensure the `BOT_TOKEN` environment variable is set on the hosting platform. Consider using process managers like systemd or Docker. Monitor logs for errors and performance.
