using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// Player movement and physics manager
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;

    [Header("Walking")]
    [SerializeField] private float _defaultSpeed;
    private Vector2 _walkVelocity;

    [Header("Roll")]
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _rollDuration;
    private bool _isRolling = false;
    private Vector2 _rollVelocity = Vector2.right;

    [Header("Dash")]
    [SerializeField] private float _dashSpeed;
    private int _dashesActive = 0;
    private Vector2 _dashVelocity = Vector2.zero; // cumulative dashes speed.

    [HideInInspector] public Vector2 lookDirection { get; private set; }

    [Header("Other")]
    [SerializeField] private GameObject _lookPoint;
    [SerializeField] private LayerMask _rollMask;
    [SerializeField] private LayerMask _waterMask;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        DashAction.OnActivate += OnDashActive;
        RollAction.OnActivate += Roll;
    }
    private void OnDestroy()
    {
        DashAction.OnActivate -= OnDashActive;
        RollAction.OnActivate -= Roll;
    }

    void FixedUpdate()
    {
        if(_isRolling == false && _walkVelocity != Vector2.zero)
        {
            _rollVelocity = _walkVelocity;
        }

        if (_dashesActive != 0)
        {
            _rb.linearVelocity = _dashVelocity * _dashSpeed;
            return;
        }

        if (_isRolling)
        {
            _rb.linearVelocity = _rollVelocity * _rollSpeed;
            return;
        }
        _rb.linearVelocity = _walkVelocity * _defaultSpeed;

        CheckFalling();
    }

    private void CheckFalling()
    {
        Tilemap waterTilemap = LevelManager.Instance.waterTilemap;
        Vector3Int cellPosition = waterTilemap.WorldToCell(transform.position);

        TileBase tile = waterTilemap.GetTile(cellPosition);

        if (tile != null)
        {
            PlayerStats.Instance.KillPlayer();
        }
    }
    public void setMoveDirection(Vector2 moveDirection)
    {
        _walkVelocity = moveDirection;
    }
    private void OnDashActive(DashData dashData)
    {
        float newAngle = PlayerCaster.Instance.GetCastAngle() + DirectionHandler.Vector2ToAngle(dashData.direction);
        Vector2 dir = DirectionHandler.AngleToVector2(newAngle, dashData.direction.magnitude);
        Dash(dir, dashData.duration);
    }
    public void Dash(Vector2 velocity, float duration)
    {
        _dashesActive++;
        _dashVelocity += velocity;
        StartCoroutine(StopDash(velocity, duration));
    }
    public IEnumerator StopDash(Vector2 velocity, float duration)
    {
        yield return new WaitForSeconds(duration);
        _dashesActive--;
        _dashVelocity -= velocity;
    }

    public void Roll()
    {
        if (_isRolling) return;
        _isRolling = true;

        _rb.excludeLayers = _rollMask;
        // play animation
        StartCoroutine(StopRoll());
    }
    public IEnumerator StopRoll()
    {
        yield return new WaitForSeconds(_rollDuration);

        // stop animation
        _rb.excludeLayers = 0;
        _isRolling = false;
    }

    public void SetLookDirection(Vector2 direction)
    {
        lookDirection = direction;
        _lookPoint.transform.localPosition = lookDirection;
        //Debug.Log(lookDirection);
    }
}
