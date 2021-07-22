using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStoneScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _speed;

    [Header("References")]
    [SerializeField]
    private Rigidbody _rigidbody;

    //otherFields
    float _explosionRadius;
    float _damage;
    Collider _ownerCollider;

    Vector3 _explosionSpherePosition;

    //DEBUG================================================================================================================================================================

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(_explosionSpherePosition, _explosionRadius);
    }


    //Public Initialization Methods========================================================================================================================================
    public void ActivateStone(Collider ownerCollider,Vector3 direction, float explosionRadius, float damage)
    {
        _ownerCollider = ownerCollider;
        _explosionRadius = explosionRadius;
        _damage = damage;
        Debug.Log("Stone Created");
        _rigidbody.AddForce(direction * _speed);
        StartCoroutine(SelfDestructIn(10f));

    }


    //Internal Logic========================================================================================================================================================

    //ColliderUpdate
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider != _ownerCollider)
        {
            ExplodeStone();
        }
    }

    private void ExplodeStone()
    {
        //TODO Move to new object
        _explosionSpherePosition = this.transform.position;
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, _explosionRadius);
        foreach(Collider collider in colliders)
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
                if(movementScript != null)
                {
                    Vector3 pushDirection = topmostPartent.transform.position + Vector3.up - this.transform.position;
                    pushDirection.Normalize();
                    movementScript.Push(pushDirection, 500f);
                }
            }
        }

        Destroy(this.gameObject);
    }

    //Coroutines--------------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator SelfDestructIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ExplodeStone();
    }

}
