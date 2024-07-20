using System.Text;

namespace Agent.Utilities
{
    public class AnsiMessageBuilder
    {
        private static readonly Dictionary<Color, string> ColorCodes = new()
        {
        { Color.Black, "\u001b[30m" },
        { Color.Red, "\u001b[31m" },
        { Color.Green, "\u001b[32m" },
        { Color.Yellow, "\u001b[33m" },
        { Color.Blue, "\u001b[34m" },
        { Color.Magenta, "\u001b[35m" },
        { Color.Cyan, "\u001b[36m" },
        { Color.White, "\u001b[37m" },
        { Color.Reset, "\u001b[0m" },
        { Color.BrightBlack, "\u001b[90m" },
        { Color.BrightRed, "\u001b[91m" },
        { Color.BrightGreen, "\u001b[92m" },
        { Color.BrightYellow, "\u001b[93m" },
        { Color.BrightBlue, "\u001b[94m" },
        { Color.BrightMagenta, "\u001b[95m" },
        { Color.BrightCyan, "\u001b[96m" },
        { Color.BrightWhite, "\u001b[97m" }
    };

        private readonly StringBuilder _builder;

        public AnsiMessageBuilder()
        {
            _builder = new StringBuilder();
        }

        public enum Color
        {
            Black,
            Red,
            Green,
            Yellow,
            Blue,
            Magenta,
            Cyan,
            White,
            Reset,
            BrightBlack,
            BrightRed,
            BrightGreen,
            BrightYellow,
            BrightBlue,
            BrightMagenta,
            BrightCyan,
            BrightWhite
        }

        public AnsiMessageBuilder AddText(string text, Color color = Color.Reset)
        {
            _builder.Append(ColorCodes[color]);
            _builder.Append(text);
            _builder.Append(ColorCodes[Color.Reset]);
            return this;
        }

        public AnsiMessageBuilder AddNewLine()
        {
            _builder.Append(Environment.NewLine);
            return this;
        }

        public byte[] Build()
        {
            return Encoding.ASCII.GetBytes(_builder.ToString());
        }
    }
}