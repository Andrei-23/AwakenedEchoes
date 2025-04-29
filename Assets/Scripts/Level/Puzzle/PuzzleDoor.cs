using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{
    //[SerializeField] private List<PuzzleButton> buttons;
    //private bool complete = false;
    
    //void Update()
    //{
    //    if (buttons == null || buttons.Count == 0) return;

    //    bool flag = true;
    //    foreach (var button in buttons)
    //    {
    //        if(button != null && button.activated == false) {
    //            flag = false;
    //            break;
    //        }
    //    }
    //    if(flag)
    //    {
    //        Open();
    //    }
    //}

    public void SetOpened(bool isOpened)
    {
        gameObject.SetActive(isOpened == false);
    }
}
