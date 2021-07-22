using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager_LookingAtTarget : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _raycastLength;
    [SerializeField,Tag]
    private string _playerTag;
    [SerializeField,Tag]
    private string _generatorTag;
    [Header("References")]
    [SerializeField]
    private Transform _raycastOrigin;

    //other fields
    RaycastHit _hit;
    Vector3 _hitPos;

    //DEBUG=================================================================================================================================================================
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_raycastOrigin.position, _raycastOrigin.forward*_raycastLength);
        Gizmos.DrawSphere(_hitPos, 0.01f);
    }
    //Public Methods========================================================================================================================================================
    public float RequestReward()
    {

        float reward = 0f;
        if(Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward, out _hit, _raycastLength))
        {
            _hitPos = _hit.transform.position;
            if (_hit.transform.tag == _playerTag || _hit.transform.tag == _generatorTag)
            {
                reward += 0.01f;
            }
        }
        return reward;
    }
}
