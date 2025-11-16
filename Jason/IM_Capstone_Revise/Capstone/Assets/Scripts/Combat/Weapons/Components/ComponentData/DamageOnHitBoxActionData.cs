public class DamageOnHitBoxActionData : ComponentData<AttackProperties>
{
    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(EffectsOnHitBoxAction);
    }
}