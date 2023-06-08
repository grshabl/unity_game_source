using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    private TabButton _defaultSelectedTab;



    private TabButton _curentSelected;
    private TabButton _previousSelected;
    private Dictionary<TabButton, GameObject> _tabDictionary = new Dictionary<TabButton, GameObject>();


    public TabButton Selected => _curentSelected;
    public TabButton DefaultSelected => _defaultSelectedTab;

    public Action<TabButton, GameObject> OnTabChanged;

    private void Start()
    {
        if (_defaultSelectedTab != null)
            OnTabSelected(_defaultSelectedTab);
    }

    public void Subscribe(TabButton button, GameObject correspondingObject)
    {
        if (_tabDictionary.ContainsKey(button))
            throw new ArgumentOutOfRangeException("Tried to subscribe already existing TabButton");


        _tabDictionary.Add(button, correspondingObject);
    }

    public void OnTabSelected(TabButton button)
    {
        if (button == null)
            throw new ArgumentNullException("Tried to select null TabButton");

        if (!_tabDictionary.ContainsKey(button))
            throw new ArgumentException("Tried to select non-subscribed TabButton");

        if (button == _curentSelected || !button.interactable)
            return;

        _curentSelected = button;
        _curentSelected.SetButtonState(true);

        _tabDictionary[_curentSelected].SetActive(true);

        if (_previousSelected != null)
        {
            _previousSelected.SetButtonState(false);
            _tabDictionary[_previousSelected].SetActive(false);
        }

        _previousSelected = _curentSelected;

        OnTabChanged?.Invoke(button, _tabDictionary[_curentSelected]);
    }
}
