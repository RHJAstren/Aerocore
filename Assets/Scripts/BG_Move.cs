using System.Collections;
using UnityEngine;

public class BG_Move : MonoBehaviour
{
    void Start(){
        StartCoroutine(MoveBG());
    }

    IEnumerator MoveBG(){
        float startX = transform.position.x;
        float endX = startX + 5;
        float speed = 2.0f;
        bool movingForward = true;

        while (true){
            if (movingForward){
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.position.x >= endX){
                    movingForward = false;
                }
            } else {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (transform.position.x <= startX){
                    movingForward = true;
                }
            }
            yield return null;
        }
    }
}
