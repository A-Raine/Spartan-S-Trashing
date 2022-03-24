using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackOnInput))]
public class AttackOnInputEditor : Editor
{
	int toolBarTab;
	string currentTab;

	AttackOnInput myObject;
	SerializedObject soTarget;

	private SerializedProperty inputName;
	private SerializedProperty attackLayerMask;
	private SerializedProperty attackAngle;
	private SerializedProperty attackRange;
	private SerializedProperty damageDelay;
	private SerializedProperty attackCooldown;

	private SerializedProperty attackDamage;
	private SerializedProperty attackKnockback;
	private SerializedProperty attackUpwardsKnockback;
	private SerializedProperty hitFX;

	private SerializedProperty attackTriggerParameterName;
	private SerializedProperty animator;

	GUIStyle warningStyle;
	GUIStyle headerStyle;

	GUIStyle subStyle1;
	GUIStyle subStyle2;

	GUIStyle buttonStyle;
	GUIStyle buttonStyle2;

	private void OnEnable ()
	{
		myObject = (AttackOnInput)target;
		soTarget = new SerializedObject(target);

		////

		inputName = soTarget.FindProperty("inputName");
		attackLayerMask = soTarget.FindProperty("attackLayerMask");
		attackAngle = soTarget.FindProperty("attackAngle");
		attackRange = soTarget.FindProperty("attackRange");
		damageDelay = soTarget.FindProperty("damageDelay");
		attackCooldown = soTarget.FindProperty("attackCooldown");

		attackDamage = soTarget.FindProperty("attackDamage");
		attackKnockback = soTarget.FindProperty("attackKnockback");
		attackUpwardsKnockback = soTarget.FindProperty("attackUpwardsKnockback");
		hitFX = soTarget.FindProperty("hitFX");

		attackTriggerParameterName = soTarget.FindProperty("attackTriggerParameterName");
		animator = soTarget.FindProperty("animator");
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

		EditorGUILayout.PropertyField(inputName);
		EditorGUILayout.PropertyField(attackLayerMask);
		EditorGUILayout.PropertyField(attackAngle);
		EditorGUILayout.PropertyField(attackRange);
		EditorGUILayout.PropertyField(damageDelay);
		EditorGUILayout.PropertyField(attackCooldown);

		if (myObject.attackCooldown > 0f)
		{
			if(myObject.timer > 0f)
			{
				EditorGUI.ProgressBar(new Rect(20, 175, EditorGUIUtility.currentViewWidth - 40, 20), 1f - (myObject.timer / myObject.attackCooldown), "Cooldown");
			}
			else
			{
				EditorGUI.ProgressBar(new Rect(20, 175, EditorGUIUtility.currentViewWidth - 40, 20), 1f - (myObject.timer / myObject.attackCooldown), "Can attack now !");
			}
		}

		EditorGUILayout.Space(20f);

		EditorGUILayout.PropertyField(attackDamage);
		EditorGUILayout.PropertyField(attackKnockback);
		EditorGUILayout.PropertyField(attackUpwardsKnockback);
		EditorGUILayout.PropertyField(hitFX);

		EditorGUILayout.PropertyField(animator);

		if(myObject.animator != null)
		{
			EditorGUILayout.PropertyField(attackTriggerParameterName);
		}

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}

	}

	private void OnSceneGUI ()
	{
		Handles.color = Color.blue;
		Vector3 effectAngleA = DirFromAngle(-myObject.attackAngle / 2, false);
		Vector3 effectAngleB = DirFromAngle(myObject.attackAngle / 2, false);

		Handles.DrawWireArc(myObject.transform.position, Vector3.up, effectAngleA, myObject.attackAngle, myObject.attackRange);

		Handles.DrawLine(myObject.transform.position, myObject.transform.position + effectAngleA * myObject.attackRange);
		Handles.DrawLine(myObject.transform.position, myObject.transform.position + effectAngleB * myObject.attackRange);
	}

	public Vector3 DirFromAngle (float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += myObject.transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
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
