using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ferida : MonoBehaviour
{
    [SerializeField] private int limiteLimpezaPorFora = 4;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private GameObject corpoFerida;
    [SerializeField] private GameObject corpoFeridaComParteAmarela;
    [SerializeField] private Collider areaFerida;
    private float _limiteLavagemDaFerida = 2f;
    private float _limiteAplicacaoDeHidrogel = 2f;
    private float _limiteSecagemPorFora = 2f;


    private readonly string[] _instructions =
    {
        "Limpar a parte de fora da ferida com gaze",
        "Remover o tecido amarelo do local com tesoura ou bisturi",
        "Lavar a ferida com jatos de soro usando a seringa",
        "Colocar hidrogel",
        "Secar com gaze por fora de ferida",
        "Umidecer gaze e colocar por cima da lesão",
        "Fechar com esparadrapo",
        "Tratamento finalizado"
    };

    [SerializeField] private Tratamento tratamento = Tratamento.LimpezaPorFora;
    [SerializeField] private float velocity = 0.2f;

    private void Start()
    {
        corpoFerida.SetActive(false);
        corpoFeridaComParteAmarela.SetActive(true);
        progressBar.UpdateProgressBar(_instructions[(int)tratamento]);
    }

    private void HandleInteraction(Collision other, Interacao interacao)
    {
        Debug.Log($"Velocidade relativa: {other.relativeVelocity}");
        switch (tratamento)
        {
            case Tratamento.LimpezaPorFora:
                if (interacao != Interacao.Discreta) break;
                if (CollisionHasAnyComponents(other, typeof(Gaze)))
                {
                    Debug.Log(limiteLimpezaPorFora);
                    limiteLimpezaPorFora -= 1;
                    if (limiteLimpezaPorFora == 0)
                    {
                        AtualizarTratamento();
                    }
                }

                break;

            case Tratamento.RemocaoDoTecidoAmarelo:
                if (interacao != Interacao.Discreta) break;
                if (CollisionHasAnyComponents(other, typeof(Tesoura), typeof(Bisturi)))
                {
                    AtualizarTratamento();
                }

                break;

            case Tratamento.LavagemDaFerida:
                if (interacao != Interacao.Continua) break;
                if (CollisionHasAnyComponents(other, typeof(Seringa)))
                {
                    _limiteLavagemDaFerida -= Time.deltaTime;
                    if (_limiteLavagemDaFerida <= 0) AtualizarTratamento();
                }

                break;

            case Tratamento.AplicacaoDeHidrogel:
                if (interacao != Interacao.Continua) break;
                if (CollisionHasAnyComponents(other, typeof(Hidrogel)))
                {
                    Debug.Log(_limiteAplicacaoDeHidrogel);
                    _limiteAplicacaoDeHidrogel -= Time.deltaTime;
                    if (_limiteAplicacaoDeHidrogel <= 0) AtualizarTratamento();
                }

                break;
            case Tratamento.SecagemPorFora:
                if (interacao != Interacao.Continua) break;
                if (CollisionHasAnyComponents(other, typeof(Gaze)))
                {
                    Debug.Log(_limiteSecagemPorFora);
                    _limiteSecagemPorFora -= Time.deltaTime;
                    if (_limiteSecagemPorFora <= 0) AtualizarTratamento();
                }

                break;
            case Tratamento.AplicarGazeHumidecida:
                if (interacao != Interacao.Discreta) break;
                if (CollisionHasAnyComponents(other, typeof(Gaze)))
                {
                    AtualizarTratamento();
                }

                break;
            case Tratamento.FecharComEsparadrapo:
                if (interacao != Interacao.Discreta) break;
                if (CollisionHasAnyComponents(other, typeof(Esparadrapo)))
                {
                    AtualizarTratamento();
                }

                break;
            default:
                Debug.Log($"{tratamento} não tratado.");
                break;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log("Discreta");
        HandleInteraction(other, Interacao.Discreta);
    }

    private void OnCollisionStay(Collision other)
    {
        Debug.Log("Continua");
        HandleInteraction(other, Interacao.Continua);
    }

    private void OnTriggerExit(Collision other)
    {
        Debug.Log("Discreta");
        HandleInteraction(other, Interacao.Discreta);
    }

    private void OnTriggerStay(Collision other)
    {
        Debug.Log("Continua");
        HandleInteraction(other, Interacao.Continua);
    }

    public void AtualizarTratamento()
    {
        if (tratamento == Tratamento.RemocaoDoTecidoAmarelo)
        {
            corpoFerida.SetActive(true);
            corpoFeridaComParteAmarela.SetActive(false);
        }


        Debug.Log($"Indo de {tratamento} para {tratamento.Next()}");
        tratamento = tratamento.Next();
        progressBar.UpdateProgressBar(_instructions[(int)tratamento]);
    }

    private bool CollisionHasAnyComponents(Collision collision, params Type[] components)
    {
        foreach (var component in components)
        {
            if (collision.gameObject.TryGetComponent(component, out _))
            {
                return true;
            }
        }

        return false;
    }
}