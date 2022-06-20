using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Status92.Tools.Editor
{
    [InitializeOnLoad]
    public static class RequiredByComponentTrigger
    {
        private static List<(Type, Type)> RequiredTypes = new();

        static RequiredByComponentTrigger()
        {
            FindRequiredTypes();

            EditorSceneManager.sceneDirtied += EnsureRequiredComponents;
            EditorSceneManager.sceneOpened += (scene, mode) => EnsureRequiredComponents(scene);
            EditorSceneManager.sceneSaving += (scene, path) => EnsureRequiredComponents(scene);
        }

        static void EnsureRequiredComponents(Scene scene)
        {
            foreach (var (componentType, requiredType) in RequiredTypes)
            {
                scene
                    .GetRootGameObjects()
                    .SelectMany(root => root.GetComponentsInChildren(componentType))
                    .Where(component => component.GetComponent(requiredType) == null)
                    .ForEach(component => { component.gameObject.AddComponent(requiredType); });
            }
        }

        static void FindRequiredTypes()
        {
            RequiredTypes = TypeUtils
                .GetAttributeInstances<RequiredByComponentAttribute>()
                .Where(pair =>
                    typeof(MonoBehaviour).IsAssignableFrom(pair.Item2))
                .Select(pair => (pair.Item1.RequiredBy, pair.Item2))
                .ToList();
        }
    }
}