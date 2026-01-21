using System;

namespace TwoCore
{
    [Serializable]
    public abstract class BaseDefinition : BaseData
    {
        protected BaseDefinition() { }

        public BaseDefinition(string name) : base(name) { }
    }
}