using UnityEngine;

public class Spikes : MonoBehaviour
{
    private GameObject playerObject;
    public float knockbackForce;

    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            playerObject.GetComponent<PlayerController>().TakeDamage(1);
            playerObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * knockbackForce);
        }
    }
} 