using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using PuppeteerSharp;

namespace cqbot
{
    public static class ImageCapt
    {
        public static async Task CaptCall(string url)
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = "/usr/bin/chromium-browser"
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            Thread.Sleep(TimeSpan.FromSeconds(20D));
            await page.ScreenshotAsync("/home/cqbot/images/answer.png", new ScreenshotOptions {FullPage = true});
            await browser.CloseAsync();
        }

        public static string UrlHandle(string input)
        {
            var searchItem = HttpUtility.UrlEncode(input);
            return "https://www.wolframalpha.com/input/?i=" + searchItem;
        }

        public static async Task<bool> IsUserLocked(List<long> userLock, long userId)
        {
            return userLock.Contains(userId);
        }

        public static async Task AddUserToLocks(List<long> userLock, long userId)
        {
            userLock.Add(userId);
        }

        public static async Task RemoveUserFromLocks(List<long> userLock, long userId)
        {
            userLock.Remove(userId);
        }
    }
}