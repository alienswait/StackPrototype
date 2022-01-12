using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
   
   public enum Direction
    {
        Left,Right,Front,Back
    }
   
   
    [SerializeField] private Transform referance;
    [SerializeField] private MeshRenderer referanceMesh;
    [SerializeField] private GameObject fallingPreFab;
    [SerializeField] private GameObject standPreFab;
    [SerializeField] private Transform last;




    [SerializeField] float speed;
    [SerializeField] float limit;


    bool _isForward;
    bool _isAxisX;
    bool _isStop;


    //[SerializeField] private float TEST_VALUE;
    //[SerializeField] private bool TEST_IS_HORIZONTAL;
    //[ContextMenu("TEST")]
    //private void TEST() 
   //{
   //    DividedObject(TEST_IS_HORIZONTAL , TEST_VALUE);
   //}

    

   private void Update() 
   {

       if(_isStop)
       {
           return;
       }

       var position = transform.position;
       var Direction = _isForward ? 1 : -1; 
       
       var move = speed * Time.deltaTime * Direction;
       if (_isAxisX)
       {
           position.x += move;   
       }
       else
       {
           position.z += move;
       }
        // objenin limitlerine ve dönmelerine baktım.
       if (_isAxisX)
       {
           if (position.x < -limit || position.x > limit)
           {
               position.x = Mathf.Clamp(position.x, -limit, limit);
               _isForward = !_isForward;
           }
       }
       else
       {
           if (position.z < -limit || position.z > limit)
           {
               position.z = Mathf.Clamp(position.z, -limit, limit);
               _isForward = !_isForward;
           }
       }

       transform.position = position;
   }

    private void DividedObject(bool isAxisX,float value)
    {
        
        bool isFallingFirst = value > 0;

        var falling = Instantiate(fallingPreFab).transform;
        var stand = Instantiate(standPreFab).transform;


        //boyut ayarlama
        var fallingSize = referance.localScale;
        if (isAxisX)
        {
            fallingSize.x = Mathf.Abs(value);
        }
        else
        {
            fallingSize.z = Mathf.Abs(value);
        }
        
        falling.localScale = fallingSize;
    
        var standSize = referance.localScale;
        if (isAxisX)
        {
            standSize.x = referance.localScale.x - Mathf.Abs(value);
        }
        else
        {
            standSize.z = referance.localScale.z - Mathf.Abs(value);
        }
        stand.localScale = standSize;


        var minDirection = isAxisX ? Direction.Left : Direction.Back;
        var maxDirection = isAxisX ? Direction.Right : Direction.Front;


        //pozisyon ayarlama
        var fallingPosition = GetPositionEdge(referanceMesh, isFallingFirst ? minDirection : maxDirection);
        
        var fallingMultiply = (isFallingFirst ? 1 : -1); //kod tekrarını engellemek için atadım.

        if (isAxisX)
        {
            fallingPosition.x += (fallingSize.x / 2) * fallingMultiply; 
        }
        else
        {
            fallingPosition.z += (fallingSize.z / 2) * fallingMultiply; 
        } 
        falling.position = fallingPosition;
        
        
        var standPosition = GetPositionEdge(referanceMesh, !isFallingFirst ? minDirection : maxDirection);
        
        var standMultiply = (!isFallingFirst ? 1 : -1); // kod tekrarını engellemek için değişkenlere atadım.
        
        if(isAxisX)
        {
            standPosition.x += (standSize.x / 2 ) * standMultiply;
        }
        else
        {
            standPosition.z += (standSize.z / 2 ) * standMultiply;
        }
        stand.position = standPosition;
    
        last = stand;
    
    } 



// MeshRenderer sayesinde köşe bilgilerini aldık
    private Vector3 GetPositionEdge(MeshRenderer mesh, Direction direction)
    {
        var extends = mesh.bounds.extents;
        var positions = mesh.transform.position;

        
        switch (direction)
        {
            case Direction.Left:
            positions.x += -extends.x;
            break;
            case Direction.Right:
            positions.x += extends.x;
            break;
            case Direction.Front:
            positions.z += extends.z;
            break;
            case Direction.Back:
            positions.z += -extends.z;
            break;
        }

        return positions;   
    }

    

    public void OnClick()
    {
       _isStop = true;

       var distance = last.position - transform.position;

       DividedObject(_isAxisX,_isAxisX ? distance.x : distance.z);
    
        //objeleri resetleme.
        _isAxisX = !_isAxisX;
        var NewPosition = last.position;
        NewPosition.y += transform.localScale.y;

        if (!_isAxisX)
        {
            NewPosition.z = limit;
        }
        else
        {
            NewPosition.x = limit;
        }
        transform.position =NewPosition;
        

        transform.localScale = last.localScale;

        _isStop = false;
    
    }



    
 


}
