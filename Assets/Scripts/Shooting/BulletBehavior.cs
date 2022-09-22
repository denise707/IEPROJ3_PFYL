using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float ticks = 0.0f;
    private Vector3 firepoint;
    [SerializeField]private float speed = 10;
    private void FixedUpdate()
    {
        ticks += Time.deltaTime;
        if (ticks >= 10)
        {
            Destroy(gameObject);
        }
        transform.position += this.transform.forward * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit");
            collision.gameObject.GetComponent<EnemyBehaviour>().ReceiveDamage(30);

        }

        Destroy(gameObject);
        Debug.Log("Destroy gameobject");
    }




}
