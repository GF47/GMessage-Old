using System;
using System.Collections.Generic;

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

        /// <summary>
        /// 对输入值进行预处理
        /// </summary>
        public Func<T, T> Pretreating;

        private Binder _binder;

        private T _value;

        /// <summary>
        /// 关联的值，变动时，自动触发 ValueChanging
        /// </summary>
        public T Value
        {
            get { return _value; }
            set
            {
                if (SetValueWithoutNotify(value))
                {
                    _binder?.OnValueChanging(this, _value);
                }
            }
        }

        public bool SetValueWithoutNotify(T nv)
        {
            T valueTreated = Pretreating == null ? nv : Pretreating(nv);
            if (!Equals(_value, valueTreated))
            {
                T old = _value;
                _value = valueTreated;
                ValueChanging?.Invoke(old, _value);
                return true;
            }
#if UNITY_EDITOR
            else
            {
                UnityEngine.Debug.LogWarning($"{this} receive invalid value, {nv} is the same as {_value}");
                return false;
            }
#endif
        }

        /// <summary>
        /// 与指定属性进行双向绑定
        /// </summary>
        /// <param name="b">指定属性</param>
        public void Bind(BindableProperty<T> b) { Bind(this, b); }

        /// <summary>
        /// 解除双向绑定
        /// </summary>
        public void Unbind() { Unbind(this); }

        /// <summary>
        /// 将两个属性进行双向绑定
        /// </summary>
        public static void Bind(BindableProperty<T> a, BindableProperty<T> b)
        {
            var binder = a._binder ?? b._binder ?? new Binder();

            binder.Bind(a);
            binder.Bind(b);
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        public static void Unbind(BindableProperty<T> property)
        {
            var binder = property._binder;
            if (binder != null && binder.IsBinding(property))
            {
                binder.Unbind(property);
            }
        }

        public class Binder
        {
            private List<BindableProperty<T>> _properties;

            public Binder(params BindableProperty<T>[] properties)
            {
                _properties = new List<BindableProperty<T>>(properties);

                foreach (var item in _properties)
                {
                    Bind(item);
                }
            }

            ~Binder()
            {
                foreach (var item in _properties)
                {
                    Unbind(item);
                }
            }

            public void Bind(BindableProperty<T> property)
            {
                if (!_properties.Contains(property)) 
                {
                    _properties.Add(property);
                    if (property._binder != this) { property._binder = this; }
                }
            }

            public void Unbind(BindableProperty<T> property)
            {
                property._binder = null;
                _properties.Remove(property);
            }

            public bool IsBinding(BindableProperty<T> property) 
            {
                return property._binder == this && _properties.Contains(property);
            }

            public void OnValueChanging(BindableProperty<T> property, T nv)
            {
                foreach (var item in _properties)
                {
                    if (item != property)
                    {
                        item.SetValueWithoutNotify(nv);
                    }
                }
            }
        }
    }
}
