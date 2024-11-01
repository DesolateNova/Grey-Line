using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    [SerializeField] private float speed, scrollSpeed;
    [SerializeField] private GameObject lBound, rBound, lwBound, upBound;
    new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the position of the edges of the camera current frame
        Vector3 boundsLeft = camera.ViewportToWorldPoint(new Vector3(1, 0.5f, camera.transform.position.z));
        Vector3 boundsRight = camera.ViewportToWorldPoint(new Vector3(0, 0.5f, camera.transform.position.z));
        Vector3 boundsTop = camera.ViewportToWorldPoint(new Vector3(0.5f, 0, camera.transform.position.z));
        Vector3 boundsBottom = camera.ViewportToWorldPoint(new Vector3(0.5f, 1, camera.transform.position.z));

        //Get and record boundries of the maps edge
        float left = lBound.transform.position.x; 
        float right = rBound.transform.position.x;
        float top = upBound.transform.position.y;
        float bottom = lwBound.transform.position.y;
        

        //Regulator for camera movespeed if shift is being held
        float moveSpeed = speed;
        if (Input.GetButton("Shift"))
        {
            moveSpeed = speed * 2.5f;
        }
        else
            moveSpeed = speed;
        

        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float moveZ;

        //Regulator for camera scrool in and out
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            moveZ = 1f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            moveZ = -1f;
        }
        else
            moveZ = 0f;

        //Keep camera scroll within appropriate bounds
        if (transform.position.z < -30f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -30f);
            moveZ = 0f;
        }
        else if (transform.position.z > -5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
            moveZ = 0f;
        }

        if (boundsLeft.x < left)
        {
            if (boundsTop.y > top || boundsBottom.y < bottom)
                moveY = 0f;
            transform.Translate(0, moveY, moveZ);
            transform.position = new Vector3(camera.transform.position.x + 0.01f, camera.transform.position.y, camera.transform.position.z);
        }
        else if (boundsRight.x > right)
        {
            if (boundsTop.y > top || boundsBottom.y < bottom)
                moveY = 0f;
            transform.Translate(0, moveY, moveZ);
            transform.position = new Vector3(camera.transform.position.x - 0.01f, camera.transform.position.y, camera.transform.position.z);
        }
        else if (boundsTop.y > top)
        {
            if (boundsLeft.x < left || boundsRight.x > right)
                moveX = 0f;
            transform.Translate(moveX, 0, moveZ);
            transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y - 0.01f, camera.transform.position.z);

        }
        else if (boundsBottom.y < bottom)
        {
            if (boundsLeft.x < left || boundsRight.x > right)
                moveX = 0f;
            transform.Translate(moveX, 0, moveZ);
            transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y + 0.01f, camera.transform.position.z);

        }
        else
            transform.Translate(moveX, moveY, moveZ * scrollSpeed);

    }
}

