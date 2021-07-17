using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject doorToOpen;
    public Sprite unpushedPressurePlate;
    public Sprite pushedPressurePlate;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MetalBlock")
        {
            doorToOpen.SetActive(false);
            sprite.sprite = pushedPressurePlate;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MetalBlock")
        {
            doorToOpen.SetActive(true);
            sprite.sprite = unpushedPressurePlate;
        }
    }
}
