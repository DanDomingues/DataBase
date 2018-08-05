using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericUtility
{

    public static Boolean ConcatBool(Boolean[] compares)
    {
        Boolean output = false;

        for (int i = 0; i < compares.Length; i++)
        {
            if (compares[i]) return true;
        }

        return output;
    }

    public static float ConcatAxis(float[] axis, float threashold)
    {
        float output = 0.0f;
        int counter = 0;

        for (int i = 0; i < axis.Length; i++)
        {
            if (Math.Abs(axis[i]) > threashold)
            {
                output += Mathf.Clamp(axis[i], -1.0f, 1.0f);
                counter++;
            }
        }
        counter = counter == 0 ? 1 : counter;

        //Debug.Log(string.Format("Output: {0} Raw :{1} Counter:{2}", output / counter, output, counter));
        return output / counter;
    }

    public static int LoopInput(int value, int length)
    {
        while (value < 0) value += length;
        while (value >= length) value -= length;

        return value;
    }

    public static Enum[] GetEnums (Type type)
    {
        var names = Enum.GetNames(type);
        int length = names.Length;

        var enums = new Enum[length];
        for (int i = 0; i < length; i++)
        {
            enums[i] = (Enum) Enum.Parse(type, names[i]);
        }

        return enums;

    }

    public static Enum GetRandom(this Enum[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

        public static object[] ExpandArray(object[] input, object insert)
    {
        List<object> list = new List<object>(input);
        list.Add(insert);
        return list.ToArray();
    }

    public static int BiggestOfTwo(int a, int b)
    {
        int result = 0;

        if (a > b) result = a;
        else result = b;

        return result;
    }

    public static bool Compare(float compared, float comparison, int way)
    {

        if (way < 0) return compared < comparison;

        if (way > 0) return compared > comparison;

        else return false;

    }

}
