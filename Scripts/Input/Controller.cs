using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller {
    public static Controller controller = new Controller();

    public Controls Inputs;
    public Controller() {
        Inputs = new Controls();
        Inputs.Enable();
    }
}