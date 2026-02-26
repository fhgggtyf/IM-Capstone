using UnityEngine;
using UnityEngine.Localization;

public enum ActorID
{
	PT, // Protagonist
	SB, // Sibling
	BR, // Boar
	BT, // Bat
	ML, // Mole
	FX, // Fox

}

/// <summary>
/// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
/// </summary>
[CreateAssetMenu(fileName = "newActor", menuName = "Dialogues/Actor")]
public class ActorSO : ScriptableObject
{
	[SerializeField] private ActorID _actorId = default;
	[SerializeField] private LocalizedString _actorName = default;

	public ActorID ActorId { get => _actorId; }
	public LocalizedString ActorName { get => _actorName; }
}


