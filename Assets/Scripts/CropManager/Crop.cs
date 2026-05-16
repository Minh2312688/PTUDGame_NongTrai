using UnityEngine;

public class Crop : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Vector3Int tilePosition;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Sprite[] growthStages;

    public float growthTime = 20f;

    [Header("Harvest")]
    public Item harvestItem;

    private int currentStage = 0;

    private float timer;

    private bool fullyGrown = false;

    private void Start()
    {
        spriteRenderer.sprite = growthStages[0];
    }

    private void Update()
    {
        if (fullyGrown) return;

        timer += Time.deltaTime;

        float timePerStage =
            growthTime / (growthStages.Length - 1);

        if (timer >= timePerStage)
        {
            timer = 0;

            currentStage++;

            if (currentStage >= growthStages.Length - 1)
            {
                currentStage =
                    growthStages.Length - 1;

                fullyGrown = true;
            }

            spriteRenderer.sprite =
                growthStages[currentStage];
        }
    }

    public bool CanHarvest()
    {
        return fullyGrown;
    }

    public void Harvest()
    {
        if (!fullyGrown) return;

        Vector2 offset =
    Random.insideUnitCircle * 0.8f;

        Item itemObj = Instantiate(
            harvestItem,
            (Vector2)transform.position + offset,
            Quaternion.identity);
        itemObj.rb2d.AddForce(offset * 2f, ForceMode2D.Impulse);
        Destroy(gameObject);
    }
}