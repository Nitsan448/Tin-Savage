using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public class Colorer : MonoBehaviour
{
    public ColorPallete ColorPallete;

    public bool ShouldColorChildren;

    [HideInInspector] public Color PreviousColor = UnityEngine.Color.white;
    public Color SpecificColor = UnityEngine.Color.white;

    public void Color(Color color)
    {
        Debug.Log("Coloring");
        if (ShouldColorChildren)
        {
            ColorChildren(color);
        }
        else
        {
            ColorElement(color);
        }
    }

    private void ColorChildren(Color color)
    {
        foreach (Graphic graphic in GetComponentsInChildren<Graphic>())
        {
            PreviousColor = graphic.color;
            graphic.color = color;
        }

        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            PreviousColor = spriteRenderer.color;
            spriteRenderer.color = color;
        }

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            PreviousColor = renderer.material.color;
            renderer.material.color = color;
        }
    }

    private void ColorElement(Color color)
    {
        if (GetComponent<Graphic>() != null)
        {
            PreviousColor = GetComponent<Graphic>().color;
            GetComponent<Graphic>().color = color;
        }

        else if (GetComponent<SpriteRenderer>() != null)
        {
            PreviousColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = color;
        }
        else if (GetComponent<Renderer>() != null)
        {
            PreviousColor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.color = color;
        }
    }
}
#endif