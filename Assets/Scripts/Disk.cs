using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour
{
    public delegate void DiskSpinHandler(float spinAmount);
    public event DiskSpinHandler spinEvent;

    public SpriteRenderer diskSprite;
    public float spinsToFull;

    bool spinning;
    Vector3 spinCenter;
    Vector3 prevMousePos;

    // Start is called before the first frame update
    void Start()
    {
        spinning = false;
        spinCenter = diskSprite.bounds.center;
    }

    // Update is called once per frame
    void Update()
    {
        if (ActivityCheck.IsClicked(diskSprite.bounds, 0, Rotate.Side.FRONT))
        {
            spinning = true;
            prevMousePos = ActivityCheck.GetMousePositionWorld() - spinCenter;
            //prevMousePos.z = 0;
        } else if (Input.GetMouseButtonUp(0))
        {
            spinning = false;
        }

        if (spinning)
        {
            Vector3 mousePos = ActivityCheck.GetMousePositionWorld() - spinCenter;
            //mousePos.z = 0;
            float angle = Vector3.SignedAngle(prevMousePos, mousePos, diskSprite.transform.forward);
            spinEvent(angle / (spinsToFull * 360f));
            transform.rotation *= Quaternion.AngleAxis(angle, diskSprite.transform.forward);
            prevMousePos = mousePos;
        }
    }
}
