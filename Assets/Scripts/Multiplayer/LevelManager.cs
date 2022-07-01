using System.Collections;
using System.Collections.Generic;
using Fusion;
using FusionExamples.FusionHelpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


namespace Multiplayer
{
	public class LevelManager : NetworkSceneManagerBase
	{
		[SerializeField] private int _lobby;
		[SerializeField] private int[] _levels;

		private Scene _loadedScene;

		public FusionLauncher launcher { get; set; }

		protected override void Shutdown(NetworkRunner runner)
		{
			base.Shutdown(runner);
			if(_loadedScene!=default)
				SceneManager.UnloadSceneAsync(_loadedScene);
			_loadedScene = default;
		}

		protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
		{
			throw new System.NotImplementedException();
		}

		public void LoadLevel(int nextLevelIndex)
		{
			Runner.SetActiveScene(nextLevelIndex < 0 ? _lobby:_levels[nextLevelIndex]);
		}
	}
}