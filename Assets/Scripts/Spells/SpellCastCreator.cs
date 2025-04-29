using System.ComponentModel;
using UnityEngine;
using Zenject;

/// <summary>
/// Instantiates spellcasts
/// </summary>
public class SpellCastCreator : MonoBehaviour
{
    public static SpellCastCreator Instance { get; private set; }

    [SerializeField] private GameObject _spellCastPrefab;
    [SerializeField] private GameObject _folder;

    [Inject] private DiContainer _container;
    private void Awake()
    {
        Instance = this;
    }

    public SpellCast CreateSpellCast()
    {
        GameObject go = _container.InstantiatePrefab(_spellCastPrefab, _folder.transform);
        return go.GetComponent<SpellCast>();
    }

    public void DestroyAllSpellCasts()
    {
        for (int i = _folder.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = _folder.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
