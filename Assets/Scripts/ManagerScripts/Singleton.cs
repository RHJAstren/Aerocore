using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    public static T instance { get; private set; }

    protected virtual void Awake(){
        if (instance == null){
            instance = this as T;
        } else {
            Destroy(gameObject);
        }
    }
}
