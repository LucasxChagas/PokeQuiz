using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeletorDeEnigma : MonoBehaviour {
    [Header ("Pergunta/Respostas")]
    [SerializeField] ListaDeEnigmas lista;
    [SerializeField] Text perguntaTexto;
    [SerializeField] Text botao1Texto;
    [SerializeField] Text botao2Texto;
    [SerializeField] Text botao3Texto;
    [SerializeField] Text botao4Texto;
    [SerializeField] Text nivel;
    [SerializeField] Text scoreTexto;
    [SerializeField] Text recordTexto;

    [Header ("Telas")]
    [SerializeField] GameObject perguntas;
    [SerializeField] GameObject endGame;

    [Header ("Imagem")]
    [SerializeField] Image imagem;

    [Header ("Ending Sound")]
    [SerializeField] AudioSource song;

    [Header ("Power Up")]
    [SerializeField] Button powerUp;

    [Header ("End Game")]
    [SerializeField] Text scoreTextEnd;
    [SerializeField] Text recordTextEnd;

    List<string> respostasPossiveis = new List<string> ();
    int index;
    int score;
    int record;

    void Start () {
        // Resolu��o
        Screen.SetResolution (1366, 768, Screen.fullScreen);

        // Inicia o Bot�o de PowerUp Desativado
        powerUp.interactable = false;

        // Inicia o Score em 0
        scoreTexto.text = "Score: " + score.ToString () + "/1110";

        // Pega a chave/vari�vel record em PlayerPrefs, e se n�o tiver ele preenche com 0
        record = PlayerPrefs.GetInt ("record", 0);
        recordTexto.text = "Record: " + record.ToString ();

        // Seleciona um n�mero aleat�rio entre 0 e o tamanho da Lista de Enigmas
        index = Random.Range (0, lista.listaDeEnigmas.Count);

        // Ap�s selecionar o n�mero, utiliza ele como index para gerar a pergunta, a imagem, o n�vel e as respostas
        perguntaTexto.text = lista.listaDeEnigmas[index].pergunta;
        imagem.sprite = lista.listaDeEnigmas[index].pokeImage;
        nivel.text = lista.listaDeEnigmas[index].nivel;

        // Adiciona as respostas baseadas no index que foi selecionado � lista de respostas poss�veis
        respostasPossiveis.Add (lista.listaDeEnigmas[index].respostaCorreta);
        respostasPossiveis.Add (lista.listaDeEnigmas[index].respostaErrada1);
        respostasPossiveis.Add (lista.listaDeEnigmas[index].respostaErrada2);
        respostasPossiveis.Add (lista.listaDeEnigmas[index].respostaErrada3);

        // Seleciona um outro n�mero aleat�rio mas dessa vez dentro da lista de respostas poss�veis
        int indexRespostas = Random.Range (0, respostasPossiveis.Count);

        // Ap�s selecionar qual resposta estar� dentro do bot�o...
        botao1Texto.text = respostasPossiveis[indexRespostas]; // Seleciona qual resposta vai estar no bot�o
        respostasPossiveis.Remove (respostasPossiveis[indexRespostas]); // Remove essa resposta da lista
        indexRespostas = Random.Range (0, respostasPossiveis.Count); // Seleciona mais um n�mero aleat�rio dentro dessa lista

        botao2Texto.text = respostasPossiveis[indexRespostas];
        respostasPossiveis.Remove (respostasPossiveis[indexRespostas]);
        indexRespostas = Random.Range (0, respostasPossiveis.Count);

        botao3Texto.text = respostasPossiveis[indexRespostas];
        respostasPossiveis.Remove (respostasPossiveis[indexRespostas]);
        indexRespostas = Random.Range (0, respostasPossiveis.Count);

        botao4Texto.text = respostasPossiveis[indexRespostas];
        respostasPossiveis.Remove (respostasPossiveis[indexRespostas]);

    }

    public void Finalizar () {
        perguntas.SetActive (false);
        endGame.SetActive (true);
        song.Play ();
        scoreTextEnd.text = score.ToString () + " Points";
        record = PlayerPrefs.GetInt ("record", 0);
        recordTextEnd.text = record.ToString () + " Points";
    }

    public void Responder (Text TextoBotao) {
        if (TextoBotao.text == lista.listaDeEnigmas[index].respostaCorreta) {
            if (lista.listaDeEnigmas[index].nivel.Equals ("Easy")) {
                score += 5;
            } else if (lista.listaDeEnigmas[index].nivel.Equals ("Hard")) {
                score += 10;
            } else {
                score += 1;
            }
            lista.listaDeEnigmas.Remove (lista.listaDeEnigmas[index]);

            scoreTexto.text = "Score: " + score.ToString () + "/1110";

            if (lista.listaDeEnigmas.Count == 0) {
                if (score > record) {
                    record = score;
                    recordTexto.text = "Record: " + record.ToString ();
                    PlayerPrefs.SetInt ("record", record);
                }
                Finalizar ();
            }

            Start ();
        } else {
            if (score > record) {
                record = score;
                recordTexto.text = "Record: " + record.ToString ();
                PlayerPrefs.SetInt ("record", record);
            }
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        }
    }

    public void PowerUp () {
        lista.listaDeEnigmas.Remove (lista.listaDeEnigmas[index]);
        score -= 10;
        Start ();

    }

    public void EnableButton () {
        if (score >= 10) {
            powerUp.interactable = true;
        }
    }

    public void BackToMenu () {
        SceneManager.LoadScene (0);
    }

    public void QuitGame () {
        Application.Quit ();
    }

}