import * as proxy from "core/services/apiProxy";
import { TokenResponse } from "models";

export async function getToken(username: string, password: string): Promise<TokenResponse> {
  const data = {
    username: username,
    password: password,
  };
  return await proxy.post<TokenResponse>("/Token/createtoken", data);
}
