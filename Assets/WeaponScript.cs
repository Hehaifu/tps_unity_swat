using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public float avgDamage;
    public float stdDamage;
    public float fireInterval;
    public float recoil;
    public float bulletForce = 20f;

    public int ammoCapacity = 30;
    public int availableAmmo = 30;
    //public int carryAmmo = 50;

    [SerializeField] string gunName;
    [SerializeField] GameObject muzzleFlash;
    //[SerializeField] GameObject bullet;
    [SerializeField] float muzzleEffectDuration = 1.0f;
    [SerializeField] GameObject bulletImpact;  //弹孔来自于磁盘
    //[SerializeField] GameObject bulletObject;
    //[SerializeField] Transform bulletHolder;

    public Sprite icon;
    private WeaponManager weaponManager;
    private Collider collider;
    private Vector3 boxColliderCenter;
    private Vector3 boxColliderSize;



    private void Awake()
    {
        TurnOffMuzzleEffect();
        weaponManager = FindObjectOfType<WeaponManager>();
        collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("no collider is attached in this weapon");
        }
        BoxCollider boxCollider = (BoxCollider)collider;
        boxColliderCenter = boxCollider.center;
        boxColliderSize = boxCollider.size;
    }

    public BoxCollider AddCollider()
    {
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider>();
        }
        ((BoxCollider)collider).center = boxColliderCenter;
        ((BoxCollider)collider).size = boxColliderSize;
        return (BoxCollider)collider;
    }

    public string GetName()
    {
        //用于ui获得枪的名字
        return gunName;
    }
    public void PlayFireEffect() 
    {
        //没有子弹的情况
        if(availableAmmo <= 0)
        {
            //if(carryAmmo <= 0)
            //{
            //    print("no ammo");
            //    return;
            //}
            //if (carryAmmo < ammoCapacity)
            //{
            //    availableAmmo = carryAmmo;
            //    carryAmmo = 0;
            //}
            //else
            //{
            //    carryAmmo -= ammoCapacity;
            //    availableAmmo = ammoCapacity;
            //}
            int carryAmmo = weaponManager.getCarryAmmo(gunName);
            if (carryAmmo <= 0)
            {
                print("no ammo");
                return;
            }
            int obtaniedAmmo = weaponManager.useCarryAmmo(gunName, ammoCapacity, 0);
            availableAmmo = obtaniedAmmo;
        }
        //拥有子弹
        availableAmmo -= 1;
        weaponManager.UpdateAmmo(availableAmmo, gunName);
        muzzleFlash.SetActive(true);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            Vector3 hitPoint = hit.point;
            print("hit " + hit.collider.name);
            GameObject bulletImpactObj = Instantiate<GameObject>(bulletImpact);
            bulletImpactObj.transform.position = hitPoint;
            //gameObject.transform.LookAt(hitPoint);
            GameObject hitObject = hit.collider.gameObject;
            Rigidbody hitRigidbody = hitObject.GetComponent<Rigidbody>();
            if (hitRigidbody !=null)
            {
                hitRigidbody.AddForceAtPosition(ray.direction * bulletForce, hitPoint);
            }
        }

        Invoke("TurnOffMuzzleEffect", muzzleEffectDuration);
        //GameObject bullet = Instantiate<GameObject>(bulletObject);
        //bullet.transform.position = bulletHolder.position;
        //bullet.transform.rotation = bulletHolder.rotation;
        //bullet.SetActive(true);

    }

    public bool IsFiring()
    {
        return muzzleFlash.activeSelf;
    }

    void TurnOffMuzzleEffect()
    {
        muzzleFlash.SetActive(false);
    }
    //武器落地后用collider来触发
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = false;
        rigidbody.angularVelocity = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        collider = GetComponent<Collider>();
        collider.isTrigger = true;

    }
}
