using UnityEngine;

public class Player : MonoBehaviour
{
    private const string GROUND_LAYER = "Ground";

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 150f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] GameObject _camera;

    public float health { get; private set; } = 100f; 
    public float maxHealth { get; private set; } = 100f;

    private Rigidbody _rb;
    private float _speedUpCount = 2f;
    private bool _isGrounded = true;
    private bool _isWalk = false;
    private bool _isSpeedingUp = false;
    private PlayerInput _playerInput;
    private PlayerVisual _playerVisual;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.Enable();
        _playerInput.Player.Jump.performed += _ => Jump();
        _playerInput.Player.SwitchCamera.performed += _ => SwitchCamera();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerVisual = transform.GetComponentInChildren<PlayerVisual>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer(GROUND_LAYER))
        {
            _isGrounded = true;
            _playerVisual.isJumpOnce = false;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            SpeedUp();
        }
        else
        {
            StopSpeedUp();
        }
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);

        if (worldMoveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(worldMoveDirection);
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, newRotation, _rotationSpeed * Time.fixedDeltaTime);
            _isWalk = true;
        }
        else
        {
            _isWalk = false;
        }

        Vector3 moveForce = worldMoveDirection * _moveSpeed;
        _rb.velocity = new Vector3(moveForce.x, _rb.velocity.y, moveForce.z);
    }

    public void GetDamage(float damage)
    {
        health -= damage;
    }

    private Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInput.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _isGrounded = false;
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void SwitchCamera()
    {
        if (_camera != null) 
        {
            bool isCameraActive = _camera.activeSelf; 
            _camera.SetActive(!isCameraActive);
        }
    }


    private void SpeedUp()
    {
        if (!_isSpeedingUp)
        {
            _isSpeedingUp = true;
            _moveSpeed *= _speedUpCount;
        }
    }

    private void StopSpeedUp()
    {
        if (_isSpeedingUp)
        {
            _isSpeedingUp = false;
            _moveSpeed /= _speedUpCount;
        }
    }

    public bool IsWalk()
    {
        return _isWalk;
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    public bool IsRun()
    {
        return _isSpeedingUp;
    }
}