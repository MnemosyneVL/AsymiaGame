using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int _ticksPerSecond = 2;
    [SerializeField]
    private float _setGrowthMultiplyer = 2f;
    [SerializeField]
    private float _flySpeed = 3f;

    [Header("References")]
    [SerializeField]
    private SphereCollider _sphereCollider;
    [SerializeField]
    private Rigidbody _rigidbody;

    //Other fields
    private float _damagePerTick;
    private float _secondsToExist;

    private float _damageCoolDown;

    private float _actualGrowthMultiplyer;
    private float _startingSize;

    private float _startingTime;

    private Collider _owner;


    //Public Initialization Methods=========================================================================================================================================
    public void ActivateFireBall( Collider owner ,Vector3 pushDirection,float damagePerTick, float secondsToExist)
    {
        Debug.Log("FireBall Created");
        _owner = owner;
        pushDirection.Normalize();
        _rigidbody.AddForce((pushDirection+new Vector3(Random.Range(-0.25f,0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f))) * _flySpeed);
        _damagePerTick = damagePerTick;
        _secondsToExist = secondsToExist;
        StartCoroutine(DestroyIn(secondsToExist));
        _actualGrowthMultiplyer = _setGrowthMultiplyer + Random.Range(-0.5f, 0.5f);
        _startingSize = this.transform.localScale.x;
        _startingTime = Time.time;
    }

    //Update Methods========================================================================================================================================================
    private void Update()
    {
        if (_damageCoolDown >= 0) _damageCoolDown -= Time.deltaTime;
        float currentTime = (Time.time - _startingTime) / _secondsToExist;
        float newScale = Mathf.Lerp(_startingSize, _actualGrowthMultiplyer, currentTime);
        this.transform.localScale = new Vector3(newScale, newScale, newScale);
    }


    //Internal Logic=========================================================================================================================================================

    //ColliderUpdate
    private void OnTriggerStay(Collider other)
    {
        if (other == _owner) return;
        Transform topmostPartent = other.transform.root;
        IDamageable damageableSubject = topmostPartent.gameObject.GetComponentInChildren<IDamageable>();
        if (damageableSubject != null && _damageCoolDown <= 0)
        {
            damageableSubject.DealDamage(_damagePerTick);
            _damageCoolDown = 1f / _ticksPerSecond;
        }
    }

    //Destroys game object in set amount of seconds
    IEnumerator DestroyIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
