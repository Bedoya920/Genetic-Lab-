using UnityEngine;

public class PredatorBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gato"))
        {
            Movement mov = other.GetComponent<Movement>();
            if (mov != null)
            {
                mov.alive = false;
            }
        }
    }
}
