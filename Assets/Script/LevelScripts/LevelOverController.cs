using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elle2D
{
    /*this class is for level over part we use box collider with trigger 
    when player touches collider new scene button will become active */
    public class LevelOverController : MonoBehaviour
    {
        public PlayerController player;
        private void OnTriggerEnter2D(Collider2D collison)
        {
            if (collison.gameObject.GetComponent<PlayerController>() != null)
            {
                    LevelManager.Instance.MarkCurrentLevelComplete();
                    player.nextSceneButtonImage.gameObject.SetActive(true);
            }
        }
    }
} 