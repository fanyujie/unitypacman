using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    public SpriteRenderer []spriteRenderer;

    void Start()
    {
        animator=GetComponentInChildren<Animator>();
        spriteRenderer=GetComponentsInChildren<SpriteRenderer>();
        animator.Play("Pacman_Y",0);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            animator.Play("Pacman_Y",0);
            spriteRenderer[1].flipY=false;

           
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            // animator.Play("Pacman_Y",0);
            animator.Play("Pacman_Y",0);
            spriteRenderer[1].flipY=true;

        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            animator.Play("Pacman_X",0);
            spriteRenderer[1].flipX=false;

           
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            // animator.Play("Pacman_Y",0);
            animator.Play("Pacman_X",0);
            spriteRenderer[1].flipX=true;

        }
         Debug.Log("update");
    }
}
