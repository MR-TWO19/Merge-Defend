using UnityEngine.Scripting;

namespace TwoCore
{
    [Preserve]
    public class UserData : BaseUserData
    {
        public static UserData Ins => LocalData.Get<UserData>();
        public static string UserId;

    }
}