using System;
using System.Drawing;
using System.Windows.Forms;

namespace smartLibraryForC_.Services
{
    /// <summary>
    /// مدير السمات (Dark Mode / Light Mode)
    /// </summary>
    public class ThemeManager
    {
        private static ThemeManager _instance;
        private bool _isDarkMode = false;

        public static ThemeManager Instance => _instance ?? (_instance = new ThemeManager());

        public bool IsDarkMode => _isDarkMode;

        // Light Theme Colors
        private static class LightTheme
        {
            public static readonly Color Primary = ColorTranslator.FromHtml("#2196F3");
            public static readonly Color Secondary = ColorTranslator.FromHtml("#673AB7");
            public static readonly Color Accent = ColorTranslator.FromHtml("#FF5722");
            public static readonly Color Background = ColorTranslator.FromHtml("#FAFAFA");
            public static readonly Color Surface = Color.White;
            public static readonly Color TextPrimary = ColorTranslator.FromHtml("#212121");
            public static readonly Color TextSecondary = ColorTranslator.FromHtml("#757575");
            public static readonly Color CardBorder = ColorTranslator.FromHtml("#E0E0E0");
            public static readonly Color SidebarGradientStart = Color.WhiteSmoke;
            public static readonly Color SidebarGradientEnd = Color.WhiteSmoke;
        }

        // Dark Theme Colors
        private static class DarkTheme
        {
            public static readonly Color Primary = ColorTranslator.FromHtml("#64B5F6");
            public static readonly Color Secondary = ColorTranslator.FromHtml("#9575CD");
            public static readonly Color Accent = ColorTranslator.FromHtml("#FF7043");
            public static readonly Color Background = ColorTranslator.FromHtml("#121212");
            public static readonly Color Surface = ColorTranslator.FromHtml("#1E1E1E");
            public static readonly Color TextPrimary = ColorTranslator.FromHtml("#FFFFFF");
            public static readonly Color TextSecondary = ColorTranslator.FromHtml("#B0B0B0");
            public static readonly Color CardBorder = ColorTranslator.FromHtml("#2C2C2C");
            public static readonly Color SidebarGradientStart = ColorTranslator.FromHtml("#1E1E1E");
            public static readonly Color SidebarGradientEnd = ColorTranslator.FromHtml("#1E1E1E");
        }

        public Color GetColor(string colorName)
        {
            try
            {
                var colorType = _isDarkMode ? typeof(DarkTheme) : typeof(LightTheme);
                var field = colorType.GetField(colorName, 
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                
                return field != null ? (Color)field.GetValue(null) : Color.Gray;
            }
            catch
            {
                return Color.Gray;
            }
        }

        public void ToggleTheme()
        {
            _isDarkMode = !_isDarkMode;
        }

        public void SetTheme(bool darkMode)
        {
            _isDarkMode = darkMode;
        }

        /// <summary>
        /// تطبيق السمة على نموذج أو تحكم
        /// </summary>
        public void ApplyTheme(Control control)
        {
            if (control == null) return;

            control.BackColor = GetColor("Background");
            control.ForeColor = GetColor("TextPrimary");

            ApplyThemeToControls(control.Controls);
        }

        private void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // Panels
                if (control is Panel panel)
                {
                    if (panel.Name == "panelSidebar" || panel.Name.Contains("Sidebar"))
                    {
                        // Sidebar gets special gradient color
                        panel.BackColor = _isDarkMode 
                            ? GetColor("SidebarGradientEnd")  // Dark purple for dark mode
                            : GetColor("SidebarGradientStart"); // Blue for light mode
                    }
                    else if (panel.Name.Contains("Card") || panel.Name.Contains("Stat"))
                    {
                        panel.BackColor = GetColor("Surface");
                        panel.ForeColor = GetColor("TextPrimary");
                    }
                    else
                    {
                        panel.BackColor = GetColor("Background");
                    }
                }
                // Buttons
                else if (control is Button button)
                {
                    // Sidebar buttons - special treatment
                    if (button.Parent?.Name == "panelSidebar")
                    {
                        button.ForeColor = GetColor("TextPrimary");
                        button.BackColor = Color.Transparent;
                    }
                    else if (button.FlatStyle == FlatStyle.Flat)
                    {
                        if (button.BackColor == Color.DodgerBlue || button.BackColor == SystemColors.Control)
                        {
                            button.BackColor = GetColor("Primary");
                            button.ForeColor = Color.White;
                        }
                    }
                }
                // Labels
                else if (control is Label label)
                {
                    // Labels in sidebar should follow theme
                    if (label.Parent?.Name == "panelSidebar" || 
                        label.Parent?.Parent?.Name == "panelSidebar" ||
                        label.Parent?.Name == "panelUser")
                    {
                        label.ForeColor = GetColor("TextPrimary");
                    }
                    // Standard text colors -> apply theme
                    else if (label.ForeColor == Color.Black || 
                             label.ForeColor == Color.White || 
                             label.ForeColor == Color.Gray || 
                             label.ForeColor == Color.DarkGray || 
                             label.ForeColor == SystemColors.ControlText)
                    {
                        if (label.ForeColor == Color.Gray || label.ForeColor == Color.DarkGray)
                            label.ForeColor = GetColor("TextSecondary");
                        else
                            label.ForeColor = GetColor("TextPrimary");
                    }
                    // Distinct colors (Blue, Red, Green...) -> Preserve them
                }
                // TextBoxes
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = GetColor("Surface");
                    textBox.ForeColor = GetColor("TextPrimary");
                }
                // Group Boxes
                else if (control is GroupBox groupBox)
                {
                    groupBox.ForeColor = GetColor("TextPrimary");
                    // Recursively apply to children
                    ApplyThemeToControls(control.Controls);
                }

                // Recursively apply to children
                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls);
                }
            }
        }
    }
}
