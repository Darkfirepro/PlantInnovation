using Microsoft.Azure.SpatialAnchors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Input;

public class AzureSpatialAnchor : MonoBehaviour
{
    public GameObject idText;

    /// <summary>
    /// The sphere prefab.
    /// </summary>
    public GameObject indicator;

    /// <summary>
    /// Set this string to the Spatial Anchors account id provided in the Spatial Anchors resource.
    /// </summary>
    protected string SpatialAnchorsAccountId = "05900743-ed1a-42f3-9404-894c208a5749";

    /// <summary>
    /// Set this string to the Spatial Anchors account key provided in the Spatial Anchors resource.
    /// </summary>
    protected string SpatialAnchorsAccountKey = "UHZLjzaKrPgXUrDl13lFZrWDmVKBffqhycsKXoBBww8=";

    /// <summary>
    /// Our queue of actions that will be executed on the main thread.
    /// </summary>
    private readonly Queue<Action> dispatchQueue = new Queue<Action>();

    protected CloudSpatialAnchorSession cloudSpatialAnchorSession;

    /// <summary>
    /// The CloudSpatialAnchor that we either 1) placed and are saving or 2) just located.
    /// </summary>
    protected CloudSpatialAnchor currentCloudAnchor;


    /// <summary>
    /// The ID of the CloudSpatialAnchor that was saved. Use it to find the CloudSpatialAnchor
    /// </summary>
    protected string cloudSpatialAnchorId = "";


    /// <summary>
    /// Indicate if we are ready to save an anchor. We can save an anchor when value is greater than 1.
    /// </summary>
    protected float recommendedForCreate = 0;

    // Start is called before the first frame update
    void Start()
    {

        InitializeSession();
    }

    // Update is called once per frame
    void Update()
    {
        lock (dispatchQueue)
        {
            if (dispatchQueue.Count > 0)
            {
                dispatchQueue.Dequeue()();
            }
        }
        idText.GetComponent<TextMeshProUGUI>().text = cloudSpatialAnchorId;
    }

    /// <summary>
    /// Queues the specified <see cref="Action"/> on update.
    /// </summary>
    /// <param name="updateAction">The update action.</param>
    protected void QueueOnUpdate(Action updateAction)
    {
        lock (dispatchQueue)
        {
            dispatchQueue.Enqueue(updateAction);
        }
    }


    /// <summary>
    /// Initializes a new CloudSpatialAnchorSession.
    /// </summary>
    void InitializeSession()
    {
        Debug.Log("ASA Info: Initializing a CloudSpatialAnchorSession.");

        if (string.IsNullOrEmpty(SpatialAnchorsAccountId))
        {
            Debug.LogError("No account id set.");
            return;
        }

        if (string.IsNullOrEmpty(SpatialAnchorsAccountKey))
        {
            Debug.LogError("No account key set.");
            return;
        }

        cloudSpatialAnchorSession = new CloudSpatialAnchorSession();

        cloudSpatialAnchorSession.Configuration.AccountId = SpatialAnchorsAccountId.Trim();
        cloudSpatialAnchorSession.Configuration.AccountKey = SpatialAnchorsAccountKey.Trim();

        cloudSpatialAnchorSession.LogLevel = SessionLogLevel.All;

        cloudSpatialAnchorSession.Error += CloudSpatialAnchorSession_Error;
        cloudSpatialAnchorSession.OnLogDebug += CloudSpatialAnchorSession_OnLogDebug;
        cloudSpatialAnchorSession.SessionUpdated += CloudSpatialAnchorSession_SessionUpdated;
        //cloudSpatialAnchorSession.AnchorLocated += CloudSpatialAnchorSession_AnchorLocated;
        //cloudSpatialAnchorSession.LocateAnchorsCompleted += CloudSpatialAnchorSession_LocateAnchorsCompleted;

        cloudSpatialAnchorSession.Start();

        Debug.Log("ASA Info: Session was initialized.");
    }

    private void CloudSpatialAnchorSession_Error(object sender, SessionErrorEventArgs args)
    {
        Debug.LogError("ASA Error: " + args.ErrorMessage);
    }

    private void CloudSpatialAnchorSession_OnLogDebug(object sender, OnLogDebugEventArgs args)
    {
        Debug.Log("ASA Log: " + args.Message);
        System.Diagnostics.Debug.WriteLine("ASA Log: " + args.Message);
    }

    private void CloudSpatialAnchorSession_SessionUpdated(object sender, SessionUpdatedEventArgs args)
    {
        Debug.Log("ASA Log: recommendedForCreate: " + args.Status.RecommendedForCreateProgress);
        recommendedForCreate = args.Status.RecommendedForCreateProgress;
    }

