using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyModule : Module
{
    public Disk disk;
    public Battery battery;

    public float drainSpeed;

    public override void ResetModule()
    {
        battery.Level = 1;
        disk.transform.localRotation = Quaternion.identity;
    }

    public override void Break()
    {
        battery.Level = 0;
        AudioManager.Instance.Play("poweroff");
    }

    public override bool IsOperational()
    {
        return battery.Level > 0;
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        disk.spinEvent += OnDiskSpin;
    }

    private void OnDiskSpin(float spinAmount)
    {
        battery.Level += spinAmount;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        bool prevStatus = battery.Level > 0;
        battery.Level -= drainSpeed * Time.deltaTime;
        if (prevStatus && battery.Level <= 0)
        {
            AudioManager.Instance.Play("poweroff");
        }
    }
}
