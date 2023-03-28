using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof((Rigidbody,Collider)))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform _foot;
    [SerializeField]private float _maxSpeed = 5f;
    [SerializeField]private float _acceleration = 0.1f;
    [SerializeField]Camera _camera;
    private Rigidbody _rigidBody;
    private Vector2 _moveVector;
    private Quaternion _xQuat;
    private Quaternion _yQuat;
    private Transform _transform;

    private GameWorld _world;

    private Vector2Int _playerIsInChunk;
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _transform = transform;
        _playerIsInChunk = _world.worldPosToChunkCoordinates(_transform.position);
        _world.LoadChunksAround(_playerIsInChunk);
        _world.RenderLoadedChunks(_playerIsInChunk);
    }

    private void Update() 
    {
        Vector2Int newPlayerIsInChunk = _world.worldPosToChunkCoordinates(_transform.position);
        if (_playerIsInChunk!=newPlayerIsInChunk)
        {
            _world.LoadChunksAround(_playerIsInChunk);
            _world.RenderLoadedChunks(_playerIsInChunk);
            //_world.UnrenderLoadedChunks(_playerIsInChunk);
            _playerIsInChunk = newPlayerIsInChunk;
        }
    }

    private void FixedUpdate()
    {
        if (_moveVector!=new Vector2(0,0))
        {
            
            Vector2 addSpeed = new Vector2(_moveVector.x*_acceleration,_moveVector.y*_acceleration);
            _rigidBody.AddRelativeForce(new Vector3(addSpeed.x,0,addSpeed.y),ForceMode.Acceleration);
            Vector2 horizontalMovement = new Vector2(_rigidBody.velocity.x,_rigidBody.velocity.z);
            if (_rigidBody.velocity.magnitude>=_maxSpeed)
            {
                horizontalMovement = horizontalMovement.normalized*_maxSpeed;
                _rigidBody.velocity = new Vector3(horizontalMovement.x,_rigidBody.velocity.y,horizontalMovement.y);
            }
            Ray ray = new Ray(_foot.position,_foot.forward);
            Physics.Raycast(ray,out RaycastHit hitInfo,_world._blockSize*10f);
            if (hitInfo.transform!=null)
            {
                _transform.Translate(Vector3.up*_world._blockSize);
            }
            
        }
        _camera.transform.localRotation = _yQuat;
        _transform.localRotation = _xQuat;
    }
    public void UpdateMoveVector(Vector2 newValue)
    {
        _moveVector = newValue;
    }
    public void UpdateCameraAngle(Quaternion xQuat,Quaternion yQuat)
    {
        _xQuat = xQuat;
        _yQuat = yQuat;
    }
    public void OnRightMouse()
    {
        var hitInfo = CastARay(30f);

        Vector3 targetPos = hitInfo.point+hitInfo.normal*_world._blockSize/2;
        Vector3Int blockWorldPos = Vector3Int.FloorToInt(targetPos/_world._blockSize);
        Vector2Int chunkCoordinates = new Vector2Int(blockWorldPos.x/_world._chunkWidth,blockWorldPos.z/_world._chunkWidth);
        Vector3Int blockPos = new Vector3Int(
            blockWorldPos.x-chunkCoordinates.x*_world._chunkWidth,
            blockWorldPos.y,
            blockWorldPos.z-chunkCoordinates.y*_world._chunkWidth
        );

        bool success = _world.ModifyBlock(chunkCoordinates,blockPos,BlockType.Stone);

    }
    public void OnLeftMouse()
    {
        var hitInfo = CastARay(30f);

        Vector3 targetPos = hitInfo.point+hitInfo.normal*-_world._blockSize/2;
        Vector3Int blockWorldPos = Vector3Int.FloorToInt(targetPos/_world._blockSize);
        Vector2Int chunkCoordinates = new Vector2Int(blockWorldPos.x/_world._chunkWidth,blockWorldPos.z/_world._chunkWidth);
        Vector3Int blockPos = new Vector3Int(
            blockWorldPos.x-chunkCoordinates.x*_world._chunkWidth,
            blockWorldPos.y,
            blockWorldPos.z-chunkCoordinates.y*_world._chunkWidth
        );

        bool success = _world.ModifyBlock(chunkCoordinates,blockPos,BlockType.Air);
    }
    private RaycastHit CastARay(float maxDistance)
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f));
        Physics.Raycast(ray,out RaycastHit hitInfo,maxDistance);
        return hitInfo;
    }
    public void SetWorld(GameWorld world)
    {
        _world = world;
    }
}
