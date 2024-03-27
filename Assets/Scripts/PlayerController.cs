using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    [Header("Move Settings")]
    [SerializeField] private float _speed;
    private float _directionX;
    private bool _flipped = false;

    [Header("Jump Settings")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _checkRadius;
    [SerializeField] private LayerMask _groundLayers;

    [Header("Dash Settings")]
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashDistance;
    private bool _dashing;

    [Header("UpDash")]
    [SerializeField] private float _upDashDistance;

    [Space]

    [SerializeField] private GameObject _dieParticle;
    [SerializeField] private float _delayAfterDie;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_dashing)
        {
            HorizontalMove();
            FaceDirection();
            VerticalDirection();
        }     
        SetAnimParametrs();
    }

    public bool Jump()
    {
        if (!_dashing)
        {
            if (CheckGround())
            {
                if (Physics2D.gravity.y <= 0) _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                else _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_jumpForce);

                return true;
            }
            return false;
        }
        else return false;
    }

    public bool UpDash()
    {
        if (!_dashing)
        {
            if (Physics2D.gravity.y <= 0) transform.position = new Vector2(transform.position.x, transform.position.y + _upDashDistance);
            else transform.position = new Vector2(transform.position.x, transform.position.y - _upDashDistance);

            _rigidbody.velocity = Vector2.zero;

            return true;
        }
        else return false;
    }

    public bool Dash()
    {
        if (!_dashing)
        {
            StartCoroutine(DashCore());
            return true;
        }
        else return false;
    }

    public void Die() => StartCoroutine(DieCore());

    private void HorizontalMove()
    {
        _directionX = Input.GetAxis("Horizontal");
        _rigidbody.velocity = new Vector2(_directionX * _speed, _rigidbody.velocity.y);
    }

    private bool CheckGround()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _groundLayers);
    }

    private void FaceDirection()
    {
        if (_directionX < 0 && !_flipped)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _flipped = !_flipped;
        }
        else if (_directionX > 0 && _flipped)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _flipped = !_flipped;
        }
    }

    private void VerticalDirection()
    {
        if (Physics2D.gravity.y <= 0) transform.localScale = new Vector2(transform.localScale.x, Mathf.Abs(transform.localScale.y));
        else transform.localScale = new Vector2(transform.localScale.x, -Mathf.Abs(transform.localScale.y));
    }

    private void SetAnimParametrs()
    {
        _animator.SetFloat("directionX", Mathf.Abs(_directionX));
        _animator.SetBool("isGround", CheckGround());
    }

    private IEnumerator DashCore()
    {
        _dashing = true;
        float time = 0;
        Vector2 startPos = transform.position;
        Vector2 endPos;

        if (!_flipped) endPos = new Vector2(startPos.x + _dashDistance, startPos.y);
        else endPos = new Vector2(startPos.x - _dashDistance, startPos.y);

        while (time <= _dashTime) 
        {
            Vector2 newPos = Vector2.Lerp(startPos, endPos, time / _dashTime);
            _rigidbody.MovePosition(newPos);
            time += Time.deltaTime;
            yield return null;
        }

        _rigidbody.MovePosition(endPos);
        _rigidbody.velocity = Vector2.zero;
        _dashing = false;
    }

    private IEnumerator DieCore()
    {
        Instantiate(_dieParticle, transform.position, transform.rotation);
        transform.position = new Vector2(1000, 1000);
        yield return new WaitForSeconds(_delayAfterDie);
        SceneLoader.Instance.Restart();
    }
}
