using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllyState { idle, fleeing, fleeingPlayer }
public class Ally : Agent
{
    float time = 0;

    private AllyState state = AllyState.idle;

    public AllyState State
    {
        set
        {
            if(state != AllyState.fleeingPlayer)
            {
                state = AllyState.fleeingPlayer;
            }
        }
    }

    protected List<SpriteInfo> bulletInfo;


    protected override void CalculateSteeringForces()
    {
        GameObject attacker = FindClosest(agentMngr.Attackers);

        switch (state)
        {
            case AllyState.idle:
                renderer.color = Color.green;
                totalForce += Separate(agentMngr.Allies, .5f) * 3;
                totalForce += StayInBounds(3) * 4;

                if (Vector3.Distance(player.transform.position, transform.position) > 1)
                {
                    totalForce += Seek(player);
                }
                totalForce += arrive(player);
                totalForce += Alignment(agentMngr.Allies);
                totalForce += AvoidObstacle() * 30; 
                //swap states
                if (Vector3.Distance(attacker.transform.position, transform.position) < 3)
                {
                    state = AllyState.fleeing;

                }
                
                break;
            case AllyState.fleeing:
                renderer.color = Color.red;
                totalForce += Separate(agentMngr.Allies, .5f) * 3;
                totalForce += StayInBounds(3) * 4;
                totalForce += Flee(attacker) * 1.25f;
                totalForce += AvoidObstacle() * 30;
                if (Vector3.Distance(attacker.transform.position, transform.position) > 3)
                {
                    state = AllyState.idle;

                }
                break;
            case AllyState.fleeingPlayer:
                time += Time.deltaTime;
                renderer.color = Color.yellow;
                totalForce += Separate(agentMngr.Allies, .5f) * 3;
                totalForce += StayInBounds(3) * 4;
                totalForce += Flee(player);
                totalForce += AvoidObstacle() * 30;
                totalForce += Flee(attacker);
                if (time > 10)
                {
                    time = 0;
                    state = AllyState.idle;
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
            if(obj != null)
            Gizmos.DrawLine(transform.position, obj.transform.position);
        }
        //draw agent safe zone
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(0, futureSqrDist / 2f, 0), new Vector3(avoidRadius * 2f, futureSqrDist, avoidRadius));
    }


}
