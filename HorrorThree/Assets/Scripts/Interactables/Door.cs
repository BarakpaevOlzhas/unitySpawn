using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door settings")]
    [SerializeField]
    private float openAngle;

    [SerializeField]
    private float closedAngle;

    [SerializeField]
    private float smooth;

    private bool isLocked = true, isOpen = false;

    [Header("Audio settings")]
    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip doorClosed, doorOpen;

    public void Open(){
        if(isLocked == false){
            isOpen = true;
            // помещаем звук открытой двери
            audio.clip = doorOpen;
            // проигрываем звук
            audio.Play();
        }else{
            // помещаем звук закрытой двери
            audio.clip = doorClosed;
            // проигрываем звук
            audio.Play();
        }
    }

    private void Unlock(){
        isLocked = false;
    }

    private void Update (){
        if (isOpen){
            // Открыть дверь
            Quaternion targetRotationOpen = Quaternion.Euler(0, openAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotationOpen, smooth * Time.deltaTime);
        }else{
            Quaternion targetRotationClosed = Quaternion.Euler(0, closedAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotationClosed, smooth * Time.deltaTime);
        }
    }

}
