using StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Monster_AttackGenerator : State<MonsterAIController>
{
    MonsterAIController _monsterAI;
    LivingEntityStatusManager _healthManager;
    WeaponHolder _weaponManager;
    MonsterAgent _monsterAgent;
    Transform _leftHeadChecker;
    Transform _rigthHeadChecker;
    bool _blockState = false;
    float headDot = 0f;

    public override void EnterState(MonsterAIController _owner)
    {
        Debug.Log("Entering Monster attack mode State");
        _monsterAI = _owner;
        _healthManager = _monsterAI.GetHealthManager();
        _blockState = false;
        _weaponManager = _monsterAI.GetWeaponManager();
        _monsterAgent = _monsterAI.GetMonsterAgent();
        _leftHeadChecker = _monsterAI.GetLeftHeadChecker();
        _rigthHeadChecker = _monsterAI.GetRightHeadChecker();

    }

    public override void ExitState(MonsterAIController _owner)
    {

    }

    public override void UpdateState(MonsterAIController _owner)
    {

        CheckTransitions();
        if (_blockState) return;
        RotateHead();
        ShootWeapon();
    }

    private void RotateHead()
    {
        Vector3 directionFromHead = ((_monsterAI.GetGeneratorPos() + Vector3.up*2) - _monsterAI.GetMovementManager().GetCameraHolder().position).normalized;
        headDot = Vector3.Dot(directionFromHead, _monsterAI.GetMovementManager().GetCameraHolder().forward);
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
            _monsterAgent.RotateMonster(mouseMovementAmount);
        }
    }

    private bool ShouldRotateRight()
    {
        Vector3 directionFromLeftChecker = ((_monsterAI.GetGeneratorPos() + Vector3.up*2) - _leftHeadChecker.position).normalized;
        Vector3 directionFromRightChecker = ((_monsterAI.GetGeneratorPos() + Vector3.up*2) - _rigthHeadChecker.position).normalized;
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
            _monsterAgent.UseAbility1();
            _monsterAgent.UseAbility2();
            _monsterAgent.UseAbility3();
        }
    }

    private void CheckTransitions()
    {
        /*bool targetVisible = false;
        RaycastHit[] mutihit = Physics.RaycastAll(_monsterAI.GetMovementManager().GetCameraHolder().position,
            _monsterAI.GetGeneratorPos() + Vector3.up * 2f, _monsterAI.GetGeneratorPatrolDistance());

        System.Array.Sort(mutihit, (x, y) => x.distance.CompareTo(y.distance));
        if (mutihit.Length > 0)
        {
            foreach (RaycastHit raycastHit in mutihit)
            {
                if (raycastHit.collider != _monsterAI.GetWeaponManager().GetOwnerCollider() && raycastHit.transform.tag == "Generator")
                {
                    targetVisible = true;
                    break;
                }
            }
        }
        if (!targetVisible)
        {
            Debug.LogError("target invisible");
            _monsterAI.RequestStateTransition(State_Monster_RotateAroundGenerator.Instance);
            _blockState = true;
        }*/
        if (CalculateWeaponReadyness() == 0 )
        { 
            _monsterAI.RequestStateTransition(State_Monster_RotateAroundGenerator.Instance);
            _blockState = true;
        }

        float healthPercentage = _healthManager.GetHealth() / _healthManager.GetMaxHealth();
        if (healthPercentage <= _monsterAI.GetHealthPercentageToRetreat())
        {
            _monsterAI.RequestStateTransition(State_Monster_RetreatState.Instance);
            _blockState = true;
        }
    }
    private int CalculateWeaponReadyness()
    {
        int readyness = 0;
        if (_weaponManager.GetEquippedAbilitiy(0).CanUse()) readyness++;
        if (_weaponManager.GetEquippedAbilitiy(1).CanUse()) readyness++;
        if (_weaponManager.GetEquippedAbilitiy(2).CanUse()) readyness++;
        return readyness;
    }


    private static State_Monster_AttackGenerator _instance;

    private State_Monster_AttackGenerator()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static State_Monster_AttackGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                new State_Monster_AttackGenerator();
            }

            return _instance;
        }
    }
}
