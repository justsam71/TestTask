using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Alive,
    Dead,
    DoNothing
}

public class Player : MonoBehaviour
{
    [Header("PlayerStateMachine")]
    private PlayerState currentStateType = PlayerState.Alive;
    private BasePlayerState currentState;

    [Header("Movement")]
    [SerializeField] private float currentSpeed = 6;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    [Header("References")]
    public Transform playerBody;
    [SerializeField] private GameObject playerHood;

    [Header("Flags")]
    public bool canMove = true;
    public bool canShoot = false;
    public bool dead = false;

    public Vector2 GetDirection() => playerInput.actions["Move"].ReadValue<Vector2>().normalized;
    public float GetCurrentSpeed() => currentSpeed;
    public void Move(Vector2 velocity) => rb.velocity = velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        if (currentState == null) SetNewPlayerState(PlayerState.DoNothing);
    }

    private void Update()
    {
        currentState.Update();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    public void SetNewPlayerState(PlayerState newState)
    {
        currentState?.Exit();

        switch (newState)
        {
            case PlayerState.Alive:
                currentState = new PlayerAliveState(this);
                break;
            case PlayerState.Dead:
                currentState = new PlayerDeadState(this);
                break;
            case PlayerState.DoNothing:
                currentState = new PlayerDoNothingState(this); 
                break;
        }

        currentState.Enter();
        currentStateType = newState;
    }

    public void HidePlayerHood(bool hide) 
    {
        if (hide) 
            playerHood.SetActive(false);
        else 
            playerHood.SetActive(true);
    }

    public void FreezePlayer(bool freeze)
    {
        if (freeze)
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDestroy()
    {
        currentState?.Exit();
    }
}
