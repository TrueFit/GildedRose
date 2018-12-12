import * as React from "react";
import "./LoginBox.css";

type LoginProps = {
  handleLogin: () => void,
  handleQuit: () => void,
};

export const LoginBox = (props: LoginProps) => {
  return (
    <div>
      <div id="userbox">
        <input type="name" placeholder="ID" />
        <input type="password" placeholder="Password" />
        <button id="login" onClick={props.handleLogin} >Login</button>
        <button id="quit" onClick={props.handleQuit} >Quit</button>
      </div>
    </div>
  );
};
