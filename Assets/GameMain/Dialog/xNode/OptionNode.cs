﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Dialog;

[NodeWidth(400)]
public class OptionNode : Node {

	[Input] public float input;

    [SerializeField, Output(dynamicPortList = true)]
    public List<OptionData> optionDatas = new List<OptionData>();

    // Use this for initialization
    protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}
