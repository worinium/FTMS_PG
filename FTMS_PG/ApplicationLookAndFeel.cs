
using MaterialSkin;
using MaterialSkin.Controls;
using System.Drawing;
using System.Windows.Forms;



namespace FTMS_PG
{
    public class ApplicationLookAndFeel
    {
        static void ApplyTheme(TextBox c)
        {
            c.Font = new Font("Arial", 10.0f);
        }
        static void ApplyTheme(Label c)
        {
            c.Font = new Font("Arial", 8.0f); c.ForeColor = Color.White; c.BackColor = Color.Transparent;
        }
        static void ApplyTheme(TabControl c)
        {
            c.BackColor = Color.FromArgb(15, 91, 153);
        }
        static void ApplyTheme(MaterialForm c)
        {
            c.BackColor = Color.LightGray;
        }
        static void ApplyTheme(Form c)
        {
            c.BackColor = Color.LightGray;
        }
        static void ApplyTheme(DataGridView c)
        {
            c.BackColor = Color.LightGray;

        }
        private static void UseCustomTheme(Form form)
        {
            ApplyTheme(form);
            foreach (var c in form.Controls)
            {
                switch (c.GetType().ToString())
                {
                    case "System.Windows.Forms.Label":
                        ApplyTheme((Label)c);
                        break;
                    case "System.Windows.Forms.TextBox":
                        ApplyTheme((TextBox)c);
                        break;
                    case "System.Windows.Forms.TabControl":
                        ApplyTheme((TabControl)c);
                        break;
                }
            }
        }
        private static void UseCustomTheme(MaterialForm form)
        {
            ApplyTheme(form);
            foreach (var c in form.Controls)
            {
                switch (c.GetType().ToString())
                {
                    case "System.Windows.Forms.Label":
                        ApplyTheme((Label)c);
                        break;
                    case "System.Windows.Forms.TextBox":
                        ApplyTheme((TextBox)c);
                        break;
                    case "System.Windows.Forms.TabControl":
                        ApplyTheme((TabControl)c);
                        break;
                    //case "System.Windows.Forms.DataGridView":
                    //    ApplyTheme((TabControl)c);
                    //    break;
                }
            }
        }
        public static void UseMaterialSkin(MaterialForm form, int count)
        {
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(form);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            UseCustomTheme(form);
            switch (count)
            {
                case 1:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.Orange700, Primary.Orange900, Primary.Orange500, Accent.Orange400, TextShade.WHITE);

                    break;
                case 2:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.Green700, Primary.Green900, Primary.Green800, Accent.Green400, TextShade.WHITE);
                    break;
                case 3:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.Cyan700, Primary.Cyan900, Primary.Blue800, Accent.Green400, TextShade.WHITE);
                    break;
                case 4:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey800, Primary.BlueGrey700, Accent.LightBlue400, TextShade.WHITE);
                    break;
                case 5:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.DeepPurple700, Primary.DeepPurple900, Primary.DeepPurple900, Accent.DeepPurple400, TextShade.WHITE);
                    break;
                case 6:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo700, Primary.Indigo900, Primary.Indigo800, Accent.Pink400, TextShade.WHITE);
                    break;
                case 7:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.DeepOrange700, Primary.DeepOrange900, Primary.DeepOrange700, Accent.Green400, TextShade.WHITE);
                    break;
                case 9:
                    materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(Primary.LightBlue700, Primary.LightBlue700, Primary.BlueGrey800, Accent.LightBlue400, TextShade.WHITE);
                    break;
                case 8:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue400, Primary.Blue500, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
                    break;
                default:
                    materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo700, Primary.Indigo900, Primary.Indigo800, Accent.Indigo400, TextShade.WHITE);
                    break;
            }
        }
    }
}
