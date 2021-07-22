using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainReferenceHolder : MonoBehaviour
{

    [Header("Player References")]
    [SerializeField]
    private GameObject _playerGO;
    
    [Header("Monster References")]
    [SerializeField]
    private GameObject _monsterGO;

    [Header("Generator References")]
    [SerializeField]
    private GameObject _generatorGO;


    //other fields
    //player
    private WeaponHolder _playerWeaponHolder;
    private FirstPersonController3D _playerMovementController;
    private LivingEntityStatusManager _playerStatusManager;
    private EntityTeamManager _playerTeamManager;
    //monster
    private WeaponHolder _monsterWeaponHolder;
    private FirstPersonController3D _monsterMovementController;
    private LivingEntityStatusManager _monsterStatusManager;
    private EntityTeamManager _monsterTeamManager;
    //generator
    private LivingEntityStatusManager _generatorStatusManager;
    private EntityTeamManager _generatorTeamManager;

    //singleton fields
    private static MainReferenceHolder _instance = null;

    public static MainReferenceHolder Instance
    {
        get
        {
            return _instance;
        }
    }

    //Initialization Methods================================================================================================================================================

    //Unity Awake
    private void Awake()
    {
        InitializeSingleton();
        AutoFindReferences();
    }

    private void AutoFindReferences()
    {
        if (_playerGO != null)
        {
            _playerWeaponHolder = _playerGO.GetComponentInChildren<WeaponHolder>();
            _playerMovementController = _playerGO.GetComponentInChildren<FirstPersonController3D>();
            _playerStatusManager = _playerGO.GetComponentInChildren<LivingEntityStatusManager>();
            _playerTeamManager = _playerGO.GetComponentInChildren<EntityTeamManager>();
        }

        if (_monsterGO != null)
        {
            _monsterWeaponHolder = _monsterGO.GetComponentInChildren<WeaponHolder>();
            _monsterMovementController = _monsterGO.GetComponentInChildren<FirstPersonController3D>();
            _monsterStatusManager = _monsterGO.GetComponentInChildren<LivingEntityStatusManager>();
            _monsterTeamManager = _monsterGO.GetComponentInChildren<EntityTeamManager>();
        }
    }


    //Singleton Methods=====================================================================================================================================================

    private void InitializeSingleton()
    {
        // if the singleton hasn't been initialized yet
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
    }

    //Public Methods========================================================================================================================================================

    //Player Private Fields Getters-----------------------------------------------------------------------------------------------------------------------------------------

    public WeaponHolder GetPlayerWeaponHolder() => _playerWeaponHolder;
    public FirstPersonController3D GetPlayerMovementController() => _playerMovementController;
    public LivingEntityStatusManager GetPlayerHealthManager() => _playerStatusManager;

    //Monster Private Fields Getters----------------------------------------------------------------------------------------------------------------------------------------
    public WeaponHolder GetMonsterWeaponHolder() => _monsterWeaponHolder;
    public FirstPersonController3D GetMonsterMovementController() => _monsterMovementController;
    public LivingEntityStatusManager GetMonsterHealthManager() => _monsterStatusManager;
}
