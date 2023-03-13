using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Cinemachine;

public class TPSShootController : MonoBehaviour
{
    // Start is called before the first frame update
    StarterAssetsInputs starterAssetsInputs;
    [SerializeField] CinemachineVirtualCamera aimCamera;
    [SerializeField] Transform rifleAimHolder;
    [SerializeField] Transform rifleCarryHolder;
    void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (starterAssetsInputs.aim) {
            aimCamera.gameObject.SetActive(true);
        }
        else
        {
            aimCamera.gameObject.SetActive(false);
        }
    }

    public void PullOutRifle()
    {
        //print("PullOutRifle");
        GameObject rifle = GameObject.FindGameObjectWithTag("Rifle");
        if (rifle != null)
        {
            rifle.transform.parent = rifleAimHolder;
            rifle.transform.localPosition = Vector3.zero;
            rifle.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void PutBackRifle()
    {
        //print("PutBackRifle");
        GameObject rifle = GameObject.FindGameObjectWithTag("Rifle");
        if (rifle != null)
        {
            rifle.transform.parent = rifleCarryHolder;
            rifle.transform.localPosition = Vector3.zero;
            rifle.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
