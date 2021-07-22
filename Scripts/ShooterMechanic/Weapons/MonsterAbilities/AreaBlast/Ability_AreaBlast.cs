using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_AreaBlast : BasicAbility
{
    [Header("Settings")]
    [SerializeField]
    private float _damage = 50f;
    [SerializeField]
    private float _pushForce = 500f;
    [SerializeField]
    private float _radius = 5f;

    [Header("References")]
    [SerializeField]
    private AbilitiesUIManager _abilitiesUI;


    //Update Methods========================================================================================================================================================

    //Unity Update
    private void Update()
    {
        if (_currentCooldown >= 0)
        {
            //_abilitiesUI.DisableAbility();
            _currentCooldown -= Time.deltaTime;
            //_abilitiesUI.UpdateCooldownNr(_currentCooldown);
        }
        else
        {
            //_abilitiesUI.EnableAbility();
        }
    }

    //Public BasicWeapon Override Methods===================================================================================================================================

    //
    public override bool CanUse()
    {
        return _currentCooldown <= 0;
    }

    public override void OnActivate()
    {
        Debug.Log("BlastArea Activated");
        Transform ownerTopmostPartent = _ownerCollider.transform.root;
        FirstPersonController3D monsterMovementController = ownerTopmostPartent.gameObject.GetComponentInChildren<FirstPersonController3D>();
        Collider[] colliders = Physics.OverlapSphere(monsterMovementController.GetOriginPoint(), _radius);
        foreach (Collider collider in colliders)
        {

            if (collider != _ownerCollider)
            {
                Transform topmostPartent = collider.transform.root;
                IDamageable healthScript = topmostPartent.GetComponentInChildren<IDamageable>();
                IMoveable movementScript = topmostPartent.GetComponentInChildren<IMoveable>();

                if (healthScript != null)
                {
                    healthScript.DealDamage(_damage);
                }
                if (movementScript != null)
                {
                    Vector3 pushDirection = topmostPartent.transform.position + Vector3.up - this.transform.position;
                    pushDirection.Normalize();
                    movementScript.Push(pushDirection, _pushForce);
                }
            }
        }
        _currentCooldown = _usageCooldown;
    }

    //ML methods-----------------------------------------------------------------------------------------------------------------------------------------------------------

    public override float RequestCooldown()
    {
        return _currentCooldown;
    }
}
