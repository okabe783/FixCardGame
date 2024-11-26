using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Timeline.Actions;
using UnityEngine.SceneManagement;

/// <summary>Menu画面にシーンを移動するボタンを追加</summary>
public static class SceneNavigation
{
    [MenuItem("Scene/TitleScene")]
    public static void Scene0()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(0);
    }
    
    [MenuItem("Scene/GameScene")]
    public static void Scene01()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(1);
    }

    [MenuItem("Scene/ResultScene")]
    public static void Scene02()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(2);
    }
    
    [MenuItem("Scene/StageSelect")]
    public static void Scene03()
    {
        EditorSceneManager.SaveOpenScenes();
        OpenScene(3);
    }
    
    private static void OpenScene(int sceneIndex)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        
        if (!string.IsNullOrEmpty(scenePath))
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
