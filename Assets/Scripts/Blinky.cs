using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using UnityEngine.Scripting;
//https://zhuanlan.zhihu.com/p/54510444
public class Blinky : MonoBehaviour
{
    // Start is called before the first frame update

    public class Knode:Priority_Queue.FastPriorityQueueNode
    {
        public MeshDetector meshDetector;
        public Knode(MeshDetector meshDetector_)
        {
           meshDetector=meshDetector_;
        }

    }
    public float view_r;
    public Vector2 direction;
    
    public MeshdetectorManager manager;

    public Pacman pacman;
    // public Priority_Queue.FastPriorityQueue<Knode> priques;
    
    bool testlock=false;
    public List<MeshDetector> viewed=new List<MeshDetector>();
    public Stack<MeshDetector> walkpoints=new Stack<MeshDetector>();

   
    

    public float Accum=0;
    void Start()
    {

        manager=MeshdetectorManager.manager;
        // priques=new Priority_Queue.FastPriorityQueue<Knode>(manager.y_num*manager.x_num);

        Clear_Trace();

    }

    // Update is called once per frame
    

    void Update()
    {
        // if (!testlock)
        // {
        //         Findpath_(this.transform.position);
        //         testlock=true;
        // }
        if(Accum>=0.75)
        {
          StopAllCoroutines();
          Clear_Trace();
        //   this.Findpath_(this.transform.position);
          StartCoroutine(this.Findpath(this.transform.position));
          
        //    Findpath(this.transform.position);
           Accum=0;
        }
        else
        {
          Accum+=Time.deltaTime;
        }

          
    }
    

    void Clear_Trace()
    {
        foreach(var viewed_ in viewed)
        {
            viewed_.parent=null;
            viewed_.SetTrace(false,Color.black);
        }
        // manager.Clean();
        viewed.Clear();
        walkpoints.Clear();
    }
    // IEnumerator do_Pathfinding()
    // {
    
    // }
     


    private void FixedUpdate() {
        Automove();
        
    }

    void Automove()
    {
       if(walkpoints.Count>0)
       {
             if(Vector2.Distance(walkpoints.Peek().GetComponentsInChildren<SpriteRenderer>()[1].bounds.center,
             this.transform.position
             )<0.1)
             {
                 walkpoints.Peek().SetTrace(false,Color.black);
                walkpoints.Pop();
             }
             else
             {
                 Vector2 dir=walkpoints.Peek().GetComponentsInChildren<SpriteRenderer>()[1].bounds.center-
                 this.transform.position;
                 dir.Normalize();
              
                 this.transform.Translate(dir.x*Time.deltaTime,dir.y*Time.deltaTime,0);
             }
       }
    }

    
    
