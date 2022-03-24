using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mover))]
public class MoverEditor : Editor
{
	int toolBarTab;
	string currentTab;

	Mover myMover;
	SerializedObject soTarget;

	private SerializedProperty movementType;

	private SerializedProperty useXAxis;
	private SerializedProperty useYAxis;
	private SerializedProperty useZAxis;

	private SerializedProperty requiresInput;
	private SerializedProperty xInputName;
	private SerializedProperty yInputName;
	private SerializedProperty zInputName;

	private SerializedProperty speed;
	private SerializedProperty maximumVelocity;
	private SerializedProperty lookAtDirection;
	private SerializedProperty isMovementLocal;

	private SerializedProperty useWallAvoidance;
	private SerializedProperty wallAvoidanceSize;
	private SerializedProperty wallAvoidanceOffset;
	private SerializedProperty wallAvoidanceLayers;

	private SerializedProperty animator;
	private SerializedProperty movementParticleSystem;
	private SerializedProperty minVelocityToToggle;

	private SerializedProperty xParameterName;
	private SerializedProperty yParameterName;
	private SerializedProperty zParameterName;

	private SerializedProperty keepOrientationOnIdle;

	private SerializedProperty trackSpeed;
	private SerializedProperty speedParameterName;

	private SerializedProperty animateWhenGroundedOnly;
	private SerializedProperty collisionCheckRadius;
	private SerializedProperty collisionOffset;
	private SerializedProperty groundedParameterName;
	private SerializedProperty groundLayerMask;

	GUIStyle warningStyle;

	GUIStyle subStyle1;
	GUIStyle subStyle2;
	GUIStyle buttonStyle;
	GUIStyle buttonStyle2;

