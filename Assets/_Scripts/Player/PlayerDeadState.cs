using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : BasePlayerState
{
    public PlayerDeadState(Player player) : base(player) { }

    public override void Enter()
    {
        player.HidePlayerHood(true);
        player.playerBody.gameObject.SetActive(false);

        player.canMove = false;
        player.canShoot = false;
        player.dead = true;

        player.FreezePlayer(true);
    }

}
