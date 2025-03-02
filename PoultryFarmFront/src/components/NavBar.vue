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

        </div>
      </div>
    </div>

  </header>
</template>

<style scoped>
header {
  padding: 20px 0px;
  background-image: url("../assets/images/field.jpg");
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;

  position: relative;

  &:before{
    content: '';
    display: block;
    width: 100%;
    height: 100%;
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color:rgba(0, 0, 0, .5);
  }

}

.header{
  @media (max-width: 1920px) {
    width: 1440px;
    margin: 0 auto;
  }
  @media (max-width: 1440px) {
    width: 1280px;
    margin: 0 auto;
  }
  @media (max-width: 1280px) {
    width: 720px;
    margin: 0 auto;
  }
  @media (max-width: 720px) {
    width: 480px;
    margin: 0 auto;
  }
}

.navbar-container {
  display: flex;
  flex-direction: row;


}
</style>