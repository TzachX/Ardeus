using ADV.Core.Components;
using UnityEngine.SceneManagement;

namespace ADV.Core.Loader
{
    public class ADVGameLoader : ADVMonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void Start()
        {
            int i = 0;
            new ADVGameManager(() =>
            {
                SceneManager.LoadScene("MainGameScene");

            });
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainGameScene")
            {
                Destroy(gameObject);
            }
        }
    }
}