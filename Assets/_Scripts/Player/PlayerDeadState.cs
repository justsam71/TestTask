using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : BasePlayerState
{
    private Animator animator;
    public PlayerDeadState(Player player) : base(player)
    {
        animator = player.GetComponent<Animator>();
    }

    public override void Enter()
    {
        animator.SetTrigger("Die");
        player.HidePlayerHood(true);

        player.canMove = false;
        player.canShoot = false;
        player.dead = true;

        player.FreezePlayer(true);
    }

}
