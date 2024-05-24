using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField]
    private GameObject KeyboardPrompt = null;
    [SerializeField]
    private GameObject GamepadPrompt = null;

    //public enum Controller

    public void DisplayPrompt()
    {
        KeyboardPrompt.gameObject.SetActive(true);
        GamepadPrompt.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        KeyboardPrompt.gameObject.SetActive(false);
        GamepadPrompt.gameObject.SetActive(false);
    }
}
