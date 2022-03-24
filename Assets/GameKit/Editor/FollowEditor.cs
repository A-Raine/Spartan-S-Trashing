using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Follow))]
public class FollowEditor : Editor
{
	int toolBarTab;
	string currentTab;

	Follow myFollow;
	SerializedObject soTarget;

	private SerializedProperty _target;

	private SerializedProperty followOnXAxis;
	private SerializedProperty followOnYAxis;
	private SerializedProperty followOnZAxis;
	private SerializedProperty smoothTime;
	private SerializedProperty maxDistance;

	private SerializedProperty shareOrientation;
	private SerializedProperty rotateOnXAxis;
	private SerializedProperty rotateOnYAxis;
	private SerializedProperty rotateOnZAxis;
	private SerializedProperty rotateSpeed;

	GUIStyle warningStyle;

	GUIStyle subStyle1;
	GUIStyle subStyle2;


	private void OnEnable ()
	{
		myFollow = (Follow)target;
		soTarget = new SerializedObject(target);

		////

		_target = soTarget.FindProperty("target");

		followOnXAxis = soTarget.FindProperty("followOnXAxis");
		followOnYAxis = soTarget.FindProperty("followOnYAxis");
		followOnZAxis = soTarget.FindProperty("followOnZAxis");
		smoothTime = soTarget.FindProperty("smoothTime");
		maxDistance = soTarget.FindProperty("maxDistance");

		shareOrientation = soTarget.FindProperty("shareOrientation");
		rotateOnXAxis = soTarget.FindProperty("rotateOnXAxis");
		rotateOnYAxis = soTarget.FindProperty("rotateOnYAxis");
		rotateOnZAxis = soTarget.FindProperty("rotateOnZAxis");
		rotateSpeed = soTarget.FindProperty("rotateSpeed");
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

		EditorGUILayout.Space();

		if(myFollow.target == null)
		{
			EditorGUILayout.BeginVertical(warningStyle);
			{
				EditorGUILayout.PropertyField(_target);
			}
			EditorGUILayout.EndVertical();
		}
		else
		{
			EditorGUILayout.BeginVertical("box");
			{
				EditorGUILayout.PropertyField(_target);
			}
			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.BeginVertical("box");
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Position", "Direction" });

		switch (toolBarTab)
		{
			case 0:
			currentTab = "Position";
			break;

			case 1:
			currentTab = "Direction";
			break;
		}

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
			GUI.FocusControl(null);
		}

		EditorGUI.BeginChangeCheck();

		
			switch (currentTab)
			{
				case "Position":
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(followOnXAxis);
					EditorGUILayout.PropertyField(followOnYAxis);
					EditorGUILayout.PropertyField(followOnZAxis);
					EditorGUILayout.Space();
					EditorGUILayout.PropertyField(smoothTime);
					EditorGUILayout.PropertyField(maxDistance);
				}
				EditorGUILayout.EndVertical();
				break;

				case "Direction":
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(shareOrientation);
					if (myFollow.shareOrientation)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(rotateOnXAxis);
							EditorGUILayout.PropertyField(rotateOnYAxis);
							EditorGUILayout.PropertyField(rotateOnZAxis);
							EditorGUILayout.PropertyField(rotateSpeed);
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
				break;

			}
		}
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}

		if (myFollow.target == null)
		{
			EditorGUILayout.BeginVertical(warningStyle);
			{
				EditorGUILayout.LabelField("No target set !", EditorStyles.boldLabel);
			}
			EditorGUILayout.EndVertical();
		}
	}
}

