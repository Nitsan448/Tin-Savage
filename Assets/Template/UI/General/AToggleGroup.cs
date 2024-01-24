using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AToggleGroup : MonoBehaviour
{
    [SerializeField] protected List<Toggle> _toggles;

    protected ToggleGroup _toggleGroup;

    protected abstract void OnToggleValueChanged(Toggle toggle, int toggleIndex);

    protected abstract int GetStartingToggleIndex();

    private void Start()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
        for (int i = 0; i < _toggles.Count; i++)
        {
            int temp = i;
            _toggles[i].onValueChanged.AddListener(delegate { OnToggleValueChanged(_toggles[temp], temp); });
        }
        SetStartingToggle();
    }

    private void SetStartingToggle()
	{
        int startingToggleIndex = GetStartingToggleIndex();
        for (int i = 0; i < _toggles.Count; i++)
        {
            if (startingToggleIndex == i)
            {
                _toggles[i].isOn = true;
            }
        }
    }
}
