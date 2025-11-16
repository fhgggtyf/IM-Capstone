public interface IDamageable
{
    public bool Invincible { get; set; }
    void Damage(DamageData data);
}