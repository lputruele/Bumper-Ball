using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI
{
    public class LoadScene : MonoBehaviour
    {
        public GameObject loadingScreen;
        public Image loadingBarFill;
        public AudioClip pressSound;

        public void LoadTargetScene(string sceneName)
        {
            //AudioUtility.CreateSFX(pressSound, transform.position, AudioUtility.AudioGroups.HUDVictory, 0f);
            StartCoroutine(LoadTargetSceneAsync(sceneName));
        }

        IEnumerator LoadTargetSceneAsync(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName); 
            loadingScreen.SetActive(true);
            while (!operation.isDone)
            {
                float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
                loadingBarFill.fillAmount = progressValue;
                yield return null;
            }
            loadingScreen.SetActive(false);
        }
    }
}
