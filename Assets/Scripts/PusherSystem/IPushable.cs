using UnityEngine;

namespace PusherSystem
{
    public interface IPushable
    {
        void Push(Vector3 force, Vector3 collisionPoint);
    }
}