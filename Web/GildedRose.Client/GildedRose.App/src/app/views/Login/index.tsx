import * as React from "react";
import { Header } from "app/components/Header";
import { Footer } from "app/components/Footer";
import { Login } from "app/components/Login";

export class LoginView extends React.Component<{}> {
  // tslint:disable-next-line:member-ordering
  constructor(props: {}) {
    super(props);
  }

  public render(): JSX.Element {
    const loginStyle = {
      textAlign: "center",
    } as React.CSSProperties;

    return (
      <>
        <Header title={"Login Screen"} isAuthenticated={false} />
        <div>
          <div style={loginStyle}>
            <div>
              <Login />
            </div>
          </div>
        </div>
        <Footer language={"© Copyright 2018 GildedRose LLC"} />
      </>
    );
  }
}
