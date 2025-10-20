using UnityEngine;

public class CleanableObject : MonoBehaviour
{

    [Header("Material Charco")]
    public Renderer charcoRenderer;
    //public float cleanSpeed = .34f;
    public bool destroyWhenClean = true;


    Material materialInstance;
    //Color baseColor;

    private void Start()
    {
        if(charcoRenderer == null)
        {
            charcoRenderer = GetComponent<Renderer>();
        }
        //Crear instancia en base al material original
        materialInstance = charcoRenderer.material;
        //baseColor = materialInstance.color;
    }


    public void Clean(float cantidad_)
    {
        Color color = materialInstance.color;
        color.a = Mathf.Clamp01(color.a - cantidad_);
        materialInstance.color = color;
        if(destroyWhenClean && color.a <= 0.05f)  Destroy(gameObject);  
    }

}
