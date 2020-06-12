using System;
using System.Diagnostics;
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
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = "/usr/bin/google-chrome"
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            Thread.Sleep(TimeSpan.FromSeconds(20D));
            await page.ScreenshotAsync($"/home/cqbot/images/answer-{time.Minute}-{time.Hour}-{time.Day}-{time.Month}-{time.Year}-{userId}.png", new ScreenshotOptions {FullPage = true});
            await page.CloseAsync();
            await browser.CloseAsync();
            browser.Dispose();
            KillChromeProcess();
        }

        public static string UrlHandle(string input)
        {
            var searchItem = HttpUtility.UrlEncode(input);
            return "https://www.wolframalpha.com/input/?i=" + searchItem;
        }

        public static void KillChromeProcess() {
            try
            {
                var proc = Process.Start("killall", "chrome");
                proc.WaitForExit();
                if (proc.ExitCode == 0) {
                    System.Console.WriteLine("Kill Chrome Process Successfully!");
                }
                proc.Close();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }
    }
}