using MudBlazor;

namespace Adoptrix.Client.Services;

public class ThemeProvider
{
    public MudTheme Theme { get; } = new()
    {
        Typography = new Typography
        {
            Default = new Default
            {
                FontFamily = ["Quicksand", "sans-serif"]
            }
        }
    };
}
