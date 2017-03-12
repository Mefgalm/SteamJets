using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{

    public const int maxHealth = 100;

    [SyncVar]
    public float currentHealth = maxHealth;

    public void OnGUI()
    {
        
    }    

    // Update is called once per frame
    public void TakeDamage(int amount)
    {
        if (!isServer || isLocalPlayer)
        {
            return;
        }

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;            

            RpcRespawn();
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // move back to zero location
            transform.position = Vector3.zero;            
        }
    }
}
