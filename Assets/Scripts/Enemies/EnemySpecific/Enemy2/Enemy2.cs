using UnityEngine;

public class Enemy2 : Entity
{
    public E2_MoveState MoveState { get; private set; }
    public E2_IdleState IdleState { get; private set; }
    public E2_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E2_MeleeAttackState MeleeAttackState { get; private set; }
    public E2_LookForPlayerState LookForPlayerState { get; private set; }
    public E2_StunState StunState { get; private set; }
    public E2_DeadState DeadState { get; private set; }

    [SerializeField] private Transform meleeAttackPosition;

    [Space]
    [SerializeField] private D_Move moveStateData;
    [SerializeField] private D_Idle idleStateData;
    [SerializeField] private D_PlayerDetected playerDetectedStateData;
    [SerializeField] private D_MeleeAttack meleeAttackStateData;
    [SerializeField] private D_LookForPlayer lookForPlayerStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;

    public override void Start()
    {
        base.Start();

        MoveState = new E2_MoveState(this, StateMachine, "move", moveStateData, this);
        IdleState = new E2_IdleState(this, StateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E2_PlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
        MeleeAttackState = new E2_MeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        LookForPlayerState = new E2_LookForPlayerState(this, StateMachine, "lookForPlayer", lookForPlayerStateData, this);
        StunState = new E2_StunState(this, StateMachine, "stun", stunStateData, this);
        DeadState = new E2_DeadState(this, StateMachine, "dead", deadStateData, this);

        StateMachine.Initialize(MoveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDead)
            StateMachine.ChangeState(DeadState);
        else if (isStunned && StateMachine.CurrentState != StunState)
            StateMachine.ChangeState(StunState);
        else if (!CheckPlayerInMinAgroRange())
        {
            LookForPlayerState.SetTurnImmediately(true);
            StateMachine.ChangeState(LookForPlayerState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}