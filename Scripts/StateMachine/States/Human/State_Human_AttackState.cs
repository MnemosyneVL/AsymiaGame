using StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Human_AttackState : State<HumanAIController>
{
    //otherFields
    Transform _leftHeadChecker;
    Transform _rigthHeadChecker;
    HumanAIController _humanAI;
    private WeaponHolder _weaponManager;
    LivingEntityStatusManager _healthManager;
    float headDot = 0f;
    bool _blockState = false;

    private static State_Human_AttackState _instance;

    private State_Human_AttackState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static State_Human_AttackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new State_Human_AttackState();
            }

            return _instance;
        }
    }

    public override void EnterState(HumanAIController _owner)
    {
        Debug.Log("Entring Human Attack State");
        _humanAI = _owner;
        _weaponManager = _humanAI.GetWeaponManager();
        _leftHeadChecker = _humanAI.GetLeftHeadChecker();
        _rigthHeadChecker = _humanAI.GetRightHeadChecker();
        _healthManager = _humanAI.GetHealthManager();
        _blockState = false;
    }

    public override void ExitState(HumanAIController _owner)
    {
        //_weaponManager.DeactivateCurrentWeapon();
    }

    public override void UpdateState(HumanAIController _owner)
    {
        CheckTransitions();
        if (_blockState) return;
        RotateHead();
        ShootWeapon();
    }

    private void RotateHead()
    {
        Vector3 directionFromHead = ((_humanAI.GetMonsterPos() + Vector3.up) - _humanAI.GetMovementManager().GetCameraHolder().position).normalized;
        headDot = Vector3.Dot(directionFromHead, _humanAI.GetMovementManager().GetCameraHolder().forward);
        if (headDot < 0.97f)
        {
            float mouseMovementAmount = 0;
            if (ShouldRotateRight())
            {
                mouseMovementAmount = 1 - headDot;
                Mathf.Clamp(mouseMovementAmount, 0.1f, 1f);
            }
            else
            {
                mouseMovementAmount = -1 + headDot;
                Mathf.Clamp(mouseMovementAmount, -0.1f, -1f);
            }
            _humanAI.GetMovementManager().ChangeLookOnInput(new Vector2(mouseMovementAmount, 0f));
        }
    }

    private bool ShouldRotateRight()
    {
        Vector3 directionFromLeftChecker = ((_humanAI.GetMonsterPos() + Vector3.up) - _leftHeadChecker.position).normalized;
        Vector3 directionFromRightChecker = ((_humanAI.GetMonsterPos() + Vector3.up) - _rigthHeadChecker.position).normalized;
        if (Vector3.Dot(directionFromLeftChecker, _leftHeadChecker.forward) < Vector3.Dot(directionFromRightChecker, _rigthHeadChecker.forward))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShootWeapon()
    {
        if (headDot >= 0.9f)
        {
            _weaponManager.ActivateCurrentWeapon();
        }
        else
        {
            //_weaponManager.DeactivateCurrentWeapon();
        }
    }

    private void CheckTransitions()
    {
        bool targetVisible = false;
        RaycastHit[] mutihit = Physics.RaycastAll(_humanAI.GetMovementManager().GetCameraHolder().position,
            _humanAI.GetMonsterPos() + Vector3.up * 1.5f, _humanAI.GetDistanceToChaseMonster());

        System.Array.Sort(mutihit, (x, y) => x.distance.CompareTo(y.distance));
        if (mutihit.Length > 0)
        {
            foreach (RaycastHit raycastHit in mutihit)
            {
                if (raycastHit.collider != _humanAI.GetWeaponManager().GetOwnerCollider() && raycastHit.transform.tag == "Monster")
                {
                    targetVisible = true;
                    break;
                }
            }
        }
        if(!targetVisible)
        {
            Debug.LogError("target invisible");
            _humanAI.RequestStateTransition(State_Human_ChaseMonster.Instance);
            _blockState = true;
        }
        if (!_humanAI.GetWeaponManager().GetEquippedWeapon().CanUse() )
        {
            _humanAI.RequestStateTransition(State_Human_ChaseMonster.Instance);
            _blockState = true;
        }

        if (Vector3.Distance(_humanAI.GetPlayerPos(), _humanAI.GetMonsterPos()) > _humanAI.GetDistanceToChaseMonster())
        {
            _humanAI.RequestStateTransition(State_Human_PatrolGenerator.Instance);
            _blockState = true;
        }

        float healthPercentage = _healthManager.GetHealth() / _healthManager.GetMaxHealth();
        if (healthPercentage <= _humanAI.GetHealthPercentageToRetreat())
        {
            _humanAI.RequestStateTransition(State_Human_RetreatState.Instance);
            _blockState = true;
        }
    }
}
