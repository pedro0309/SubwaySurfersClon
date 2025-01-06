using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum CollisionX { None, Left, Middle, Right}
public enum CollisionY { None, Up, Middle, Down, LowDown}
public enum CollisionZ { None, Forward,Middle, Backward}

public class PlayerCollision : MonoBehaviour
{
    private PlayerController playerController;
    private CollisionX _collisionX;
    private CollisionY _collisionY;
    private CollisionZ _collisionZ;

    public CollisionX CollisionX { get => _collisionX; set => _collisionX = value; }
    public CollisionY CollisionY { get => _collisionY; set => _collisionY = value; }
    public CollisionZ CollisionZ { get => _collisionZ; set => _collisionZ = value; }

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void OnCharacterCollision(Collider collider)
    {
        _collisionX = GetCollisionX(collider);
        _collisionY = GetCollisionY(collider);
        _collisionZ = GetCollisionZ(collider);
        SetAnimatorByCollision(collider);
    }

    private void SetAnimatorByCollision(Collider collider)
    {
        if (_collisionZ == CollisionZ.Backward && _collisionX == CollisionX.Middle) //Si choca por debajo del obj y en medio
        {
            CollisionZOnBackwardAndMiddle(collider);
        }
        else if (_collisionZ == CollisionZ.Middle)
        {
            CollisionZOnMiddle(collider);  
        }
        else
        {
            CollisionXOnSide(collider);
        }
    }

    private CollisionX GetCollisionX(Collider collider) 
    {
        Bounds characterControllerBounds = playerController.myCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minX = Mathf.Max(colliderBounds.min.x, characterControllerBounds.min.x);
        float maxX = Mathf.Min(colliderBounds.max.x, characterControllerBounds.max.x);
        float average = (minX + maxX) /2 - colliderBounds.min.x;
        CollisionX colx;
        if (average > colliderBounds.size.x - 0.33f)
        {
            colx = CollisionX.Right;
        }
        else if (average < 0.33f)
        {
            colx = CollisionX.Left;
        }
        else
        {
            colx = CollisionX.Middle;
        }
        return colx;
    }

    private CollisionY GetCollisionY(Collider collider)
    {
        Bounds characterControllerBounds = playerController.myCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minY = Mathf.Max(colliderBounds.min.y, characterControllerBounds.min.y);
        float maxY = Mathf.Min(colliderBounds.max.y, characterControllerBounds.max.y);
        float average = (minY + maxY) / 2 - colliderBounds.min.y;
        CollisionY coly;
        if (average > colliderBounds.size.y - 0.33f)
        {
            coly = CollisionY.Up;
        }
        else if (average < 0.17f)
        {
            coly = CollisionY.LowDown;
        }
        else if (average < 0.33f)
        {
            coly = CollisionY.Down;
        }
        else
        {
            coly = CollisionY.Middle;
        }
        return coly;
    }

    private CollisionZ GetCollisionZ(Collider collider)
    {
        Bounds characterControllerBounds = playerController.myCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minZ = Mathf.Max(colliderBounds.min.z, characterControllerBounds.min.z);
        float maxZ = Mathf.Min(colliderBounds.max.z, characterControllerBounds.max.z);
        float average = (minZ + maxZ) / 2 - colliderBounds.min.z;
        CollisionZ colz;
        if (average > colliderBounds.size.z - 0.33f)
        {
            colz = CollisionZ.Forward;
        }
        else if (average < 0.33f)
        {
            colz = CollisionZ.Backward;
        }
        else
        {
            colz = CollisionZ.Middle;
        }
        return colz;
    }
    public void CollisionZOnBackwardAndMiddle(Collider collider)
    {
        if (_collisionY == CollisionY.LowDown) // Se agachará corriendo el Player
        {
            collider.enabled = false;
            playerController.SetPlayerAnimator(playerController.IdStumbleLow, false);
        }
        else if (_collisionY == CollisionY.Down) //Morirá chocando porque no cabe en el espacio libre del objeto y el suelo
        {
            playerController.SetPlayerAnimator(playerController.IdDeathLower, false);
            GameManager.instance.GameOver();
        }
        else if (_collisionY == CollisionY.Middle) //Colisión Frontal y a la Mitad
        {
            if (collider.CompareTag("TrainOn"))
            {
                playerController.SetPlayerAnimator(playerController.IdDeathMovingTrain, false);
                GameManager.instance.GameOver();
            }
            else if (!collider.CompareTag("Ramp")) //Si la colisión no es con Ramp, que ejecute lo sig:
            {
                playerController.SetPlayerAnimator(playerController.IdDeathBounce, false);
                GameManager.instance.GameOver();
            }
        }
        else if (_collisionY == CollisionY.Up && !playerController.IsRolling)
        {
            playerController.SetPlayerAnimator(playerController.IdDeathUpper, false);
            GameManager.instance.GameOver();
        }
    }
    public void CollisionZOnMiddle(Collider collider)
    {
        if (_collisionX == CollisionX.Right && PlayerController.instance.position == Side.Left)
        {
            playerController.SetPlayerAnimator(playerController.IdStumbleSideRight, false);
            PlayerController.instance.UpdatePlayerXPosition(Side.Middle);
            print("Rebotado a la derecha");
        }
        else if (_collisionX == CollisionX.Right && PlayerController.instance.position == Side.Middle)
        {
            playerController.SetPlayerAnimator(playerController.IdStumbleSideRight, false);
            PlayerController.instance.UpdatePlayerXPosition(Side.Right);
        }

        if (_collisionX == CollisionX.Left && PlayerController.instance.position == Side.Right)
        {
            playerController.SetPlayerAnimator(playerController.IdStumbleSideLeft, false);
            print("Rebotado a la izquierda");
            PlayerController.instance.UpdatePlayerXPosition(Side.Middle);
        }
        else if (_collisionX == CollisionX.Left && PlayerController.instance.position == Side.Middle)
        {
            playerController.SetPlayerAnimator(playerController.IdStumbleSideRight, false);
            PlayerController.instance.UpdatePlayerXPosition(Side.Left);
        }
    }
    public void CollisionXOnSide(Collider collider)
    {
        if (_collisionX == CollisionX.Right)
        {
            playerController.SetPlayerAnimatorWithLayer(playerController.IdStumbleCornerRight);//Combinará la Animación de CornerRight con el del SetPlayerAnimatorWithLayer, el cual es animator.SetLayerWeight(1,1)
        }
        else if (_collisionX == CollisionX.Left)
        {
            playerController.SetPlayerAnimatorWithLayer(playerController.IdStumbleCornerLeft);
        }
    }
}
