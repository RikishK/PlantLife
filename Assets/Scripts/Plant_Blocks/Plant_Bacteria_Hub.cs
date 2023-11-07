using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Plant_Bacteria_Hub : Plant_Block
{
    public GameObject bacteriaObjA, bacteriaObjB, bacteriaObjC; // The object to be instantiated
    public Transform launchPoint; // The point from which objects are launched
    public float launchForce = 5.0f; // Force to launch the object
    public float launchInterval = 20.0f; // Time interval between launches
    public float stoppingDistance = 5.0f; // Distance at which the object stops
    public bool initiallyShootRight = true; // Initial shooting direction

    public Transform extensionPoint;

    [SerializeField] private SpriteRenderer hubRenderer;
    [SerializeField] private Sprite bacteriaA, bacteriaB, bacteriaC;
    private bool isShootingRight;
    private Coroutine shootingCoroutine;
    private State state = State.A;

    private enum State{
        A, B, C
    }

    private void OnMouseDown() {
        switch(state){
            case State.A:
                state = State.B;
                break;
            case State.B:
                state = State.C;
                break;
            case State.C:
                state = State.A;
                break;
        }
        RenderHub();
    }

    private void RenderHub(){
        switch(state){
            case State.A:
                hubRenderer.sprite = bacteriaA;
                break;
            case State.B:
                hubRenderer.sprite = bacteriaB;
                break;
            case State.C:
                hubRenderer.sprite = bacteriaC;
                break;
        }
    }

    private void Start()
    {
        // Determine the initial shooting direction
        isShootingRight = initiallyShootRight;

        // Start shooting objects periodically
        shootingCoroutine = StartCoroutine(ShootObjectsPeriodically());
        Init();
    }


    protected override void Highlight()
    {
        hubRenderer.color = hoverTint;
    }

    protected override void UnHighlight()
    {
        hubRenderer.color = originalColor;
    }

    private GameObject getBacteriaObject(){
        switch(state){
            case State.A:
                return bacteriaObjA;
            case State.B:
                return bacteriaObjB;
            case State.C:
                return bacteriaObjC;
        }
        return null;
    }

    private IEnumerator ShootObjectsPeriodically()
    {
        while (true)
        {
            // Instantiate the object and disable the Bacteria script
            GameObject newObj = Instantiate(getBacteriaObject(), launchPoint.position, Quaternion.identity);
            Bacteria bacteriaScript = newObj.GetComponent<Bacteria>();
            if (bacteriaScript != null)
            {
                bacteriaScript.enabled = false;
            }

            // Calculate the direction vector for downward movement
            Vector2 downwardDirection = Vector2.down;

            // Apply a speed for the downward movement (you can adjust this value)
            float downwardSpeed = 2.0f;

            // A set distance to stop the object (you can adjust this value)
            float stopDistance = 1.0f;

            StartCoroutine(MoveDownwardAndStop(newObj, downwardDirection, downwardSpeed, stopDistance, bacteriaScript));

            // Toggle the shooting direction
            isShootingRight = !isShootingRight;

            // Wait for the specified launch interval before shooting the next object
            yield return new WaitForSeconds(launchInterval);
        }
    }

    private IEnumerator MoveDownwardAndStop(GameObject obj, Vector2 direction, float speed, float stopDistance, Bacteria bacteriaScript)
    {
        while (obj != null && Vector2.Distance(obj.transform.position, transform.position) < stopDistance)
        {
            
            obj.transform.Translate(direction * speed * Time.deltaTime);
            yield return null;
        }

        // Re-enable the Molecule script when the object has reached the stopping distance
        if (bacteriaScript != null)
        {
            
            bacteriaScript.Setup();
            bacteriaScript.enabled = true;
        }
    }

    public override List<PlantData.UpgradeData> getUpgrades()
    {
        return new List<PlantData.UpgradeData>();
    }
}
