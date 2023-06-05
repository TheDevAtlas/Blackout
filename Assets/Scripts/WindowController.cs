using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    public bool isBoarded;
    public GameObject window;

    private void Update()
    {
        if(isBoarded)
        {
            window.SetActive(true);
        }
        else
        {
            window.SetActive(false);
        }
    }
}
