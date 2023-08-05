using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    private enum State { Moving, Knockback, Dead }
    private State _currentState;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private float touchDamageCooldown;
    [SerializeField] private float touchDamage;

    [SerializeField] private Vector2 knockbackSpeed;
    [SerializeField] private Vector2 touchDamageArea;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform touchDamageChech;

    [SerializeField] private GameObject hitParticle;
    [SerializeField] private GameObject deathChunkParticle;
    [SerializeField] private GameObject deathBloodParticle;

    private int _facingDirection;
    private int _damageDirection;

    private float _currentHealth;
    private float _knockbackStartTime;
    private float[] _attackDetails = new float[2];
    private float _lastTouchDamageTime;

    private bool _groundDetected;
    private bool _wallDetected;

    private Vector2 _movement;
    private Vector2 _touchDamageBottomLeft;
    private Vector2 _touchDamageTopRight;

    private Rigidbody2D _aliveRb;
    private Animator _aliveAnimator;

    private GameObject _alive;

    private void Awake()
    {
        _alive = transform.Find("Alive").gameObject;
        _aliveRb = _alive.GetComponent<Rigidbody2D>();
        _aliveAnimator = _alive.GetComponent<Animator>();
    }

    private void Start()
    {
        _facingDirection = 1;
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    private void EnterMovingState()
    {

    }

    private void UpdateMovingState()
    {
        _groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        _wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        CheckTouchDamage();

        if (!_groundDetected || _wallDetected)
            Flip();
        else
        {
            _movement.Set(movementSpeed * _facingDirection, _aliveRb.velocity.y);
            _aliveRb.velocity = _movement;
        }
    }

    private void ExitMovingState()
    {

    }

    private void EnterKnockbackState()
    {
        _knockbackStartTime = Time.time;
        _movement.Set(knockbackSpeed.x * _damageDirection, knockbackSpeed.y);
        _aliveRb.velocity = _movement;
        _aliveAnimator.SetBool("knockback", true);
    }

    private void UpdateKnockbackState()
    {
        if (Time.time >= _knockbackStartTime + knockbackDuration)
            SwitchState(State.Moving);
    }

    private void ExitKnockbackState()
    {
        _aliveAnimator.SetBool("knockback", false);
    }

    private void EnterDeadState()
    {
        Instantiate(deathBloodParticle, _alive.transform.position, deathBloodParticle.transform.rotation);
        Instantiate(deathChunkParticle, _alive.transform.position, deathChunkParticle.transform.rotation);

        Destroy(gameObject);
    }

    private void UpdateDeadState()
    {

    }

    private void ExitDeadState()
    {

    }

    private void CheckTouchDamage()
    {
        if (Time.time >= _lastTouchDamageTime + touchDamageCooldown)
        {
            _touchDamageBottomLeft.Set(touchDamageChech.position.x - (touchDamageArea.x / 2), touchDamageChech.position.y - (touchDamageArea.y / 2));
            _touchDamageTopRight.Set(touchDamageChech.position.x + (touchDamageArea.x / 2), touchDamageChech.position.y + (touchDamageArea.y / 2));

            var hit = Physics2D.OverlapArea(_touchDamageBottomLeft, _touchDamageTopRight, whatIsPlayer);

            if (hit != null)
            {
                _lastTouchDamageTime = Time.time;
                _attackDetails[0] = touchDamage;
                _attackDetails[1] = _alive.transform.position.x;
                hit.SendMessage("Damage", _attackDetails);
            }
        }
    }

    private void Damage(float[] attackDetails)
    {
        _currentHealth -= attackDetails[0];

        Instantiate(hitParticle, _alive.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));

        _damageDirection = attackDetails[1] > _alive.transform.position.x ? -1 : 1;

        if (_currentHealth > 0)
            SwitchState(State.Knockback);
        else if(_currentHealth < 0)
            SwitchState(State.Dead);
    }

    private void Flip()
    {
        _facingDirection *= -1;
        _alive.transform.Rotate(0, 180, 0);
    }

    private void SwitchState(State newState)
    {
        switch (_currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }

        switch (newState)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }

        _currentState = newState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        var bottomLeft = new Vector2(touchDamageChech.position.x - (touchDamageArea.x / 2), touchDamageChech.position.y - (touchDamageArea.y / 2));
        var bottomRight = new Vector2(touchDamageChech.position.x + (touchDamageArea.x / 2), touchDamageChech.position.y - (touchDamageArea.y / 2));
        var topRight = new Vector2(touchDamageChech.position.x + (touchDamageArea.x / 2), touchDamageChech.position.y + (touchDamageArea.y / 2));
        var topLeft = new Vector2(touchDamageChech.position.x - (touchDamageArea.x / 2), touchDamageChech.position.y + (touchDamageArea.y / 2));

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(bottomRight, topRight);
    }
}