using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // <-- add this line
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [Header("Nav mesh settings")]
    [SerializeField]
    private Transform player;

    [SerializeField]
    private NavMeshAgent navMesh;

    [SerializeField]
    private Animator anim;

    // состояние врага
    private string state = "idle";
    private bool isAlive = true;

    [Header("Enemy settings")]
    [SerializeField]
    private float searchRadius;

    [SerializeField]
    private Transform deathCamera;

    [SerializeField]
    private Transform deathCameraPosition;

    [SerializeField]
    private float waitTime;
    private float wait;

    [SerializeField]
    private Transform triggerZone;

    [SerializeField]
    private AudioSource audioSource;

    private bool highAlert = false;
    private float alertLevel = 0;

    private void Start(){
        navMesh.speed = 1;
        anim.speed = 1;
    }

    private void Update(){
        if(isAlive == false)
            return;
        Debug.DrawLine(triggerZone.position, player.position);
        anim.SetFloat("speed", navMesh.velocity.magnitude);

        // если остановился
        if(state == "idle"){
            // иду к следующей точке
            GoToRandomPoint();
        }

        // пока иду
        if(state == "walk"){
            // проверяю дистанцию до цели
            CheckDistance();
        }

        // если осматриваюсь
        if(state == "search"){
            // осматриваюсь
            Search();
        }

        if (state == "chase")
        {
            ChaseForPlayer();
        }

        if (state == "hunt")
        {
            CheckDistance();
        }

        if (state == "Kill")
        {
            deathCamera.position = Vector3.Slerp(deathCamera.position, deathCameraPosition.position,10 * Time.deltaTime);
            deathCamera.rotation = Quaternion.Slerp(deathCamera.rotation, deathCameraPosition.rotation, 10 * Time.deltaTime);

            anim.speed = 0.4f;
        }

        if (state == "killed")
        {            
            var buf = GetComponent<Enemy>();
            Destroy(buf.gameObject);
        }
    }

    public void SetStatekilled()
    {        
        state = "killed";
    }


    private void ChaseForPlayer()
    {
        navMesh.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);

        var remainingDistance = navMesh.remainingDistance;
        var stoppingDistance = navMesh.stoppingDistance;

        if (distance > 10)
        {
            state = "hunt";
            highAlert = true;
            alertLevel = 20;
        }
        else if (remainingDistance <= stoppingDistance && navMesh.pathPending == false)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController.isAlive == true) {
                state = "kill";
                KillPlayer();
            }
        }
    }

    private void KillPlayer()
    {
        anim.SetTrigger("kill");
        var playerControler = player.GetComponent<PlayerController>();
        playerControler.KillPlayer();
        deathCamera.gameObject.SetActive(true);
        deathCamera.transform.position = Camera.main.transform.position;
        deathCamera.transform.rotation = Camera.main.transform.rotation;

        Camera.main.gameObject.SetActive(false);    

        audioSource.pitch = 0.8f;
        audioSource.Play();

        Invoke("RestartLevel",1.5f);
    }

    private void RestartLevel()
    {
        var currentScena = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScena);
    }

    public void CheckSight(){
        if(isAlive == false)
            return;
        
        RaycastHit hit;
        if(Physics.Linecast(triggerZone.position, player.position, out hit)){
            Debug.Log("Hit " + hit.collider.name);

            if (hit.collider.tag == "Player")
            {
                if (state != "kill")
                {
                    state = "chase";
                    navMesh.speed = 2;
                    anim.speed = 2;

                    audioSource.pitch = 1.2f;
                    audioSource.Play();
                }
            }
        }
    }

    private void GoToRandomPoint(){
        // генерируем случайную позицию внутри сферы
        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * searchRadius;
        NavMeshHit navHit;
        NavMesh.SamplePosition(
            transform.position + randomPos, 
            out navHit, 
            searchRadius, 
            NavMesh.AllAreas
        );

        if (highAlert)
        {
            NavMesh.SamplePosition(
           player.position,
           out navHit,
           searchRadius,
           NavMesh.AllAreas
       );
        }
        alertLevel -= 5;
        if (alertLevel <= 0)
        {
            highAlert = false;
            navMesh.speed = 1;
            anim.speed = 1;
        }
        navMesh.SetDestination(navHit.position);
        state = "walk";
    }

    private void CheckDistance(){
        var remainingDistance = navMesh.remainingDistance;
        var stoppingDistance = navMesh.stoppingDistance;
        // когда достигли цели
        if(remainingDistance <= stoppingDistance && navMesh.pathPending == false){
            state = "search";
            wait = waitTime;
        }
    }

    private void Search(){
        if(wait <= 0){
            state = "idle";
            return;
        }

        wait -= Time.deltaTime;
        transform.Rotate(0, 120f * Time.deltaTime ,0);
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
