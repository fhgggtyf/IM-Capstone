using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    [Header("Image Settings")]
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite image1;
    [SerializeField] private Sprite image2;

    [Header("Current State")]
    [SerializeField] private bool isImage1Active = true;

    private void Start()
    {
        // Initialize with correct image
        if (targetImage != null)
        {
            targetImage.sprite = isImage1Active ? image1 : image2;
        }
    }

    /// <summary>
    /// Switch between the two images
    /// </summary>
    public void SwitchImage()
    {
        if (targetImage == null || image1 == null || image2 == null)
        {
            Debug.LogWarning("ImageSwitcher missing references!", this);
            return;
        }

        isImage1Active = !isImage1Active;
        targetImage.sprite = isImage1Active ? image1 : image2;
    }

    /// <summary>
    /// Switch to a specific image
    /// </summary>
    public void SwitchToImage1()
    {
        if (targetImage != null && image1 != null)
        {
            isImage1Active = true;
            targetImage.sprite = image1;
        }
    }

    public void SwitchToImage2()
    {
        if (targetImage != null && image2 != null)
        {
            isImage1Active = false;
            targetImage.sprite = image2;
        }
    }

    /// <summary>
    /// Set images dynamically
    /// </summary>
    public void SetImages(Sprite newImage1, Sprite newImage2)
    {
        image1 = newImage1;
        image2 = newImage2;

        if (targetImage != null)
        {
            targetImage.sprite = isImage1Active ? image1 : image2;
        }
    }
}