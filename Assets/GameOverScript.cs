using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("die");
    }

    // Update is called once per frame
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
