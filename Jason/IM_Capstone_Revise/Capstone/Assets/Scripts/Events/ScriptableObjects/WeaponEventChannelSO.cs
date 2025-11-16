using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>

[CreateAssetMenu(menuName = "Events/Weapon Event Channel")]
public class WeaponEventChannelSO : DescriptionBaseSO
{
	public UnityAction<int, WeaponDataSO> OnEventRaised;

	public void RaiseEvent(int index, WeaponDataSO value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(index, value);
	}
}

