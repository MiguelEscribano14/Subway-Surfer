using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] private float curveX;
    [SerializeField, Range(-1, 1)] private float curveY;
    [SerializeField] private Material[] materials;
    private GameManager mygameManager;
    private int dirCurve;
    private float tempCurve = 2;
    public int posCurve = 0;
    public int posObj;

    void Start()
    {
        mygameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        dirCurve = Random.Range(0, 3);

    }


    
    void Update()
    {
        LevelCurve();

        foreach (var m in materials)
        {
            m.SetFloat(Shader.PropertyToID("_Curve_X"), curveX);
            m.SetFloat(Shader.PropertyToID("_Curve_Y"), curveY);
        }
    }

    

    private void LevelCurve()
    {
        
        if (mygameManager.reset)
        {
            curveX = 0;
            curveY = 0;
        }
        if (mygameManager.reset == false)
        {
            
            if (tempCurve >= 0)
            {
                if (posCurve == 0)
                {
                    
                    if (dirCurve == 0)
                    {
                        tempCurve -= Time.deltaTime;
                        posObj = 0;
                    }
                    if (dirCurve == 1)
                    {
                        curveX += Time.deltaTime / 2;
                        curveY += Time.deltaTime / 2;
                        tempCurve -= Time.deltaTime;
                        posObj = 1;
                    }
                    if (dirCurve == 2)
                    {
                        curveX -= Time.deltaTime / 2;
                        curveY -= Time.deltaTime / 2;
                        tempCurve -= Time.deltaTime;
                        posObj = -1;
                    }
                    
                }
                
                if (posCurve == -1)
                {
                    if (dirCurve == 1)
                    {
                        
                        curveX += Time.deltaTime / 2;
                        curveY += Time.deltaTime / 2;
                        tempCurve -= Time.deltaTime;
                        posObj = 0;
                    }
                    else
                    {
                        
                        tempCurve -= Time.deltaTime;
                        posObj = -1;
                    }
                    
                }
                if (posCurve == 1)
                {
                    if (dirCurve == 2)
                    {
                        
                        curveX -= Time.deltaTime / 2;
                        curveY -= Time.deltaTime / 2;
                        tempCurve -= Time.deltaTime;
                        posObj = 0;
                    }
                    else
                    {
                        
                        tempCurve -= Time.deltaTime;
                        posObj = 1;
                    }
                    
                }
            }
            
            if (tempCurve <0)
            {
                if (posObj == 0)
                {
                    posCurve = 0;
                    curveX = 0;
                    curveY = 0;
                }
                if (posObj == 1)
                {
                    posCurve = 1;
                    curveX = 1;
                    curveY = 1;
                }
                if (posObj == -1)
                {
                    posCurve = -1;
                    curveX = -1;
                    curveY = -1;
                }

                if (tempCurve >= -3) { 
                
                tempCurve -= Time.deltaTime;
                }

                if (tempCurve < -3)
                {
                    tempCurve = 2;
                    dirCurve = Random.Range(0, 3);
                }
            }
        }
    }
}

