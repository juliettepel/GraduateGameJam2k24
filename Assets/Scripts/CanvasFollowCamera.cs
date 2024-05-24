using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollowCamera : MonoBehaviour
{
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    void LateUpdate()
    {
        if (transform != null)
        {
            transform.LookAt(transform.position + _camera.transform.forward);
        }
    }
}
