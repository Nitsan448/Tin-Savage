using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class TextExtensions
{
    public static string GetColoredText(this string text, string color)
    {
        StringBuilder stringBuilder = new StringBuilder("<color=");
        stringBuilder.Append(color);
        stringBuilder.Append(">");
        stringBuilder.Append(text);
        stringBuilder.Append("</color>");
        return stringBuilder.ToString();
    }
}