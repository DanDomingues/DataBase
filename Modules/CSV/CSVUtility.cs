using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CSVUtility
{
    public static void ParseDamage(string input, out int value)
    {
        value = 0;
        int second = 0;
        ParseDamage(input, out value, out second);
    }
    public static void ParseDamage(string input, out int value, out int secondaryValue)
    {
        value = 0;
        secondaryValue = 0;

        input.Trim('x');
        if(input.Contains("/"))
        {
            var split = input.Split('/');
            int.TryParse(split[0], out value);
            int.TryParse(split[1], out secondaryValue);
        }
        else
        {
            int.TryParse(input, out value);
        }
    }

    public static void ParseRange(string input, out int min, out int max)
    {
        input = input.Trim(' ');
        min = 0;
        max = 0;

        if(input.Contains("--"))
        {

        }
        else if(input.Contains("-"))
        {
            var split = input.Split('-');
            int.TryParse(split[0], out min);

            if (split[1].ToLower().Contains("all"))
                max = 1000000;
            else
                int.TryParse(split[1], out max);
        }
        else if(input.Contains("to"))
        {
            for (int i = 0; i < input.Length; i++)
            {
                if(input[i] == 't')
                {
                    int.TryParse(input.Substring(0, i), out min);
                    int.TryParse(input.Substring(i + 2, input.Length - (i + 2)), out max);
                    break;
                }
            }
        }
        else
        {
            int.TryParse(input, out min);
            int.TryParse(input, out max);
        }
    }

    public static void TryParse(string input, out float value)
    {
        input = input.Trim(' ').Replace('.', ',');
        float.TryParse(input, out value);
    }
}
