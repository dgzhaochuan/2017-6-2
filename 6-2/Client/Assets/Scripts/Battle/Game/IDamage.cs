


public interface IDamage
{
    int Damage(DamageMessage damage);
    int Hit(int value);
    void Cure(int value);
    void Die();
}