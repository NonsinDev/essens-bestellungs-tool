
import type {  Login_response } from "@/model/AuthentificationInterface";
import { ref } from "vue";

import { checkSession } from '@/services/Login'


const user = ref<Login_response | null>(null);
const isLoading = ref(true);

export async function initAuth() {
  try {
    user.value = await checkSession();
  } catch {
    user.value = null;
  } finally {
    isLoading.value = false;
  }
}

export function useAuth() {
  return {
    user,
    isAuthenticated: () => user.value?.logged_in === true,
    isLoading,
  };
}