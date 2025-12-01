using System;
using Godot;

public partial class PlayerStatInt : Resource
{
    [Signal]
    public delegate void OnValueChangedEventHandler(int value);

    private StatHelper<int> helper;

    public PlayerStatInt(int initialValue)
    {
        helper = new StatHelper<int>(initialValue, v => EmitSignal(SignalName.OnValueChanged, v));
    }

    public int Value
    {
        get => helper.Value;
        set => helper.Value = value;
    }

    public PlayerStatInt SetMin(int min)
    {
        helper.SetMin(min);
        return this;
    }

    public PlayerStatInt SetMax(int max)
    {
        helper.SetMax(max);
        return this;
    }

    public PlayerStatInt OnMin(Action<int> action)
    {
        helper.SetOnMin(action);
        return this;
    }

    public PlayerStatInt OnMax(Action<int> action)
    {
        helper.SetOnMax(action);
        return this;
    }

    public void Add(int amount) => Value += amount;
    public void Subtract(int amount) => Value -= amount;
    public void Multiply(int amount) => Value *= amount;
    public void Divide(int amount) => Value /= amount;
}