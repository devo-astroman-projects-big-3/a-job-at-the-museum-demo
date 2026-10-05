using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _gravity = -20f;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private CharacterController _controller;
    private float _verticalVelocity;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        // Prevent automatically stepping over low obstacles.
        // Increase this in the Inspector if your game needs stairs.
        _controller.stepOffset = 0f;
        _controller.minMoveDistance = 0f;
    }

    private void Update()
    {
        if (!_controller.enabled)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        direction = Vector3.ClampMagnitude(direction, 1f);

        if (_spriteRenderer != null)
        {
            if (horizontal < 0f)
                _spriteRenderer.flipX = true;
            else if (horizontal > 0f)
                _spriteRenderer.flipX = false;
        }

        if (_controller.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        _verticalVelocity += _gravity * Time.deltaTime;

        // Speed affects horizontal movement, not gravity.
        Vector3 velocity = direction * _speed;
        velocity.y = _verticalVelocity;

        Vector3 previousPosition = transform.position;

        // Move handles collision blocking and sliding along walls.
        CollisionFlags collisions =
            _controller.Move(velocity * Time.deltaTime);

        if ((collisions & CollisionFlags.Above) != 0 &&
            _verticalVelocity > 0f)
        {
            _verticalVelocity = 0f;
        }

        if ((collisions & CollisionFlags.Below) != 0 &&
            _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }

        // Animate actual horizontal movement, not pushing into a wall.
        Vector3 displacement = transform.position - previousPosition;
        displacement.y = 0f;

        bool isRunning =
            direction.sqrMagnitude > 0.01f &&
            displacement.sqrMagnitude > 0.000001f;

        if (_animator != null)
            _animator.SetBool("isRunning", isRunning);
    }
    #endregion
}