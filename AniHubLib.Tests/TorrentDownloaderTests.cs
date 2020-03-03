using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AniHubLib.Tests
{
	public class TorrentDownloaderTests
	{
		[Fact]
		public async Task Download()
		{
			var downloader = new TorrentDownloader();

			//string result = scrapper.Download("Kaguya-sama wa Kokurasetai").Result;
			downloader.Run();
		}
	}
}
