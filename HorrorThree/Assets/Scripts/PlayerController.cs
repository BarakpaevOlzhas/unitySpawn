using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController firstPersonController;

    public bool isAlive = true;

    public void KillPlayer()
    {
        firstPersonController.enabled = false;
        isAlive = false;
    }
}
