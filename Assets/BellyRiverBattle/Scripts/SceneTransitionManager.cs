using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] FadeScreen fadeScreen; 
    public static int LastSceneIndex { private set; get; }

    private void Start()
    {
        LastSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex));
    }
    public void GoToAsyncScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }
    IEnumerator GoToSceneRoutine(int sceneIndex)
    {
        WaitForSeconds waitTime= new WaitForSeconds(fadeScreen._fadeDuration);
       fadeScreen.FadeOut();
        yield return waitTime;

        //Launch new scene
        SceneManager.LoadScene(sceneIndex);
    }
    IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();

        //Launch new scene
        AsyncOperation operation=SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0;
        while(timer<=fadeScreen._fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        operation.allowSceneActivation = true;
    }

    public void fadeOutScene()
    {
        Debug.Log("hit");
        gameObject.SendMessage("FadeOutScene"); //added to enable scene transitions from video controller
    }

    //ToDisable. Used only for testing scene transitions.
    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            int currentSceneIndex = LastSceneIndex;
            GoToAsyncScene(currentSceneIndex+=1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int currentSceneIndex = LastSceneIndex;
            GoToAsyncScene(currentSceneIndex -=1);
        }
    }*/
}
