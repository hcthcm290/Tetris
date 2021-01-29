using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionBlock : MonoBehaviour
{
    public Transform sourceBlock { get; set; }
    void Update()
    {
        this.transform.position = sourceBlock.position;
        this.transform.rotation = sourceBlock.rotation;

        while (ValidMove())
        {
            transform.position += new Vector3(0, -1, 0);
        }
        transform.position -= new Vector3(0, -1, 0);
    }

    bool ValidMove()
    {
        foreach (Transform child in transform)
        {
            int xPos = Mathf.RoundToInt(child.transform.position.x);
            int yPos = Mathf.RoundToInt(child.transform.position.y);

            if (xPos < 0 || xPos >= Block.width || yPos < 0)
            {
                return false;
            }

            if (Block.grid[xPos, yPos] != null)
            {
                return false;
            }
        }

        return true;
    }
}
