using System.Collections.Generic;
using UnityEngine;

public class SpellArguments : MonoBehaviour
{
    public static List<string> ReadInput(string format, string input)
    {
        if(format == null || format.Length == 0) {
            Debug.LogError("Action format is not specified.");
            return new List<string>();
        }
        int argMax = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(format[i]))
            {
                int k = format[i] - '1';
                argMax = Mathf.Max(argMax, k);
            }
        }
        List<string> args = new List<string>(new string[argMax + 1]);
        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(format[i]))
            {
                int k = format[i] - '1';
                args[k] += input[i];
            }
        }
        for (int i = 0; i < argMax; i++)
        {
            if (args[i].Length == 0)
            {
                Debug.LogWarning("Empty argument");
            }
        }
        return args;
    }

    public static int SymbolToInt(char symbol)
    {
        switch (symbol)
        {
            case 'S': return 0;
            case 'D': return 1;
            case 'A': return 2;
            case 'W': return 3;
            default:
                Debug.LogWarning("unexpected symbol char");
                return 0;
        }
    }
    public static List<int> ArgToIntList(string arg)
    {
        List<int> res = new List<int>();
        foreach (char c in arg)
        {
            res.Add(SymbolToInt(c));
        }
        return res;
    }

    /// <summary>
    /// Converts two symbol arg to direction (first is general direction, second is small offset). 
    /// </summary>
    /// <param name="arg">Argument in two symbols</param>
    /// <param name="correct">Displays if format is correct</param>
    /// <returns>Direction angle or NaN if wrong format</returns>
    public static float ArgToAngle(string arg, ref bool correct)
    {
        List<string> dirs = new List<string> { "WW", "WA", "AW", "AA", "AS", "SA", "SS", "SD", "DS", "DD", "DW", "WD" };
        List<float> angle = new List<float> { 0f, 30f, 60f, 90f, 120f, 150f, 180f, -150f, -120f, -90f, -60f, -30f };
        for (int i = 0; i < dirs.Count; i++)
        {
            if (arg == dirs[i])
            {
                correct = true;
                return angle[i];
            }
        }
        correct = false;
        return float.NaN;
    }
    public static Direction ArgToDirection(string arg, ref bool correct)
    {
        List<string> dirs = new List<string> { "WW", "WD", "DW", "DD", "DS", "SD", "SS", "SA", "AS", "AA", "AW", "WA" };
        List<int> vals = new List<int> { 0, 1, 1, 2, 3, 3, 4, 5, 5, 6, 7, 7 };
        for (int i = 0; i < dirs.Count; i++)
        {
            if (arg == dirs[i])
            {
                correct = true;
                return (Direction)vals[i];
            }
        }
        correct = false;
        return Direction.Up;
    }
}
