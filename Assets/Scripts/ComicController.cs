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
    // Use this for initialization
    void Start()
    {
        pageLR = true;
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
    void pageLoad()
    {
        Page[pageLR ? 1 : 0].GetComponent<Image>().sprite = Pages[currentPage];
        Page[!pageLR ? 1 : 0].GetComponent<Animation>().Play("FadeOut");
        Page[pageLR ? 1 : 0].GetComponent<Animation>().Play("FadeIn");
        pageLR = !pageLR;

    }
    public void loadComic(Comic source){
        Pages=source.pages;
    }
}
