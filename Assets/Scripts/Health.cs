using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {

    public int Lives;

    private void Update()
    {
        if(Lives <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("demo");
        }
    }
}
