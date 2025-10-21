using UnityEngine;

public class LiquidoPelota : MonoBehaviour
{
    public Color colorActual = Color.blue;
    public Renderer rend;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = colorActual;
        }
    }
    public void SetColor(Color nuevoColor)
    {
        colorActual = nuevoColor;
        if (rend != null)
        {
            rend.material.color = nuevoColor;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //si choca con otra pelota(liquido)
        if (collision.gameObject.CompareTag("Liquido"))
        {
            LiquidoPelota otro = collision.gameObject.GetComponent<LiquidoPelota>();
            if(otro != null)
            {
                //si los colores son distintos, se mezclan
                if(colorActual != otro.colorActual)
                {
                    Color mezcla = (colorActual + otro.colorActual) / 2f;
                    //Color mezclaVerde = Color.green;
                    SetColor(mezcla);
                    otro.SetColor(mezcla);
                }
                
            }
        }
    }

}
