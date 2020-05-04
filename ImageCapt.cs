using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using PuppeteerSharp;

namespace cqbot
{
    public static class ImageCapt
    {
        public static async Task CaptCall(string url, long userId, DateTimeOffset time)
        {
            var options = new LaunchOptions
            {
                Headless = true,
                ExecutablePath = "/usr/bin/chromium-browser"
            };
            var browser = await Puppeteer.LaunchAsync(options);
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            Thread.Sleep(TimeSpan.FromSeconds(20D));
            await page.ScreenshotAsync($"/home/cqbot/images/answer-{time.Minute}-{time.Hour}-{time.Day}-{time.Month}-{time.Year}-{userId}.png", new ScreenshotOptions {FullPage = true});
            await page.CloseAsync();
            await browser.CloseAsync();
            browser.Dispose();
        }

        public static string UrlHandle(string input)
        {
            var searchItem = HttpUtility.UrlEncode(input);
            return "https://www.wolframalpha.com/input/?i=" + searchItem;
        }
    }
}