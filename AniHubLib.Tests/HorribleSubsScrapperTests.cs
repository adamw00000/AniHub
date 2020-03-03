using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AniHubLib.Tests
{
	public class HorribleSubsScrapperTests
	{
		[Fact]
		public async Task Scrapping()
		{
			var scrapper = new HorribleSubsScrapper();

			//string result = scrapper.Download("Kaguya-sama wa Kokurasetai").Result;
			IList<HorribleSubsLink>? result = await scrapper.Download("Gintama").ConfigureAwait(true);

			result.ShouldNotBeNull();
		}
	}
}
