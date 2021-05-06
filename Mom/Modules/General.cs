using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Mom.Modules {
	public class General : ModuleBase {
		[Command("ping")]
		public async Task command_ping() {
			await Context.Channel.SendMessageAsync("Pong!");
		}

		[Command("info")]
		public async Task command_myinfo(SocketGuildUser user = null) {
			if (user == null) {
				var builder = new EmbedBuilder()
					.WithTitle($"{Context.User.Username}'s user information.")
					.WithThumbnailUrl(Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl())
					.AddField("ID", $"`{Context.User.Id}`", true)
					.AddField("Username", $"`{Context.User.Username}`", true)
					.AddField("Discriminator", $"`#{Context.User.Discriminator}`", true)
					.AddField("Visibility", $"`{Context.User.Status}`", true)
					.AddField("Join date", $"`{(Context.User as SocketGuildUser).JoinedAt.Value.ToString("dd/MM/yyyy")}`", true)
					.AddField("Roles", $"{string.Join(" `|` ", (Context.User as SocketGuildUser).Roles)}", false)
					//.AddField("User is playing", Context.User.Activity, true)
					.WithColor(new Color(80, 120, 255))
					.WithCurrentTimestamp();
				var embed = builder.Build();
				await Context.Channel.SendMessageAsync(null, false, embed);
			} else {
				var builder = new EmbedBuilder()
					.WithTitle($"{user.Username}'s user information.")
					.WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
					.AddField("ID", $"`{user.Id}`", true)
					.AddField("Username", $"`{user.Username}`", true)
					.AddField("Discriminator", $"`#{user.Discriminator}`", true)
					.AddField("Visibility", $"`{user.Status}`", true)
					.AddField("Join date", $"`{user.JoinedAt.Value.ToString("dd/MM/yyyy")}`", true)
					.AddField("Roles", $"{string.Join(" `|` ", user.Roles)}", false)
					//.AddField("User is playing", Context.User.Activity, true)
					.WithColor(new Color(80, 120, 255))
					.WithCurrentTimestamp();
				var embed = builder.Build();
				await Context.Channel.SendMessageAsync(null, false, embed);
			}
		}

		[Command("server")]
		public async Task command_server() {
			var builder = new EmbedBuilder()
				.WithThumbnailUrl(Context.Guild.IconUrl)
				.WithTitle($"{Context.Guild.Name} Information")
				.AddField("Creation", $"`{Context.Guild.CreatedAt.ToString("dd/MM/yyyy")}`", true)
				.AddField("Members ", $"`{(Context.Guild as SocketGuild).MemberCount}`", true)
				.AddField("Members online", $"`{(Context.Guild as SocketGuild).Users.Where(x => x.Status != UserStatus.Offline).Count<SocketGuildUser>()}`", true)
				.WithColor(new Color(80, 120, 255))
				.WithCurrentTimestamp();
			var embed = builder.Build();
			await Context.Channel.SendMessageAsync(null, false, embed);
		}

		[Command("poll")]
		public async Task command_poll(string pollName) {
			var builder = new EmbedBuilder()
				.WithTitle($"{pollName}")
				.WithDescription("React to vote.")
				.WithColor(new Color(80, 120, 255));
			var embed = builder.Build();
			var msg = await Context.Channel.SendMessageAsync(null, false, embed);
			Emoji[] emotes = { new Emoji("👍"), new Emoji("👎") };
			await msg.AddReactionAsync(emotes[0], RequestOptions.Default);
			await msg.AddReactionAsync(emotes[1], RequestOptions.Default);
		}
	}
}
