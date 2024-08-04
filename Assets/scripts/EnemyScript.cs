using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyNotxt;
    [SerializeField] private GameObject destinationCell;
    public  Animator enemyAnim; 
    public int enemyNo;
    public bool flipPlayer;
    bool fighting;

    Pathfinding pathfinding;
    PlayerScript playerScript;
   
    void Start()
    {
       
        enemyNotxt.text= enemyNo.ToString();
        enemyAnim=GetComponent<Animator>();
        if (gameObject.name == "chest")
        {
            enemyNotxt.text  = "x" + enemyNo;
        }
        
    }

    public void SetTarget(){ 

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        fighting = playerScript.fighting;
        //if (!fighting)
        //{
            if(gameObject.name == "barrel")
            {
                gameObject.layer = 0;
            }
            pathfinding= GameObject.FindGameObjectWithTag("algorithm").GetComponent<Pathfinding>();
            pathfinding.target = destinationCell.transform;
        //}
       

    }

    
}
