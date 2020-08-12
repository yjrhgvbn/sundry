using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Player player;
    public float distance;

    private float speed = 4;
    private float maxSpeed = 3;
    private Vector3 velocity = Vector3.zero;
    private Vector3 playerPosPre;
    private float timer = 0f;
    //private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        //rigidbody2d = GetComponent<Rigidbody2D>();
        speed = player.speed;
        maxSpeed = player.maxSpeed;
        playerPosPre = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 dir = (player.transform.position - playerPosPre).normalized;
        Vector3 playerSpeed = (player.transform.position - playerPosPre) / Time.deltaTime;
        Vector3 playerPredictPos = player.transform.position + playerSpeed * 0.5f;     //预测玩家位置
        Vector3 targetpos = (transform.position - playerPredictPos).normalized * distance + playerPredictPos;
        
        float d = Mathf.Min(1, Time.deltaTime / 0.5f);
        Vector3 dirSpeed = targetpos - transform.position;
        if (Vector3.Distance(transform.position, player.transform.position) < distance)
        {
            d *= 2;
            velocity = velocity * (1 - d) + (transform.position - playerPredictPos).normalized * (distance*2f - Vector3.Distance(transform.position, playerPredictPos)) * d;
        }
        else
        {
            velocity = velocity * (1 - d) + dirSpeed.normalized * Mathf.Clamp(dirSpeed.sqrMagnitude, 0, 5) * d;
        }
        transform.position += velocity * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            Debug.Log(dirSpeed.normalized * Mathf.Clamp(dirSpeed.sqrMagnitude, 0, 4));
            timer = 0f;
        }
        playerPosPre = player.transform.position;
        //rigidbody2d.velocity = velocity;
    }
    
}


