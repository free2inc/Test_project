using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleScript : MonoBehaviour
{
    public float speed;
    public GameObject linePrefab;

    public List<Vector3> pointsList = new List<Vector3>();
    public List<GameObject> drawLines = new List<GameObject>();


    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("Enter");
            foreach (var touch in Input.touches)
            {
                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                    return;
            }

                
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0);

            if (hit)
            {
                Debug.Log(hit.transform.name);
            }

            Vector2 touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointsList.Add(touchPoint);

            GameObject clone = Instantiate(linePrefab);
            drawLines.Add(clone);

            if (drawLines.Count == 1)
            {
                clone.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                clone.GetComponent<LineRenderer>().SetPosition(1, touchPoint);
            }

            else
            {
                clone.GetComponent<LineRenderer>().SetPosition(0, drawLines[drawLines.Count - 2].GetComponent<LineRenderer>().GetPosition(1));
                clone.GetComponent<LineRenderer>().SetPosition(1, touchPoint);
            }
        }



        if (pointsList.Count != 0)
        {
            drawLines[0].GetComponent<LineRenderer>().SetPosition(0, transform.position);


            if (transform.position != pointsList[0])
            {
                transform.position = Vector2.MoveTowards(transform.position, pointsList[0], speed * Time.deltaTime);
            }

            if (Vector2.Distance(transform.position, pointsList[0]) <= 0.01f)
            {
                pointsList.RemoveAt(0);
                Destroy(drawLines[0]);
                drawLines.RemoveAt(0);
            }
        }
    }

    public void ChangeSpeed(float _speed)
    {
        speed = _speed;
    }
}
