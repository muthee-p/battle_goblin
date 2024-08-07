using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource slash;
    [SerializeField] Transform target;
    [SerializeField] CanvasGroup blackScreen;
    public float fadeDuration;
    private Animator animator;
    bool isMoving=false;    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving == true)
            transform.position = Vector3.MoveTowards(transform.position, target.position, 3);
    }

    public void PlayGame()
    {
        animator.SetFloat("speed", 1);
        isMoving = true;
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetFloat("speed", 1);
        //enemyCollison
        if (collision.gameObject.CompareTag("enemy"))
        {
            GameObject enemy = collision.gameObject;
            slash.Play();
            if (enemy.GetComponent<EnemyScript>().flipPlayer == true) { GetComponent<SpriteRenderer>().flipX = true; }
            if (enemy.GetComponent<EnemyScript>().flipPlayer == false) { GetComponent<SpriteRenderer>().flipX = false; }
            StartCoroutine(CombatSequence(enemy));
        }
    }
    private IEnumerator CombatSequence(GameObject enemy)
    {
        Vector3 enemyPos = enemy.transform.position;
        Animator enemyAnimator = enemy.GetComponent<Animator>();
        int enemyNo = enemy.GetComponent<EnemyScript>().enemyNo;

        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.2f);

        enemyAnimator.SetTrigger("takeHit");
        yield return new WaitForSeconds(.2f);

        enemyAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(.4f);

        // Character A blocks0
        animator.SetTrigger("block");
        yield return new WaitForSeconds(.4f);

        animator.SetTrigger("attack");
        yield return new WaitForSeconds(.6f);

        enemyAnimator.SetTrigger("die");
        yield return new WaitForSeconds(.6f);
        animator.SetTrigger("victory");
        slash.Stop();

        animator.SetFloat("speed", 0);

        blackScreen.gameObject.SetActive(true);
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            blackScreen.alpha=Mathf.Clamp01(timeElapsed/fadeDuration);
            yield return null;
        }
        blackScreen.alpha = 1;
        SceneManager.LoadScene(1);


    }
}
