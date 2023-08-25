using UnityEngine;
namespace Valem
{
    public class HandData : MonoBehaviour
    {
        public enum HandModelType { Left, Right }

        public HandModelType handType;
        public Transform root;
        public Animator animator;
        public Transform[] fingerBones;
    }

}

