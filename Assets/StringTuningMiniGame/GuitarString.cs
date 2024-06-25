
using UnityEngine;
//GuitarString class defines behavior for guitarStrings, tuning flat and sharp, and logic behind rotation of tunerBackground
public class GuitarString : MonoBehaviour
{
    // startValue and currentValue are values of notes. 
    private float startValue = 0;
    private float currentValue = 0; 
    private bool Selected = false;
    private TuningStrings tuningStrings;
    public TunerNote tuner;
    private float timer = 0;
    private float timeForSuccessfulTuning = 1f;
    public string note; 
    
    

    // Start is called before the first frame update
    void Start()
    {
        while (startValue == 0) 
        {
            startValue = new System.Random().Next(-1000, 1000);
        }
        tuningStrings = GetComponentInParent<TuningStrings>();
        currentValue = startValue;

        
        transform.localRotation = Quaternion.Euler(0, 0, ValueToAngle());


    }

    // Update is called once per frame
    void Update() 
    {


        //If within specified bounds for x time, move on to next string. 
        if (Selected && currentValue < TuningStrings.targetValue + 50 && currentValue > TuningStrings.targetValue - 50)
        {

            if (timer >= timeForSuccessfulTuning)
            {
                tuner.UnDisplayNote(tuner.noteDisplays[tuningStrings.indexOfString]);
                tuningStrings.NextString();
                //Debug.Log("Timer: " + timer);
            }
            else
            {
                timer += Time.deltaTime;
                //Debug.Log("Timer: " + timer);
            }
        }
        else if (Selected)
        {
            timer = 0;
            //Debug.Log("Timer: " + timer);
        }



        float randomValue = new System.Random().Next(0, 10); 
        if (Selected && Input.GetKey(KeyCode.LeftArrow) && currentValue > -1000)
        {
            currentValue -= randomValue ;
            if (currentValue < -1000)
                currentValue = -1000;
            //Debug.Log("ValueToAngle: " + ValueToAngle() + " currentValue: " + currentValue + " tuner: "+ tuner.TunerBackground.transform.rotation.eulerAngles.z);
            tuner.TunerBackground.transform.eulerAngles = new Vector3(0, 0, ValueToAngle()); // rotates tunerBackground in respect to frequency

        }
        

        if (Selected && Input.GetKey(KeyCode.RightArrow) && currentValue < 1000)
        {
            currentValue += randomValue ;
            if (currentValue > 1000)
                currentValue = 1000;
            //Debug.Log("ValueToAngle: " + ValueToAngle() + " currentValue: " + currentValue + " tuner: " + tuner.TunerBackground.transform.rotation.eulerAngles.z);
            tuner.TunerBackground.transform.eulerAngles = new Vector3(0, 0, ValueToAngle());

        }

        
        //Checks to see which string is currently being tuned, will display on tuner. 
        if (Selected)
        {
            tuner.DisplayNote(tuner.noteDisplays[tuningStrings.indexOfString]);
            tuner.SetNote(note);
            tuner.SetValue(currentValue);
            transform.Rotate(0, 0, 50 * Time.deltaTime);
            
            if (currentValue > -100 && currentValue < 100)
                tuner.TunerBackground.GetComponent<SpriteRenderer>().color = new Color(153f/255f, 1, 153f/255f); // green
            else if (currentValue > -200 && currentValue < 200)
                tuner.TunerBackground.GetComponent<SpriteRenderer>().color = new Color(204f/255f, 1, 153f/255f);//light green
            else if (currentValue > -400 && currentValue < 400)
                tuner.TunerBackground.GetComponent<SpriteRenderer>().color = new Color(1, 1, 153f/255f);//yellow
            else if (currentValue > -600 && currentValue < 600)
                tuner.TunerBackground.GetComponent<SpriteRenderer>().color = new Color(1, 204f/255f, 153f/255f);//light red
            else
                tuner.TunerBackground.GetComponent<SpriteRenderer>().color = new Color(1, 153f / 255f, 153f/255f); //red

        }
      

    }
    //converts currentValue to Angle on the tunerBackground
    private float ValueToAngle()
    {
        float AbsoluteValue = currentValue + 1000;
        return 180 - (180 * (AbsoluteValue / 2000)); 
    }

    //Helper methods to choose which guitar string is currently being tuned

    public void Select()
    {
        Selected = true;
       
    }

    public void Unselect()
    {
        Selected = false;
         
    }

    

}
