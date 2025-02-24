using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Simulation : MonoBehaviour
{

    Vector2 predictedPosition;

    float density;
    float radius;
    private Rigidbody2D rb;
    Vector2 boundsSize = new Vector2(8f, 6f);
    float collisionDamping = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 particleSize = GetComponent<Renderer>().bounds.size;
        radius = particleSize.x;

        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody component

        setPosition(rb.position);
        setVelocity(new Vector2(0,0));
    }

    // Update is called once per frame
    // public void UpdatePhysics(Vector2 pressureAcceleration)
    // {
    //     Vector2 pressureVelocity = pressureAcceleration * Time.deltaTime;

    //     rb.velocity += Vector2.down * gravity * Time.deltaTime + pressureVelocity;
    //     rb.position += rb.velocity * Time.deltaTime;

    //     velocity = rb.velocity;
    //     position = rb.position;

    //     ResolveCollisions();
    // }


    public void ResolveCollisions()
    {
        ResolveBoundCollisions();
        ResolveObjectCollisions();
    }


    public void ResolveBoundCollisions() {
        Vector2 halfBoundsSize = boundsSize / 2 - Vector2.one * radius;

        Vector2 newPosition = rb.position;
        Vector2 newVelocity = rb.velocity;

        // If bigger than right bound, make vel negative 
        if(newPosition.x > halfBoundsSize.x) {

            newPosition.x = halfBoundsSize.x * math.sign(newPosition.x);
            newVelocity.x = -1 * collisionDamping * math.abs(newVelocity.x);

            rb.position = newPosition;
            rb.velocity = newVelocity;
        }
        // If smaller than left bound, make vel positive 
        if(newPosition.x < -halfBoundsSize.x) {

            newPosition.x = halfBoundsSize.x * math.sign(newPosition.x);
            newVelocity.x = collisionDamping * math.abs(newVelocity.x);

            rb.position = newPosition;
            rb.velocity = newVelocity;
        }
        // If bigger than top bound, make vel negative 
        if(newPosition.y > halfBoundsSize.y) {

            newPosition.y = halfBoundsSize.y * math.sign(newPosition.y);
            newVelocity.y = -1 * collisionDamping * math.abs(newVelocity.y);

            rb.position = newPosition;
            rb.velocity = newVelocity;
        }
        // If smaller than bottom bound, make vel positive 
        if(newPosition.y < -halfBoundsSize.y) {

            newPosition.y = halfBoundsSize.y * math.sign(newPosition.y);
            newVelocity.y = collisionDamping * math.abs(newVelocity.y);

            rb.position = newPosition;
            rb.velocity = newVelocity;
        }

    }

    public void ResolveObjectCollisions()
    {
        GameObject cube = GameObject.FindGameObjectWithTag("CollisionObject");
        Vector3 cubeSize = cube.GetComponent<Renderer>().bounds.size;
        Vector3 cubeVelocity = cube.GetComponent<Rigidbody>().velocity;

        Vector2 halfCubeSize = cubeSize / 2;
        Vector3 cubePosition = cube.transform.position;

        Vector2 newPosition = rb.position;
        Vector2 newVelocity = rb.velocity;

        float diffToLeft = newPosition.x + radius - (cubePosition.x - halfCubeSize.x);
        float diffToRight = - newPosition.x + radius + (cubePosition.x + halfCubeSize.x);
        float diffToBottom = newPosition.y + radius - (cubePosition.y - halfCubeSize.y);
        float diffToTop = - newPosition.y + radius + (cubePosition.y + halfCubeSize.y);

        bool withinCube = (diffToLeft >= 0) && (diffToRight >= 0) && (diffToBottom >= 0) && (diffToTop >= 0);

        if(withinCube)
        {
            if (diffToLeft < diffToRight && diffToLeft < diffToTop && diffToLeft < diffToBottom)
            {
                newPosition.x = (cubePosition.x - halfCubeSize.x) - radius;
            } 
            else if (diffToRight < diffToLeft && diffToRight < diffToTop && diffToRight < diffToBottom)
            {
                newPosition.x = (cubePosition.x + halfCubeSize.x) + radius;
            }
            else if (diffToBottom < diffToTop && diffToBottom < diffToRight && diffToBottom < diffToLeft)
            {
                newPosition.y = (cubePosition.y - halfCubeSize.y) - radius;
            }
            else 
            {
                newPosition.y = (cubePosition.y + halfCubeSize.y) + radius;
            }
            newVelocity.x *= -1 * collisionDamping;
            newVelocity.y *= -1 * collisionDamping;

            //newVelocity.x += cubeVelocity.x;
            //newVelocity.y += cubeVelocity.y;

            rb.position = newPosition;
            rb.velocity = newVelocity;
        }

    }



    public Vector2 getPosition()
    {
        return rb.position;
    }
    public void setPosition(Vector2 newPosition)
    {
        rb.position = newPosition;
    }

    public Vector2 getPredictedPosition()
    {
        return predictedPosition;
    }
    public void setPredictedPosition(Vector2 newPosition)
    {
        predictedPosition = newPosition;
    }
    

    public Vector2 getVelocity()
    {
        return rb.velocity;
    }
    public void setVelocity(Vector2 newVelocity)
    {
        rb.velocity = newVelocity;
    }


    public float getDensity()
    {
        return density;
    }
    public void setDensity(float newDensity)
    {
        density = newDensity;
    }

}
