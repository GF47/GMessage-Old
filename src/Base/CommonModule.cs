using System.Collections.Generic;

namespace GFramework
{
    public class CommonModule : Module
    {
        #region Singleton
        public static CommonModule Instance(int id)
        {
            if (_instances.TryGetValue(id, out var result))
            {
                return result;
            }
            else
            {
                return new CommonModule(id);
            }
        }
        private static readonly Dictionary<int, CommonModule> _instances = new Dictionary<int, CommonModule>();
        #endregion Singleton

        public int ID { get; }

        public CommonModule(int id) : base()
        {
            ID = id;
            _instances.Add(id, this);
        }

        ~CommonModule()
        {
            _instances.Remove(ID);
        }
    }
}
