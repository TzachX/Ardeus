using Firebase.Extensions;
using System;
using UnityEngine;

namespace ADV.Core
{
    public class ADVGameManager
    {
        public static ADVGameManager Instance;
        public ADVFactoryManager FactoryManager;
        public ADVEventManager EventManager;
        public ADVCardManager CardManager;
        public ADVResourceManager ResourceManager;
        public ADVConfigManager ConfigManager;
        private Action onCompleteAction;
        public bool IsInitialized = false;
        public bool isGameOver = false;

        public ADVGameManager(Action onComplete)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("2 Managers (Singleton) exist, a new manager wasn't created");
            }

            onCompleteAction = onComplete;
            InitFirebase(InitManagers);
        }

        private void InitFirebase(Action onComplete)
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;

                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    var defaultInstance = Firebase.FirebaseApp.DefaultInstance;
                }
                else
                {
                    Debug.LogError(String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }

                onComplete.Invoke();
            });
        }

        void InitManagers()
        {
            new ADVEventManager(result =>
            {
                EventManager = (ADVEventManager)result;

                new ADVConfigManager(result => 
                {
                    ConfigManager = (ADVConfigManager)result;
                
                    new ADVCardManager(result =>
                    {
                        CardManager = (ADVCardManager)result;

                        new ADVResourceManager(result =>
                        {
                            ResourceManager = (ADVResourceManager)result;

                            new ADVFactoryManager(result =>
                            {
                                FactoryManager = (ADVFactoryManager)result;
                                IsInitialized = true;
                                onCompleteAction.Invoke();
                            });
                        });
                    });
                });
            });
            
        }
    }
}