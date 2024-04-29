using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int totalDeTarefas;
    public int tarefasFeitas;
    public Image mask;
    public Image fill;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)tarefasFeitas / (float)totalDeTarefas;
        mask.fillAmount = fillAmount;

        fill.color = color;
    }

     public void FixedUpdate()
    {
        tarefasFeitas++;
        float fillAmount = (float)tarefasFeitas / (float)totalDeTarefas;
        mask.fillAmount = fillAmount;

        fill.color = color;
    }
}
