using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject currentGameObject;
    [SerializeField] GameObject hightLight;
    // private GameObject oldGameObject;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject==currentGameObject)
        {
            Debug.Log("Still in same gameobject");
            return;
        }
        Debug.Log("Go to new game object");
        currentGameObject = other.gameObject;
        hightLight.transform.position = currentGameObject.transform.position;
        
    }
}
