import * as Cookie from "js-cookie";

export function logout(): void {
    Cookie.remove("Authorization");
    window.location.href = "http://localhost/logout";
}
