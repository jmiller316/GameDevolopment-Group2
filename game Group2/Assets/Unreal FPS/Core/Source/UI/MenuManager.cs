/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnrealFPS
{
	public class MenuManager : MonoBehaviour
	{
		[Header("Loading")]
		[SerializeField] private Transform loadingBar;
		[SerializeField] private Text loadingText;
		[SerializeField] private Text progress;
		[SerializeField] private string compliteLoadingText;
		[SerializeField] private KeyCode openLoadedSceneKey;

		[Header("Scenes")]
		[SerializeField] private Transform sceneList;
		[SerializeField] private Transform sceneTemplate;
		[SerializeField] private Scene[] exceptions;

		private string levelName;
		private float loadingSceneProgress;

		/// <summary>
		/// 
		/// </summary>
		public void LoadScene()
		{
			StartCoroutine(LoadAsyncScene());
		}

		/// <summary>
		/// 
		/// </summary>
		public void QuitGame()
		{
			Application.Quit();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="levelName"></param>
		public void SetLevelName(string levelName)
		{
			this.levelName = levelName;
		}

		public float GetLoadingProgress()
		{
			return loadingSceneProgress;
		}

		public void OpenWeb(string action)
		{
			switch (action)
			{
				case "Documentation":
					Application.OpenURL("https://docs.google.com/document/d/1H6NnAOCbOH2GLAAi3XyTcFFI0szRGHgk8ysuvuly9Ag/edit?usp=sharing");
					break;
				case "Forum":
					Application.OpenURL("https://assetstore.unity.com/packages/templates/systems/next-gen-fps-81972");
					break;
				case "Twitter":
					Application.OpenURL("https://docs.google.com/document/d/1H6NnAOCbOH2GLAAi3XyTcFFI0szRGHgk8ysuvuly9Ag/edit?usp=sharing");
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private IEnumerator LoadAsyncScene()
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
			asyncLoad.allowSceneActivation = false;
			loadingBar.gameObject.SetActive(true);
			while (!asyncLoad.isDone)
			{
				if (loadingSceneProgress < 100)
				{
					progress.text = loadingSceneProgress.ToString() + "%";
				}
				else
				{
					progress.text = "";
				}
				if (asyncLoad.progress == 0.9f)
				{
					loadingSceneProgress = Mathf.MoveTowards(loadingSceneProgress, 100, 100 * Time.deltaTime);
					if (loadingSceneProgress == 100)
					{
						loadingText.text = compliteLoadingText;
						if (Input.GetKeyDown(openLoadedSceneKey))
						{
							asyncLoad.allowSceneActivation = true;
						}
					}
				}
				else
				{
					loadingSceneProgress = Mathf.MoveTowards(loadingSceneProgress, 100, 3 * Time.deltaTime);
				}
				yield return null;
			}
		}

		public void GenerateSceneList(Scene[] exceptions)
		{

		}
	}
}