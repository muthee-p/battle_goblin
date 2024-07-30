using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.Mathematics;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNotxt;
    [SerializeField] private GameObject coin;
    public float moveSpeed;
    public float moveTime;
    public GridNode grid;
   
    
    //private objects
    private int playerNo;
    private List<Node> path;
    private int currentPathIndex = 0;
    private Animator animator;

    Vector3 targetPos;
    

    void Start()
    {    
        playerNo=1;
        playerNotxt.text=playerNo.ToString();
        animator = GetComponent<Animator>();
        
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
            currentPathIndex = 0; // Start from the beginning of the new path
        }

        if (path.Count > 0 && currentPathIndex < path.Count)
        {
            targetPos = path[currentPathIndex].worldPosition; // int targetIndex = Mathf.Clamp(currentPathIndex + stopNodes, 0, path.Count - 1);targetPos = path[targetIndex].worldPosition;
            MovePlayer();
        }
    }


void MovePlayer()
{
    float speed = moveSpeed * Time.deltaTime;
        animator.SetFloat("speed", Mathf.Abs( speed));
    transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);

    // Check if the player has reached the current target position
    if (Vector3.Distance(transform.position, targetPos) < 0.1f)
    {
        currentPathIndex++; // Move to the next node in the path

        // Optionally, you could reset path if the player has reached the end of the path
        if (currentPathIndex >= path.Count)
        {
            path.Clear(); // Clear path if done
                speed = 0;
            currentPathIndex = 0; // Optionally reset index
        }
    }
}

    void OnCollisionEnter2D(Collision2D collision){
        Debug.Log("collided");

        animator.SetTrigger("attack");
        //enemyCollison
        if(collision.gameObject.CompareTag("enemy")){
            GameObject enemy = collision.gameObject;
            Vector3 enemyPos = enemy.transform.position;
            Animator enemyAnimator = enemy.GetComponent<Animator>();
            int enemyNo = enemy.GetComponent<EnemyScript>().enemyNo;

            if(playerNo > enemyNo){
                playerNo += enemyNo;
                GameObject coinDropped = Instantiate(coin, enemyPos+ new Vector3(0f,1f,0), quaternion.identity);

                Destroy(enemy, 3f);

                playerNotxt.text=playerNo.ToString();
               //SizeIncrease();
                Destroy(coinDropped,3f);

            }
            else{
                //enemy.GetComponent<EnemyScript>().enemyAttackAnim.Play();
                animator.SetTrigger("die");
                Invoke("HidePlayer", 3f);
            }
        }

        //coin collision
         //if(collision.gameObject.CompareTag("coin")){
         //   GameObject coin = collision.gameObject;
         //   Destroy(coin);
         //}

        //objectCollision
        if(collision.gameObject.CompareTag("object")){
            GameObject obj= collision.gameObject;
            int objNo = obj.GetComponent<EnemyScript>().enemyNo;
            if(obj.name=="chest")
                playerNo*=objNo;
            else if(obj.name == "barrel")
                playerNo += objNo;
            obj.SetActive(false);

            playerNotxt.text=playerNo.ToString();
            //SizeIncrease();

           
        }
    }

    void HidePlayer() { gameObject.SetActive(false);}

//   void SizeIncrease(){
//    float scaleMultiplier = 1.1f; // Increase by 10%, adjust as needed
//    spriteHolder.transform.localScale *= scaleMultiplier;
//}
    
    
}
