using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Mom.Modules {
	public class Fun : ModuleBase {

		private Random random = new Random();

		[Command("meme")]
		public async Task command_meme() {
			var client = new HttpClient();
			var result = await client.GetStringAsync("https://reddit.com/r/dankmemes/random.json?limit=1");
			JArray raw = JArray.Parse(result);
			JObject postData = JObject.Parse(raw[0]["data"]["children"][0]["data"].ToString()); // gets rid of meta data
			await EmbedRedditPost(postData);
			await Context.Message.DeleteAsync();
		}

		[Command("reddit")]
		public async Task command_reddit(string sub) {
			var client = new HttpClient();
			var result = await client.GetStringAsync($"https://reddit.com/r/{sub}/random.json?limit=1");
			if (result == null) { Console.WriteLine("Invalid subreddit.");  return; }
			JArray raw = JArray.Parse(result);
			JObject postData = JObject.Parse(raw[0]["data"]["children"][0]["data"].ToString()); // gets rid of meta data
			await EmbedRedditPost(postData);
			await Context.Message.DeleteAsync();
		}

		[Command("randomreddit")]
		public async Task command_randomreddit() {
			string[] subFile = File.ReadAllLines("subreddits.txt");
			string sub = subFile[random.Next(0, subFile.Length)];
			var client = new HttpClient();
			var result = await client.GetStringAsync($"https://reddit.com/r/{sub}/random.json?limit=1");
			JArray raw = JArray.Parse(result);
			JObject postData = JObject.Parse(raw[0]["data"]["children"][0]["data"].ToString()); // gets rid of meta data
			await EmbedRedditPost(postData);
			await Context.Message.DeleteAsync();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		private async Task EmbedRedditPost(JObject postData) {
			var builder = new EmbedBuilder()
				.WithColor(new Color(255, 69, 0))
				.WithTitle(postData["title"].ToString())
				.WithDescription($"`🗨 {postData["num_comments"]}` | `👍 {postData["ups"]}` | `👎 {postData["downs"]}` | `🙋 {postData["author"]}`")
				.WithUrl("https://reddit.com" + postData["permalink"].ToString())
				.WithImageUrl(postData["url"].ToString())
				.WithFooter($"From {postData["subreddit_name_prefixed"]}")
				.WithCurrentTimestamp();
			var embed = builder.Build();
			await Context.Channel.SendMessageAsync(null, false, embed);
		}
	}
}
