using UnityEngine;
using UnityEngine.Android;
using Google.Play.Review;
using System.Collections;

public class InAppReviewManager : MonoBehaviour
{

#if UNITY_ANDROID
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
    private void Start()
    {
        Invoke(nameof(InitiateReview), 1);
    }
    private void InitiateReview()
    {
        Debug.Log("Initiate Review Function");
        _reviewManager = new ReviewManager();
        StartCoroutine(RequestReview());
    }

    private IEnumerator RequestReview()
    {
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Handle error case here
            yield break;
        }

        _playReviewInfo = requestFlowOperation.GetResult();

        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;

        _playReviewInfo = null;

        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Handle error case here
        }
    }
#endif
}
