using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform slotsConteudo; //objeto que possui todos os slots como filho
    [HideInInspector]
    public List<Item> itens = new List<Item>(); //lista de itens do inventário
    [HideInInspector]
    public List<Transform> slots = new List<Transform>(); //lista de slots do inventário

    private void Start()
    {
        slots.Clear();

        for (int i = 0; i < slotsConteudo.childCount; i++)
        {
            slots.Add(slotsConteudo.GetChild(i));
        }
    }

    public void AdicionarItem(Item item) //função que adiciona itens ao inventário
    {
        if (!itens.Contains(item))
        {
            itens.Add(item);
            PosicionarItem(itens.Count - 1);
        }
    }

    public void RemoverItem(Item item) //função que remove itens ao inventário
    {
        if (itens.Contains(item))
        {
            itens.Remove(item);
            PosicionarTodos();
        }
    }

    private void PosicionarItem(int index) //função que posiciona o item adicionado
    {
        if (index >= slots.Count)
        {
            Debug.LogWarning("Inventário cheio.");
            return;
        }

        Transform slot = slots[index];
        slot.gameObject.SetActive(true);

        Item item = itens[index];

        item.transform.SetParent(null);
        item.transform.SetParent(slot, false);

        RectTransform transformUI = item.GetComponent<RectTransform>();
        if (transformUI != null)
        {
            transformUI.anchoredPosition = Vector2.zero;
            transformUI.localScale = Vector3.one;
        }
    }

    public void PosicionarTodos() //função que reposiciona todos os itens
    {
        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(false);
        }
            
        for (int i = 0; i < itens.Count; i++)
        {
            PosicionarItem(i);
        }
    }
}
