using TMPro;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyNotxt;
    public  Animator enemyAnim; 
    
    
    public int enemyNo;
    Pathfinding pathfinding;
    void Start()
    {
        enemyNotxt.text= enemyNo.ToString();
        enemyAnim=GetComponent<Animator>();
        
    }

    public void SetTarget(){
       pathfinding= GameObject.FindGameObjectWithTag("algorithm").GetComponent<Pathfinding>();
      

        pathfinding.target = transform;

    }

    
}
