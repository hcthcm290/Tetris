using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    public GameObject[] Block;
    List<int> queueNextBlock;
    [SerializeField] GameObject queueZone;

    // Start is called before the first frame update
    void Start()
    {
        queueNextBlock = new List<int>();
        while(queueNextBlock.Count < 5)
        {
            int rand = Random.Range(0, 7);
            while (queueNextBlock.Contains(rand))
            {
                rand = Random.Range(0, 7);
            }
            queueNextBlock.Add(rand);
            GameObject newQueueBlock = Instantiate(Block[rand], transform.position, Quaternion.identity);
            newQueueBlock.GetComponent<Block>().enabled = false;
            bool ret = queueZone.GetComponent<QueueBlock>().Enqueue(newQueueBlock);
        }
        NewBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject NewBlock()
    {
        GameObject ding = queueZone.GetComponent<QueueBlock>().Dequeue();
        Instantiate(ding, transform.position, Quaternion.identity).GetComponent<Block>().enabled = true;
        Destroy(ding);

        queueNextBlock.RemoveAt(0);
        int rand = Random.Range(0, 7);
        int sizeOfQ = queueNextBlock.Count;
        while (rand == queueNextBlock[sizeOfQ - 1] || rand == queueNextBlock[sizeOfQ - 2])
        {
            rand = Random.Range(0, 7);
        }
        queueNextBlock.Add(rand);

        GameObject newQueueBlock = Instantiate(Block[rand], transform.position, Quaternion.identity);
        newQueueBlock.GetComponent<Block>().enabled = false;
        queueZone.GetComponent<QueueBlock>().Enqueue(newQueueBlock);

        string list = "";
        foreach(var i in queueNextBlock)
        {
            list += i.ToString() + ",";
        }
        Debug.Log(list);

        return newQueueBlock;
    }

    public IEnumerator NewBlock(float t)
    {
        yield return new WaitForSecondsRealtime(t);
        NewBlock();
    }

    public GameObject NewBlock(GameObject obj)
    {
        return Instantiate(obj, transform.position, Quaternion.identity);
    }
}
