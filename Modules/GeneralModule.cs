// ReSharper disable UnusedType.Global, UnusedMember.Global

using Discord;
using Discord.Interactions;

namespace DiscordBot.Modules;

public class GeneralModule : InteractionModuleBase<SocketInteractionContext> {
  [SlashCommand("ping", "Pings the bot and returns its latency.")]
  public async Task PingAsync() =>
    await RespondAsync("mewo emow!");

  [SlashCommand("echo", "Echoes the provided message.")]
  public async Task EchoAsync([Summary("message", "The message to echo back.")] string message) =>
    await RespondAsync(message);

  [DefaultMemberPermissions(GuildPermission.Administrator)]
  [SlashCommand("adminonly", "An admin-only command.")]
  public async Task AdminsOnlyCommandAsync() =>
    await RespondAsync("This is an admin-only command.");
}
