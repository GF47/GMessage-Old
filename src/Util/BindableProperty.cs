using System;

namespace GMessage
{
    /// <summary>
    /// 关联属性，当值 Value 改变时自动触发 ValueChanging
    /// </summary>
    /// <typeparam name="T">Value的类型</typeparam>
    public class BindableProperty<T>
    {
        /// <summary>
        /// 当值 Value 改变时自动触发 ValueChanging，第一个参数为旧值，第二个参数为新值
        /// </summary>
        public Action<T, T> ValueChanging;

        private T _value;

        /// <summary>
        /// 关联的值，变动时，自动触发 ValueChanging
        /// </summary>
        public T Value
        {
            get { return _value; }
            set
            {
                if (!Equals(_value, value))
                {
                    T old = _value;
                    _value = value;
                    ValueChanging?.Invoke(old, _value);
                }
            }
        }
    }
}