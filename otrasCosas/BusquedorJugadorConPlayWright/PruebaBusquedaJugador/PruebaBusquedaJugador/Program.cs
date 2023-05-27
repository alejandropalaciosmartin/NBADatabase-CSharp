using Microsoft.Playwright;

namespace PruebaBusquedaJugador;

class Program
{
    static async Task Main(string[] args)
    {
        Microsoft.Playwright.Program.Main(new[] { "install" });

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });

        var context = await browser.NewContextAsync();

        // Abrir una nueva página
        var page = await context.NewPageAsync();

        //Texto a buscar
        string nombre = "Pikachu";

        // Navegar a Google
        await page.GotoAsync($"https://www.google.es/search?q={nombre}&source=lnms&tbm=isch&sa=X&ved=2ahUKEwi_-fumwoT_AhWkTqQEHV6eBPYQ_AUoA3oECAMQBQ");

        // Esperar a que se cargue la página
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //Aceptar las cookies
        ILocator locator = page.Locator("div.VtwTSb [aria-label=\"Aceptar todo\"]");
        await locator.WaitForAsync(new() { Timeout = 3000 });
        await locator.ClickAsync();

        Thread.Sleep(1500);

        //Cojo todas las imagenes
        IElementHandle itemList = await page.QuerySelectorAsync("div.isv-r img.rg_i");

        //Me quedo con la src de la primera imagen
        string url = await itemList.GetAttributeAsync("src");

        Console.WriteLine(url);
    }
}