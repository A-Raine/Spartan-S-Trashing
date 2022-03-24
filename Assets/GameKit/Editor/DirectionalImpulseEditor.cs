using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(DirectionalImpulse))]
public class DirectionalImpulseEditor : Editor
{

	DirectionalImpulse myObject;
	SerializedObject soTarget;

	private SerializedProperty directionInputType;
	private SerializedProperty horizontalAxis;
	private SerializedProperty horizontalAxisName;

	private SerializedProperty verticalAxis;
	private SerializedProperty verticalAxisName;

	private SerializedProperty impulseInputName;

	private SerializedProperty depthAxis;

	private SerializedProperty resetVelocityOnImpulse;
	private SerializedProperty impulseForce;

	private SerializedProperty useCooldown;
	private SerializedProperty cooldown;

	private SerializedProperty minimalHeightToImpulse;
	private SerializedProperty heightAxis;
	private SerializedProperty groundDetectionLayerMask;
	private SerializedProperty collisionOffset;

	private SerializedProperty requireResources;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceIndex;
	private SerializedProperty resourceCostOnUse;

	private SerializedProperty preventMovingAfterImpulse;
	private SerializedProperty mover;
	private SerializedProperty movePreventionDuration;

	private SerializedProperty impulseFX;
	private SerializedProperty FXOffset;
	private SerializedProperty timeBeforeDestroyFX;

	private SerializedProperty animator;
	private SerializedProperty dashTriggerName;

	GUIStyle headerStyle;
	GUIStyle subStyle1;
	GUIStyle subStyle2;
	GUIStyle subStyle3;

	private void OnEnable ()
	{


		myObject = (DirectionalImpulse)target;
		soTarget = new SerializedObject(target);
		
		////

		directionInputType = soTarget.FindProperty("directionInputType");
		horizontalAxis = soTarget.FindProperty("horizontalAxis");
		horizontalAxisName = soTarget.FindProperty("horizontalAxisName");

		verticalAxis = soTarget.FindProperty("verticalAxis");
		verticalAxisName = soTarget.FindProperty("verticalAxisName");

		impulseInputName = soTarget.FindProperty("impulseInputName");
		depthAxis = soTarget.FindProperty("depthAxis");

		resetVelocityOnImpulse = soTarget.FindProperty("resetVelocityOnImpulse");
		impulseForce = soTarget.FindProperty("impulseForce");

		useCooldown = soTarget.FindProperty("useCooldown");
		cooldown = soTarget.FindProperty("cooldown");

		minimalHeightToImpulse = soTarget.FindProperty("minimalHeightToImpulse");
		heightAxis = soTarget.FindProperty("heightAxis");
		groundDetectionLayerMask = soTarget.FindProperty("groundDetectionLayerMask");
		collisionOffset = soTarget.FindProperty("collisionOffset");

		requireResources = soTarget.FindProperty("requireResources");
		resourceManager = soTarget.FindProperty("resourceManager");
		resourceIndex = soTarget.FindProperty("resourceIndex");
		resourceCostOnUse = soTarget.FindProperty("resourceCostOnUse");

		preventMovingAfterImpulse = soTarget.FindProperty("preventMovingAfterImpulse");
		mover = soTarget.FindProperty("mover");
		movePreventionDuration = soTarget.FindProperty("movePreventionDuration");

		impulseFX = soTarget.FindProperty("impulseFX");
		FXOffset = soTarget.FindProperty("FXOffset");
		timeBeforeDestroyFX = soTarget.FindProperty("timeBeforeDestroyFX");

		animator = soTarget.FindProperty("animator");
		dashTriggerName = soTarget.FindProperty("dashTriggerName");

	}


