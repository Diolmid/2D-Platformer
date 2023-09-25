using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private float damageRadius;

    private float _speed;
    private float _travelDistance;
    private float _xStartPosition;

    private bool _isGravityOn;
    private bool _hasHitGround;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    [SerializeField] private Transform damagePosition;

    private AttackDetails _attackDetails;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rb.gravityScale = 0;
        _rb.velocity = transform.right * _speed;

        _xStartPosition = transform.position.x;

        _isGravityOn = false;
    }

    private void Update()
    {
        if (!_hasHitGround)
        {
            _attackDetails.position = transform.position;

            if (_isGravityOn)
            {
                var angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_hasHitGround)
        {
            var damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
            var groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

            if (damageHit)
            { 
                damageHit.transform.SendMessage("Damage", _attackDetails);
                Destroy(gameObject);
            }

            if (groundHit)
            {
                _hasHitGround = true;
                _rb.gravityScale = 0;
                _rb.velocity = Vector2.zero;
            }

            if(Mathf.Abs(_xStartPosition - transform.position.x) >= _travelDistance && !_isGravityOn)
            {
                _isGravityOn = true;
                _rb.gravityScale = gravity;
            }
        }
    }

    public void SetupProjectile(float speed, float travelDistance, float damage)
    {
        _speed = speed;
        _travelDistance = travelDistance;
        _attackDetails.damage = damage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}