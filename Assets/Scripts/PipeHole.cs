using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipeHole : MonoBehaviour
{
    public delegate void HolePatchHandler(PipeHole hole);
    public event HolePatchHandler HolePatchEvent;

    public static Image patchBar = null;
    public float patchTime;

    SpriteRenderer sprite;
    bool patching;
    float patchTimer;

    // Start is called before the first frame update
    void Start()
    {
        if (patchBar == null)
        {
            patchBar = GameObject.Find("PatchBar").GetComponent<Image>();
            patchBar.enabled = false;
        }
        sprite = GetComponent<SpriteRenderer>();
        patching = false;
        patchTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (patching)
        {
            if (ActivityCheck.IsPressed(sprite.bounds, 0, Rotate.Side.RIGHT))
            {
                patchTimer += Time.deltaTime;
                if (patchTimer >= patchTime)
                {
                    // Hole patched
                    HolePatchEvent(this);
                    Destroy(gameObject);
                    patching = false;
                }
            } else
            {
                // Stop patching
                patching = false;
                patchTimer = 0;
            }

            // Draw patch bar
            patchBar.enabled = patching;
            patchBar.fillAmount = 2 * patchTimer;
            patchBar.rectTransform.position = Input.mousePosition;
        }
        else if (ActivityCheck.IsClicked(sprite.bounds, 0, Rotate.Side.RIGHT))
        {
            // Start patching
            AudioManager.Instance.Play("metal");
            patching = true;
            patchTimer = 0;
        }

        
    }
}
