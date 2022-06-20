using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Status92.Tools.Editor
{
    [InitializeOnLoad]
    public static class OnSceneSavedTrigger
    {
        static OnSceneSavedTrigger()
        {
            EditorSceneManager.sceneSaving += (scene, path) =>
            {
                var info = new SceneInfo(scene.name);

                IEnumerable<dynamic> FindTargets(Type classType)
                {
                    if (typeof(MonoBehaviour).IsAssignableFrom(classType))
                    {
                        return scene.GetAllComponentsInScene(classType);
                    }
                    else if (classType.ImplementsOpenGenericClass(typeof(S92SingletonScriptableObject<>)))
                    {
                        var result = Resources.FindObjectsOfTypeAll(classType);
                        if (result.Length > 0) return result;

                        var guids = AssetDatabase.FindAssets($"t:{classType.GetNiceName()}");

                        Debug.Log($"Found {guids.Length} GUIDs. First: {guids.FirstOrDefault()}");

                        return guids.Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(
                            AssetDatabase.GUIDToAssetPath(guids[0])
                        ));
                    }

                    return Array.Empty<dynamic>();
                }

                dynamic FindTarget(Type classType)
                {
                    if (typeof(MonoBehaviour).IsAssignableFrom(classType))
                    {
                        return scene.GetAllComponentsInScene(classType).FirstOrDefault();
                    }
                    else if (classType.IsArray)
                    {
                        var components = scene
                            .GetAllComponentsInScene(classType.GetElementType())
                            .ToArray();
                        var final = Array.CreateInstance(classType.GetElementType(), components.Length);
                        Array.Copy(components, final, final.Length);
                        return final;
                    }
                    else if (classType.ImplementsOpenGenericClass(typeof(S92SingletonScriptableObject<>)))
                    {
                        var result = Resources.FindObjectsOfTypeAll(classType).FirstOrDefault();
                        if (result != null) return result;

                        var guids = AssetDatabase.FindAssets($"t:{classType.GetNiceName()}");
                        if (guids.Length < 1) return result;

                        var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                        var resource = Resources.Load(Path.GetFileNameWithoutExtension(assetPath));
                        if (resource != null) return resource;
                        return AssetDatabase.LoadAssetAtPath<ScriptableObject>(
                            AssetDatabase.GUIDToAssetPath(guids[0])
                        );
                    }

                    return null;
                }

                scene.GetAllComponentsInScene<OnSceneSavedHandler>().ForEach(component =>
                {
                    component.OnSceneSaved(info);
                });

                new[]
                    {
                        typeof(OnSceneSavedHandler<>),
                        typeof(OnSceneSavedHandler<,>),
                        typeof(OnSceneSavedHandler<,,>),
                        typeof(OnSceneSavedHandler<,,,>),
                        typeof(OnSceneSavedHandler<,,,,>),
                        typeof(OnSceneSavedHandler<,,,,,>),
                        typeof(OnSceneSavedHandler<,,,,,,>),
                        typeof(OnSceneSavedHandler<,,,,,,,>),
                    }
                    .SelectMany(openType =>
                        TypeUtils
                            .GetTypesForOpenGenericInterface(openType)
                            .Where(type => type.IsClass)
                            .SelectMany(classType =>
                                FindTargets(classType).Select(target =>
                                (
                                    target,
                                    classType.GetMethod("OnSceneSaved"),
                                    classType.GetArgumentsOfInheritedOpenGenericInterface(openType)
                                        .Select(FindTarget)
                                        .Prepend(info)
                                        .ToArray()
                                ))))
                    .ForEach(tuple =>
                    {
                        var (target, method, arguments) = tuple;
                        if (target == null) return;
                        try
                        {
                            method.Invoke(target, arguments);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    });
            };
        }
    }
}