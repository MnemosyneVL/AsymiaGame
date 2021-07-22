using StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Human_HealState : State<HumanAIController>
{
    //other fields
    HumanAIController _humanAI;
    private WeaponHolder _weaponManager;

    bool _healingDone = false;
    float _waitTimer = 0f;
    bool _blockState = false;

    private static State_Human_HealState _instance;

    private State_Human_HealState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static State_Human_HealState Instance
    {
        get
        {
            if (_instance == null)
            {
                new State_Human_HealState();
            }

            return _instance;
        }
    }

    public override void EnterState(HumanAIController _owner)
    {
        Debug.Log("Entring Human Heal State");
        _humanAI = _owner;
        _healingDone = false;
        _weaponManager = _humanAI.GetWeaponManager();
        _weaponManager.ActivateAbility(0);
        _waitTimer = _humanAI.GetHealZoneWaitTime();
        _blockState = false;
    }

    public override void ExitState(HumanAIController _owner)
    {
        
    }

    public override void UpdateState(HumanAIController _owner)
    {
        CheckTransitions();
        if (_blockState) return;
        if (!_healingDone)
        {
            if(_waitTimer <= 0f)
            {
                _healingDone = true;
                return;
            }
            _waitTimer -= Time.deltaTime;
        }
    }

    private void CheckTransitions()
    {
        if (Vector3.Distance(_humanAI.GetPlayerPos(), _humanAI.GetMonsterPos()) < _humanAI.GetDistanceToChaseMonster())
        {
            _humanAI.RequestStateTransition(State_Human_ChaseMonster.Instance);
            _blockState = true;
        }

        if (_healingDone)
        {
            _humanAI.RequestStateTransition(State_Human_PatrolGenerator.Instance);
            _blockState = true;
        }
    }

}
