using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [Header("CurriculumStages")]
    [Header("MonsterSpawn")]
    [SerializeField]
    private float _outerMin;
    [SerializeField]
    private float _outerMax;
    [SerializeField]
    private float _innerMin;
    [SerializeField]
    private float _innerMax;
    [Header("Stage1")]
    [SerializeField]
    private Transform _playerIdlePosition;
    //Stage 2
    [SerializeField]
    private GameObject _innerWalls;
    //Stage 3
    [SerializeField]
    private List<GameObject> _obstacleGroups = new List<GameObject>();
    [Header("Stage4")]
    [SerializeField]
    private List<Transform> _playerSpawnPositions = new List<Transform>();


    [Header("REFERENCES")]
    [Header("GeneratorReferences")]
    [SerializeField]
    private LivingEntityStatusManager _generatorHealth;
    [Header("PlayerReferences")]
    [SerializeField]
    private LivingEntityStatusManager _playerHealth;
    [SerializeField]
    private FirstPersonController3D _playerMovement;
    [SerializeField]
    private WeaponHolder _playerWeapon;
    //[SerializeField]
    //private HumanAIController _humanAI;
    [Header("MonsterReferences")]
    [SerializeField]
    private LivingEntityStatusManager _monsterHealth;
    [SerializeField]
    private FirstPersonController3D _monsterMovement;
    [SerializeField]
    private WeaponHolder _monsterWeapon;
    //[SerializeField]
    //private MonsterAIController _monsterAI;

    //other fields
    int _defaultDifficulty = 5;

    //Debug=================================================================================================================================================================
    private void OnDrawGizmos()
    {
        //Onter Perimeter
        Vector3 topLeftCorner = new Vector3(_outerMin, 0, _outerMin);
        Vector3 topRightCorner = new Vector3(_outerMin, 0, _outerMax);
        Vector3 bottomLeftCorner = new Vector3(_outerMax, 0, _outerMin);
        Vector3 bottomRightCorner = new Vector3(_outerMax, 0, _outerMax);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(topLeftCorner, topRightCorner);
        Gizmos.DrawLine(bottomLeftCorner, bottomRightCorner);
        Gizmos.DrawLine(topLeftCorner, bottomLeftCorner);
        Gizmos.DrawLine(topRightCorner, bottomRightCorner);

        topLeftCorner = new Vector3(_innerMin, 0, _innerMin);
        topRightCorner = new Vector3(_innerMin, 0, _innerMax);
        bottomLeftCorner = new Vector3(_innerMax, 0, _innerMin);
        bottomRightCorner = new Vector3(_innerMax, 0, _innerMax);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(topLeftCorner, topRightCorner);
        Gizmos.DrawLine(bottomLeftCorner, bottomRightCorner);
        Gizmos.DrawLine(topLeftCorner, bottomLeftCorner);
        Gizmos.DrawLine(topRightCorner, bottomRightCorner);

    }

    //Public Methods========================================================================================================================================================

    //PublicControlMethods--------------------------------------------------------------------------------------------------------------------------------------------------

    public void ResetScene()
    {
        int curriculumValue = Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("difficulty", _defaultDifficulty));
        Debug.Log($"lesson Value {curriculumValue}");
        switch (curriculumValue)
        {
            case 0:
                LoadDifficulty6();
                break;
            case 1:
                LoadDifficulty6();
                break;
            case 2:
                LoadDifficulty6();
                break;
            case 3:
                LoadDifficulty6();
                break;
            case 4:
                LoadDifficulty6();
                break;
            case 5:
                LoadDifficulty6();
                break;
            default:
                Debug.Log("Default case");
                break;
        }
    }
    //Update Methods========================================================================================================================================================

    private void Update()
    {
    }

    //Public Curriclum methods==============================================================================================================================================
    public void LoadDifficulty1() 
    {
        BaseReload();
        Vector3 playerPos = _playerIdlePosition.position;
        SetPlayerPos(new Vector3(playerPos.x, 0f, playerPos.z));
        DisableAllObstacles();
        DisableInnerWall();
    }
    public void LoadDifficulty2()
    {
        BaseReload();
        Vector3 playerPos = _playerIdlePosition.position;
        SetPlayerPos(new Vector3(playerPos.x, 0f, playerPos.z));
        DisableAllObstacles();
        EnableInnerWall();
    }
    public void LoadDifficulty3()
    {
        BaseReload();
        Vector3 playerPos = _playerIdlePosition.position;
        SetPlayerPos(new Vector3(playerPos.x, 0f, playerPos.z));
        DisableAllObstacles();
        EnableObstacles(Random.Range(1,4));
        EnableInnerWall();
    }
    public void LoadDifficulty4()//Add TurretAI
    {
        BaseReload();
        Vector3 playerPos = _playerIdlePosition.position;
        SetPlayerPos(new Vector3(playerPos.x, 0f, playerPos.z));
        EnableAllObstacles();
        EnableInnerWall();
    }
    public void LoadDifficulty5() //Add leash AI
    {
        BaseReload();
        Vector3 playerPos = _playerIdlePosition.position;
        SetPlayerPos(new Vector3(playerPos.x, 0f, playerPos.z));
        EnableAllObstacles();
        EnableInnerWall();
    }
    public void LoadDifficulty6() //Add freeAI
    {
        BaseReload();
        int randomPos = Random.Range(0,(_playerSpawnPositions.Count - 1) );
        Vector3 playerPos = _playerSpawnPositions[randomPos].position;
        SetPlayerPos(new Vector3(playerPos.x, 0f, playerPos.z));
        DisableAllObstacles();
        EnableAllObstacles();
        EnableInnerWall();
        //_monsterAI.ResetPlayer();
        //_humanAI.ResetPlayer();
    }

    private void BaseReload()
    {
        ResetHealth();
        ResetAbilities();
        SetRandomMonsterPosition();
    }

    //Inner Logic ==========================================================================================================================================================
    //Relocation------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void SetRandomMonsterPosition()
    {
        Vector2 newPos = RandomizePositionFromRange(_outerMin, _innerMin, _innerMax, _outerMax);
        SetMonsterPos(new Vector3(newPos.x, 0.7f, newPos.y));
    }


    private void SetMonsterPos(Vector3 position)
    {
        _monsterMovement.SetPosition(position);
    }
    private void SetPlayerPos(Vector3 position)
    {
        _playerMovement.SetPosition(position);
    }

    

    private Vector2 RandomizePositionFromRange(float min1,float max1, float min2, float max2)
    {
        Vector2 spawnPosition = new Vector2();
        if(Random.value >0.5f)
        {
            spawnPosition.x = Random.value > 0.5f ?
            Random.Range(min1, max1) :
            Random.Range(min2, max2);
            spawnPosition.y = Random.Range(min1, max2);
        }
        else
        {
            spawnPosition.y = Random.value > 0.5f ?
            Random.Range(min1, max1) :
            Random.Range(min2, max2);
            spawnPosition.x = Random.Range(min1, max2);
        }
        

        return spawnPosition;
    }

    //HealthReset
    private void ResetHealth()
    {
        ResetMonsterHealth();
        ResetPlayerHealth();
        ResetGeneratorHealth();
    }
    private void ResetMonsterHealth()
    {
        _monsterHealth.ResetMaxHealth();
    }
    private void ResetPlayerHealth()
    {
        _playerHealth.ResetMaxHealth();
    }
    private void ResetGeneratorHealth()
    {
        _generatorHealth.ResetMaxHealth();
    }

    //WeaponReset
    private void ResetAbilities()
    {
        ResetMonsterAbilities();
        ResetPlayerAbilities();
    }
    private void ResetMonsterAbilities()
    {
        _monsterWeapon.ResetAllAbilities();
    }
    private void ResetPlayerAbilities()
    {
        _playerWeapon.ResetAllAbilities();
    }

    //ArenaControls
    private void EnableInnerWall()
    {
        _innerWalls.SetActive(true);
    }
    private void DisableInnerWall()
    {
        _innerWalls.SetActive(false);
    }


    private void EnableObstacles(int amount)
    {
        List<GameObject> obstaclesCopy = new List<GameObject>(_obstacleGroups);
        if (amount > obstaclesCopy.Count) amount = obstaclesCopy.Count;
        for (int i = 0; i < amount; i++)
        {
            int randomNR = Random.Range(0, obstaclesCopy.Count);
            obstaclesCopy[randomNR].SetActive(true);
            obstaclesCopy.RemoveAt(randomNR);
        }
        Debug.Log($"created {amount} obstacles");
    }

    private void EnableAllObstacles()
    {
        foreach (GameObject obstacleGroup in _obstacleGroups)
        {
            obstacleGroup.SetActive(true);
        }
    }

    private void DisableAllObstacles()
    {
        foreach (GameObject obstacleGroup in _obstacleGroups)
        {
            obstacleGroup.SetActive(false);
        }
    }

}
