using MudBlazor;

namespace WebBuilder2.Client.Layouts;

public partial class BaseLayout
{
    public MudTheme MyCustomTheme { get; set; } = new MudTheme()
    {
        Palette = new PaletteLight()
        {
            Primary = "#F21137",
            Secondary = "#68020F",
        },
    };
}