	public override void OnInspectorGUI ()
	{
		#region Styles

		headerStyle = new GUIStyle("box");
		headerStyle.normal.background = MakeTex(1, 1, new Color(0.25f, 0.25f, 0.25f, 1f));
		headerStyle.stretchWidth = true;
		headerStyle.fontStyle = FontStyle.Bold;
		headerStyle.normal.textColor = Color.white;
		headerStyle.hover.background = MakeTex(1, 1, new Color(0.40f, 0.40f, 0.40f, 1f));
		headerStyle.onHover.background = MakeTex(1, 1, new Color(0.40f, 0.40f, 0.40f, 1f));

		subStyle1 = new GUIStyle("box");
		subStyle1.normal.background = MakeTex(1, 1, new Color(0.25f, 0.25f, 0.25f, 1f));
		subStyle1.normal.textColor = Color.black;

		subStyle2 = new GUIStyle("box");
		subStyle2.normal.background = MakeTex(1, 1, new Color(0.30f, 0.30f, 0.30f, 1f));
		subStyle2.normal.textColor = Color.black;

		subStyle3 = new GUIStyle("box");
		subStyle3.normal.textColor = Color.black;
		subStyle3.normal.background = MakeTex(1, 1, new Color(0.35f, 0.35f, 0.35f, 1f));

		#endregion

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical("box");
		{
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button(" Input ", headerStyle, GUILayout.MaxHeight(20f)))
				{
					myObject.showInput = !myObject.showInput;
				}

				if (GUILayout.Button(" + ", headerStyle, GUILayout.MaxWidth(20f)))
				{
					myObject.showInput = true;
					myObject.showCollision = true;
					myObject.showCooldown = true;
					myObject.showForce = true;
					myObject.showFX = true;
					myObject.showMovement = true;
					myObject.showResources = true;
				}

				if (GUILayout.Button(" - ", headerStyle, GUILayout.MaxWidth(20f)))
				{
					myObject.showInput = false;
					myObject.showCollision = false;
					myObject.showCooldown = false;
					myObject.showForce = false;
					myObject.showFX = false;
					myObject.showMovement = false;
					myObject.showResources = false;
				}
			}
			EditorGUILayout.EndHorizontal();
			if (myObject.showInput)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(impulseInputName);

					EditorGUILayout.Space();

					EditorGUILayout.PropertyField(directionInputType);

					switch (myObject.directionInputType)
					{
						case DirectionalImpulse.DirectionInputType.Axis:
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(horizontalAxis);
							if(myObject.horizontalAxis != DirectionalImpulse.Axis.None)
							{
								EditorGUILayout.BeginVertical(subStyle3);
								{
									EditorGUILayout.PropertyField(horizontalAxisName);
								}
								EditorGUILayout.EndVertical();
							}
						}
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(verticalAxis);
							if (myObject.verticalAxis != DirectionalImpulse.Axis.None)
							{
								EditorGUILayout.BeginVertical(subStyle3);
								{
									EditorGUILayout.PropertyField(verticalAxisName);
								}
								EditorGUILayout.EndVertical();
							}
						}
						EditorGUILayout.EndVertical();
						break;

						case DirectionalImpulse.DirectionInputType.MousePos:
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(depthAxis);
						}
						EditorGUILayout.EndVertical();
						break;
					}
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical("box");
		{
			if (GUILayout.Button(" Force ", headerStyle, GUILayout.MaxHeight(20f)))
			{
				myObject.showForce = !myObject.showForce;
			}

			if(myObject.showForce)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(resetVelocityOnImpulse);
					EditorGUILayout.PropertyField(impulseForce);
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical("box");
		{
			if (GUILayout.Button(" Cooldown ", headerStyle, GUILayout.MaxHeight(20f)))
			{
				myObject.showCooldown = !myObject.showCooldown;
			}

			if (myObject.showCooldown)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(useCooldown);

					if (myObject.useCooldown)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(cooldown);
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
			if (GUILayout.Button(" Collision ", headerStyle, GUILayout.MaxHeight(20f)))
			{
				myObject.showCollision = !myObject.showCollision;
			}

			if (myObject.showCollision)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(minimalHeightToImpulse);

					if (myObject.minimalHeightToImpulse > 0)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(heightAxis);
							EditorGUILayout.PropertyField(groundDetectionLayerMask);
							EditorGUILayout.PropertyField(collisionOffset);
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
			if (GUILayout.Button(" Resources ", headerStyle, GUILayout.MaxHeight(20f)))
			{
				myObject.showResources = !myObject.showResources;
			}

			if (myObject.showResources)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(requireResources);

					if (myObject.requireResources)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(resourceManager);
							EditorGUILayout.PropertyField(resourceIndex);
							EditorGUILayout.PropertyField(resourceCostOnUse);
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
			if (GUILayout.Button(" Movement Prevention ", headerStyle, GUILayout.MaxHeight(20f)))
			{
				myObject.showMovement = !myObject.showMovement;
			}

			if (myObject.showMovement)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(preventMovingAfterImpulse);

					if (myObject.preventMovingAfterImpulse)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(mover);
							EditorGUILayout.PropertyField(movePreventionDuration);
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
			if (GUILayout.Button(" FX ", headerStyle, GUILayout.MaxHeight(20f)))
			{
				myObject.showFX = !myObject.showFX;
			}

			if (myObject.showFX)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(impulseFX);

					if (myObject.impulseFX != null)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(FXOffset);
							EditorGUILayout.PropertyField(timeBeforeDestroyFX);
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
				myObject.showAnim = !myObject.showAnim;
			}

			if (myObject.showAnim)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(animator);

					if (myObject.animator != null)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(dashTriggerName);
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