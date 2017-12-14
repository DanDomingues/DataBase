using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public  static class SceneUtility
{

    private static List<string> AddedSceneNames
    {
        get
        {
            List<string> names = new List<string>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                names.Add(SceneManager.GetSceneAt(i).name);
            }
            return names;
        }
    }

    /// <summary>
    /// Returns true if any loaded scene's name contains the 'domain' parameter.
    /// </summary>
    /// <param name="domain">Domain to be checked</param>
    /// <returns></returns>
    public static bool CheckForDomain(string domain) 
    {
        bool result = false;

         for(int i =  0; i < SceneManager.sceneCount;i++)
        {
            if (SceneManager.GetSceneAt(i).name.Contains(domain))
                result = true;
        }

        return result;
    }

    /// <summary>
    /// Returns true if the current scene is the only loaded scene.
    /// </summary>
    /// <returns></returns>
    public static bool CheckIfAlone()
    {

        if (SceneManager.sceneCount == 1)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Starts a scene transition and unloads current scene between fades.
    /// Requires Package scene or a derived scene to work. 
    /// </summary>
    /// <param name="curScene">Scene to be unloaded</param>
    /// <param name="nextScene">Scene to be loaded</param>
    /// <param name="duration">Duration of the transition</param>
    /// <param name="action">Action to be executed</param>
    public static Coroutine SceneTransition(string curScene, string nextScene, float duration)
    {
        //return filter.StartCoroutine(SceneTransitionEffect(curScene, nextScene, filter, duration));
        return TransitionUtility.Transition(duration, SceneTransitionEffect(curScene, nextScene, null));
    }

    /// <summary>
    /// Starts a scene transition and runs an action between fades.
    /// Requires Package scene or a derived scene to work. 
    /// </summary>
    /// <param name="curScene">Scene to be unloaded</param>
    /// <param name="nextScene">Scene to be loaded</param>
    /// <param name="duration">Duration of the transition</param>
    /// <param name="action">Action to be executed</param>
    public static Coroutine SceneTransition(string curScene, string nextScene, float duration, UnityAction action)
    {
        //return filter.StartCoroutine(SceneTransitionEffect(curScene, nextScene, filter, duration));
        return TransitionUtility.Transition(duration, SceneTransitionEffect(curScene, nextScene, action));
    }

    /// <summary>
    /// Starts a scene transition and runs an action between fades.
    /// Requires Package scene or a derived scene to work. 
    /// </summary>
    /// <param name="curScene">Scene to be unloaded</param>
    /// <param name="nextScene">Scene to be loaded</param>
    /// <param name="duration">Duration of the transition</param>
    /// <param name="routine">Action to be executed</param>
    public static Coroutine SceneTransition(string curScene, string nextScene, float duration, IEnumerator routine)
    {
        //return filter.StartCoroutine(SceneTransitionEffect(curScene, nextScene, filter, duration));
        return TransitionUtility.Transition(duration, SceneTransitionEffect(TransitionUtility.TransitionImage, curScene, nextScene, routine));
    }

    /// <summary>
    /// Starts a scene transition and runs an action between fades.
    /// Requires Package scene or a derived scene to work. 
    /// </summary>
    /// <param name="curScene">Scene to be unloaded</param>
    /// <param name="nextScene">Scene to be loaded</param>
    /// <param name="duration">Duration of the transition</param>
    /// <param name="routine">Action to be executed</param>
     public static Coroutine SceneTransition(string curScene, string nextScene, float duration, IEnumerator routine,  UnityAction action)
    {
        //return filter.StartCoroutine(SceneTransitionEffect(curScene, nextScene, filter, duration));
        return TransitionUtility.Transition(duration, SceneTransitionEffect(TransitionUtility.TransitionImage, curScene, nextScene, routine, action));
    }


    /// <summary>
    /// Scene transition effect used in local function
    /// </summary>
    private static IEnumerator SceneTransitionEffect(string curScene, string nextScene, UnityAction action)
    {
        //sceneTransitionCleared = false;

        yield return SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        if(action != null) action();
        yield return new WaitForEndOfFrame();

        yield return SceneManager.UnloadSceneAsync(curScene);

        yield return new WaitForEndOfFrame();
        //yield return new WaitUntil(() => { return sceneTransitionCleared; });

        Time.timeScale = 1;

    }

    /// <summary>
    /// Scene transition effect used in local function
    /// </summary>
    private static IEnumerator SceneTransitionEffect(Image transitionImage, string curScene, string nextScene, IEnumerator routine)
    {
        //sceneTransitionCleared = false;

        yield return SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        if (routine != null) yield return transitionImage.StartCoroutine(routine);
        yield return new WaitForEndOfFrame();

        yield return SceneManager.UnloadSceneAsync(curScene);

        yield return new WaitForEndOfFrame();
        //yield return new WaitUntil(() => { return sceneTransitionCleared; });

        Time.timeScale = 1;

    }

    /// <summary>
    /// Scene transition effect used in local function
    /// </summary>
    private static IEnumerator SceneTransitionEffect(Image transitionImage, string curScene, string nextScene, IEnumerator routine, UnityAction action)
    {
        //sceneTransitionCleared = false;

        yield return SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        yield return new WaitForEndOfFrame();

        if (action != null) yield return transitionImage.StartCoroutine(routine);
        yield return new WaitForEndOfFrame();

        action();
        yield return new WaitForEndOfFrame();

        yield return SceneManager.UnloadSceneAsync(curScene);

        yield return new WaitForEndOfFrame();
        //yield return new WaitUntil(() => { return sceneTransitionCleared; });

        Time.timeScale = 1;

    }


    /// <summary>
    /// Merges a scene into the base scene through their names
    /// </summary>
    /// <param name="baseSceneName">Scene to receive the added scene</param>
    /// <param name="addedSceneName">Scene to be added in the base Scene</param>
    public static void MergeScenes(string baseSceneName , string addedSceneName)
    {


        if (!AddedSceneNames.Contains(baseSceneName) || !AddedSceneNames.Contains(addedSceneName))
        {
            Debug.Log("Merge Scenes error: One or more of the scenes requested are not loaded at the moment");
            return;

        }

        SceneManager.MergeScenes(SceneManager.GetSceneByName(addedSceneName), SceneManager.GetSceneByName(baseSceneName));
    }

    /// <summary>
    /// Loads a scene in additive mode. Won't load if scene is already loaded.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public static Coroutine LoadAdditive(MonoBehaviour starter, string sceneName)
    {
        return starter.StartCoroutine(LoadAdditiveEffect(sceneName));
    }

    /// <summary>
    /// Loads a scene in additive mode. Won't load if scene is already loaded.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private static IEnumerator LoadAdditiveEffect(string sceneName)
    {
        if (!AddedSceneNames.Contains(sceneName))
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// Loads a scene in additive mode. Won't load if scene is already loaded.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public static Coroutine UnloadScene(MonoBehaviour starter, string sceneName)
    {
        return starter.StartCoroutine(UnloadSceneEffect(sceneName));
    }

    /// <summary>
    /// Unloads a loaded scene. Will only work if the scene is present when the method is called.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private static IEnumerator UnloadSceneEffect(string sceneName)
    {
        if (AddedSceneNames.Contains(sceneName))
        {
            yield return SceneManager.UnloadSceneAsync(sceneName );
        }
    }


    public static Coroutine ResetScene(string sceneName, float duration)
    {
        return TransitionUtility.Transition(duration, ResetSceneEffect(sceneName));
             
    }

    /// <summary>
    /// Loads a scene in additive mode. Won't load if scene is already loaded.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private static IEnumerator ResetSceneEffect(string sceneName)
    {
        if (AddedSceneNames.Contains(sceneName))
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
 

}
