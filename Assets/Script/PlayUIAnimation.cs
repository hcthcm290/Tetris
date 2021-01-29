using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUIAnimation : MonoBehaviour
{
    public EasyTween ezTween;
    // Start is called before the first frame update
    void Start()
    {
        ezTween.OpenCloseObjectAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
