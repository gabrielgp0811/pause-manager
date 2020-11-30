using UnityEngine;
using UnityEngine.Events;
#if PAUSE_MANAGER_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace PauseManagement.Core
{
	/// <summary>
	/// 
	/// </summary>
	public class PauseManager : MonoBehaviour
	{
		public delegate void PauseDelegateAction(bool paused);

		public static event PauseDelegateAction PauseAction;

		/// <summary>
		/// Use Unity's timeScale to stop time when paused ?
		/// </summary>
		public bool useTimeScale = true;

		/// <summary>
		/// Use Unity's Input Manager button to pause ?
		/// </summary>
		[SerializeField]
		private bool useUnityInputManager = true;

		/// <summary>
		/// Button name from Unity's Input Manager.
		/// Default is "Cancel"
		/// </summary>
		[SerializeField]
		private string buttonName = "Cancel";

		/// <summary>
		/// Use Unity's new Input System
		/// </summary>
		public bool useUnityInputSystem = false;

#if PAUSE_MANAGER_INPUT_SYSTEM
		/// <summary>
		/// The pause's input action
		/// </summary>
		public InputAction pauseAction = null;

		/// <summary>
		/// Use Input Action Asset's reference ?
		/// </summary>
		public bool useActionReference = false;

		/// <summary>
		/// The Input Action Asset's reference to apply to pauseInputAction
		/// </summary>
		public InputActionReference pauseActionReference = null;
#endif

		/// <summary>
		/// Custom button for pausing, if you don't use Unity's Input Manager
		/// </summary>
		[SerializeField]
		private KeyCode pauseKey = KeyCode.Escape;

		/// <summary>
		/// Assign custom pause button from PlayerPrefs
		/// </summary>
		[SerializeField]
		private bool assignKeyFromPrefs = false;

		/// <summary>
		/// Property from PlayerPrefs to assign a custom pause button
		/// Default is "Pause"
		/// </summary>
		[SerializeField]
		private string propertyFromPrefs = "Pause";

		/// <summary>
		/// Events triggered when paused
		/// </summary>
		[SerializeField]
		private UnityEvent pauseEvent = null;

		/// <summary>
		/// Events triggered when resumed
		/// </summary>
		[SerializeField]
		private UnityEvent resumeEvent = null;

		/// <summary>
		/// 
		/// </summary>
		private bool executeEvents = true;

		/// <summary>
		/// 
		/// </summary>
		private bool executeDelegateActions = true;

		// Awake is called before Start function
		void Awake()
		{
#if PAUSE_MANAGER_INPUT_SYSTEM
			if (useUnityInputSystem)
			{
				if (useActionReference && pauseActionReference)
					pauseAction = pauseActionReference.action;

				pauseAction.performed += _ => TogglePause();
			}
#else
			useUnityInputSystem = false;
#endif
			if (assignKeyFromPrefs && PlayerPrefs.HasKey(propertyFromPrefs))
				pauseKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(propertyFromPrefs));

			IsPaused = false;
		}

		// This function is called when the object becomes enabled and active
		void OnEnable()
		{
#if PAUSE_MANAGER_INPUT_SYSTEM
			pauseAction.Enable();
#endif
		}

		// This function is called when the behaviour becomes disabled.
		void OnDisable()
		{
#if PAUSE_MANAGER_INPUT_SYSTEM
			pauseAction.Disable();
#endif
		}

		// Update is called once per frame
		void Update()
		{
			if (useUnityInputSystem) return;

			if (useUnityInputManager && Input.GetButtonDown(buttonName))
				TogglePause();

			if (!useUnityInputManager && Input.GetKeyDown(pauseKey))
				TogglePause();
		}

		void OnApplicationPause(bool pause)
		{
			if (pause && !IsPaused)
				Pause();
		}

		public void TogglePause()
		{
			if (!IsPaused)
				Pause();
			else
				Resume();
		}

		public void Pause()
		{
			if (useTimeScale)
				StopTime();

			IsPaused = true;

			if (executeEvents)
				pauseEvent.Invoke();

			if (executeDelegateActions && PauseAction != null)
				PauseAction.Invoke(IsPaused);
		}

		public void Resume()
		{
			ResetTime();

			IsPaused = false;

			if (executeEvents)
				resumeEvent.Invoke();

			if (executeDelegateActions && PauseAction != null)
				PauseAction.Invoke(IsPaused);
		}

		public void StopTimeDelayed(float time)
		{
			Invoke("StopTime", time);
		}

		public void StopTime()
		{
			Time.timeScale = 0;
		}

		public void ResetTimeDelayed(float time)
		{
			Invoke("ResetTime", time);
		}

		public void ResetTime()
		{
			Time.timeScale = 1;
		}

		public void SavePauseKeyOnPrefs()
		{
			PlayerPrefs.SetString(propertyFromPrefs, pauseKey.ToString());
		}

		public static bool IsPaused { get; set; }

		public bool ExecuteEvents
		{
			set
			{
				executeEvents = value;
			}
		}

		public bool ExecuteDelegateActions
		{
			set
			{
				executeDelegateActions = value;
			}
		}
	}
}