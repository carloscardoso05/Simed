using System;
using UnityEngine;

public class AoRedor : MonoBehaviour
{
    public event Action<Collision, Interacao, bool> OnCollision;

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.TryGetComponent<Ferida>(out _)) return;
        OnCollision?.Invoke(other, Interacao.Discreta, true);
    }

    private void OnCollisionStay(Collision other) {
        if (other.gameObject.TryGetComponent<Ferida>(out _)) return;
        OnCollision?.Invoke(other, Interacao.Continua, true);
    }
}