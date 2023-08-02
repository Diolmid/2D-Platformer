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

    [Space]
    [SerializeField] private float ledgeClimbXOffset1 = 0;
    [SerializeField] private float ledgeClimbXOffset2 = 0;
    [SerializeField] private float ledgeClimbYOffset1 = 0;
    [SerializeField] private float ledgeClimbYOffset2 = 0;
    [SerializeField] private Transform ledgeCheck;

    [Space]
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float distanceBetweenImages;
    [SerializeField] private float dashCoolDown;

    private int _jumpsAmountLeft;
    private int _lastWallJumpDirection;
    private int _facingDirection = 1;

    private float _movementInputDirection;
    private float _jumpTimer;
    private float _turnTimer;
    private float _wallJumpTimer;
    private float _dashTimeLeft;
    private float _lastImagePositionX;
    private float _lastDash = -100;

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
    private bool _isTouchingLedge;
    private bool _canClimbLedge = false;
    private bool _ledgeDetected;
    private bool _isDashing;

    private Vector2 _ledgePosBot;
    private Vector2 _ledgePos1;
    private Vector2 _ledgePos2;

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
        CheckLedgeClimb();
        CheckDash();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    public void FinishLedgeClimb()
    {
        _canClimbLedge = false;
        transform.position = _ledgePos2;
        _canMove = true;
        _canFlip = true;
        _ledgeDetected = false;
        _animator.SetBool("canClimbLedge", _canClimbLedge);
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded || (_jumpsAmountLeft > 0 && _isTouchingWall))
                NormalJump();
            else
            {
                _jumpTimer = jumpTimerSet;
                _isAttemptingToJump = true;
            }
        }

        if (Input.GetButtonDown("Horizontal") && _isTouchingWall)
        {
            if (!_isGrounded && _movementInputDirection != _facingDirection)
            {
                _canMove = false;
                _canFlip = false;

                _turnTimer = turnTimerSet;
            }
        }

        if (_turnTimer >= 0)
        {
            _turnTimer -= Time.deltaTime;
            if (_turnTimer <= 0)
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
            AttemptToDash();
    }
    
    private void AttemptToDash()
    {
        _isDashing = true;
        _dashTimeLeft = dashTime;
        _lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        _lastImagePositionX = transform.position.x;
    }

    private void CheckMovementDirection()
    {
        if (_isFacingRight && _movementInputDirection < 0)
            Flip();
        else if (!_isFacingRight && _movementInputDirection > 0)
            Flip();

        _isWalking = Mathf.Abs(_rb.velocity.x) >= 0.01f;
    }

    private void CheckIfWallSliding()
    {
        _isWallSliding = (_isTouchingWall && _movementInputDirection == _facingDirection && _rb.velocity.y < 0 && !_canClimbLedge);
    }

    private void CheckIfCanJump()
    {
        if (_isGrounded && _rb.velocity.y <= 0.01f)
            _jumpsAmountLeft = jumpsAmount;

        if (_isTouchingWall)
            _canWallJump = true;

        _canNormalJump = _jumpsAmountLeft > 0;
    }

    private void CheckLedgeClimb()
    {
        if(_ledgeDetected && !_canClimbLedge)
        {
            _canClimbLedge = true;

            if (_isFacingRight)
            {
                _ledgePos1 = new Vector2(Mathf.Floor(_ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(_ledgePosBot.y) + ledgeClimbYOffset1);
                _ledgePos2 = new Vector2(Mathf.Floor(_ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(_ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                _ledgePos1 = new Vector2(Mathf.Ceil(_ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(_ledgePosBot.y) + ledgeClimbYOffset1);
                _ledgePos2 = new Vector2(Mathf.Ceil(_ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(_ledgePosBot.y) + ledgeClimbYOffset2);
            }

            _canMove = false;
            _canFlip = false;

            _animator.SetBool("canClimbLedge", _canClimbLedge);
        }

        if (_canClimbLedge)
            transform.position = _ledgePos1;
    }

    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        _isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        _isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if(_isTouchingWall && !_isTouchingLedge && !_ledgeDetected)
        {
            _ledgeDetected = true;
            _ledgePosBot = wallCheck.position;
        }
    }

    private void UpdateAnimations()
    {
        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetFloat("yVelocity", _rb.velocity.y);
        _animator.SetBool("isWallSliding", _isWallSliding);
    }

    public void DisableFlip()
    {
        _canFlip = false;
    }

    public void EnableFlipp()
    {
        _canFlip = true;
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

    private void CheckDash()
    {
        if (_isDashing)
        {
            if(_dashTimeLeft > 0)
            {
                _canMove = false;
                _canFlip = false;
                _rb.velocity = new Vector2(dashSpeed * _facingDirection, 0);
                _dashTimeLeft -= Time.deltaTime;

                if(Mathf.Abs(transform.position.x - _lastImagePositionX) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    _lastImagePositionX = transform.position.x;
                }
            }

            if(_dashTimeLeft <= 0 || _isTouchingWall)
            {
                _isDashing = false;
                _canMove = true;
                _canFlip = true;
            }
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