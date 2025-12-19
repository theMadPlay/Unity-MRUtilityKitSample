using System.Linq;
using TMPro;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using NUnit.Framework;
using System.Collections.Generic;

public class Calc_QR_Code_Distance : MonoBehaviour
{

    [SerializeField] private MRUK mruk_instance;
    private List<MRUKTrackable> scannedQRs = new List<MRUKTrackable>();

    public TextMeshPro distanceText;


    private void OnEnable()
    {
        //Find MRUK instance in the scene if not already assigned
        if (!mruk_instance)
        {
            mruk_instance = FindAnyObjectByType<MRUK>();
        }

        mruk_instance.SceneSettings.TrackableAdded.AddListener(OnTrackableAdded);
        mruk_instance.SceneSettings.TrackableRemoved.AddListener(OnTrackableRemoved);
    }

    void OnDisable()
    {
        if (mruk_instance)
        {
            mruk_instance.SceneSettings.TrackableAdded.RemoveListener(OnTrackableAdded);
            mruk_instance.SceneSettings.TrackableRemoved.RemoveListener(OnTrackableRemoved);
        }
    }

    private void OnTrackableAdded(MRUKTrackable trackable)
    {
        if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            return;
        Debug.Log($"QR added! UUID: {trackable.Anchor.Uuid} Payload: {trackable.MarkerPayloadString}");

        scannedQRs.Add(trackable);
    }

    private void OnTrackableRemoved(MRUKTrackable trackable)
    {
        if (trackable.TrackableType != OVRAnchor.TrackableType.QRCode)
            return;
        Debug.Log("QR removed!");
    }


    // Update is called once per frame
    void Update()
    {
        //are there exactly two QR Codes scanned?
        if (scannedQRs.Count == 2)
        {
            Debug.Log("Two QR Codes detected. Calculating distance...");
            //make sure both QR Codes are being tracked
            if (scannedQRs[0].IsTracked && scannedQRs[1].IsTracked)
            {
                //Calculate the distance between the two QR Codes
                float distance = Vector3.Distance(scannedQRs[0].transform.position, scannedQRs[1].transform.position);
                Debug.Log("Distance between QR Codes: " + distance + " meters");
                //Print the distance to a textmeshpro component attached to this GameObject
                distanceText.text = distance.ToString() + " m";
            }

            
        }
    }

}
