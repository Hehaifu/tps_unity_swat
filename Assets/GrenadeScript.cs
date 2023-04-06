using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    public float countingSecounds = 2f;
    [SerializeField] public float radius = 5.0f;
    [SerializeField] public float power = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartCounter()
    {
        Invoke("Explosion", countingSecounds);
    }

    void Explosion()
    {
        GameObject explosionObj = Instantiate<GameObject>(explosion);
        explosionObj.transform.position = transform.position;
        //Õ¨·É¶«Î÷
        Collider[] colliders = Physics.OverlapSphere(transform.position,radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(power, transform.position, radius, 3.0f);
            }
        }
        Destroy(gameObject);
    }
}
