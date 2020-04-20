using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleState
{
    WORKING, READY, BROKEN
}

public class MainSwitch : MonoBehaviour
{
    public ModuleState state;

    public ModuleState State
    {
        get { return state; }
        set
        {
            if (state != value)
            {
                if (state == ModuleState.WORKING) { Toggle(); }
                else if (value == ModuleState.WORKING) { Toggle(); }
            }
            state = value;
            switch (state)
            {
                case ModuleState.WORKING:
                    indicator.color = operatingColor;
                    break;
                case ModuleState.READY:
                    indicator.color = readyColor;
                    break;
                case ModuleState.BROKEN:
                    indicator.color = brokenColor;
                    break;
            }
        }
    }

    public Color operatingColor;
    public Color readyColor;
    public Color brokenColor;

    public SpriteRenderer switchSprite;
    public SpriteRenderer indicator;
    public Module controllingModule;

    // Start is called before the first frame update
    void Start()
    {
        State = ModuleState.WORKING;
    }

    // Update is called once per frame
    void Update()
    {
        bool isOperational = controllingModule.IsOperational();
        switch (State)
        {
            case ModuleState.WORKING:
                if (!isOperational)
                {
                    // Signal break
                    State = ModuleState.BROKEN;
                }
                break;
            case ModuleState.READY:
                if(!isOperational)
                {
                    State = ModuleState.BROKEN;
                }
                break;
            case ModuleState.BROKEN:
                if (isOperational)
                {
                    // Fixed
                    State = ModuleState.READY;
                }
                break;
        }

        if (ActivityCheck.IsClicked(switchSprite.bounds, 0, Rotate.Side.FRONT))
        {
            if (State == ModuleState.READY)
            {
                State = ModuleState.WORKING;
            }
        }
    }

    void Toggle()
    {
        Vector3 scale = switchSprite.transform.localScale;
        scale.y = -scale.y;
        switchSprite.transform.localScale = scale;
        AudioManager.Instance.Play("toggle");
    }
}
