﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using as3mbus.Story;

public class PhaseController : MonoBehaviour
{
    public Transform kameraRoute, kamera, baloonPos, baloonsizer;
    public Text dName, dText;
    public GameObject dPanel;
    int currentLine = 0, currentChar = 0;
    public SpriteRenderer pageL, pageR;
    public float speed = 5f, routeRadius = 1f, typeDelay = 0.2f;
    public float duration;

    public float times;
    float timeCount;
    Phase activePhase;
    private StoryController ssControl;
    Vector3 originPosition;
    float originZoom;
    bool movingCamera;
    public bool pageLR = true;
    public float shake = 0.2f;

    //Start phase by setting active phase and load phase 
    public void startPhase(Phase fase)
    {
        this.activePhase = fase;
        // Debug.Log(fase.toJson());
        currentLine = 0;
        kameraRoute.position = activePhase.Lines[currentLine].Effects.CameraEffects.Position;
        kamera.GetComponent<Camera>().orthographicSize = activePhase.Lines[currentLine].Effects.CameraEffects.Size;
        readLine(currentLine);
    }
    // Update is called once per frame
    void Start()
    {
        //access story controller 
        ssControl = FindObjectOfType<StoryController>();
    }

    void Update()
    {
        if (currentLine >= activePhase.Lines.Count) return;

        //read fire 1 button pressed  
        if (Input.GetButtonDown("Fire1"))
        {
            //skip transition if there are any duration
            if (duration > 0)
            {
                //skip transition 
                times = duration;
                spriteFade(activePhase.Lines[currentLine].Effects.FadeMode);
                camRoute();
            }

            //if line finished reading 
            // read next line or hide phase (complete phase) 
            if (currentChar >= activePhase.Lines[currentLine].Message.Length)
            {
                //if there are more line after current line
                //read next line 
                if (currentLine < activePhase.Lines.Count - 1)
                {
                    currentLine++;
                    readLine(currentLine);
                }
                //hide / complete the phase 
                else
                {
                    hidePhase();
                }
            }

            // read complete line
            else
                showLine(activePhase.Lines[currentLine].Message);
        }

        //transitioning camera and fading effect if there are any duration
        if (times < duration)
        {
            //fade transition 
            spriteFade(activePhase.Lines[currentLine].Effects.FadeMode);
            //if not using color fade
            //slowly move camera position (pan and zoom) 
            if (activePhase.Lines[currentLine].Effects.FadeMode != fadeMode.color)
                camRoute();
        }
        //if there arent just set the camera to designated position and size
        else
            camPos();

        //text per sec 
        textPerSec(typeDelay);

        //show talking baloon
        // showBaloon();

        //shake camera
        shakeCamera(activePhase.Lines[currentLine].Effects.CameraEffects.Shake, shake);
    }
    //read complete line 
    public void showLine(string line)
    {
        dText.text = line;
        currentChar = line.Length;
    }
    //show talking baloon 
    public void showBaloon()
    {
        if (times >= duration && !baloonPos.gameObject.activeSelf)
        {
            baloonPos.gameObject.SetActive(true);
            baloonPos.GetComponent<Animation>().Play();
        }
    }
    //read every data about the line and use it to make player interface/ looks
    public void readLine(int line)
    {
        if (line >= activePhase.Lines.Count) return;
        //swapping page on fademode and loading it if there are any transition and duration
        if (activePhase.Lines[line].Effects.FadeMode != fadeMode.none && duration > 0)
        {
            pageLR = !pageLR;
            activePage().color = new Color(1, 1, 1, 0);
            activePage().sprite =
            activePhase.comic.pages[
                activePhase.Lines[
                    currentLine].Effects.Page];
        }
        //if there are no transition nor duration just change page without any page swap
        else
        {
            inactivePage().color = new Color(1, 1, 1, 0);
            activePage().color = Color.white;
            activePage().sprite = activePhase.comic.pages[activePhase.Lines[currentLine].Effects.Page];
        }
        
        // baloonsizer.transform.localScale = new Vector2(activePhase.baloonsize[line], activePhase.baloonsize[line]);
        originPosition = kameraRoute.localPosition;
        originZoom = kamera.GetComponent<Camera>().orthographicSize;
        times = 0;
        duration = activePhase.Lines[line].Effects.Duration;
        currentChar = 0;
        dPanel.SetActive(false);
        dName.text = activePhase.Lines[line].Character;
        dText.text = "";
        kamera.GetComponent<Camera>().backgroundColor = activePhase.Lines[line].Effects.CameraEffects.BackgroundColor;

        // if (
        //     Mathf.Abs(activePhase.baloonpos[line].x)
        //      + Mathf.Abs(activePhase.baloonpos[line].y)
        //      != 0)
        // {
        //     baloonPos.gameObject.SetActive(true);
        //     baloonPos.localPosition = activePhase.baloonpos[line];
        //     baloonPos.gameObject.SetActive(false);
        // }
        // else
        //     baloonPos.gameObject.SetActive(false);
    }

