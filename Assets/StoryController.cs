using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    private bool pageLR = true;
    public int currentPage;
    public List<Sprite> Pages;
    public GameObject PageL, PageR;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            nextPage();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            prevPage();
        }
    }
    void nextPage()
    {
        if (currentPage < Pages.Count - 1)
        {
            currentPage++;
            pageLoad();
        }
    }
    void prevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            pageLoad();
        }
    }
    void pageLoad(){
        if (pageLR)
            {
                PageR.GetComponent<Image>().sprite = Pages[currentPage];
                PageL.GetComponent<Animation>().Play("FadeOut");
                PageR.GetComponent<Animation>().Play("FadeIn");
                pageLR=false;
            }
            else{
                PageL.GetComponent<Image>().sprite = Pages[currentPage];
                PageR.GetComponent<Animation>().Play("FadeOut");
                PageL.GetComponent<Animation>().Play("FadeIn");
                pageLR=true;
            }
    }
}
