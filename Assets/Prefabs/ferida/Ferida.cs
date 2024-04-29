using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ferida : MonoBehaviour
{
    [SerializeField] private GameObject parteVermelha;
    [SerializeField] private GameObject parteAmarela;
    [SerializeField] private AoRedor aoRedor;
    [SerializeField] private int limiteLimpezaPorFora = 4;
    private float limiteLavagemDaFerida = 2f;
    private float limiteAplicacaoDeHidrogel = 2f;
    private float limiteSecagemPorFora = 2f;

    private Material vermelho
    {
        get => parteVermelha.GetComponent<MeshRenderer>().material;
    }
    private Material amarelo
    {
        get => parteAmarela.GetComponent<MeshRenderer>().material;
    }
    [SerializeField] private Tratamento tratamento = Tratamento.LimpezaPorFora;
    [SerializeField] private float velocity = 0.2f;

    private void Start()
    {
        amarelo.color = new Color(amarelo.color.r, amarelo.color.g, amarelo.color.b, 1);
        vermelho.color = new Color(vermelho.color.r, vermelho.color.g, vermelho.color.b, 1);
        aoRedor.OnCollision += HandleInteraction;
    }

    private void HandleInteraction(Collision other, Interacao interacao, bool aoRedor)
    {
        Debug.Log($"Velocidade relativa: {other.relativeVelocity}");
        switch (tratamento)
        {
            case Tratamento.LimpezaPorFora:
                if (interacao != Interacao.Discreta || !aoRedor) break;
                if (CollisionHasAnyComponents(other, typeof(Gaze)))
                {
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
                    amarelo.color = new Color(amarelo.color.r, amarelo.color.g, amarelo.color.b, math.max(amarelo.color.a - velocity, 0));
                    if (amarelo.color.a <= 0)
                    {
                        AtualizarTratamento();
                    }
                }
                break;
            case Tratamento.LavagemDaFerida:
                if (interacao != Interacao.Continua) break;
                if (CollisionHasAnyComponents(other, typeof(Seringa)))
                {
                    limiteLavagemDaFerida -= Time.deltaTime;
                    if (limiteLavagemDaFerida <= 0) AtualizarTratamento();
                }
                break;
            case Tratamento.AplicacaoDeHidrogel:
                if (interacao != Interacao.Continua) break;
                if (CollisionHasAnyComponents(other, typeof(Hidrogel)))
                {
                    limiteAplicacaoDeHidrogel -= Time.deltaTime;
                    if (limiteAplicacaoDeHidrogel <= 0) AtualizarTratamento();
                }
                break;
            case Tratamento.SecagemPorFora:
                if (interacao != Interacao.Continua || !aoRedor) break;
                if (CollisionHasAnyComponents(other, typeof(Gaze)))
                {
                    limiteSecagemPorFora -= Time.deltaTime;
                    if (limiteSecagemPorFora <= 0) AtualizarTratamento();
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
        if (other.gameObject.TryGetComponent<AoRedor>(out _)) return;
        HandleInteraction(other, Interacao.Discreta, false);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent<AoRedor>(out _)) return;
        HandleInteraction(other, Interacao.Continua, false);
    }

    public void AtualizarTratamento()
    {
        switch (tratamento)
        {
            case Tratamento.RemocaoDoTecidoAmarelo:
                Destroy(parteAmarela);
                break;
        }
        Debug.Log($"Indo de {tratamento} para {tratamento.Next()}");
        tratamento = tratamento.Next();
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
