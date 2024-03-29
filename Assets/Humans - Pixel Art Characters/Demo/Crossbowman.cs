﻿using UnityEngine;
using System.Collections;

public class Crossbowman : MonoBehaviour {

    [SerializeField] float      m_speed = 1.4f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_projectile;
    [SerializeField] Vector3    m_projectionSpawnOffset;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Humans     m_groundSensor;
    private bool                m_grounded = false;
    private float               m_delayToIdle = 0.0f;
    private int                 m_facingDirection = 1;

    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Humans>();
    }

    // Update is called once per frame
    void Update ()
    {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e"))
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0))
            m_animator.SetTrigger("Attack");

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon && Input.GetKey("left shift"))
        {
            m_speed = 3.5f;
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 2);
        }

        //Walk
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_speed = 1.4f;
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }
    // Animation Event: Called in Crossbow Attack animation.
    public void SpawnProjectile()
    {
        if (m_projectile != null)
        {
            // Set correct arrow spawn position
            Vector3 facingVector = new Vector3(m_facingDirection, 1, 1);
            Vector3 projectionSpawnPosition = transform.localPosition + Vector3.Scale(m_projectionSpawnOffset, facingVector);
            GameObject bolt = Instantiate(m_projectile, projectionSpawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            bolt.transform.localScale = facingVector;
        }
    }
}
