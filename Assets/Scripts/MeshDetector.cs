using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public Color detectcolor=new Color(1,0,0,1);
    public Color defaultcolor=new Color(0,1,0,1);
    

    public List<MeshDetector> neibor=new List<MeshDetector>();
    // public List<int> neibor_=new List<int>();
    public MeshDetector parent=null;
    public bool isobstacle=false;
    public bool islive=false;

    public Vector3 old;
    void Start()
    {
        this.GetComponentsInChildren<SpriteRenderer>()[1].color=defaultcolor;
        // Physics2D.Raycast()
        checkRaycast();
        old=this.GetComponentsInChildren<SpriteRenderer>()[1].transform.localScale;
    }


    void checkRaycast()
    {
        var size_=this.GetComponent<SpriteRenderer>().bounds.size;
        var center_=this.GetComponent<SpriteRenderer>().bounds.center;
        Vector2 init=new Vector2((size_.x+size_.y)/4,0);
        float angle=0;
        for (int i = 0; i<8; i++)
        {
            angle=i*Mathf.PI/4;
            var direction=rotate(init,angle);
            var hit_= Physics2D.Raycast(center_,direction,(size_.x+size_.y)/4);
            if(hit_.collider!=null)
            {
               if(hit_.collider.tag=="Live")
               {
                 continue;
               }
               this.GetComponentsInChildren<SpriteRenderer>()[1].color=detectcolor;
            //    print("detect");
               isobstacle=true;
               return;
            }
        }
        // print("nondetect");
        this.GetComponentsInChildren<SpriteRenderer>()[1].color=defaultcolor;
        isobstacle=false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        
    }
    public void SetTrace(bool open,Color color)
    {
       if(open)
       {
        //    old=this.GetComponentsInChildren<SpriteRenderer>()[1].transform.localScale;
           this.GetComponentsInChildren<SpriteRenderer>()[1].transform.localScale=new Vector3(2,2,1);
           this.GetComponentsInChildren<SpriteRenderer>()[1].color=color;
       }
       else
       {
           
           this.GetComponentsInChildren<SpriteRenderer>()[1].transform.localScale=old;
           
            this.GetComponentsInChildren<SpriteRenderer>()[1].color=defaultcolor;
       }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        print("collider");
    }
    private void OnTriggerEnter2D(Collider2D other) {
        print("trigger");
    }
    public static Vector2 rotate(Vector2 v, float delta) {
    return new Vector2(
        v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
        v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
    );
}
}
