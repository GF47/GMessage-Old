using System;

namespace GFramework
{
    public class CommonService : IService
    {
        public Delegate Call { get; }

        public CommonService(Delegate @delegate)
        {
            Call = @delegate;
        }
    }
}
