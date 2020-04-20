using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public SpriteRenderer sprite;

    public void SetColor(Color color)
    {
        sprite.color = color;
    }
}
