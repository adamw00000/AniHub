using AniHubLib;
using System;
using System.Linq;

namespace AniHubConsole
{
	static class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 1)
			{
				Console.WriteLine("magnetUri not provided");
				return;
			}

			var downloader = new TorrentDownloader();

			downloader.Run(args.Select(uri => new Uri(uri)));
		}
	}
}
