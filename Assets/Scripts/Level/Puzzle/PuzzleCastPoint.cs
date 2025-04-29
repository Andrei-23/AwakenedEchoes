using System;
using UnityEngine;

public class PuzzleCastPoint : MonoBehaviour
{
    public float triggerRadius;
    [SerializeField] private ParticleSystem _activeParticles;

    //public static event Action OnCast;

    //[SerializeField] private LayerMask _playerMask;
    public bool IsTriggered(Vector2 playerPos)
    {
        Vector2 point = transform.position;
        Vector2 d = playerPos - point;
        return d.magnitude <= triggerRadius;
    }

    private void Update()
    {
        bool inside = IsTriggered(PlayerCaster.Instance.transform.position);
        if(inside && _activeParticles.isPlaying == false)
            _activeParticles.Play();
        if(inside == false && _activeParticles.isPlaying)
            _activeParticles.Stop();
    }

    //public void StartCast()
    //{
    //    OnCast?.Invoke();
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(((1 << collision.gameObject.layer) & _playerMask) != 0)
    //    {

    //    }
    //}
}
