import * as React from "react";
import { Header } from "components/Header";
import { Footer } from "components/Footer";
// export namespace App {
//   export interface Props {
//   }
// }

export class Home extends React.Component<{}> {
  // tslint:disable-next-line:no-any
  constructor(props: {}, context?: any) {
    super(props, context);
  }

  public render(): JSX.Element {
    return (
      <>
        <Header title={"Giled Rose Inventory"} />
        <div>
          Body

        </div>
        <Footer language={"Hello"} />
      </>
    );
  }
}
