using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableArenaManager : MonoBehaviour
{

    //Public Initialization Methods=========================================================================================================================================
    public void ActivatePortableArena (float secondsToExist)
    {
        Debug.Log("Portable Arena Created");
        StartCoroutine(DestroyIn(secondsToExist));
    }

    //Destroys game object in set amount of seconds
    IEnumerator DestroyIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
