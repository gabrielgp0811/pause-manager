using System;
using UnityEngine;

namespace PauseManagement.Editor
{
	using UnityEditor;
	using Core;

	/// <summary>
	/// 
	/// </summary>
	[CustomEditor(typeof(PauseManager))]
	public class PauseManagerEditor : Editor
	{
		SerializedProperty scriptProp;

		SerializedProperty useTimeScaleProp;
		SerializedProperty useUnityInputManagerProp;
		SerializedProperty buttonNameProp;
		SerializedProperty useUnityInputSystemProp;
		SerializedProperty pauseKeyProp;
		SerializedProperty assignKeyFromPrefsProp;
		SerializedProperty propertyFromPrefsProp;
		SerializedProperty onPauseEventProp;
		SerializedProperty onResumeEventProp;
#if PAUSE_MANAGER_INPUT_SYSTEM
		SerializedProperty pauseActionProp;
		SerializedProperty useActionReferenceProp;
		SerializedProperty pauseActionReferenceProp;
#endif

		KeyCode pauseKeyCode = KeyCode.None;
		Array keyCodeArray;

		void OnEnable()
		{
			scriptProp = serializedObject.FindProperty("m_Script");

			useTimeScaleProp = serializedObject.FindProperty("useTimeScale");
			useUnityInputManagerProp = serializedObject.FindProperty("useUnityInputManager");
			buttonNameProp = serializedObject.FindProperty("buttonName");
			useUnityInputSystemProp = serializedObject.FindProperty("useUnityInputSystem");
			pauseKeyProp = serializedObject.FindProperty("pauseKey");
			assignKeyFromPrefsProp = serializedObject.FindProperty("assignKeyFromPrefs");
			propertyFromPrefsProp = serializedObject.FindProperty("propertyFromPrefs");
			onPauseEventProp = serializedObject.FindProperty("pauseEvent");
			onResumeEventProp = serializedObject.FindProperty("resumeEvent");
#if PAUSE_MANAGER_INPUT_SYSTEM
			pauseActionProp = serializedObject.FindProperty("pauseAction");
			useActionReferenceProp = serializedObject.FindProperty("useActionReference");
			pauseActionReferenceProp = serializedObject.FindProperty("pauseActionReference");
#endif

			keyCodeArray = Enum.GetValues(typeof(KeyCode));
			pauseKeyCode = (KeyCode)keyCodeArray.GetValue(pauseKeyProp.enumValueIndex);
		}

		public override void OnInspectorGUI()
		{
			// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
			serializedObject.Update();

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(scriptProp);
			EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("General Properties", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			useTimeScaleProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Use time scale?", "Use Unity's time scale to pause/resume the game?"), useTimeScaleProp.boolValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Controller Properties", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			useUnityInputManagerProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Use Input Manager?", "Use entries of Unity's Input Manager defined on 'Project Settings > Input' ?"), useUnityInputManagerProp.boolValue);
			EditorGUILayout.EndHorizontal();

			if (useUnityInputManagerProp.boolValue)
			{
				EditorGUILayout.BeginHorizontal();
				buttonNameProp.stringValue = EditorGUILayout.TextField(new GUIContent("Button's Name:", "The name of the entry in Unity's Input Manager for pause button. Default is 'Cancel'"), buttonNameProp.stringValue, GUILayout.ExpandWidth(true));
				EditorGUILayout.EndHorizontal();

				// If using Unity's Input Manager, must disable Unity's Input System usage
				useUnityInputSystemProp.boolValue = false;
			}
			else
			{
				EditorGUILayout.BeginHorizontal();
				useUnityInputSystemProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Use Input System?", "Use bindings of Unity's Input System ?"), useUnityInputSystemProp.boolValue);
				EditorGUILayout.EndHorizontal();

				if (useUnityInputSystemProp.boolValue)
				{
#if PAUSE_MANAGER_INPUT_SYSTEM
					EditorGUILayout.BeginHorizontal();
					useActionReferenceProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Use reference?", "Use Input Action Asset's reference ?"), useActionReferenceProp.boolValue);
					EditorGUILayout.EndHorizontal();

					if (useActionReferenceProp.boolValue)
						EditorGUILayout.PropertyField(pauseActionReferenceProp, new GUIContent("Action Reference"));
					else
						EditorGUILayout.PropertyField(pauseActionProp);
#else
					EditorGUILayout.HelpBox("The Unity's Input System is not installed.", MessageType.Warning, true);
#endif
					assignKeyFromPrefsProp.boolValue = false;
				}
				else
				{
					EditorGUILayout.BeginVertical();

					EditorGUILayout.BeginHorizontal();
					assignKeyFromPrefsProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Use PlayerPrefs?", "Assign custom pause key from PlayerPrefs?"), assignKeyFromPrefsProp.boolValue);
					EditorGUILayout.EndHorizontal();

					if (assignKeyFromPrefsProp.boolValue)
					{
						EditorGUILayout.BeginHorizontal();
						propertyFromPrefsProp.stringValue = EditorGUILayout.TextField(new GUIContent("Property's Name:", "Property from PlayerPrefs to assign a custom pause key. Default is 'Pause'"), propertyFromPrefsProp.stringValue, GUILayout.ExpandWidth(true));
						EditorGUILayout.EndHorizontal();
					}
					else
					{
						EditorGUILayout.BeginHorizontal();
						pauseKeyCode = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Pause Key:", "The key code for pausing"), pauseKeyCode, GUILayout.ExpandWidth(true));
						EditorGUILayout.EndHorizontal();

						for (int i = 0; i < keyCodeArray.Length; i++)
						{
							if (pauseKeyCode == (KeyCode)keyCodeArray.GetValue(i))
							{
								pauseKeyProp.enumValueIndex = i;
								break;
							}
						}
					}

					EditorGUILayout.EndVertical();
				}
			}

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Events Properties", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.PropertyField(onPauseEventProp, GUILayout.ExpandWidth(true));

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(onResumeEventProp, GUILayout.ExpandWidth(true));

			// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
			serializedObject.ApplyModifiedProperties();
		}
	}
}