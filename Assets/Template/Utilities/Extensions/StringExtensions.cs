using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Unity.VisualScripting;

public static class StringExtensions
{
    public static Vector3 ToVector3(this string vector)
    {
        List<float> floatList = vector.ToFloatList();
        return new Vector3(floatList[0], floatList[1], floatList[2]);
    }

    private static List<float> ToFloatList(this string vector)
    {
        RemoveIrrelevantChars(ref vector);

        string[] vectorValues = vector.Split(',');

        List<float> result = new();
        foreach (string vectorValue in vectorValues)
        {
            result.Add(float.Parse(vectorValue));
        }

        return result;
    }

    private static void RemoveIrrelevantChars(ref string row)
    {
        row = row.Replace(" ", string.Empty);
        row = row.Replace("+", string.Empty);
        row = row.Replace("\n", string.Empty);
        row = row.Replace("(", string.Empty);
        row = row.Replace(")", string.Empty);
    }

    public static string Vector3ToString(Vector3 vector)
    {
        StringBuilder stringBuilder = new("(");
        stringBuilder.Append(vector.x.ToString());
        stringBuilder.Append(", ");
        stringBuilder.Append(vector.y.ToString());
        stringBuilder.Append(", ");
        stringBuilder.Append(vector.z.ToString());
        stringBuilder.Append(")");
        return stringBuilder.ToString();
    }

    public static bool IsVectorStringFormatValid(string vector)
    {
        RemoveIrrelevantChars(ref vector);
        string[] vectorValues = vector.Split(',');

        foreach (string vectorValue in vectorValues)
        {
            bool floatInputWasValid = float.TryParse(vectorValue, out float value);
            if (!floatInputWasValid)
            {
                return false;
            }
        }

        return true;
    }

    public static int GetNumericPartStartIndex(this string input)
    {
        int numericPartStartIndex = 0;
        while (!char.IsDigit(input[numericPartStartIndex]))
        {
            numericPartStartIndex++;
            if (numericPartStartIndex > input.Length)
            {
                Debug.Log("input is not in valid format");
                return -1;
            }
        }

        return numericPartStartIndex;
    }
}