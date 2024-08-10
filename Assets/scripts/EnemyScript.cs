using System.Collections;
using TMPro;
using UnityEngine;


public class EnemyScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyNotxt;
    [SerializeField] private GameObject destinationCell;
    public  Animator enemyAnim; 
    public int enemyNo;
    public bool flipPlayer;
    public float scaleFactor = 1.2f;
    public float animationDuration = .2f;
    AudioSource clickSound;
    bool fighting;

    Pathfinding pathfinding;
    PlayerScript playerScript;
   
    void Start()
    {
       
        enemyNotxt.text= enemyNo.ToString();
        enemyAnim=GetComponent<Animator>();
        clickSound=GetComponent<AudioSource>();
        if (gameObject.name == "chest")
        {
            enemyNotxt.text  = "x" + enemyNo;
        }
        
    }

    public void SetTarget(){ 
        clickSound.Play();
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
        StartCoroutine(AnimateScale());
        //}
       

    }
    private IEnumerator AnimateScale()
    {
        Vector3 startScale = transform.localScale;
        Vector3 scale = startScale * scaleFactor;
        
        float elaspedTime = 0f;
        while (elaspedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, scale, elaspedTime / animationDuration);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = scale;

        while (elaspedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, startScale, elaspedTime / animationDuration);
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = startScale;
    }


}
