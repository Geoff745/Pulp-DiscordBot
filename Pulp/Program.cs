using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Pulp
{
  class logger
  {

    public enum state
    {
      warning=6,
      fetc=6,
      error=12,
      idle=10,
      init=14
    }
    public void Write(state status, string Source, string Message )
    {
      Console.Write("[ " + DateTime.Now.ToString().Split(' ')[0] + " ] [ ");
      ConsoleColor ColourValue = (ConsoleColor)((int)status);
      Console.ForegroundColor = ColourValue;
      Console.Write(status);
      Console.ResetColor();
      Console.Write(" ] : {");
      if (Source == "Discord")
        Console.ForegroundColor = ConsoleColor.Magenta;
      if (Source == "Gateway")
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
      Console.Write(Source);
      Console.ResetColor();
      Console.Write("} : " + Message + "\n");
    }
  }
  class Program
  {
    private readonly DiscordSocketClient _client;
    internal static logger logging = new logger();
    internal static SocketGuild def_guild;
    static void Main(string[] args)
    {
      new Program().MainAsync().GetAwaiter().GetResult();
    }

    public Program()
    {
      DiscordSocketConfig Config = new DiscordSocketConfig();
      Config.AlwaysDownloadUsers = true;
      _client = new DiscordSocketClient(Config);
      _client.Log += LogAsync;
      _client.Ready += ReadyAsync;
      _client.MessageReceived += MessageReceivedAsync;
    }

    public async Task MainAsync()
    {
      await _client.LoginAsync(TokenType.Bot, "");
      await _client.StartAsync();
      await Task.Delay(Timeout.Infinite);
    }
    private Task LogAsync(LogMessage log)
    {
      string[] blacklisted_output = {""};
      if(!blacklisted_output.Contains(log.Message))
        logging.Write(logger.state.init,log.Source,log.Message);
      return Task.CompletedTask;
    }
    private Task ReadyAsync()
    {
      logging.Write(logger.state.init,"Discord", $"{_client.CurrentUser.Username} is connected!");
      return Task.CompletedTask;
    }

    private async Task MessageReceivedAsync(SocketMessage message)
    {
      logging.Write(logger.state.init,"Discord", $"Fetch guild:{_client.Guilds.First().Id}");
      def_guild = (_client.Guilds.First());
      var users = await (def_guild as IGuild).GetUsersAsync();
      logging.Write(logger.state.init,"Discord", $"guild: {def_guild.Id}:\n                           - Name:           {def_guild.Name}\n                           - MemberCount:    {users.Count}\n                           - DefaultChannel: {def_guild.DefaultChannel.Id}\n                           - Users:          [");
      foreach (var user in users) {
        Console.Write($"                                                  {user.Username}\n");
      }
      Console.Write($"                                             ]\n");
    }
  }
}