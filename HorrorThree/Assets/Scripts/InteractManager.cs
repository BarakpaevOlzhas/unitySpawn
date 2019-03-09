using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float interactDistance;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private Transform cameraPosition;

    [SerializeField]
    private float respawnEneny;

    [SerializeField]
    private FlashLight flashLight;

    [SerializeField]
    private Image interactImage;

    [SerializeField]
    private Text textForCountRedSphere;

    [SerializeField]
    private int countRedSphereMax;

    [SerializeField]
    private Enemy enemy;

    private float startTimeDieEnemy;
    private Vector3 positionEnemyRespawn = new Vector3(298.2f, 1.173f, 37.38f);

    private void Start(){
        interactImage.gameObject.SetActive(false);
    }

    private void Update(){
        // origin - откуда исходит луч, direction - направление
        Ray ray = new Ray(cameraPosition.position, cameraPosition.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, interactDistance, layerMask)){
            interactImage.gameObject.SetActive(true);

            // если нажата клавиша E
            if(Input.GetKeyDown(KeyCode.E)){
                // если попали в объект с tag == Battery
                if(hit.collider.tag == "Battery"){
                    // добавить энергии
                    flashLight.AddEnergy(1.5f);
                    // уничтожаем объект
                    Destroy(hit.collider.gameObject);
                }else if(hit.collider.tag == "Candle"){
                    // получаем скрипт Candle у свечи
                    var candle  = hit.collider.GetComponent<Candle>();
                    candle.SetActive();
                }else if(hit.collider.tag == "Door"){
                    var door  = hit.collider.GetComponentInParent<Door>();
                    door.Open();
                }
                else if (hit.collider.tag == "RedSphere")
                {                   
                    int countRedSphere;

                    int.TryParse(textForCountRedSphere.text, out countRedSphere);

                    countRedSphere++;

                    textForCountRedSphere.text = countRedSphere.ToString();

                    if (countRedSphereMax == countRedSphere)
                    {
                        enemy.SetStatekilled();
                        gameManager.EnemyIsDied();
                    }

                    Destroy(hit.collider.gameObject);
                }
            }            

        }else{
            interactImage.gameObject.SetActive(false);
        }
    }    
}
