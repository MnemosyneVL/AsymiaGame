using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferenceHolder : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private FirstPersonController3D _fpsController;
    [SerializeField]
    private WeaponHolder _weaponHolder;
    [SerializeField]
    private LivingEntityStatusManager _playerStatusManager;

    //singleton fields
    private static PlayerReferenceHolder _instance = null;

    public static PlayerReferenceHolder Instance
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

    //Data Requesters-------------------------------------------------------------------------------------------------------------------------------------------------------

    public FirstPersonController3D RequestFPSCOntroller() => _fpsController;

    public WeaponHolder RequestWeaponHolder() => _weaponHolder;

    public LivingEntityStatusManager RequestPlayerStatusManager() => _playerStatusManager;
}
