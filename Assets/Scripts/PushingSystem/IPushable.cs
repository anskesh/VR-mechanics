using UnityEngine;

namespace PushingSystem
{
    public interface IPushable
    {
        void Push();
        void Push(Vector3 force, Vector3 collisionPoint);
    }
}