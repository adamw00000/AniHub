﻿using System;
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
			downloader.Run(new List<Uri> { new Uri("magnet:?xt=urn:btih:TVVLPUS5IIHR64HCHMELVZCNMFJIAJC2&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://tracker.coppersurfer.tk:6969/announce&tr=udp://tracker.internetwarriors.net:1337/announce&tr=udp://tracker.leechersparadise.org:6969/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://open.stealth.si:80/announce&tr=udp://p4p.arenabg.com:1337/announce&tr=udp://mgtracker.org:6969/announce&tr=udp://tracker.tiny-vps.com:6969/announce&tr=udp://peerfect.org:6969/announce&tr=http://share.camoe.cn:8080/announce&tr=http://t.nyaatracker.com:80/announce&tr=https://open.kickasstracker.com:443/announce") });
		}
	}
}
