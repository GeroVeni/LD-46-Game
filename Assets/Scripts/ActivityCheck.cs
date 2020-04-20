using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityCheck : MonoBehaviour
{
    static Camera mainCamera;
    static Rotate engine;

    public static Rotate GetEngine ()
    {
        // Get engine to keep track of active side
        if (engine == null) { engine = GameObject.Find("Engine").GetComponent<Rotate>(); }
        return engine;
    }

    public static Camera GetMainCamera()
    {
        // Get main camera
        if (mainCamera == null) { mainCamera = Camera.main; }
        return mainCamera;
    }

    public static bool IsClicked(Bounds bounds, int mouseButtonCode, Rotate.Side side)
    {
        if (!Input.GetMouseButtonDown(mouseButtonCode)) { return false; }
        if (!IsActive(side)) { return false; }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 9.4f;
        mousePos = GetMainCamera().ScreenToWorldPoint(mousePos);
        return bounds.Contains(mousePos);
    }

    public static bool IsPressed(Bounds bounds, int mouseButtonCode, Rotate.Side side)
    {
        if (!Input.GetMouseButton(mouseButtonCode)) { return false; }
        if (!IsActive(side)) { return false; }

        Vector3 mousePos = GetMousePositionWorld();
        return bounds.Contains(mousePos);
    }

    public static Vector3 GetMousePositionWorld()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 9.4f;
        return GetMainCamera().ScreenToWorldPoint(mousePos);
    }

    public static bool IsActive(Rotate.Side side)
    {
        return side == GetEngine().currentSide && !GetEngine().isRotating;
    }
}
