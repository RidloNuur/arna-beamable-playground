using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace Arna.Runtime
{
    public class RuntimeManager : MonoBehaviour
    {
        private static RuntimeManager _instance;

        public static RuntimeManager Instance
        {
            get
            {
                return _instance;
            }
        }

        #region PUBLIC VARIABLES
        public string defaultScene;
        public Camera defaultCamera;
        public int sceneLoadingDelay;
        #endregion

        #region STATIC VARIABLES / PROPERTIES
        public static bool IsReady => _instance != null;
        public static bool IsLoadingScene { get; private set; }
        #endregion

        #region PRIVATE VARIABLES
        private static List<string> _activeScenes;
        #endregion

        #region CONSTANT VARIABLES
        private const string RUNTIME_SCENE = "Runtime";
        #endregion

        #region EVENTS
        public static Action onRuntimeReady;
        public static Action<string> onSceneStartLoad;
        public static Action<string> onSceneEndLoad;
        public static Action<Scene, AsyncOperation> onSceneLoading;
        #endregion

        #region INITIALIZATION
        [RuntimeInitializeOnLoadMethod]
        private static async void Initialize()
        {
            _activeScenes = new List<string>();
            await UniTask.Yield();
            if (_instance == null && SceneManager.GetActiveScene().name != RUNTIME_SCENE)
            {
                _activeScenes.Add(SceneManager.GetActiveScene().name);
                await SceneManager.LoadSceneAsync(RUNTIME_SCENE, LoadSceneMode.Additive);
            }
            else if (SceneManager.GetActiveScene().name != _instance.defaultScene)
            {
                await LoadingScene(_instance.defaultScene, LoadSceneMode.Single);
            }
            onRuntimeReady?.Invoke();
        }

        private void Awake()
        {
            _instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            CameraCheck();
        }
        #endregion

        #region SCENE MANAGEMENT
        public static void LoadScene(string sname, LoadSceneMode mode)
        {
            if (_activeScenes.Contains(sname))
                return;

            LoadingScene(sname, mode);
        }

        private static async UniTask LoadingScene(string sname, LoadSceneMode mode)
        {
            await UniTask.WaitForEndOfFrame();
            GameState.ChangeState(GameState.State.LOADING);
            onSceneStartLoad?.Invoke(sname);
            await UniTask.Delay(_instance.sceneLoadingDelay);

            if (mode == LoadSceneMode.Single)
                await UnloadAllScenes();

            var loading = SceneManager.LoadSceneAsync(sname, LoadSceneMode.Additive);
            onSceneLoading?.Invoke(SceneManager.GetSceneByName(sname), loading);
            await loading;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sname));

            await UniTask.Delay(_instance.sceneLoadingDelay);
            onSceneEndLoad?.Invoke(sname);
        }

        private static async UniTask UnloadAllScenes()
        {
            foreach (var e in _activeScenes)
            {
                Debug.Log("Unloading scene: " + e);
                await SceneManager.UnloadSceneAsync(e);
            }
            _activeScenes.Clear();
        }
        private void OnSceneLoaded(Scene scn, LoadSceneMode mode)
        {
            if (scn.name == RUNTIME_SCENE)
                return;

            Debug.Log("Scene loaded: " + scn.name);
            CameraCheck();
            _activeScenes.Add(scn.name);
        }

        private void OnSceneUnloaded(Scene scn)
        {
            Debug.Log("Scene unloaded: " + scn.name);
            CameraCheck();
        }

        private void CameraCheck()
        {
            defaultCamera.gameObject.SetActive(FindObjectsOfType(typeof(Camera), true).Length <= 1);
        }
        #endregion
    }
}