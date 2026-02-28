import type { CurrenUser_response } from "@/model/AuthentificationInterface";
import { ref } from "vue";


const user = ref<CurrenUser_response | null>(null);
const isLoading = ref(false);

export async function initAuth() {
  try {
      user.value = {
          id: 1,
          username: "max",
          role: "User"
    }
  } catch {
    user.value = null;
  } finally {
    isLoading.value = false;
  }
}

export function useAuth() {
  return {
    user,
    isAuthenticated: () => !!user.value,
    isLoading,
  };
}
