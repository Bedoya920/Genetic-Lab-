using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PopulationManager : MonoBehaviour
{
    public GameObject GatoPrefab;
    public int x;
    public Movement movement;
    public int evalTime; 

    public List<Movement> GatoPopulation = new List<Movement>();

    private void Start()
    {
        FirstPeople();
        StartCoroutine(EvalTemp());
    }
    public void FirstPeople()
    {
        for (int i = 0; i < x; i++)
        {
            Movement gato = Instantiate(GatoPrefab).GetComponent<Movement>();
            gato.RandomGenoma();
            GatoPopulation.Add(gato);
        }
    }

    IEnumerator EvalTemp()
    {
        while (true)
        {
            Debug.Log("Timer ejecutado: " + Time.time);
            yield return new WaitForSeconds(evalTime);
        }

    }
}


