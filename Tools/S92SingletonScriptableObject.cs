using System.IO;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Status92.Tools
{
    public class S92SingletonScriptableObject<T> : S92ScriptableObject where T : S92ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                _instance ??= Resources.Load<T>(typeof(T).GetNiceName());
                
#if UNITY_EDITOR
                if (_instance == null)
                {
                    var guids = AssetDatabase.FindAssets($"t:{typeof(T).GetNiceName()}");
                    if (guids.Length < 1) return _instance;
                    var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _instance = Resources.Load<T>(Path.GetFileNameWithoutExtension(assetPath));
                }
#endif
                return _instance;
            }
        }
    }
}