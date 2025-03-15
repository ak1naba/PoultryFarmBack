import { createRouter, createWebHistory } from 'vue-router'


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('@/views/MainView.vue')
    },
    {
      path: '/statistics',
      name: 'statistics',
      component: () => import('@/views/StatisticView.vue')
    },
    {
      path: '/cages',
      name: 'cages',
      component: () => import('@/views/CageView.vue')
    },
    {
      path: '/employee',
      name: 'employee',
      component: () => import('@/views/EmployeeView.vue')
    },
  ],
})

export default router
