using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayAnimOnTrigger))]
public class PlayAnimOnTriggerEditor : Editor
{

	PlayAnimOnTrigger myObject;
	SerializedObject soTarget;

	private SerializedProperty animator;
	private SerializedProperty triggerName;
	private SerializedProperty useTag;
	private SerializedProperty tagName;
	private SerializedProperty triggerOnce;

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
		myObject = (PlayAnimOnTrigger)target;
		soTarget = new SerializedObject(target);

		////

		animator = soTarget.FindProperty("animator");
		triggerName = soTarget.FindProperty("triggerName");
		useTag = soTarget.FindProperty("useTag");
		tagName = soTarget.FindProperty("tagName");
		triggerOnce = soTarget.FindProperty("triggerOnce");
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

		EditorGUILayout.BeginVertical("box");
		{
			EditorGUILayout.PropertyField(animator);
			if(myObject.animator != null)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(triggerName);
					EditorGUILayout.PropertyField(triggerOnce);
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(useTag);

					if(myObject.useTag)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(tagName);
						}
						EditorGUILayout.EndVertical();
					}
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

	}
}