    IEnumerator Findpath(Vector2 begin)
    {
        
        // Vector2 e=manager.Maze.GetComponent<SpriteRenderer>().bounds.min;
        // Vector2 a= begin-new Vector2(e.x,e.y);
        // var block= manager.array[(int)(a.y/manager.y_maze)][(int)(a.x/manager.x_maze)];
        // block.GetComponents<SpriteRenderer>()[1].color=Color.cyan;

        Priority_Queue.FastPriorityQueue<Knode> priques=
        new Priority_Queue.FastPriorityQueue<Knode>(manager.y_num*manager.x_num*5);


        int count=0,interval=180;


        
        var pacpos=new Vector2(pacman.transform.position.x,pacman.transform.position.y);
        Vector2Int begin_index;
        Vector2Int end_index;
        var block_begin= manager.getPos(begin,out begin_index);
        var block_end=manager.getPos(pacpos,out end_index);
        
        // block_end.SetTrace(true,Color.black);
        // HashSet<MeshDetector> open_set=new HashSet<MeshDetector>();
        HashSet<MeshDetector> close_set=new HashSet<MeshDetector>();
        Dictionary<MeshDetector,Knode> open_set=new Dictionary<MeshDetector, Knode>();

        MeshDetector current=block_begin;
        priques.Enqueue(new Knode(current),0);
        
        
        float current_distance=0;

        
        

        // print(current);
        // print(current!=block_end);
        // print(priques.Count>0);
        float feet=(manager.x_dect+manager.y_dect)/2;//dijstra距离
        while(current!=block_end)
        {
            
            
           foreach(MeshDetector meshDetector_ in current.neibor)
           {
                   count++;

                   float eular_distance=
                   Vector2.Distance(
                         pacpos,
                       meshDetector_.GetComponent<SpriteRenderer>().bounds.center
                     );//欧式距离

                    // float eular_distance=0;//欧式距离


                   if(close_set.Contains(meshDetector_))
                   {

                       continue;
                   }
                   if(meshDetector_.isobstacle==true)
                   {
                       continue;
                   }
                   if(open_set.ContainsKey(meshDetector_))
                   {

                     feet=Vector2.Distance(
                         current.GetComponent<SpriteRenderer>().bounds.center,
                       meshDetector_.GetComponent<SpriteRenderer>().bounds.center
                     );
                       

                       float thisway=current_distance+feet+eular_distance;//启发式函数
                       var noderef=open_set[meshDetector_];
                       float oldway=noderef.Priority;

                       if(thisway<oldway)
                       {
                            noderef.meshDetector.parent=current;
                            priques.UpdatePriority(noderef,thisway);
                        
                       }
                       
                    //    else
                    //    {

                    //    }
                       

                   }
                   else
                   {
                        meshDetector_.parent=current;
                        viewed.Add(meshDetector_);
                        

                       feet=Vector2.Distance
                       (current.GetComponent<SpriteRenderer>().bounds.center,
                       meshDetector_.GetComponent<SpriteRenderer>().bounds.center);
                        var nodefef=new Knode(meshDetector_);
                        priques.Enqueue(nodefef,current_distance+feet+eular_distance);

                        // meshDetector_.SetTrace(true,Color.yellow);    
                        open_set.Add(meshDetector_,nodefef);
                   }
                   if(count%interval==0)
                   {
                       yield return null;
                   }

           }
           close_set.Add(current);
     
           if(priques.Count<=0)
           {
            //    print("no way");
               break;
           }
           var node=priques.Dequeue();
           current= node.meshDetector;
           current_distance=node.Priority;
           
        //    print(current);
           
        }

       
        if(current!=null&&current==block_end)
        {
            while(current!=block_begin)
            {
            //  current.GetComponentsInChildren<SpriteRenderer>()[1].color=Color.yellow;
             if(current==block_end)
             {
                 current.SetTrace(true,Color.grey);
             }
             else
             {
                 current.SetTrace(true,Color.blue);
                 walkpoints.Push(current);
             }
            //   var old=current;
              current=current.parent;
            //   old.parent=null;
            }
        }

        // foreach(var viewed_ in viewed)
        // {
        //     viewed_.parent=null;
        //     // viewed_.GetComponentsInChildren<SpriteRenderer>()[1].color=Color.green;
        //     // viewed_.SetTrace(false,Color.black);
        // }
        // viewed.Clear();
       
        
        
        // var block_end=



        // block.GetComponentsInChildren<SpriteRenderer>()[1].color=Color.blue;

    }
      void Findpath_(Vector2 begin)
    {
        
        // Vector2 e=manager.Maze.GetComponent<SpriteRenderer>().bounds.min;
        // Vector2 a= begin-new Vector2(e.x,e.y);
        // var block= manager.array[(int)(a.y/manager.y_maze)][(int)(a.x/manager.x_maze)];
        // block.GetComponents<SpriteRenderer>()[1].color=Color.cyan;

        Priority_Queue.FastPriorityQueue<Knode> priques=
        new Priority_Queue.FastPriorityQueue<Knode>(manager.y_num*manager.x_num*5);


        int count=0,interval=150;


        
        var pacpos=new Vector2(pacman.transform.position.x,pacman.transform.position.y);
        Vector2Int begin_index;
        Vector2Int end_index;
        var block_begin= manager.getPos(begin,out begin_index);
        var block_end=manager.getPos(pacpos,out end_index);
        
        // block_end.SetTrace(true,Color.black);
        // HashSet<MeshDetector> open_set=new HashSet<MeshDetector>();
        HashSet<MeshDetector> close_set=new HashSet<MeshDetector>();
        Dictionary<MeshDetector,Knode> open_set=new Dictionary<MeshDetector, Knode>();

        MeshDetector current=block_begin;
        priques.Enqueue(new Knode(current),0);
        
        
        float current_distance=0;

        
        

        // print(current);
        // print(current!=block_end);
        // print(priques.Count>0);
        float feet=(manager.x_dect+manager.y_dect)/2;//dijstra距离
        while(current!=block_end)
        {
            
            
           foreach(MeshDetector meshDetector_ in current.neibor)
           {
                   count++;

                   float eular_distance=
                   Vector2.Distance(
                         pacpos,
                       meshDetector_.GetComponent<SpriteRenderer>().bounds.center
                     );//欧式距离

                    // float eular_distance=0;//欧式距离


                   if(close_set.Contains(meshDetector_))
                   {

                       continue;
                   }
                   if(meshDetector_.isobstacle==true)
                   {
                       continue;
                   }
                   if(open_set.ContainsKey(meshDetector_))
                   {

                     feet=Vector2.Distance(
                         current.GetComponent<SpriteRenderer>().bounds.center,
                       meshDetector_.GetComponent<SpriteRenderer>().bounds.center
                     );
                       

                       float thisway=current_distance+feet+eular_distance;//启发式函数
                       var noderef=open_set[meshDetector_];
                       float oldway=noderef.Priority;

                       if(thisway<oldway)
                       {
                            noderef.meshDetector.parent=current;
                            priques.UpdatePriority(noderef,thisway);
                        
                       }
                       
                    //    else
                    //    {

                    //    }
                       

                   }
                   else
                   {
                        meshDetector_.parent=current;
                        viewed.Add(meshDetector_);
                        

                       feet=Vector2.Distance
                       (current.GetComponent<SpriteRenderer>().bounds.center,
                       meshDetector_.GetComponent<SpriteRenderer>().bounds.center);
                        var nodefef=new Knode(meshDetector_);
                        priques.Enqueue(nodefef,current_distance+feet+eular_distance);

                        // meshDetector_.SetTrace(true,Color.yellow);    
                        open_set.Add(meshDetector_,nodefef);
                   }
                //    if(count%interval==0)
                //    {
                //        yield return null;
                //    }

           }
           close_set.Add(current);
     
           if(priques.Count<=0)
           {
            //    print("no way");
               break;
           }
           var node=priques.Dequeue();
           current= node.meshDetector;
           current_distance=node.Priority;
           
        //    print(current);
           
        }

       
        if(current!=null&&current==block_end)
        {
            while(current!=block_begin)
            {
            //  current.GetComponentsInChildren<SpriteRenderer>()[1].color=Color.yellow;
             if(current==block_end)
             {
                 current.SetTrace(true,Color.grey);
             }
             else
             {
                 current.SetTrace(true,Color.blue);
                 walkpoints.Push(current);
             }
            //   var old=current;
              current=current.parent;
            //   old.parent=null;
            }
        }

        // foreach(var viewed_ in viewed)
        // {
        //     viewed_.parent=null;
        //     // viewed_.GetComponentsInChildren<SpriteRenderer>()[1].color=Color.green;
        //     // viewed_.SetTrace(false,Color.black);
        // }
        // viewed.Clear();
       
        
        
        // var block_end=



        // block.GetComponentsInChildren<SpriteRenderer>()[1].color=Color.blue;

    }
    void view_collision()
    {
        // var render=GetComponent<SpriteRenderer>().bounds;
        // int step=5;
        // for (int i = 0; i < step; i++)
        // {
        //     Physics2D.Raycast(render.center,direction,view_r);
        // }
    }
}
