using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instruction;
    public int totalDeTarefas;
    public int tarefasFeitas;
    public Image mask;
    public Image fill;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(instruction);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     GetCurrentFill();
    // }

    // void GetCurrentFill()
    // {
    //     float fillAmount = tarefasFeitas / (float)totalDeTarefas;
    //     mask.fillAmount = fillAmount;

    //     fill.color = color;
    // }

    public void UpdateProgressBar(string instructionText)
    {
        tarefasFeitas++;
        float fillAmount = tarefasFeitas / (float)totalDeTarefas;
        mask.fillAmount = fillAmount;
        instruction.text = instructionText;

        fill.color = color;
    }
}
