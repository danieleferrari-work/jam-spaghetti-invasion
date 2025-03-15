using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnvironmentFadeOut : MonoBehaviour
{
    [Header("Fade Settings")]
    [Tooltip("Delay prima che inizi il fade-out (in secondi)")]
    [SerializeField] private float delayBeforeFade = 2f;
    [Tooltip("Durata del fade-out (in secondi)")]
    [SerializeField] private float fadeDuration = 5f;

    [Header("Light Settings")]
    [Tooltip("Intensità finale della luce")]
    [SerializeField] private float targetLightIntensity = 5f;
    [Tooltip("Riferimento al componente Light da intensificare")]
    [SerializeField] private Light sceneLight;

    [Header("Material Settings")]
    [Tooltip("Materiale HDRP Lit da applicare a tutti i figli")]
    [SerializeField] private Material hdrpLitMaterial;
    [Tooltip("Nome della proprietà colore del materiale (solitamente \"_BaseColor\")")]
    [SerializeField] private string colorPropertyName = "_BaseColor";

    // Variabili per gestire il fade della luce
    private float originalLightIntensity;

    // Collezioni per gestire i materiali e i GameObject dei figli
    private List<Material> allMaterials = new List<Material>();
    private Dictionary<Material, float> initialAlphas = new Dictionary<Material, float>();
    private List<GameObject> affectedObjects = new List<GameObject>();

    void Start()
    {
        // Se la luce è assegnata, salviamo la sua intensità iniziale
        if (sceneLight != null)
        {
            originalLightIntensity = sceneLight.intensity;
        }

        // Verifica che sia assegnato il materiale HDRP Lit
        if (hdrpLitMaterial == null)
        {
            Debug.LogError("HDRP Lit Material non assegnato!");
        }
        else
        {
            // Ottieni tutti i Renderer dei figli (anche inattivi)
            Renderer[] childRenderers = GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in childRenderers)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = hdrpLitMaterial;
                    if (hdrpLitMaterial.HasProperty(colorPropertyName))
                    {
                        allMaterials.Add(hdrpLitMaterial);
                        initialAlphas[hdrpLitMaterial] = hdrpLitMaterial.GetColor(colorPropertyName).a;
                        if (!affectedObjects.Contains(renderer.gameObject))
                        {
                            affectedObjects.Add(renderer.gameObject);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Il materiale su {renderer.gameObject.name} non ha la proprietà {colorPropertyName}.");
                    }
                }
                renderer.materials = materials;
            }
        }

        // Avvia la coroutine per il fade-out sia della luce che dei materiali
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        // Attende il delay prima di iniziare il fade-out
        yield return new WaitForSeconds(delayBeforeFade);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;  // t varia da 0 a 1

            // Aggiorna l'intensità della luce
            if (sceneLight != null)
            {
                sceneLight.intensity = Mathf.Lerp(originalLightIntensity, targetLightIntensity, t);
            }

            // Aggiorna gradualmente l'alpha di ogni materiale
            foreach (Material mat in allMaterials)
            {
                if (mat.HasProperty(colorPropertyName))
                {
                    Color col = mat.GetColor(colorPropertyName);
                    float startAlpha = initialAlphas[mat];
                    col.a = Mathf.Lerp(startAlpha, 0f, t);
                    mat.SetColor(colorPropertyName, col);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Al termine, forza il completamento del fade-out e disattiva gli oggetti
        if (sceneLight != null)
        {
            sceneLight.intensity = targetLightIntensity;
        }
        foreach (Material mat in allMaterials)
        {
            if (mat.HasProperty(colorPropertyName))
            {
                Color col = mat.GetColor(colorPropertyName);
                col.a = 0f;
                mat.SetColor(colorPropertyName, col);
            }
        }

        // Disattiva tutti gli oggetti su cui sono stati applicati i materiali
        foreach (GameObject obj in affectedObjects)
        {
            obj.SetActive(false);
        }

        // Disattiva il GameObject padre
        gameObject.SetActive(false);
    }
}
