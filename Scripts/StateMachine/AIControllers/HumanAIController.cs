using StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAIController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _generatorPatrolDistance = 10f;
    [SerializeField]
    private float _monsterPatrolDistance = 4f;
    [SerializeField]
    private float _checkpointReachDistance = 0.2f;
    [SerializeField]
    private float _distanceToChaseMonster = 7f;
    [SerializeField, Range(0f, 1f)]
    private float _healthPercentageToRetreat = 0.5f;
    [SerializeField]
    private float _healZoneWaitTime = 5f;

    [SerializeField]
    private Transform _humanTransform;
    [SerializeField]
    private Transform _monsterTransform;
    [SerializeField]
    private Transform _generatorTransform;
    [SerializeField]
    private FirstPersonController3D _movementManager;
    [SerializeField]
    private LivingEntityStatusManager _healthManager;
    [SerializeField]
    private WeaponHolder _weaponManager;
    [SerializeField]
    private Transform _leftHeadChecker;
    [SerializeField]
    private Transform _rightHeadChecker;
    [SerializeField]
    private Transform _respawnPosition;


    //Other fields
    private StateMachine<HumanAIController> _stateMachine;

    //Public Methods========================================================================================================================================================
    //Public Getters--------------------------------------------------------------------------------------------------------------------------------------------------------\
    public Transform GetPlayerTransform() => _humanTransform;
    public Vector3 GetPlayerPos() => _humanTransform.position;
    public Vector3 GetMonsterPos() => _monsterTransform.position;
    public Vector3 GetGeneratorPos() => _generatorTransform.position;

    public FirstPersonController3D GetMovementManager() => _movementManager;
    public LivingEntityStatusManager GetHealthManager() => _healthManager;
    public WeaponHolder GetWeaponManager() => _weaponManager;


    public Transform GetLeftHeadChecker() => _leftHeadChecker;
    public Transform GetRightHeadChecker() => _rightHeadChecker;

    public float GetGeneratorPatrolDistance() => _generatorPatrolDistance;
    public float GetMonsterPatrolDistance() => _monsterPatrolDistance;
    public float GetCheckPointReachDistance() => _checkpointReachDistance;
    public float GetDistanceToChaseMonster() => _distanceToChaseMonster;
    public float GetHealthPercentageToRetreat() => _healthPercentageToRetreat;
    public float GetHealZoneWaitTime() => _healZoneWaitTime;

    /*public void ResetPlayer()
    {
        _humanTransform.position = _respawnPosition.position;
        _stateMachine.ChangeState(State_Human_PatrolGenerator.Instance);
    }
    */
    //Public Methods---------------------------------------------------
    public void RequestStateTransition(State<HumanAIController> newState)
    {
        _stateMachine.ChangeState(newState);
    }
    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new StateMachine<HumanAIController>(this);
        _stateMachine.ChangeState(State_Human_PatrolGenerator.Instance);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _stateMachine.Update();
    }

}
