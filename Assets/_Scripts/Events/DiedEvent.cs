using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiedEvent 
{
    public Health health;
    public GameObject owner;
    public DiedEvent(Health health)
    {
        this.health = health;
        owner = health.gameObject;
    }
}
