using UnityEngine;
using UnityEngine.Events;

namespace PauseManagement.Core
{
	/// <summary>
	/// 
	/// </summary>
	public class PauseEventHandler : MonoBehaviour
	{
		/// <summary>
		/// Events to be triggered when game is paused
		/// </summary>
		[SerializeField]
		private UnityEvent pauseEvents = null;

		/// <summary>
		/// Events to be triggered when game is resumed
		/// </summary>
		[SerializeField]
		private UnityEvent resumeEvents = null;

		// This function is called when the object becomes enabled and active
		void OnEnable()
		{
			PauseManager.PauseAction += PauseHandler;
		}

		// This function is called when the behaviour becomes disabled.
		void OnDisable()
		{
			PauseManager.PauseAction -= PauseHandler;
		}

		void PauseHandler(bool paused)
		{
			if (paused)
				pauseEvents.Invoke();
			else
				resumeEvents.Invoke();
		}
	}
}