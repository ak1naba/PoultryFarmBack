<script>
export default {
  name: "NavBar"
}
</script>

<template>
  <header>
    <div class="header">
      <div class="navbar-container">
        <div class="navbar-container__logo">
          <img src="../assets/icons/chicken-logo.png" alt="logo">
        </div>
        <div class="navbar-container__links">
          <router-link class="header-link" :to="{name: 'home'}">
            Главная
          </router-link>
          <router-link class="header-link" :to="{name: 'cages'}">
            Клетки
          </router-link>
          <router-link class="header-link" :to="{name: 'employee'}">
            Сотрудники
          </router-link>
          <router-link class="header-link" :to="{name: 'statistics'}">
            Статистический раздел
          </router-link>
        </div>
      </div>
    </div>

  </header>
</template>

<style scoped lang="scss">
header {
  padding: 20px 0px;
  background: linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)),
  url("../assets/images/field.jpg");
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;

}

.header {
  width: 100%;
  max-width: 1440px;
  margin: 0 auto;

  @media (max-width: 1440px) {
    max-width: 1280px;
  }

  @media (max-width: 1280px) {
    max-width: 720px;
  }

  @media (max-width: 720px) {
    max-width: 480px;
  }
}


.navbar-container {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;

  &__links{
    display: flex;
    flex-direction: row;
    gap: 25px;
    align-items: center;
  }

}

.header-link{
  text-decoration: none;
  color: white;
}
</style>