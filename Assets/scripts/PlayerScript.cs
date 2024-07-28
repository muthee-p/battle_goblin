using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNotxt;
    [SerializeField] private GameObject coin, spriteHolder;
    [SerializeField] private Pathfinding pathfinding;
    public float moveSpeed;
    public float moveTime;
    
    //private objects
    private Rigidbody2D rb;
    private int playerNo;
    Transform targetPos;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    
        playerNo=3;
        playerNotxt.text=playerNo.ToString();

        targetPos = pathfinding.target;
    }


    public void MoveToEnemy(Transform _targetPos){
       
        targetPos= _targetPos;
    }
    void FixedUpdate(){
         float speed= moveSpeed* Time.fixedDeltaTime;
    //     // Calculate the difference between the current position and the target position
    // float deltaX = Mathf.Abs(transform.position.x - targetPos.x);
    // float deltaY = Mathf.Abs(transform.position.y - targetPos.y);

    // // Determine whether to move horizontally or vertically
    // if (deltaX > deltaY) {
    //     // Move horizontally
    //     targetPos.y = transform.position.y; // Keep the y-coordinate constant
    // } else {
    //     // Move vertically
    //     targetPos.x = transform.position.x; // Keep the x-coordinate constant
    // }

    // Move towards the target position
        
        transform.position = Vector3.MoveTowards(transform.position, targetPos.position, speed); 
    }

    void OnCollisionEnter2D(Collision2D collision){
        
        //enemyCollison
        if(collision.gameObject.CompareTag("enemy")){
            GameObject enemy= collision.gameObject;
            Vector3 enemyPos = enemy.transform.position;
            int enemyNo = enemy.GetComponent<EnemyScript>().enemyNo;
            if(playerNo>enemyNo){
                playerNo+=enemyNo;
                GameObject coinDropped = Instantiate(coin, enemyPos+ new Vector3(0f,1f,0), quaternion.identity);
                enemy.SetActive(false);

                playerNotxt.text=playerNo.ToString();
               SizeIncrease();
                Destroy(coinDropped,3f);

            }
            else{
                //enemy.GetComponent<EnemyScript>().enemyAttackAnim.Play();
                gameObject.SetActive(false);
            }
        }

        //coin collision
         if(collision.gameObject.CompareTag("coin")){
            GameObject coin = collision.gameObject;
            Destroy(coin);
         }

        //objectCollision
        if(collision.gameObject.CompareTag("object")){
            GameObject obj= collision.gameObject;
            int objNo = obj.GetComponent<EnemyScript>().enemyNo;
            playerNo+=objNo;
            obj.SetActive(false);

            playerNotxt.text=playerNo.ToString();
            SizeIncrease();

           
        }
    }

   void SizeIncrease(){
    float scaleMultiplier = 1.1f; // Increase by 10%, adjust as needed
    spriteHolder.transform.localScale *= scaleMultiplier;
}
    
    
}
