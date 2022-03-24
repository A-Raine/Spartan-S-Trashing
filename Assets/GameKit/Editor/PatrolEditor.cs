using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Patrol))]
public class PatrolEditor : Editor
{
	Patrol myObject;
	SerializedObject soTarget;

	private SerializedProperty patrolPoints;
	private SerializedProperty areCoordinatesLocal;
	private SerializedProperty moveSpeed;
	private SerializedProperty minDistance;
	private SerializedProperty loop;

	private SerializedProperty lookAtTargetPos;
	private SerializedProperty smoothLookAt;
	private SerializedProperty rotateSpeed;
	private SerializedProperty lockXRotation;
	private SerializedProperty lockYRotation;
	private SerializedProperty lockZRotation;

	GUIStyle warningStyle;

	GUIStyle subStyle1;
	GUIStyle subStyle2;
	GUIStyle buttonStyle;
	GUIStyle buttonStyle2;

	bool showPatrolPoints = true;
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
		myObject = (Patrol)target;
		soTarget = new SerializedObject(target);

		////

		patrolPoints = soTarget.FindProperty("patrolPoints");
		areCoordinatesLocal = soTarget.FindProperty("areCoordinatesLocal");
		moveSpeed = soTarget.FindProperty("moveSpeed");
		minDistance = soTarget.FindProperty("minDistance");
		loop = soTarget.FindProperty("loop");