    //type a character after a delay 
    public void textPerSec(float delay)
    {
        if (times < duration)
            return;
        dPanel.SetActive(activePhase.Lines[currentLine].Message != "");
        if (currentChar >= activePhase.Lines[currentLine].Message.Length)
            return;

        timeCount += Time.deltaTime;
        if (timeCount > delay)
        {
            dText.text = dText.text + activePhase.Lines[currentLine].Message[currentChar];
            currentChar++;
            timeCount = 0;
        }
    }
    //fading effect
    void spriteFade(fadeMode fadeM)
    {
        if (times < duration)
            times += Time.deltaTime;
        else times = duration;
        if (fadeM == fadeMode.color)
            colorFade();
        else if (fadeM == fadeMode.transition)
            transitionFade();
    }
    //Transition Fade 
    void transitionFade()
    {
        inactivePage().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), times / duration);
        activePage().color = Color.Lerp(Color.white, new Color(1, 1, 1, 1), times / duration);
    }
    //color fade 
    void colorFade()
    {
        inactivePage().color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), (times * 2) / duration);
        activePage().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, (times * 2 - duration) / duration);
        if (times >= duration / 2)
            camPos();
    }
    // shake camera 
    void shakeCamera(float frequency, float magnitude)
    {
        Vector2 shakeVector;
        float seed = Time.time * frequency;
        // print(Time.time + " * " + frequency + " = " + seed);
        // print("Perlin = " + Mathf.PerlinNoise(seed, 0f));
        shakeVector.x = Mathf.PerlinNoise(seed, 0f) - 0.5f;
        shakeVector.y = Mathf.PerlinNoise(0f, seed) - 0.5f;
        shakeVector = shakeVector * magnitude;
        kamera.localPosition = shakeVector;

    }
    //accessing active page sprite 
    public SpriteRenderer activePage()
    {
        return pageLR ? pageL : pageR;
    }
    //accessing inactive page sprite 
    public SpriteRenderer inactivePage()
    {
        return !pageLR ? pageL : pageR;
    }

    //moving camera to designated point 
    public void camRoute()
    {
        kameraRoute.localPosition = Vector3.Lerp(originPosition, activePhase.Lines[currentLine].Effects.CameraEffects.Position, times / duration);
        kamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(originZoom, activePhase.Lines[currentLine].Effects.CameraEffects.Size, times / duration);
    }

    //set camera position
    public void camPos()
    {
        kameraRoute.localPosition = activePhase.Lines[currentLine].Effects.CameraEffects.Position;
        kamera.GetComponent<Camera>().orthographicSize = activePhase.Lines[currentLine].Effects.CameraEffects.Size;

    }
    //hide phase and call story controller next phase
    public void hidePhase()
    {
        pageLR = true;
        pageL.color = Color.white;
        pageR.color = Color.white;
        gameObject.SetActive(false);
        ssControl.nextPhase();
    }
}
