using UnityEngine;

public class LiquidoPelota : MonoBehaviour
{
    public Color colorActual = Color.blue;
    Color mezclaFinal = Color.green;
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
                if (colorActual == mezclaFinal && otro.colorActual == mezclaFinal) return;
                //si los colores son distintos, se mezclan
                if(colorActual != otro.colorActual)
                {
                    //Color mezcla = (colorActual + otro.colorActual) / 2f;
                    //Color mezclaVerde = Color.green;
                    SetColor(mezclaFinal);
                    otro.SetColor(mezclaFinal);
                }
                
            }
        }
        else if (collision.gameObject.CompareTag("Matraz"))
        {
            if(colorActual == mezclaFinal)
            {
                Mezclas matraz = collision.gameObject.GetComponent<Mezclas>();
                if(matraz != null)
                {
                    matraz.CambioColorInterno(mezclaFinal);
                }
            }
        }
        else if (collision.gameObject.name == "Liquido")
        {
            if(colorActual == mezclaFinal)
            {
                Renderer liquidoRendere = collision.gameObject.GetComponent<Renderer>();
                if(liquidoRendere != null)
                {
                    liquidoRendere.material.color = mezclaFinal;
                }
            }
        }
        else if (collision.gameObject.CompareTag("Matraz"))
        {
            if(collision.gameObject.CompareTag("Liquido"))
            {
                Destroy(gameObject);
            }
        }
    }

}
