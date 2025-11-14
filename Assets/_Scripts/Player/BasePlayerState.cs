using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerState 
{
    protected Player player;
    public BasePlayerState(Player player)
    {
        this.player = player;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { } 

}
