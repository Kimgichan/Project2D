using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedEffect : MonoBehaviour
{
    public float timeToChangeColor = 3f;
    public int flickeringCount = 3;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine("SwitchAlpha");
        }
    }

    IEnumerator SwitchAlpha()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        Color newColor;
        //foreach (SpriteRenderer renderer in renderers)
        //{
        //    if (renderer.gameObject.name != "Shadow")
        //    {
        //        newColor = renderer.color;
        //        newColor.a = 0;
        //        renderer.color = newColor;
        //    }
        //}
        //yield return new WaitForSeconds(timeToChangeColor);
        ////return to original
        //foreach (SpriteRenderer renderer in renderers)
        //{
        //    newColor = renderer.color;
        //    newColor.a = 1;
        //    renderer.color = newColor;
        //}


        //change alpha
        int count = 0;
        while (count < flickeringCount)
        {
            foreach (SpriteRenderer renderer in renderers)
            {
                if (renderer.gameObject.name != "Shadow")
                {
                    newColor = renderer.color;
                    newColor.a = 0f;
                    renderer.color = newColor;
                }
            }
            yield return new WaitForSeconds(timeToChangeColor);
            //return to original
            foreach (SpriteRenderer renderer in renderers)
            {
                newColor = renderer.color;
                newColor.a = 1;
                renderer.color = newColor;
            }
            yield return new WaitForSeconds(timeToChangeColor);

            count++;
        }

    }
}
