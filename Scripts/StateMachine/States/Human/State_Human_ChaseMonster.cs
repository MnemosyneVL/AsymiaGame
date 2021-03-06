using StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Human_ChaseMonster : State<HumanAIController>
{
    //other fields
    NavMeshPath _path = new NavMeshPath();
    HumanAIController _humanAI;
    FirstPersonController3D _movementManager;
    LivingEntityStatusManager _healthManager;


    bool _reachedGoal = false;
    int _currentCheckpoint = 0;
    bool _blockState = false;

    private static State_Human_ChaseMonster _instance;

    private State_Human_ChaseMonster()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static State_Human_ChaseMonster Instance
    {
        get
        {
            if (_instance == null)
            {
                new State_Human_ChaseMonster();
            }

            return _instance;
        }
    }

    public override void EnterState(HumanAIController _owner)
    {
        Debug.Log("Entering Monster Chase State");
        _humanAI = _owner;
        _healthManager = _humanAI.GetHealthManager();
        _movementManager = _humanAI.GetMovementManager();
        _blockState = false;
    }

    public override void ExitState(HumanAIController _owner)
    {

    }

    public override void UpdateState(HumanAIController _owner)
    {
        //check transitions
        CheckTransitions();
        if (_blockState) return;
        //perform state tick update
        if (_reachedGoal)
        {
            CalculateNewPath();
        }
        else
        {
            ProceedToGoal();
        }
    }

    private void CalculateNewPath()
    {
        float patrolDistance = _humanAI.GetMonsterPatrolDistance();
        Vector3 positionNearMonster = _humanAI.GetMonsterPos() + new Vector3(Random.Range(-patrolDistance, patrolDistance), 0f, Random.Range(-patrolDistance, patrolDistance));
        Debug.DrawLine(positionNearMonster, _humanAI.GetMonsterPos(), Color.yellow, 10f);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(positionNearMonster, out hit, 50f, NavMesh.AllAreas))
        {
            NavMesh.CalculatePath(_humanAI.GetPlayerPos(),hit.position, NavMesh.AllAreas ,_path);
            for (int i = 0; i < _path.corners.Length - 1; i++)
                Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red, 100f);
            _reachedGoal = false;
        }
        else
        {
            return;
        }

        _currentCheckpoint = 0;
    }



    private void ProceedToGoal()
    {
        if (_path.corners.Length <= 0)
        {
            CalculateNewPath();
            return;
        }

        Vector3 movementDirection = _path.corners[_currentCheckpoint] - _humanAI.GetPlayerPos();
        movementDirection.Normalize();
        //movementDirection = Quaternion.AngleAxis(_humanAI.GetPlayerTransform().eulerAngles.y, Vector3.up) * movementDirection;

        _movementManager.MovePlayerAbsoluteOnInput(new Vector2(movementDirection.x, movementDirection.z));

        if (Vector3.Distance(_path.corners[_currentCheckpoint], _humanAI.GetPlayerPos()) < _humanAI.GetCheckPointReachDistance())
        {
            SetNewCheckpoint();
        }

    }

    private void SetNewCheckpoint()
    {
        _currentCheckpoint += 1;
        if (_currentCheckpoint > _path.corners.Length - 1)
        {
            _reachedGoal = true;
        }
    }

    //State transitions ================================================
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
                    Debug.Log("Monster visible");
                    break;
                }
            }
        }
        
        if(_humanAI.GetWeaponManager().GetEquippedWeapon().CanUse() && targetVisible) 
        {
            _humanAI.RequestStateTransition(State_Human_AttackState.Instance);
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
