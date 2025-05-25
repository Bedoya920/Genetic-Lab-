using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;
using System.Linq;  // Para usar LINQ en la caza por depredador

public class PopulationManager : MonoBehaviour
{
    [Header("Population Data")]
    public GameObject GatoPrefab;
    public Transform target;
    public int population;
    //public Movement movement;
    private int generation = 1;

    [SerializeField] private int evalTime = 20; 

    [SerializeField] private List<Movement> GatoPopulation = new List<Movement>();
    [SerializeField] private float timeScale = 1;

    [Header("Predator Settings")] 
    [SerializeField] private float huntStartDelay = 5f;   // espera inicial antes de que el depredador comience a cazar
    [SerializeField] private float huntInterval   = 1f;   // intervalo entre cada caza

    [Header("Predator Visual (opcional)")]
    public GameObject PredatorPrefab;    // prefab de la cápsula que representará al depredador
    [SerializeField] private float predatorSpeed = 5f; // velocidad de desplazamiento de la cápsula
    private GameObject predatorInstance; // instancia en escena

    private void Start()
    {
        FirstPeople();
        StartCoroutine(EvalTemp());
        Time.timeScale = timeScale;

        // Si asignaste un prefab, créalo en la escena como cápsula depredadora
        if (PredatorPrefab != null)
            predatorInstance = Instantiate(PredatorPrefab, Vector3.zero, Quaternion.identity);

        // Iniciar la rutina de caza del depredador
        StartCoroutine(PredatorHunt());
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
                    float distA = -a.transform.position.z + target.position.z;
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
                Destroy(gato.gameObject);

            GatoPopulation.Clear();

            // La nueva era: primer tercio copia sin mutación
            for (int i = 0; i < population / 3; i++)
            {
                Movement oldCat = oldCats[i % survivors];
                Movement child = Instantiate(GatoPrefab).GetComponent<Movement>();
                child.CopyGenoma(oldCat);
                //child.Mutate();
                GatoPopulation.Add(child);
            }

            // segundo tercio reproducción cruzada sin mutación
            for (int i = 0; i < population / 3; i++)
            {
                Movement oldCatM = oldCats[i % survivors];
                Movement oldCatP = oldCats[(i + 1) % survivors];
                Movement child = Instantiate(GatoPrefab).GetComponent<Movement>();
                child.GiveBirth(oldCatM, oldCatP);
                //child.Mutate();
                GatoPopulation.Add(child);
            }

            // último tercio copia con mutación
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

    // Coroutine sutil que implementa al depredador con snapshot de posición
    private IEnumerator PredatorHunt()
    {
        // 1) espera inicial antes de empezar a cazar
        yield return new WaitForSeconds(huntStartDelay);

        // 2) mientras haya gatos, continuará cazando
        while (GatoPopulation.Count > 0)
        {
            // Encuentra al gato más alejado del objetivo
            Movement worstCat = GatoPopulation
                .OrderByDescending(c => Vector3.Distance(c.transform.position, target.position))
                .First();

            // 2a) captura su posición antes de destruirlo
            Vector3 preyPosition = worstCat.transform.position;

            // 2b) destruye al gato
            worstCat.alive = false;
            Destroy(worstCat.gameObject);
            GatoPopulation.Remove(worstCat);

            // 3) mueve la cápsula depredadora hacia la posición “congelada”
            if (predatorInstance != null)
            {
                while (Vector3.Distance(predatorInstance.transform.position, preyPosition) > 0.1f)
                {
                    predatorInstance.transform.position = Vector3.MoveTowards(
                        predatorInstance.transform.position,
                        preyPosition,
                        predatorSpeed * Time.deltaTime
                    );
                    yield return null;
                }
            }

            // 4) espera antes de la siguiente caza
            yield return new WaitForSeconds(huntInterval);
        }
    }
}
