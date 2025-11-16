using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class UIBarManager : MonoBehaviour
{
    [SerializeField] protected IngameStatsSO _protagonistStats = default; //the HealthBar is watching this object, which is the health of the player
    [SerializeField] protected TMP_Text _text = default;
    [SerializeField] protected Image _barImage;
    [SerializeField] protected float _timeToDrop;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _UIUpdateNeeded = default; //The player's Damageable issues this

    protected float _percentage;

    private void OnEnable()
    {
        _UIUpdateNeeded.OnEventRaised += UpdateBar;

        InitializeHealthBar();
    }

    private void OnDisable()
    {
        _UIUpdateNeeded.OnEventRaised -= UpdateBar;
    }

    private void InitializeHealthBar()
    {
        UpdateBar();
    }

    protected virtual void UpdateBar()
    {
    }


    protected IEnumerator SmoothBar()
    {
        float preChangeValue = _barImage.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < _timeToDrop)
        {
            elapsedTime += Time.deltaTime;
            _barImage.fillAmount = Mathf.Lerp(preChangeValue, _percentage, elapsedTime / _timeToDrop);
            yield return null;  // Wait for the next frame
        }

        _barImage.fillAmount = _percentage;  // Ensure the final value is set correctly
    }
}
