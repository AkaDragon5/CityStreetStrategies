﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SimpleCity.AI
{
    [RequireComponent(typeof(Animator))]
    public class AiAgent : MonoBehaviour
    {
        public event Action OnDeath;

        Animator animator;
        public float speed = 0.2f;
        public float rotationSpeed = 10f;

        List<Vector3> pathToGo = new List<Vector3>();
        bool moveFlag = false;
        int index = 0;
        Vector3 endPosition;

        public Color pathColor;
        PathVisualizer pathVisualizer;

        Rigidbody rb;

        private bool stop;

        public GameObject aiDirector;
        public GameObject gameManager;

        private float timer;

        public bool Stop
        {
            get { return stop; }
            set
            {
                stop = value;
                if (animator != null)
                {
                    if (stop)
                    {
                        rb.velocity = Vector3.zero;
                        animator.SetBool("Walk", false);
                    }
                    else
                    {
                        animator.SetBool("Walk", true);
                    }
                }

            }
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            aiDirector = GameObject.Find("AiDirector");
            gameManager = GameObject.Find("GameManager");
            pathVisualizer = FindObjectOfType<PathVisualizer>();
            pathColor = UnityEngine.Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f);
        }

        public void ShowPath()
        {
            pathVisualizer.ShowPath(pathToGo, this, pathColor);
        }

        public void Initialize(List<Vector3> path)
        {
            pathToGo = path;
            index = 1;
            moveFlag = true;
            endPosition = pathToGo[index];
            animator = GetComponent<Animator>();
            Stop = false;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (gameManager.GetComponent<GameManager>().end)
            {
                aiDirector.GetComponent<AiDirector>().agents--;
                Destroy(gameObject);
            }
            if (moveFlag && Stop == false)
            {
                PerformMovement();
            }
        }

        private void PerformMovement()
        {
            if (pathToGo.Count > index)
            {
                float distanceToGo = MoveTheAgent();
                if (distanceToGo < 0.05f)
                {
                    index++;
                    if (index >= pathToGo.Count)
                    {
                        moveFlag = false;
                        aiDirector.GetComponent<AiDirector>().agents--;
                        gameManager.GetComponent<GameManager>().agentsArrived++;
                        gameManager.GetComponent<GameManager>().agentsTimes += timer;
                        Destroy(gameObject);
                        return;
                    }
                    endPosition = pathToGo[index];
                }
            }
        }

        private float MoveTheAgent()
        {
            float step = speed * Time.deltaTime;
            Vector3 endPositionCorrect = new Vector3(endPosition.x, transform.position.y, endPosition.z);
            //transform.position = Vector3.MoveTowards(transform.position, endPositionCorrect, step);
            rb.velocity = transform.forward * speed;

            var lookDirection = endPositionCorrect - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * rotationSpeed);
            return Vector3.Distance(transform.position, endPositionCorrect);
        }

        private void OnDestroy()
        {

            OnDeath?.Invoke();

        }
    }
}

