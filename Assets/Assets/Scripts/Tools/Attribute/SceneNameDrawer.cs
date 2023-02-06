using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    private int sceneIndex = -1;
    private GUIContent[] sceneNames;

    private readonly string[] scenePathSplit = {"/", ".unity"};
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(EditorBuildSettings.scenes.Length==0) return;
        if(sceneIndex==-1)
            GetSceneNameArray(property);
        sceneIndex= EditorGUI.Popup(position, label, sceneIndex, sceneNames);
        property.stringValue = sceneNames[sceneIndex].text;
    }

    private void GetSceneNameArray( SerializedProperty property)
    {
        var scenes = EditorBuildSettings.scenes;
        sceneNames = new GUIContent[scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            string path = scenes[i].path;
            var splitPath = path.Split(scenePathSplit, StringSplitOptions.RemoveEmptyEntries);

            string sceneName = "";
            if (splitPath.Length > 0)
            {
                sceneName = splitPath[splitPath.Length - 1];
            }
            else
            {
                sceneName = "Deleted Scene";
            }

            sceneNames[i] = new GUIContent(sceneName);
        }

        if (sceneNames.Length == 0)
        {
            sceneNames = new[] {new GUIContent("check build settings")};
        }

        if (!string.IsNullOrEmpty(property.stringValue))
        {
            bool nameFound = false;
            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (sceneNames[i].text == property.stringValue)
                {
                    sceneIndex = i;
                    nameFound = true;
                    break;
                }
                
            }

            if (!nameFound)
                sceneIndex = 0;
        }
        else
        {
            sceneIndex = 0;
        }

        property.stringValue = sceneNames[sceneIndex].text;

    }
}
