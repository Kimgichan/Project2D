using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagedEffect : MonoBehaviour
{
    public Color myColor = Color.red;
    private Color originalColor = Color.white;

    public float timeToChangeColor = 3.0f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("SwitchColor");
        }
    }

    IEnumerator SwitchColor()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = myColor;
            if (renderer.gameObject.name == "Shadow")
            {
                renderer.color = Color.black;
            }
        }
        yield return new WaitForSeconds(timeToChangeColor);
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = originalColor;

            if (renderer.gameObject.name == "Shadow")
            {
                renderer.color = Color.black;
            }
        }
    }
}
