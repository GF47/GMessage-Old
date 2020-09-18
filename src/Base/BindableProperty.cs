using System;

namespace GFramework
{
    public class BindableProperty<T>
    {
        public Action<T, T> OnValueChanged;

        private T _value;

        public T Value
        {
            get { return _value; }
            set
            {
                if (!Equals(_value, Value))
                {
                    T old = _value;
                    _value = value;
                    ValueChanged(old, _value);
                }
            }
        }

        private void ValueChanged(T oldValue, T newValue)
        {
            OnValueChanged?.Invoke(oldValue, newValue);
        }
    }
}