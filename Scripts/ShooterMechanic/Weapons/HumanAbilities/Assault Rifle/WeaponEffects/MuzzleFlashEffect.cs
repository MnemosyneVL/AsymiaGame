using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashEffect : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _lifeTime = 0.01f;
    [SerializeField]
    private float _minSize = 0.1f;
    [SerializeField]
    private float _maxSize = 0.2f;

    [Header("References")]
    [SerializeField]
    private GameObject _muzzleFlashObject;
    [SerializeField]
    private AudioSource _audioSource;

    //Initialization Methods================================================================================================================================================

    //Unity Start
    private void Start()
    {
        _muzzleFlashObject.SetActive(false);

    }
    //Public Methods========================================================================================================================================================
    public void CreateFlash()
    {
        StopAllCoroutines();
        _muzzleFlashObject.SetActive(true);
        float randomScale = Random.Range(_minSize, _maxSize);
        this.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        _muzzleFlashObject.transform.Rotate(Random.Range(0, 90), 0, 0);

        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.Play();
        StartCoroutine(DisableIn(_lifeTime));
    }

    //Internal Logic========================================================================================================================================================
    public IEnumerator DisableIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _muzzleFlashObject.SetActive(false);
    }
}
