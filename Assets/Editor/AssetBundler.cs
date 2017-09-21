using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBundler: EditorWindow
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundle()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetsBundles",BuildAssetBundleOptions.None,BuildTarget.Android);
		AssetDatabase.Refresh();
    }
}
