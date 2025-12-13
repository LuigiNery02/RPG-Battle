using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public RectTransform tooltip; //objeto tooltip
    public TextMeshProUGUI texto; //texto do tooltip
    public Vector2 deslocamento; //deslocamento do tooltip

    private Canvas canvas; //canvas

    private void Awake()
    {
        Instance = this;
        canvas = GetComponentInParent<Canvas>();
        tooltip.gameObject.SetActive(false);
    }

    public void Mostrar(string descricao, Vector2 posicaoTela) //função que mostra o tooltip
    {
        if (!string.IsNullOrEmpty(descricao))
        {
            tooltip.gameObject.SetActive(true);
            texto.text = descricao;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, posicaoTela, canvas.worldCamera, out Vector2 posicaoLocal);

            Vector2 offset = deslocamento;
            tooltip.anchoredPosition = AjusteDeTamanho(posicaoLocal + offset);
        }
    }

    public void Esconder() //função que esconde o tooltip
    {
        tooltip.gameObject.SetActive(false);
    }

    private Vector2 AjusteDeTamanho(Vector2 posicao) //função que ajusta o tamanho do tooltip
    {
        RectTransform canvasRect = canvas.transform as RectTransform;
        Vector2 size = tooltip.sizeDelta;

        Vector2 minimo = canvasRect.rect.min + size * 0.5f;
        Vector2 maximo = canvasRect.rect.max - size * 0.5f;

        posicao.x = Mathf.Clamp(posicao.x, minimo.x, maximo.x);
        posicao.y = Mathf.Clamp(posicao.y, minimo.y, maximo.y);

        return posicao;
    }
}
