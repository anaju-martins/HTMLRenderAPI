using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace HtmlRenderAPI.Services
{
    public enum OutputFormat
    {
        Jpeg,
        Pdf
    }

    public class HtmlRenderService : IAsyncDisposable
    {
        private IBrowser _browser;
        private readonly ILogger<HtmlRenderService> _logger;
        private readonly SemaphoreSlim _lock = new(1, 1); 

        public HtmlRenderService(ILogger<HtmlRenderService> logger)
        {
            _logger = logger;
        }

        private async Task EnsureBrowserInitializedAsync()
        {
            if (_browser != null && _browser.IsConnected)
                return;

            await _lock.WaitAsync();
            try
            {
                if (_browser != null)
                {
                    await _browser.DisposeAsync();
                    _browser = null;
                }

                _logger.LogInformation("Baixando Chromium...");
                await new BrowserFetcher().DownloadAsync();

                var launchOptions = new LaunchOptions
                {
                    Headless = true,
                    Args = new[]
                    {
                        "--no-sandbox",
                        "--disable-setuid-sandbox",
                        "--disable-dev-shm-usage",
                        "--disable-gpu"
                    }
                };

                _browser = await Puppeteer.LaunchAsync(launchOptions);
                _logger.LogInformation("Chromium iniciado. PID={pid}", _browser.Process?.Id);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<string> ConvertHtmlToFormattedOutput(string html, OutputFormat format)
        {
            try
            {
                await EnsureBrowserInitializedAsync();

                using var page = await _browser.NewPageAsync();

                await page.SetViewportAsync(new ViewPortOptions
                {
                    Width = 700,
                    Height = 600, // dimensao da imagem quando for jpeg
                    DeviceScaleFactor = 2 
                });

                _logger.LogInformation("Renderizando HTML com {Length} caracteres para {Format}", html.Length, format);

                await page.SetContentAsync(html, new NavigationOptions
                {
                    Timeout = 60000 // 60s para evitar timeout por conteúdo pesado
                });

                byte[] outputBytes;

                if (format == OutputFormat.Jpeg)
                {
                    outputBytes = await page.ScreenshotDataAsync(new ScreenshotOptions
                    {
                        Type = ScreenshotType.Jpeg,
                        Quality = 80,
                        FullPage = true
                    });
                }
                else if (format == OutputFormat.Pdf)
                {
                    outputBytes = await page.PdfDataAsync(new PdfOptions
                    {
                        Format = PuppeteerSharp.Media.PaperFormat.A4,
                        PrintBackground = true,
                        MarginOptions = new MarginOptions { Top = "1cm", Bottom = "1cm", Left = "1cm", Right = "1cm" }
                    });
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(format), "Invalid output format specified.");
                }

                return Convert.ToBase64String(outputBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao converter HTML para {Format}", format);
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
                await _browser.DisposeAsync();
                _logger.LogInformation("Chromium encerrado.");
            }
        }
    }
}