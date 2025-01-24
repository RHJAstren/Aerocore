using UnityEngine;

public class Character : MonoBehaviour
{
    [Header ("Base Character Variables")]
    public float MovementSpeed = 5.0f;

    public virtual void Attack(){
        Debug.Log("Character is attacking.");
    }
}
