using System;
using System.Collections.Generic;
using System.Text;

namespace Common;

public class Helpers
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
