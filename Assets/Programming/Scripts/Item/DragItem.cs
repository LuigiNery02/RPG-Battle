using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DragItem : Interactable
{
    [HideInInspector]
    public ScrollRect scrollRect; //componente scroll rect dentro de um pai

    private Canvas canvas; //canvas
    private CanvasGroup grupoCanvas; //grupo canvas
    private LayoutGroup layout; //grupo do layout que o item estiver
    private RectTransform transformUI; //transform (UI) do objeto
    private bool arrastando; //verifica se está arrastando o item
    private Vector2 deslocamento; //valores de deslocamento
    private Vector2 posicaoInicial; //posição inicial do item
    private Transform paiInicial; //pai inicial do item
    private Transform destinoGlobal; //destino global do item após ser arrastado pela primeira vez
    private Transform dragCanvas; //canvas para quando o item for arrastado

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        grupoCanvas = GetComponent<CanvasGroup>();
        transformUI = GetComponent<RectTransform>();
        layout = GetComponentInParent<LayoutGroup>();

        destinoGlobal = canvas.transform;
    }
    private void OnEnable()
    {
        //configura os inputs às funções
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.inputManager.Shop.PointerPress.started += Clicar;
            PlayerManager.Instance.inputManager.Shop.PointerPress.canceled += Clicar;
            PlayerManager.Instance.inputManager.Shop.PointerPosition.performed += Mover;
            PlayerManager.Instance.inputManager.Shop.PointerPosition.performed += VerificarHover;
        }
    }
    private void OnDisable()
    {
        //retira as configurações dos inputs das funções
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.inputManager.Shop.PointerPress.started -= Clicar;
            PlayerManager.Instance.inputManager.Shop.PointerPress.canceled -= Clicar;
            PlayerManager.Instance.inputManager.Shop.PointerPosition.performed -= Mover;
            PlayerManager.Instance.inputManager.Shop.PointerPosition.performed -= VerificarHover;
        }
        TooltipManager.Instance?.Esconder();
    }
    public void Clicar(InputAction.CallbackContext ctx) //função de clique ou apertar
    {
        if (ctx.started)
        {
            ArrastarInicial();
        }
        else if (ctx.canceled)
        {
            FinalizarArrastar();
        }
    }
    public void Mover(InputAction.CallbackContext ctx) //função de mover o mouse/ponteiro
    {
        if (arrastando)
        {
            Vector2 posicao = ctx.ReadValue<Vector2>();
            Arrastar(posicao);
        }
    }
    private void ArrastarInicial() //função que define a posição incial do item ao começar a arrastar
    {
        posicaoInicial = transformUI.anchoredPosition;
        paiInicial = transform.parent;

        Vector2 posicao;

        if (PlayerManager.Instance.inputManager != null)
        {
            posicao = PlayerManager.Instance.inputManager.Shop.PointerPosition.ReadValue<Vector2>();

            var dados = new PointerEventData(EventSystem.current)
            {
                position = posicao
            };
            var resultados = new List<RaycastResult>();
            EventSystem.current.RaycastAll(dados, resultados);

            if(resultados.Count > 0 && resultados[0].gameObject == gameObject)
            {
                arrastando = true;

                if (scrollRect != null)
                {
                    scrollRect.enabled = false;
                }

                transform.SetParent(destinoGlobal, true);

                if (layout != null)
                {
                    layout.enabled = false;
                }

                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, posicao, canvas.worldCamera, out Vector2 canvasPonteiroPosicao);
                deslocamento = canvasPonteiroPosicao - transformUI.anchoredPosition;
                grupoCanvas.blocksRaycasts = false;
            }
        }
    }
    private void Arrastar(Vector2 posicao) //função que arrasta o item em tempo real
    {
        if (arrastando)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, posicao, canvas.worldCamera, out Vector2 canvasPonteiroPosicao);
            transformUI.anchoredPosition = canvasPonteiroPosicao - deslocamento;
        }
    }

    public bool EstaArrastando()//verifica se o item está sendo arrastado
    {
        return arrastando;
    }

    private void FinalizarArrastar() //função que finaliza o arrastar e solta o item
    {
        if (arrastando)
        {
            arrastando = false;
            grupoCanvas.blocksRaycasts = true;

            Vector2 posicaoPonteiro = PlayerManager.Instance.inputManager.Shop.PointerPosition.ReadValue<Vector2>();
            var dados = new PointerEventData(EventSystem.current)
            {
                position = posicaoPonteiro
            };
            var resultados = new List<RaycastResult>();
            EventSystem.current.RaycastAll(dados, resultados);

            bool encontrouDestino = false;

            foreach (var resultado in resultados)
            {
                var dropArea = resultado.gameObject.GetComponent<DropArea>();
                if (dropArea != null)
                {
                    if (scrollRect != null)
                    {
                        scrollRect.enabled = true;
                    }
                    dropArea.AoSoltarItem(this);
                    encontrouDestino = true;
                    break;
                }
            }

            if (!encontrouDestino)
            {
                transform.SetParent(paiInicial, false);
                transformUI.anchoredPosition = posicaoInicial;
            }

            if (layout != null)
            {
                layout.enabled = true;
            }
        }
    }

    public void VoltarPosicaoOriginal()
    {
        transform.SetParent(paiInicial, false);
        transformUI.anchoredPosition = posicaoInicial;
    }
}
