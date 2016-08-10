using UnityEngine;
using System.Collections;

public class ToothpickJarFlair : MonoBehaviour
{
    public bool flairLoaded = false;

    public void OnStickyAttached(Transform stickyParent)
    {
        HatFlair flair = stickyParent.GetComponent<HatFlair>();
        if (!flair)
        {
            Debug.LogError("Sticky item was attached, but it's not a HatFlair instance", stickyParent);
            return;
        }
        Debug.Log("Attaching flair to jar", flair);
        SaveHatShelf.AddToothpickFlair(gameObject, flair);
    }

    public void OnStickyDetached(Transform stickyParent)
    {
        HatFlair flair = stickyParent.GetComponent<HatFlair>();
        if (!flair)
        {
            Debug.LogError("Sticky item was detached, but it's not a HatFlair instance", stickyParent);
            return;
        }
        SaveHatShelf.RemoveToothpickFlair(flair);
    }

}
