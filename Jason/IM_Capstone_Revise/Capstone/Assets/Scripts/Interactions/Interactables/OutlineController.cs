using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [Header("边框设置")]
    public Color outlineColor = Color.red;
    [Range(0.0f, 0.1f)]
    public float outlineWidth = 0.01f;

    private Material outlineMaterial;
    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;

            // 创建边框材质
            outlineMaterial = new Material(Shader.Find("Custom/SpriteOutline"));
            outlineMaterial.SetColor("_OutlineColor", outlineColor);
            outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
        }
    }

    // 启用边框效果
    public void EnableOutline()
    {
        if (spriteRenderer != null && outlineMaterial != null)
        {
            spriteRenderer.material = outlineMaterial;
        }
    }

    // 禁用边框效果
    public void DisableOutline()
    {
        if (spriteRenderer != null && originalMaterial != null)
        {
            spriteRenderer.material = originalMaterial;
        }
    }

    // 动态更新边框颜色
    public void SetOutlineColor(Color color)
    {
        outlineColor = color;
        if (outlineMaterial != null)
        {
            outlineMaterial.SetColor("_OutlineColor", color);
        }
    }

    // 动态更新边框宽度
    public void SetOutlineWidth(float width)
    {
        outlineWidth = Mathf.Clamp(width, 0.0f, 0.1f);
        if (outlineMaterial != null)
        {
            outlineMaterial.SetFloat("_OutlineWidth", width);
        }
    }

    void OnDestroy()
    {
        // 清理材质资源
        if (outlineMaterial != null)
        {
            Destroy(outlineMaterial);
        }
    }
}