		lookAtTargetPos = soTarget.FindProperty("lookAtTargetPos");
		smoothLookAt = soTarget.FindProperty("smoothLookAt");
		rotateSpeed = soTarget.FindProperty("rotateSpeed");
		lockXRotation = soTarget.FindProperty("lockXRotation");
		lockYRotation = soTarget.FindProperty("lockYRotation");
		lockZRotation = soTarget.FindProperty("lockZRotation");

	}

	void AddPatrolPoint()
	{
		myObject.patrolPoints.Add(new Patrol.PatrolPoint());
		int patrolPointCount = myObject.patrolPoints.Count;
		myObject.patrolPoints[patrolPointCount -1].pointName = "Patrol Point " + patrolPointCount;

		if(myObject.areCoordinatesLocal)
		{
			myObject.patrolPoints[patrolPointCount - 1].pointPosition = myObject.transform.localPosition;
		}
		else
		{
			myObject.patrolPoints[patrolPointCount - 1].pointPosition = myObject.transform.position;
		}

		if(!showPatrolPoints)
		{
			showPatrolPoints = true;
		}
	}

	void DuplicatePatrolPoint (int index)
	{
		myObject.patrolPoints.Insert(index+1, new Patrol.PatrolPoint());

		myObject.patrolPoints[index+1].pointColor = myObject.patrolPoints[index].pointColor;
		myObject.patrolPoints[index + 1].pointName = myObject.patrolPoints[index].pointName + " (Copy)";
		myObject.patrolPoints[index + 1].pointPosition = myObject.patrolPoints[index].pointPosition;
		myObject.patrolPoints[index + 1].waitTimeAtPoint = myObject.patrolPoints[index].waitTimeAtPoint;
	}

	void MovePatrolPointUp (int index)
	{
		if(index < myObject.patrolPoints.Count -1)
		{
			Patrol.PatrolPoint patrol = new Patrol.PatrolPoint();

			patrol.pointColor = myObject.patrolPoints[index + 1].pointColor;
			patrol.pointName = myObject.patrolPoints[index + 1].pointName;
			patrol.pointPosition = myObject.patrolPoints[index + 1].pointPosition;
			patrol.waitTimeAtPoint = myObject.patrolPoints[index + 1].waitTimeAtPoint;

			myObject.patrolPoints[index + 1].pointColor = myObject.patrolPoints[index].pointColor;
			myObject.patrolPoints[index + 1].pointName = myObject.patrolPoints[index].pointName;
			myObject.patrolPoints[index + 1].pointPosition = myObject.patrolPoints[index].pointPosition;
			myObject.patrolPoints[index + 1].waitTimeAtPoint = myObject.patrolPoints[index].waitTimeAtPoint;

			myObject.patrolPoints[index].pointColor = patrol.pointColor;
			myObject.patrolPoints[index].pointName = patrol.pointName;
			myObject.patrolPoints[index].pointPosition = patrol.pointPosition;
			myObject.patrolPoints[index].waitTimeAtPoint = patrol.waitTimeAtPoint;
		}
	}

	void MovePatrolPointDown (int index)
	{
		if (index > 0)
		{
			Patrol.PatrolPoint patrol = new Patrol.PatrolPoint();

			patrol.pointColor = myObject.patrolPoints[index].pointColor;
			patrol.pointName = myObject.patrolPoints[index].pointName;
			patrol.pointPosition = myObject.patrolPoints[index].pointPosition;
			patrol.waitTimeAtPoint = myObject.patrolPoints[index].waitTimeAtPoint;

			myObject.patrolPoints[index].pointColor = myObject.patrolPoints[index - 1].pointColor;
			myObject.patrolPoints[index].pointName = myObject.patrolPoints[index - 1].pointName;
			myObject.patrolPoints[index].pointPosition = myObject.patrolPoints[index - 1].pointPosition;
			myObject.patrolPoints[index].waitTimeAtPoint = myObject.patrolPoints[index - 1].waitTimeAtPoint;

			myObject.patrolPoints[index - 1].pointColor = patrol.pointColor;
			myObject.patrolPoints[index - 1].pointName = patrol.pointName;
			myObject.patrolPoints[index - 1].pointPosition = patrol.pointPosition;
			myObject.patrolPoints[index - 1].waitTimeAtPoint = patrol.waitTimeAtPoint;
		}
	}

	void RemovePatrolPoint (int index)
	{
		myObject.patrolPoints.RemoveAt(index);
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

		EditorGUILayout.BeginVertical("box");
		EditorGUILayout.Space(5);
		EditorGUILayout.BeginHorizontal();
		{
			if (!showPatrolPoints)
			{
				if (GUILayout.Button(" Show Patrol Points (" + myObject.patrolPoints.Count + ")", buttonStyle2, GUILayout.MaxHeight(20f)))
				{
					showPatrolPoints = true;
				}
			}
			else
			{
				if (GUILayout.Button(" Hide Patrol Points (" + myObject.patrolPoints.Count + ")", buttonStyle, GUILayout.MaxHeight(20f)))
				{
					showPatrolPoints = false;
				}
			}
			if (GUILayout.Button(" Add Patrol Point ", buttonStyle2, GUILayout.MaxHeight(20f)))
			{
				AddPatrolPoint();
			}

			//EditorGUILayout.LabelField("Total Patrol Points : " + myObject.patrolPoints.Count);
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space(5);
		EditorGUILayout.EndVertical();

		if(showPatrolPoints)
		{
			EditorGUILayout.BeginVertical("box");
			{
				for (int i = 0; i < myObject.patrolPoints.Count; i++)
				{
					EditorGUILayout.BeginHorizontal(subStyle1);
					{
						myObject.patrolPoints[i].pointName = EditorGUILayout.TextField(myObject.patrolPoints[i].pointName, GUILayout.MaxWidth(100f));
						myObject.patrolPoints[i].pointColor = EditorGUILayout.ColorField(myObject.patrolPoints[i].pointColor, GUILayout.MaxWidth(40f));
						myObject.patrolPoints[i].pointPosition = EditorGUILayout.Vector3Field("", myObject.patrolPoints[i].pointPosition, GUILayout.MaxWidth(140f));
						EditorGUILayout.LabelField("Wait : ", GUILayout.MaxWidth(40f));
						myObject.patrolPoints[i].waitTimeAtPoint = EditorGUILayout.FloatField(myObject.patrolPoints[i].waitTimeAtPoint, GUILayout.MaxWidth(40f));

						if (GUILayout.Button("Copy", buttonStyle2))
						{
							DuplicatePatrolPoint(i);
						}

						if(i < myObject.patrolPoints.Count -1)
						{
							if (GUILayout.Button("v", "box"))
							{
								MovePatrolPointUp(i);
							}
						}

						if (i > 0)
						{
							if (GUILayout.Button("^", "box"))
							{
								MovePatrolPointDown(i);
							}
						}

						if (GUILayout.Button("X", buttonStyle))
						{
							RemovePatrolPoint(i);
						}

					}
					EditorGUILayout.EndHorizontal();
				}
				//EditorGUILayout.PropertyField(patrolPoints);
			
			}
			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.BeginVertical("box");
		{
			EditorGUILayout.PropertyField(lookAtTargetPos);
			if (myObject.lookAtTargetPos)
			{
				EditorGUILayout.BeginVertical(subStyle1);
				{
					EditorGUILayout.PropertyField(smoothLookAt);
					if(myObject.smoothLookAt)
					{
						EditorGUILayout.BeginVertical(subStyle2);
						{
							EditorGUILayout.PropertyField(rotateSpeed);
						}
						EditorGUILayout.EndVertical();
					}

					EditorGUILayout.BeginVertical(subStyle2);
					{
						EditorGUILayout.PropertyField(lockXRotation);
						EditorGUILayout.PropertyField(lockYRotation);
						EditorGUILayout.PropertyField(lockZRotation);
					}
					EditorGUILayout.EndVertical();
					
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.PropertyField(areCoordinatesLocal);
		EditorGUILayout.PropertyField(moveSpeed);
		EditorGUILayout.PropertyField(minDistance);
		EditorGUILayout.PropertyField(loop);

		
		soTarget.ApplyModifiedProperties();
		if (EditorGUI.EndChangeCheck())
		{
			//GUI.FocusControl(GUI.GetNameOfFocusedControl());
		}

		EditorGUILayout.Space();

	}
}
