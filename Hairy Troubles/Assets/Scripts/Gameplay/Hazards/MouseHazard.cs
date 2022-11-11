using UnityEngine;

public class MouseHazard : Hazard
{
    #region EXPOSED_FIELD
    [Header("Particle Splash")]
    [SerializeField] private GameObject starsPref = null;
    #endregion

    #region UNITY_CALLS
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StatusEffects player))
        {
            player.TrappedState(timerEffect);

            EnableCollider(false);

            Vector3 newPosition = other.ClosestPoint(new Vector3(this.transform.position.x, other.transform.position.y, this.transform.position.z));
            GameObject go = Instantiate(starsPref, newPosition, starsPref.transform.rotation);
            Destroy(go, 3f);
        }
    }
    #endregion

    #region OVERRIDE_CALLS
    protected override void TriggerEvent()
    {

    }
    #endregion
}