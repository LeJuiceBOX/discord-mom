using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Mom.Services {
	class CommandHandler {

		public static IServiceProvider _provider;
		public static DiscordSocketClient _discord;
		public static CommandService _commands;
		public static IConfigurationRoot _config;

		public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider) {
			_provider = provider;
			_config = config;
			_discord = discord;
			_commands = commands;

			_discord.Ready += OnReady;
			_discord.MessageReceived += OnMessageRecieved;
		}

		private async Task OnMessageRecieved(SocketMessage arg) {
			var msg = arg as SocketUserMessage;
			if (msg.Author.IsBot) return;

			var context = new SocketCommandContext(_discord, msg);
			int pos = 0;
			if (msg.HasStringPrefix( _config["prefix"],ref pos) || msg.HasMentionPrefix(_discord.CurrentUser, ref pos)) {
				var result = await _commands.ExecuteAsync(context, pos, _provider);
				if (!result.IsSuccess) {
					var reason = result.Error;
					await context.Channel.SendMessageAsync($"**An error occured:** `{reason}`");
					Console.WriteLine("Error: "+reason);
				}
			}
		}

		private Task OnReady() {
			Console.WriteLine($"Connected as {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}.");
			_discord.SetGameAsync("the kids",null,ActivityType.Watching);
			return Task.CompletedTask;
		}
	}
}
