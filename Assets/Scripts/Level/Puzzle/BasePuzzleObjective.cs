using System;
using UnityEngine;

public abstract class BasePuzzleObjective : MonoBehaviour
{
    public static event Action OnStateChanged;
    protected bool completed = false;
    protected bool locked = false;

    public void SetComplete(bool value = true)
    {
        if (completed == value) return;
        if(value && locked) return; // can't complete if locked
        completed = value;
        OnStateChanged?.Invoke();
    }

    public virtual void Reset()
    {
        completed = false;
    }

    public virtual void SetLocked(bool isLocked)
    {
        locked = isLocked;
    }

    public bool IsCompleted()
    {
        return completed;
    }
    public bool IsLocked()
    {
        return locked;
    }
}

//public interface IActionCaster
//{
//    public Vector2 GetPosition();
//    public Vector2 GetCastPosition();
//    public float GetCastAngle();

//}
