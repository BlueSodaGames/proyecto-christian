using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrushers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public Slider loadingBar;
        public GameObject loadingScreen;
        public GameObject gameOverUITopDown;
        public GameObject gameOverUIPlatformer;
        // Your other variables...

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
