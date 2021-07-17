using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elle2D
{
    /*this class is comes in use when player dies and also for button in menu*/
    public class RestartLevel : MonoBehaviour
    { 
        public string restartGame;
        public string Scene01;
        public string Scene02;
        public void onButtonClick()
        {
            SceneManager.LoadScene(restartGame);
        }
        public void onEasyButtonClick()
        {
            SceneManager.LoadScene(Scene01);
        }
        public void onHardButtonClick()
        {
            SceneManager.LoadScene(Scene02);
        }
    }
}
