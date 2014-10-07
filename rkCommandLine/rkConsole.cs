using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rkCommandLine {
  public static class rkConsole {
    //static readonly Regex colorFlagRegex = new Regex(@"&([0-9a-fA-FrR-]{1,2});");
    static readonly Regex colorFlagRegex = new Regex(@"&([kbgcrmywKBGCRMYW.-]{1,2});");

    static readonly Dictionary<char, ConsoleColor> hrColorMap = new Dictionary<char, ConsoleColor>() {
      {'k', ConsoleColor.Black},
      {'b', ConsoleColor.DarkBlue},
      {'g', ConsoleColor.DarkGreen},
      {'c', ConsoleColor.DarkCyan},
      {'r', ConsoleColor.DarkRed},
      {'m', ConsoleColor.DarkMagenta},
      {'y', ConsoleColor.DarkYellow},
      {'w', ConsoleColor.Gray},
      {'K', ConsoleColor.DarkGray},
      {'B', ConsoleColor.Blue},
      {'G', ConsoleColor.Green},
      {'C', ConsoleColor.Cyan},
      {'R', ConsoleColor.Red},
      {'M', ConsoleColor.Magenta},
      {'Y', ConsoleColor.Yellow},
      {'W', ConsoleColor.White},
    };

    public static void Write(object obj) { Console.Write(obj); }

    public static void Write(string format, params object[] args) {
      ConsoleColor currFG = Console.ForegroundColor,
        currBG = Console.BackgroundColor;
      string[] split = colorFlagRegex.Split(format);
      for(int i = 0; i < split.Length; i++) {
        if(i % 2 != 0) {
          char part = split[i][0];
          if(part != '-') {
            Console.ForegroundColor = part == '.' ? currFG : hrColorMap[part];
          }
          if(split[i].Length == 2) {
            part = split[i][1];
            if(part != '-') {
              Console.BackgroundColor = part == '.' ? currBG : hrColorMap[part];
            }
          }
        }
        else Console.Write(split[i], args);
      }
      Console.ForegroundColor = currFG;
      Console.BackgroundColor = currBG;
    }

    public static void Write(string str) { Write(format: str); }

    public static void WriteLine() { Console.WriteLine(); }

    public static void WriteLine(object obj) { Console.WriteLine(obj); }

    public static void WriteLine(string format, params object[] args) {
      if(!string.IsNullOrEmpty(format)) Write(format, args);
      Console.WriteLine();
    }

    public static void WriteLine(string str) { WriteLine(format: str); }

    public static void Pause(string msg = null) {
      Write(msg);
      Console.CursorVisible = false;
      Console.ReadKey(true);
      Console.WriteLine();
      Console.CursorVisible = true;
    }

    static ConsoleColor? currColor = null,
      currBkgd = null;

    public static void PushColor(ConsoleColor color) {
      if(!currColor.HasValue) currColor = Console.ForegroundColor;
      Console.ForegroundColor = color;
    }

    public static void PushColor(char hrColor) {
      PushColor(hrColorMap[hrColor]);
    }

    public static void PushColor(string hrColors) {
      if(hrColors.Length < 1 || hrColors.Length > 2) throw new InvalidOperationException("Exactly one or two colors can be pushed.");
      PushColor(hrColorMap[hrColors[0]]);
      if(hrColors.Length == 2) {
        PushBkgd(hrColorMap[hrColors[1]]);
      }
    }

    public static void PopColor() {
      if(!currColor.HasValue) throw new InvalidOperationException("No color to pop!");
      Console.ForegroundColor = currColor.Value;
      currColor = null;
    }

    public static void PushBkgd(ConsoleColor bkgd) {
      if(!currBkgd.HasValue) currBkgd = Console.BackgroundColor;
      Console.BackgroundColor = bkgd;
    }

    public static void PushBkgd(char hrBkgd) {
      PushBkgd(hrColorMap[hrBkgd]);
    }

    public static void PopBkgd() {
      if(!currBkgd.HasValue) throw new InvalidOperationException("No color to pop!");
      Console.BackgroundColor = currBkgd.Value;
      currBkgd = null;
    }
  }
}
