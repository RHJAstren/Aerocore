using UnityEngine;

public class Character : MonoBehaviour
{
    [Header ("Base Character Variables")]
    public float MovementSpeed;
    public float RunSpeed;

    public virtual void Attack(){
        Debug.Log("Character is attacking.");
    }
}
