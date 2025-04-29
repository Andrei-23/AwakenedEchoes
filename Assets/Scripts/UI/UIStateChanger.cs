using UnityEngine;
using Zenject;

public class UIStateChanger : MonoBehaviour
{
    [Inject] private UIManager _UIManager;

    public void OpenPause()
    {
        _UIManager.SetState(UIManager.State.Pause);
    }
    public void OpenMap()
    {
        _UIManager.SetState(UIManager.State.Map);
    }
    public void OpenSpell()
    {
        _UIManager.SetState(UIManager.State.SpellMenu);
    }
    public void OpenGame()
    {
        _UIManager.SetState(UIManager.State.Game);
    }
}
