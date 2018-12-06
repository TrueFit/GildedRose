import * as React from "react";
import { User } from "../../models/user";

type Props = {
    user?: User,
};

export const Header = (props: Props) => {
    // TODO: USE SETTINGS HERE FOR YEAR!
    return (
        <div style={{ backgroundColor: "#fff", borderBottom: "1px solid #ccc" }}>
            Header Foo
        </div>
    );
};
