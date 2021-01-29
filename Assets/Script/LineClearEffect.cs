using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineClearEffect : MonoBehaviour
{
    [SerializeField] GameObject Effect;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearLines(List<int> lines)
    {
        foreach(var y in lines)
        {
            for (int x = 0; x < Block.width; x++)
            {
                Instantiate(Effect, new Vector3(x, y), Quaternion.identity);
            }
        }
    }
}
