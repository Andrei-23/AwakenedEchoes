using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public enum State // current UI panel
    {
        Game = 0,
        Pause,
        SpellInGame,
        SpellMenu,
        Map,
    }

    public State CurrentState {  get; private set; }

    private InputActionMap _playerActionMap;
    private InputActionMap _UIActionMap;
    private InputActionMap _spellWriteActionMap;
    private InputActionMap _menuNavigationActionMap;

    [SerializeField] private GameObject _panelPause;
    //[SerializeField] private GameObject _panelSpellInGame;
    [SerializeField] private GameObject _panelSpell;
    [SerializeField] private GameObject _panelMap;

    [SerializeField] private PlayerInput _playerInput;
    private InputAction _actionPause;
    private InputAction _actionSpellMenu;
    private InputAction _actionOpenMap;

    private void Awake()
    {
        _actionPause = _playerInput.actions["Pause"];
        _actionSpellMenu = _playerInput.actions["SpellMenu"];
        _actionOpenMap = _playerInput.actions["Map"];

        _playerActionMap = _playerInput.actions.FindActionMap("Player");
        _UIActionMap = _playerInput.actions.FindActionMap("UI");
        _spellWriteActionMap = _playerInput.actions.FindActionMap("SpellWrite");
        _menuNavigationActionMap = _playerInput.actions.FindActionMap("MenuNavigation");

        CurrentState = State.Game;

        UpdateActionMaps();

        //SetActionMapEnabled(_playerActionMap, true);
        //SetActionMapEnabled(_menuNavigationActionMap, true);
        //SetActionMapEnabled(_spellWriteActionMap, false);
    }

    private void Update()
    {
        UpdateActionMaps();
    }

    private void OnEnable()
    {
        _actionPause.performed += TryPause;
        _actionSpellMenu.performed += TryOpenSpellMenu;
        _actionOpenMap.performed += TryOpenMap;
    }
    private void OnDisable()
    {
        _actionPause.performed -= TryPause;
        _actionSpellMenu.performed -= TryOpenSpellMenu;
        _actionOpenMap.performed -= TryOpenMap;
    }

    private void TryPause(InputAction.CallbackContext context)
    {
        if (CurrentState == State.Game)
            SetState(State.Pause);
        else if (CurrentState == State.Pause)
            SetState(State.Game);
        else // other menues
            SetState(State.Game);
    }

    private void TryOpenMap(InputAction.CallbackContext context)
    {
        if (CurrentState == State.Map) SetState(State.Game);
        else SetState(State.Map);
    }
    private void TryOpenSpellMenu(InputAction.CallbackContext context)
    {
        if(CurrentState == State.SpellMenu) SetState(State.Game);
        else SetState(State.SpellMenu);
    }

    public void SetState(State newState)
    {
        if (newState == CurrentState) return;
        if (CurrentState == State.Pause && newState != State.Game) return;

        CurrentState = newState;
        UpdateUIPanel();
        UpdateActionMaps();
        // set ui navigation controls
        // event?
    }

    private void SetActionMapEnabled(InputActionMap map, bool enabled)
    {
        //if (_spellWriteActionMap.enabled == enabled)
        //    return;
        if (enabled) map.Enable();
        else map.Disable();

        //Debug.Log(map.ToString() + " action map " + (enabled ? "enabled" : "disabled"));
    }
    public void SetEnableSpellWriting(bool enabled)
    {
        SetActionMapEnabled(_spellWriteActionMap, enabled);
        SetActionMapEnabled(_menuNavigationActionMap, !enabled);
    }

    private void UpdateActionMaps()
    {
        SetActionMapEnabled(_playerActionMap, CurrentState == State.Game);
        SetActionMapEnabled(_spellWriteActionMap, CurrentState == State.SpellMenu);
        SetActionMapEnabled(_menuNavigationActionMap, CurrentState != State.Pause);
        //SetActionMapEnabled(_menuNavigationActionMap,
            //CurrentState != State.Pause && CurrentState != State.SpellMenu);
    }

    private void UpdateUIPanel()
    {
        _panelPause.SetActive(CurrentState == State.Pause);
        _panelSpell.SetActive(CurrentState == State.SpellMenu);
        //_panelSpellInGame.SetActive(CurrentState == State.SpellInGame);
        _panelMap.SetActive(CurrentState == State.Map);
    }
}
