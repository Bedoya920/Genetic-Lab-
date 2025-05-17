using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PopulationManager : MonoBehaviour
{
    [Header("Population Data")]
    public GameObject GatoPrefab;
    public Transform target;
    public int population;
    //public Movement movement;
    private int generation = 1;

    [SerializeField]private int evalTime = 20; 

    [SerializeField] private List<Movement> GatoPopulation = new List<Movement>();

    private void Start()
    {
        FirstPeople();
        StartCoroutine(EvalTemp());
    }

    public void FirstPeople()
    {
        for (int i = 0; i < population; i++)
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
            yield return new WaitForSeconds(evalTime);

            GatoPopulation.Sort((a, b) =>
            {
                float distA = Vector3.Distance(a.transform.position, target.position);
                float distB = Vector3.Distance(b.transform.position, target.position);
                return distA.CompareTo(distB);
            });

            int survivors = population / 2;
            List<Movement> oldCats = new List<Movement>();

            // Guarda los survivors antes de destruirlos a todos
            for (int i = 0; i < survivors; i++)
            {
                oldCats.Add(GatoPopulation[i]);
            }
            //Preguntar por que con GameObject no deja utilizar foreach
            foreach (Movement gato in GatoPopulation)
            {
                Destroy(gato.gameObject);
            }

            GatoPopulation.Clear();

            //La nueva era
            for (int i = 0; i < population; i++)
            {
                Movement oldCat = oldCats[i % survivors];
                Movement child = Instantiate(GatoPrefab).GetComponent<Movement>();
                child.CopyGenoma(oldCat);
                child.Mutate();
                GatoPopulation.Add(child);
            }

            print($"Generación {generation} completada.");
            generation++;
        }
    }

}


