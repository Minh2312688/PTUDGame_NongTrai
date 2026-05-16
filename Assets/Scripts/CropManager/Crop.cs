using UnityEngine;

public class Crop : MonoBehaviour
{

    public Sprite[] growthStages;

    private SpriteRenderer sr;

    private int currentStage = 0;

    private float timer;
    public float growTime = 5f;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (growthStages.Length > 0)
        {
            sr.sprite = growthStages[0];
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= growTime)
        {
            timer = 0;
            Grow();
        }
    }

    void Grow()
    {
        if (currentStage < growthStages.Length - 1)
        {
            currentStage++;
            sr.sprite = growthStages[currentStage];
        }
    }

    public bool IsFullyGrown()
    {
        return currentStage >= growthStages.Length - 1;
    }
}
