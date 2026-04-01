using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    public Transform[] backgrounds;
    [SerializeField] public int scrollSpeed;
    public float lowerBgLimit = 128f;
    void Start()
    {
        randomizeBGs(backgrounds);
    }

    
    void Update()
    {
        foreach (Transform bg in backgrounds)
        {
            bg.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
            if (bg.transform.position.y <= -lowerBgLimit)
            {
                float highestBGY = getHighestBG();
                bg.position = new Vector3(bg.position.x, highestBGY + lowerBgLimit, bg.position.z);
            }
        }
    }
    
    
    float getHighestBG()
    {
        float highest = backgrounds[0].position.y;

        foreach (Transform bg in backgrounds)
        {
            if (bg.position.y > highest)
                highest = bg.position.y;
        }

        return highest;
    }

    void randomizeBGs(Transform[] backgrounds)
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            Transform temp = backgrounds[i];
            int randomIndex = Random.Range(i, backgrounds.Length);
            backgrounds[i] = backgrounds[randomIndex];
            backgrounds[randomIndex] = temp;
        }
    }
}
