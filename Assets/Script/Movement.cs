using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Movement : MonoBehaviour
{
    private bool isMoving = false;
    private float movementTime = 0.5f; 
    private float timerMove;
    public int[] genoma;
    public int genSize = 100;
    public int genIndex;
    public bool alive = true; 


    void Start()
    {
        StartCoroutine(RandomMovement());
    }

    public void RandomGenoma()
    {
        genoma = new int[genSize];
        for (int i = 0; i < genSize; i++)
        {
            genoma[i] = Random.Range(0, 4);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            alive = false;
        }
    }

    IEnumerator RandomMovement()
    {
        while (alive)
        {
            if (!isMoving)
            {
                int i = genoma[genIndex%genoma.Length];
                genIndex++;
                Vector3 actualPos = transform.position;
                Vector3 nextMove = actualPos;

                switch (i)
                {
                    case 0:
                        nextMove += Vector3.forward;
                        break;
                    case 1:
                        nextMove += Vector3.back;
                        break;
                    case 2:
                        nextMove += Vector3.left;
                        break;
                    case 3:
                        nextMove += Vector3.right;
                        break;
                }

                isMoving = true;
                timerMove = 0f; 

                while (timerMove < movementTime)
                {
                    transform.position = Vector3.Lerp(actualPos, nextMove, timerMove / movementTime);
                    timerMove += Time.deltaTime;
                    yield return null; 
                }

                transform.position = nextMove; 
                isMoving = false;

                yield return new WaitForSeconds(0.5f); 
            }

            yield return null; 
        }
    }
}
