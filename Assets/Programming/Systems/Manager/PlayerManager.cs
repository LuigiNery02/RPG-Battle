using UnityEngine;
using UnityEngine.Events;

public enum Fase { FaseDeLoja, FaseDeBatalha}
public enum Round { Round1, Round2, Round3, Round4, Round5, Round6, Round7, Round8, Round9, Round10, Round11, Round12, Round13, Round14, Round15, Round16, Round17, Round18, Round19 }
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public int ouro; //valor do ouro do jogador
    public Fase faseDeJogoAtual; //define a fase do jogo atual
    public int roundAtual; //define o round atual

    public UnityEvent quandoIniciarFaseDeLoja; //evento que é chamado quando se inicia a fase de loja
    public UnityEvent quandoIniciarFaseDeBatalha; //evento que é chamado quando se inicia a fase de batalha

    public InputManager inputManager;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        inputManager = new InputManager();
        inputManager.Enable();
    }

    private void Start()
    {
        faseDeJogoAtual = Fase.FaseDeLoja;
        roundAtual = 1;
        ouro += (40 * 5);
    }

    public void TrocarFase() //função que troca a fase de jogo
    {
        if(faseDeJogoAtual == Fase.FaseDeLoja)
        {
            faseDeJogoAtual = Fase.FaseDeBatalha;
            quandoIniciarFaseDeLoja.Invoke();
        }
        else
        {
            faseDeJogoAtual = Fase.FaseDeLoja;
            quandoIniciarFaseDeBatalha.Invoke();
        }
    }

    public void ProximoRound() //função que avança para o próximo round
    {
        roundAtual++;

        if(roundAtual >= 2 && roundAtual <= 4)
        {
            ouro += (45 * 5);
        }
        else if(roundAtual == 5 || roundAtual == 6)
        {
            ouro += (50 * 5);
        }
        else if (roundAtual == 7)
        {
            ouro += (55 * 5);
        }
        else if (roundAtual == 8)
        {
            ouro += (60 * 5);
        }
        else if (roundAtual == 9)
        {
            ouro += (65 * 5);
        }
        else if(roundAtual >= 10)
        {
            ouro += (75 * 5);
        }

        TrocarFase();
    }
}
