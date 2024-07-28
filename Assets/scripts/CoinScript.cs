using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private  GameObject player;
    public float moveSpeed;

    void Start(){
        player=GameObject.FindGameObjectWithTag("Player");
    }
    // void Update(){
    //     Vector3 targetPos = player.transform.position;
    //     float speed = moveSpeed * Time.deltaTime;
    //     transform.position = Vector3.MoveTowards(transform.position, targetPos,speed); 
       
    // }
}
