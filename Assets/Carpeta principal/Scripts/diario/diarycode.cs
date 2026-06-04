using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class diarycode : MonoBehaviour
{
   
    public List<Transform>pages;
    int pageIndex = -1;
    float angle;
    [SerializeField] GameObject forwardButton;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject diary;
    public void Start()
    {
        backButton.SetActive(false);
    }
    
    public void rotatePageforward()
    {
            pageIndex++;
            ForwardButtonActions();
            angle = 180;
            pages[pageIndex].SetAsLastSibling();
            pages[pageIndex].transform.Rotate(0,angle,0);
            if(pages[pageIndex -1].gameObject.activeInHierarchy == true && pages[pageIndex] !=null )
            {
                pages[pageIndex - 1].gameObject.SetActive(false);  
            }  
    }
    public void rotatePageBackward()
    {
            BackwardButtonActions();
            angle = -180;
            pages[pageIndex].SetAsLastSibling();
            pages[pageIndex].transform.Rotate(0,angle,0);
            pageIndex--;
            if(pages[pageIndex].gameObject.activeInHierarchy == false && pages[pageIndex] != null)
            {
                pages[pageIndex].gameObject.SetActive(true);
            }
         
    }
    private void ForwardButtonActions()
    {
        if (backButton.activeInHierarchy == false && pageIndex != -1)
        {
            backButton.SetActive(true);
        }
        if (pageIndex + 1 >= pages.Count || pages[pageIndex + 1].gameObject.activeInHierarchy == false)
        {
            {
                forwardButton.SetActive(false);
            }
        }
    }
    private void BackwardButtonActions()
    {
        if (forwardButton.activeInHierarchy == false)
        {
            forwardButton.SetActive(true);
        }
        if (pageIndex -1 == -1)
        {
            backButton.SetActive(false);
        }
    }
}
