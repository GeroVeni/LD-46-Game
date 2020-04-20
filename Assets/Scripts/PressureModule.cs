using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureModule : Module
{
    public float sensitivity;
    public float margin;

    public RotatorSwitch[] switches = new RotatorSwitch[3];
    public IndicatorBar[] bars = new IndicatorBar[3];

    Vector3 weights0 = new Vector3(+1, +1, +1);
    Vector3 weights1 = new Vector3(-1, -1, +1);
    Vector3 weights2 = new Vector3(+1, -1, -1);

    float[] offsets;
    bool isOperational;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        ResetModule();
    }

    public override void ResetModule()
    {
        offsets = new float[] { 0.5f, 0.5f, 0.5f };
        for (int i = 0; i < switches.Length; ++i) { switches[i].Value = 0; }
        isOperational = true;
        PlaySound(isOperational);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        float[] barHeights = GetBarHeights();
        bool status = CheckHeights(barHeights);
        for (int i = 0; i < bars.Length; ++i) { bars[i].Height = barHeights[i]; }
        if (status != isOperational)
        {
            isOperational = status;
            PlaySound(isOperational);
        }
    }

    void PlaySound(bool isOperational)
    {
        if (isOperational)
        {
            AudioManager.Instance.Play("pressure_module_working");
            AudioManager.Instance.Stop("pressure_module_broken");
        } else
        {
            AudioManager.Instance.Stop("pressure_module_working");
            AudioManager.Instance.Play("pressure_module_broken");
        }
    }

    public override bool IsOperational()
    {
        return CheckHeights(GetBarHeights());
    }

    bool CheckHeights(float[] heights)
    {
        for (int i = 0; i < offsets.Length; ++i)
        {
            if (Mathf.Abs(heights[i] - 0.5f) > margin) { return false; }
        }
        return true;
    }

    public override void Break()
    {
        float[] barHeights = GetBarHeights();
        for (int i = 0; i < offsets.Length; ++i)
        {
            offsets[i] += Random.Range(0f, 1f) - barHeights[i];
        }
    }

    float[] GetBarHeights()
    {
        Vector3 switchValues = new Vector3(
            switches[0].Value,
            switches[1].Value,
            switches[2].Value
        );

        float[] res = new float[3];
        res[0] = sensitivity * Vector3.Dot(weights0, switchValues) + offsets[0];
        res[1] = sensitivity * Vector3.Dot(weights1, switchValues) + offsets[1];
        res[2] = sensitivity * Vector3.Dot(weights2, switchValues) + offsets[2];
        return res;
    }
}
