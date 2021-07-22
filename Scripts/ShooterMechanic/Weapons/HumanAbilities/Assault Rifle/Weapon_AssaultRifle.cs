using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_AssaultRifle : BasicAbility
{
    [Header("General Settings")]
    [SerializeField]
    private LayerMask _ignoreLayer;
    [Header("Weapon Settings")]
    [SerializeField]
    private int _maxAmmo = 30;
    [SerializeField]
    private float _shotCooldownLength = 0.1f;
    [SerializeField]
    private float _reloadCooldownLength = 3f;
    [SerializeField]
    private float _damage = 10f;
    [SerializeField]
    private float _range = 100f;
    [Header("References")]
    [SerializeField]
    private Transform _shootingPoint;
    [SerializeField]
    private MuzzleFlashEffect _muzzleFlash;




    //other fields
    private bool _canShoot = true;
    private float _shotCooldown = 0f;
    private int _currentAmmo = 30;
    private Vector3 _aimPoint;

    private bool _weaponActivated = false;

    //Debug Methods=========================================================================================================================================================

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_aimPoint,0.1f);
    }
    private void DB_ShotDebug(RaycastHit hit)
    {
        Debug.Log(hit.transform);
        Debug.DrawRay(MainReferenceHolder.Instance.GetPlayerMovementController().GetCameraHolder().position, 
            MainReferenceHolder.Instance.GetPlayerMovementController().GetCameraHolder().forward * 10f, Color.red, 2f);
        Debug.DrawLine(_shootingPoint.position, _aimPoint, Color.green, 2f);
    }

    //Initialization Methods================================================================================================================================================

    //Unity Awake
    private void Awake()
    {
    }

    //Update Methods========================================================================================================================================================

    //Unity update
    private void Update()
    {
        if(_shotCooldown>=0) _shotCooldown -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        OnHoldUpdateAction();
    }

    //Public BasicWeapon Override Methods===================================================================================================================================

    //Weapon Interaction Methods--------------------------------------------------------------------------------------------------------------------------------------------

    public override bool CanUse()
    {
        return _currentAmmo >= 0;
    }
    public override void OnActivate()
    {
        _weaponActivated = true;
    }

    public override void OnDeactivate()
    {
        _weaponActivated = false;
    }

    //Internal Logic========================================================================================================================================================
    
    //Primary Weapon Actions------------------------------------------------------------------------------------------------------------------------------------------------

    private void OnHoldUpdateAction()
    {
        if (!_weaponActivated) return;
        if (!_canShoot) return;
        if (_shotCooldown > 0f) return;
        if (_currentAmmo <= 0)
        {
            RunReloadSequence();
            return;
        }
        CreateEffects();
        ShootAmmo();
        _shotCooldown = _shotCooldownLength;
        _currentAmmo--;
    }

    //Shoots one bullet
    private void ShootAmmo()
    {
        Debug.Log("Shot performed!!");
        RaycastHit[] mutihit = Physics.RaycastAll(MainReferenceHolder.Instance.GetPlayerMovementController().GetCameraHolder().position,
            MainReferenceHolder.Instance.GetPlayerMovementController().GetCameraHolder().forward, _range);

        System.Array.Sort(mutihit, (x, y) => x.distance.CompareTo(y.distance));
        if (mutihit.Length > 0)
        {
            foreach (RaycastHit raycastHit in mutihit)
            {
                if (raycastHit.collider != _ownerCollider)
                {
                    _aimPoint = raycastHit.point;
                    DB_ShotDebug(raycastHit);//Calls debug method
                    Transform topmostPartent = raycastHit.transform.root;
                    IDamageable healthScript = topmostPartent.transform.GetComponentInChildren<IDamageable>();
                    if (healthScript != null)
                    {
                        healthScript.DealDamage(_damage);
                    }
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Hit air!");
        }
        

    }

    private void CreateEffects()
    {
        _muzzleFlash.CreateFlash();
    }

    private void RunReloadSequence()
    {
        _canShoot = false;
        StartCoroutine(ReloadCooldown(_reloadCooldownLength));
    }

    private IEnumerator ReloadCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _currentAmmo = _maxAmmo;
        _canShoot = true;
    }

}
