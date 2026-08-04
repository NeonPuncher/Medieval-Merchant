using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Shop_StationInteract : MonoBehaviour
{
    public UnityEvent interactEvent;
    public UnityEvent hoverEnterEvent;
    public UnityEvent hoverExitEvent;
    public Animator animator;
    public UnityEvent openUI;

    public List<GameObject> activeGameObjects;
    public List<GameObject> inactiveGameObjects;

    [Header("has animation")]
    public string animatorName;
    public bool valueAnim;
    public string triggerAnim;
    private bool isActive;


    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            interactEvent.Invoke();
        }

    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            hoverEnterEvent.Invoke();
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            hoverExitEvent.Invoke();
        }
    }

    public void HasInteraction(string tagName)
    {
        if (this.tag == tagName)
        {
            openUI.Invoke();
        }
    }

    public void HasOutline(string tagName)
    {
        if (this.tag == tagName)
        {
            this.gameObject.GetComponent<Renderer>().materials[1].SetFloat("_Outline_thickness", 0.013f);
        }
    }

    public void HasNoOutline(string tagName)
    {
        if (this.tag == tagName)
        {
            this.gameObject.GetComponent<Renderer>().materials[1].SetFloat("_Outline_thickness", 0f);
        }
    }

    public void HasAnimationBool()
    {
        animator.SetBool(animatorName, valueAnim);
        valueAnim = valueAnim ? false : true;
    }

    public void HasObjectActive()
    {
        isActive = isActive ? false : true;
        foreach (GameObject obj in activeGameObjects)
        {
            obj.SetActive(!isActive);
        }

        foreach (GameObject obj in inactiveGameObjects)
        {
            obj.SetActive(isActive);
        }
        
    }

    public void HasAnimationTrigger()
    {
        animator.SetTrigger(triggerAnim);
    }
}
