using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public BoxCollider2D bc;
    public GameObject light;
    public int line;
    public int column;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInChildren<Piece>())
        {
            bc.enabled = false;
        }
        else
        {
            bc.enabled = true;
        }
    }

    public void ChangeEnableCollider()
    {
        bc.enabled = !bc.enabled;
    }
}
