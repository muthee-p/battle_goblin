using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNotxt;
    [SerializeField] private GameObject coin;
    [SerializeField] private AudioSource slash, chestopen, woodBreak, coinSound;
    public float moveSpeed;
    public float moveTime;
    public GridNode grid;
    public bool fighting;
   
    
    //private objects
    private int playerNo;
    private List<Node> path;
    private int currentPathIndex = 0;
    private Animator animator;

    Vector3 targetPos;
    float speed;
    int visibleEnemies;

    void Start()
    {    
        playerNo=1;
        playerNotxt.text=playerNo.ToString();
        animator = GetComponent<Animator>();
        fighting= false;
        // Start with an empty path
        path = new List<Node>();
    }


    void Update()
    {
        if (grid == null)
        {
            
            return;
        }

        if (grid.path == null)
        {
            
            return;
        }

        if (grid.path != path)
        {
            path = new List<Node>(grid.path);
            currentPathIndex = 0; 
        }

        if (path.Count > 0 && currentPathIndex < path.Count)
        {
            //targetPos = path[currentPathIndex].worldPosition;

            int targetIndex = Mathf.Clamp(currentPathIndex + 1, 0, path.Count - 1);
            targetPos = path[targetIndex].worldPosition;
            MovePlayer();
        }

        }


void MovePlayer()
{
    speed = moveSpeed * Time.deltaTime;
        animator.SetFloat("speed", Mathf.Abs( speed));
    transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
        if (transform.position == targetPos)
        {
            animator.SetFloat("speed", 0);
        }

        if (targetPos.x > transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (targetPos.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

    }

    [SerializeField] Sprite openChest, brokenBarrel;

    void OnCollisionEnter2D(Collision2D collision){
        //visibleEnemies = 0;
        
        animator.SetFloat("speed", 0);
        //enemyCollison
        if(collision.gameObject.CompareTag("enemy")){
            GameObject enemy = collision.gameObject;
            slash.Play();
            if(enemy.name=="shroom" || enemy.name == "boss") { enemy.transform.Find("shadow").gameObject.SetActive(false); }
            if (enemy.GetComponent<EnemyScript>().flipPlayer == true) { GetComponent<SpriteRenderer>().flipX = true; }
            if (enemy.GetComponent<EnemyScript>().flipPlayer == false) { GetComponent<SpriteRenderer>().flipX = false; }
            StartCoroutine(CombatSequence(enemy));
            fighting = false;
        }

        //objectCollision
        if(collision.gameObject.CompareTag("object")){

            animator.SetTrigger("attack");
            slash.PlayOneShot(slash.clip);
            GameObject obj= collision.gameObject;
            int objNo = obj.GetComponent<EnemyScript>().enemyNo;
            obj.transform.Find("Canvas").gameObject.SetActive(false);
            if (obj.name == "chest")
            {
                chestopen.Play();
                playerNo *=objNo;
                obj.GetComponent<SpriteRenderer>().sprite=openChest;
            }
               
            else if(obj.name == "barrel")
            {
                woodBreak.Play();
                playerNo += objNo;
                obj.GetComponent<SpriteRenderer>().sprite = brokenBarrel;
                obj.layer = 6;
            }
            animator.SetTrigger("objOpen");
            playerNotxt.text=playerNo.ToString();
            fighting = false;
            //CheckEnemyNo();

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
        yield return new WaitForSeconds(.5f);

        // Character A blocks0
        animator.SetTrigger("block");
        yield return new WaitForSeconds(.5f);

        if (playerNo > enemyNo)
        {
            int coinCount = 0;
            if (enemyNo < 10) coinCount = 1;
            else if (enemyNo > 10) coinCount = Mathf.Abs(enemyNo / 10);
            else if (enemyNo > 100) coinCount = Mathf.Abs(enemyNo / 100)+4;
            for (int i = 0; i < coinCount; i++){
                GameObject coinDropped = Instantiate(coin, enemyPos + new Vector3(Random.Range(.2f,2f), Random.Range(.2f, 2f), 0), Quaternion.identity);
                coinSound.Play();
            }
    
            enemyAnimator.SetTrigger("die");
            animator.SetTrigger("victory");
            Destroy(enemy, 1.5f);
            animator.SetFloat("speed", 0);
            playerNo += enemyNo;
            playerNotxt.text = playerNo.ToString();
            //CheckEnemyNo();
            if (enemy.name == "boss")
            {
                StartCoroutine(GameOver(3));
            }

        }
        else
        {
            animator.SetTrigger("die");
            enemyAnimator.SetTrigger("victory");
            Invoke("HidePlayer", 1.5f);
            StartCoroutine(GameOver(2));
        }
        slash.Stop();   
    }

    [SerializeField] CanvasGroup blackScreen;
    private IEnumerator GameOver(int level)
    {
        blackScreen.gameObject.SetActive(true);
        float fadeDuration = 2;
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            blackScreen.alpha = Mathf.Clamp01(timeElapsed / fadeDuration);
            yield return null;
        }
        blackScreen.alpha = 1;
        SceneManager.LoadScene(level);
       yield return null;
    }

    public Camera camera1;
    void CheckEnemyNo()
    {
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject[] objects = GameObject.FindGameObjectsWithTag("object");
        foreach (GameObject enemy in enemies)
        {
            Renderer renderer = enemy.GetComponent<Renderer>();
            if (renderer != null && IsVisibleFrom(enemy, camera1))
            {
                visibleEnemies++;
            }
        }
        foreach (var item in objects)
        {
            Renderer renderer = item.GetComponent<Renderer>();
            if (renderer != null && IsVisibleFrom(item,camera1))
            {
                visibleEnemies++;
            }
        }
        if (visibleEnemies == 0)
        {
            StartCoroutine(GameOver(2));
            Invoke("HidePlayer", 1.5f);
        }
        Debug.Log(visibleEnemies);
    }

    bool IsVisibleFrom(GameObject enemy, Camera camera)
    {
        Renderer ren = enemy.GetComponent<Renderer>();
        Vector3 viewportpos = camera.WorldToViewportPoint(ren.bounds.center);
        if (viewportpos.x >= 0 &&  viewportpos.x <= 1 && viewportpos.y >=0 & viewportpos.y <= 0 && viewportpos.z > 0)
        {
            return true;
        }
        return false;
    }

     void HidePlayer() { gameObject.GetComponent<SpriteRenderer>().enabled = false; }

    public void ReStart()
    {
        SceneManager.LoadScene(1);
    }

}
