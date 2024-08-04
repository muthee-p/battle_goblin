using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.Mathematics;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNotxt;
    [SerializeField] private GameObject coin;
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
        //if (transform.position == targetPos)
        //{
            
        //}

        if (targetPos.x > transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (targetPos.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

    }

    [SerializeField] Sprite openChest;

    void OnCollisionEnter2D(Collision2D collision){
        fighting = true;
        animator.SetFloat("speed", 0);
        //enemyCollison
        if(collision.gameObject.CompareTag("enemy")){
            GameObject enemy = collision.gameObject;
            if (enemy.GetComponent<EnemyScript>().flipPlayer == true) { GetComponent<SpriteRenderer>().flipX = true; }
            if (enemy.GetComponent<EnemyScript>().flipPlayer == false) { GetComponent<SpriteRenderer>().flipX = false; }
            StartCoroutine(CombatSequence(enemy));
            fighting = false;
        }

        //coin collision
         //if(collision.gameObject.CompareTag("coin")){
         //   GameObject coin = collision.gameObject;
         //   Destroy(coin);
         //}

        //objectCollision
        if(collision.gameObject.CompareTag("object")){

            animator.SetTrigger("attack");
            GameObject obj= collision.gameObject;
            int objNo = obj.GetComponent<EnemyScript>().enemyNo;
            if (obj.name == "chest")
            {
                obj.transform.Find("Canvas").gameObject.SetActive(false);
                playerNo *=objNo;
                obj.GetComponent<SpriteRenderer>().sprite=openChest;
            }
               
            else if(obj.name == "barrel")
            {
                playerNo += objNo;            
                Destroy(obj, 1f);
            }
            animator.SetTrigger("objOpen");
            playerNotxt.text=playerNo.ToString();
            fighting = false;
            //SizeIncrease();


        }
    }



    private IEnumerator CombatSequence(GameObject enemy)
    {
        Vector3 enemyPos = enemy.transform.position;
        Animator enemyAnimator = enemy.GetComponent<Animator>();
        int enemyNo = enemy.GetComponent<EnemyScript>().enemyNo;

        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.5f);
        
        enemyAnimator.SetTrigger("takeHit");
        yield return new WaitForSeconds(.5f);

        enemyAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(1f);

        // Character A blocks0
        animator.SetTrigger("block");
        yield return new WaitForSeconds(1f);

        if (playerNo > enemyNo)
        {

            GameObject coinDropped = Instantiate(coin, enemyPos + new Vector3(0f, 1f, 0), quaternion.identity);
    
            enemyAnimator.SetTrigger("die");
            animator.SetTrigger("victory");
            Destroy(enemy, 1.5f);
            animator.SetFloat("speed", 0);
            playerNo += enemyNo;
            playerNotxt.text = playerNo.ToString();
            Destroy(coinDropped, 3f);

        }
        else
        {
            animator.SetTrigger("die");
            enemyAnimator.SetTrigger("victory");
            Invoke("HidePlayer", 1.5f);
        }
    }

        void HidePlayer() { gameObject.SetActive(false);}
    
    
}
