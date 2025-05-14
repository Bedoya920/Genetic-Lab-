using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    private bool isMoving = false;
    private float movementTime = 0.5f; 
    private float timerMove;

    void Start()
    {
        StartCoroutine(RandomMovement());
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            if (!isMoving)
            {
                int i = Random.Range(0, 4);
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
