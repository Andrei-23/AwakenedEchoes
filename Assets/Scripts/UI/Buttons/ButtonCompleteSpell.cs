using UnityEngine;

public class ButtonCompleteSpell : MonoBehaviour
{
    [SerializeField] private SpellCompiler _spellCompiler;
    public void Activate()
    {
        _spellCompiler.TrySaveSpell();
    }
}
