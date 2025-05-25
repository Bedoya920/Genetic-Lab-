using UnityEngine;

public class PredatorBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // asumimos que los gatos tienen tag "Gato"
        if (other.CompareTag("Gato"))
        {
            var movimiento = other.GetComponent<Movement>();
            if (movimiento != null)
            {
                movimiento.alive = false;
                Destroy(other.gameObject);
            }
        }
    }
}
