using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public int speed = 12;

    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;

    public int selectX = 0;
    public int selectY = 0;

    public int Vertical;
    public int Horizontal;


    public GameObject units_gui;
    public GridManager gm;
    public Sprite selected;
    GameObject g = new GameObject("cursor_select");


    public bool unitMode;
    public int selectMoveSpeed = 1;
    public int moveCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        unitMode = false;
        g = new GameObject("cursor_select");
        var s = g.AddComponent<SpriteRenderer>();
        g.transform.position = new Vector3(selectY - (Horizontal - 0.5f), selectX - (Vertical - 0.5f));

        units_gui.SetActive(false);
        Vertical = (int)Camera.main.orthographicSize;
        Horizontal = Vertical * (Screen.width / Screen.height);

       
        s.sortingOrder = 2;
        s.sprite = selected;



    }

    // Update is called once per frame
    void Update()

    {
        float fov = Camera.main.fieldOfView;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse is down");
            float screenRelativeX = Input.mousePosition.x / Screen.width;
            float screenRelativeY = Input.mousePosition.y / Screen.height;

            if (unitMode && screenRelativeX <= 0.27 && screenRelativeY <= 0.17)
            {
                return;
            }

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 100.0f);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                string name = hitInfo.transform.gameObject.name;

                if (name.StartsWith("X: "))
                {


                    string first = name.Replace("X: ", "");
                    Debug.Log(first);
                    string[] str = first.Split(new char[] { 'Y'});
                    str[1] = str[1].Replace(": ", "");
                    Debug.Log(str[1]);

                    Debug.Log(str[0]);

                    int oldSelectX = selectX;
                    int oldSelectY = selectY;
                    selectX = int.Parse(str[1]);
                    selectY = int.Parse(str[0]);

                    if(oldSelectX == selectX && oldSelectY == selectY)
                    {
                        return;
                    }

                    if (Input.GetMouseButtonDown(1) && unitMode)
                    {
                        if (!(gm.getArmy(oldSelectX, oldSelectY) is null))
                        {
                            Army a = (Army)gm.getArmy(oldSelectX, oldSelectY);
                            if (true) //CHECK FOR VALID MOVE
                            {
                                gm.setArmy(selectX, selectY, a);
                                gm.removeArmy(oldSelectX, oldSelectY, a);
                            }
                        }
                    }

                    g.transform.position = new Vector3(selectY - (Horizontal - 0.5f), selectX - (Vertical - 0.5f));



                    if (!(gm.getArmy(selectX, selectY) is null))
                    {
                        Army a = (Army)gm.getArmy(selectX, selectY);
                        units_gui.SetActive(true);
                        unitMode = true;
                    }
                    else
                    {
                        units_gui.SetActive(false);
                        unitMode = false;
                    }
                }



            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveCounter++;
            if (moveCounter * Time.fixedDeltaTime >= selectMoveSpeed)
            {
                moveCounter = 0;
                selectY++;
                g.transform.position = new Vector3(selectY - (Horizontal - 0.5f), selectX - (Vertical - 0.5f));
            }


        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveCounter++;
            if (moveCounter * Time.fixedDeltaTime >= selectMoveSpeed)
            {
                moveCounter = 0;
                selectY--;
                g.transform.position = new Vector3(selectY - (Horizontal - 0.5f), selectX - (Vertical - 0.5f));
            }


        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveCounter++;
            if (moveCounter * Time.fixedDeltaTime >= selectMoveSpeed)
            {
                moveCounter = 0;
                selectX--;
                g.transform.position = new Vector3(selectY - (Horizontal - 0.5f), selectX - (Vertical - 0.5f));
            }


        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveCounter++;
            if (moveCounter * Time.fixedDeltaTime >= selectMoveSpeed)
            {
                moveCounter = 0;
                selectX++;
                g.transform.position = new Vector3(selectY - (Horizontal - 0.5f), selectX - (Vertical - 0.5f));
            }

        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed * 2 * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed * 2 * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, -speed * 2 * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, speed * 2 * Time.deltaTime, 0));
        }





        if (Input.GetKey(KeyCode.Plus))
        {
            fov += 1;
        }

        if (Input.GetKey(KeyCode.Minus)) {
            fov -= 1;
        }
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }
}
