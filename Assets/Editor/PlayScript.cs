using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using System.IO;

public class PlayScript
{
    [MenuItem("Government/PlayGame")]
    public static void OpenLoader()
    {
        string currentSceneName = "MainGameScene";
        File.WriteAllText(".lastScene", currentSceneName);
        EditorSceneManager.OpenScene($"{Directory.GetCurrentDirectory()}/Assets/_ADV/Scenes/LoaderScene.unity");
        EditorApplication.isPlaying = true;
    }

    [MenuItem("Government/LoadEditedScene")]
    public static void ReturnToLastScene()
    {
        string lastScene = File.ReadAllText(".lastScene");
        EditorSceneManager.OpenScene($"{Directory.GetCurrentDirectory()}/Assets/_ADV/Scenes/{lastScene}.unity");
    }

    [MenuItem("Government/Tests/Delete Saves")]
    public static void ClearAllData()
    {
        var path = Application.persistentDataPath;
        var files = Directory.GetFiles(path);

        foreach (var fileName in files)
        {
            if (fileName.Contains("_ADV"))
            {
                Debug.Log($"Deleting {fileName}");
                File.Delete(fileName);
            }
        }
    }
}
