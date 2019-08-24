using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float totalLoadTime;
    public int spawnCount;
    public string goalName;
    public GameObject whatIsHuippis;
    public GameObject whatIsTitle;
    public GameObject whatIsLoadingIndicator;

    private float startLoadTime;
    private bool loading;
    private bool playerClose;

    private TMPro.TextMeshPro titleMesh;
    private LoadingIndicator loadingIndicator;
    // Start is called before the first frame update
    void Start()
    {
        titleMesh = Instantiate(
            whatIsTitle,
            transform.position + Vector3.up * (transform.localScale.y + 2),
            Quaternion.identity)
            .GetComponent<TMPro.TextMeshPro>();
        titleMesh.text = goalName;

        loadingIndicator = Instantiate(
            whatIsLoadingIndicator,
            transform.position + Vector3.up * (transform.localScale.y  + 2) + Vector3.left * (transform.localScale.x + 1),
            Quaternion.identity)
            .GetComponent<LoadingIndicator>();
        ResetLoadingIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        if (IsLoading()) {
            loadingIndicator.transform.localScale = new Vector3(4 * GetCompletion(), 1, 0.05f);
            loadingIndicator.transform.position = transform.position +
                Vector3.up * (transform.localScale.y + 1) +
                Vector3.left * (transform.localScale.x + 1) +
                Vector3.right * (transform.localScale.x + 2 * GetCompletion());
        } else
        {
            ResetLoadingIndicator();
        }
    }

    private void ResetLoadingIndicator()
    {
        loadingIndicator.transform.localScale = new Vector3(0, 0, 0);
        loadingIndicator.transform.position = transform.position +
            Vector3.up * (transform.localScale.y + 1) +
            Vector3.left * (transform.localScale.x + 1);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            playerClose = true;
            if (GetCompletion() >= 1.0)
            {
                loading = false;
                startLoadTime = Time.time;

                SpawnHuippi();
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            playerClose = false;
    }

    public bool IsLoading()
    {
        return loading;
    }

    public float GetCompletion()
    {
        if (!loading)
        {
            return 0.0f;
        }

        return Mathf.Min(1, (Time.time - startLoadTime) / totalLoadTime);
    }

    private void SpawnHuippi()
    {
        foreach (int _ in Enumerable.Range(0, spawnCount))
        {
            Instantiate(whatIsHuippis, transform.position, transform.rotation);
        }
    }

    private void HandleInput()
    {
        if (playerClose && Input.GetButton("Jump"))
        {            
            if (!loading)
            {
                startLoadTime = Time.time;
            }
            loading = true;
        }
        else
        {
            loading = false;
        }
    }
}
