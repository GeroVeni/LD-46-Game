using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorSwitch : MonoBehaviour
{
    public float value;

    public SpriteRenderer sr;
    public float speed;
    public float valueToRotation = 90;

    public float Value
    {
        get { return value; }
        set
        {
            this.value = value;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z = value * valueToRotation;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int mouseDown = -1;
        if (Input.GetMouseButton(0)) { mouseDown = 0; }
        else if (Input.GetMouseButton(1)) { mouseDown = 1; }
        if (mouseDown >= 0)
        {
            float sign = mouseDown == 0 ? +1 : -1;
            if (ActivityCheck.IsPressed(sr.bounds, mouseDown, Rotate.Side.LEFT))
            {
                Value += sign * speed * Time.deltaTime;
            }
        }
    }
}