    private void CloudSpatialAnchorSession_AnchorLocated(object sender, AnchorLocatedEventArgs args)
    {
        switch (args.Status)
        {
            case LocateAnchorStatus.Located:
                Debug.Log("ASA Info: Anchor located! Identifier: " + args.Identifier);
                // Create a green sphere.
                QueueOnUpdate(() =>
                {
                    GameObject refObj = GameObject.Find("RefernceSceneBuilder");
                    indicator.AddComponent<WorldAnchor>();
                    Material indicatorMaterial = indicator.GetComponent<MeshRenderer>().material;
                    indicatorMaterial.color = Color.green;

                    // Get the WorldAnchor from the CloudSpatialAnchor and use it to position the sphere.
                    refObj.GetComponent<UnityEngine.XR.WSA.WorldAnchor>().SetNativeSpatialAnchorPtr(args.Anchor.LocalAnchor);

                    // Clean up state so that we can start over and create a new anchor.
                    //cloudSpatialAnchorId = "";
                });
                break;
            case LocateAnchorStatus.AlreadyTracked:
                Debug.Log("ASA Info: Anchor already tracked. Identifier: " + args.Identifier);
                break;
            case LocateAnchorStatus.NotLocated:
                Debug.Log("ASA Info: Anchor not located. Identifier: " + args.Identifier);
                break;
            case LocateAnchorStatus.NotLocatedAnchorDoesNotExist:
                Debug.LogError("ASA Error: Anchor not located does not exist. Identifier: " + args.Identifier);
                break;
        }
    }

    private void CloudSpatialAnchorSession_LocateAnchorsCompleted(object sender, LocateAnchorsCompletedEventArgs args)
    {
        Debug.Log("ASA Info: Locate anchors completed. Watcher identifier: " + args.Watcher.Identifier);
    }

    public void LocateAnchor()
    {

        // We have saved an anchor, so we will now look for it.
        if (!String.IsNullOrEmpty(cloudSpatialAnchorId))
        {
            // Create a Watcher to look for the anchor we created.
            AnchorLocateCriteria criteria = new AnchorLocateCriteria();
            criteria.Identifiers = new string[] { cloudSpatialAnchorId };
            cloudSpatialAnchorSession.CreateWatcher(criteria);

            Debug.Log("ASA Info: Watcher created. Number of active watchers: " + cloudSpatialAnchorSession.GetActiveWatchers().Count);

            cloudSpatialAnchorSession.AnchorLocated += CloudSpatialAnchorSession_AnchorLocated;
            cloudSpatialAnchorSession.LocateAnchorsCompleted += CloudSpatialAnchorSession_LocateAnchorsCompleted;
        }

        //Debug.Log("ASA Info: We will create a new anchor.");

        //CreateAndSaveSphere();
    }


    /// <summary>
    /// Creates a sphere at the hit point, and then saves a CloudSpatialAnchor there.
    /// </summary>
    /// <param name="hitPoint">The hit point.</param>
    public virtual void CreateAndSaveSphere()
    {
        GameObject refObj = GameObject.Find("RefernceSceneBuilder");
        refObj.AddComponent<WorldAnchor>();
        Material indicatorMat = indicator.GetComponent<MeshRenderer>().material;
        indicatorMat.color = Color.black;
        Debug.Log("ASA Info: Created a local anchor.");

        // Create the CloudSpatialAnchor.
        currentCloudAnchor = new CloudSpatialAnchor();

        // Set the LocalAnchor property of the CloudSpatialAnchor to the WorldAnchor component of our white sphere.

        WorldAnchor worldAnchor = refObj.GetComponent<WorldAnchor>();
        if (worldAnchor == null)
        {
            throw new Exception("ASA Error: Couldn't get the local anchor pointer.");
        }

        // Save the CloudSpatialAnchor to the cloud.
        currentCloudAnchor.LocalAnchor = worldAnchor.GetNativeSpatialAnchorPtr();
        Task.Run(async () =>
        {
            // Wait for enough data about the environment.
            while (recommendedForCreate < 1.0F)
            {
                await Task.Delay(330);
            }

            bool success = false;
            try
            {
                QueueOnUpdate(() =>
                {
                    // We are about to save the CloudSpatialAnchor to the Azure Spatial Anchors, turn it yellow.
                    indicatorMat.color = Color.yellow;
                });

                await cloudSpatialAnchorSession.CreateAnchorAsync(currentCloudAnchor);
                success = currentCloudAnchor != null;

                if (success)
                {

                    // Record the identifier to locate.
                    cloudSpatialAnchorId = currentCloudAnchor.Identifier;

                    QueueOnUpdate(() =>
                    {
                        // Turn the sphere blue.
                        indicatorMat.color = Color.green;
                    });

                    Debug.Log("ASA Info: Saved anchor to Azure Spatial Anchors! Identifier: " + cloudSpatialAnchorId);
                }
                else
                {
                    indicatorMat.color = Color.red;
                    Debug.LogError("ASA Error: Failed to save, but no exception was thrown.");
                }
            }
            catch (Exception ex)
            {
                QueueOnUpdate(() =>
                {
                    indicatorMat.color = Color.red;
                });
                Debug.LogError("ASA Error: " + ex.Message);
            }
        });
    }
}