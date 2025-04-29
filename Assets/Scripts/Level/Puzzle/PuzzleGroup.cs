using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Zenject;

public class PuzzleGroup : MonoBehaviour
{
    [SerializeField] private List<BasePuzzleObjective> _objectiveList;
    [SerializeField] private List<PuzzleDoor> _doorList;
    [SerializeField] private PuzzleCastPoint _puzzleCastPoint;
    [SerializeField] private Collider2D _puzzleZone;
    private bool _isComplete = false;
    private bool _isLocked = false;

    private void OnEnable()
    {
        BasePuzzleObjective.OnStateChanged += CheckComplete;
        //PuzzleCastPoint.OnCast += OnPuzzleCast;
    }
    private void OnDisable()
    {
        BasePuzzleObjective.OnStateChanged -= CheckComplete;
        //PuzzleCastPoint.OnCast -= OnPuzzleCast;
    }
    private void Start()
    {
        if (_puzzleCastPoint != null)
        {
            SetPuzzleLocked(true);
        }
    }

    /// <summary>
    /// Called when player casts spell from point
    /// </summary>
    public void OnPuzzleCast()
    {
        BulletManager.Instance.DestroyAllPlayerBullets();
        ResetPuzzle(false);
    }

    /// <summary>
    /// Called on spell cast from point and world reset
    /// </summary>
    public void ResetPuzzle(bool locked = false)
    {
        if (_isComplete) return;
        if (_puzzleCastPoint == null) locked = false;

        foreach (BasePuzzleObjective obj in _objectiveList)
        {
            obj.Reset();
        }

        SetPuzzleLocked(locked);
    }

    public void LockPuzzle()
    {
        if (_puzzleCastPoint == null) return;
        SetPuzzleLocked(true);
    }
    // locks all incomplete objectives, you can complete them after lock reset.
    private void SetPuzzleLocked(bool isLocked)
    {
        if (_isLocked == isLocked) return;

        _isLocked = isLocked;
        foreach (BasePuzzleObjective obj in _objectiveList)
        {
            obj.SetLocked(isLocked);
        }
    }
    private void CheckComplete()
    {
        foreach (BasePuzzleObjective obj in _objectiveList)
        {
            if (obj.IsCompleted() == false)
            {
                SetCompleteState(false);
                return;
            }
        }
        SetCompleteState(true);
    }

    private void SetCompleteState(bool isComplete)
    {
        if (_isComplete == isComplete) return;
        _isComplete = isComplete;
        
        foreach (PuzzleDoor door in _doorList)
        {
            door.SetOpened(isComplete);
        }
    }
    public PuzzleCastPoint GetCastPoint()
    {
        return _puzzleCastPoint;
    }
}
