using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {
	private Color ambientLight;
	public float restartTime=2.0f;
	public Light sun;
	private Color sunColor;
	public string levelToLoad;

	private void RestartLevel(){
		string currentScene = SceneManager.GetActiveScene ().name;
		SceneManager.LoadScene (currentScene);
	}

	private void ExitToMenu(){
		SceneManager.LoadScene ("Menu");
	} 

	private void ExitGame(){
		Application.Quit ();
	}

	private void LoadLevel(){
		SceneManager.LoadScene (levelToLoad);
	}

	public void InitRestart(){
		ambientLight = RenderSettings.ambientLight;
		sunColor = sun.color;
		StartCoroutine ("AnimateRestart");

	}

	public void CancelRestart(){
		StopCoroutine ("AnimateRestart");
		RenderSettings.ambientLight = ambientLight;
		sun.color = sunColor;
	}

	private IEnumerator AnimateRestart(){
		float startTime = Time.time;
		Color setLight;
		Color setSun;
		while (Time.time - startTime < restartTime) {
			setLight = Color.Lerp (ambientLight, Color.black, (Time.time - startTime) / restartTime);
			RenderSettings.ambientLight = setLight;
			setSun = Color.Lerp (sunColor, Color.black, (Time.time - startTime) / restartTime);
			sun.color = setSun;
			yield return null;
		}
		RestartLevel ();
	}

	public void InitExitToMenu(){
		ambientLight = RenderSettings.ambientLight;
		sunColor = sun.color;
		StartCoroutine ("AnimateExitToMenu");

	}

	public void CancelExitToMenu(){
		StopCoroutine ("AnimateExitToMenu");
		RenderSettings.ambientLight = ambientLight;
		sun.color = sunColor;
	}

	private IEnumerator AnimateExitToMenu(){
		float startTime = Time.time;
		Color setLight;
		Color setSun;
		while (Time.time - startTime < restartTime) {
			setLight = Color.Lerp (ambientLight, Color.black, (Time.time - startTime) / restartTime);
			RenderSettings.ambientLight = setLight;
			setSun = Color.Lerp (sunColor, Color.black, (Time.time - startTime) / restartTime);
			sun.color = setSun;
			yield return null;
		}
		ExitToMenu ();
	}

	public void InitQuit(){
		ambientLight = RenderSettings.ambientLight;
		sunColor = sun.color;
		StartCoroutine ("AnimateQuit");

	}

	public void CancelQuit(){
		StopCoroutine ("AnimateQuit");
		RenderSettings.ambientLight = ambientLight;
		sun.color = sunColor;
	}

	private IEnumerator AnimateQuit(){
		float startTime = Time.time;
		Color setLight;
		Color setSun;
		while (Time.time - startTime < restartTime) {
			setLight = Color.Lerp (ambientLight, Color.black, (Time.time - startTime) / restartTime);
			setSun = Color.Lerp (sunColor, Color.black, (Time.time - startTime) / restartTime);
			sun.color = setSun;
			RenderSettings.ambientLight = setLight;
			yield return null;
		}
		ExitGame ();
	}


	public void InitLoadLevel(){
		ambientLight = RenderSettings.ambientLight;
		sunColor = sun.color;
		StartCoroutine ("AnimateLoadLevel");

	}

	public void CancelLoadLevel(){
		StopCoroutine ("AnimateLoadLevel");
		RenderSettings.ambientLight = ambientLight;
		sun.color = sunColor;
	}

	private IEnumerator AnimateLoadLevel(){
		float startTime = Time.time;
		Color setLight;
		Color setSun;
		while (Time.time - startTime < restartTime) {
			setLight = Color.Lerp (ambientLight, Color.black, (Time.time - startTime) / restartTime);
			setSun = Color.Lerp (sunColor, Color.black, (Time.time - startTime) / restartTime);
			sun.color = setSun;
			RenderSettings.ambientLight = setLight;
			yield return null;
		}
		LoadLevel ();
	}
}
