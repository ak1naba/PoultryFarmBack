<template>
    <div class="layout">
      <NavBar />
      <main>
        <slot></slot>
      </main>
      <footer>
        Footer
      </footer>
    </div>
  </template>
  
<script>
  import NavBar from "@/components/NavBar.vue";

  export default {
    name: 'BaseLayout',
    components: {NavBar}
  }
</script>
  
<style scoped>
main{
  padding: 20px 0px;

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

</style>
  