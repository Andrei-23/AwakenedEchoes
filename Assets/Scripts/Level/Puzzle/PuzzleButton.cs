using UnityEngine;
using System;

public class PuzzleButton : BasePuzzleObjective
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private LayerMask _activationMask;

    [Header("Colors")]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _completeColor;
    [SerializeField] private Color _lockedColor;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _activationMask.value) == 0) return;

        SetComplete();
        UpdateSprite();
    }
    private void UpdateSprite()
    {
        Color newColor = _defaultColor;
        if (completed) newColor = _completeColor;
        else if (locked) newColor = _lockedColor;

        _spriteRenderer.color = newColor;
    }

    public override void Reset()
    {
        base.Reset();
        UpdateSprite();
    }

    public override void SetLocked(bool isLocked)
    {
        base.SetLocked(isLocked);
        UpdateSprite();
    }
}
