using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class Knife : MonoBehaviour
{
    [SerializeField]
    LayerMask planeLayerMask;
    [SerializeField]
    float knifeHeight;
    [SerializeField]
    float knifeLength;
    [SerializeField]
    Slicer slicer;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform knifePart;

    [SerializeField]
    float maxKnifeRotation;

    [SerializeField]
    LayerMask excludeWhilePushing;
    [SerializeField]
    LayerMask excludeWhileNotPushing;

    [SerializeField]
    float smoothMove;
    [SerializeField]
    float smoothRotate;

    [SerializeField]
    AudioSource chop;

    public bool hasGameBegun = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    bool isSlicing;
    bool pushing;
    Vector3 hitPoint;
    Vector3 cursorPosition;
    Vector3 targetPosition;
    bool snapRotation;

    Quaternion targetKnifePartLocalRotation;
    Quaternion targetRotation;

    
    // Update is called once per frame
    void Update()
    {
        if (!Menus.hasGameStarted)
            return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, planeLayerMask))
        {
            hitPoint = hit.point;
            
        }

        if (!isSlicing && !pushing)
        {
            targetPosition = hitPoint + Vector3.up * knifeHeight;
            targetRotation = Quaternion.identity;
        }


        if (isSlicing)
        {
            if (targetPosition != hitPoint)
                targetRotation = Quaternion.LookRotation(targetPosition - hitPoint, Vector3.up);
            

            float distance = Vector3.Distance(targetPosition, hitPoint);

            targetKnifePartLocalRotation = Quaternion.Euler(Mathf.Lerp(maxKnifeRotation, 0f, distance / knifeLength), 0f, 0f);
            if (distance == 0f)
            {
                knifePart.localRotation = targetKnifePartLocalRotation;
            }
            

            if (Input.GetMouseButtonUp(0) ||  distance > knifeLength)
            {
                if (distance > .02f){
                    chop.pitch = Random.Range(.9f, 1.1f);
                    
                    chop.Play();
                    slicer.SliceBelowKnife();
                }
                    

                isSlicing = false;
                Mouse.current.WarpCursorPosition(cursorPosition);
            }
        }
        else
        {
            targetKnifePartLocalRotation = Quaternion.identity;
        }

        snapRotation = isSlicing;

        knifePart.localRotation = Quaternion.Slerp(knifePart.localRotation, targetKnifePartLocalRotation, Time.deltaTime * 5f);

        rb.excludeLayers = pushing ? excludeWhilePushing : excludeWhileNotPushing;
        if (pushing)
        {
            targetPosition = hitPoint;
            targetRotation = Quaternion.identity;
        }


        if (Input.GetMouseButtonDown(1) && !isSlicing)
        {
            pushing = true;
        }
        if (Input.GetMouseButtonUp(1) && pushing)
        {
            pushing = false;
        }

        if (Input.GetMouseButtonDown(0) && !pushing)
        {
            isSlicing = true;
            targetPosition = hitPoint;
            cursorPosition = Input.mousePosition;
        }

        
    }


    private void FixedUpdate()
    {
        if (!Menus.hasGameStarted)
            return;

        try
        {

            Quaternion rotate = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * smoothRotate);

            if (snapRotation)
                rotate = targetRotation;

            rb.Move(Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothMove), rotate);

        }
        catch
        {
            Debug.Log("rb not set");
        }
       
    }


}
