import * as React from "react";
import "styles/footer.css";

export namespace Footer {
  export interface Props {
    language?: string;
  }
}

export class Footer extends React.Component<Footer.Props> {
  // tslint:disable-next-line:no-any
  constructor(props: Footer.Props, context?: any) {
    super(props, context);
  }

  public render(): JSX.Element {
    const socialMediaIconContainerStyle = {
      textAlign: "right",
    } as React.CSSProperties;
    const socialMediaIconStyle = {
      paddingRight: "24px",
    } as React.CSSProperties;
    return (
      <>
        <div className="footer">
          <div className="pure-u-3">
            <div className="pure-u-2-3">
              {this.props.language}
            </div>
            <div className="pure-u-1-3" style={socialMediaIconContainerStyle}>
              <i className="fa fa-facebook" style={socialMediaIconStyle} aria-hidden="true" title="Visit our facebook page"></i>
              <i className="fa fa-twitter" style={socialMediaIconStyle} aria-hidden="true" title="Visit our twitter page"></i>
              <i className="fa fa-instagram" style={socialMediaIconStyle} aria-hidden="true" title="Visit our instagram page"></i>
            </div>
          </div>
        </div>
      </>
    );
  }
}
