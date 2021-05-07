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
using Mom.Services;

namespace Mom {
	class Program {
		static async Task Main(string[] args) {
			var builder = new HostBuilder()
				.ConfigureAppConfiguration(x =>
				{
					var configuration = new ConfigurationBuilder()
						.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile("Data\\appsettings.json",false,true)
						.Build();

					x.AddConfiguration(configuration);
				})
				.ConfigureLogging(x =>
				{
					x.AddConsole();
					x.SetMinimumLevel(LogLevel.Debug);
				})
				.ConfigureDiscordHost<DiscordSocketClient>((context, config) =>
				{
					config.SocketConfig = new DiscordSocketConfig
					{
						LogLevel = LogSeverity.Debug,
						AlwaysDownloadUsers = true,
						MessageCacheSize = 200
					};

					config.Token = File.ReadLines("Data\\token.txt").First();
				})
				.UseCommandService((context, config) =>
				{
					config = new CommandServiceConfig()
					{
						CaseSensitiveCommands = false,
						LogLevel = LogSeverity.Debug
					};
				})
				.ConfigureServices((context, services) =>
				{
					services.AddHostedService<CommandHandler>();
				})
				.UseConsoleLifetime();

			var host = builder.Build();
			using (host) {
				await host.RunAsync();
			}
		}
	}
}
