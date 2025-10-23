using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DiscordBot;

public class Program {
  private DiscordSocketClient? m_client;
  private InteractionService? m_interactions;
  private ILogger<Program>? m_logger;
  private IServiceProvider? m_services;

  public static Task Main() => new Program().MainAsync();

  private async Task MainAsync() {
    Env.Load();

    ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
    m_logger = loggerFactory.CreateLogger<Program>();

    m_client = new DiscordSocketClient(new DiscordSocketConfig {
      GatewayIntents =
        GatewayIntents.AllUnprivileged ^ GatewayIntents.GuildScheduledEvents ^ GatewayIntents.GuildInvites |
        GatewayIntents.MessageContent
    });

    m_interactions = new InteractionService(m_client);

    m_services = new ServiceCollection()
      .AddSingleton(m_client)
      .AddSingleton(m_interactions)
      .AddSingleton(m_logger)
      .BuildServiceProvider();

    m_client!.Log += Log;
    m_interactions!.Log += Log;
    m_client!.Ready += ClientReady;
    m_client!.InteractionCreated += HandleInteraction;

    string? token = Environment.GetEnvironmentVariable("BOT_TOKEN");

    if (string.IsNullOrEmpty(token)) {
      m_logger!.LogError(
        "BOT_TOKEN not found. Please ensure you have BOT_TOKEN in your environment variables or .env file."
      );

      return;
    }

    await m_client!.LoginAsync(TokenType.Bot, token);
    await m_client.StartAsync();

    await Task.Delay(Timeout.Infinite);
  }

  private Task Log(LogMessage msg) {
    m_logger!.Log(ParseLogLevel(msg.Severity), msg.Exception, "{Source} {Message}", msg.Source, msg.Message);
    return Task.CompletedTask;
  }

  private async Task ClientReady() {
    await m_interactions!.AddModulesAsync(Assembly.GetEntryAssembly(), m_services);
    await m_interactions!.RegisterCommandsGloballyAsync();

    m_logger!.LogInformation(
      "Connected as {CurrentUserUsername}#{CurrentUserDiscriminator}",
      m_client!.CurrentUser.Username,
      m_client!.CurrentUser.Discriminator
    );
  }

  private static LogLevel ParseLogLevel(LogSeverity severity) => severity switch {
    LogSeverity.Critical => LogLevel.Critical,
    LogSeverity.Error => LogLevel.Error,
    LogSeverity.Warning => LogLevel.Warning,
    LogSeverity.Info => LogLevel.Information,
    LogSeverity.Verbose => LogLevel.Debug,
    LogSeverity.Debug => LogLevel.Debug,
    _ => LogLevel.Information
  };

  private async Task HandleInteraction(SocketInteraction interaction) {
    try {
      var context = new SocketInteractionContext(m_client!, interaction);

      await m_interactions!.ExecuteCommandAsync(context, m_services);
    } catch (Exception ex) {
      m_logger!.LogError(ex, "Error handling interaction");

      if (interaction.Type == InteractionType.ApplicationCommand)
        await interaction.GetOriginalResponseAsync().ContinueWith(async msg => await msg.Result.DeleteAsync());
    }
  }
}
