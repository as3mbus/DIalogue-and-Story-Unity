using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicController : MonoBehaviour
{
    private bool pageLR;
    public GameObject[] Page;
    public int currentPage;
    public List<Sprite> Pages;
    private StorySceneController ssControl;
    // Use this for initialization
    
    public void startComic(Comic source)
    {
        Pages=source.pages;
        pageLR = true;
        currentPage = 0;
        Page[!pageLR ? 1 : 0].GetComponent<Image>().sprite = Pages[currentPage];
        Page[!pageLR ? 1 : 0].GetComponent<Animation>().Play("FadeIn");

    }
    void Start(){
        ssControl=FindObjectOfType<StorySceneController>();
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
        currentPage++;
        if (currentPage < Pages.Count)
        {
            pageLoad();
        }
        else{
            gameObject.SetActive(false);
            ssControl.nextPhase();
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
    void pageLoad()
    {
        Page[pageLR ? 1 : 0].GetComponent<Image>().sprite = Pages[currentPage];
        Page[!pageLR ? 1 : 0].GetComponent<Animation>().Play("FadeOut");
        Page[pageLR ? 1 : 0].GetComponent<Animation>().Play("FadeIn");
        pageLR = !pageLR;

    }
    void closeComic(){
        gameObject.SetActive(false);
        ssControl.nextPhase();
    }
    
}
