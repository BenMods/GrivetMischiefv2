using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFog : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            RenderSettings.fogDensity = 0.15f;
            RenderSettings.fogColor = HexToColor("#0049CC");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            RenderSettings.fogDensity = 0f;
            RenderSettings.fogColor = HexToColor("#0049CC");
        }
    }

    // Function to convert hexadecimal color string to Color
    private Color HexToColor(string hex)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}