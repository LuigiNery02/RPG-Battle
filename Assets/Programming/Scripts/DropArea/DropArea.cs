using UnityEngine;
using UnityEngine.UI;

public enum Area { Inventario, Equipamento, Venda}
public class DropArea : MonoBehaviour
{
    public Area tipoDeArea; //tipo de área 

    private Inventory inventario; //inventário
    private ScrollRect scrollRect; //componente scroll rect dentro de um filho

    private void Start()
    {
        inventario = FindFirstObjectByType<Inventory>();

        if(tipoDeArea == Area.Inventario)
        {
            scrollRect = GetComponentInChildren<ScrollRect>();
        }
    }
    public void AoSoltarItem(DragItem item) //função que verifica em qual área o item está sendo solto
    {
        if(tipoDeArea == Area.Inventario)
        {
            Item itemOriginal = item.GetComponent<Item>();
            if (!inventario.itens.Contains(itemOriginal))
            {
                inventario.AdicionarItem(itemOriginal);
                itemOriginal.scrollRect = scrollRect;
            }
            else
            {
                itemOriginal.VoltarPosicaoOriginal();
            }
        }
        else if(tipoDeArea == Area.Equipamento)
        {
            Item itemOriginal = item.GetComponent<Item>();
            if (inventario.itens.Contains(itemOriginal))
            {
                inventario.RemoverItem(itemOriginal);
                itemOriginal.scrollRect = null;
            }

            itemOriginal.transform.position = transform.position;
                
        }
        else if(tipoDeArea == Area.Venda)
        {
            Item itemOriginal = item.GetComponent<Item>();
            if (inventario.itens.Contains(itemOriginal))
            {
                inventario.RemoverItem(itemOriginal);
                itemOriginal.scrollRect = null;
            }

            item.gameObject.SetActive(false);
        }
    }
}
