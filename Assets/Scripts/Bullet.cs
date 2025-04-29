using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IActionCaster
{
    private Vector2 _velocity;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime;
    [SerializeField] private float _damage;
    [SerializeField] private int _playerDamage = 1;

    [Header("Collision")]
    private int _teamType; // 0 - player, 1 - enemy, 2 - neutral
    private bool _canHitPlayer;
    private bool _canHitEnemy;
    //[SerializeField] private bool _canHitObjects; // boxes, tnt barrels
    //[SerializeField] private bool _canHitWalls;
    [SerializeField] private int _pierceCount = 1; // how much hits before bullet destroys
    private int _pierceHitCount = 0; // how much hits before bullet destroys

    [Header("Colors")]
    [SerializeField] private Color _playerColor;
    [SerializeField] private Color _enemyColor;
    [SerializeField] private Color _dangerColor;
    [SerializeField] private Color _neutralColor;

    //[Header("Masks")]
    //[SerializeField] private LayerMask _playerBulletLayer;
    //[SerializeField] private LayerMask _enemyBulletLayer;
    //[SerializeField] private LayerMask _neutralBulletLayer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        StartCoroutine(DestroyDelay());
    }
    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _velocity * _speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        var enemyHitbox = collision.gameObject.GetComponent<EnemyHitBox>();
        if (enemyHitbox != null)
        {
            enemyHitbox.Hit(_damage);
        }
        var playerHitbox = collision.gameObject.GetComponent<PlayerHitCollider>();
        if (playerHitbox != null)
        {
            playerHitbox.Hit(_playerDamage);
        }

        if (collision.isTrigger) return; // e.g. buttons

        _pierceHitCount += 1;
        if(_pierceHitCount >= _pierceCount)
        {
            Destroy(gameObject);
        }
    }
    
    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }
    public Vector2 GetCastPosition()
    {
        return transform.position;
    }
    public float GetCastAngle()
    {
        Vector2 dir = transform.up;
        return DirectionHandler.Vector2ToAngle(dir);
    }

    public int GetTeamType()
    {
        return _teamType;
    }
    public void SetTeam(int teamType)
    {
        if (teamType == 0) // player
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
            _spriteRenderer.color = _playerColor;
        }
        if (teamType == 1) // enemy
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
            _spriteRenderer.color = _playerDamage > 1 ? _dangerColor : _enemyColor;
        }
        if (teamType == 2) // neutral
        {
            gameObject.layer = LayerMask.NameToLayer("NeutralBullet");
            _spriteRenderer.color = _neutralColor;
        }
    }

}
