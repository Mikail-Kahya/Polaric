using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Difference between LevelSelector and level transitioner is
// in game feature like level end and menu feature
public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string _nextLevel = null;


    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
            SceneManager.LoadScene(_nextLevel);
    }
}
