using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueBlock : MonoBehaviour
{
    Queue<GameObject> queue;
    [SerializeField] float miniScaleFactor = 0.5f;
    [SerializeField] float smallScaleFactor = 0.7f;
    
    void Start()
    {
        queue = new Queue<GameObject>();
    }

    public bool Enqueue(GameObject obj)
    {
        if(queue.Count == transform.childCount)
        {
            Destroy(obj);
            return false;
        }

        Vector3 rotatePoint = obj.GetComponent<Block>().rotatePoint;
        Vector3 basePosition = transform.GetChild(queue.Count).transform.position;
        obj.transform.position = new Vector3(basePosition.x - rotatePoint.x * miniScaleFactor, basePosition.y);
        obj.transform.localScale = new Vector3(miniScaleFactor, miniScaleFactor);

        queue.Enqueue(obj);

        rotatePoint = queue.Peek().GetComponent<Block>().rotatePoint;
        basePosition = transform.GetChild(0).transform.position;
        queue.Peek().transform.position = new Vector3(basePosition.x - rotatePoint.x * smallScaleFactor, basePosition.y);
        queue.Peek().transform.localScale = new Vector3(smallScaleFactor, smallScaleFactor);

        return true;
    }

    public GameObject Dequeue()
    {
        GameObject returnObj = queue.Dequeue();
        Vector3 rotatePoint;
        Vector3 basePosition;

        for (int i = 0; i < queue.Count; i++)
        {
            GameObject obj = queue.Dequeue();
            rotatePoint = obj.GetComponent<Block>().rotatePoint;
            basePosition = transform.GetChild(i).transform.position;
            obj.transform.position = new Vector3(basePosition.x - rotatePoint.x * miniScaleFactor, basePosition.y);
            queue.Enqueue(obj);
        }

        returnObj.transform.localScale = new Vector3(1f, 1f);

        return returnObj;
    }

}
