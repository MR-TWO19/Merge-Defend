using System.Collections.Generic;

namespace TwoCore
{
    public class Profiles : SingletonScriptObject<Profiles>
    {
        public List<ProfileConfig> profiles = new List<ProfileConfig>();
    }
}