using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class CameraManager : MonoBehaviour
{
    public List<Transform> PlayerSpawnPoints = new List<Transform>();

    private int _currentPlayerCount;

    public void OnPlayerJoin(PlayerInput input)
    {
        ThirdPersonController playerController = input.GetComponent<ThirdPersonController>();
        playerController._isInitialPositionSet = false;
        playerController.InitialPosition = PlayerSpawnPoints[_currentPlayerCount].position;

        _currentPlayerCount++;
        if (_currentPlayerCount == PlayerSpawnPoints.Count)
        {
            _currentPlayerCount = 0;
        }

        GetComponent<CinemachineTargetGroup>().AddMember(input.transform, 1, 0);
    }
}
