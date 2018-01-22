using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/**
 * Script to show the ammount of resources collected in the spaceship
 * 
 * @author "Sebastián José Díaz Rodríguez", "Ernesto Echeverría González", "Antonio Jesús López Garnier", "Germán Andrés Pescador Barreto"
 * @since October 2017
 * 
 * @version 1.0.0
 */
public class RecolectorMovement : MonoBehaviour
{
    public float tiempoCaminaRandom = 0f;
    public float tiempoReinicio = 5f;
    public float tiempoSueltaMiga = 1f;
    public float tiempoRecogiendoRecurso = 3f;
    public float speedFree = 15.0f;
    public float speedCrumb = 6.0f;
    public float cantidadRecurso = 0f;

    private int distanciaEntreMigas = 10;
   
    private Vector3 nuevaPosicion;
    private Vector3 antiguaPosicion;

    private bool recursoVisitado = false;
    public bool buscandoRecurso = true;
    public bool encuentraRecurso = false;
    public bool encuentraMigas = false;
    public bool haciaLaNave = false;
    public bool temporizador = false;

    NavMeshAgent robot;

	public GameObject miga;
    public Transform target;

    // Use this for initialization
    private void Awake()
    {
        robot = GetComponent<NavMeshAgent>();
        antiguaPosicion = robot.transform.position;
    }


    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        if (temporizador && haciaLaNave)
        {
            tiempoRecogiendoRecurso -= Time.deltaTime;
            if (tiempoRecogiendoRecurso <= 0)
            {
                tiempoRecogiendoRecurso = 3f;
                encuentraRecurso = false;
                buscandoRecurso = true;
                temporizador = false;
                resumeRobot();
                haciaLaNave = false;
            }
        }
        if (buscandoRecurso)
           randomMovement();
        if (encuentraRecurso)
        	recogeRecurso();
        if (encuentraMigas)
        {
           tiempoReinicio -= Time.deltaTime;
           if (tiempoReinicio <= 0)
           {
               encuentraMigas = false;
               buscandoRecurso = true;
               tiempoReinicio = 5f;
           }
        }
        if (haciaLaNave && !temporizador && !recursoVisitado)
            soltandoMigas();
    }

    void recogeRecurso()
    {
        tiempoRecogiendoRecurso -= Time.deltaTime;
        if (tiempoRecogiendoRecurso <= 0)
        {
            tiempoRecogiendoRecurso = 3f;
            encuentraRecurso = false;
            toTheShip();
        }
    }

    void stopRobot ()
    {
         gameObject.GetComponent<NavMeshAgent>().enabled = false;
    }

    void resumeRobot()
    {
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

    void soltandoMigas ()
    {
        tiempoSueltaMiga -= Time.deltaTime;
        if (tiempoSueltaMiga <= 0)
        {
            if (!recursoVisitado)
                tiraMiga();
            tiempoSueltaMiga = 0.5f;
        }
    }

    void tiraMiga ()
    {
        GameObject auxiliar = new GameObject ();
        auxiliar.transform.position = new Vector3(robot.transform.position.x, robot.transform.position.y + 0.5f, robot.transform.position.z);
        auxiliar.transform.rotation = robot.transform.rotation;
        auxiliar.transform.LookAt(antiguaPosicion);
        auxiliar.transform.rotation.Set(auxiliar.transform.rotation.x, auxiliar.transform.rotation.y + 180, auxiliar.transform.rotation.z, auxiliar.transform.rotation.w);
        Instantiate(miga, new Vector3(robot.transform.position.x, robot.transform.position.y + 0.5f, robot.transform.position.z), auxiliar.transform.rotation);
        Destroy(auxiliar);
        antiguaPosicion = robot.transform.position;
    }

    void randomMovement()
    {
        tiempoCaminaRandom -= Time.deltaTime;   
        if (tiempoCaminaRandom <= 0)
        {
            randomDirection();
            tiempoCaminaRandom = Random.Range(1f, 3f);
        }
    }

    void randomDirection()
    {
        nuevaPosicion = transform.position + new Vector3(Random.onUnitSphere.x * 100, 0f, Random.onUnitSphere.z * 100);
        resumeRobot();
        robot.SetDestination(nuevaPosicion);
        robot.speed = speedFree;
    }
    void toTheShip()
    {
        haciaLaNave = true;
        resumeRobot();
        robot.SetDestination(target.position);
        robot.speed = speedCrumb;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Resource" && !haciaLaNave)
        {
            stopRobot();
            antiguaPosicion = col.gameObject.transform.position;
            cantidadRecurso = 10f;
            
            recursoVisitado = col.gameObject.GetComponent<Resource>().isVisitado;
            if (!col.gameObject.GetComponent<Resource>().isVisitado)
                col.gameObject.GetComponent<Resource>().isVisitado = true;
            buscandoRecurso = false;
            encuentraRecurso = true;
            encuentraMigas = false;
            col.gameObject.GetComponent<Resource>().decrementa();
            // stopRobot();
        }
        if (col.gameObject.tag == "SupplyBoat" && haciaLaNave) {
            stopRobot();
            temporizador = true;
            col.gameObject.GetComponent<Cantidad>().incrementa(cantidadRecurso);
            col.gameObject.GetComponent<Cantidad>().play();
            cantidadRecurso = 0;
        }
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.tag == "Crumb" && !haciaLaNave)
        {
            if (!encuentraMigas)
            {
                stopRobot();
                resumeRobot();
                robot.speed = speedCrumb;
            }
            tiempoReinicio = 5f;
            buscandoRecurso = false;
            encuentraMigas = true;
            nuevaPosicion = col.gameObject.transform.position + col.gameObject.transform.forward * distanciaEntreMigas;
            robot.SetDestination(nuevaPosicion);
            col.gameObject.GetComponent<Miga>().decrementa();
            if (col.gameObject.GetComponent<Miga>().coste <= 0)
                Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "Crumb" && haciaLaNave)
        {
            col.gameObject.GetComponent<Miga>().incrementa();
        }
    }

}
