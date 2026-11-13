using UnityEngine;

// Scrolls textures for "stationary" world elements
public class ScrollWithWorld : MonoBehaviour
{
    [SerializeField] Material material;
    Renderer _renderer;

    private Vector2 scrollMult;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _renderer  = GetComponent<Renderer>();
        _renderer.material = Instantiate(material);
        scrollMult = Vector2.up / _renderer.material.mainTextureScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (material != null){
            _renderer.material.mainTextureOffset += scrollMult * (GameManager.currentSpeed * Time.deltaTime);
        }
    }
}
