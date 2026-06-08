namespace UI;

public static class AppTheme
{
    public static MudTheme Theme => new MudTheme
    {
        Typography = new Typography
        {
            Default = new DefaultTypography { FontFamily = new[] { "Roboto", "sans-serif" } }
        },
        PaletteLight = new PaletteLight
        {
            Primary         = "#7C3AED",
            PrimaryDarken   = "#5B21B6",
            PrimaryLighten  = "#DDD6FE",
            Secondary       = "#DB2777",
            Tertiary        = "#0284C7",
            Success         = "#059669",
            Warning         = "#D97706",
            Error           = "#DC2626",
            Info            = "#0284C7",
            Background      = "#F8F8FC",
            BackgroundGray  = "#F3F2F8",
            Surface         = "#FFFFFF",
            AppbarBackground = "rgba(248,248,252,0.85)",
            AppbarText      = "#1E1040",
            DrawerBackground = "#F5F4FA",
            DrawerText      = "#1E1040",
            DrawerIcon      = "#6D28D9",
            TextPrimary     = "#1E1040",
            TextSecondary   = "#6D28D9",
            TextDisabled    = "rgba(109,40,217,0.4)",
            ActionDefault   = "#7C3AED",
            ActionDisabled  = "rgba(124,58,237,0.26)",
            ActionDisabledBackground = "rgba(124,58,237,0.12)",
            Divider         = "rgba(124,58,237,0.18)",
            LinesDefault    = "rgba(124,58,237,0.18)",
            TableLines      = "rgba(124,58,237,0.18)",
            OverlayDark     = "rgba(13,8,23,0.5)",
        },
        PaletteDark = new PaletteDark
        {
            Primary         = "#7C3AED",
            PrimaryDarken   = "#5B21B6",
            PrimaryLighten  = "#A78BFA",
            Secondary       = "#EC4899",
            Tertiary        = "#06B6D4",
            Success         = "#10B981",
            Warning         = "#F59E0B",
            Error           = "#EF4444",
            Info            = "#06B6D4",
            Background      = "#0D0817",
            BackgroundGray  = "#160D2B",
            Surface         = "#1E1040",
            AppbarBackground = "rgba(13,8,23,0.75)",
            AppbarText      = "#F3F0FF",
            DrawerBackground = "#110C24",
            DrawerText      = "#F3F0FF",
            DrawerIcon      = "#A78BFA",
            TextPrimary     = "#F3F0FF",
            TextSecondary   = "#A78BFA",
            TextDisabled    = "rgba(167,139,250,0.4)",
            ActionDefault   = "#7C3AED",
            ActionDisabled  = "rgba(124,58,237,0.26)",
            ActionDisabledBackground = "rgba(124,58,237,0.12)",
            Divider         = "rgba(167,139,250,0.15)",
            LinesDefault    = "rgba(167,139,250,0.12)",
            TableLines      = "rgba(167,139,250,0.12)",
            OverlayDark     = "rgba(13,8,23,0.7)",
        }
    };
}