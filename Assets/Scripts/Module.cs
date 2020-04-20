using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    public abstract void ResetModule();
    public abstract void Break();
    public abstract bool IsOperational();

    Color workingColor = new Color(0, 0.8018868f, 0.1633289f);
    Color brokenColor = new Color(1, 0.07075471f, 0.07075471f);

    Lamp lamp;

    protected void Start()
    {
        lamp = GetComponentInChildren<Lamp>();
    }

    protected void Update()
    {
        if (IsOperational())
        {
            lamp.SetColor(workingColor);
        } else
        {
            lamp.SetColor(brokenColor);
        }
    }
}
