using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDissolve : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetBool("isDestroyed", true);
            Destroy(gameObject, 2.2f);
        }
    }
}
