using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshdetectorManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MeshdetectorManager manager;

    public MeshDetector getPos(Vector2 pos,out Vector2Int index)
    {
        Vector2 e=Maze.GetComponent<SpriteRenderer>().bounds.min;
        Vector2 a= pos-new Vector2(e.x,e.y);
        // print(a);
        // print((int)(a.y/manager.y_maze));
        // print((int)(a.x/manager.x_maze));
        var block= array[(int)(a.y/manager.y_dect)][(int)(a.x/manager.x_dect)];


        index=new Vector2Int((int)(a.x/manager.x_dect),(int)(a.y/manager.y_dect));
        print("isnull");
        print(block);
        return block;
    } 
    public MeshDetector getValidpos(Vector2 pos,out Vector2Int index)
    {
        Vector2 e=Maze.GetComponent<SpriteRenderer>().bounds.min;
        Vector2 a= pos-new Vector2(e.x,e.y);
        // print(a);
        // print((int)(a.y/manager.y_maze));
        // print((int)(a.x/manager.x_maze));
        var block= array[(int)(a.y/manager.y_dect)][(int)(a.x/manager.x_dect)];
        index=new Vector2Int((int)(a.x/manager.x_dect),(int)(a.y/manager.y_dect));
        if(block.isobstacle)
        {
            foreach (var nei in block.neibor)
            {
                if(!nei.isobstacle)
                return nei;
            }
        }

        
        // print("isnull");
        print(block);
        return block;
    } 
    public GameObject Mesh_detector;
    public GameObject Maze;

    public GameObject Detector_prefab;

    public int x_num;
    public int y_num;
    public float x_maze;
    public float y_maze;

    public float x_dect;

    public float y_dect;

    

   public MeshDetector [][] array ;

   private void Awake() {
       manager=this;
       x_maze=Maze.GetComponent<SpriteRenderer>().bounds.size.x;
        y_maze=Maze.GetComponent<SpriteRenderer>().bounds.size.y;
        var dect=Mesh_detector.GetComponent<SpriteRenderer>().bounds.size;

        x_dect=dect.x; 
        y_dect=dect.y;
        x_num=(int)(x_maze/x_dect);
        y_num=(int)(y_maze/y_dect);
       float a=x_maze/x_dect;
    //    print(Maze.GetComponent<SpriteRenderer>().bounds.size);
       print(string.Format("arraysize:{0},{1}",x_num,y_num));
    //    print(a);
    //    Debug.Log(Mesh_detector.GetComponent<SpriteRenderer>().bounds.size.x);
       
         
         array=new MeshDetector[y_num][];
         for (int  i = 0;  i < y_num;  i++)
         {
             array[i]=new MeshDetector[x_num];
         }
    }
    void Start()
    {
       plot_detectors();

    }

    // Update is called once per frame
    void plot_detectors()
    {
        var mins=Maze.GetComponent<SpriteRenderer>().bounds.min;
        var begin=new Vector2(mins.x,mins.y);
         for (int i = 0; i < y_num; i++)
         {
             for (int j = 0; j < x_num; j++)
             {
                //  array[i][j]
                var pos=new Vector2(begin.x+j*x_dect+x_dect/2,begin.y+i*y_dect+y_dect);
                
                array[i][j]=
                Instantiate(Detector_prefab,pos,Quaternion.identity).GetComponent<MeshDetector>();

                // print("isnull");
                // print(array[i][j]);
             }
         }
         


        
         for (int i = 0; i < y_num; i++)
         {
             for (int j = 0; j < x_num; j++)
             {
                //  array[i][j]
                
                for (int ii = -1; ii <= 1; ii++)
                {
                    for (int jj = -1; jj <=1 ; jj++)
                    {
                        if(i+ii>=0 && i+ii<y_num && j+jj>=0 && j+jj<x_num )
                        {
                            if(!(ii==0&&jj==0))
                            array[i][j].neibor.Add(array[i+ii][j+jj]);
                            // array[i][j].neibor_.Add(ii*jj);
                        }

                    }
                }
                // array[i][j];

             }
         }
    }
    void Update()
    {
        
    }
  public void Clean() {
        for (int i = 0; i < y_num; i++)
         {
             for (int j = 0; j < x_num; j++)
             {
                 array[i][j].parent=null;   
             }

         }
    }
}
