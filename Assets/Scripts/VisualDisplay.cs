using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDisplay : MonoBehaviour
{
    private Camera MainCam;
    [HideInInspector]
    private bool is3D;

    void Start()
    {
        MainCam = GetComponent<Camera>();
        MainCam.cullingMask = 1 | 1 << 8;
        is3D = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (is3D)
            {
                MainCam.cullingMask = 1 | 1 << 9;
                is3D = false;
            }
            else
            {
                MainCam.cullingMask = 1 | 1 << 8;
                is3D = true;
            }
        }
    }
}
