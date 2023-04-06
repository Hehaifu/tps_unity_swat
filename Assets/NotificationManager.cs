using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] GameObject PickUpItem;
    // Start is called before the first frame update
    void Awake()
    {
        HidePickUpNotification();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPickUpNotification()
    {
        PickUpItem.SetActive(true);
    }
    public void HidePickUpNotification()
    {
        PickUpItem.SetActive(false);
    }
}
