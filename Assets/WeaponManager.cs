using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject[] rifles; //prefabs
    List<GameObject> riflesInGame;  //GameObject
    [SerializeField] GameObject grenade; //prefabs 
    [SerializeField] float throwSpeed = 20f;
    [SerializeField] float throwGunSpeed = 2f;
    [SerializeField] List<GunAmmo> gunAmmoMap;

    public GameObject currentWeapon;
    private Transform rifleCarryHolder;
    private Transform rifleAimHolder;
    private Transform weaponBack;
    private Transform grenadeHolder;
    private Transform rifleLeftHolder;
    private Animator _animator;
    public bool needToSwitchWeapon = true;
    private UIController uiController;
    private StarterAssetsInputs inputs;
    private Dictionary<string, int> gunAmmoDictionary;

    public int currentWeaponIdx;
    public int grenadeCount = 3;
    public int MaxGrenageCapacity = 5;

    private void Awake()
    {
        currentWeaponIdx = 0;
        rifleCarryHolder = GameObject.FindGameObjectWithTag("RifleCarryHolder").transform;
        rifleAimHolder = GameObject.FindGameObjectWithTag("RifleAimHolder").transform;
        weaponBack = GameObject.FindGameObjectWithTag("WeaponBackHolder").transform;
        grenadeHolder = GameObject.FindGameObjectWithTag("GrenadeHolder").transform;
        uiController = FindObjectOfType<UIController>();
        rifleLeftHolder = GameObject.FindGameObjectWithTag("RifleLeftHolder").transform;
        inputs = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
        riflesInGame = new List<GameObject>();

        gunAmmoDictionary = new Dictionary<string, int>();
        foreach(GunAmmo gunAmmo in gunAmmoMap)
        {
            gunAmmoDictionary[gunAmmo.gunName] = gunAmmo.carryAmmo;
        }
    }

    public int getCarryAmmo(string gunName)
    {
        return gunAmmoDictionary[gunName];
    }

    public int useCarryAmmo(string gunName,int magizineCap, int askAmmo)
    {
        if (askAmmo <= 0)
        {
            //ammo
            askAmmo = magizineCap;
        }
        if(gunAmmoDictionary[gunName] > askAmmo)
        {
            gunAmmoDictionary[gunName] -= askAmmo;
            return askAmmo;
        }
        else
        {
           int allLeft = gunAmmoDictionary[gunName];
            gunAmmoDictionary[gunName] = 0;
            return allLeft;
        }
    }
    private void Start()
    {
        if (rifles.Length > 0)
        {
            GameObject weapon = Instantiate<GameObject>(rifles[0]);
            weapon.tag = "Rifle";
            Destroy(weapon.GetComponent<Rigidbody>());
            Transform weaponTransform = weapon.transform;
            weaponTransform.parent = rifleCarryHolder;
            weaponTransform.localPosition = Vector3.zero;
            weaponTransform.localRotation = Quaternion.Euler(Vector3.zero);
            currentWeapon = weapon;
            riflesInGame.Add(weapon);
            WeaponScript script = weapon.GetComponent<WeaponScript>();
            if (script != null)
            {
                uiController.setWeapon(riflesInGame.Count - 1, script.icon);
                uiController.setAmmoText(riflesInGame.Count - 1, script.availableAmmo, gunAmmoDictionary[script.GetName()]);
            }
        }
        for (int i =0 ; i<rifles.Length ; i++)
        {
            if (i == currentWeaponIdx)
            {
                continue;
            }
            GameObject weapon = Instantiate<GameObject>(rifles[i]);
            weapon.tag = "Rifle";
            Destroy(weapon.GetComponent<Rigidbody>());
            Transform weaponTransform = weapon.transform;
            weaponTransform.parent = weaponBack;
            weaponTransform.localPosition = Vector3.zero;
            weaponTransform.localRotation = Quaternion.Euler(Vector3.zero);
            riflesInGame.Add(weapon);
            WeaponScript script = weapon.GetComponent<WeaponScript>();
            if (script != null)
            {
                uiController.setWeapon(riflesInGame.Count - 1, script.icon);
                uiController.setAmmoText(riflesInGame.Count - 1, script.availableAmmo, gunAmmoDictionary[script.GetName()]);
            }
        }

        uiController.deselectAllWeapons();
    }

    public void AddNewWeapon(GameObject weapon)
    {
        riflesInGame.Add(weapon);
        WeaponScript script = weapon.GetComponent<WeaponScript>();
        if (script != null)
        {
            //uiController.setWeapon(riflesInGame.Count - 1, script.icon);
            uiController.refreshStatus(riflesInGame , gunAmmoDictionary);
        }
        
    }

    public void EnableCurrentWeaponUI()
    {
        uiController.selectWeapon(currentWeaponIdx);
    }

    public void DisableCurrentWeaponUI()
    {
        uiController.deselectAllWeapons();
    }

    public void UpdateAmmo(int ammo, string gunName)
    {
        uiController.setAmmoText(currentWeaponIdx,ammo,gunAmmoDictionary[gunName]);
    }
    public void UseNextWeapon()
    {
        if (riflesInGame.Count == 0)
        {
            return;
        }
        currentWeaponIdx++;
        if (currentWeaponIdx >= riflesInGame.Count)
        {
            currentWeaponIdx = 0;
        }
        currentWeapon = riflesInGame[currentWeaponIdx];
        if (rifleAimHolder.childCount > 0)
        {
            uiController.selectWeapon(currentWeaponIdx);
        }
        MoveWeapon();
    }
    public void UsePrevWeapon()
    {
        if (riflesInGame.Count == 0)
        {
            return;
        }
        currentWeaponIdx--;
        if (currentWeaponIdx < 0)
        {
            currentWeaponIdx = riflesInGame.Count - 1;
        }
        currentWeapon = riflesInGame[currentWeaponIdx];
        if (rifleAimHolder.childCount > 0)
        {
            uiController.selectWeapon(currentWeaponIdx);
        }
        MoveWeapon();
    }

    public void UseLastWeapon() 
    {
        if (riflesInGame.Count == 0)
        {
            return;
        }
        currentWeaponIdx = riflesInGame.Count -1;
        currentWeapon = riflesInGame[currentWeaponIdx];
        if (rifleAimHolder.childCount > 0)
        {
            uiController.selectWeapon(currentWeaponIdx);
        }
        MoveWeapon();
    }

    bool CheckIfWeaponInWaist()
    {
        if (rifleCarryHolder.childCount >0)
        {
            return true;
        }
        return false;
    }

    bool CheckIfWeaponInHands()
    {
        if (rifleAimHolder.childCount > 0)
        {
            return true;
        }
        return false;
    }

    void EqiupNewWeapon()
    {
        if (CheckIfWeaponInHands())
        {

        }
        else
        {
            
        }
    }

    void MoveWeapon()
    {
        needToSwitchWeapon = false;
        if (_animator.GetBool("Weapon")) 
        {
            needToSwitchWeapon = true;
        }
        if (CheckIfWeaponInHands()) 
        {
            //if a gun in hands put it directly on the hands
            Transform oldGun = rifleAimHolder.GetChild(0);
            oldGun.parent = weaponBack;
            oldGun.localPosition = Vector3.zero;
            oldGun.localRotation = Quaternion.Euler(Vector3.zero);

            if (CheckIfWeaponInWaist())
            {
                //weapon in waist
                for (int i = 0; i< rifleCarryHolder.childCount; i++)
                {
                    Transform oldGun2 = rifleCarryHolder.GetChild(i);
                    oldGun2.parent = weaponBack;
                    oldGun2.localPosition = Vector3.zero;
                    oldGun2.localRotation = Quaternion.Euler(Vector3.zero);
                }
                
            }

            //current weapon -> wait
            currentWeapon.transform.parent = rifleCarryHolder;
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);

            if (needToSwitchWeapon)
            {
                _animator.SetTrigger("switchWeapon");
            }
            
        }
        else
        {
            if (CheckIfWeaponInWaist())
            {
                //weapon in waist
                Transform oldGun = rifleCarryHolder.GetChild(0);
                oldGun.parent = weaponBack;
                oldGun.localPosition = Vector3.zero;
                oldGun.localRotation = Quaternion.Euler(Vector3.zero);
            }

            currentWeapon.transform.parent = rifleCarryHolder;
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        //if (CheckIfWeaponInWaist())
        // {
        //    Transform oldGun = rifleCarryHolder.GetChild(0);
        //     oldGun.parent = weaponBack;
        //     oldGun.localPosition = Vector3.zero;
        //     oldGun.localRotation = Quaternion.Euler(Vector3.zero);
        // }
        // if (CheckIfWeaponInHands())
        // {
        //
        //}
        if (rifleAimHolder.childCount == 0)
        {
            uiController.deselectAllWeapons();
        }
        
    }
    public void FireWeapon()
    {
        
        if (currentWeapon == null)
        {
            print("no weapon in hand");
            return;
        }
        WeaponScript weaponScript = currentWeapon.GetComponent<WeaponScript>();
        if (weaponScript == null)
        {
            return;
        }
        weaponScript.PlayFireEffect();
    }

    public bool IsGunFiring()
    {
        if (currentWeapon == null)
        {
            print("no weapon in hand");
            return false;
        }
        WeaponScript weaponScript = currentWeapon.GetComponent<WeaponScript>();
        if (weaponScript == null)
        {
            return false;
        }
        return weaponScript.IsFiring();
    }

    public void ThrowGrenade()
    {
        if(grenadeCount <= 0)
        {
            return;
        }
        _animator.SetTrigger("Toss");
    }

    public void PutGrenadeInHand()
    {
        GameObject grenadeObject = Instantiate<GameObject>(grenade);
        grenadeObject.transform.parent = grenadeHolder;
        grenadeObject.transform.localPosition = Vector3.zero;
        grenadeObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void PutGrenadeOutOfHand()
    {
        if(grenadeHolder.childCount == 0)
        {
            return;
        }
        Transform grenade = grenadeHolder.GetChild(0);
        Vector3 throwDirection = transform.forward;
        grenade.parent = null;
        grenade.gameObject.AddComponent<CapsuleCollider>();
        Rigidbody rigidbody = grenade.gameObject.AddComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidbody.velocity = throwDirection * throwSpeed;
        GrenadeScript grenadeScript= grenade.gameObject.GetComponent<GrenadeScript>();
        if (grenadeScript != null)
        {
            grenadeScript.StartCounter();
        }
        grenadeCount--;
    }

    public void HoldGrenadeAndWeapon()
    {
        if (currentWeapon == null)
        {
            print("no weapon in hand");
            return;
        }
        PutGrenadeInHand();
        currentWeapon.transform.parent = rifleLeftHolder;
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void MoveWeaponToRightHand()
    {
        if (currentWeapon == null)
        {
            print("no weapon in hand");
            return;
        }
        currentWeapon.transform.parent = rifleAimHolder;
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void AddGrenade()
    {
        grenadeCount++;
        if (grenadeCount >MaxGrenageCapacity)
        {
            grenadeCount = MaxGrenageCapacity;
        }
    }
    public void RemoveGrenade()
    {
        grenadeCount--;
        if (grenadeCount < 0)
        {
            grenadeCount = 0;
        }
    }

    public void ThrowWeapon()
    {
        if (currentWeapon == null)
        {
            print("no weapon in hand");
            return;
        }
        //¸üÐÂUI
        //uiController.removeWeapon(currentWeaponIdx);
        currentWeapon.transform.parent = null;
        Rigidbody rigidbody = currentWeapon.GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            rigidbody = currentWeapon.AddComponent<Rigidbody>();
        }
        rigidbody.useGravity = true;
        //CapsuleCollider collider = currentWeapon.AddComponent<CapsuleCollider>();
        Collider collider = currentWeapon.GetComponent<Collider>();
        if(collider == null)
        {
            collider = currentWeapon.GetComponent<WeaponScript>().AddCollider();
        }
        collider.isTrigger = false;
        rigidbody.velocity = transform.forward * throwGunSpeed;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        riflesInGame.RemoveAt(currentWeaponIdx);
        uiController.refreshStatus(riflesInGame , gunAmmoDictionary);
        currentWeapon.tag = "Weapon";
        currentWeapon = null;
        currentWeaponIdx = 0;
        inputs.weaponState = false;
        _animator.SetTrigger("ThrowWeapon");
        if (riflesInGame.Count == 0)
        {
            return;
        }
        else
        {
            currentWeapon = riflesInGame[currentWeaponIdx];
            currentWeapon.transform.parent = rifleCarryHolder;
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
            _animator.SetTrigger("Carry");
        }
    }

    public bool hasWeapon()
    {
        if(riflesInGame.Count > 0)
        {
            return true;
        }
        return false;
    }
}

[Serializable]
struct GunAmmo
{
    public string gunName;
    public int carryAmmo;
}
