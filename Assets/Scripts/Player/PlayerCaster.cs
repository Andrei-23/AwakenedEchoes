using UnityEngine;
using Zenject;

/// <summary>
/// Player class, must be attached to player object only
/// </summary>
public class PlayerCaster : MonoBehaviour, IActionCaster
{
    [SerializeField] private Transform _castPoint;

    [Inject] private PlayerMovement _movement;

    private static PlayerCaster instance;
    public static PlayerCaster Instance { 
        get { 
            if(instance == null)
            {
                instance = FindAnyObjectByType<PlayerCaster>();
            }
            return instance;
        }
        private set { instance = value; }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public Vector2 GetPosition()
    {
        return Instance.transform.position;
    }
    public Vector2 GetCastPosition()
    {
        return _castPoint.position;
    }
    public float GetCastAngle()
    {
        Vector2 dir = _movement.lookDirection;
        return DirectionHandler.Vector2ToAngle(dir);
    }

}
