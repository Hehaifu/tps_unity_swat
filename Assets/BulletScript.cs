using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage = 30;
    public float flySpeed = 2f;
    private Rigidbody rigidbody;
    [SerializeField] GameObject groundBulletImpact;
    [SerializeField] GameObject bulletMesh;
    private bool hit = false;

    private void Awake()
    {
        groundBulletImpact.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit)
        {
            gameObject.transform.Translate(0f, 0f, flySpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Rifle"))
        {
            hit = true;
            groundBulletImpact.SetActive(true);
            bulletMesh.SetActive(false);
            Destroy(gameObject,5);
        }
    }
}
