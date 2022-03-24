using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Jumper))]
public class JumperEditor : Editor
{
	int toolBarTab;
	string currentTab;

	Jumper myJumper;
	SerializedObject soTarget;

	private SerializedProperty jumpInputName;
	private SerializedProperty jumpForce;
	private SerializedProperty jumpLayerMask;
	private SerializedProperty fallGravityMultiplier;
	private SerializedProperty jumpEndYVelocity;
	private SerializedProperty lowJumpGravityMultiplier;

	private SerializedProperty jumpAmount;
	private SerializedProperty airJumpForce;
	private SerializedProperty resetVelocityOnAirJump;

	private SerializedProperty useCollisionCheck;
	private SerializedProperty collisionOffset;
	private SerializedProperty collisionCheckRadius;
	private SerializedProperty characterHeight;

	private SerializedProperty jumpFX;
	private SerializedProperty airJumpFX;
	private SerializedProperty FXOffset;
	private SerializedProperty timeBeforeDestroyFX;

	private SerializedProperty animator;
	private SerializedProperty jumpTriggerName;
	private SerializedProperty airJumpTriggerName;


	GUIStyle warningStyle;

	GUIStyle subStyle1;
	GUIStyle subStyle2;
	private void OnEnable ()
	{
		myJumper = (Jumper)target;
		soTarget = new SerializedObject(target);

		jumpInputName = soTarget.FindProperty("jumpInputName");
		jumpForce = soTarget.FindProperty("jumpForce");
		jumpLayerMask = soTarget.FindProperty("jumpLayerMask");

		jumpEndYVelocity = soTarget.FindProperty("jumpEndYVelocity");
		fallGravityMultiplier = soTarget.FindProperty("fallGravityMultiplier");
		lowJumpGravityMultiplier = soTarget.FindProperty("lowJumpGravityMultiplier");

		jumpAmount = soTarget.FindProperty("jumpAmount");
		airJumpForce = soTarget.FindProperty("airJumpForce");
		resetVelocityOnAirJump = soTarget.FindProperty("resetVelocityOnAirJump");

		useCollisionCheck = soTarget.FindProperty("useCollisionCheck");
		collisionOffset = soTarget.FindProperty("collisionOffset");
		collisionCheckRadius = soTarget.FindProperty("collisionCheckRadius");
		characterHeight = soTarget.FindProperty("characterHeight");

		jumpFX = soTarget.FindProperty("jumpFX");
		airJumpFX = soTarget.FindProperty("airJumpFX");
		FXOffset = soTarget.FindProperty("FXOffset");
		timeBeforeDestroyFX = soTarget.FindProperty("timeBeforeDestroyFX");

		animator = soTarget.FindProperty("animator");
		jumpTriggerName = soTarget.FindProperty("jumpTriggerName");
		airJumpTriggerName = soTarget.FindProperty("airJumpTriggerName");

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
		subStyle1.normal.background = MakeTex(1, 1, new Color(0.3f, 0.3f, 0.3f, 1f));
		subStyle1.normal.textColor = Color.black;

		subStyle2 = new GUIStyle("box");
		subStyle2.normal.background = MakeTex(1, 1, new Color(0.35f, 0.35f, 0.35f, 1f));
		subStyle2.normal.textColor = Color.black;

		#endregion

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Jump", "Air Jump", "Collision", "FX", "Animation" });
		EditorGUILayout.Space();
		// Les checks étaient là avant
		switch (toolBarTab)
		{
			case 0:
			currentTab = "Jump";
			break;

			case 1:
			currentTab = "Air Jump";
			break;

			case 2:
			currentTab = "Collision";
			break;

			case 3:
			currentTab = "FX";
			break;

			case 4:
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
			case "Jump":
			EditorGUILayout.BeginVertical("box");
			{
				EditorGUILayout.PropertyField(jumpInputName);
				EditorGUILayout.PropertyField(jumpForce);
				EditorGUILayout.PropertyField(jumpLayerMask);

				EditorGUILayout.Separator();

				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.LabelField("Better Jump", EditorStyles.boldLabel);
					EditorGUILayout.PropertyField(jumpEndYVelocity);
					EditorGUILayout.PropertyField(fallGravityMultiplier);
					EditorGUILayout.PropertyField(lowJumpGravityMultiplier);
				}
				EditorGUILayout.EndVertical();
				
			}
			EditorGUILayout.EndVertical();
			break;

			case "Air Jump":

			//EditorGUILayout.PropertyField(jumpAmount);
			EditorGUILayout.BeginVertical("box");
			{
				EditorGUILayout.IntSlider(jumpAmount, 1, 10);

				if (myJumper.jumpAmount > 1)
				{
					EditorGUILayout.BeginVertical(subStyle1);
					{
						EditorGUILayout.PropertyField(airJumpForce);
						EditorGUILayout.PropertyField(resetVelocityOnAirJump);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;

			case "Collision":
			EditorGUILayout.BeginVertical("box");
			{
				EditorGUILayout.PropertyField(useCollisionCheck);
				if (myJumper.useCollisionCheck)
				{
					EditorGUILayout.BeginVertical(subStyle1);
					{
						EditorGUILayout.PropertyField(collisionOffset);
						EditorGUILayout.PropertyField(collisionCheckRadius);
						EditorGUILayout.PropertyField(characterHeight);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;

			case "FX":
			EditorGUILayout.BeginVertical("box");
			{
				EditorGUILayout.PropertyField(jumpFX);
				EditorGUILayout.PropertyField(airJumpFX);

				if (myJumper.jumpFX != null || myJumper.airJumpFX != null)
				{
					EditorGUILayout.BeginVertical(subStyle1);
					{
						EditorGUILayout.PropertyField(FXOffset);
						EditorGUILayout.PropertyField(timeBeforeDestroyFX);
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

				if(myJumper.animator != null)
				{
					EditorGUILayout.BeginVertical(subStyle1);
					{
						EditorGUILayout.PropertyField(jumpTriggerName);
						EditorGUILayout.PropertyField(airJumpTriggerName);
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
	}
}
