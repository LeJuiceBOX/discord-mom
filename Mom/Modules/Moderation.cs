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

namespace Mom.Modules {
	public class Moderation : ModuleBase {
		[Command("purge")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task CMD_purge(int amt) {
			if (amt < 1) { amt = 100; }
			var messages = await Context.Channel.GetMessagesAsync(amt + 1).FlattenAsync();
			await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
			var message = await Context.Channel.SendMessageAsync($"**{messages.Count() - 1}** chats got purged.");
			await Task.Delay(3000);
			await message.DeleteAsync();
		}
	}
}
