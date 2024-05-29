using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public Texture2D fogTexture; // Tekstura mg³y wojennej
    public Color fogColor = Color.black; // Kolor mg³y wojennej
    public float fogAlpha = 0.5f; // Przezroczystoœæ mg³y wojennej

    private Material fogMaterial;

    void Start()
    {
        // Utwórz nowy materia³ z tekstur¹ mg³y wojennej
        fogMaterial = new Material(Shader.Find("Unlit/Transparent"));
        fogMaterial.mainTexture = fogTexture;
        fogMaterial.color = fogColor;
        fogMaterial.SetFloat("_Alpha", fogAlpha);

        // Przypisz materia³ do p³aszczyzny lub innego obiektu reprezentuj¹cego obszar mapy
        GetComponent<Renderer>().material = fogMaterial;
    }

    // Metoda aktualizuj¹ca obszary mapy na podstawie danych o odkrytych obszarach
    public void UpdateFogOfWar(bool[,] exploredMap)
    {
        int width = fogTexture.width;
        int height = fogTexture.height;

        // Uaktualnij teksturê mg³y wojennej na podstawie danych o odkrytych obszarach
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // SprawdŸ, czy obszar zosta³ odkryty
                if (exploredMap[x, y])
                {
                    // Zaktualizuj piksel na teksturze mg³y wojennej na przezroczysty
                    fogTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
            }
        }

        // Zastosuj zmiany do tekstury mg³y wojennej
        fogTexture.Apply();
    }
}