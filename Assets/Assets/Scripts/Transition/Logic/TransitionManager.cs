using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName]
        public string sceneName = String.Empty;

        private CanvasGroup fadeCanvasGroup;
        private bool isfade = false;
        private IEnumerator Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
         //   Debug.Log(fadeCanvasGroup.gameObject.name);
            yield return StartCoroutine(LoadSceneSetActive(sceneName));
            EventHandler.CallAfterSceneLoadEvent();
        }

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        private void OnTransitionEvent(string arg1, Vector3 arg2)
        {
            if(!isfade)
                StartCoroutine(Transition(arg1, arg2));
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);
            
        }

        /// <summary>
        /// 场景转换
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName, Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            yield return LoadSceneSetActive(sceneName);
            
            EventHandler.CallMoveToPosition(targetPosition);
            
            EventHandler.CallAfterSceneLoadEvent();
            yield return Fade(0);
            
        }
        /// <summary>
        /// 淡入淡出场景
        /// </summary>
        /// <param name="targetAlpha">1是黑，0是透明</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha) 
        {
            isfade = true;
            fadeCanvasGroup.blocksRaycasts = true;
            //Debug.Log("a");
            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha)/Settings.fadeDuration;
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;
            isfade = false;
        }
    }
}