using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshdetectorManager : MonoBehaviour
{
    // Start is called before the first frame update
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
    void Start()
    {
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
                // array[i][j]=
                Instantiate(Detector_prefab,pos,Quaternion.identity).GetComponent<MeshDetector>();
             }
         }
    }
    void Update()
    {
        
    }
}
