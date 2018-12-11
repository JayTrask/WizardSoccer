using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadButton : GUIButtons {

	public string sceneToLoad;

	public override void WhenPressed()
	{
		StartCoroutine(LoadLevel());
	}

	IEnumerator LoadLevel()
	{
		AsyncOperation theLoadOp = SceneManager.LoadSceneAsync(sceneToLoad);

		while (!theLoadOp.isDone)
		{
			yield return null;
		}
	}
}
