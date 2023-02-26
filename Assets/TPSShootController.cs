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
}
