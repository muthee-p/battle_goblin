using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyNotxt;
    public  Animator enemyAnim; 
    public int enemyNo; 
    bool fighting;

    Pathfinding pathfinding;
    PlayerScript playerScript;
   
    void Start()
    {
       
        enemyNotxt.text= enemyNo.ToString();
        enemyAnim=GetComponent<Animator>();
        
    }

    public void SetTarget(){ 
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        fighting = playerScript.fighting;
        if (!fighting)
        {
            if(gameObject.name == "barrel")
            {
                gameObject.layer = 0;
            }
            pathfinding= GameObject.FindGameObjectWithTag("algorithm").GetComponent<Pathfinding>();
            pathfinding.target = transform;
        }
       

    }

    
}
