using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Reflection;

namespace Mom.Services {
	public class CommandHandler : InitializedService {

		public readonly IServiceProvider _provider;
		public readonly DiscordSocketClient _client;
		public readonly CommandService _service;
		public readonly IConfiguration _config;

		public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration config) {
			_provider = provider;
			_config = config;
			_client = client;
			_service = service;
		}

		public override async Task InitializeAsync(CancellationToken cancellationToken)
		{
			_client.Ready += OnReady;
			_client.MessageReceived += OnMessageRecieved;
			_service.CommandExecuted += OnCommandExecuted;
			await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
		}

		private async Task OnReady() {
			Console.WriteLine("Bot started.");
			await _client.SetGameAsync("the kids.", null, ActivityType.Watching);
		}

		private async Task OnMessageRecieved(SocketMessage arg) {
			if (!(arg is SocketUserMessage message)) return;
			if (message.Source != MessageSource.User) return;

			var argPos = 0;
			if (!message.HasStringPrefix(_config["prefix"], ref argPos) && !message.HasMentionPrefix(_client.CurrentUser,ref argPos)) return;

			var context = new SocketCommandContext(_client, message);
			await _service.ExecuteAsync(context, argPos, _provider);
		}

		private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context,IResult result) {
			if (command.IsSpecified && !result.IsSuccess) await context.Channel.SendMessageAsync($"**An error occured:** ```{result}```");
		}

	}
}
