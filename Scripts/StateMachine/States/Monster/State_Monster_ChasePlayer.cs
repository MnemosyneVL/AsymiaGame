using StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Monster_ChasePlayer : State<MonsterAIController>
{

    public override void EnterState(MonsterAIController _owner)
    {

    }

    public override void ExitState(MonsterAIController _owner)
    {

    }

    public override void UpdateState(MonsterAIController _owner)
    {

    }


    private static State_Monster_ChasePlayer _instance;

    private State_Monster_ChasePlayer()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static State_Monster_ChasePlayer Instance
    {
        get
        {
            if (_instance == null)
            {
                new State_Monster_ChasePlayer();
            }

            return _instance;
        }
    }
}
