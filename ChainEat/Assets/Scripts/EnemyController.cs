using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float knockbackForce = 1000000;

    public Text enemyHealth;
    private int enemyHealthCounter = 2;
    public GameObject rock;

    // Update is called once per frame
    void Update()
    {
        enemyHealth.text = "" + enemyHealthCounter;
        if (enemyHealthCounter == 0)
        {
            Destroy(gameObject);
        }
        
        
    }
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Rock")
        {
            //Debug.Log(knockbackForce);
            //gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up + Vector2.right * knockbackForce);
            enemyHealthCounter--;

        }
    }
}
