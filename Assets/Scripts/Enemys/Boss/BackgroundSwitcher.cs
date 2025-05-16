using System.Collections;
using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    [SerializeField] private Sprite[] backgrounds;
    [SerializeField] private float switchInterval = 1f;

    private SpriteRenderer spriteRenderer;
    private Coroutine switchingCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartSwitching()
    {
        if (switchingCoroutine == null)
        {
            switchingCoroutine = StartCoroutine(SwitchBackgrounds());
        }
    }

    public void StopSwitching()
    {
        if (switchingCoroutine != null)
        {
            StopCoroutine(switchingCoroutine);
            switchingCoroutine = null;
        }
    }

    private IEnumerator SwitchBackgrounds()
    {
        int index = 0;
        while (true)
        {
            spriteRenderer.sprite = backgrounds[index];
            index = (index + 1) % backgrounds.Length;
            yield return new WaitForSeconds(switchInterval);
        }
    }
}
