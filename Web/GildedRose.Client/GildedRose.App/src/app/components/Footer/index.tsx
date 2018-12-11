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
    return (
      <>
        <div className="footer">
          {this.props.language}
          <i className="fa fa-minus-circle" />
        </div>
      </>
    );
  }
}
