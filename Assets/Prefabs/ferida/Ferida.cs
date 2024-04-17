using UnityEngine;

public enum EstadoFerida { Ruim, Medio, Bom }
[RequireComponent(typeof(Collider))]
public class Ferida : MonoBehaviour
{
    [SerializeField] private GameObject parteVermelha;
    [SerializeField] private GameObject parteAmarela;
    [SerializeField] private Material vermelho;
    [SerializeField] private Material amarelo;
    [SerializeField] private EstadoFerida estadoFerida;
    [SerializeField] private float velocity = 0.2f;

    private void Start()
    {
        amarelo.color = new Color(amarelo.color.r, amarelo.color.g, amarelo.color.b, 1);
        vermelho.color = new Color(vermelho.color.r, vermelho.color.g, vermelho.color.b, 1);
        AtualizarEstadoFerida(EstadoFerida.Ruim);
    }

    private void HandleInteraction()
    {
        switch (estadoFerida)
        {
            case EstadoFerida.Ruim:
                amarelo.color = new Color(amarelo.color.r, amarelo.color.g, amarelo.color.b, amarelo.color.a - velocity * Time.deltaTime);
                if (amarelo.color.a <= 0) AtualizarEstadoFerida(EstadoFerida.Medio);
                break;
            case EstadoFerida.Medio:
                vermelho.color = new Color(vermelho.color.r, vermelho.color.g, vermelho.color.b, vermelho.color.a - velocity * Time.deltaTime);
                if (vermelho.color.a <= 0) AtualizarEstadoFerida(EstadoFerida.Bom);
                break;
            case EstadoFerida.Bom:
                Debug.Log("Bom");
                break;
        }
    }

    private void OnDestroy()
    {
        amarelo.color = new Color(amarelo.color.r, amarelo.color.g, amarelo.color.b, 1);
        vermelho.color = new Color(vermelho.color.r, vermelho.color.g, vermelho.color.b, 1);
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log("Colision detected");
        other.gameObject.TryGetComponent<Pinca>(out var pinca);
        other.gameObject.TryGetComponent<Pomada>(out var pomada);
        if (pinca == null && pomada == null) return;
        Debug.Log("Colision Pinça");
        HandleInteraction();
    }
    
    public void AtualizarEstadoFerida(EstadoFerida novoEstado)
    {
        estadoFerida = novoEstado;
        switch (novoEstado)
        {
            case EstadoFerida.Medio:
                Destroy(parteAmarela);
                break;
            case EstadoFerida.Bom:
                Destroy(parteVermelha);
                break;
        }
    }
}
