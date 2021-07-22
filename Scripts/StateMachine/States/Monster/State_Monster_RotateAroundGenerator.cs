using StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Monster_RotateAroundGenerator : State<MonsterAIController>
{
    NavMeshPath _path = new NavMeshPath();
    MonsterAIController _monsterAI;
    FirstPersonController3D _movementManager;
    LivingEntityStatusManager _healthManager;
    WeaponHolder _weaponManager;
    MonsterAgent _monsterAgent;

    bool _reachedGoal = false;
    int _currentCheckpoint = 0;
    bool _blockState = false;
    float _recalculationDelay = 1f;
    float _recalculationVar = 0f;

    public override void EnterState(MonsterAIController _owner)
    {
        Debug.Log("Entering Monster moving to generator State");
        _reachedGoal = true;
        _monsterAI = _owner;
        _movementManager = _monsterAI.GetMovementManager();
        _healthManager = _monsterAI.GetHealthManager();
        _blockState = false;
        _weaponManager = _monsterAI.GetWeaponManager();
        _monsterAgent = _monsterAI.GetMonsterAgent();
        _recalculationVar = _recalculationDelay;
    }

    public override void ExitState(MonsterAIController _owner)
    {

    }

    public override void UpdateState(MonsterAIController _owner)
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
            if (_recalculationVar > 0f)
                _recalculationVar -= Time.deltaTime;
            else
            {
                CalculateNewPath();
                _recalculationVar = _recalculationDelay;
            }
            ProceedToGoal();
        }
    }

    private void CalculateNewPath()
    {
        Debug.Log("calculating new path");
        float patrolDistance = _monsterAI.GetGeneratorPatrolDistance();
        Vector3 positionNearGenerator = _monsterAI.GetGeneratorPos() + new Vector3(Random.Range(-patrolDistance, patrolDistance), 0f, Random.Range(-patrolDistance, patrolDistance));
        Debug.DrawLine(positionNearGenerator, _monsterAI.GetGeneratorPos(), Color.yellow, 10f);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(positionNearGenerator, out hit, 10f, NavMesh.AllAreas))
        {
            NavMesh.CalculatePath(_monsterAI.GetMonsterPos(), hit.position, NavMesh.AllAreas, _path);
            for (int i = 0; i < _path.corners.Length - 1; i++)
                Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red, 100f);
            Debug.LogWarning("Line drawn");
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

        Vector3 movementDirection = _path.corners[_currentCheckpoint] - _monsterAI.GetMonsterPos();
        movementDirection.Normalize();
        //movementDirection = Quaternion.AngleAxis(_humanAI.GetPlayerTransform().eulerAngles.y, Vector3.up) * movementDirection;
        _monsterAgent.MoveMonster(new Vector2(movementDirection.x, movementDirection.z));

        //_movementManager.MovePlayerAbsoluteOnInput(new Vector2(movementDirection.x, movementDirection.z));

        if (Vector3.Distance(_path.corners[_currentCheckpoint], _monsterAI.GetMonsterPos()) < _monsterAI.GetCheckPointReachDistance())
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
        /*RaycastHit[] mutihit = Physics.RaycastAll(_monsterAI.GetMovementManager().GetCameraHolder().position,
            _monsterAI.GetPlayerPos() + Vector3.up, _monsterAI.GetGeneratorPatrolDistance());

        System.Array.Sort(mutihit, (x, y) => x.distance.CompareTo(y.distance));
        if (mutihit.Length > 0)
        {
            foreach (RaycastHit raycastHit in mutihit)
            {
                if (raycastHit.collider != _monsterAI.GetWeaponManager().GetOwnerCollider() && raycastHit.transform.tag == "Generator")
                {
                    targetVisible = true;
                    Debug.Log("Generator visible");
                    break;
                }
            }
        }*/
        if(Vector3.Distance(_monsterAI.GetMonsterPos(), _monsterAI.GetGeneratorPos()) <= 10f)
        {
            targetVisible = true;
        }

        if (CalculateWeaponReadyness() > 0 && targetVisible)
        {
            _monsterAI.RequestStateTransition(State_Monster_AttackGenerator.Instance);
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


    private static State_Monster_RotateAroundGenerator _instance;

    private State_Monster_RotateAroundGenerator()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static State_Monster_RotateAroundGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                new State_Monster_RotateAroundGenerator();
            }

            return _instance;
        }
    }
}