	private void OnEnable ()
	{
		myMover = (Mover)target;
		soTarget = new SerializedObject(target);

		movementType = soTarget.FindProperty("movementType");

		useXAxis = soTarget.FindProperty("useXAxis");
		useYAxis = soTarget.FindProperty("useYAxis");
		useZAxis = soTarget.FindProperty("useZAxis");

		requiresInput = soTarget.FindProperty("requiresInput");
		xInputName = soTarget.FindProperty("xInputName");
		yInputName = soTarget.FindProperty("yInputName");
		zInputName = soTarget.FindProperty("zInputName");

		speed = soTarget.FindProperty("speed");
		maximumVelocity = soTarget.FindProperty("maximumVelocity");
		lookAtDirection = soTarget.FindProperty("lookAtDirection");
		isMovementLocal = soTarget.FindProperty("isMovementLocal");

		useWallAvoidance = soTarget.FindProperty("useWallAvoidance");
		wallAvoidanceSize = soTarget.FindProperty("wallAvoidanceSize");
		wallAvoidanceOffset = soTarget.FindProperty("wallAvoidanceOffset");
		wallAvoidanceLayers = soTarget.FindProperty("wallAvoidanceLayers");

		animator = soTarget.FindProperty("animator");
		movementParticleSystem = soTarget.FindProperty("movementParticleSystem");
		minVelocityToToggle = soTarget.FindProperty("minVelocityToToggle");

		xParameterName = soTarget.FindProperty("xParameterName");
		yParameterName = soTarget.FindProperty("yParameterName");
		zParameterName = soTarget.FindProperty("zParameterName");

		keepOrientationOnIdle = soTarget.FindProperty("keepOrientationOnIdle");
		trackSpeed = soTarget.FindProperty("trackSpeed");
		speedParameterName = soTarget.FindProperty("speedParameterName");

		animateWhenGroundedOnly = soTarget.FindProperty("animateWhenGroundedOnly");
		collisionCheckRadius = soTarget.FindProperty("collisionCheckRadius");
		collisionOffset = soTarget.FindProperty("collisionOffset");
		groundedParameterName = soTarget.FindProperty("groundedParameterName");
		groundLayerMask = soTarget.FindProperty("groundLayerMask");
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

		buttonStyle = new GUIStyle("box");
		buttonStyle.normal.background = MakeTex(1, 1, new Color(0.8f, 0.2f, 0.2f, 1f));
		buttonStyle.normal.textColor = Color.white;

		buttonStyle2 = new GUIStyle("box");
		buttonStyle2.normal.background = MakeTex(1, 1, new Color(0.2f, 0.6f, 0.2f, 1f));
		buttonStyle2.normal.textColor = Color.white;

		#endregion



		soTarget.Update();
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(movementType);
		EditorGUILayout.Space();
		toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Axis Constraints", "Input", "Movement", "Animation" });
		EditorGUILayout.Space();
		// Les checks étaient là avant
		switch (toolBarTab)
		{
			case 0:
			currentTab = "Axis Constraints";
			break;

			case 1:
			currentTab = "Input";
			break;

			case 2:
			currentTab = "Movement";
			break;

			case 3:
			currentTab = "Animation";
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
			case "Axis Constraints":

			EditorGUILayout.PropertyField(useXAxis);
			EditorGUILayout.PropertyField(useYAxis);
			EditorGUILayout.PropertyField(useZAxis);

			break;

			case "Input":
			EditorGUILayout.BeginVertical(subStyle1);
			{
				EditorGUILayout.PropertyField(requiresInput);
				if (myMover.requiresInput && (myMover.useXAxis || myMover.useYAxis || myMover.useZAxis))
				{
					EditorGUILayout.BeginVertical(subStyle2);
					{
						if (myMover.useXAxis && myMover.requiresInput)
						{
							EditorGUILayout.PropertyField(xInputName);
						}
						if (myMover.useYAxis && myMover.requiresInput)
						{
							EditorGUILayout.PropertyField(yInputName);
						}
						if (myMover.useZAxis && myMover.requiresInput)
						{
							EditorGUILayout.PropertyField(zInputName);
						}
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;

			case "Movement":

			EditorGUILayout.PropertyField(speed);
			if(myMover.movementType == Mover.MovementType.Force)
			{
				EditorGUILayout.PropertyField(maximumVelocity);
			}
			EditorGUILayout.PropertyField(lookAtDirection);
			EditorGUILayout.PropertyField(isMovementLocal);
			
			EditorGUILayout.BeginVertical(subStyle1);
			{
				EditorGUILayout.PropertyField(useWallAvoidance);
				if (myMover.useWallAvoidance)
				{
					EditorGUILayout.BeginVertical(subStyle2);
					{
						EditorGUILayout.PropertyField(wallAvoidanceSize);
						EditorGUILayout.PropertyField(wallAvoidanceOffset);
						EditorGUILayout.PropertyField(wallAvoidanceLayers);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;

			case "Animation":

			EditorGUILayout.BeginVertical("box");
			{
				EditorGUILayout.PropertyField(animator);
				if (myMover.animator != null)
				{
					EditorGUILayout.BeginVertical(subStyle1);
					{
						if (myMover.useXAxis)
						{
							EditorGUILayout.PropertyField(xParameterName);
						}
						if (myMover.useYAxis)
						{
							EditorGUILayout.PropertyField(yParameterName);
						}
						if (myMover.useZAxis)
						{
							EditorGUILayout.PropertyField(zParameterName);
						}
					}
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginVertical(subStyle2);
					{
						EditorGUILayout.PropertyField(keepOrientationOnIdle);
					}
					EditorGUILayout.EndVertical();
					

					EditorGUILayout.BeginVertical(subStyle1);
					{
						EditorGUILayout.PropertyField(trackSpeed);
						if (myMover.trackSpeed)
						{
							EditorGUILayout.BeginVertical(subStyle2);
							{
								EditorGUILayout.PropertyField(speedParameterName);
							}
							EditorGUILayout.EndVertical();	
						}
					}
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginVertical(subStyle1);
					{
						EditorGUILayout.PropertyField(animateWhenGroundedOnly);
						if (myMover.animateWhenGroundedOnly)
						{
							EditorGUILayout.BeginVertical(subStyle2);
							{
								EditorGUILayout.PropertyField(collisionCheckRadius);
								EditorGUILayout.PropertyField(collisionOffset);
								EditorGUILayout.PropertyField(groundedParameterName);
								EditorGUILayout.PropertyField(groundLayerMask);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}

				EditorGUILayout.PropertyField(movementParticleSystem);
				if (myMover.movementParticleSystem != null)
				{
					EditorGUILayout.BeginVertical(subStyle2);
					{
						EditorGUILayout.PropertyField(minVelocityToToggle);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;
		}

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}

		EditorGUILayout.Space();
		///

		#region DebugMessages

		switch (toolBarTab)
		{
			case 0:
			if (!myMover.useXAxis && !myMover.useYAxis && !myMover.useZAxis)
			{
				EditorGUILayout.BeginVertical(warningStyle);
				{
					EditorGUILayout.LabelField("Add at least one axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case 1:
			if (myMover.xInputName == "" && myMover.useXAxis && myMover.requiresInput)
			{
				EditorGUILayout.BeginVertical(warningStyle);
				{
					EditorGUILayout.LabelField("Input name not set for X Axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (myMover.yInputName == "" && myMover.useYAxis && myMover.requiresInput)
			{
				EditorGUILayout.BeginVertical(warningStyle);
				{
					EditorGUILayout.LabelField("Input name not set for Y Axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (myMover.zInputName == "" && myMover.useZAxis && myMover.requiresInput)
			{
				EditorGUILayout.BeginVertical(warningStyle);
				{
					EditorGUILayout.LabelField("Input name not set for Z Axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case 2:
			if (myMover.speed.x == 0 && myMover.useXAxis)
			{
				EditorGUILayout.BeginVertical(warningStyle);
				{
					EditorGUILayout.LabelField("X Speed is equal to 0. Either disable useXAxis or set a speed to X Axis", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (myMover.speed.y == 0 && myMover.useYAxis)
			{
				EditorGUILayout.BeginVertical(warningStyle);
				{
					EditorGUILayout.LabelField("Y Speed is equal to 0. Either disable useYAxis or set a speed to Y Axis", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (myMover.speed.z == 0 && myMover.useZAxis)
			{
				EditorGUILayout.BeginVertical(warningStyle);
				{
					EditorGUILayout.LabelField("Z Speed is equal to 0. Either disable useZAxis or set a speed to Z Axis", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case 3:
			
			break;
		}

		#endregion
	}
}
