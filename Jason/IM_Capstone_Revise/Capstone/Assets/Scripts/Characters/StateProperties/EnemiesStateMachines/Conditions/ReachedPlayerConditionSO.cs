using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// </summary>
[CreateAssetMenu(fileName = "ReachPlayerCondition", menuName = "State Machines/Conditions/Enemies/Reached Player")]
public class ReachedPlayerConditionSO : StateConditionSO<ReachedPlayerCondition>
{
    public float didstanceThreshold = 0.5f;
    public TransformAnchor _playerTransform;
}

public class ReachedPlayerCondition : Condition
{

    ReachedPlayerConditionSO _origin => (ReachedPlayerConditionSO)base.OriginSO;

    private NonPlayerCharacter _npc;
    private Transform _thisTransform;


    private float didstanceThreshold;

    private TransformAnchor _playerTransformAnchor;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
        _thisTransform = _npc.gameObject.transform;
        _playerTransformAnchor = _origin._playerTransform;
        didstanceThreshold = _origin.didstanceThreshold;
    }

    protected override bool Statement()
    {
        
        return Vector3.Distance(_playerTransformAnchor.Value.position, _thisTransform.position) <= didstanceThreshold;
    }

}
