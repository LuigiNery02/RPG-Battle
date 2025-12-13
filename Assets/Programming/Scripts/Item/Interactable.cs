using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [TextArea]
    public string descricao; //descrição do interagível

    private bool mouseSobreItem; //verifica se o mouse está sobre o interagível
    private DragItem dragItem; //item arrastável

    private void Awake()
    {
        dragItem = GetComponent<DragItem>();
    }

    public void VerificarHover(InputAction.CallbackContext ctx) //função que verifica se o mouse/ponteiro está sobre o item interagível
    {
        if (dragItem != null && dragItem.enabled && dragItem.EstaArrastando())
        {
            if (mouseSobreItem)
            {
                mouseSobreItem = false;
                TooltipManager.Instance.Esconder();
            }
            return;
        }

        Vector2 posicao = ctx.ReadValue<Vector2>();

        var dados = new PointerEventData(EventSystem.current)
        {
            position = posicao
        };

        var resultados = new List<RaycastResult>();
        EventSystem.current.RaycastAll(dados, resultados);

        bool estaSobreEsteItem = false;

        foreach (var resultado in resultados)
        {
            if (resultado.gameObject == gameObject)
            {
                estaSobreEsteItem = true;
                break;
            }
        }

        if (estaSobreEsteItem && !mouseSobreItem)
        {
            mouseSobreItem = true;
            TooltipManager.Instance.Mostrar(descricao, posicao);
        }
        else if (!estaSobreEsteItem && mouseSobreItem)
        {
            mouseSobreItem = false;
            TooltipManager.Instance.Esconder();
        }

        if (mouseSobreItem)
        {
            TooltipManager.Instance.Mostrar(descricao, posicao);
        }
    }
}
