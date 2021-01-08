using UnityEditor;

public class PostBuildPlayerScriptTest : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        //Debug.Log(movedFromAssetPaths);
    }
}
