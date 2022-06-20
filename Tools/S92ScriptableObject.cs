using System;
using Sirenix.OdinInspector;
using Unity.Collections;

namespace Status92.Tools
{
    public class S92ScriptableObject : SerializedScriptableObject
    {

        [ReadOnly]
        public Guid Guid = Guid.Empty;

        protected virtual void Awake()
        {
            if (Equals(Guid, Guid.Empty))
            {
                Guid = Guid.NewGuid();
            }
        }

    }
}
