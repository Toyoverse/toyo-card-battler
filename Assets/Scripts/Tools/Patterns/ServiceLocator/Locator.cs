using System;
using Player;
using UnityEngine;

namespace ServiceLocator
{
    
    public class Locator : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        private static SceneControl _sceneControlService;
        private static GlobalConfig _globalConfigService;

        public static SceneControl GetSceneControl() => _sceneControlService;
        public static void Provide(SceneControl service)
        { 
            if(_sceneControlService != null) return;
            _sceneControlService = service;
        }

        public static GlobalConfig GetGlobalConfig() => _globalConfigService;

        public static void Provide(GlobalConfig service)
        {
            if(_globalConfigService != null) return;
            _globalConfigService = service;
        }
    }
}