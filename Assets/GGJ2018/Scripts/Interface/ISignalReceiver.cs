using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISignalReceiverHandler {
	
	void OnSignalReceiver(Signal signal);
}
