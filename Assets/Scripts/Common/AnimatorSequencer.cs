using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSequencer : MonoBehaviour
{
    public Animator beforeAnimator;
    
    public Animator[] afterAnimators;
    public GameObject[] afterShowObjects;

    private void Start()
    {
        beforeAnimator.enabled = true;
        
        foreach (Animator anime in afterAnimators)
        {
            anime.enabled = false;
        }

        foreach (GameObject obj in afterShowObjects)
        {
            obj.SetActive(false);
        }
    }

    public void StartAfterAnimations()
    {
        beforeAnimator.enabled = false;

        foreach (Animator anime in afterAnimators)
        {
            anime.enabled = true;
        }

        foreach (GameObject obj in afterShowObjects)
        {
            obj.SetActive(true);
        }
    }
}
