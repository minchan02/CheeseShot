using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCheeze : MonoBehaviour
{

    private Transform player;
    private Vector2 velocity;

    void Start()
    {
        player = Player.instance.transform;
        velocity = Random.insideUnitCircle * 50;
    }

    void Update()
    {
        velocity = Vector2.Lerp(velocity, (player.position - transform.position).normalized * 20, Time.deltaTime * 5);
        transform.Translate(velocity * Time.deltaTime, Space.World);
        if(Vector2.Distance(transform.position, player.position) <= 0.2f)
        {
            Player.instance.AddAttackNode();
            Destroy(gameObject);
        }
    }

}
