using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnOnTrigger))]
public class SpawnOnTriggerEditor : Editor
{

	SpawnOnTrigger mySpawner;
	SerializedObject soTarget;

	private SerializedProperty useTagOnTrigger;
	private SerializedProperty tagName;
	private SerializedProperty onlyOnce;

	private SerializedProperty shareOrientation;
	private SerializedProperty prefabToSpawn;
	private SerializedProperty randomMinOffset;
	private SerializedProperty randomMaxOffset;

	private SerializedProperty spawnInsideParent;
	private SerializedProperty spawnInsideCollidingObject;

	private SerializedProperty requireResources;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceIndex;
	private SerializedProperty resourceCostOnUse;

	private SerializedProperty requireInput;
	private SerializedProperty inputName;

	private SerializedProperty parent;

	GUIStyle warningStyle;

	GUIStyle subStyle1;
	GUIStyle subStyle2;
	GUIStyle buttonStyle;
	GUIStyle buttonStyle2;

	private Texture2D MakeTex (int width, int height, Color col)
	{
		Color[] pix = new Color[width * height];

		for (int i = 0; i < pix.Length; i++)
			pix[i] = col;

		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();

		return result;
	}

	private void OnEnable ()
	{
		mySpawner = (SpawnOnTrigger)target;
		soTarget = new SerializedObject(target);

		////
		useTagOnTrigger = soTarget.FindProperty("useTagOnTrigger");
		tagName = soTarget.FindProperty("tagName");
		onlyOnce = soTarget.FindProperty("onlyOnce");

		shareOrientation = soTarget.FindProperty("shareOrientation");
		prefabToSpawn = soTarget.FindProperty("prefabToSpawn");
		randomMinOffset = soTarget.FindProperty("randomMinOffset");
		randomMaxOffset = soTarget.FindProperty("randomMaxOffset");

		////

		spawnInsideParent = soTarget.FindProperty("spawnInsideParent");
		spawnInsideCollidingObject = soTarget.FindProperty("spawnInsideCollidingObject");
		parent = soTarget.FindProperty("parent");

		////

		requireResources = soTarget.FindProperty("requireResources");
		resourceManager = soTarget.FindProperty("resourceManager");
		resourceIndex = soTarget.FindProperty("resourceIndex");
		resourceCostOnUse = soTarget.FindProperty("resourceCostOnUse");

		requireInput = soTarget.FindProperty("requireInput");
		inputName = soTarget.FindProperty("inputName");

	}

	public override void OnInspectorGUI ()
	{
		#region Styles

		warningStyle = new GUIStyle("box");
		warningStyle.normal.background = MakeTex(1, 1, new Color(0.7f, 0, 0, 1f));
		warningStyle.normal.textColor = Color.black;

		subStyle1 = new GUIStyle("box");
		subStyle1.normal.background = MakeTex(1, 1, new Color(0.4f, 0.4f, 0.4f, 1f));
		subStyle1.normal.textColor = Color.black;

		subStyle2 = new GUIStyle("box");
		subStyle2.normal.background = MakeTex(1, 1, new Color(0.45f, 0.45f, 0.45f, 1f));
		subStyle2.normal.textColor = Color.black;

		buttonStyle = new GUIStyle("box");
		buttonStyle.normal.background = MakeTex(1, 1, new Color(0.8f, 0.2f, 0.2f, 1f));
		buttonStyle.normal.textColor = Color.white;

		buttonStyle2 = new GUIStyle("box");
		buttonStyle2.normal.background = MakeTex(1, 1, new Color(0.2f, 0.6f, 0.2f, 1f));
		buttonStyle2.normal.textColor = Color.white;

		#endregion

		soTarget.Update();
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.LabelField("Collision", EditorStyles.boldLabel);

		EditorGUILayout.PropertyField(onlyOnce);
		EditorGUILayout.BeginVertical("box");
		{
			EditorGUILayout.PropertyField(useTagOnTrigger);
			if (mySpawner.useTagOnTrigger)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(tagName);
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Spawning", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical("box");
		{
			EditorGUILayout.PropertyField(prefabToSpawn);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.PropertyField(randomMinOffset);
		EditorGUILayout.PropertyField(randomMaxOffset);
		EditorGUILayout.PropertyField(shareOrientation);

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Nested Spawning", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical("box");
		{
			EditorGUILayout.PropertyField(spawnInsideParent);
			if (mySpawner.spawnInsideParent)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(spawnInsideCollidingObject);
					if (mySpawner.spawnInsideCollidingObject == false)
					{
						EditorGUILayout.PropertyField(parent);
					}
				}
				EditorGUILayout.EndVertical();
			}

		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.LabelField("Resources", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical("box");
		{
			EditorGUILayout.PropertyField(requireResources);
			if (mySpawner.requireResources)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(resourceManager);
					EditorGUILayout.PropertyField(resourceIndex);
					EditorGUILayout.PropertyField(resourceCostOnUse);
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.LabelField("Input", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical("box");
		{
			EditorGUILayout.PropertyField(requireInput);

			if (mySpawner.requireInput)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(inputName);
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
			//GUI.FocusControl(GUI.GetNameOfFocusedControl());
		}

		EditorGUILayout.Space();

		if (mySpawner.prefabToSpawn.Length == 0 || mySpawner.prefabToSpawn[0] == null)
		{
			EditorGUILayout.BeginVertical(warningStyle);
			{
				EditorGUILayout.LabelField("No Prefab to spawn set !! Please add one", EditorStyles.boldLabel);
			}
			EditorGUILayout.EndVertical();
		}

	}
}
