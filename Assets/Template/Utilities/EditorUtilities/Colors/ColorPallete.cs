using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ColorPallete", fileName = "New Color Pallete")]
public class ColorPallete : ScriptableObject
{
    public List<Color> Colors;
}