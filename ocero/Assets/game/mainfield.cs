using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainfield : MonoBehaviour {

    Vector3[,] position = new Vector3[8, 8];
    GameObject[,] plates = new GameObject[8, 8];
    bool[,] boh = new bool[8, 8];


    GameObject sign;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                position[i, j] = new Vector3((float)0.5 + i, 0.917f, (float)0.5 + j);

                plates[i, j] = Instantiate(Resources.Load("objects/plate")) as GameObject;
                plates[i, j].transform.position = position[i, j];
                plate_Admin_buf(i,j,false);
                boh[i, j] = false;
                plates[i, j].transform.FindChild("Cube").gameObject.name = i.ToString() + j.ToString();
            }
        }

        deforltSet();

        sign = transform.FindChild("sign").gameObject;
        sign.transform.Rotate(180, 0, 0);


        //put(!white_order, 4, 2);
        //plates[2, 3].SetActive(true);


        //put(false, 3, 2);
        
       
    }

    public bool plate_buf(int i,int j)
    {
        GameObject bu = plates[i, j].transform
           .FindChild("buffer").gameObject;

        return bu.activeSelf;
    }

    public void plate_Admin_buf(int i,int j,bool active)
    {
        GameObject bu = plates[i, j].transform
            .FindChild("buffer").gameObject;
        bu.SetActive(active);

    }

    bool white_order = true;

	
	// Update is called once per frame
	void Update () {
        int x, y;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if(Physics.Raycast(ray, out hit, 100f))
            {
                string objName = hit.collider.gameObject.name;
                //Debug.Log(objName);
                x=int.Parse(objName.Substring(0, 1));
                y=int.Parse(objName.Substring(1, 1));
                
                put(white_order, x, y);
            }
        }
	}

    public bool judge(bool turn,int i,int j)
    {
        bool ret=false;
        if (judge_once(turn, i, j, 0, 1))
        {
            Debug.Log("j01");
            ret = true;
        }
        if (judge_once(turn, i, j, 0, -1))
        {
            Debug.Log("j0,1");
            ret = true;
        }
        if (judge_once(turn, i, j, 1, 1))
        {
            Debug.Log("j11");
            ret = true;
        }
        if (judge_once(turn, i, j, 1,0))
        {
            Debug.Log("j10");
            ret = true;
        }
        if (judge_once(turn, i, j, 1,-1))
        {
            Debug.Log("j1-1");
            ret = true;
        }
        if (judge_once(turn, i, j, -1,1))
        {
            Debug.Log("j-11");
            ret = true;
        }
        if (judge_once(turn, i, j, -1,0))
        {
            Debug.Log("j-10");
            ret = true;
        }
        if (judge_once(turn, i, j, -1,-1))
        {
            Debug.Log("j-1-1");
            ret = true;
        }
        return ret;

    }

    public bool judge_once(bool me,int i,int j,int x,int y)
    {
        int counter=1;
        int px = i + x ;
        int py = j + y ;

        bool reverse = false;
       
        bool loop = true;

        while(px<8&&px>=0&&py<8&&py>=0&&loop&&plate_buf(px,py))
        {
            Debug.Log(boh[px,py]+":"+plates[px,py].activeSelf);
            if(boh[px,py]==me)
            {
                if (counter > 1)
                {
                    counter--;
                    reverse = true;
                }
                loop = false;
            }
           
            counter++;
            px += x;
            py += y;
        }
        //Debug.Log(reverse + "reverse");
        if (reverse)
        {
            px = i + x;
            py = j + y;
            counter--;
            //Debug.Log(counter + ":counter:"+px+","+py);

            while (counter > 0) {
                //Debug.Log("rotate:" + px + "," + py);

                rotate(px, py);
                counter--;
                px += x;
                py += y;
            }
        }
        return reverse;
    }
    int[,] range = new int[,]
    {
        {-1,-1 },
        {-1,0 },
        {-1,1 },
        {0,1 },
        {0,-1 },
        {1,1 },
        {1,0 },
        {1,-1 },
    };
    public void put(bool me,int i,int j)
    {
        bool ok = false;

       
        for (int a = 0; a < 8&&!ok; a++)
        {
            int bi = i + range[a, 0];
            int bj = j + range[a, 1];
            if (bi >= 0 && bj >= 0&&bi<8&&bj<8)
            {
               ok = plate_buf(bi,bj);
            }
        }
        if (plate_buf(i,j))
        {
            ok = false;
        }
        if (judge(me, i, j)&&ok)
        {
        plate_Admin_buf(i,j,true);
            if (me)
            {
                rotate(i, j);
            }
            white_order = !white_order;
            sign.transform.Rotate(180, 0, 0);
        }

    }

    public void deforltSet()
    {
        int h = 3;
        int v = 3;
        plate_Admin_buf(h,v,true);
        plate_Admin_buf(h,v+1,true);
        //boh[v, h+1] = true;
       plate_Admin_buf(v+1,h,true);
        //boh[v+1, h] = true;
        plate_Admin_buf(v+1,h+1,true);

        rotate(h, v + 1);
        rotate(h + 1, v);
    }

    public void rotate(int i,int j)
    {
        plates[i, j].transform.Rotate(new Vector3(180, 0, 0));
        boh[i, j] = !boh[i, j];
    }
}
