using UnityEngine;

public class IconDragging : MonoBehaviour {

    [Header("Audio")]
    public AudioClip restartClip1;
    public AudioClip restartClip2;
    public AudioClip restartClip3;  
    public AudioClip restartClip;

    [Header("Setup")]
    public Vector3 iconHolderPosition;
    public CounterController referenceToCounterController;
    public GameObject pauseWindow; 

    private Vector3 tapPoint;
    private Vector3 ourObjectOffset;

    private void Update()
    {
        
        if (pauseWindow.activeSelf)
        {
            restartPosition();           
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (!pauseWindow.activeSelf)
        {
            restartPosition();           
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        
    }

    void OnMouseDown()
    {
        if (!pauseWindow.activeSelf)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;         
            tapPoint = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);           
            ourObjectOffset = this.gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, tapPoint.x));
        }
        else
        {
            preventHolding();
        }
    }

    void OnMouseDrag()
    {
        if (!pauseWindow.activeSelf)
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, tapPoint.x);            
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + ourObjectOffset;            
            this.transform.position = cursorPosition;
        }
        else
        {
            preventHolding();
        }
    }

    void OnMouseUp()
    {
        if (!pauseWindow.activeSelf)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            AudioSource.PlayClipAtPoint(restartClip, new Vector3(0, 0, -7));           
            Invoke("restartPosition", 0.1f);
        }
        else
        {
            preventHolding();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //3 different types of Holes, 3 different types of Icons to match

        if (collision.gameObject.tag == "Hole" && this.gameObject.tag == "Icon")
        {
            matchingIcon(collision);
        }
        else if (collision.gameObject.tag == "Hole1" && this.gameObject.tag == "Icon1")
        {
            matchingIcon(collision);
        }
        else if (collision.gameObject.tag == "Hole2" && this.gameObject.tag == "Icon2")
        {
            matchingIcon(collision);
        }
    }

    void matchingIcon(Collider2D collision)
    {
        Destroy(collision.gameObject);

        int audioDraw = Random.Range(0, 3);

        if (audioDraw == 0) AudioSource.PlayClipAtPoint(restartClip1, new Vector3(0, 0, -10));
        else if (audioDraw == 1) AudioSource.PlayClipAtPoint(restartClip2, new Vector3(0, 0, -10));
        else AudioSource.PlayClipAtPoint(restartClip3, new Vector3(0, 0, -10));

        referenceToCounterController.PointIncrement();
    }

    void preventHolding()
    {
        Instantiate(this, iconHolderPosition, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void restartPosition()
    {
        this.transform.position = iconHolderPosition;
    }

}
