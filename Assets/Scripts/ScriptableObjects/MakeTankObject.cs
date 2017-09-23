using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeTankObject
{

	[MenuItem("Assets/Create/Tank Object")]
	public static void CreateMyAsset()
	{
		TankObject asset = ScriptableObject.CreateInstance<TankObject>();
	
		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/TankObject.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}