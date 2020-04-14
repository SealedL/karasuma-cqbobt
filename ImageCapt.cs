using System;
using System.Diagnostics;
using System.Web;

namespace cqbot
{
    public static class ImageCapt
    {
        public static void CaptCall(string url)
        {
            try
            {
                var qtcapt = new ProcessStartInfo("xvfb-run", "--server-args=\"-screen 0, 1920x1080x24\" " +
                                                               $"cutycapt --url=\"{url}\" " +
                                                               "--delay=5000" +
                                                               "--out=\"/home/cqbot/images/result.png\"");
                qtcapt.EnvironmentVariables.Add("http_proxy", "http://localhost:8118");
                qtcapt.EnvironmentVariables.Add("https_proxy", "http://localhost:8118");
                var proc = Process.Start(qtcapt);
                if (proc == null) return;
                var sr = proc.StandardOutput;
                    
                while (!sr.EndOfStream)
                {
                    Console.WriteLine(sr.ReadLine());
                }

                if (!proc.HasExited)
                {
                    proc.Kill();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static string UrlHandle(string input)
        {
            var searchItem = HttpUtility.UrlEncode(input);
            return "https://www.wolframalpha.com/input/?i=" + searchItem;
        }
    }
}