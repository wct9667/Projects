 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{

    public float score;
    public float playerHealth;
    public float shieldCharge;


    [SerializeField]
    Text scoreLabel;

    [SerializeField]
    Slider healthBar;

    [SerializeField]
    GameObject collisionMngr;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject bubble;

    [SerializeField]
    Slider shield;
    // Start is called before the first frame update
    void Start()
    {
        score = collisionMngr.GetComponent<CollisionManager>().Score;
        playerHealth = player.GetComponent<PlayerMove>().Health;
        shieldCharge = bubble.GetComponent<BubbleMove>().Charge;
    }

    // Update is called once per frame
    void Update()
    {
        score = collisionMngr.GetComponent<CollisionManager>().Score;
        scoreLabel.text = "Score:" + score;
        healthBar.value = playerHealth = player.GetComponent<PlayerMove>().Health;
        shield.value = shieldCharge = bubble.GetComponent<BubbleMove>().Charge;
    }
}
