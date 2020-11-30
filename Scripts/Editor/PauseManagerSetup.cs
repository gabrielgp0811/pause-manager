using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace PauseManagement.Editor
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public class PauseManagerSetup
	{
		/// <summary>
		/// Scripting Define Symbol for Unity's Input System
		/// </summary>
		private const string INPUT_SYSTEM_DEFINE = "PAUSE_MANAGER_INPUT_SYSTEM";

		private static readonly List<string> Symbols = new List<string>();

		// Minimum version of Unity's Input System required is 0.2.10-preview
		private const int MINIMUM_RELEASE_VERSION = 0;
		private const int MINIMUM_MAJOR_VERSION = 2;
		private const int MINIMUM_MINOR_VERSION = 10;

		/// <summary>
		/// Eveytime Unity loads, the static constructor is executed
		/// </summary>
#if UNITY_EDITOR
		static PauseManagerSetup()
		{
			CheckPackagePresency("com.unity.inputsystem", INPUT_SYSTEM_DEFINE);
		}
#endif

		private static void CheckPackagePresency(string packageNameOrId, string define)
		{
			// Getting package's full path
			string path = Path.GetFullPath(string.Format("Packages/{0}", packageNameOrId));

			// Checking if Input System is installed by searching for the package in 'Packages' directory
			bool isPresent = Directory.Exists(path);

			if (isPresent)
			{
				string file = Path.GetFileName(path);
				string version = file.Split('@')[1];
				int release = int.Parse(version.Split('.')[0]);
				int major = int.Parse(version.Split('.')[1]);
				int minor = int.Parse(version.Split('.')[2].Split('-')[0]);

				if (release > MINIMUM_RELEASE_VERSION)
					AddDefine(define);
				else if (release == MINIMUM_RELEASE_VERSION)
					if (major > MINIMUM_MAJOR_VERSION)
						AddDefine(define);
					else if (major == MINIMUM_MAJOR_VERSION)
						if (minor >= MINIMUM_MINOR_VERSION)
							AddDefine(define);
						else
							RemoveDefine(define);
					else
						RemoveDefine(define);
				else
					RemoveDefine(define);
			}
			else
			{
				RemoveDefine(define);
			}
		}

		private static void AddDefine(string define)
		{
			if (IsDefineAdded(define)) return;

			if (!Symbols.Contains(define))
				Symbols.Add(define);

			var group = EditorUserBuildSettings.selectedBuildTargetGroup;
			var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
			List<string> allDefines = definesString.Split(';').ToList();

			allDefines.AddRange(Symbols.Except(allDefines));

			PlayerSettings.SetScriptingDefineSymbolsForGroup(
					 group,
					 string.Join(";", allDefines.ToArray())
			);
		}

		private static void RemoveDefine(string define)
		{
			if (!IsDefineAdded(define)) return;

			var group = EditorUserBuildSettings.selectedBuildTargetGroup;
			var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
			List<string> allDefines = definesString.Split(';').ToList();

			allDefines.Remove(define);

			PlayerSettings.SetScriptingDefineSymbolsForGroup(
					 group,
					 string.Join(";", allDefines.ToArray())
			);
		}

		private static bool IsDefineAdded(string define)
		{
			var group = EditorUserBuildSettings.selectedBuildTargetGroup;
			var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
			List<string> allDefines = definesString.Split(';').ToList();

			return allDefines.Contains(define);
		}
	}
}