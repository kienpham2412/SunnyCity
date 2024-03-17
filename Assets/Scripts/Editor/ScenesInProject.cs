using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ScenesInProject : EditorWindow
{
    [MenuItem("Tools/Scenes In Project", false)]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ScenesInProject));
    }

    FileInfo[] _files;
    Dictionary<string, bool> _folds;
    Vector2 _scrollPosition;

    private const string BUILD = "build";
    private const string SCENES_IN_BUILD = "Scenes In Build";
    static string[] sceneNames;
    static EditorBuildSettingsScene[] scenes;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        scenes = EditorBuildSettings.scenes;
        sceneNames = scenes.Select(x => AsSpacedCamelCase(Path.GetFileNameWithoutExtension(x.path))).ToArray();
    }

    public ScenesInProject()
    {
        _scrollPosition = new Vector2();
        _folds = new Dictionary<string, bool>
        {
            { "build", false }
        };
        GetScenes();
    }

    void OnProjectChange()
    {
        GetScenes();
        Repaint();
    }

    void OnUpdate()
    {
        GetScenes();
    }

    void OnGUI()
    {
        if (EditorApplication.isPlaying) return;

        EditorGUILayout.Space();
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));
        DisplayScenesInProject();
        DisplayScenesInBuild();
        GUILayout.EndScrollView();
    }

    private void DisplayScenesInBuild()
    {
        bool inBuildShow = _folds.ContainsKey(BUILD) && _folds[BUILD];
        _folds[BUILD] = EditorGUILayout.Foldout(inBuildShow, SCENES_IN_BUILD);

        if (_folds[BUILD])
            for (int i = 0; i < scenes.Length; i++)
            {
                DisplaySceneOptions(sceneNames[i], scenes[i].path);
            }
    }

    private void DisplayScenesInProject()
    {
        string d = "";

        for (int i = 0; i < _files.Length; i++)
        {
            FileInfo f = _files[i];

            if (d != f.DirectoryName)
            {
                d = f.DirectoryName;
                var label = f.DirectoryName.Replace(Application.dataPath.Replace("/", "\\") + "\\", "");
                bool show = !_folds.ContainsKey(d) ? false : _folds[d];
                _folds[d] = EditorGUILayout.Foldout(show, label);
            }

            if (_folds[d]) DisplaySceneOptions(f);
        }
    }

    private void DisplaySceneOptions(FileInfo f)
    {
        var sceneName = f.Name.Replace(".unity", "");
        DisplaySceneOptions(sceneName, f.FullName);
    }

    private void DisplaySceneOptions(string sceneName, string path)
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(sceneName);

        if (GUILayout.Button("Open", GUILayout.MaxWidth(85), GUILayout.MaxHeight(20)))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(path);
            }
        }

        if (GUILayout.Button("Play", GUILayout.MaxWidth(85), GUILayout.MaxHeight(20)))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(path);
                EditorApplication.EnterPlaymode();
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    FileInfo[] GetScenes()
    {
        DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
        _files = directory.GetFiles("*.unity", SearchOption.AllDirectories);
        System.Array.Sort(_files, delegate (FileInfo f1, FileInfo f2)
        {
            return f1.DirectoryName.CompareTo(f2.DirectoryName);
        });

        var dict = new Dictionary<string, bool>();
        var d = "";
        for (int i = 0; i < _files.Length; i++)
        {
            FileInfo f = _files[i];
            if (d != f.DirectoryName)
            {
                d = f.DirectoryName;
                var label = f.DirectoryName.Replace(Application.dataPath.Replace("/", "\\") + "\\", "");
                if (_folds.ContainsKey(d))
                {
                    dict[d] = _folds[d];
                }
            }
        }
        _folds = dict;
        return _files;
    }

    public string AsSpacedCamelCase(string text)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder(text.Length * 2);
        sb.Append(char.ToUpper(text[0]));
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                sb.Append(' ');
            sb.Append(text[i]);
        }
        return sb.ToString();
    }
}
