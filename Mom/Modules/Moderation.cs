using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Mom.Modules {
	public class Moderation : ModuleBase {
		[Command("purge")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task command_purge(int amt) {
			if (amt < 1) { amt = 100; }
			var messages = await Context.Channel.GetMessagesAsync(amt + 1).FlattenAsync();
			await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
			var message = await Context.Channel.SendMessageAsync($"**{messages.Count() - 1}** chats got purged.");
			await Task.Delay(int.Parse(StartupService._config["chat-removedelay"]));
			await message.DeleteAsync();
		}
	}
}
