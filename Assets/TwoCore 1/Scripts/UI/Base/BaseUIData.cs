using UnityEngine;

namespace TwoCore
{
    public abstract class BaseUIData<T> : MonoBehaviour
    {
        public abstract void SetData(T t);
    }
}