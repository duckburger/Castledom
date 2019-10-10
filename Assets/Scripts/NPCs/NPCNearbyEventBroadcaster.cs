using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNearbyEventBroadcaster : MonoBehaviour
{
    [SerializeField] LayerMask notificationMask;
    List<NPCAI> nearbyNPCs = new List<NPCAI>();

    public void BroadcastAttackReaction(Health attacker)
    {
        for (int i = 0; i < nearbyNPCs.Count; i++)
        {
            nearbyNPCs[i].SetInCombatExternally(attacker);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (notificationMask == ((1 << collision.gameObject.layer) | notificationMask))
        {
            NPCAI aiController = collision.GetComponent<NPCAI>();
            if (aiController != null && !nearbyNPCs.Contains(aiController))
                nearbyNPCs.Add(aiController);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (notificationMask == ((1 << collision.gameObject.layer) | notificationMask))
        {
            NPCAI aiController = collision.GetComponent<NPCAI>();
            if (aiController != null && nearbyNPCs.Contains(aiController))
                nearbyNPCs.Remove(aiController);
        }
    }
}
