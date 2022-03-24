using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomGravity))]
public class CustomGravityEditor : Editor
{

	CustomGravity myObject;
	SerializedObject soTarget;

	private SerializedProperty baseGravityForce;
	private SerializedProperty secondaryGravityForce;
	private SerializedProperty maxVelocity;

	private SerializedProperty invertOnInput;
	private SerializedProperty inputName;
	private SerializedProperty instantGravityChangeOnInput;
	private SerializedProperty invertJumpingDirection;

	private SerializedProperty onlyWhenGrounded;
	private SerializedProperty collisionCheckDistance;

	private SerializedProperty InvertScaleOnGravityChange;
	private SerializedProperty transformToInvert;
	private SerializedProperty invertedRotation;
	private SerializedProperty normalRotation;

	GUIStyle warningStyle;

	GUIStyle headerStyle;
	GUIStyle subStyle1;
	GUIStyle subStyle2;


	private void OnEnable ()
	{
		myObject = (CustomGravity)target;
		soTarget = new SerializedObject(target);

		////

		baseGravityForce = soTarget.FindProperty("baseGravityForce");
		secondaryGravityForce = soTarget.FindProperty("secondaryGravityForce");
		maxVelocity = soTarget.FindProperty("maxVelocity");

		invertOnInput = soTarget.FindProperty("invertOnInput");
		inputName = soTarget.FindProperty("inputName");
		instantGravityChangeOnInput = soTarget.FindProperty("instantGravityChangeOnInput");
		invertJumpingDirection = soTarget.FindProperty("invertJumpingDirection");

		onlyWhenGrounded = soTarget.FindProperty("onlyWhenGrounded");
		collisionCheckDistance = soTarget.FindProperty("collisionCheckDistance");

		InvertScaleOnGravityChange = soTarget.FindProperty("InvertScaleOnGravityChange");
		transformToInvert = soTarget.FindProperty("transformToInvert");
		invertedRotation = soTarget.FindProperty("invertedRotation");
		normalRotation = soTarget.FindProperty("normalRotation");
	}

	public override void OnInspectorGUI ()
	{
		#region Styles

		headerStyle = new GUIStyle("box");
		headerStyle.normal.background = MakeTex(1, 1, new Color(0.30f, 0.30f, 0.30f, 1f));
		headerStyle.stretchWidth = true;
		headerStyle.fontStyle = FontStyle.Bold;
		headerStyle.normal.textColor = Color.black;
		headerStyle.hover.background = MakeTex(1, 1, new Color(0.40f, 0.40f, 0.40f, 1f));
		headerStyle.onHover.background = MakeTex(1, 1, new Color(0.40f, 0.40f, 0.40f, 1f));

		warningStyle = new GUIStyle("box");
		warningStyle.normal.background = MakeTex(1, 1, new Color(0.7f, 0, 0, 1f));
		warningStyle.normal.textColor = Color.black;

		subStyle1 = new GUIStyle("box");
		subStyle1.normal.background = MakeTex(1, 1, new Color(0.3f, 0.3f, 0.3f, 1f));
		subStyle1.normal.textColor = Color.black;

		subStyle2 = new GUIStyle("box");
		subStyle2.normal.background = MakeTex(1, 1, new Color(0.35f, 0.35f, 0.35f, 1f));
		subStyle2.normal.textColor = Color.black;

		#endregion

		soTarget.Update();
		EditorGUI.BeginChangeCheck();


		EditorGUILayout.BeginVertical("box");
		{
			if (GUILayout.Button(" Forces ", headerStyle, GUILayout.MaxHeight(20f)))
			{
				myObject.showForces = !myObject.showForces;
			}

			if (myObject.showForces)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(baseGravityForce);
					EditorGUILayout.PropertyField(secondaryGravityForce);
					EditorGUILayout.PropertyField(maxVelocity);
				}
				EditorGUILayout.EndVertical();
			}

		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical("box");
		{
			//EditorGUILayout.LabelField("Gravity Inversion", EditorStyles.boldLabel);
			if(GUILayout.Button(" Gravity Inversion ", headerStyle, GUILayout.MaxHeight(20f)))
			{
				myObject.showGravity = !myObject.showGravity;
			}

			if(myObject.showGravity)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(invertOnInput);

					if (myObject.invertOnInput)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(inputName);
							EditorGUILayout.PropertyField(instantGravityChangeOnInput);
							EditorGUILayout.PropertyField(invertJumpingDirection);
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical("box");
		{
			if (GUILayout.Button(" Ground Check ", headerStyle, GUILayout.MaxHeight(20f)))
			{
				myObject.showGroundCheck = !myObject.showGroundCheck;
			}

			if (myObject.showGroundCheck)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(onlyWhenGrounded);

					if (myObject.onlyWhenGrounded)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(collisionCheckDistance);
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical("box");
		{
			if (GUILayout.Button(" Animation ", headerStyle, GUILayout.MaxHeight(20f)))
			{
				myObject.showAnimation = !myObject.showAnimation;
			}

			if (myObject.showAnimation)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(InvertScaleOnGravityChange);
					if (myObject.InvertScaleOnGravityChange)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(transformToInvert);
							EditorGUILayout.PropertyField(invertedRotation);
							EditorGUILayout.PropertyField(normalRotation);
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
