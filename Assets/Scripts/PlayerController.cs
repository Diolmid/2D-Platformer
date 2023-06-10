using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float airDragMultiplier = 0.95f;
    [SerializeField] private float turnTimerSet = 0.1f;

    [Space]
    [SerializeField] private int jumpsAmount = 1;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float variableJumpHeightMultiplier = 0.5f;
    [SerializeField] private float jumpTimerSet = 0.15f;

    [Space]
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float wallJumpTimerSet = 0.5f;
    [SerializeField] private Vector2 wallJumpDirection;

    [Space]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;

    [Space]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform wallCheck;

    private int _jumpsAmountLeft;
    private int _lastWallJumpDirection;
    private int _facingDirection = 1;

    private float _movementInputDirection;
    private float _jumpTimer;
    private float _turnTimer;
    private float _wallJumpTimer;

    private bool _isFacingRight = true;
    private bool _isWalking;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isWallSliding;
    private bool _canNormalJump;
    private bool _canWallJump;
    private bool _isAttemptingToJump;
    private bool _checkJumpMultiplier;
    private bool _canMove;
    private bool _canFlip;
    private bool _hasWallJumped;

    private Rigidbody2D _rb;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _jumpsAmountLeft = jumpsAmount;
        wallJumpDirection.Normalize();
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckIfCanJump();
        CheckIfWallSliding();
        UpdateAnimations();
        CheckJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if(_isGrounded || (_jumpsAmountLeft > 0 && _isTouchingWall))
                NormalJump();
            else
            {
                _jumpTimer = jumpTimerSet;
                _isAttemptingToJump = true;
            }
        }

        if(Input.GetButtonDown("Horizontal") && _isTouchingWall)
        {
            if(!_isGrounded && _movementInputDirection != _facingDirection)
            {
                _canMove = false;
                _canFlip = false;

                _turnTimer = turnTimerSet;
            }
        }

        if (!_canMove)
        {
            _turnTimer -= Time.deltaTime;
            if(_turnTimer <= 0)
            {
                _canMove = true;
                _canFlip = true;
            }
        }

        if (_checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            _checkJumpMultiplier = false;
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    private void CheckMovementDirection()
    {
        if (_isFacingRight && _movementInputDirection < 0)
            Flip();
        else if (!_isFacingRight && _movementInputDirection > 0)
            Flip();

        _isWalking = _rb.velocity.x != 0;
    }

    private void CheckIfWallSliding()
    {
        _isWallSliding = (_isTouchingWall && _movementInputDirection == _facingDirection && _rb.velocity.y < 0);
    }

    private void CheckIfCanJump()
    {
        if (_isGrounded && _rb.velocity.y <= 0.01f)
            _jumpsAmountLeft = jumpsAmount;

        if (_isTouchingWall)
            _canWallJump = true;

        _canNormalJump = _jumpsAmountLeft > 0;
    }

    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        _isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void UpdateAnimations()
    {
        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetFloat("yVelocity", _rb.velocity.y);
        _animator.SetBool("isWallSliding", _isWallSliding);
    }

    private void Flip()
    {
        if (!_isWallSliding && _canFlip)
        {
            _facingDirection *= -1;
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    private void CheckJump()
    {
        if (_jumpTimer > 0)
        {
            if (!_isGrounded && _isTouchingWall && _movementInputDirection != 0 && _movementInputDirection != _facingDirection)
                WallJump();
            else if (_isGrounded)
                NormalJump();
        }

        if(_isAttemptingToJump)
            _jumpTimer -= Time.deltaTime;

        if(_wallJumpTimer > 0)
        {
            if (_hasWallJumped && _movementInputDirection == -_lastWallJumpDirection)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
                _hasWallJumped = false;
            }
            else if (_wallJumpTimer <= 0)
                _hasWallJumped = false;
            else
                _wallJumpTimer -= Time.deltaTime;
        }
    }

    private void NormalJump()
    {
        if (_canNormalJump && !_isWallSliding)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            _jumpsAmountLeft--;
            _jumpTimer = 0;
            _isAttemptingToJump = false;
            _checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (_canWallJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);

            _isWallSliding = false;
            _jumpsAmountLeft = jumpsAmount;
            _jumpsAmountLeft--;
            _jumpTimer = 0;
            _isAttemptingToJump = false;
            _checkJumpMultiplier = true;
            _turnTimer = 0;
            _canMove = true;
            _canFlip = true;
            _hasWallJumped = true;
            _wallJumpTimer = wallJumpTimerSet;
            _lastWallJumpDirection = -_facingDirection;

            var forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * _movementInputDirection, wallJumpForce * wallJumpDirection.y);
            _rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    private void ApplyMovement()
    {
        
        if (!_isGrounded && !_isWallSliding && _movementInputDirection == 0)
            _rb.velocity = new Vector2(_rb.velocity.x * airDragMultiplier, _rb.velocity.y);
        else if(_canMove)
            _rb.velocity = new Vector2(movementSpeed * _movementInputDirection, _rb.velocity.y);

        if (_isWallSliding)
        {
            if(_rb.velocity.y < -wallSlidingSpeed)
                _rb.velocity = new Vector2(_rb.velocity.x, -wallSlidingSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z)); ;
    }
}