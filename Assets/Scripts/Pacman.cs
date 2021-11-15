using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    public SpriteRenderer []spriteRenderer;
    
    public Rigidbody2D rigidbody2D;
    public float speed=1;
    void Start()
    {
        animator=GetComponentInChildren<Animator>();
        spriteRenderer=GetComponentsInChildren<SpriteRenderer>();
        rigidbody2D=GetComponent<Rigidbody2D>();
        animator.Play("Pacman_Y",0);

    }

    // Update is called once per frame
    void Update()
    {
        var delta=Time.deltaTime*speed;

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
        if(Input.GetKey(KeyCode.A))
        {
            // animator.Play("Pacman_Y",0);
            animator.Play("Pacman_X",0);
            spriteRenderer[1].flipX=true;
        }

        float x=Input.GetAxis("Horizontal");
        float y=Input.GetAxis("Vertical");
        this.transform.Translate(new Vector2(x,y)*delta);
        //  Debug.Log("update");
    }
    void OnCollisionEnter2D(Collision2D other) {
         print("endter");
    }
    private void FixedUpdate() {
        
    }
}
