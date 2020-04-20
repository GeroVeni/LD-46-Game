using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    float level;

    public Color fullColor;
    public Color halfColor;
    public Color emptyColor;
    public SpriteRenderer[] bars;

    public float Level
    {
        get { return level; }
        set
        {
            level = Mathf.Clamp01(value);
            int barCount = Mathf.CeilToInt(level / 0.2f);
            for (int i = 0; i < bars.Length; ++i)
            {
                bars[i].color = GetColor();
                if (i < barCount) { bars[i].enabled = true; }
                else { bars[i].enabled = false; }
            }
        }
    }

    Color GetColor()
    {
        if (level > 0.5)
        {
            return Color.Lerp(halfColor, fullColor, 2 * level - 1);
        }
        return Color.Lerp(emptyColor, halfColor, 2 * level);
    }

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
