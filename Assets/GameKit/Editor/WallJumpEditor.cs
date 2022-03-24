using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WallJump))]
public class WallJumpEditor : Editor
{
	int toolBarTab;
	string currentTab;

	WallJump myJumper;
	SerializedObject soTarget;

	private SerializedProperty wallJumpInputName;
	private SerializedProperty wallJumpForce;
	private SerializedProperty wallJumpLayerMask;
	private SerializedProperty invertLookDirectionOnWallJump;
	private SerializedProperty gravityDampenWhileOnWall;

	private SerializedProperty preventMovingAfterWallJump;
	private SerializedProperty movePreventionDuration;
	private SerializedProperty mover;

	private SerializedProperty resetJumpCountOnWallJump;
	private SerializedProperty jumper;

	private SerializedProperty collisionCheckDistance;
	private SerializedProperty collisionOffset;
	private SerializedProperty minimalHeightAllowedToWallJump;

	private SerializedProperty jumpFX;
	private SerializedProperty FXOffset;
	private SerializedProperty timeBeforeDestroyFX;

	private SerializedProperty animator;
	private SerializedProperty wallJumpTriggerName;

	
	GUIStyle warningStyle;

	GUIStyle subStyle1;
	GUIStyle subStyle2;
	GUIStyle buttonStyle;
	GUIStyle buttonStyle2;
	private void OnEnable ()
	{


		myJumper = (WallJump)target;
		soTarget = new SerializedObject(target);

		wallJumpInputName = soTarget.FindProperty("wallJumpInputName");
		wallJumpForce = soTarget.FindProperty("wallJumpForce");
		wallJumpLayerMask = soTarget.FindProperty("wallJumpLayerMask");
		invertLookDirectionOnWallJump = soTarget.FindProperty("invertLookDirectionOnWallJump");
		gravityDampenWhileOnWall = soTarget.FindProperty("gravityDampenWhileOnWall");

		preventMovingAfterWallJump = soTarget.FindProperty("preventMovingAfterWallJump");
		movePreventionDuration = soTarget.FindProperty("movePreventionDuration");
		mover = soTarget.FindProperty("mover");

		resetJumpCountOnWallJump = soTarget.FindProperty("resetJumpCountOnWallJump");
		jumper = soTarget.FindProperty("jumper");

		collisionCheckDistance = soTarget.FindProperty("collisionCheckDistance");
		collisionOffset = soTarget.FindProperty("collisionOffset");
		minimalHeightAllowedToWallJump = soTarget.FindProperty("minimalHeightAllowedToWallJump");

		jumpFX = soTarget.FindProperty("jumpFX");
		FXOffset = soTarget.FindProperty("FXOffset");
		timeBeforeDestroyFX = soTarget.FindProperty("timeBeforeDestroyFX");

		animator = soTarget.FindProperty("animator");
		wallJumpTriggerName = soTarget.FindProperty("wallJumpTriggerName");

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

		toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Wall Jump", "Synergies", "Collision", "FX", "Animation" });
		EditorGUILayout.Space();
		// Les checks étaient là avant
		switch (toolBarTab)
		{
			case 0:
			currentTab = "Wall Jump";
			break;

			case 1:
			currentTab = "Synergies";
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
			case "Wall Jump":

			EditorGUILayout.PropertyField(wallJumpInputName);
			EditorGUILayout.PropertyField(wallJumpForce);
			EditorGUILayout.PropertyField(wallJumpLayerMask);
			EditorGUILayout.PropertyField(invertLookDirectionOnWallJump);
			EditorGUILayout.PropertyField(gravityDampenWhileOnWall);

			break;

			case "Synergies":

			//EditorGUILayout.PropertyField(jumpAmount);
			EditorGUILayout.BeginVertical("box");
			{
				
				EditorGUILayout.PropertyField(preventMovingAfterWallJump);

				if (myJumper.preventMovingAfterWallJump)
				{
					EditorGUILayout.BeginVertical(subStyle1);
					{
						EditorGUILayout.PropertyField(mover);
						EditorGUILayout.PropertyField(movePreventionDuration);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical("box");
			{

				EditorGUILayout.PropertyField(resetJumpCountOnWallJump);

				if (myJumper.resetJumpCountOnWallJump)
				{
					EditorGUILayout.BeginVertical(subStyle1);
					{
						EditorGUILayout.PropertyField(jumper);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;

			case "Collision":

				EditorGUILayout.PropertyField(collisionCheckDistance);
				EditorGUILayout.PropertyField(minimalHeightAllowedToWallJump);
				EditorGUILayout.PropertyField(collisionOffset);

			break;

			case "FX":
			EditorGUILayout.BeginVertical("box");
			{
				EditorGUILayout.PropertyField(jumpFX);

				if (myJumper.jumpFX != null)
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
						EditorGUILayout.PropertyField(wallJumpTriggerName);
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
