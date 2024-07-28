using TMPro;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyNotxt;
    //[SerializeField] public  Animation enemyAttackAnim; 
    public PlayerScript playerScript;
    public int enemyNo;
    
   
    float x,y;
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        enemyNotxt.text= enemyNo.ToString();
    }

    public void SummonPlayer(){
        playerScript.MoveToEnemy(transform);

    }
}
