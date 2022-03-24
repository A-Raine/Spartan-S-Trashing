using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bumper))]
public class BumperEditor : Editor
{

	Bumper myObject;
	SerializedObject soTarget;

	private SerializedProperty bumpForce;
	private SerializedProperty bumpTowardsOther;
	private SerializedProperty additionalForceTowardsOther;
	private SerializedProperty preventInputHolding;

	private SerializedProperty useTag;
	private SerializedProperty tagName;

	GUIStyle warningStyle;

	GUIStyle subStyle1;
	GUIStyle subStyle2;


	private void OnEnable ()
	{
		myObject = (Bumper)target;
		soTarget = new SerializedObject(target);

		////

		bumpForce = soTarget.FindProperty("bumpForce");
		bumpTowardsOther = soTarget.FindProperty("bumpTowardsOther");
		additionalForceTowardsOther = soTarget.FindProperty("additionalForceTowardsOther");
		preventInputHolding = soTarget.FindProperty("preventInputHolding");

		useTag = soTarget.FindProperty("useTag");
		tagName = soTarget.FindProperty("tagName");

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

		#endregion

		soTarget.Update();
		EditorGUI.BeginChangeCheck();


		EditorGUILayout.BeginVertical("box");
		{
			EditorGUILayout.LabelField("Forces", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical(subStyle1);
			{
				EditorGUILayout.PropertyField(bumpForce);
				EditorGUILayout.PropertyField(bumpTowardsOther);

				if(myObject.bumpTowardsOther)
				{
					EditorGUILayout.BeginVertical(subStyle2);
					{
						EditorGUILayout.PropertyField(additionalForceTowardsOther);
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.PropertyField(preventInputHolding);

				
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical("box");
		{
			EditorGUILayout.LabelField("Tag", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical(subStyle1);
			{
				EditorGUILayout.PropertyField(useTag);

				if (myObject.useTag)
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
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
			//GUI.FocusControl(GUI.GetNameOfFocusedControl());
		}

		EditorGUILayout.Space();

	}

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
}
