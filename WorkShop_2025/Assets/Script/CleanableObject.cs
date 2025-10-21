using UnityEngine;

public class CleanableObject : MonoBehaviour
{

    [Header("Material Charco")]
    public Renderer charcoRenderer;
    //public float cleanSpeed = .34f;
    public bool destroyWhenClean = true;


    Material materialInstance;
    //Color baseColor;

    //colision que se encarga de tomar el renderer del liquido cuando choca
    //dando la sensecion que se mancho con ese color
    private void OnCollisionEnter(Collision collision)
    {
        charcoRenderer = collision.gameObject.GetComponent<Renderer>();
        if (collision.collider.gameObject.CompareTag("Liquido"))
        {
            if (charcoRenderer != null)
            {
                Renderer pelota = GetComponent<Renderer>();
                pelota.material = charcoRenderer.material;
            }
        }
    }
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
