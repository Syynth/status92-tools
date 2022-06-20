using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Status92.Tools
{
    public static class SceneExtensions
    {
        
        public static IEnumerable<Component> GetAllComponentsInScene(this Scene scene, Type type)
        {
            return scene
                .GetRootGameObjects()
                .SelectMany(root =>
                    root.GetComponentsInChildren(type));
        }

        public static IEnumerable<T> GetAllComponentsInScene<T>(this Scene scene)
        {
            return scene
                .GetRootGameObjects()
                .SelectMany(root =>
                    root.GetComponentsInChildren<T>());
        }

    }
}