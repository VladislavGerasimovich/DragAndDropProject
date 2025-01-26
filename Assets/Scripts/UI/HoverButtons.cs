using System.Collections.Generic;
using UnityEngine;

public class HoverButtons : MonoBehaviour
{
    [SerializeField] private List<HoverButton> _buttons;

    public void SetActive(bool value)
    {
        foreach (var button in _buttons)
        {
            button.SetActive(value);
        }
    }
}