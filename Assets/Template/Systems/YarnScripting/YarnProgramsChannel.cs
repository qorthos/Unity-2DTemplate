using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Yarn;

[CreateAssetMenu(fileName = "YarnProgramChannel", menuName = "Channels/YarnProgramsChannel")]
public class YarnProgramsChannel : ScriptableObject
{
    public List<YarnProgram> DialoguePrograms = new List<YarnProgram>();
    private Program program;

    public Program Program { 
        get 
        { 
            if (program == null)
            {
                var programs = new List<Program>();
                DialoguePrograms.ForEach(x => programs.Add(x.GetProgram()));
                program = Program.Combine(programs.ToArray());
            }

            return program; 
        } 
        private set { program = value; } 
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        Program = null;
    }
}
