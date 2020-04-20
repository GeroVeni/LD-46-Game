using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rotate : MonoBehaviour
{
    public enum Side
    {
        FRONT, RIGHT, BACK, LEFT, NONE
    }

    public float lambda = 10;
    public float angleThreshold = 0.3f;
    public Side editorSide;
    
    [HideInInspector]
    public Side currentSide;
    [HideInInspector]
    public bool isRotating;

    Quaternion targetRotation;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        isRotating = false;
        currentSide = editorSide;
        targetRotation = transform.rotation = Quaternion.Euler(0, GetSideAngles(currentSide), 0);
    }

    // Update is called once per frame
    void Update()
    {
        Side newSide = Side.NONE;
        if (Input.GetKeyDown(KeyCode.S)) { newSide = Side.FRONT; }
        if (Input.GetKeyDown(KeyCode.D)) { newSide = Side.RIGHT; }
        if (Input.GetKeyDown(KeyCode.W)) { newSide = Side.BACK; }
        if (Input.GetKeyDown(KeyCode.A)) { newSide = Side.LEFT; }

        if (newSide != Side.NONE && newSide != currentSide)
        {
            currentSide = newSide;
            targetRotation = Quaternion.Euler(0, GetSideAngles(currentSide), 0);
            isRotating = true;

            timer = Time.time;
        }

        if (isRotating)
        {
            Quaternion currentRotation = transform.rotation;
            float angle = Quaternion.Angle(currentRotation, targetRotation);
            if (angle >= angleThreshold)
            {
                transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, lambda * angle * Time.deltaTime);
            }
            else
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    private void OnValidate()
    {
        transform.rotation = Quaternion.Euler(0, GetSideAngles(editorSide), 0);
    }

    float GetSideAngles(Side side)
    {
        switch (side)
        {
            case Side.FRONT:
                return 0;
            case Side.RIGHT:
                return 90;
            case Side.BACK:
                return 180;
            case Side.LEFT:
                return 270;
            default:
                return 0;
        }
    }
}
