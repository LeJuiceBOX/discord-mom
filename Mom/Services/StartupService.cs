using System;
using System.IO;
using System.Linq;
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
		public static IConfigurationRoot _config;

		public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config) {
			_provider = provider;
			_discord = discord;
			_commands = commands;
			_config = config;
		}

		public async Task StartAsync() {
			string token = File.ReadLines("token.txt").First(); ;
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
