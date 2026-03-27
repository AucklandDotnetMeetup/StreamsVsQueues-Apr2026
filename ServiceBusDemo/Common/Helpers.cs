using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common;

public static class Helpers
{
    public static string GetCurrentTime(string format = "HH:mm:ss.ff")
    {
        return DateTime.UtcNow.ToString(format);
    }

    public static int GetValueFromMinToMax(int input, int min, int max)
    {
        return input < min ? min : input > max ? max : input;
    }
}
