using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Cinemachine;
using UnityEngine.UI;

public class TPSShootController : MonoBehaviour
{
    // Start is called before the first frame update
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController tpController;
    [SerializeField] float aimMouseSensitivity = 0.5f;
    [SerializeField] float followMouseSensitivity = 1f;
    [SerializeField] Image aimCross;

    [SerializeField] CinemachineVirtualCamera aimCamera;
    [SerializeField] Transform rifleAimHolder;
    [SerializeField] Transform rifleCarryHolder;
    private float _rotationVelocity;
    [SerializeField] float bodyRotationWithWeaponMoving;
    [SerializeField] float bodyRotationWithWeaponStanding;
    [SerializeField] float RotationSmoothTime = 0.12f;
    [SerializeField] Transform bodyMesh;
    private WeaponManager weaponManager;
    private NotificationManager notificationManager;
    private Animator animator;

    [SerializeField] public static GameObject toBePickedUp;
    GameObject pickUpItem;
    [SerializeField] GameObject centerDebug;

    void Awake()
    {
        aimCamera.gameObject.SetActive(false);
        aimCross.gameObject.SetActive(false);
        tpController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        weaponManager = GetComponent<WeaponManager>();
        animator = GetComponent<Animator>();
        notificationManager = FindObjectOfType<NotificationManager>();
        rifleAimHolder = GameObject.FindGameObjectWithTag("RifleAimHolder").transform;
        rifleCarryHolder = GameObject.FindGameObjectWithTag("RifleCarryHolder").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (starterAssetsInputs.weaponState)
        {
            if (starterAssetsInputs.aim)
            {
                aimCamera.gameObject.SetActive(true);
                aimCross.gameObject.SetActive(true);
                tpController.SetMouseSensitivity(aimMouseSensitivity);
            }
            else
            {
                aimCamera.gameObject.SetActive(false);
                aimCross.gameObject.SetActive(false);
                tpController.SetMouseSensitivity(followMouseSensitivity);
            }

            if (starterAssetsInputs.move != Vector2.zero)
            {
                float rotation = Mathf.SmoothDampAngle(bodyMesh.localEulerAngles.y,
                    bodyRotationWithWeaponMoving, ref _rotationVelocity, RotationSmoothTime);
                bodyMesh.localRotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                float rotation = Mathf.SmoothDampAngle(bodyMesh.localEulerAngles.y,
                    bodyRotationWithWeaponStanding, ref _rotationVelocity, RotationSmoothTime);
                bodyMesh.localRotation = Quaternion.Euler(0, rotation, 0);
            }
        }
        else 
        {
            bodyMesh.localRotation = Quaternion.Euler(Vector3.zero);
        }
        // CenterGun();

    }

    public void PullOutRifle()
    {
        //print("PullOutRifle");
        //GameObject rifle = GameObject.FindGameObjectWithTag("Rifle");  找tag
        GameObject rifle = weaponManager.currentWeapon; 
        if (rifle != null)
        {
            rifle.transform.parent = rifleAimHolder;
            rifle.transform.localPosition = Vector3.zero;
            rifle.transform.localRotation = Quaternion.Euler(Vector3.zero);
            weaponManager.EnableCurrentWeaponUI();
        }
    }
    void CenterGun()
    {
        Vector3 worldCenterPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.farClipPlane));
        centerDebug.transform.position = worldCenterPos;
        GameObject rifle = weaponManager.currentWeapon;
        if (rifle != null)
        {
            rifle.transform.LookAt(worldCenterPos);
        }
    }

    public void PutBackRifle()
    {
        //print("PutBackRifle");
        //GameObject rifle = GameObject.FindGameObjectWithTag("Rifle");
        GameObject rifle = weaponManager.currentWeapon;  
        if (rifle != null)
        {
            rifle.transform.parent = rifleCarryHolder;
            rifle.transform.localPosition = Vector3.zero;
            rifle.transform.localRotation = Quaternion.Euler(Vector3.zero);
            weaponManager.DisableCurrentWeaponUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            notificationManager.ShowPickUpNotification();
            toBePickedUp = other.gameObject;
        }
        if (other.CompareTag("Item"))
        {
            notificationManager.ShowPickUpNotification();
            toBePickedUp = other.gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            notificationManager.HidePickUpNotification();
            toBePickedUp = null;
        }
        if (other.CompareTag("Item"))
        {
            notificationManager.HidePickUpNotification();
            toBePickedUp = null;
        }
    }

    public void PickUpCurrentObject()
    {
        //把物品放在手里
        toBePickedUp.transform.parent = rifleAimHolder;
        toBePickedUp.transform.localPosition = Vector3.zero;
        toBePickedUp.transform.localRotation = Quaternion.Euler(Vector3.zero);
        GameObject weapon = toBePickedUp;
        pickUpItem = toBePickedUp;
        if (toBePickedUp.CompareTag("Weapon"))
        {
            Invoke("EquipWeapon", 1.5f);
        }
        else if (toBePickedUp.CompareTag("Item"))
        {
            print("this is Item");
            Invoke("GetItem", 1.5f);
        }
       
        //toBePickedUp = null;
    }

    void EquipWeapon()
    {
        print("EquipWeapon");
        //GameObject weapon = Instantiate<GameObject>(pickUpItem);
        pickUpItem.tag = "Rifle";
        weaponManager.AddNewWeapon(pickUpItem);
        weaponManager.UseLastWeapon();
        Destroy(pickUpItem.GetComponent<Collider>());
        notificationManager.HidePickUpNotification();
        toBePickedUp = null;
    }

    void GetItem()
    {
        if(pickUpItem.GetComponent<GrenadeScript>() != null)
        {
            Destroy(pickUpItem);
            weaponManager.AddGrenade();
            weaponManager.UseLastWeapon();
        }
        notificationManager.HidePickUpNotification();
        toBePickedUp = null;
    }
}
