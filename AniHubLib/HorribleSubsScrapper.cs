using HtmlAgilityPack;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AniHubLib
{
    public class HorribleSubsAnime
    {
        public string Title { get; set; } = default!;
        public string Url { get; set; } = default!;
    }

    public class HorribleSubsLink
    {
        public string AnimeTitle { get; set; } = default!;
        public string Episode { get; set; } = default!;
        public string? Url { get; set; }
        public LinkType Type { get; set; }
    }

    public enum LinkType { Torrent, Magnet, Missing };

    public class HorribleSubsScrapper
	{
		private const string baseUrl = "https://horriblesubs.info";

        public async Task<IList<HorribleSubsLink>?> Download(string name = "")
        {
            const string url = baseUrl + "/shows/";
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url).ConfigureAwait(false);

            var trackedShowList = doc.DocumentNode
                .SelectNodes("//*[contains(@class, 'ind-show')]" +
                    "/a")
                .Select(node => new HorribleSubsAnime
                {
                    Title = node.Attributes["title"].Value,
                    Url = baseUrl + node.Attributes["href"].Value
                })
                .Where(anime => anime.Title == name);

            if (!trackedShowList.Any())
            {
                return null;
            }
            var trackedShow = trackedShowList.First();

            return await GetDls(trackedShow.Url, name).ConfigureAwait(false);
        }

        private async Task<IList<HorribleSubsLink>> GetDls(string url, string animeName)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision).ConfigureAwait(false);
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }).ConfigureAwait(false);
            var page = await browser.NewPageAsync().ConfigureAwait(false);
            await page.SetRequestInterceptionAsync(true).ConfigureAwait(false);
            page.Request += OptimizeRequest;

            await page.GoToAsync(url).ConfigureAwait(false);

            for (int i = 0; ; i++)
            {
                var moreButton = await page.XPathAsync($"//*[contains(@class, 'more-button') and @id='{i}']").ConfigureAwait(false);
                if (moreButton.Length == 0)
                    break;
                await moreButton[0].ClickAsync().ConfigureAwait(false);
            }

            var matches = await page.XPathAsync($"//*[contains(@class, 'rls-label')]").ConfigureAwait(false);
            matches.ToList()
                .ForEach(async match => await match.ClickAsync().ConfigureAwait(false));

            var episodeDivs = await page
                .XPathAsync($"//*[contains(@class, 'rls-info-container')]").ConfigureAwait(false);

            List<HorribleSubsLink> links = new List<HorribleSubsLink>();
            foreach (var episodeDiv in episodeDivs)
            {
                string episodeNumber = await (
                        await episodeDiv.GetPropertyAsync("id").ConfigureAwait(false)
                    ).JsonValueAsync<string>().ConfigureAwait(false);

                LinkType type = LinkType.Torrent;
                var linkSpans = await page
                    .XPathAsync($"//*[contains(@class, 'rls-info-container') and @id='{episodeNumber}']" +
                       $"/*[contains(@class, 'rls-links-container')]" +
                       $"/*[contains(@class, 'rls-link link-720p')]" +
                       $"/*[contains(@class, 'dl-type hs-torrent-link')]" +
                       $"/a").ConfigureAwait(false);

                if (linkSpans.Length == 0)
                {
                    type = LinkType.Magnet;
                    linkSpans = await page
                        .XPathAsync($"//*[contains(@class, 'rls-info-container') and @id='{episodeNumber}']" +
                           $"/*[contains(@class, 'rls-links-container')]" +
                           $"/*[contains(@class, 'rls-link link-720p')]" +
                           $"/*[contains(@class, 'dl-type hs-magnet-link')]" +
                           $"/a").ConfigureAwait(false);
                    if (linkSpans.Length == 0)
                    {
                        links.Add(new HorribleSubsLink
                        {
                            AnimeTitle = animeName,
                            Episode = episodeNumber,
                            Url = null,
                            Type = LinkType.Missing
                        });

                        continue;
                    }
                }
                var linkSpan = linkSpans[0];

                string link = await (
                        await linkSpan.GetPropertyAsync("href").ConfigureAwait(false)
                    ).JsonValueAsync<string>().ConfigureAwait(false);
                links.Add(new HorribleSubsLink
                {
                    AnimeTitle = animeName,
                    Episode = episodeNumber,
                    Url = link,
                    Type = type
                });
            }

            return links;
        }

        private void OptimizeRequest(object sender, RequestEventArgs e)
        {
            if (e.Request.ResourceType == ResourceType.Font || e.Request.ResourceType == ResourceType.Image || e.Request.ResourceType == ResourceType.StyleSheet)
            {
                e.Request.AbortAsync();
            }
            else
            {
                e.Request.ContinueAsync();
            }
        }
    }
}
