using System;

public class StatHelper<T> where T : struct, IComparable<T>
{
    protected T value;
    protected T minValue;
    protected T maxValue;
    protected bool hasMin;
    protected bool hasMax;
    protected Action<T> onReachMin;
    protected Action<T> onReachMax;
    protected Action<T> onValueChanged;

    public StatHelper(T initialValue, Action<T> onValueChangedCallback)
    {
        value = initialValue;
        onValueChanged = onValueChangedCallback;
        hasMin = false;
        hasMax = false;
    }

    public T Value
    {
        get => value;
        set
        {
            T newValue = value;

            if (hasMin && newValue.CompareTo(minValue) < 0)
            {
                newValue = minValue;
                onReachMin?.Invoke(newValue);
            }

            if (hasMax && newValue.CompareTo(maxValue) > 0)
            {
                newValue = maxValue;
                onReachMax?.Invoke(newValue);
            }

            if (this.value.Equals(newValue)) return;

            this.value = newValue;
            onValueChanged?.Invoke(newValue);
        }
    }

    public void SetMin(T min)
    {
        minValue = min;
        hasMin = true;
        if (Value.CompareTo(minValue) < 0)
        {
            Value = minValue;
        }
    }

    public void SetMax(T max)
    {
        maxValue = max;
        hasMax = true;
        if (Value.CompareTo(maxValue) > 0)
        {
            Value = maxValue;
        }
    }

    public void SetOnMin(Action<T> action) => onReachMin = action;
    public void SetOnMax(Action<T> action) => onReachMax = action;
}
