using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    private bool transition = false;
    public int currentPage;
    public List<Sprite> Pages;
    public GameObject activePage, nextPage, prevPage;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") )
        {
            if (currentPage < Pages.Count -1)
            {
                currentPage++;
                loadPage(currentPage);
                activePage.GetComponent<Animation>().Play("FadeOut");
                nextPage.GetComponent<Animation>().Play("FadeIn");
            }
            else
            {
                
            }

        }
    }
    void loadPage(int pageNum){
        if (pageNum>0){
            prevPage.GetComponent<Image>().sprite = Pages[pageNum-1];
        }
        activePage.GetComponent<Image>().sprite = Pages[pageNum];
        if (pageNum<Pages.Count-1){
            nextPage.GetComponent<Image>().sprite = Pages[pageNum+1];
        }
    }
    void opacityTransition(){
    }
}
