using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorBar : MonoBehaviour
{
    public Color color;
    public SpriteRenderer background;
    public SpriteRenderer bar;

    [Range(0, 1)]
    public float startHeight;
    float height;

    public float Height
    {
        get { return height; }
        set
        {
            height = Mathf.Clamp01(value);

            // Set the bar position
            float extentY = background.bounds.extents.y;
            Vector3 center = background.bounds.center;
            Vector3 bottom = center - new Vector3(0, extentY);
            bar.transform.position = Vector3.Lerp(bottom, center, height);

            // Set the bar scale
            Vector3 backgroundScale = background.transform.localScale;
            Vector3 scale = bar.transform.localScale;
            scale.y = height * backgroundScale.y;
            bar.transform.localScale = scale;
        }
    }

    private void OnValidate()
    {
        startHeight = Mathf.Clamp01(startHeight);
        Height = startHeight;
        bar.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        bar.color = color;
        Height = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
