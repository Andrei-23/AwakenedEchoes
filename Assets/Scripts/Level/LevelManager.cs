using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{ 
    public static LevelManager Instance;
    public static event Action OnWorldReset;
    private List<PuzzleGroup> puzzleGroups;

    [Header("")]
    public Tilemap waterTilemap;

    private void Awake()
    {
        Instance = this;
        puzzleGroups = FindObjectsByType<PuzzleGroup>(FindObjectsSortMode.None).ToList();
    }

    public void WorldReset()
    {
        OnWorldReset?.Invoke();
        BulletManager.Instance.DestroyAllBullets();

        foreach (var puzzleGroup in puzzleGroups)
        {
            puzzleGroup.ResetPuzzle(true);
        }
    }
    public void LockAllPuzzles()
    {
        foreach (var puzzleGroup in puzzleGroups)
        {
            puzzleGroup.LockPuzzle();
        }
    }
    public PuzzleGroup FindActiveCastPointGroup(Vector2 playerPos)
    {
        foreach(var puzzle in puzzleGroups)
        {
            var point = puzzle.GetCastPoint();
            if (point != null && point.IsTriggered(playerPos))
            {
                return puzzle;
            }
        }
        return null;
    }
}
