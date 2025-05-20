using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;

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
    [SerializeField] private float timeScale = 1;
    private void Start()
    {
        FirstPeople();
        StartCoroutine(EvalTemp());
        Time.timeScale = timeScale;
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
            for (int i = 0; i < population; i++)
            {
                GatoPopulation.Sort((a, b) =>
                {
                    float distA = -a.transform.position.z + target.position.z; // Vector3.Distance(a.transform.position, target.position);
                    float distB = -b.transform.position.z + target.position.z;
                    return distA.CompareTo(distB);
                });
            }
            

            int survivors = population / 2;
            List<Movement> oldCats = new List<Movement>();

            // Guarda los survivors antes de destruirlos a todos
            for (int i = 0; i < survivors; i++)
            {
                oldCats.Add(GatoPopulation[i]);
            }
            
            foreach (Movement gato in GatoPopulation)
            {
                Destroy(gato.gameObject);
            }

            GatoPopulation.Clear();

            //La nueva era
            for (int i = 0; i < population / 3; i++)
            {
                Movement oldCat = oldCats[i % survivors];
                Movement child = Instantiate(GatoPrefab).GetComponent<Movement>();
                child.CopyGenoma(oldCat);
                //child.Mutate();
                GatoPopulation.Add(child);
            }

            for (int i = 0; i < population/3; i++)
            {
                Movement oldCatM = oldCats[i % survivors];
                Movement oldCatP = oldCats[(i +1) % survivors];
                Movement child = Instantiate(GatoPrefab).GetComponent<Movement>();
                child.GiveBirth(oldCatM, oldCatP);
                //child.Mutate();
                GatoPopulation.Add(child);
            }

            for (int i = 0; i < population / 3; i++)
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


