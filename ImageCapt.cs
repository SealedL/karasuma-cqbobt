using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using PuppeteerSharp;

namespace cqbot
{
    public static class ImageCapt
    {
        public static async Task CaptCall(Browser browser, string url, long userId, DateTimeOffset time)
        {
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            Thread.Sleep(TimeSpan.FromSeconds(20D));
            await page.ScreenshotAsync($"/home/cqbot/images/answer-{time}-{userId}.png", new ScreenshotOptions {FullPage = true});
            await page.CloseAsync();
        }

        public static string UrlHandle(string input)
        {
            var searchItem = HttpUtility.UrlEncode(input);
            return "https://www.wolframalpha.com/input/?i=" + searchItem;
        }
    }
}