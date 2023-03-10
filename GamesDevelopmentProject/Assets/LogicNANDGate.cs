using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicNANDGate : LogicNode
{
    public LogicNANDGate(LogicNode parentNode) : base(parentNode) { }

    public override void Interact() { }

    public override void Check()
    {
        bool isTrue = false;

        foreach (Toggle toggle in inputs)
        {
            if (!toggle.isOn)
                isTrue = true;
        }

        currentToggle.isOn = isTrue;
        //gameObject.GetComponent<Image>().enabled = isTrue;

        parentNode.Check();
    }
}
