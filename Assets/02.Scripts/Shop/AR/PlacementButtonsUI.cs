using System;
using UnityEngine;
using UnityEngine.UI;

public class PlacementButtonsUI : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    [SerializeField] Button _cancelButton;
    [SerializeField] Button _applyButton;

    private Action _OnCancel;
    private Action _OnApply;

    public bool PanelVisible { 
        get { return _panel.activeSelf; } 
        set { _panel.SetActive(value); }
    }
    public bool ApplyInteractable { 
        get { return _applyButton.interactable; }
        set { _applyButton.interactable = value; } 
    }

    public Action OnCancel
    {
        set { 
            _OnCancel = value;
            _cancelButton.onClick.AddListener(() => _OnCancel.Invoke());
        }
    }

    public Action OnApply
    {
        set
        {
            _OnApply = value;
            _applyButton.onClick.AddListener(() => _OnApply.Invoke());
        }
    }
}

