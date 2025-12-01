using System;
using Godot;

public partial class PlayerStatFloat : Resource
{
    [Signal]
    public delegate void OnValueChangedEventHandler(float value);

    private StatHelper<float> helper;

    public PlayerStatFloat(float initialValue)
    {
        helper = new StatHelper<float>(initialValue, v => EmitSignal(SignalName.OnValueChanged, v));
        SetMin(0);
    }

    public float Value
    {
        get => helper.Value;
        set => helper.Value = value;
    }

    public PlayerStatFloat SetMin(float min)
    {
        helper.SetMin(min);
        return this;
    }

    public PlayerStatFloat SetMax(float max)
    {
        helper.SetMax(max);
        return this;
    }

    public PlayerStatFloat OnMin(Action<float> action)
    {
        helper.SetOnMin(action);
        return this;
    }

    public PlayerStatFloat OnMax(Action<float> action)
    {
        helper.SetOnMax(action);
        return this;
    }

    public void Add(float amount) => Value += amount;
    public void Subtract(float amount) => Value -= amount;
    public void Multiply(float amount) => Value *= amount;
    public void Divide(float amount) => Value /= amount;
}
