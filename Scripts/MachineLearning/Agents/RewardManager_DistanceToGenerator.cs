using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager_DistanceToGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _marginFromGenerator = 6f;
    [SerializeField]
    private float _minHealDistance = 50f;
    [SerializeField]
    private float _marginFromOutside = 70f;
    [SerializeField]
    private float _rewardMultiplyer = 0.01f;
    [Header("References")]
    [SerializeField]
    private Transform _monsterTransform;
    [SerializeField]
    private Transform _generatorTransform;

    //other fields
    private LivingEntityStatusManager _monsterHealthManager;
    private float _monsterMaxHealth;

    //rewards
    private float _healReward;
    private float _closureReward;



    //DEBUG=================================================================================================================================================================
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_generatorTransform.position, _marginFromGenerator);
        Gizmos.DrawWireSphere(_generatorTransform.position, _minHealDistance - 1f);
        Gizmos.DrawWireSphere(_generatorTransform.position, _marginFromOutside);
    }

    //Initialization Methods================================================================================================================================================
    private void Awake()
    {
        Transform topmostPartent = _monsterTransform.root;
        _monsterHealthManager = topmostPartent.GetComponentInChildren<LivingEntityStatusManager>();
        _monsterMaxHealth = _monsterHealthManager.GetMaxHealth();
    }

    //Public Mathods========================================================================================================================================================
    public float RequestRewardValue()
    {
        return CalculateReward();
    }

    //Inner Logic

    private float CalculateReward()
    {
        float monsterDistanceToGenerator = Vector3.Distance(_monsterTransform.position, _generatorTransform.position);

        //calculate percentage
        _closureReward = 1f - (monsterDistanceToGenerator - _marginFromGenerator) / (_marginFromOutside - _marginFromGenerator);
        //clamp
        _closureReward = Mathf.Clamp(_closureReward, 0f, 1f );
        _closureReward *= _rewardMultiplyer;

        //calculate percantage
        _healReward = (monsterDistanceToGenerator - _marginFromGenerator) / (_minHealDistance - _marginFromGenerator);
        //clamp
        _healReward = Mathf.Clamp(_healReward, 0f, 1f);
        //apply health percentage
        _healReward = _healReward * (1f - (_monsterHealthManager.GetHealth() / _monsterMaxHealth));
        _closureReward *= _rewardMultiplyer;

        float finalReward = _healReward + _closureReward;
        return finalReward;
    }
}
