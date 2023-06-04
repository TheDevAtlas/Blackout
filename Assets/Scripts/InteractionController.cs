using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionController : MonoBehaviour
{
    [Header("Inventory GUI Settings")]
    public Sprite selectedSprite;
    public Sprite defaultSprite;
    public TextMeshProUGUI selectedText;
    public float selectedSize = 1.2f;
    public float defaultSize = 1f;
    public int selected = 0;

    [Header("Inventory Objects")]
    public GameObject[] inventorySlots;
    public string[] inventoryNames;

    [Header("Objects Settings")]
    public int cameraNum = 3;
    public int generatorNum = 1;
    public float interactDistance = 5f;
    public GameObject cameraPrefab;
    public GameObject generatorPrefab;

    [Header("Interaction Text")]
    public TextMeshProUGUI cameraText;
    public TextMeshProUGUI generatorText;

    [Header("Camera Viewer")]
    public GameObject viewScreen;
    public bool isViewCam;
    public GameObject inventoryUI;
    public int cameraViewIndex = 0;

    public List<GameObject> placedCameras;
    public RenderTexture cameraTexture;
    public Texture noCameraTexture;
    public RawImage rawScreen;

    private void Start()
    {
        updateGUI();
    }

    private void Update()
    {
        cameraText.text = cameraNum.ToString();
        generatorText.text = generatorNum.ToString();
    }

    public void changeSlot(float dir)
    {
        if (!isViewCam)
        {
            selected += (int)dir;
            if (selected < 0)
            {
                selected = inventorySlots.Length - 1;
            }
            if (selected >= inventorySlots.Length)
            {
                selected = 0;
            }
        }
        else
        {
            // scroll through camera ui //
            cameraViewIndex += (int)dir;
            if(cameraViewIndex < 0)
            {
                cameraViewIndex = placedCameras.Count - 1;
            }
            if(cameraViewIndex >= placedCameras.Count)
            {
                cameraViewIndex = 0;
            }
        }
        

        updateGUI();
    }

    void updateGUI()
    {
        if (!isViewCam)
        {
            for (int x = 0; x < inventorySlots.Length; x++)
            {
                if (x == selected)
                {
                    inventorySlots[x].transform.localScale = new Vector3(selectedSize, selectedSize, selectedSize);
                    inventorySlots[x].GetComponent<Image>().sprite = selectedSprite;
                    selectedText.text = inventoryNames[x];
                }
                else
                {
                    inventorySlots[x].transform.localScale = new Vector3(defaultSize, defaultSize, defaultSize);
                    inventorySlots[x].GetComponent<Image>().sprite = defaultSprite;
                }
            }
        }
        else
        {
            // scroll through the cameras in an undefigned list of cameras //

            // TO DO MAKE ADJUSTABLE UI //

            // Camera Turn On / Off //
            for(int x = 0; x < placedCameras.Count; x++)
            {
                if(x == cameraViewIndex)
                {
                    placedCameras[x].GetComponent<PlacedCameraController>().cameraObj.SetActive(true);
                }
                else
                {
                    placedCameras[x].GetComponent<PlacedCameraController>().cameraObj.SetActive(false);
                }
            }
        }
    }

    public void tryInteract(Transform mainCamera, Transform player)
    {
        // Create a raycast //
        Ray ray = new Ray(mainCamera.position, mainCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            // Check if object is interactable OR if you want to place object //
            if (hit.collider.CompareTag("PlaceCamera"))
            {
                // Pick up camera //
                for(int x = 0; x < placedCameras.Count; x++)
                {
                    if(hit.collider.gameObject.GetComponent<PlacedCameraController>().cameraObj.GetComponent<Camera>() == placedCameras[x].GetComponent<PlacedCameraController>().cameraObj.GetComponent<Camera>())
                    {
                        placedCameras.RemoveAt(x);
                    }
                }
                Destroy(hit.collider.gameObject);
                cameraNum++;
            }
            else if (hit.collider.CompareTag("PlaceGenerator"))
            {
                // Pick up camera //
                Destroy(hit.collider.gameObject);
                generatorNum++;
            }
            else if (hit.collider.CompareTag("CameraViewer"))
            {
                isViewCam = true;
                viewScreen.SetActive(true);
                inventoryUI.SetActive(false);
                cameraViewIndex = 0;
                if (placedCameras.Count > 0)
                {
                    rawScreen.texture = cameraTexture;
                    placedCameras[0].GetComponent<PlacedCameraController>().cameraObj.SetActive(true);
                }
                else
                {
                    rawScreen.texture = noCameraTexture;
                }
            }
            else
            {
                if(selected == 1)
                {
                    cameraNum--;
                    if(cameraNum < 0)
                    {
                        cameraNum = 0;
                        return;
                    }
                    // Find Rotation For Camera Part //
                    Vector3 playerPosition = player.position;
                    Vector3 hitPoint = hit.point;
                    Quaternion rotation = Quaternion.LookRotation(hitPoint - playerPosition, hit.normal);

                    // Instantiate the prefab at the hit point with the calculated rotation
                    GameObject newCamera = Instantiate(cameraPrefab, hitPoint + new Vector3(0f, 0.5f, 0f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                    newCamera.transform.Find("CameraPivot").transform.rotation = rotation;
                    placedCameras.Add(newCamera);
                }
                else if(selected == 2)
                {
                    generatorNum--;
                    if (generatorNum < 0)
                    {
                        generatorNum = 0;
                        return;
                    }
                    // Attempt to place object //
                    Vector3 hitPoint = hit.point;

                    // Instantiate the prefab at the hit point with the calculated rotation //
                    GameObject newGenerator = Instantiate(generatorPrefab, hitPoint + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
                }
            }
        }
    }

    public void tryEscape()
    {
        if (isViewCam)
        {
            isViewCam = false;
            viewScreen.SetActive(false);
            inventoryUI.SetActive(true);
        }
    }
}
