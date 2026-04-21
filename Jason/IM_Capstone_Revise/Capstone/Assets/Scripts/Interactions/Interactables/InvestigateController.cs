using UnityEngine;

public class InvestigateController : MonoBehaviour
{
    [Header("Investigation Indicator")]
    [Tooltip("The GameObject to activate when investigation is enabled, and deactivate when disabled.")]
    public GameObject investigationIndicator;

    // Enable the investigation indicator GameObject
    public void EnableInvestigation()
    {
        if (investigationIndicator != null)
            investigationIndicator.gameObject.SetActive(true);
    }

    // Disable the investigation indicator GameObject
    public void DisableInvestigation()
    {
        if (investigationIndicator != null)
            investigationIndicator.gameObject.SetActive(false);
    }
}