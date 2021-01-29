using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBlock : MonoBehaviour
{
    public bool canHold = true;
    GameObject storedBlocked;

    private void Start()
    {
        storedBlocked = null;
        canHold = true;
    }

    public bool Hold(GameObject obj)
    {
        if(!canHold)
        {
            return false;
        }

        if (storedBlocked == null)
        {
            FindObjectOfType<SpawnBlock>().NewBlock();
        }
        else
        {
            storedBlocked.transform.localScale = new Vector3(1f, 1f);
            FindObjectOfType<SpawnBlock>().NewBlock(storedBlocked).AddComponent<Block>();
            Destroy(storedBlocked);
        }
        storedBlocked = obj;

        Vector3 rotatePoint = obj.GetComponent<Block>().rotatePoint;
        Vector3 basePosition = transform.GetChild(0).transform.position;
        obj.transform.position = new Vector3(basePosition.x - rotatePoint.x * 0.7f, basePosition.y);
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = new Vector3(0.7f, 0.7f);
        Destroy(storedBlocked.transform.GetComponent<Block>());

        canHold = false;
        return true;
    }

}
