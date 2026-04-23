using System;
using System.Collections.Generic;
using UnityEngine;

public class diarycode : MonoBehaviour
{
   
    [SerializeField] List<Transform>pages;
    int pageIndex = -1;
    float angle;
    [SerializeField] GameObject forwardButton;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject diary;

    public void Start()
    {
        backButton.SetActive(false);
    }

    public void Update()
    {
        if(diary.activeInHierarchy == true)
        {
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (forwardButton.activeInHierarchy == true)
            {
                rotatePageforward();
            }
            //ForwardButtonActions();
        }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if (backButton.activeInHierarchy == true)
                {
                    rotatePageBackward();
                }
            //BackwardButtonActions();
            }
        }
    }
    
    private void rotatePageforward()
    {
            pageIndex++;
            ForwardButtonActions();
            angle = 180;
            pages[pageIndex].SetAsLastSibling();
            pages[pageIndex].transform.Rotate(0,angle,0);
    }
    private void rotatePageBackward()
    {
            BackwardButtonActions();
            angle = -180;
            pages[pageIndex].SetAsLastSibling();
            pages[pageIndex].transform.Rotate(0,angle,0);
            pageIndex--;
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
