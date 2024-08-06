using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private  GameObject parent;
    public float minMoveSpeed, maxMoveSpeed;
    Vector3 _velocity = Vector3.zero;
    Vector3 targetPos;
    public bool isFollowing = false;

    void Start(){
        parent = GameObject.FindGameObjectWithTag("parent");
        targetPos = parent.transform.position;
        //Debug.Log(parent.transform.position);
        Invoke("StartFollowing", .8f);
    }

    void StartFollowing()
    {
        isFollowing = true;
    }
    void Update()
    {
        if (isFollowing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, Time.deltaTime*Random.Range(minMoveSpeed, maxMoveSpeed));

        }

    }
}
