using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HealthChangedEvent
{
    public Health health;
    public float current;
    public float max;

    public HealthChangedEvent(Health health, float current, float max)
    {
        this.health = health;
        this.current = current;
        this.max = max;
    }
}