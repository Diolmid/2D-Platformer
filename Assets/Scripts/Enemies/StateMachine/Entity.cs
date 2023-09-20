using UnityEngine;

public class Entity : MonoBehaviour
{
    public int FacingDirection { get; private set; }
    public int LastDamageDirection { get; private set; }

    public D_Entity entityData;

    public FiniteStateMachine StateMachine { get; private set; }
    public AnimationToStateMachine AnimationToStateMachine { get; private set; }

    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }
    public GameObject AliveGO { get; private set; }

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;

    protected bool isStunned;
    protected bool isDead;

    private float _currentHealth;
    private float _currentStunResistance;
    private float _lastDamageTime;

    private Vector2 _velocityWorkspace;

    public virtual void Awake()
    {
        AliveGO = transform.Find("Alive").gameObject;
        Rb = AliveGO.GetComponent<Rigidbody2D>();
        Animator = AliveGO.GetComponent<Animator>();
        AnimationToStateMachine = AliveGO.GetComponent<AnimationToStateMachine>();
    }

    public virtual void Start()
    {
        FacingDirection = 1;
        _currentHealth = entityData.maxHealth;
        _currentStunResistance = entityData.stunResistance;

        StateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        StateMachine.CurrentState.LogicUpdate();

        if(Time.time >= LastDamageDirection + entityData.stunRecoveryTime)
            ResetStunResistance();
    }

    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        _currentStunResistance = entityData.stunResistance;
    }
    
    public virtual void Damage(AttackDetails attackDetails)
    {
        _lastDamageTime = Time.time;

        _currentHealth -= attackDetails.damage;
        _currentStunResistance -= attackDetails.stunDamageAmount;

        DamageHop(entityData.damageHopSpeed);

        Instantiate(entityData.hitParticle, AliveGO.transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));

        LastDamageDirection = attackDetails.position.x > AliveGO.transform.position.x ? -1 : 1;

        isStunned = _currentStunResistance <= 0;
        isDead = _currentHealth <= 0;
    }

    public virtual void DamageHop(float velocity)
    {
        _velocityWorkspace.Set(Rb.velocity.x, velocity);
        Rb.velocity = _velocityWorkspace;
    }

    public virtual void Flip()
    {
        FacingDirection *= -1;
        AliveGO.transform.Rotate(0, 180, 0);
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        _velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        Rb.velocity = _velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity)
    {
        _velocityWorkspace.Set(FacingDirection * velocity, Rb.velocity.y);
        Rb.velocity = _velocityWorkspace;
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, AliveGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, AliveGO.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, AliveGO.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, AliveGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(entityData.wallCheckDistance * FacingDirection * Vector2.right));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(entityData.ledgeCheckDistance * Vector2.down));

        Gizmos.DrawLine(playerCheck.position, playerCheck.position + (Vector3)(entityData.maxAgroDistance * FacingDirection * Vector2.right));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + (Vector3)(entityData.minAgroDistance * FacingDirection * Vector2.right));
    }
}