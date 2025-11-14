using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAliveState : BasePlayerState
{
    private Animator animator;
    private Vector2 moveDirection;

    private static readonly int velocity = Animator.StringToHash("velocity");

    public PlayerAliveState(Player player) : base(player)
    {
        animator = player.GetComponent<Animator>();
    }

    public override void Enter()
    {
        EventBus.Subscribe<ShootButtonPressedEvent>(OnShootButtonPressed);
        player.canMove = true;
        player.canShoot = true;
    }

    public override void Exit()
    {
        EventBus.Unsubscribe<ShootButtonPressedEvent>(OnShootButtonPressed);
    }

    public override void Update()
    {
        moveDirection = player.GetDirection();

        HandleAnimation();
    }

    public override void FixedUpdate()
    {
        if (player.canMove)
            player.Move(moveDirection * player.GetCurrentSpeed());
    }

    private void HandleAnimation()
    {
        animator.SetFloat(velocity, moveDirection.magnitude);
        player.playerBody.transform.FlipTowards(moveDirection.x);
    }

    private void OnShootButtonPressed(ShootButtonPressedEvent e)
    {
        if (e.Target == null) return;

        float dirX = e.Target.position.x - player.transform.position.x;
        player.playerBody.transform.FlipTowards(dirX);
    }
}
