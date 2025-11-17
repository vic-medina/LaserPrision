using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LaserPool pool;
    [SerializeField] private AudioSource audioS;

    public void SetPool(LaserPool laserPool)
    {
        pool = laserPool;
    }

    private void OnEnable()
    {
        // Regresar al pool después de 3 segundos
        Invoke(nameof(ReturnSelf), 3f);
        audioS.Play();
    }

    private void OnDisable()
    {
        CancelInvoke(); // Evita invocaciones pendientes
    }

    private void ReturnSelf()
    {
        if (pool != null)
            pool.ReturnToPool(gameObject);
    }
}
