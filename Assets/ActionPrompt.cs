using UnityEngine;

public class ActionPrompt : MonoBehaviour
{
    [SerializeField] protected bool enabledAtStart = false;
   
    public virtual void Display()
    {
        this.gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
