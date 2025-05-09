using PuppeteerSharp;

namespace HtmlRenderAPI.Services
{
    public class HtmlRenderService
    {

        public async Task<string> ConvertHtmlToImage(string html)
        {
            var browserFetcher = new BrowserFetcher(); //download do chromium 
            await browserFetcher.DownloadAsync();

            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }); //inicia o navegador e abre uma aba
            using var page = await browser.NewPageAsync(); 
            await page.SetContentAsync(html);
            var screenshot = await page.ScreenshotDataAsync(); //insere o html na pagina e tira print 

            return Convert.ToBase64String(screenshot); //converte a imagem para base64
        }
    }
}
