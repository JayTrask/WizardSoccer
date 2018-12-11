using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TargetList_Test {

	[Test]
	public void SetTarget_Test()
	{
		TargetList targetList = new TargetList();
		Vector3 newTarget = new Vector3(1.0f, 2.0f, 3.0f);

		targetList.SetTarget(newTarget);

		List<Vector3> targets = new List<Vector3>();
		targets.Add(newTarget);

		Assert.That(targetList.targetPos, Is.EqualTo(targets));
	}


}
