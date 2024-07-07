using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator anim;
    [SerializeField] private int maxHeart = 3;
    [SerializeField] private GameObject game;
    [SerializeField] private SpriteRenderer spriteR;
    [SerializeField] private Sprite sprite;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (maxHeart < 1)
        {   
            anim.SetBool("dead", true);
            spriteR.sprite = sprite;
            StartCoroutine(Destroy(0.4f));
        }
    }
    IEnumerator Destroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(game);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Weapon"))
        {
            maxHeart -= 1;
            anim.SetBool("hit", true);
            StartCoroutine(StopHit(0.5f));
        }
    }

    IEnumerator StopHit(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("hit", false);
    }
}
