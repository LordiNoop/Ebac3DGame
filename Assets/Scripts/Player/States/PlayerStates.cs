using Ebac.Core.Singleton;
using Ebac.StateMachine;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public enum PlayerMovementStates
    {
        IDLE,
        RUN,
        JUMP
    }

    public StateMachine<PlayerMovementStates> stateMachine;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        stateMachine = new StateMachine<PlayerMovementStates>();

        stateMachine.Init();
        stateMachine.RegisterStates(PlayerMovementStates.IDLE, new PSIdle());
        stateMachine.RegisterStates(PlayerMovementStates.RUN, new PSRun());
        stateMachine.RegisterStates(PlayerMovementStates.JUMP, new PSJump());

        stateMachine.SwitchState(PlayerMovementStates.IDLE);
    }
}