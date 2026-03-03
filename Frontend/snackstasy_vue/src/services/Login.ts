import axios from "axios";


// const BaseUrl = "https://cooking-recipe-web-production.up.railway.app/api";
const BaseUrl = "http://localhost:8080/api";

export async function Logout(): Promise<void> {
  const request = await axios.post(`${BaseUrl}/login/logout`, { withCredentials: true });
  if (request.data) {
    return request.data;
  } else {
    throw new Error("Error fetching logout");
  }
}
