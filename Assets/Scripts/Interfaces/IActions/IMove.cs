using UnityEngine;

public interface IMove
{
    float maxSpeed { get; set; }
    float baseSpeed { get; set; }
    float currentSpeed { get; set; }
    Vector3 moveDirection { get; set; }
    bool orientToDirection { get; set; }

    void Move();
}
