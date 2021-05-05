using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Mom {
	class StartupService {
		public static IServiceProvider _provider;
		private readonly DiscordSocketClient _discord;
		private readonly CommandService _commands;
		private readonly IConfigurationRoot _config;


		public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config) {
			_provider = provider;
			_config = config;
			_discord = discord;
			_commands = commands;
		}

		public async Task StartAsync() {
			string token = _config["token"];
			if (string.IsNullOrEmpty(token)) {
				Console.WriteLine("No discord token.");
				return;
			}
			await _discord.LoginAsync(TokenType.Bot, token);
			await _discord.StartAsync();

			await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
		}
	}
}
