using System;
using UnityEngine;

public delegate void voidDelegate();

[CreateAssetMenu]
public class BoolVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    private bool value;
    public bool Value
    {
        get { return value; }
        set
        {
            if (value != this.value)
            {
                this.value = value;
                onToggle?.Invoke();
            }
        }
    }

    public event voidDelegate onToggle;

    public void SetValue(bool value)
    {
        Value = value;
    }

    public void SetValue(BoolVariable value)
    {
        SetValue(value.Value);
    }

    public static implicit operator bool(BoolVariable variable)
    {
        return variable.Value;
    }
}
