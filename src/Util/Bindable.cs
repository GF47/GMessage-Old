using System;

namespace GMessage
{
    public class Bindable<T>
    {
        private Bindable<T> _left;
        private Bindable<T> _right;

        private T _content;

        /// <summary> 当内容改变时触发，第一个为新值，第二个为旧值 </summary>
        public event Action<T, T> Changing;

        /// <summary> 修改内容前对其进行预处理 </summary>
        public Func<T, T> Preprocessing;

        /// <summary> 所包含内容 </summary>
        public T Content
        {
            get => _content; set
            {
                if (SetContentWithoutNotify(value))
                {
                    NotifyChange();
                }
            }
        }

        /// <summary> 修改内容但不向所绑定的其他对象发送通知 </summary>
        /// <param name="newContent">新内容</param>
        /// <returns>修改成功</returns>
        public bool SetContentWithoutNotify(T newContent)
        {
            T ppnc = Preprocessing == null ? newContent : Preprocessing(newContent);
            if (!Equals(_content, ppnc))
            {
                T oc = _content;
                _content = ppnc;
                Changing?.Invoke(_content, oc);
                return true;
            }
            return false;
        }

        /// <summary> 强制修改内容并进行通知 </summary>
        /// <param name="newContent">新内容</param>
        public void SetContentByForce(T newContent)
        {
            T ppnc = Preprocessing == null ? newContent : Preprocessing(newContent);

            T oc = _content;
            _content = ppnc;
            Changing?.Invoke(_content, oc);

            NotifyChange();
        }

        private void NotifyChange()
        {
            ForLeft(this, NotifyAction);
            ForRight(this, NotifyAction);
        }

        private bool NotifyAction(Bindable<T> bindable)
        {
            bindable.SetContentWithoutNotify(_content);
            return true; // 返回值为是否继续遍历
        }

        /// <summary> 与指定对象进行双向绑定 </summary>
        /// <param name="that">指定对象</param>
        public void Bind(Bindable<T> that)
        {
            Bind(this, that);
        }

        /// <summary> 对自身解除绑定 </summary>
        public void Unbind()
        {
            Unbind(this);
        }

        public override string ToString() => _content.ToString();

        /// <summary> 判断两个对象是否已经绑定 </summary>
        public static bool InTheSameBundle(Bindable<T> left, Bindable<T> right)
        {
            var result = false;

            ForEach(left, bindable =>
            {
                if (bindable == right) { result = true; }
                return !result;
            });

            return result;
        }

        /// <summary> 将两个对象进行绑定，对象本身已经绑定的其他对象也会连带绑定 </summary>
        public static void Bind(Bindable<T> left, Bindable<T> right)
        {
            if (InTheSameBundle(left, right)) { return; }

            var rightEnd = GetRightEnd(left);
            var leftEnd = GetLeftEnd(right);
            rightEnd._right = leftEnd;
            leftEnd._left = rightEnd;
        }

        /// <summary> 解除自身的绑定 </summary>
        public static void Unbind(Bindable<T> bindable)
        {
            var left = bindable._left;
            var right = bindable._right;

            if (left != null) { left._right = right; }
            if (right != null) { right._left = left; }

            bindable._left = null;
            bindable._right = null;
        }

        /// <summary> 对指定对象的左方节点进行遍历操作，操作返回值的意义为是否继续进行遍历 </summary>
        public static void ForLeft(Bindable<T> bindable, Func<Bindable<T>, bool> action)
        {
            if (action == null) { return; }

            var n = bindable._left;
            while (n != null)
            {
                if (n == bindable || !action.Invoke(n)) { return; }

                n = n._left;
            }
        }

        /// <summary> 对指定对象的右方节点进行遍历操作，操作返回值的意义为是否继续进行遍历 </summary>
        public static void ForRight(Bindable<T> bindable, Func<Bindable<T>, bool> action)
        {
            if (action == null) { return; }

            var n = bindable._right;
            while (n != null)
            {
                if (n == bindable || !action.Invoke(n)) { return; }

                n = n._right;
            }
        }

        /// <summary> 对指定对象所绑定的所有节点进行遍历操作，操作返回值的意义为是否继续进行遍历 </summary>
        public static void ForEach(Bindable<T> bindable, Func<Bindable<T>, bool> action)
        {
            if (action == null || !action.Invoke(bindable)) { return; }

            var n = bindable._left;
            while (n != null)
            {
                if (n == bindable || !action.Invoke(n)) { return; }

                n = n._left;
            }

            n = bindable._right;
            while (n != null)
            {
                if (n == bindable || !action.Invoke(n)) { return; }

                n = n._right;
            }
        }

        public static Bindable<T> GetLeftEnd(Bindable<T> bindable)
        {
            var n = bindable;
            while (n._left != null)
            {
                if (n == bindable)
                {
                    return bindable._right; // 防止环形链表
                }
                n = n._left;
            }
            return n;
        }

        public static Bindable<T> GetRightEnd(Bindable<T> bindable)
        {
            var n = bindable;
            while (n._right != null)
            {
                if (n == bindable)
                {
                    return bindable._left; // 防止环形链表
                }
                n = n._right;
            }
            return n;
        }

        public static implicit operator T(Bindable<T> bindable) => bindable._content;
    }
}