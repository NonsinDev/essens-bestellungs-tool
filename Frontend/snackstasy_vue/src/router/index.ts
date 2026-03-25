import { createRouter, createWebHistory, createWebHashHistory } from 'vue-router'
import { useAuth, initAuth } from "../services/Authentification";
import StaffHome from '../views/EmployeeHome.vue';
import Login from '@/views/Login.vue'
import FoodMenu from '../views/FoodMenu.vue';
import StaffLogin from '../views/EmployeeLogin.vue';

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    {
      path: '/',
      name: 'login',
      component: Login,
      meta: { showHeader: false }
    },
    {
      path: '/employee-home',
      name: 'employee-home',
      component: StaffHome,
      meta: { showHeader: false }
    },
    {
      path: '/menu',
      name: 'home',
      component: FoodMenu,
      meta: { requiresAuth: true,  
              showHeader: true
      },
    },
    {
      path: '/employee-login',
      name: 'employee-login',
      component: StaffLogin,
      meta: { showHeader: false }
    },

  ],
})


// 🔒 Globaler Login-Schutz
router.beforeEach(async (to, _from, next) => {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading.value) {
    await initAuth(); // 👈 Session im Backend prüfen
  }

  if (to.meta.requiresAuth && !isAuthenticated()) {
    return next("/login");
  } else {
    next();
  }
});

export default router
