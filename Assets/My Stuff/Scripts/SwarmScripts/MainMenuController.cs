using System.Collections;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup[] backgrounds; //List of all backgrounds given

    public float fadeDuration = 2f; //time it takes to perform a fade transition
    public float imageTime = 15f; //how long the image stays on screen

    private int BGIndex = 0; //holds the index of our backgrounds array 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < backgrounds.Length; i++)
        {
            if(i == 0)
            {
                backgrounds[i].alpha = 1;
            }
            else
            {
                backgrounds[i].alpha = 0;
            }
        }

        StartCoroutine(CycleBG());
    }

    IEnumerator CycleBG()
    {
        //Need to give the first image time to sit before starting the loop
        yield return new WaitForSeconds(imageTime);
        //This is always running, therefore infinite while loop
        while(true)
        {
            //we find our next index by just adding one, 
            //however as this will run forever we need to pull it back down to 0 using the modulo
            int nextBGIndex = (BGIndex + 1) % backgrounds.Length;

            //start a coroutine that changes the background
            yield return StartCoroutine(FadeBackground(BGIndex, nextBGIndex));

            //increment index
            BGIndex = nextBGIndex;
            //wait the set amount of time before looping
            yield return new WaitForSeconds(imageTime);

        }
    }

    IEnumerator FadeBackground(int oldIndex, int newIndex)
    {
        //initialize our time holding variable for fade duration
        float t = 0f;

        //grab our two images
        CanvasGroup oldImage = backgrounds[oldIndex];
        CanvasGroup newImage = backgrounds[newIndex];

        //while our time fading is less than fade duration
        while(t<fadeDuration)
        {
            t += Time.deltaTime;

            //normalize our time variable so we can manipulate our alpha var with it
            float normailzedTime = t/fadeDuration;

            oldImage.alpha = 1-normailzedTime;
            newImage.alpha = normailzedTime;

            yield return null;
        }

        oldImage.alpha = 0f;
        newImage.alpha = 1f;
    }
}
