using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    public void OnPlayerJoin(PlayerInput input)
    {
        GetComponent<CinemachineTargetGroup>().AddMember(input.transform, 1, 0);
    }
}
