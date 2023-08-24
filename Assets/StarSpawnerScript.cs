using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawnerScript : MonoBehaviour
{
    private float[] notes = { 0.4049398899078369f, 0.40492987632751465f, 0.5285072326660156f, 0.3827641010284424f, 0.40506982803344727f, 0.41637492179870605f, 0.38222813606262207f, 0.37120604515075684f, 0.4274628162384033f, 0.39357614517211914f, 0.3937530517578125f, 0.3716559410095215f, 0.3935658931732178f, 0.405062198638916f, 0.4051530361175537f, 0.3822619915008545f, 0.39385294914245605f, 0.3709838390350342f, 0.416550874710083f, 0.4049673080444336f, 0.4049856662750244f, 0.40504908561706543f, 0.3711860179901123f, 0.3938281536102295f, 0.3937489986419678f, 0.3824608325958252f, 0.4049839973449707f, 0.3713831901550293f, 0.3825211524963379f, 0.4158935546875f, 0.4053192138671875f, 0.40470099449157715f, 0.3937709331512451f, 0.17997217178344727f, 0.20258378982543945f, 0.20256519317626953f, 0.1909329891204834f, 0.1917438507080078f, 0.20206117630004883f, 0.16875171661376953f, 0.20281028747558594f, 0.21345877647399902f, 0.20268511772155762f, 0.21401691436767578f, 0.2022080421447754f, 0.20250487327575684f, 0.19135117530822754f, 0.19101405143737793f, 0.21389079093933105f, 0.17990803718566895f, 0.2026200294494629f, 0.20224213600158691f, 0.21343088150024414f, 0.20254206657409668f, 0.17908000946044922f, 0.19121408462524414f, 0.20230674743652344f, 0.20229601860046387f, 0.19050216674804688f, 0.20285391807556152f, 0.19033598899841309f, 0.40442800521850586f, 0.4158751964569092f, 0.16790509223937988f, 0.20228195190429688f, 0.19092774391174316f, 0.19097399711608887f, 0.19119715690612793f, 0.19092583656311035f, 0.1911449432373047f, 0.21332025527954102f, 0.21358895301818848f, 0.2244868278503418f, 0.18022823333740234f, 0.21313095092773438f, 0.1799790859222412f, 0.20197081565856934f, 0.1912069320678711f, 0.2138528823852539f, 0.19049525260925293f, 0.19083094596862793f, 0.19091200828552246f, 0.19109296798706055f };
    //private int[] stringz = {1,1,2,1,3,4,1,2,1,2,3,4, 1, 1, 2, 1, 3, 4, 1, 2, 1, 2, 3, 4 };
    private int[] stringz = { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 3, 4, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 3, 4, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 3, 4};
    private bool hasStarted = false;
    public GameObject pinkStar;
    public GameObject blackStar;
    public GameObject purpleStar;
    public GameObject redStar;
    public Vector3 pinkSpawnPosition;
    public Vector3 blackSpawnPosition;
    public Vector3 purpleSpawnPosition;
    public Vector3 redSpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(waiter());
                hasStarted = true;
            }
        }
        
    }

    IEnumerator waiter()
    {
        int i = 0;
        foreach (float t in notes)
        {
            // spawn note
            int r = stringz[i++];
            if (i == stringz.Length)
                i = 0;
            //Random.Range(1, 4);
            if (r == 1)
                Instantiate(pinkStar, pinkSpawnPosition, Quaternion.identity);
            else if (r == 2)
                Instantiate(blackStar, blackSpawnPosition, Quaternion.identity);
            else if (r == 3)
                Instantiate(purpleStar, purpleSpawnPosition, Quaternion.identity);
            else if (r == 4)
                Instantiate(redStar, redSpawnPosition, Quaternion.identity);
            yield return new WaitForSecondsRealtime(t);
        }
        foreach (float t in notes)
        {
            // spawn note
            int r = stringz[i++];
            if (i == stringz.Length)
                i = 0;
            //Random.Range(1, 4);
            if (r == 1)
                Instantiate(pinkStar, pinkSpawnPosition, Quaternion.identity);
            else if (r == 2)
                Instantiate(blackStar, blackSpawnPosition, Quaternion.identity);
            else if (r == 3)
                Instantiate(purpleStar, purpleSpawnPosition, Quaternion.identity);
            else if (r == 4)
                Instantiate(redStar, redSpawnPosition, Quaternion.identity);
            yield return new WaitForSecondsRealtime(t);
        }
    }

}
