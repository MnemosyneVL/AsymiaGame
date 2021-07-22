using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilitiesUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject _disablesUIObject;
    [SerializeField]
    private TextMeshProUGUI _coolDownText;

    //other Fields
    private bool _isEnabled = true;

    //Initialization Methods================================================================================================================================================

    private void Start()
    {
        _disablesUIObject.SetActive(false);
    }

    //Public Methods========================================================================================================================================================


    public void DisableAbility()
    {
        if (_isEnabled)
        {
            _isEnabled = false;
            _disablesUIObject.SetActive(true);
        }
    }

    public void EnableAbility()
    {

        if (!_isEnabled)
        {
            _isEnabled = true;
            _disablesUIObject.SetActive(false);
        };
    }

    public void UpdateCooldownNr(float cooldown)
    {
        int text = Mathf.CeilToInt(cooldown);
        _coolDownText.text = text.ToString();
    }
}
