using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/SO Event Channel")]
public class SOEventChannelSO : DescriptionBaseSO
{

	public UnityAction<ScriptableObject> OnEventRaised;

	public void RaiseEvent(ScriptableObject value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}

}
