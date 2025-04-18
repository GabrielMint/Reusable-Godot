using Godot;

public partial class HealthComponent : Node2D
{
    [Signal] public delegate void HealthChangedEventHandler(float health);
    [Signal] public delegate void HealthDepletedEventHandler();

    public override void _Ready()
    {
        InitiateHealth();
    }

    [Export]
    public float MaxHealth
    {
        get => maxHealth;
        private set
        {
            maxHealth = value;
            if (CurrentHealth > maxHealth)
            {
                CurrentHealth = maxHealth;
            }
        }
    }

    public bool HasHealhRemaining => !Mathf.IsEqualApprox(CurrentHealth, 0f);

    public float CurrentHealth
    {
        get => currentHealth;
        private set
        {
            GD.Print($"{Owner.Name} health changed from: " + currentHealth + " to: " + value);
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            EmitSignal(SignalName.HealthChanged, currentHealth);
            if (!HasHealhRemaining && !hasDied)
            {
                EmitSignal(SignalName.HealthDepleted);
            }
        }
    }

    private float currentHealth;
    private float maxHealth;
    private bool hasDied;

    public void TakeDamage(float damage)
    {
        GD.Print($"{Owner.Name} took damage: " + damage);
        CurrentHealth -= damage;
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
    }

    public void InitiateHealth()
    {
        CurrentHealth = MaxHealth;
        hasDied = false;
    }
}
