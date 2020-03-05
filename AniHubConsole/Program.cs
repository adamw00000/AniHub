using AniHubLib;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AniHubConsole
{
	static class Program
	{
		static async Task Main(string[] args)
		{
			if (args.Length != 1)
			{
				Console.WriteLine("magnetUri not provided");
				return;
			}

			var scrapper = new HorribleSubsScrapper();
			var episodes = await scrapper.Download(args[0]).ConfigureAwait(true);

			var downloader = new TorrentDownloader();
			downloader.Run(episodes.Select(e => e.Url));
		}
	}
}
