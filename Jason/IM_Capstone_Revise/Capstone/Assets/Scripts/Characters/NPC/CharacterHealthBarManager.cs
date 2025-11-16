using UnityEngine;
using TMPro;
using System.Collections;

public class CharacterHealthBarManager : MonoBehaviour
{
    [SerializeField] private NonPlayerStatsManager _statsManager = default;
    [SerializeField] private Damageable _damageable = default;
    [SerializeField] private SpriteRenderer _healthBarSprite;
    [SerializeField] private float _updateSpeed;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _barUpdateNeeded; //The player's Damageable issues this

    private float targetScaleX;

    private void OnEnable()
    {
        _barUpdateNeeded = _damageable.updateHealthBar;

        _barUpdateNeeded.OnEventRaised += UpdateHealth;

        InitializeHealthBar();
    }

    private void OnDestroy()
    {
        _barUpdateNeeded.OnEventRaised -= UpdateHealth;
    }

    private void InitializeHealthBar()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        targetScaleX = Mathf.Max((float)_statsManager.CurrentStatsSO.CurrentHealth / _statsManager.StatsConfig.InitialHealth, 0);
        StartCoroutine(SmoothHealthBar());
    }

    private IEnumerator SmoothHealthBar()
    {
        float preChangeScaleX = _healthBarSprite.transform.localScale.x;
        float elapsedTime = 0f;

        while (elapsedTime < _updateSpeed)
        {
            elapsedTime += Time.deltaTime;
            float newScaleX = Mathf.Max(Mathf.Lerp(preChangeScaleX, targetScaleX, elapsedTime / _updateSpeed), 0);
            SetHealthBarScale(newScaleX);
            yield return null;
        }

        SetHealthBarScale(targetScaleX);  // Ensure the final value is set correctly
    }

    private void SetHealthBarScale(float scaleX)
    {
        _healthBarSprite.transform.localScale = new Vector3(scaleX, _healthBarSprite.transform.localScale.y, _healthBarSprite.transform.localScale.z);
    }
}
