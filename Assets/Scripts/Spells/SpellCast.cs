using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpellCast : MonoBehaviour
{
    public enum SelectMode // modes of select action/function
    {
        Previous = 0,
        All = 1,
        Counted = 2,
    }

    // where action activates from
    [HideInInspector] public ActionTargetType targetType;
    [HideInInspector] public List<GameObject> targets;

    [HideInInspector] public List<GameObject> lastBullets; // bullets from last shoot spell
    [HideInInspector] public List<GameObject> allBullets; // bullets from this spell

    private GameObject _castObject; // what casted the spell

    private Stack<BaseAction> _actions; // stack of all actions in spell, can insert more actions

    public void Activate(Spell spell)
    {
        StartCoroutine(ActivateIE(spell));
    }

    private IEnumerator ActivateIE(Spell spell)
    {
        SelectTarget(ActionTargetType.Caster);
        
        _actions = new Stack<BaseAction> ();
        AddActionInList(spell.actions);

        while(_actions.Count > 0)
        {
            BaseAction action = _actions.Pop();
            if (action.duration == 0) ActivateActionInstant(action);
            else yield return ActivateActionDelay(action);
        }

        DestroyCaster();
    }

    public void AddActionInList(BaseAction action)
    {
        _actions.Push(action);
    }
    public void AddActionInList(List<BaseAction> actions)
    {
        for (int i = actions.Count - 1; i >= 0; i--)
        {
            AddActionInList(actions[i]);
        }
    }

    // bad code lol
    public void ActivateActionInstant(BaseAction action)
    {
        if(action.duration != 0f) { Debug.LogWarning("Incorrect duration"); }

        action.Initiate(this);
        List<GameObject> targetsCopy = new List<GameObject>(targets);


        // initiate all, activate, finish, final
        for (int i = 0; i <= 4; i++)
        {
            if (i == 0)
            {
                //action.Initiate(this);
                continue;
            }
            if (i == 2)
            {
                // no delay
                continue;
            }
            if (i == 4)
            {
                action.Final(this);
                continue;
            }
            foreach (GameObject target in targetsCopy)
            {
                CastData castData = new CastData(targetType, target, this);

                if (target == null) { continue; }
                if (action.IsCorrectTarget(castData) == false) { continue; }

                if (i == 1)
                {
                    action.Activate(castData);
                }
                if (i == 3)
                {
                    action.Finish(castData);
                }
                if (action.castOnce) { break; }
            }
        }
    }
    public IEnumerator ActivateActionDelay(BaseAction action)
    {
        if (action.duration == 0f) { Debug.LogWarning("Incorrect duration"); }

        action.Initiate(this);
        List<GameObject> targetsCopy = new List<GameObject>(targets);

        // initiate all, activate, wait, finish, final
        for(int i = 0; i <= 4; i++)
        {
            if (i == 0)
            {
                //action.Initiate(this);
                continue;
            }
            if (i == 2)
            {
                yield return new WaitForSeconds(action.duration);
                continue;
            }
            if(i == 4)
            {
                action.Final(this);
                continue;
            }
            foreach (GameObject target in targetsCopy)
            {
                CastData castData = new CastData(targetType, target, this);

                if (target == null) { continue; }
                if (action.IsCorrectTarget(castData) == false) { continue; }

                if(i == 1)
                {
                    action.Activate(castData);
                }
                if(i == 3)
                {
                    action.Finish(castData);
                }
                if (action.castOnce) { break; }
            }
        }
        yield break;
    }

    /// <summary>
    /// Selects last cnt created objects as targets.
    /// </summary>
    /// <param name="targetType">Type of target to cast action on.</param>
    /// <param name="selectCount">Count of targets. If its -1, select all.</param>
    public void SelectTarget(ActionTargetType targetType, SelectMode mode = SelectMode.Previous, int selectCount = 1)
    {
        this.targetType = targetType;
        if(targetType == ActionTargetType.Caster)
        {
            targets = new List<GameObject> { _castObject };
        }
        if(targetType == ActionTargetType.Bullet)
        {
            if(mode == SelectMode.Previous)
            {
                targets = lastBullets;
            }
            if(mode == SelectMode.Counted)
            {
                SelectLastKBullets(selectCount);
            }
            if(mode == SelectMode.All)
            {
                targets = allBullets;
            }
        }
    }

    public void SelectLastKBullets(int k)
    {
        if(k < 0) { return; }

        targetType = ActionTargetType.Bullet;
        targets = new List<GameObject>();
        for (int i = Mathf.Max(allBullets.Count - k, 0); i < k; i++)
        {
            targets.Add(allBullets[i]);
        }
    }
    private void DestroyCaster()
    {
        Destroy(gameObject);
    }
    public GameObject GetCastObject()
    {
        return _castObject;
    }
    public void SetCastObject(GameObject obj)
    {
        if(obj == null)
        {
            Debug.LogError("No cast object passed");
            //obj = Player.Instance.gameObject;
        }
        _castObject = obj;
    }
}
