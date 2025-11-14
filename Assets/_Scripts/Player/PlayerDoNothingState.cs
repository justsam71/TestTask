using UnityEngine;

public class PlayerDoNothingState : BasePlayerState
{
    private Animator animator;
    public PlayerDoNothingState(Player player) : base(player)
    {
        animator = player.GetComponent<Animator>();
    }
    public override void Enter()
    {
        animator.Play("player_idle");
        player.HidePlayerHood(true);

        player.canMove = false;
        player.canShoot = false;
    }

    public override void Exit()
    {
        player.HidePlayerHood(false);
    }
}