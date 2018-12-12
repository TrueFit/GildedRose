import * as React from "react";
import "./LoginBox.css";

type LoginProps = {
  handleLogin: () => void,
  handleQuit: () => void,
};

export const LoginBox = (props: LoginProps) => {
  const buttonStyle = {
    width: "80px",
  } as React.CSSProperties;
  return (
    <form className="pure-form">
      <fieldset>
        <div>
          <div className="LoginRow">
            <span>
              <legend>Gilded Rose Login</legend>
            </span>
          </div>
          <div className="LoginRow">
            <span>
              <input type="text" placeholder="name" />
            </span>
          </div>
          <div className="LoginRow">
            <span>
              <input type="password" placeholder="Password" />
            </span>
          </div>
          <div className="LoginRow">
            <span>
              <label htmlFor="remember">
                <input id="remember" type="checkbox" /> Remember me
              </label>
            </span>
          </div>
          <div className="LoginRow">
            <span>
              <button type="submit" style={buttonStyle}
                onClick={props.handleLogin}
                className="pure-button pure-button-primary">
                Sign in
              </button>
              <button type="submit" style={{ ...buttonStyle, marginLeft: "12px" }}
                onClick={props.handleQuit}
                className="pure-button pure-button-secondary">
                Cancel
              </button>
            </span>
          </div>
        </div>
      </fieldset>
    </form>
  );
};
