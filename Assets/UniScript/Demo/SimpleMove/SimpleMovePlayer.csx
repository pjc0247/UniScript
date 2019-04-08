using System;
using UnityEngine;

class SimpleMoveBehaviour : UniFileScriptBehaviour {

    public CharacterController cc;

    public void OnBind() {
        cc = GetComponent<CharacterController>();

        Debug.Log(cc);
    }

    public void Update() {
        if (this.cc.isGrounded == false) {
            this.cc.SimpleMove(new Vector3(0, 0, 0));
            return;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
            this.cc.SimpleMove(new Vector3(-1, 0, 0) * Time.deltaTime * 60);
        if (Input.GetKey(KeyCode.RightArrow))
            this.cc.SimpleMove(new Vector3(1, 0, 0) * Time.deltaTime * 60);
        if (Input.GetKey(KeyCode.UpArrow))
            this.cc.SimpleMove(new Vector3(0, 0, 1) * Time.deltaTime * 60);
        if (Input.GetKey(KeyCode.DownArrow))
            this.cc.SimpleMove(new Vector3(0, 0, -1) * Time.deltaTime * 60);
    }
}