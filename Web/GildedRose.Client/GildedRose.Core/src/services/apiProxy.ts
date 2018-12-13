import axios from "axios";
import { AxiosRequestConfig, AxiosError } from "axios";
import * as Cookie from "js-cookie";

axios.defaults.headers.common.Authorization = getHeader();
axios.defaults.headers["Cache-Control"] = "no-cache,no-store,must-revalidate,max-age=-1,private";
axios.defaults.headers.Pragma = "no-cache";
axios.defaults.headers.Expires = "0";

export async function get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
  url = `/api/${url}`;
  try {
    axios.defaults.headers.common.Authorization = getHeader();
    const response = await axios.get(url, config);
    return response.data;
  } catch (e) {
    if (e.response.status === 401) {
      window.alert("Your session has expired please log back in");
      navigateToLogout();
    }
    if (e.response.status >= 500) {
      navigateToError(e);
    }

    throw e as AxiosError;
  }
}

export async function post<T>(url: string, data: {}, config?: AxiosRequestConfig): Promise<T> {
  url = `/api/${url}`;
  try {
    axios.defaults.headers.common.Authorization = getHeader();
    const response = await axios.post(url, data, config);
    return response.data;
  } catch (e) {
    if (e.response.status === 401) {
      window.alert("Your session has expired please log back in");
      navigateToLogout();
    }
    if (e.response.status >= 500) {
      navigateToError(e);
    }

    throw e as AxiosError;
  }
}

export async function put<T>(url: string, data: {}, config?: AxiosRequestConfig): Promise<T> {
  url = `/api/${url}`;
  try {
    const response = await axios.put(url, data, config);
    return response.data;
  } catch (e) {
    if (e.response.status === 401) {
      window.alert("Your session has expired please log back in");
      navigateToLogout();
    }
    if (e.response.status >= 500) {
      navigateToError(e);
    }

    throw e as AxiosError;
  }
}

export async function patch<T>(url: string, data: {}, config?: AxiosRequestConfig): Promise<T> {
  url = `/api/${url}`;
  try {
    const response = await axios.patch(url, data, config);
    return response.data;
  } catch (e) {
    if (e.response.status === 401) {
      window.alert("Your session has expired please log back in");
      navigateToLogout();
    }
    if (e.response.status >= 500) {
      navigateToError(e);
    }

    throw e as AxiosError;
  }
}

export async function remove<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
  url = `/api/${url}`;
  try {
    const response = await axios.delete(url, config);
    return response.data;
  } catch (e) {
    if (e.response.status === 401) {
      window.alert("Your session has expired please log back in");
      navigateToLogout();
    }
    if (e.response.status >= 500) {
      navigateToError(e);
    }

    throw e as AxiosError;
  }
}

function getHeader(): string {
  const cookieValue = Cookie.get("Authorization");
  return `bearer ${cookieValue}`;
}

function navigateToLogout(): void {
  Cookie.remove("Authorization");
  window.location.assign("/assets/logout.html");
}

function navigateToError(e: Error): void {
  window.location.assign("/");
}
