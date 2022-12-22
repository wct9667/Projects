using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttackerState { idle, fleeingPlayer }

public class Attacker : Agent
{
    //sprites
    [SerializeField]
    Sprite normalSprite;

    [SerializeField]
    Sprite fleeSprite;
    
    float time = 0;

    private AttackerState state = AttackerState.idle;

    public AttackerState State
    {
        set
        {
            if (state != AttackerState.fleeingPlayer)
            {
                state = AttackerState.fleeingPlayer;
            }
        }
    }


    protected override void CalculateSteeringForces()
    {
        GameObject closestAlly = FindClosest(agentMngr.Allies);
        switch (state)
        {
            case AttackerState.idle:
                renderer.sprite = normalSprite;
                totalForce += StayInBounds(2);
                totalForce += Separate(agentMngr.Attackers, 1f) * 2;
                totalForce += Pursuit(closestAlly);
                totalForce += arrive(closestAlly);
                totalForce += AvoidObstacle();

                break;
            case AttackerState.fleeingPlayer:
                time += Time.deltaTime;
                renderer.sprite = fleeSprite;
                totalForce += Separate(agentMngr.Attackers, 1f);
                totalForce += StayInBounds(2) * 7;
                totalForce += Flee(player) * 2;
                totalForce += AvoidObstacle();
                if (time > 5)
                {
                    time = 0;
                    state = AttackerState.idle;
                }
                break;
        }
    }



    private void OnDrawGizmos()
    {
        Vector3 futurePos = physicsObject.Position + physicsObject.Velocity * (avoidTime);
        float futureSqrDist = Vector3.SqrMagnitude(futurePos - physicsObject.Position);
        futureSqrDist += avoidRadius;
        Gizmos.color = Color.red;
        foreach (GameObject obj in tempList)
        {
            if (obj != null)
                Gizmos.DrawLine(transform.position, obj.transform.position);
        }
        //draw agent safe zone
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(0, futureSqrDist / 2f, 0), new Vector3(avoidRadius * 2f, futureSqrDist, avoidRadius));
    }
}
