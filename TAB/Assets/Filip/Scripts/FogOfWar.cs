using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public Texture2D fogTexture; // Tekstura mg�y wojennej
    public Color fogColor = Color.black; // Kolor mg�y wojennej
    public float fogAlpha = 0.5f; // Przezroczysto�� mg�y wojennej

    private Material fogMaterial;

    void Start()
    {
        // Utw�rz nowy materia� z tekstur� mg�y wojennej
        fogMaterial = new Material(Shader.Find("Unlit/Transparent"));
        fogMaterial.mainTexture = fogTexture;
        fogMaterial.color = fogColor;
        fogMaterial.SetFloat("_Alpha", fogAlpha);

        // Przypisz materia� do p�aszczyzny lub innego obiektu reprezentuj�cego obszar mapy
        GetComponent<Renderer>().material = fogMaterial;
    }

    // Metoda aktualizuj�ca obszary mapy na podstawie danych o odkrytych obszarach
    public void UpdateFogOfWar(bool[,] exploredMap)
    {
        int width = fogTexture.width;
        int height = fogTexture.height;

        // Uaktualnij tekstur� mg�y wojennej na podstawie danych o odkrytych obszarach
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Sprawd�, czy obszar zosta� odkryty
                if (exploredMap[x, y])
                {
                    // Zaktualizuj piksel na teksturze mg�y wojennej na przezroczysty
                    fogTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
        }

        // Zastosuj zmiany do tekstury mg�y wojennej
        fogTexture.Apply();
    }
}