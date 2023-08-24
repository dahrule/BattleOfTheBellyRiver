using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Valem
{
    public class GrabHandPose : MonoBehaviour
    {
        [SerializeField] float poseTransitionDuration = 0.3f;
        public HandData HandData;

        private Vector3 startingHandPosition;
        private Vector3 finalHandPosition;
        private Quaternion startingHandRotation;
        private Quaternion finalHandRotation;

        private Quaternion[] startingFingerRotations;
        private Quaternion[] finalFingerRotations;



        void Start()
        {
            HandData.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
            grabInteractable.selectEntered.AddListener(SetupPose);
            grabInteractable.selectExited.AddListener(UnSetPose);
        }
        private void OnDisable()
        {
            XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
            grabInteractable.selectEntered.RemoveListener(SetupPose);
            grabInteractable.selectExited.RemoveListener(UnSetPose);
        }

        public void SetupPose(BaseInteractionEventArgs arg)
        {

            if (arg.interactorObject is XRBaseInteractor)
            {
                GameObject parent = arg.interactorObject.transform.parent.gameObject;
                HandData handData = parent.GetComponentInChildren<HandData>();
                handData.animator.enabled = false;

                SetHandDataValues(handData, HandData);
                StartCoroutine(SetHandDataRoutine(handData, finalHandPosition, finalHandRotation, finalFingerRotations, startingHandPosition, startingHandRotation, startingFingerRotations));
            }
        }

        public void UnSetPose(BaseInteractionEventArgs arg)
        {
            if (arg.interactorObject is XRBaseInteractor)
            {
                GameObject parent = arg.interactorObject.transform.parent.gameObject;
                HandData handData = parent.GetComponentInChildren<HandData>();
                handData.animator.enabled = true;

                StartCoroutine(SetHandDataRoutine(handData, startingHandPosition, startingHandRotation, startingFingerRotations, finalHandPosition, finalHandRotation, finalFingerRotations));
            }
        }
        public void SetHandDataValues(HandData h1, HandData h2)
        {
            startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x,
                h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
            finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x,
                h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

            startingHandRotation = h1.root.localRotation;
            finalHandRotation = h2.root.localRotation;

            startingFingerRotations = new Quaternion[h1.fingerBones.Length];
            finalFingerRotations = new Quaternion[h1.fingerBones.Length];

            for (int i = 0; i < h1.fingerBones.Length; i++)
            {
                startingFingerRotations[i] = h1.fingerBones[i].localRotation;
                finalFingerRotations[i] = h2.fingerBones[i].localRotation;
            }
        }
        public void SendHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
        {
            h.root.localPosition = newPosition;
            h.root.localRotation = newRotation;

            for (int i = 0; i < newBonesRotation.Length; i++)
            {
                h.fingerBones[i].localRotation = newBonesRotation[i];
            }
        }

        public IEnumerator SetHandDataRoutine(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation, Vector3 startingPosition, Quaternion startingRotation, Quaternion[] startingBonesRotation)
        {
            float timer = 0f;
            while (timer < poseTransitionDuration)
            {
                Vector3 p = Vector3.Lerp(startingPosition, newPosition, timer / poseTransitionDuration);
                Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer / poseTransitionDuration);

                h.root.localPosition = p;
                h.root.localRotation = r;

                for (int i = 0; i < newBonesRotation.Length; i++)
                {
                    h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], timer / poseTransitionDuration);
                }

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}

