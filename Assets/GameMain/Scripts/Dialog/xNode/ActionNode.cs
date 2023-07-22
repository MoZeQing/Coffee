﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(400)]
public class ActionNode : Node {
    [SerializeField, Output(dynamicPortList = true)]
    public List<Trigger> click;
    [SerializeField, Output(dynamicPortList = true)]
    public List<Trigger> idle;
    [SerializeField, Output(dynamicPortList = true)]
    public List<Trigger> coffee;
	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}