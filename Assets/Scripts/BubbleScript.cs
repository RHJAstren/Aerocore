using System.Collections;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    [Range(50f, 150f)]
    public float rotSpeed = 50f;
    public float moveSpeed = 1.0f;
    public float moveDistance = 0.5f;

    private Vector3 startPos;

    void Start(){
        startPos = transform.position;
        StartCoroutine(UpAndDown());
    }

    void Update(){
        transform.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            PlayerController.instance.AddBubble();
            FindAnyObjectByType<AudioManager>().Play("Bubble");
            gameObject.SetActive(false);
        }
    }

    IEnumerator UpAndDown(){
        while (true){
            float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
    }
}
