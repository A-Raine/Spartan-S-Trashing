using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Life))]
public class LifeEditor : Editor
{
	Life myObject;
	SerializedObject soTarget;

	private SerializedProperty startLife;
	private SerializedProperty maxLife;
	private SerializedProperty currentLife;

	private SerializedProperty invincibilityDuration;

	private SerializedProperty animator;
	private SerializedProperty hitParameterName;

	GUIStyle warningStyle;

	GUIStyle subStyle1;
	GUIStyle subStyle2;
	GUIStyle buttonStyle;
	GUIStyle buttonStyle2;

	private void OnEnable ()
	{
		myObject = (Life)target;
		soTarget = new SerializedObject(target);

		////

		startLife = soTarget.FindProperty("startLife");
		maxLife = soTarget.FindProperty("maxLife");
		currentLife = soTarget.FindProperty("currentLife");
		invincibilityDuration = soTarget.FindProperty("invincibilityDuration");

		animator = soTarget.FindProperty("animator");
		hitParameterName = soTarget.FindProperty("hitParameterName");
	}

	public override void OnInspectorGUI ()
	{
		#region Styles

		warningStyle = new GUIStyle("box");
		warningStyle.normal.background = MakeTex(1, 1, new Color(0.7f, 0, 0, 1f));
		warningStyle.normal.textColor = Color.black;

		subStyle1 = new GUIStyle("box");
		subStyle1.normal.background = MakeTex(1, 1, new Color(0.3f, 0.3f, 0.3f, 1f));
		subStyle1.normal.textColor = Color.black;

		subStyle2 = new GUIStyle("box");
		subStyle2.normal.background = MakeTex(1, 1, new Color(0.35f, 0.35f, 0.35f, 1f));
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

		EditorGUILayout.PropertyField(maxLife);
		//EditorGUILayout.PropertyField(startLife);

		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("Start Life ", GUILayout.MaxWidth(80));
			myObject.startLife = EditorGUILayout.IntSlider(myObject.startLife, 0, myObject.maxLife);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space(30f);

		if(!Application.isPlaying)
		{
			myObject.currentLife = myObject.startLife;
		}
		EditorGUI.ProgressBar(new Rect(20, 45, EditorGUIUtility.currentViewWidth - 40, 20), (float)myObject.currentLife / (float)myObject.maxLife, "Current Life");


		EditorGUILayout.PropertyField(invincibilityDuration);

		EditorGUILayout.BeginVertical(subStyle1);
		{
			EditorGUILayout.PropertyField(animator);

			if (myObject.animator != null)
			{
				EditorGUILayout.BeginVertical(subStyle2);
				{
					EditorGUILayout.PropertyField(hitParameterName);
				}
				EditorGUILayout.EndVertical();

			}
		}
		EditorGUILayout.EndVertical();
	

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}

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
